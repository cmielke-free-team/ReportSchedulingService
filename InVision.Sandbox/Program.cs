using Emdat.InVision.Generator;
using Emdat.InVision.SSRSExecution;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace InVision.Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            //parse params from command line            
            var parameterValues =
                (from a in args
                 where a.StartsWith("/p:")
                 let parts = a.Substring(3).Split('=')
                 select new ParameterValue 
                 { 
                     Name = parts[0],
                     Value = parts[1]
                 })
                 .ToArray();

			bool hideDetail = parameterValues
				.Where(pv => pv.Name == "HideDetail")
				.Select(pv => (bool.TrueString.Equals(pv.Value ?? "false", StringComparison.OrdinalIgnoreCase)))
				.DefaultIfEmpty(false)
				.FirstOrDefault();

            //Load report options. In production this will come from the DB, but for this test app, load from Options.xml file
            XElement optionsElement = XElement.Load("Options.xml");
            string reportPath = (string)optionsElement.Element("ReportPath");
            XElement reportColumnsElement = optionsElement.Element("ReportColumns");

            //get rdl template from server
            var reportService = new Emdat.InVision.SSRS.ReportingService2005();
            reportService.UseDefaultCredentials = true;
            byte[] rdlTemplate = reportService.GetReportDefinition(reportPath);

            using (var rdlTemplateStream = new MemoryStream(rdlTemplate))
            {
                string format = "CSV";

                //load RDL into XElement
                XElement rdlTemplateElement = XElement.Load(rdlTemplateStream);

                //generate the RDL
                byte[] reportDefinition = ReportGenerator.GenerateReportDefinition(rdlTemplateElement, reportColumnsElement, hideDetail, format);

                //output the rdl file for debugging
                File.WriteAllBytes("reportDefinition.rdl", reportDefinition);

                //execute the report using the new RDL
                var reportExecution = new Emdat.InVision.SSRSExecution.ReportExecutionService();
                reportExecution.UseDefaultCredentials = true;

                Emdat.InVision.SSRSExecution.Warning[] warnings;
                string extension;
                string mimeType;
                string encoding;
                string[] streamIds;

                var executionInfo = reportExecution.LoadReportDefinition(reportDefinition, out warnings);
                reportExecution.ExecutionHeaderValue = new Emdat.InVision.SSRSExecution.ExecutionHeader()
                {
                    ExecutionID = executionInfo.ExecutionID
                };

                //set report parameter values                
                executionInfo = reportExecution.SetExecutionParameters(parameterValues, "en-us");

                byte[] pdf;
                string deviceInfo = "<DeviceInfo><MarginLeft>0.5in</MarginLeft></DeviceInfo>";
                //pdf = reportExecution.Render(format, deviceInfo, out extension, out mimeType, out encoding, out warnings, out streamIds);

                //render to PDF
                var requestBuilder = new StringBuilder();
                string xmlnsSoap = "http://schemas.xmlsoap.org/soap/envelope/";
                string xmlnsSSRS = "http://schemas.microsoft.com/sqlserver/2005/06/30/reporting/reportingservices";
                using (var xw = XmlWriter.Create(requestBuilder))
                {
                    xw.WriteStartElement("Envelope", xmlnsSoap);
                    xw.WriteStartElement("Header", xmlnsSoap);
                    xw.WriteStartElement("ExecutionHeader", xmlnsSSRS);
                    xw.WriteElementString("ExecutionID", xmlnsSSRS, executionInfo.ExecutionID);
                    xw.WriteEndElement();
                    xw.WriteEndElement();
                    xw.WriteStartElement("Body", xmlnsSoap);
                    xw.WriteStartElement("Render", xmlnsSSRS);
                    xw.WriteElementString("Format", xmlnsSSRS, format);
                    xw.WriteElementString("DeviceInfo", xmlnsSSRS, deviceInfo);
                    xw.WriteEndElement();
                    xw.WriteEndElement();
                    xw.WriteEndElement();
                }                                
                
                var requestBytes = Encoding.UTF8.GetBytes(requestBuilder.ToString());
                
                var renderRequest = (HttpWebRequest)WebRequest.Create(reportExecution.Url);
                renderRequest.Method = "POST";
                renderRequest.UseDefaultCredentials = true;
                renderRequest.Headers.Add("SOAPAction", "http://schemas.microsoft.com/sqlserver/2005/06/30/reporting/reportingservices/Render");
                renderRequest.ContentType = "text/xml; charset=utf-8";
                renderRequest.ContentLength = requestBytes.LongLength;
                using (var requestStream = renderRequest.GetRequestStream())
                {
                    requestStream.Write(requestBytes, 0, requestBytes.Length);
                }
                try
                {
                    using (var response = renderRequest.GetResponse())
                    {
                        //TODO: check error code and content type, etc.

                        using (var responseStream = response.GetResponseStream())
                        {
                            XNamespace soap = xmlnsSoap;
                            XNamespace ssrs = xmlnsSSRS;
                            XElement soapEnvelope = XElement.Load(responseStream);
                            var renderResponse = soapEnvelope.Elements(soap + "Body").Elements(ssrs + "RenderResponse").First();

                            string base64Result = (string)renderResponse.Element(ssrs + "Result");
                            pdf = Convert.FromBase64String(base64Result);

                            extension = (string)renderResponse.Element(ssrs + "Extension");
                            mimeType = (string)renderResponse.Element(ssrs + "MimeType");
                            warnings =
                                (from w in renderResponse.Elements(ssrs + "Warnings").Elements(ssrs + "Warning")
                                 select new Warning
                                 {
                                     Code = (string)w.Element("Code"),
                                     Severity = (string)w.Element("Severity"),
                                     Message = (string)w.Element("Message"),
                                     ObjectName = (string)w.Element("ObjectName"),
                                     ObjectType = (string)w.Element("ObjectType")
                                 })
                                 .ToArray();
                        }
                    }

                    //save to file and open
                    File.WriteAllBytes("output." + "csv", pdf);
                    System.Diagnostics.Process.Start("output." + "csv");
                }
                catch(WebException ex)
                {
                    if (ex.Response == null)
                    {
                        throw;
                    }
                    using(var responseStream = ex.Response.GetResponseStream())
                    using(var fs = File.Create("WebException.txt"))
                    {
                        responseStream.CopyTo(fs);
                    }
                    throw;
                }
            }
        }
    }    
}

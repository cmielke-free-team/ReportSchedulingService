using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace Emdat.InVision
{
    public static class ReportParametersHelper
    {
        private static XmlSerializer _parameterValueSerializer = new XmlSerializer(typeof(Emdat.InVision.SSRS.ParameterValue));


        public static List<Emdat.InVision.SSRS.ParameterValue> GetParameterValuesFromXml(string xml)
        {
            XElement root = XElement.Parse(xml);
            var prms =
                from e in root.Elements("ParameterValue")
                select new Emdat.InVision.SSRS.ParameterValue
                {
                    Name = (string)e.Element("Name"),
                    Value = (string)e.Element("Value"),
                    Label = (string)e.Element("Label")
                };
            return prms.ToList();
        }

        public static string GetParameterValuesXml(IEnumerable<Emdat.InVision.SSRS.ParameterValue> parameterValues)
        {
            if (parameterValues == null)
            {
                return null;
            }

            //serialize the parameters to an XML string
            StringBuilder sbPrms = new StringBuilder();
            XmlWriterSettings xws = new XmlWriterSettings();
            xws.ConformanceLevel = ConformanceLevel.Fragment;
            xws.Indent = false;
            xws.OmitXmlDeclaration = true;
            using (XmlWriter xw = XmlWriter.Create(sbPrms, xws))
            {
                xw.WriteStartElement("Parameters");
                foreach (Emdat.InVision.SSRS.ParameterValue prm in parameterValues)
                {
                    xw.WriteStartElement("ParameterValue");
                    xw.WriteElementString("Name", prm.Name);
                    if (prm.Value != null) { xw.WriteElementString("Value", prm.Value); }
                    if (prm.Label != null) { xw.WriteElementString("Label", prm.Value); }
                    xw.WriteEndElement();
                }
                xw.WriteEndElement();
                xw.Close();
            }
            return sbPrms.ToString();
        }

        [Obsolete]
        private static Emdat.InVision.SSRS.ParameterValue GetParameterValueFromXElement(XElement e)
        {
            Emdat.InVision.SSRS.ParameterValue pv = null;
            using (XmlReader xr = e.CreateReader())
            {
                pv = _parameterValueSerializer.Deserialize(xr) as Emdat.InVision.SSRS.ParameterValue;
                xr.Close();
            }
            return pv;
        }        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emdat.InVision;
using Emdat.Diagnostics;
using System.Diagnostics;
using System.Net;
using Emdat;
using System.IO;
using System.Xml.Linq;
using Emdat.InVision.Models;
using System.Xml;
using System.Xml.Serialization;
using Emdat.InVision.Generator;
using Emdat.InVision.Sql;
using Emdat.InVision.SSRSExecution;
using System.Globalization;

namespace InVision.SchedulingService
{
	public class ReportExecution
	{
		#region static members

		/// <summary>
		/// Gets or sets the maximum report failures
		/// </summary>
		/// <value>The max report failures</value>
		public static int MaxReportFailures
		{
			get
			{
				return _maxReportFailures;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("MaxReportFailures");
				}
				_maxReportFailures = value;
			}
		}
		private static int _maxReportFailures = 6;

		public static string ServerTimeZoneIdentifier
		{
			get
			{
				return InVision.SchedulingService.Properties.Settings.Default.ServerTimeZoneIdentifier;
			}
		}

		public static ReportExecution GetNextJob()
		{
			ReportExecution exec = null;

			#region logging

			Logger.TraceEvent(
				TraceEventType.Verbose,
				0,
				"{0}: BEGIN GetNextJob()",
				System.Threading.Thread.CurrentThread.ManagedThreadId);

			#endregion

			try
			{
				using (ReportingDataContext data = new ReportingDataContext())
				{
					//get the next execution from the queue
					var execRow =
						(from e in data.GetExecutionFromQueue(_maxReportFailures)
						 where e.ScheduledStartDate.HasValue
						 select e).FirstOrDefault();

					if (execRow == null)
					{
						return null;
					}

					try
					{
						var format = data.GetFormat(execRow.ReportFormatID).FirstOrDefault();
						var report = data.GetReport(execRow.ReportID, execRow.EnvironmentID).FirstOrDefault();
						var subscription = data.GetSubscription(execRow.ReportSubscriptionID).FirstOrDefault();
						exec = GetReportExecutionFromRow(execRow, format, report, subscription);
					}
					catch (Exception ex)
					{
						//WI #2889: mark the execution for retry on exception
						//We can't automatically fail on max tries here since data may be incomplete
						data.SetExecutionRetry(
								execRow.ReportExecutionID,
								execRow.Name,
								null,
								null,
								ex.ToString(),
								null,
								execRow.ErrorCount + 1,
								execRow.ScheduledStartDate,
								"SYSTEM",
								DateTime.Now);

						//re-throw exception
						throw;
					}
				}
			}

			#region logging

			finally
			{
				Logger.TraceEvent(
					TraceEventType.Verbose,
					0,
					"{0}: END GetNextJob(id={1}, state={2}, run={3}, nextRun={4}, subscription={5}, reportId={6}, path={7}, formatId={8}, format={9})",
					System.Threading.Thread.CurrentThread.ManagedThreadId,
					exec != null ? exec.Id : null,
					exec != null ? (ReportExecutionStateEnum?)exec.State : null,
					exec != null ? exec.ScheduledRunTime : null,
					exec != null ? exec.NextScheduledRunTime : null,
					exec != null ? exec.SubscriptionId : null,
					exec != null ? (int?)exec.ReportId : null,
					exec != null ? exec.ReportPath : null,
					exec != null ? (int?)exec.FormatId : null,
					exec != null ? exec.Format : null);
			}

			#endregion

			return exec;
		}

		public static ReportExecution GetReportExecutionFromRow(GetExecutionFromQueueRow execRow, GetFormatRow formatRow, GetReportRow reportRow, GetSubscriptionRow subscriptionRow)
		{
			return GetReportExecutionFromRow(execRow, formatRow, reportRow, subscriptionRow, null);
		}

		public static ReportExecution GetReportExecutionFromRow(GetExecutionFromQueueRow execRow, GetFormatRow formatRow, GetReportRow reportRow, GetSubscriptionRow subscriptionRow, string destTimeZone)
		{
			var curPrms = GetActualParameterValuesFromXml(execRow.Parameters, reportRow.ReportPath, execRow.ScheduledStartDate);
			var baseCoTimeZone = !string.IsNullOrEmpty(destTimeZone) ? destTimeZone : Report.GetBaseCompanyTimeZoneIdentifier(reportRow.ReportPath, curPrms, ServerTimeZoneIdentifier);

			Logger.TraceEvent(TraceEventType.Verbose, 0, "{0}: Base TranCo Time Zone={1}", System.Threading.Thread.CurrentThread.ManagedThreadId, baseCoTimeZone);

			var startDateInBaseCoTimeZone = TimeZoneInfoExtension.SafeConvertTimeBySystemTimeZoneId(execRow.ScheduledStartDate.Value, ServerTimeZoneIdentifier, baseCoTimeZone);
			var schedule = (subscriptionRow != null ?
			new Schedule(
				subscriptionRow.ScheduleFrequencyID,
				subscriptionRow.FrequencyInterval,
				subscriptionRow.FrequencyRecurrenceFactor,
				subscriptionRow.FrequencyRelativeInterval,
				(DateTime?)startDateInBaseCoTimeZone,
				subscriptionRow.StartTime,
				subscriptionRow.EndTime) :
			null);
			var nextRun = schedule.RecurrenceOption != ScheduleRecurrence.Once ? schedule.GetNextRunDate(startDateInBaseCoTimeZone) : null;
			var nextPrms = GetActualParameterValuesFromXml(subscriptionRow.Parameters, reportRow.ReportPath, nextRun);

			//build the execution
			var exec = new ReportExecution
			{
				Id = execRow.ReportExecutionID.ToString(),
				Name = subscriptionRow.Name,
				ReportPath = Report.GetSpecificReportPath(reportRow.ReportID.ToString(), reportRow.ReportPath, curPrms),
				FormatId = formatRow.ReportFormatID,
				Format = formatRow.SSRSFormat,
				FileType = formatRow.Extension,
				DeviceInfo = formatRow.SSRSDeviceInfo,
				Error = null,
				HistoryId = null,
				ParameterLanguage = "en-us",
				Parameters =
				   (from p in curPrms
					select new Emdat.InVision.SSRSExecution.ParameterValue
					{
						Name = p.Name,
						Value = p.Value
					}).ToList(),
				ScheduledRunTime = execRow.ScheduledStartDate,
				State = (ReportExecutionStateEnum)execRow.ReportExecutionStatusID,
				Data = null,
				SubscriptionId = execRow.ReportSubscriptionID.Value,
				ReportId = execRow.ReportID.Value,
				NextScheduledRunTime = nextRun.HasValue ?
				   (DateTime?)TimeZoneInfoExtension.SafeConvertTimeBySystemTimeZoneId(nextRun.Value, baseCoTimeZone, ServerTimeZoneIdentifier) :
				   null,
				NextScheduledRunParameters = nextPrms,
				ErrorCount = execRow.ErrorCount.HasValue ? execRow.ErrorCount.Value : 0,
				SubscriptionOptions = subscriptionRow.Options,
				ReportOptions = reportRow.Options
			};
			return exec;
		}

		public static void ResetRunningJobs()
		{
			using (ReportingDataContext data = new ReportingDataContext())
			{
				data.ResetExecutionQueue();
			}
		}

		private static void SetReportExecutionContent(ReportExecution execution)
		{
			int executionId = int.Parse(execution.Id);

			//Update content
			#region logging

			execution.Log(
			TraceEventType.Verbose,
			"Setting execution data (fileType={0}, dataLength={1})",
			execution.FileType,
			execution.Data != null ? execution.Data.Length : 0);

			#endregion

			using (ReportingDataContext content = new ReportingDataContext("Emdat.InVision.ReportContent"))
			{
				//save the report data                        
				var result = content.SetExecutionData(executionId, execution.FileType, execution.Data);
				if (result < 1)
				{
					throw new ApplicationException(string.Format("The execution data could not be set: {0}", executionId));
				}
			}
		}

		private static void SetReportExecutionStatus(ReportExecution execution)
		{
			int executionId = int.Parse(execution.Id);

			using (ReportingDataContext data = new ReportingDataContext())
			{
				string nextPrmsXml = ReportParametersHelper.GetParameterValuesXml(execution.NextScheduledRunParameters);

				#region logging

				execution.Log(
					TraceEventType.Verbose,
					"Setting execution status (name={0}, startTime={1:s}, endTime={2:s}, state={3}, errorCode={4}, errorCount={5}, usedHistory={6}, nextRun={7:s}, nextPrms={8}, report={9}, subscription={10}, format={11})",
					execution.Name,
					execution.StartTime,
					execution.EndTime,
					execution.State,
					execution.ErrorCode,
					execution.ErrorCount,
					!string.IsNullOrEmpty(execution.HistoryId),
					execution.NextScheduledRunTime,
					nextPrmsXml,
					execution.ReportId,
					execution.SubscriptionId,
					execution.FormatId);

				#endregion

				//update execution state
				data.SetExecutionStatus(
					reportExecutionID: executionId,
					name: execution.Name,
					startDate: execution.StartTime,
					endDate: execution.EndTime,
					reportExecutionStatusID: (int)execution.State,
					errorDescription: execution.ErrorDescription,
					errorCount: execution.ErrorCount,
					reportExecutionErrorID: execution.ErrorCode.HasValue ? (int?)execution.ErrorCode.Value : null,
					usedHistory: !string.IsNullOrEmpty(execution.HistoryId),
					nextExecutionDate: execution.NextScheduledRunTime,
					nextExecutionParameters: nextPrmsXml,
					nextExecutionReportID: execution.ReportId,
					nextExecutionSubscriptionID: execution.SubscriptionId,
					nextExecutionFormatID: execution.FormatId,
					modifiedUser: "SYSTEM",
					modifiedDate: DateTime.Now);
			}
		}

		private static void RetryReportExecution(ReportExecution execution)
		{
			int executionId = int.Parse(execution.Id);

			//Retry report up to max report failures before automatically failing
			if (execution.ErrorCount < _maxReportFailures)
			{
				#region logging

				execution.Log(
					TraceEventType.Verbose,
					"Setting execution retry (name={0}, startTime={1:s}, endTime={2:s}, state={3}, errorCode={4}, errorCount={5}, run={6})",
					execution.Name,
					execution.StartTime,
					execution.EndTime,
					execution.State,
					execution.ErrorCode,
					execution.ErrorCount,
					execution.ScheduledRunTime);

				#endregion

				using (ReportingDataContext data = new ReportingDataContext())
				{
					//retry
					data.SetExecutionRetry(
						executionId,
						execution.Name,
						execution.StartTime,
						execution.EndTime,
						execution.ErrorDescription,
						execution.ErrorCode.HasValue ? (int?)execution.ErrorCode.Value : null,
						execution.ErrorCount,
						execution.ScheduledRunTime,
						"SYSTEM",
						DateTime.Now);
				}
			}
			else
			{
				if (false == execution.StartTime.HasValue)
				{
					execution.StartTime = DateTime.Now;
				}

				if (false == execution.EndTime.HasValue)
				{
					execution.EndTime = DateTime.Now;
				}

				execution.ErrorCode = ReportExecutionErrorEnum.ExecutionTimeout;
				execution.State = ReportExecutionStateEnum.Failed;

				SetReportExecutionStatus(execution);				
			}
		}

		public static void CompleteExecution(ReportExecution execution)
		{
			#region logging

			execution.Log(TraceEventType.Verbose, "BEGIN CompleteExecution()");

			#endregion

			try
			{
				int executionId = int.Parse(execution.Id);
			
				switch (execution.State)
				{
					default:
					{
						throw new InvalidOperationException(string.Format("Execution {0} cannot be completed from its current state: {1}", executionId, execution.State));
					}
					case ReportExecutionStateEnum.Succeeded:
					{
						if (execution.Data == null)
						{
							throw new InvalidOperationException(string.Format("Execution {0} is in the 'Succeeded' state but it does not have any data", executionId));
						}

						try
						{
							SetReportExecutionContent(execution);							
						}
						catch (Exception ex)
						{
							//TFS #13667: retry if there are problems saving the report content
							execution.Log(TraceEventType.Warning, "Exception saving report content: {0}", ex.Message);
							execution.Log(TraceEventType.Information, "{0}", ex);
							execution.Error = ex;
							execution.ErrorCount++;
							RetryReportExecution(execution);
							return;
						}

						SetReportExecutionStatus(execution);						
						break;
					}
					case ReportExecutionStateEnum.Failed:
					{
						if (execution.ErrorCode.HasValue)
						{
							SetReportExecutionStatus(execution);							
						}
						else
						{
							RetryReportExecution(execution);							
						}
						break;
					}
				}
			}

			#region logging

			finally
			{
				execution.Log(TraceEventType.Verbose, "END CompleteExecution()");
			}

			#endregion
		}

		private static List<Emdat.InVision.SSRS.ParameterValue> GetActualParameterValuesFromXml(string xml, string reportPath, DateTime? sourceDate)
		{
			if (!sourceDate.HasValue)
			{
				return null;
			}

			using (Emdat.InVision.SSRS.ReportingService2005 ssrs = new Emdat.InVision.SSRS.ReportingService2005())
			{
				//HACK: To get around issues between DEV-DB02 and the DC, use a local user
				if (0 == string.Compare(System.Configuration.ConfigurationManager.AppSettings["ReportServerUseDefaultCredentials"], "FALSE", StringComparison.OrdinalIgnoreCase))
				{
					ssrs.UseDefaultCredentials = false;
					ssrs.Credentials = new NetworkCredential(
						System.Configuration.ConfigurationManager.AppSettings["ReportServerUserName"],
						System.Configuration.ConfigurationManager.AppSettings["ReportServerPassword"]);
				}
				else
				{
					ssrs.UseDefaultCredentials = true;
				}

				var prmVariables = ReportParametersHelper.GetParameterValuesFromXml(xml);
				var rptPrms = ssrs.GetReportParameters(reportPath, null, true, null, null);
				return
					(from r in rptPrms
					 where r.PromptUser //only care about non-internal params
					 join p in prmVariables on r.Name equals p.Name into rpGroup
					 from rp in rpGroup.DefaultIfEmpty(new Emdat.InVision.SSRS.ParameterValue
					 {
						 Name = r.Name,
						 Value = r.DefaultValues != null && r.DefaultValues.Length > 0 ? r.DefaultValues[0] : null
					 })
					 select new Emdat.InVision.SSRS.ParameterValue
					 {
						 Name = r.Name,
						 Value = GetParameterValueFromExpression(r, rp, sourceDate.Value)
					 })
					 .ToList();
			}
		}

		private static string GetParameterValueFromExpression(Emdat.InVision.SSRS.ReportParameter r, Emdat.InVision.SSRS.ParameterValue v, DateTime sourceDate)
		{
			if (v == null || string.IsNullOrEmpty(v.Value))
			{
				return null;
			}

			if (r.Type == Emdat.InVision.SSRS.ParameterTypeEnum.DateTime)
			{
				DateTime? dt = DateTimeExpression.Evaluate(v.Value, sourceDate);
				return dt.HasValue ? dt.Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture) : null;
			}
			else if (r.Type == Emdat.InVision.SSRS.ParameterTypeEnum.String &&
				r.Name.EndsWith("Time", StringComparison.InvariantCultureIgnoreCase))
			{
				if (string.IsNullOrEmpty(v.Value))
				{
					return v.Value;
				}

				DateTime dt;
				if (DateTime.TryParseExact(v.Value, Report.TimeFormats, null, DateTimeStyles.None, out dt))
				{
					return dt.ToString("HH:mm");
				}
				else
				{
					return v.Value;
				}
			}
			else
			{
				return v.Value;
			}
		}

		#endregion

		private bool _cancelExecution = false;
		private WebRequest _renderRequest;

		public event EventHandler<ReportExecutionEventArgs> RenderCompleted;
		public event EventHandler<ReportExecutionEventArgs> ExecuteCompleted;

		public void ExecuteAsync(ICredentials reportServerCredentials, TimeSpan timeout, string format)
		{
			#region logging

			Log(TraceEventType.Verbose, "BEGIN ExecuteAsync()");

			#endregion

			try
			{
				if (this.State != ReportExecutionStateEnum.Queued)
				{
					throw new InvalidOperationException(string.Format("Execution {0} cannot be run from its current state: {1}", this.Id, this.State));
				}

				try
				{
					_cancelExecution = false;
					this.State = ReportExecutionStateEnum.Running;
					this.StartTime = DateTime.Now;

					//create a reference to the web service
					using (Emdat.InVision.SSRSExecution.ReportExecutionService rs = new Emdat.InVision.SSRSExecution.ReportExecutionService())
					{
						//configure the web service
						if (reportServerCredentials != null)
						{
							rs.UseDefaultCredentials = false;
							rs.Credentials = reportServerCredentials;
						}
						else
						{
							rs.UseDefaultCredentials = true;
						}
						rs.Timeout = (int)timeout.TotalMilliseconds;

						string historyId = null;

						#region logging

						Log(TraceEventType.Information, "LoadReport(reportPath='{0}', historyId='{1}')", this.ReportPath, historyId);

						#endregion

						//load the report     
						Emdat.InVision.SSRSExecution.ExecutionInfo execInfo = null;

						XElement customColumns = GetCustomColumns();
						if (null != customColumns && null != customColumns.Element("Columns") && 0 < customColumns.Element("Columns").DescendantNodes().Count())
						{
							var hideDetail = false;
							//var reportFormat = "xml";

							foreach (var parameter in this.Parameters)
							{
								if ("HideDetail" == parameter.Name)
								{
									Boolean.TryParse(parameter.Value, out hideDetail);
									break;
								}
							}

							using (var reportingService = new Emdat.InVision.SSRS.ReportingService2005())
							{
								reportingService.UseDefaultCredentials = rs.UseDefaultCredentials;
								reportingService.Credentials = rs.Credentials;

								byte[] rdlTemplate = reportingService.GetReportDefinition(this.ReportPath);

								using (var rdlTemplateStream = new MemoryStream(rdlTemplate))
								using (var rdlTemplateReader = XmlReader.Create(rdlTemplateStream))
								{
									XElement rdlTemplateElement = XElement.Load(rdlTemplateReader);
									byte[] generatedRdl = ReportGenerator.GenerateReportDefinition(rdlTemplateElement, customColumns, hideDetail, format);

									Emdat.InVision.SSRSExecution.Warning[] warnings = null;
									execInfo = rs.LoadReportDefinition(generatedRdl, out warnings);
								}
							}
						}
						else
						{
							execInfo = rs.LoadReport(this.ReportPath, historyId);
						}

						rs.ExecutionHeaderValue = new Emdat.InVision.SSRSExecution.ExecutionHeader()
						{
							ExecutionID = execInfo.ExecutionID
						};

						#region logging

						var prmStr = from p in this.Parameters select string.Format("{0}:{1}", p.Name, p.Value);
						Log(TraceEventType.Information, "SetExecutionParameters(prms='{0}', prmLanguage='{1}')", string.Join(";", prmStr.ToArray()), this.ParameterLanguage);

						#endregion

						try
						{
							//set the parameters
							execInfo = rs.SetExecutionParameters(this.Parameters.ToArray(), this.ParameterLanguage);
						}
						catch (Exception ex)
						{
							Log(TraceEventType.Information, "{0}", ex);
							throw new ReportExecutionException(ReportExecutionErrorEnum.InvalidReportParameter, ex.Message);
						}
						//validate parameters (just make sure they all have valid values)
						foreach (var p in execInfo.Parameters)
						{
							if (p.State != Emdat.InVision.SSRSExecution.ParameterStateEnum.HasValidValue)
							{
								throw new ReportExecutionException(
									ReportExecutionErrorEnum.InvalidReportParameter,
									string.Format(string.Format("Invalid parameter value specified for {0}: {1}. {2}",
										p.Name,
										string.Join(";", (from pv in this.Parameters where pv.Name == p.Name select pv.Value).ToArray()),
										p.ErrorMessage)));
							}
						}

						//We are using URL access to the report server so that we 
						//can support streaming and have a little more control over 
						//the whole rendering process.
						//string deviceInfoQueryString = GetDeviceInfoQueryString();
						//string url = string.Format(
						//    @"{0}?{1}&rs:SessionId={2}&rs:Format={3}{4}{5}",
						//    rs.Url.Replace("ReportExecution2005.asmx", string.Empty),
						//    execInfo.ReportPath,
						//    execInfo.ExecutionID,
						//    this.Format,
						//    !string.IsNullOrEmpty(deviceInfoQueryString) ? "&" : string.Empty,
						//    deviceInfoQueryString);

						//render to PDF
						var requestBuilder = new StringBuilder();
						string xmlnsSoap = "http://schemas.xmlsoap.org/soap/envelope/";
						string xmlnsSSRS = "http://schemas.microsoft.com/sqlserver/2005/06/30/reporting/reportingservices";
						using (var xw = XmlWriter.Create(requestBuilder))
						{
							xw.WriteStartElement("Envelope", xmlnsSoap);
							xw.WriteStartElement("Header", xmlnsSoap);
							xw.WriteStartElement("ExecutionHeader", xmlnsSSRS);
							xw.WriteElementString("ExecutionID", xmlnsSSRS, execInfo.ExecutionID);
							xw.WriteEndElement();
							xw.WriteEndElement();
							xw.WriteStartElement("Body", xmlnsSoap);
							xw.WriteStartElement("Render", xmlnsSSRS);
							xw.WriteElementString("Format", xmlnsSSRS, this.Format);
							xw.WriteElementString("DeviceInfo", xmlnsSSRS, this.DeviceInfo);
							xw.WriteEndElement();
							xw.WriteEndElement();
							xw.WriteEndElement();
						}

						var requestBytes = Encoding.UTF8.GetBytes(requestBuilder.ToString());

						_renderRequest = WebRequest.Create(rs.Url);
						_renderRequest.UseDefaultCredentials = rs.UseDefaultCredentials;
						_renderRequest.Credentials = rs.Credentials;
						_renderRequest.Timeout = rs.Timeout;
						_renderRequest.Method = "POST";
						_renderRequest.Headers.Add("SOAPAction", "http://schemas.microsoft.com/sqlserver/2005/06/30/reporting/reportingservices/Render");
						_renderRequest.ContentType = "text/xml; charset=utf-8";
						_renderRequest.ContentLength = requestBytes.LongLength;
						using (var requestStream = _renderRequest.GetRequestStream())
						{
							requestStream.Write(requestBytes, 0, requestBytes.Length);
						}

						#region logging

						Log(TraceEventType.Verbose, "BEGIN GetResponse(url={0})", rs.Url);

						#endregion

						_renderRequest.BeginGetResponse(
							delegate (IAsyncResult ar)
							{
								try
								{
									try
									{
										#region logging

										Log(TraceEventType.Verbose, "END GetResponse(url={0})", rs.Url);

										#endregion

										if (_cancelExecution)
										{
											return;
										}

										//end the asynchronous call
										using (WebResponse response = _renderRequest.EndGetResponse(ar))
										{
											//raise RenderCompleted event
											OnRenderCompleted();

											try
											{
												#region logging

												Log(TraceEventType.Verbose, "END GetResponse(url={0})", rs.Url);

												#endregion

												//download data to a temporary file
												string tempFile = this.GetTempFilePath();
												try
												{
													Directory.CreateDirectory(Path.GetDirectoryName(tempFile));
													using (Stream readStream = response.GetResponseStream())
													using (XmlReader xmlReader = XmlReader.Create(readStream))
													{
														XNamespace soap = "http://schemas.xmlsoap.org/soap/envelope/";
														XNamespace ssrs = "http://schemas.microsoft.com/sqlserver/2005/06/30/reporting/reportingservices";

														XElement soapEnvelope = XElement.Load(xmlReader);
														var renderResponse = soapEnvelope.Elements(soap + "Body").Elements(ssrs + "RenderResponse").First();

														string base64Result = (string)renderResponse.Element(ssrs + "Result");
														byte[] pdf;
														pdf = Convert.FromBase64String(base64Result);

														String extension = (string)renderResponse.Element(ssrs + "Extension");
														String mimeType = (string)renderResponse.Element(ssrs + "MimeType");
														Warning[] warnings =
															(from w in renderResponse.Elements(ssrs + "Warnings").Elements(ssrs + "Warning")
															 select new Warning
															 {
																 Code = (string)w.Element("Code"),
																 Severity = (string)w.Element("Severity"),
																 Message = (string)w.Element("Message"),
																 ObjectName = (string)w.Element("ObjectName"),
																 ObjectType = (string)w.Element("ObjectType")
															 }).ToArray();
														this.Data = pdf;
													}
												}
												finally
												{
													#region clean-up temporary file

													try
													{
														//clean-up temporary file
														if (File.Exists(tempFile))
														{
															File.Delete(tempFile);
														}
													}
													catch (Exception ex)
													{
														Log(TraceEventType.Warning, "Error deleting temporary file {0} --> {1}: {2}", tempFile, ex.GetType(), ex.Message);
														Log(TraceEventType.Information, ex.ToString().Replace(Environment.NewLine, " "));
													}

													#endregion
												}
											}
											catch (Exception ex)
											{
												//on other exceptions, reset the report execution, and let the caller handle retry/recovery                    
												this.HistoryId = null;
												this.Data = null;
												this.Error = ex;
												this.ErrorCount++;
												this.EndTime = DateTime.Now;
												this.State = ReportExecutionStateEnum.Failed;

												Log(TraceEventType.Information, "{0}: {1}", ex.GetType(), ex.Message);
												Log(TraceEventType.Information, "{0}", ex.ToString().Replace(Environment.NewLine, " "));
											}
										}

										//mark as successful
										this.HistoryId = historyId;
										this.Error = null;
										this.EndTime = DateTime.Now;
										this.State = ReportExecutionStateEnum.Succeeded;
									}
									catch (WebException ex)
									{
										this.HistoryId = null;
										this.Data = null;
										this.Error = ex;
										this.ErrorCount++;
										this.EndTime = DateTime.Now;
										this.State = ReportExecutionStateEnum.Failed;

										Log(TraceEventType.Warning, "{0}: {1} --> An WebException was encountered when rendering {2}. See service logs for more details.", ex.GetType(), ex.Message, this.ReportPath);
										Log(TraceEventType.Information, ex.ToString().Replace(Environment.NewLine, " "));

										//on WebException, we need to check the web response for more details
										HandleWebException(ex);
										OnRenderCompleted();
									}
									catch (Exception ex)
									{
										//on other exceptions, reset the report execution, and let the caller handle retry/recovery                    
										this.HistoryId = null;
										this.Data = null;
										this.Error = ex;
										this.ErrorCount++;
										this.EndTime = DateTime.Now;
										this.State = ReportExecutionStateEnum.Failed;

										Log(TraceEventType.Information, "{0}: {1}", ex.GetType(), ex.Message);
										Log(TraceEventType.Information, ex.ToString().Replace(Environment.NewLine, " "));

										OnRenderCompleted();
									}
								}
								finally
								{
									//raise completed event
									if (!_cancelExecution)
									{
										OnExecuteCompleted();
									}
								}
							},
							null);

						#region logging

						Log(TraceEventType.Information, "END BeginGetResponse()", rs.Url);

						#endregion
					}
				}

				#region exception handling

				catch (ReportExecutionException ex)
				{
					//on custom ReportExecutionException, fail the report with an ErrorCode
					this.HistoryId = null;
					this.Data = null;
					this.ErrorCode = ex.ErrorCode;
					this.Error = ex;
					this.ErrorCount++;
					this.EndTime = DateTime.Now;
					this.State = ReportExecutionStateEnum.Failed;

					//raise RenderCompleted event
					OnRenderCompleted();
					OnExecuteCompleted();

					Log(TraceEventType.Information, "{0}: {1}", ex.GetType(), ex.Message);
					Log(TraceEventType.Information, ex.ToString().Replace(Environment.NewLine, " "));
				}
				catch (Exception ex)
				{
					//on other exceptions, reset the report execution, and let the caller handle retry/recovery                    
					this.HistoryId = null;
					this.Data = null;
					this.Error = ex;
					this.ErrorCount++;
					this.EndTime = DateTime.Now;
					this.State = ReportExecutionStateEnum.Failed;

					//raise RenderCompleted event
					OnRenderCompleted();
					OnExecuteCompleted();

					Log(TraceEventType.Information, "{0}: {1}", ex.GetType(), ex.Message);
					Log(TraceEventType.Information, ex.ToString().Replace(Environment.NewLine, " "));
				}

				#endregion
			}

			#region logging

			finally
			{
				Log(TraceEventType.Verbose, "END ExecuteAsync()");
			}

			#endregion
		}

		private void HandleWebException(WebException ex)
		{
			if (ex.Response != null)
			{
				using (Stream responseStream = ex.Response.GetResponseStream())
				{
					byte[] readBuffer = new byte[ex.Response.ContentLength];
					responseStream.Read(readBuffer, 0, readBuffer.Length);
					string responseString = Encoding.Default.GetString(readBuffer);
					Log(TraceEventType.Information, "Error rendering {0} --> {1}", this.ReportPath, responseString);
					if (ex.Response.ContentType.StartsWith("text/xml", StringComparison.InvariantCultureIgnoreCase))
					{
						XNamespace soap = "http://schemas.xmlsoap.org/soap/envelope/";
						XNamespace ssrs = "http://schemas.microsoft.com/sqlserver/2005/06/30/reporting/reportingservices";

						XElement soapEnvelope = XElement.Parse(responseString);
						var soapFault = soapEnvelope
							.Elements(soap + "Body")
							.Elements(soap + "Fault")
							.FirstOrDefault();
						if (soapFault == null)
						{
							Log(TraceEventType.Information, "No soap:Fault element was found.");
						}
						else
						{
							string faultString = (string)soapFault.Element("faultstring") ?? string.Empty;
							if (faultString.StartsWith("Excel Rendering Extension: Number of rows exceeds the maximum possible in a worksheet of this version.", StringComparison.InvariantCultureIgnoreCase))
							{
								this.ErrorCode = ReportExecutionErrorEnum.TooManyRowsForExcel;
							}
						}
					}
				}
			}
		}

		private XElement GetCustomColumns()
		{
			if (string.IsNullOrEmpty(this.SubscriptionOptions))
			{
				return null;
			}

			if (string.IsNullOrEmpty(this.ReportOptions))
			{
				return null;
			}

			XElement reportOptionsElement = XElement.Parse(this.ReportOptions);
			bool useCustomColumns = reportOptionsElement.Elements("UseCustomColumns")
				.Select(e => (bool)e)
				.DefaultIfEmpty(true)
				.First();

			if (false == useCustomColumns)
			{
				return null;
			}

			XElement subscriptionOptionsElement = XElement.Parse(this.SubscriptionOptions);
			XElement reportColumnsElement = subscriptionOptionsElement.Element("ReportColumns");
			var availCols = reportOptionsElement.Elements("ReportColumns").Elements("AvailableColumns").FirstOrDefault();
			if (availCols == null)
			{
				return null;
			}
			reportColumnsElement.Add(availCols);
			return reportColumnsElement;
		}

		protected void OnExecuteCompleted()
		{
			#region logging

			Log(TraceEventType.Verbose, "BEGIN OnExecuteCompleted()");

			#endregion

			try
			{

				if (this.ExecuteCompleted != null)
				{
					this.ExecuteCompleted(this, new ReportExecutionEventArgs(this));
				}

			}

			#region logging

			finally
			{
				Log(TraceEventType.Verbose, "END OnExecuteCompleted()");
			}

			#endregion

		}

		protected void OnRenderCompleted()
		{
			#region logging

			Log(TraceEventType.Verbose, "BEGIN OnRenderCompleted()");

			#endregion

			try
			{
				if (this.RenderCompleted != null)
				{
					this.RenderCompleted(this, new ReportExecutionEventArgs(this));
				}
			}

			#region logging

			finally
			{
				Log(TraceEventType.Verbose, "END OnRenderCompleted()");
			}

			#endregion
		}

		private string GetTempFilePath()
		{
			string tempPath = System.Configuration.ConfigurationManager.AppSettings["TempPath"];
			if (string.IsNullOrEmpty(tempPath))
			{
				tempPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Temp");
			}
			return Path.Combine(tempPath, string.Format("{0}.{1}", this.Id, this.FileType));
		}

		private string GetDeviceInfoQueryString()
		{
			if (string.IsNullOrEmpty(this.DeviceInfo))
			{
				return string.Empty;
			}
			var queryPrms =
				from e in XElement.Parse(this.DeviceInfo).Elements()
				select string.Format("rc:{0}={1}", System.Web.HttpUtility.UrlEncode(e.Name.LocalName), System.Web.HttpUtility.UrlEncode(e.Value));
			return string.Join("&", queryPrms.ToArray());
		}

		public void CancelExecute()
		{
			if (this.State != ReportExecutionStateEnum.Running)
			{
				return;
			}

			_cancelExecution = true;
			if (_renderRequest != null)
			{
				_renderRequest.Abort();
			}

			//TODO: cancel the execution
		}

		public string Id { get; set; }

		public string Name { get; set; }

		public byte[] Data { get; set; }

		public string ReportPath { get; set; }

		public int FormatId { get; set; }

		public string Format { get; set; }

		public string FileType { get; set; }

		public string DeviceInfo { get; set; }

		public List<Emdat.InVision.SSRSExecution.ParameterValue> Parameters { get; set; }

		public string ParameterLanguage
		{
			get
			{
				return "en-us";
			}
			set
			{
				//TODO: allow for different parameter languages
			}
		}

		public string HistoryId { get; set; }

		public ReportExecutionStateEnum State { get; set; }

		public int ErrorCount { get; set; }

		public ReportExecutionErrorEnum? ErrorCode { get; set; }

		public Exception Error { get; set; }

		public string ErrorDescription
		{
			get
			{
				string str = this.Error != null ? this.Error.ToString() : null;
				return str != null ?
					str.Substring(0, Math.Min(1000, str.Length)) :
					null;
			}
		}

		public DateTime? ScheduledRunTime { get; set; }

		public DateTime? StartTime { get; set; }

		public DateTime? EndTime { get; set; }

		public int ReportId { get; set; }

		public int? SubscriptionId { get; set; }

		public DateTime? NextScheduledRunTime { get; set; }

		public List<Emdat.InVision.SSRS.ParameterValue> NextScheduledRunParameters { get; set; }

		public String SubscriptionOptions { get; set; }

		public string ReportOptions { get; set; }

		#region helper methods

		private void Log(TraceEventType eventType, string format, params object[] args)
		{
			string message = string.Format(format, args);
			Logger.TraceEvent(
				eventType,
				0,
				"{0}: ReportExecution {1}: {2}",
				System.Threading.Thread.CurrentThread.ManagedThreadId,
				this.Id,
				message);
		}

		#endregion

		public XElement reportOptionsElement { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Emdat.InVision.SSRS;
using System.Xml.Serialization;

namespace Emdat.InVision
{
    public partial class Subscription
    {
        private static XmlSerializer _parameterValueSerializer = new XmlSerializer(typeof(ParameterValue));        

        public string GetParameterValuesXml()
        {
            return ReportParametersHelper.GetParameterValuesXml(this.Parameters);
        }

        public string GetNextRunParameterValuesXml(DateTime sourceDateTime)
        {
            //calculate parameter values to be used for the first run
            var rptPrms = Report.GetReportParameters(this.ReportId, this.Parameters, int.Parse(this.OwnerId), this.ClientId, this.Application.Value, this.ReportingEnvironmentId);
            var firstRunPrms =
                from r in rptPrms                
                select new ParameterValue
                {
                    Name = r.Name,
                    Value = GetParameterValueFromExpression(r, sourceDateTime)
                };
            return ReportParametersHelper.GetParameterValuesXml(firstRunPrms);
        }

        private static string GetParameterValueFromExpression(ReportParameterViewItem r, DateTime sourceDateTime)
        {
            if (!string.IsNullOrEmpty(r.Value) && 
                r.Type == ReportParameterTypeEnum.DateTime)
            {
                DateTime? dt = DateTimeExpression.Evaluate(r.Value, sourceDateTime);
                return dt.HasValue ? dt.Value.ToString("yyyy-MM-dd") : null;                    
            }
            return r.Value;
        }

        private static ExecutionStatusEnum GetSubscriptionStatus(
            Emdat.InVision.Sql.GetSubscriptionPreviousExecutionRow previousExecution,
            Emdat.InVision.Sql.GetSubscriptionNextExecutionRow nextExecution)
        {
            if (nextExecution != null && nextExecution.ReportExecutionStatusID != (int)ExecutionStatusEnum.Idle)
            {
                //if the next execution has been queued, return its status
                return (ExecutionStatusEnum)nextExecution.ReportExecutionStatusID;
            }
            else if (previousExecution != null)
            {
                //otherwise, if there is a previous execution, return its status
                return (ExecutionStatusEnum)previousExecution.ReportExecutionStatusID;
            }

            //otherwise, the execution has never run and it is not yet queued, so it is NEW
            return ExecutionStatusEnum.New;
        }
    }
}

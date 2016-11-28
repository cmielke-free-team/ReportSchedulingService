using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Emdat.InVision.Generator
{
    public class CustomColumn
    {
        public string FieldName { get; set; }                
        public int? GroupOrder { get; set; }
        public int? SortOrder { get; set; }
        public int DisplayOrder { get; set; }        
        public string FunctionExpression { get; set; }
        public string CaptionExpression { get; set; }
        public string ValueExpression { get; set; }
        public string GroupExpression { get; set; }
        public string SortExpression { get; set; }
        public string SortDirection { get; set; }
        public double Width { get; set; }
        public string Format { get; set; }

        public string RdlWidth
        {
            get
            {
                if (this.IsGrouped)
                {
                    return "0.1in";
                }
                else
                {
                    return string.Format("{0}in", this.Width);
                }
            }
        }

        public bool IsGrouped
        {
            get
            {
                return this.GroupOrder.GetValueOrDefault(-1) > -1;
            }
        }

        public string FunctionName { get; set; }
    }
}

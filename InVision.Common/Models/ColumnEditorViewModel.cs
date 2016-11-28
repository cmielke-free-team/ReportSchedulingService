using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace Emdat.InVision.Models
{
    public class Column
    {
        public string FieldName { get; set; }
        public string FunctionName { get; set; }
        public int? GroupOrder { get; set; }
        public int? SortOrder { get; set; }
        public int DisplayOrder { get; set; }
        public string SortDirection { get; set; }
    }

    public class AvailableColumn
    {
        public string FieldName { get; set; }
        public string UIDisplayName { get; set; }
        public string UIDisplayType { get; set; }
        public string CaptionExpression { get; set; }
        public string ValueExpression { get; set; }
        public string GroupExpression { get; set; }
        public string SortExpression { get; set; }
        public double Width { get; set; }
        public string Format { get; set; }
        public List<Function> AvailableFunctions { get; set; }        
    }

    public class Function
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlText]
        public string Expression { get; set; }
    }

    [XmlRoot(ElementName = "ReportColumns")]
    public class ColumnEditorViewModel
    {
        public string FilePath { get; set; }
        public List<Column> Columns { get; set; }
        public List<AvailableColumn> AvailableColumns { get; set; }
    }
}

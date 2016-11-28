using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Linq;
using System.IO;

namespace Emdat.InVision
{
    /// <summary>
    /// Category class
    /// </summary>    
    [DataObject(true)]
    public class Category
    {
        /// <summary>
        /// Lists the categories.
        /// </summary>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static IEnumerable<Category> ListCategories(int userId, ReportingApplication application)
        {
#if MOCKUP
            XDocument doc = XDocument.Load(Path.Combine(System.Configuration.ConfigurationManager.AppSettings["AppDataPathForMockup"], "Reports.xml"));
            XElement root = doc.Element("Reports");
            var reportCategories =
                from e in root.Elements("Report")
                orderby !string.IsNullOrEmpty(e.Attribute("Category_UISortOrder").Value) ? (int)e.Attribute("Category_UISortOrder") : int.MaxValue
                group e by (string)e.Attribute("Category") into categoryGroup
                select new Category
                {
                    Name = !string.IsNullOrEmpty(categoryGroup.Key) ? categoryGroup.Key : "Miscellaneous Reports"
                };

            return reportCategories;
#else

            using (var sql = new Emdat.InVision.Sql.ReportingDataContext(application))
            {
                var reportCategories =
                    from r in sql.ListReports(userId)
                    orderby r.CategorySortOrder
                    group r by r.CategoryName into categoryGroup
                    select new Category
                    {
                        Name = !string.IsNullOrEmpty(categoryGroup.Key) ? categoryGroup.Key : "Miscellaneous Reports"
                    };

                return reportCategories.ToList();
            }

#endif
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [DataObjectField(false, true)]
        public string Name { get; set; }        

        /// <summary>
        /// Gets or sets the reports.
        /// </summary>
        /// <value>The reports.</value>
        [DataObjectField(false, false)]
        public IEnumerable<Report> Reports { get; set; }
    }
}

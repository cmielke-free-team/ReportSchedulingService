using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Linq;

namespace Emdat.InVision
{
    /// <summary>
    /// ReportFormat class
    /// </summary>
    [DataObject(true)]
    public class ReportFormat
    {
        /// <summary>
        /// Lists the report formats.
        /// </summary>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static IEnumerable<ReportFormat> ListReportFormats()
        {
#if MOCKUP

            return new ReportFormat[]
            {
                new ReportFormat
                {
                    Id = "1",
                    Name = "Comma Separated Values",
                    FileExtension = "csv",
                    Description = "",
                    IsActive = true,
                    Value = "CSV"
                },
                new ReportFormat
                {
                    Id = "2",
                    Name = "HTML",
                    FileExtension = "html",
                    Description = "",
                    IsActive = true,
                    Value = "HTML"
                },
                new ReportFormat
                {
                    Id = "3",
                    Name = "Adobe PDF",
                    FileExtension = "pdf",
                    Description = "",
                    IsActive = true,
                    Value = "PDF"
                },
                new ReportFormat
                {
                    Id = "1",
                    Name = "XML",
                    FileExtension = "xml",
                    Description = "",
                    IsActive = true,
                    Value = "XML"
                }                
            };

#else
            if (_formats == null ||
                _expireTime < DateTime.Now)
            {
                using (var sql = new Emdat.InVision.Sql.ReportingDataContext())
                {
                    var formats =
                        from f in sql.ListFormats()
                        where f.IsActive
                        orderby f.Name
                        select new ReportFormat
                        {
                            Id = f.ReportFormatID.ToString(),
                            Name = f.Name,
                            Description = f.Description,
                            IsActive = f.IsActive,
                            Value = f.SSRSFormat,
                            FileExtension = f.Extension,
                            HttpContentType = f.HTTPContentType,
                            DeviceInfo = f.SSRSDeviceInfo
                        };
                    _formats = formats.ToList();
                    _expireTime = DateTime.Now.AddMinutes(5);
                }
            }
            return _formats;
#endif
        }
        private static List<ReportFormat> _formats;
        private static DateTime _expireTime = DateTime.MinValue;

        /// <summary>
        /// Loads the report format.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static ReportFormat LoadReportFormat(string id)
        {
            int formatId = int.Parse(id);

#if MOCKUP

#else
            using (var sql = new Emdat.InVision.Sql.ReportingDataContext())
            {
                var formats =
                    from f in sql.GetFormat(formatId)
                    select new ReportFormat
                    {
                        Id = f.ReportFormatID.ToString(),
                        Name = f.Name,
                        Description = f.Description,
                        IsActive = f.IsActive,
                        Value = f.SSRSFormat,
                        FileExtension = f.Extension,
                        HttpContentType = f.HTTPContentType,
                        DeviceInfo = f.SSRSDeviceInfo
                    };
                return formats.FirstOrDefault();
            }
#endif
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        [DataObjectField(true, true, false)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [DataObjectField(false, false, false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [DataObjectField(false, false, true)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
        [DataObjectField(false, false, false)]
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the file extension.
        /// </summary>
        /// <value>The file extension.</value>
        [DataObjectField(false, false, false)]
        public string FileExtension { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        [DataObjectField(false, false, false)]
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the type of the HTTP content.
        /// </summary>
        /// <value>The type of the HTTP content.</value>
        [DataObjectField(false, false, false)]
        public string HttpContentType { get; set; }

        /// <summary>
        /// Gets or sets the device info.
        /// </summary>
        /// <value>The device info.</value>
        [DataObjectField(false, false, false)]
        public string DeviceInfo { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public string DeviceInfoQueryString
        {
            get
            {
                if (string.IsNullOrEmpty(this.DeviceInfo))
                {
                    return this.DeviceInfo;
                }

                XElement deviceInfoElement = XElement.Parse(this.DeviceInfo);
                var queryString =
                    from e in deviceInfoElement.Elements()
                    select string.Format("rc:{0}={1}", Uri.EscapeDataString(e.Name.LocalName), Uri.EscapeDataString(e.Value));

                string retVal = string.Join("&", queryString.ToArray());
                return retVal;
            }
        }
    }
}

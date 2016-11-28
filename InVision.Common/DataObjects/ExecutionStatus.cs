using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Emdat.InVision
{
    /// <summary>
    /// 
    /// </summary>
    [DataObject(true)]
    public class ExecutionStatus
    {
        /// <summary>
        /// Lists the execution statuses.
        /// </summary>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static IEnumerable<ExecutionStatus> ListExecutionStatuses()
        {
#if MOCKUP

            return new ExecutionStatus[]
            {
                new ExecutionStatus
                {
                    Id = "1",
                    Name = "Pending",
                    Description = ""
                },              
                new ExecutionStatus
                {
                    Id = "2",
                    Name = "Queued",
                    Description = ""
                },
                new ExecutionStatus
                {
                    Id = "3",
                    Name = "Running",
                    Description = ""
                },
                new ExecutionStatus
                {
                    Id = "4",
                    Name = "Succeeded",
                    Description = ""
                },
                new ExecutionStatus
                {
                    Id = "5",
                    Name = "Error",
                    Description = ""
                }  
            };

#else
            if (_statuses == null ||
                _expireTime < DateTime.Now)
            {
                using (var sql = new Emdat.InVision.Sql.ReportingDataContext())
                {
                    var statuses =
                        from s in sql.ListExecutionStatuses()
                        select new ExecutionStatus
                        {
                            Id = (ExecutionStatusEnum)s.ReportExecutionStatusID,
                            Name = s.Name,
                            Description = s.Description
                        };
                    _statuses = statuses.ToList();
                    _expireTime = DateTime.Now.AddMinutes(5);
                }
            }
            return _statuses;
#endif

        }
        private static List<ExecutionStatus> _statuses;
        private static DateTime _expireTime = DateTime.MinValue;

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        [DataObjectField(true, true, false)]
        public ExecutionStatusEnum Id { get; set; }

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
        [DataObjectField(false, false, false)]
        public string Description { get; set; }        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Emdat.InVision
{
	[DataObject(true)]
	public class ScheduleOption
	{
		/// <summary>
		/// Lists the schedule options.
		/// </summary>
		/// <returns></returns>
		[DataObjectMethod(DataObjectMethodType.Select, true)]
		public static IEnumerable<ScheduleOption> ListScheduleOptions()
		{
			return new ScheduleOption[]
			{
					new ScheduleOption
					{
							Id = ((int)ScheduleOptionEnum.QueueNow).ToString(),
							Name = "Queue Now",
							Description = ""
					},              
					new ScheduleOption
					{
							Id = ((int)ScheduleOptionEnum.Schedule).ToString(),
							Name = "Schedule",
							Description = ""
					}
			};
		}
		//private static List<ScheduleOption> _schOptions;

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
		[DataObjectField(false, false, false)]
		public string Description { get; set; }        
	}
}

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
    public class ExecutionError
    {
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

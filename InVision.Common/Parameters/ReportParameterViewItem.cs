using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Emdat.InVision.SSRS;
using System.Globalization;

namespace Emdat.InVision
{
    /// <summary>
    /// ReportParameterViewItem
    /// </summary>    
    public class ReportParameterViewItem
    {
        /// <summary>
        /// Gets the label.
        /// </summary>
        /// <param name="pv">The pv.</param>
        /// <param name="validValues">The valid values.</param>
        /// <returns></returns>
        private static string GetLabel(ParameterValue pv, ValidValue[] validValues)
        {
            if (pv == null)
            {
                return null;
            }

            if (validValues == null)
            {
                if (0 == string.Compare(pv.Name, "Associate_ID", StringComparison.OrdinalIgnoreCase) &&
                    !string.IsNullOrEmpty(pv.Value))
                {
                    var assoc = AssociateItem.Load(pv.Value);
                    return assoc.Name;
                }

                DateTime tmpDateTime;
                if (DateTime.TryParseExact(pv.Value, DateTimeExpression.ValidDateTimeFormats, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out tmpDateTime))
                {
                    return tmpDateTime.ToString("d", CultureInfo.CurrentCulture);
                }

                bool tmp;
                if (bool.TryParse(pv.Value, out tmp))
                {
                    return tmp ? "Yes" : "No";
                }
                return pv.Value;
            }


            return
                (from v in validValues
                 where pv.Value == v.Value
                 select v.Label)
                 .FirstOrDefault();
        }

        /// <summary>
        /// Gets the type of the report parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private static ReportParameterTypeEnum GetReportParameterType(ReportParameter parameter)
        {
            if (parameter.ValidValues != null || parameter.ValidValuesQueryBased)
            {
                if (parameter.MultiValue)
                {
                    return ReportParameterTypeEnum.MultiSelection;
                }
                else
                {
                    return ReportParameterTypeEnum.Selection;
                }
            }
            else if (parameter.Name == "Associate_ID")
            {
                return ReportParameterTypeEnum.Associate;
            }
            else
            {
                switch (parameter.Type)
                {
                    case ParameterTypeEnum.Boolean:
                        return ReportParameterTypeEnum.Boolean;
                    case ParameterTypeEnum.DateTime:
                        return ReportParameterTypeEnum.DateTime;
                    case ParameterTypeEnum.Float:
                        return ReportParameterTypeEnum.Float;
                    case ParameterTypeEnum.Integer:
                        return ReportParameterTypeEnum.Integer;
                    default:
                        return ReportParameterTypeEnum.String;
                }
            }
        }

        public ReportParameterViewItem() { }

        public ReportParameterViewItem(ReportParameter parameter, ParameterValue value, string[] dependents)
        {
            IsVisible = (parameter.PromptUser && !string.IsNullOrEmpty(parameter.Prompt));
            Prompt = parameter.Prompt;
            Name = parameter.Name;
            Type = GetReportParameterType(parameter);
            ValidValues = parameter.ValidValues;
            Value = value != null ? value.Value : null;
            Label = GetLabel(value, parameter.ValidValues);
            IsRequired = !parameter.Nullable;
            DependentParameters = dependents;
            State = parameter.State;
        }

        public ReportParameterViewItem(ReportParameter parameter, ParameterValue value) :
            this(parameter, value, null)
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is visible.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is visible; otherwise, <c>false</c>.
        /// </value>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>The label.</value>
        public string Prompt { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>The label.</value>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public ReportParameterTypeEnum Type { get; set; }

        /// <summary>
        /// Gets or sets the valid values.
        /// </summary>
        /// <value>The valid values.</value>
        public ValidValue[] ValidValues { get; set; }

        /// <summary>
        /// Gets or sets the dependent parameters.
        /// </summary>
        /// <value>The dependent parameters.</value>
        public string[] DependentParameters { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance has dependents.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has dependents; otherwise, <c>false</c>.
        /// </value>
        public bool HasDependents
        {
            get
            {
                return this.DependentParameters != null && this.DependentParameters.Length > 0;
            }
        }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        public ParameterStateEnum State { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsEnabled
        {
            get
            {
                return this.State != ParameterStateEnum.HasOutstandingDependencies;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is required.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is required; otherwise, <c>false</c>.
        /// </value>
        public bool IsRequired { get; set; }        
    }
}

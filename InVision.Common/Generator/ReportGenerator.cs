using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using Emdat.InVision.Models;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Emdat.InVision.Generator
{
	public static class ReportGenerator
	{
		private static readonly String ReportDefinitionNamespace = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition";
		private static readonly String ReportDesignerNamespace = "http://schemas.microsoft.com/SQLServer/reporting/reportdesigner";

		private static String GetStyleDataColor(string styleDataSetName)
		{
			return string.Format("=First(Fields!Data_Color.Value, \"{0}\")", styleDataSetName);
		}
		private static String GetStyleGroupBackColor(string styleDataSetName)
		{
			return string.Format("=First(Fields!Group_BackColor.Value, \"{0}\")", styleDataSetName);
		}
		private static String GetStyleRowHeaderFillValue(string styleDataSetName)
		{
			return string.Format("=First(Fields!row_header_fill.Value, \"{0}\")", styleDataSetName);
		}
		private static string GetStyleRowFill(string styleDataSetName)
		{
			return string.Format("=iif(RowNumber(Nothing) Mod 2, First(Fields!row_fill_1.Value, \"{0}\"), First(Fields!row_fill_2.Value, \"{0}\"))", styleDataSetName);
		}
		private static string GetStyleGrandTotalBackColor(string styleDataSetName)
		{
			return string.Format("=First(Fields!GrandTotal_BackColor.Value, \"{0}\")", styleDataSetName);
		}

		private static String GetRandomTextboxName()
		{
			return "TextBox" + Guid.NewGuid().ToString().Replace("-", "");
		}

		public static byte[] GenerateReportDefinition(XElement rdlTemplate, XElement reportColumns, bool hideDetail, string reportFormat)
		{
			//get placeholder tablix
			XNamespace xmlns = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition";
			XElement tablixPlaceholder = rdlTemplate
				.Elements(xmlns + "Body")
				.Elements(xmlns + "ReportItems")
				.Elements(xmlns + "Tablix")
				.First();

			string mainDataSetName = (string)tablixPlaceholder.Element(xmlns + "DataSetName");
			string styleDataSetName =
				(from dataSet in rdlTemplate.Elements(xmlns + "DataSets").Elements(xmlns + "DataSet")
				 let name = (string)dataSet.Attribute("Name") ?? string.Empty
				 where name.EndsWith("StyleOptions", StringComparison.InvariantCultureIgnoreCase)
				 select name)
				 .DefaultIfEmpty("StyleOptions")
				 .FirstOrDefault();

			double tablixWidth;

			//generate tablix
            XElement newTablix;
            if(reportFormat.ToUpper()=="CSV"){ //&& hideDetail == true){
               newTablix  = GenerateCSVTablix(mainDataSetName, styleDataSetName, reportColumns, hideDetail, out tablixWidth, reportFormat);
            }
            else{
                newTablix = GenerateTablix(mainDataSetName, styleDataSetName, reportColumns, hideDetail, out tablixWidth, reportFormat);
            }

			//replace placeholder tablix with the generated tablix 
			tablixPlaceholder.ReplaceWith(newTablix);

			// Set the width of non-tablix elements
			rdlTemplate.SetElementValue(xmlns + "Width", string.Format("{0}in", tablixWidth));
			var page = rdlTemplate.Elements(xmlns + "Page").FirstOrDefault();
			page.SetElementValue(xmlns + "PageWidth", string.Format("{0}in", tablixWidth + 1));
			var parametersTxt = rdlTemplate.Elements(xmlns + "Body").Elements(xmlns + "ReportItems").Elements(xmlns + "Textbox").Where(e => e.Attribute("Name").Value == "ParametersTxt").FirstOrDefault();
			if (parametersTxt != null)
			{
				parametersTxt.SetElementValue(xmlns + "Width", string.Format("{0}in", tablixWidth));
			}
			var noteTimeZone = rdlTemplate.Elements(xmlns + "Body").Elements(xmlns + "ReportItems").Elements(xmlns + "Textbox").Where(e => e.Attribute("Name").Value == "NoteTimeZone").FirstOrDefault();
			if (noteTimeZone != null)
			{
				noteTimeZone.SetElementValue(xmlns + "Width", string.Format("{0}in", tablixWidth));
			}
			var reportName = rdlTemplate.Elements(xmlns + "Body").Elements(xmlns + "ReportItems").Elements(xmlns + "Textbox").Where(e => e.Attribute("Name").Value == "ReportName").FirstOrDefault();
			if (reportName != null)
			{
				reportName.SetElementValue(xmlns + "Width", string.Format("{0}in", tablixWidth));
			}
			var pageNumber = rdlTemplate.Elements(xmlns + "Page").Elements(xmlns + "PageFooter").Elements(xmlns + "ReportItems").Elements(xmlns + "Textbox").Where(e => e.Attribute("Name").Value == "PageNumber").FirstOrDefault();
			if (pageNumber != null)
			{
				pageNumber.SetElementValue(xmlns + "Width", string.Format("{0}in", tablixWidth));
			}

			//save new RDL XML to byte[]
			return Encoding.UTF8.GetBytes(rdlTemplate.ToString());
			//using (var newRdlStream = new MemoryStream())
			//using (var newRdlWriter = XmlWriter.Create(newRdlStream))
			//{
			//    rdlTemplate.Save(newRdlWriter);

			//    newRdlWriter.Flush();
			//    newRdlStream.Flush();
			//    return newRdlStream.ToArray();
			//}
		}

        private static XElement GenerateCSVTablix(string mainDataSetName, string styleDataSetName, XElement reportColumns, bool hideDetail, out double tablixWidth, string reportFormat)
        {
            var viewModelSerializer = new XmlSerializer(typeof(ColumnEditorViewModel));
            ColumnEditorViewModel viewModel;
            using (var reader = reportColumns.CreateReader())
            {
                viewModel = viewModelSerializer.Deserialize(reader) as ColumnEditorViewModel;
            }

            var customColumns =
                from c in viewModel.Columns
                join a in viewModel.AvailableColumns on c.FieldName equals a.FieldName
                select new CustomColumn
                {
                    CaptionExpression = a.CaptionExpression,
                    DisplayOrder = c.DisplayOrder,
                    FieldName = c.FieldName,
                    Format = a.Format,
                    FunctionExpression = a.AvailableFunctions
                        .Where(f => f.Name.Equals(c.FunctionName, StringComparison.InvariantCultureIgnoreCase))
                        .Select(f => f.Expression).FirstOrDefault(),
                    GroupExpression = a.GroupExpression,
                    GroupOrder = c.GroupOrder,
                    SortDirection = c.SortDirection,
                    SortExpression = a.SortExpression,
                    SortOrder = c.SortOrder,
                    ValueExpression = a.ValueExpression,
                    Width = a.Width,
                    FunctionName = c.FunctionName
                };

            var groupColumns = customColumns.Where(c => c.GroupOrder >= 0).OrderBy(c => c.GroupOrder).ToList();

            // Details columns are filtered by hide detail here, if details are hidden then only ungrouped columns with functions are relevant
            var detailColumns = customColumns.Where(c => (c.GroupOrder < 0) && (!hideDetail || !string.IsNullOrEmpty(c.FunctionName))).OrderBy(c => c.DisplayOrder).ToList();
            var sortColumns = customColumns.Where(c => c.GroupOrder < 0 && c.SortOrder.HasValue && c.SortOrder.Value > -1).OrderBy(c => c.SortOrder.Value).ToList();
            var columns = new List<CustomColumn>();
            columns.AddRange(groupColumns);
            columns.AddRange(detailColumns);

            var groupCount = groupColumns.Count;
            var columnCount = columns.Count;

            // Set override column width to hide detail because we always want to stretch the innermost group column if we're hiding details, otherwise it is up to the layout of the columns if we need to stretch the column or not
            var overrideColumnWidth = hideDetail;

            var bodyBuilder = new StringBuilder();
            var bodyWriterSettings = new XmlWriterSettings();
            bodyWriterSettings.OmitXmlDeclaration = true;
            bodyWriterSettings.Indent = true;
            bodyWriterSettings.ConformanceLevel = ConformanceLevel.Fragment;
            using (var bodyWriter = XmlWriter.Create(bodyBuilder, bodyWriterSettings))
            {

                bodyWriter.WriteStartElement("Tablix", ReportDefinitionNamespace);
                bodyWriter.WriteAttributeString("Name", "tblCustomColumns");
                bodyWriter.WriteAttributeString("Top", "1.5in");
                bodyWriter.WriteStartElement("TablixBody");

                bodyWriter.WriteStartElement("TablixRows");

                var rowCount = 0;
                var groupColCount = 0;
                var detailColCount = 0;
                var totalColCount = 0;

                // Write Grand Header Row if HideDetail

                //get the number of groupings if any plus a row for details
                rowCount = groupCount + 2; //header row, row for each group, details row

                //BEGIN HEADER ROW
                bodyWriter.WriteStartElement("TablixRow");
                bodyWriter.WriteElementString("Height", "0.25in");
                bodyWriter.WriteStartElement("TablixCells");

                for (var i = 0; i < groupCount; i++)
                {
                    var groupColumn = groupColumns[i];
                    WriteCSVHeaderCell(bodyWriter, groupColumn.CaptionExpression, groupColumn.ValueExpression, 1, styleDataSetName, groupColumn.FieldName,"NoOutput");
                    groupColCount++;
                    
                }
                foreach (var detailColumn in detailColumns)
                {
                    if (hideDetail)
                    {
                        if (detailColumn.FunctionName.ToUpper() != "NONE")
                        {
                            WriteCSVHeaderCell(bodyWriter, detailColumn.FunctionName + "_" + detailColumn.CaptionExpression, detailColumn.ValueExpression, 1, styleDataSetName, detailColumn.FieldName, "NoOutput");
                            detailColCount++;
                        }
                    }
                    else
                    {
                        WriteCSVHeaderCell(bodyWriter, detailColumn.CaptionExpression, detailColumn.ValueExpression, 1, styleDataSetName, detailColumn.FieldName, "NoOutput");
                        detailColCount++;
                    }
                }
                totalColCount = groupColCount + detailColCount;
                bodyWriter.WriteEndElement(); // TablixCells
                bodyWriter.WriteEndElement(); // TablixRow
                //END HEADER ROW

                //BEGIN GROUP ROWS

                //If hideDetail = false then just show details
                var groupRowCount = 0;

                if (!hideDetail)
                {
                    bodyWriter.WriteStartElement("TablixRow");
                    bodyWriter.WriteElementString("Height", "0.25in");
                    bodyWriter.WriteStartElement("TablixCells");

                    for (var j = 0; j < groupCount; j++)
                    {
                        WriteCSVDetailCell(bodyWriter, groupColumns[j].ValueExpression, styleDataSetName, groupColumns[j].FieldName, null);
                    }
                    //now write the details
                    //Now write the expression for the detail columns.
                    foreach (var detailColumn in detailColumns)
                    {
                        WriteCSVDetailCell(bodyWriter, detailColumn.ValueExpression, styleDataSetName, detailColumn.FieldName, null);
                    }

                    bodyWriter.WriteEndElement(); // TablixCells
                    bodyWriter.WriteEndElement(); // TablixRow
                    groupRowCount++;
                }
                else
                {
                    for (var j = 0; j < groupCount; j++)
                    {
                        bodyWriter.WriteStartElement("TablixRow");
                        bodyWriter.WriteElementString("Height", "0.25in");
                        bodyWriter.WriteStartElement("TablixCells");
                        if (j == 0)  //first group row
                        {
                            WriteCSVDetailCell(bodyWriter, groupColumns[j].ValueExpression, styleDataSetName, groupColumns[j].FieldName, null);
                            if (j == groupCount - 1)
                            {
                                //Now write the expression for the detail columns.
                                foreach (var detailColumn in detailColumns)
                                {
                                    if (detailColumn.FunctionName.ToUpper() != "NONE")
                                    {
                                        WriteCSVDetailCell(bodyWriter, detailColumn.FunctionExpression, styleDataSetName, detailColumn.FunctionName + "_" + detailColumn.FieldName, null);
                                    }
                                }
                            }
                            else
                            {
                                WriteSpacerCells(bodyWriter, totalColCount - 1, styleDataSetName);
                            }
                        }
                        else
                        {
                            WriteSpacerCells(bodyWriter, j, styleDataSetName);
                            WriteCSVDetailCell(bodyWriter, groupColumns[j].ValueExpression, styleDataSetName, groupColumns[j].FieldName, null);
                            if (j == groupCount - 1)
                            {
                                //Now write the expression for the detail columns.
                                foreach (var detailColumn in detailColumns)
                                {
                                    if (detailColumn.FunctionName.ToUpper() != "NONE")
                                    {
                                        WriteCSVDetailCell(bodyWriter, detailColumn.FunctionExpression, styleDataSetName, detailColumn.FunctionName + "_" + detailColumn.FieldName, null);
                                    }
                                }
                            }
                            else
                            {
                                WriteSpacerCells(bodyWriter, (totalColCount - (j + 1)), styleDataSetName);
                            }
                        }


                        bodyWriter.WriteEndElement(); // TablixCells
                        bodyWriter.WriteEndElement(); // TablixRow
                        groupRowCount++;
                    }
                }
                //END GROUP ROWS

                //BEGIN TOTALS ROWS
                bodyWriter.WriteStartElement("TablixRow");
                bodyWriter.WriteElementString("Height", "0.25in");
                bodyWriter.WriteStartElement("TablixCells");

                WriteSpacerCells(bodyWriter, groupColCount, styleDataSetName);

                string dataElementOutputVal = (groupCount == 0) ? "Output" : "NoOutput";

                foreach (var detailColumn in detailColumns)
                {
                    if (hideDetail)
                    {
                        if (detailColumn.FunctionName.ToUpper() != "NONE")
                        {
                            WriteCSVGrandTotalDetailCell(bodyWriter, detailColumn.FunctionExpression, styleDataSetName, detailColumn.FunctionName + "_" + detailColumn.FieldName, dataElementOutputVal);
                        }
                    }
                    else
                    {
                        string functionExpression = detailColumn.FunctionExpression;
                        string functionName = detailColumn.FunctionName;
                        if(functionName == null)
                        {
                            functionName = "None";
                            functionExpression = "None";
                        }
                        WriteCSVGrandTotalDetailCell(bodyWriter, detailColumn.FunctionExpression, styleDataSetName, functionName + "_" + detailColumn.FieldName, "NoOutput");
                    }

                }
                bodyWriter.WriteEndElement(); //TablixCells
                bodyWriter.WriteEndElement(); //TablixRow

                bodyWriter.WriteEndElement(); //TablixRows

                bodyWriter.WriteStartElement("TablixColumns");
                for (var j = 0; j < totalColCount; j++)
                {
                    bodyWriter.WriteStartElement("TablixColumn");
                    bodyWriter.WriteElementString("Width", "2in");
                    bodyWriter.WriteEndElement(); //TablixColumn
                }
                bodyWriter.WriteEndElement(); // TablixColumns


                bodyWriter.WriteEndElement(); // TablixBody

                bodyWriter.WriteStartElement("TablixColumnHierarchy");
                bodyWriter.WriteStartElement("TablixMembers");

                for (var i = 0; i < totalColCount; i++)
                {
                    bodyWriter.WriteStartElement("TablixMember");
                    bodyWriter.WriteEndElement(); // TablixMember
                }


                bodyWriter.WriteEndElement(); // TablixMembers
                bodyWriter.WriteEndElement(); // TablixColumnHierarchy

                bodyWriter.WriteStartElement("TablixRowHierarchy");
                bodyWriter.WriteStartElement("TablixMembers");


                bodyWriter.WriteStartElement("TablixMember");
                bodyWriter.WriteEndElement();

                //write details else write goupings
                if (!hideDetail)
                {
                    bodyWriter.WriteStartElement("TablixMember");
                    bodyWriter.WriteStartElement("Group");
                    bodyWriter.WriteAttributeString("Name", "Details");
                    bodyWriter.WriteEndElement(); // Group

                    if (groupCount > 0)
                    {
                        bodyWriter.WriteStartElement("SortExpressions");
                        //for (var i = groupCount - 1; i >= 0; i--)
                        for (var i = 0; i < groupCount; i++ )
                        {
                            var groupColumn = groupColumns[i];
                            bodyWriter.WriteStartElement("SortExpression");
                            bodyWriter.WriteElementString("Value", groupColumn.SortExpression);
                            if (groupColumn.SortDirection == "Descending")
                            {
                                bodyWriter.WriteElementString("Direction", "Descending");
                            }
                            bodyWriter.WriteEndElement(); // SortExpression
                        }
                        bodyWriter.WriteEndElement(); // SortExpressions
                    }
                    bodyWriter.WriteEndElement(); // TablixMember
                }
                else
                {
                    if (groupCount > 0)
                    {
                        for (var i = 0; i < groupCount; i++)
                        {
                            var groupColumn = groupColumns[i];

                            bodyWriter.WriteStartElement("TablixMember");
                            bodyWriter.WriteStartElement("Group");
                            bodyWriter.WriteAttributeString("Name", GetRandomTextboxName());
                            bodyWriter.WriteStartElement("GroupExpressions");
                            bodyWriter.WriteElementString("GroupExpression", groupColumn.GroupExpression);
                            bodyWriter.WriteEndElement(); // GroupExpressions
                            bodyWriter.WriteEndElement(); // Group
                            bodyWriter.WriteStartElement("SortExpressions");
                            bodyWriter.WriteStartElement("SortExpression");
                            bodyWriter.WriteElementString("Value", groupColumn.SortExpression);
                            if (groupColumn.SortDirection == "Descending")
                            {
                                bodyWriter.WriteElementString("Direction", "Descending");
                            }
                            bodyWriter.WriteEndElement(); // SortExpression
                            bodyWriter.WriteEndElement(); // SortExpressions


                            // If details are hidden then we need to suppress the innermost group header row
                            if (i != groupCount - 1)
                            {
                                bodyWriter.WriteStartElement("TablixMembers");
                                bodyWriter.WriteStartElement("TablixMember");
                                bodyWriter.WriteEndElement(); // TablixMember
                                bodyWriter.WriteStartElement("TablixMember");
                                bodyWriter.WriteStartElement("TablixMembers");

                            }

                        }

                        if (groupCount == 1)
                        {
                            bodyWriter.WriteStartElement("TablixMembers");
                            bodyWriter.WriteStartElement("TablixMember");
                            bodyWriter.WriteEndElement();  //TablixMember
                        }


                        var tablixClose = (groupCount == 2) ? groupCount : groupCount + groupCount / 2;
                        for (var i = 0; i < tablixClose; i++)
                        {
                            bodyWriter.WriteEndElement(); // TablixMember
                            bodyWriter.WriteEndElement(); // TablixMembers
                        }

                        if (groupCount > 1) { 
                            bodyWriter.WriteEndElement(); //TablixMember\
                        }
                        //bodyWriter.WriteEndElement(); //TablixMember

                    }

                    if (groupCount == 0)
                    {
                        for (var i = 0; i < groupCount; i++)
                        {
                            // Group Total Value Row
                            bodyWriter.WriteStartElement("TablixMember");
                            //if (!hideDetail && (i == groupCount - 1))
                            //{
                            //    // Details Value Row
                            //    bodyWriter.WriteStartElement("Group");
                            //    bodyWriter.WriteAttributeString("Name", "Details");
                            //    bodyWriter.WriteEndElement(); // Group
                            //}
                            //else
                            //{
                            bodyWriter.WriteElementString("KeepWithGroup", "Before");
                            //}
                            bodyWriter.WriteEndElement(); // TablixMember
                            bodyWriter.WriteEndElement(); // TablixMembers
                            bodyWriter.WriteEndElement(); // TablixMember
                        }
                    }
                }

                // Grand Total Value Row
                bodyWriter.WriteStartElement("TablixMember");
                bodyWriter.WriteElementString("KeepWithGroup", "Before");
                bodyWriter.WriteEndElement(); // TablixMember

                bodyWriter.WriteEndElement(); // TablixMembers
                bodyWriter.WriteEndElement(); // TablixRowHierarchy

                //bodyWriter.Flush();
                //bodyBuilder.ToString();

                //bodyWriter.WriteEndElement(); // TablixRowHierarchy

                var tablixHeight = rowCount * 0.25;
                var bodyHeight = tablixHeight + 2.2175;
                tablixWidth = totalColCount * .25;
                bodyWriter.WriteElementString("DataSetName", mainDataSetName);
                bodyWriter.WriteElementString("Top", ".21in");
                bodyWriter.WriteElementString("Height", string.Format("{0}in", tablixHeight));
                bodyWriter.WriteElementString("Width", string.Format("{0}in", tablixWidth));
                bodyWriter.WriteStartElement("Style");
                bodyWriter.WriteEndElement(); //Style

                bodyWriter.WriteEndElement(); //Tablix
                bodyWriter.Flush();
            }
            return XElement.Parse(bodyBuilder.ToString());
        }

		private static XElement GenerateTablix(string mainDataSetName, string styleDataSetName, XElement reportColumns, bool hideDetail, out double tablixWidth, string reportFormat)
		{
			var viewModelSerializer = new XmlSerializer(typeof(ColumnEditorViewModel));
			ColumnEditorViewModel viewModel;
			using (var reader = reportColumns.CreateReader())
			{
				viewModel = viewModelSerializer.Deserialize(reader) as ColumnEditorViewModel;
			}

			var customColumns =
				from c in viewModel.Columns
				join a in viewModel.AvailableColumns on c.FieldName equals a.FieldName
				select new CustomColumn
				{
					CaptionExpression = a.CaptionExpression,
					DisplayOrder = c.DisplayOrder,
					FieldName = c.FieldName,
					Format = a.Format,
					FunctionExpression = a.AvailableFunctions
						.Where(f => f.Name.Equals(c.FunctionName, StringComparison.InvariantCultureIgnoreCase))
						.Select(f => f.Expression).FirstOrDefault(),
					GroupExpression = a.GroupExpression,
					GroupOrder = c.GroupOrder,
					SortDirection = c.SortDirection,
					SortExpression = a.SortExpression,
					SortOrder = c.SortOrder,
					ValueExpression = a.ValueExpression,
					Width = a.Width,
					FunctionName = c.FunctionName                    
				};

			var groupColumns = customColumns.Where(c => c.GroupOrder >= 0).OrderBy(c => c.GroupOrder).ToList();

			// Details columns are filtered by hide detail here, if details are hidden then only ungrouped columns with functions are relevant
			var detailColumns = customColumns.Where(c => (c.GroupOrder < 0) && (!hideDetail || !string.IsNullOrEmpty(c.FunctionName))).OrderBy(c => c.DisplayOrder).ToList();
			var sortColumns = customColumns.Where(c => c.GroupOrder < 0 && c.SortOrder.HasValue && c.SortOrder.Value > -1).OrderBy(c => c.SortOrder.Value).ToList();
			var columns = new List<CustomColumn>();
			columns.AddRange(groupColumns);
			columns.AddRange(detailColumns);

			var groupCount = groupColumns.Count;
			var columnCount = columns.Count;

			// Set override column width to hide detail because we always want to stretch the innermost group column if we're hiding details, otherwise it is up to the layout of the columns if we need to stretch the column or not
			var overrideColumnWidth = hideDetail;

			var bodyBuilder = new StringBuilder();
			var bodyWriterSettings = new XmlWriterSettings();
			bodyWriterSettings.OmitXmlDeclaration = true;
			bodyWriterSettings.Indent = true;
			bodyWriterSettings.ConformanceLevel = ConformanceLevel.Fragment;
			using (var bodyWriter = XmlWriter.Create(bodyBuilder, bodyWriterSettings))
			{
				bodyWriter.WriteStartElement("Tablix", ReportDefinitionNamespace);
				bodyWriter.WriteAttributeString("Name", "tblCustomColumns");
				bodyWriter.WriteAttributeString("Top", "1.5in");
				bodyWriter.WriteStartElement("TablixBody");

				bodyWriter.WriteStartElement("TablixRows");

				var rowCount = 0;

				// Write Grand Header Row if HideDetail
				if (hideDetail)
				{
					rowCount++;

					bodyWriter.WriteStartElement("TablixRow");
					bodyWriter.WriteElementString("Height", "0.25in");
					bodyWriter.WriteStartElement("TablixCells");

					if (groupCount == 1)
					{
						WriteLabelCell(bodyWriter, "", null, styleDataSetName);
					}
					else if (groupCount > 1)
					{
						WriteLabelCell(bodyWriter, "", groupCount.ToString(), styleDataSetName);
					}

					for (var i = 0; i < groupCount - 1; i++)
					{
						bodyWriter.WriteStartElement("TablixCell");
						bodyWriter.WriteEndElement();
					}

					foreach (var detailColumn in detailColumns)
					{
						var captionExpression = detailColumn.CaptionExpression;

						if ("Count" == detailColumn.FunctionName)
						{
							captionExpression = "Count";
						}

						WriteLabelCell(bodyWriter, captionExpression, null, styleDataSetName);
					}

					bodyWriter.WriteEndElement(); // TablixCells
					bodyWriter.WriteEndElement(); // TablixRow
				}

				// Write Header Rows
				for (var i = 0; i < groupCount; i++)
				{
					var groupColumn = groupColumns[i];
					var colSpan = columns.Count - i;
					var emptyCellCount = colSpan - 1;

					// If details are hidden then we need to suppress the innermost group header row
					if (!hideDetail || i != groupCount - 1)
					{
						rowCount++;
						bodyWriter.WriteStartElement("TablixRow");
						bodyWriter.WriteElementString("Height", "0.25in");
						bodyWriter.WriteStartElement("TablixCells");

						WriteSpacerCells(bodyWriter, i, styleDataSetName);
						WriteHeaderCell(bodyWriter, groupColumn.CaptionExpression, groupColumn.ValueExpression, colSpan, styleDataSetName, groupColumn.FieldName);

						for (var j = 0; j < emptyCellCount; j++)
						{
							bodyWriter.WriteStartElement("TablixCell");
							bodyWriter.WriteEndElement();
						}

						bodyWriter.WriteEndElement(); // TablixCells
						bodyWriter.WriteEndElement(); // TablixRow
					}
				}

				if (!hideDetail)
				{
					// Write Label Row
					rowCount++;
					bodyWriter.WriteStartElement("TablixRow");
					bodyWriter.WriteElementString("Height", "0.25in");
					bodyWriter.WriteStartElement("TablixCells");

					WriteSpacerCells(bodyWriter, groupColumns.Count, styleDataSetName);

					foreach (var detailColumn in detailColumns)
					{
						WriteLabelCell(bodyWriter, detailColumn.CaptionExpression, null, styleDataSetName);
					}

					bodyWriter.WriteEndElement(); // TablixCells
					bodyWriter.WriteEndElement(); // TablixRow

					// Write Detail Row
					rowCount++;
					bodyWriter.WriteStartElement("TablixRow");
					bodyWriter.WriteElementString("Height", "0.25in");
					bodyWriter.WriteStartElement("TablixCells");

					WriteSpacerCells(bodyWriter, groupColumns.Count, styleDataSetName);

					foreach (var detailColumn in detailColumns)
					{
						WriteDetailCell(bodyWriter, detailColumn.ValueExpression, styleDataSetName, detailColumn.FieldName, detailColumn.Format);
					}

					bodyWriter.WriteEndElement(); // TablixCells
					bodyWriter.WriteEndElement(); // TablixRow
				}

				// Write Footer Rows
				for (var i = groupCount - 1; i >= 0; i--)
				{
					var groupColumn = groupColumns[i];

					rowCount++;
					bodyWriter.WriteStartElement("TablixRow");
					bodyWriter.WriteElementString("Height", "0.25in");
					bodyWriter.WriteStartElement("TablixCells");

					String groupTotal = "Totals_For_" + groupColumn.FieldName;

					// Write Empty Spacer Cells
					WriteSpacerCells(bodyWriter, i, styleDataSetName);

					// Calculate how many columns are empty to the right of the label that can be spanned into
					var colSpan = groupCount - i;
					var emptyColumnsAvailable = 0;
					var gainedWidth = 0.0;
					foreach (var detailColumn in detailColumns)
					{
						if (string.IsNullOrEmpty(detailColumn.FunctionName))
						{
							colSpan++;
							emptyColumnsAvailable++;
							gainedWidth += detailColumn.Width;
						}
						else
						{
							break;
						}
					}

					if (gainedWidth < 2.0)
					{
						overrideColumnWidth = true;
					}

					WriteFooterLabelCell(bodyWriter, groupColumn.ValueExpression, colSpan, styleDataSetName, groupTotal);

					for (var j = 0; j < colSpan - 1; j++)
					{
						bodyWriter.WriteStartElement("TablixCell");
						bodyWriter.WriteEndElement();
					}

					// Start iterating over the detail columns beyond the ones that were replaced by empty cells
					for (var j = emptyColumnsAvailable; j < detailColumns.Count; j++)
					{
						var detailColumn = detailColumns[j];

						WriteFooterDetailCell(bodyWriter, detailColumn.FunctionExpression, styleDataSetName, detailColumn.FunctionName + detailColumn.FieldName, detailColumn.Format, null);
					}

					bodyWriter.WriteEndElement(); // TablixCells
					bodyWriter.WriteEndElement(); // TablixRow
				}

				// Write Grand Total Row
				rowCount++;
				bodyWriter.WriteStartElement("TablixRow");
				bodyWriter.WriteElementString("Height", "0.25in");
				bodyWriter.WriteStartElement("TablixCells");


				// Calculate how many columns are empty to the right of the label that can be spanned into
				var grandTotalColSpan = groupCount;
				var grandTotalEmptyColumnsAvailable = 0;
				var grandTotalGainedWidth = 0.0;

                //if groupings > 0 then do this:
                if (groupCount > 0)
                { 
				    foreach (var detailColumn in detailColumns)
				    {
					    if (string.IsNullOrEmpty(detailColumn.FunctionName))
					    {
						    grandTotalColSpan++;
						    grandTotalEmptyColumnsAvailable++;
						    grandTotalGainedWidth += detailColumn.Width;
					    }
					    else
					    {
						    break;
					    }
				    }
                }
                //

				if (grandTotalGainedWidth < 2.0)
				{
					overrideColumnWidth = true;
				}

                if (groupCount !=0){
				    WriteGrandTotalDetailCell(bodyWriter, "Grand Total", styleDataSetName, null, null, grandTotalColSpan.ToString());
                }

				for (var i = 0; i < grandTotalColSpan - 1; i++)
				{
					bodyWriter.WriteStartElement("TablixCell");
					bodyWriter.WriteEndElement();
				}

				// Start iterating over the detail columns beyond the ones that were replaced by empty cells
				for (var i = grandTotalEmptyColumnsAvailable; i < detailColumns.Count; i++)
				{
					var detailColumn = detailColumns[i];

					WriteGrandTotalDetailCell(bodyWriter, detailColumn.FunctionExpression, styleDataSetName, detailColumn.FunctionName + detailColumn.FieldName, detailColumn.Format, null);
				}

				bodyWriter.WriteEndElement(); // TablixCells
				bodyWriter.WriteEndElement(); // TablixRow
				bodyWriter.WriteEndElement(); // TablixRows

				bodyWriter.WriteStartElement("TablixColumns");

				double newPageWidth = 0;

				for (var i = 0; i < columns.Count; i++)
				{
					var column = columns[i];

					bodyWriter.WriteStartElement("TablixColumn");

					/*// Determine the maximum column width
					string newColumnWidth = column.RdlWidth;
					string delim = "in";
					string trimmedString = newColumnWidth.Trim(delim.ToCharArray());
					decimal columnWidthDecimal = Convert.ToDecimal(trimmedString);
					decimal pageWidthDecimal = Convert.ToDecimal(pageWidth);
					decimal columnCountDecimal = Convert.ToDecimal(columnCount);
					decimal groupCountDecimal = Convert.ToDecimal(groupCount);
					// maxColumnWidth is the page width minus 0.1 inches for every grouping option, divided by the number of data columns
					decimal maxColumnWidth = ((pageWidthDecimal - (groupCountDecimal / 10)) / (columnCountDecimal - groupCountDecimal));
					// Change the column width if it exceeds maxColumnWidth
					if (columnWidthDecimal > maxColumnWidth || columnWidthDecimal == 0)
					{
						newColumnWidth = Convert.ToString(maxColumnWidth) + "in";
					}*/

					// Convert column width String to Double
					string delim = "in";
					string width;

					if (overrideColumnWidth && i == groupCount - 1)
					{
						width = "2in";
					}
					else
					{
						width = column.RdlWidth;
					}

					var trimmedString = width.Trim(delim.ToCharArray());

					double columnWidthDouble = Convert.ToDouble(trimmedString);

					// Add column widths together to determine page width
					newPageWidth += columnWidthDouble;
					bodyWriter.WriteElementString("Width", width);
					bodyWriter.WriteEndElement(); // TablixColumn
				}

				bodyWriter.WriteEndElement(); // TablixColumns

				tablixWidth = newPageWidth;

				bodyWriter.WriteEndElement(); // TablixBody

				bodyWriter.WriteStartElement("TablixColumnHierarchy");
				bodyWriter.WriteStartElement("TablixMembers");

				for (var i = 0; i < columnCount; i++)
				{
					bodyWriter.WriteStartElement("TablixMember");
					bodyWriter.WriteEndElement(); // TablixMember
				}

				bodyWriter.WriteEndElement(); // TablixMembers
				bodyWriter.WriteEndElement(); // TablixColumnHierarchy

				bodyWriter.WriteStartElement("TablixRowHierarchy");
				bodyWriter.WriteStartElement("TablixMembers");

				// Write Grand Header Row
				if (hideDetail)
				{
					bodyWriter.WriteStartElement("TablixMember");
					bodyWriter.WriteElementString("RepeatOnNewPage", "true");
					bodyWriter.WriteEndElement(); // TablixMember
				}

                for (var i = 0; i < groupCount; i++)
				{
					var groupColumn = groupColumns[i];
					bodyWriter.WriteStartElement("TablixMember");
					bodyWriter.WriteStartElement("Group");
					bodyWriter.WriteAttributeString("Name", GetRandomTextboxName());
					bodyWriter.WriteStartElement("GroupExpressions");
					bodyWriter.WriteElementString("GroupExpression", groupColumn.GroupExpression);
					bodyWriter.WriteEndElement(); // GroupExpressions
					bodyWriter.WriteEndElement(); // Group
					bodyWriter.WriteStartElement("SortExpressions");
					bodyWriter.WriteStartElement("SortExpression");
					bodyWriter.WriteElementString("Value", groupColumn.SortExpression);
					if (groupColumn.SortDirection == "Descending")
					{
						bodyWriter.WriteElementString("Direction", "Descending");
					}
					bodyWriter.WriteEndElement(); // SortExpression
					bodyWriter.WriteEndElement(); // SortExpressions
					bodyWriter.WriteStartElement("TablixMembers");

					// If details are hidden then we need to suppress the innermost group header row
                    if (!hideDetail || i != groupCount - 1)
					{
						// Group Label Row
						bodyWriter.WriteStartElement("TablixMember");
						bodyWriter.WriteElementString("RepeatOnNewPage", "true");
						bodyWriter.WriteElementString("KeepWithGroup", "After");
						bodyWriter.WriteEndElement(); // TablixMember
					}
				}

				if (!hideDetail)
				{
					// Details Label Row
					bodyWriter.WriteStartElement("TablixMember");
					bodyWriter.WriteElementString("RepeatOnNewPage", "true");
					bodyWriter.WriteElementString("KeepWithGroup", "After");
					bodyWriter.WriteEndElement(); // TablixMember

					// Details Value Row
					bodyWriter.WriteStartElement("TablixMember");
					bodyWriter.WriteStartElement("Group");
					bodyWriter.WriteAttributeString("Name", "Details");
					bodyWriter.WriteEndElement(); // Group

					// Sort for detail columns
					if (sortColumns.Count > 0)
					{
						bodyWriter.WriteStartElement("SortExpressions");
						foreach (var sortColumn in sortColumns)
						{
							bodyWriter.WriteStartElement("SortExpression");
							bodyWriter.WriteElementString("Value", sortColumn.SortExpression);
							if (sortColumn.SortDirection == "Descending")
							{
								bodyWriter.WriteElementString("Direction", "Descending");
							}
							bodyWriter.WriteEndElement(); // SortExpression
						}
						bodyWriter.WriteEndElement(); // SortExpressions
					}

					bodyWriter.WriteEndElement(); // TablixMember
				}


                for (var i = 0; i < groupCount; i++)
				{
					// Group Total Value Row
					bodyWriter.WriteStartElement("TablixMember");
					bodyWriter.WriteElementString("KeepWithGroup", "Before");
					bodyWriter.WriteEndElement(); // TablixMember
					bodyWriter.WriteEndElement(); // TablixMembers
					bodyWriter.WriteEndElement(); // TablixMember
				}

				// Grand Total Value Row
				bodyWriter.WriteStartElement("TablixMember");
				bodyWriter.WriteElementString("KeepWithGroup", "Before");
				bodyWriter.WriteEndElement(); // TablixMember

				bodyWriter.WriteEndElement(); // TablixMembers
				bodyWriter.WriteEndElement(); // TablixRowHierarchy

				bodyWriter.WriteElementString("RepeatColumnHeaders", "true");
				bodyWriter.WriteElementString("FixedColumnHeaders", "true");
				bodyWriter.WriteElementString("DataSetName", mainDataSetName);
				bodyWriter.WriteElementString("Top", "2in");

				var tablixHeight = rowCount * 0.25;
				var bodyHeight = tablixHeight + 2.2175;
				bodyWriter.WriteElementString("Height", string.Format("{0}in", tablixHeight));
				bodyWriter.WriteElementString("Width", string.Format("{0}in", tablixWidth));
				bodyWriter.WriteStartElement("Visibility");
				bodyWriter.WriteElementString("Hidden", string.Format("=CountRows(\"{0}\") = 0", mainDataSetName));
				bodyWriter.WriteEndElement(); // Visibility
				bodyWriter.WriteStartElement("Style");
				bodyWriter.WriteStartElement("Border");
				bodyWriter.WriteElementString("Style", "None");
				bodyWriter.WriteEndElement(); // Border
				bodyWriter.WriteEndElement(); // Style

				bodyWriter.WriteEndElement(); //Tablix
				bodyWriter.Flush();
			}
			return XElement.Parse(bodyBuilder.ToString());
		}

		private static void WriteLabelCell(XmlWriter xmlWriter, string label, string colSpan, string styleDataSetName)
		{
			xmlWriter.WriteStartElement("TablixCell");
			xmlWriter.WriteStartElement("CellContents");
			xmlWriter.WriteStartElement("Textbox");
			xmlWriter.WriteAttributeString("Name", GetRandomTextboxName());
			xmlWriter.WriteElementString("DataElementOutput", "NoOutput");
			xmlWriter.WriteElementString("CanGrow", "true");
			xmlWriter.WriteElementString("KeepTogether", "true");
			xmlWriter.WriteStartElement("Paragraphs");
			xmlWriter.WriteStartElement("Paragraph");
			xmlWriter.WriteStartElement("TextRuns");
			xmlWriter.WriteStartElement("TextRun");
			xmlWriter.WriteElementString("Value", label);
			xmlWriter.WriteStartElement("Style");
			xmlWriter.WriteElementString("FontStyle", "Italic");
			xmlWriter.WriteElementString("FontFamily", "Verdana");
			xmlWriter.WriteElementString("FontSize", "7pt");
			xmlWriter.WriteElementString("Color", GetStyleDataColor(styleDataSetName));
			xmlWriter.WriteEndElement(); // Style
			xmlWriter.WriteEndElement(); // TextRun
			xmlWriter.WriteEndElement(); // TextRuns
			xmlWriter.WriteStartElement("Style");
			xmlWriter.WriteElementString("TextAlign", "Left");
			xmlWriter.WriteEndElement(); // Style
			xmlWriter.WriteEndElement(); // Paragraph
			xmlWriter.WriteEndElement(); // Paragraphs
			xmlWriter.WriteStartElement("Style");
			xmlWriter.WriteStartElement("Border");
			xmlWriter.WriteElementString("Color", "LightGrey");
			xmlWriter.WriteElementString("Style", "Solid");
			xmlWriter.WriteEndElement(); // Border
			xmlWriter.WriteElementString("BackgroundColor", GetStyleRowHeaderFillValue(styleDataSetName));
			WritePaddings(xmlWriter, "2pt");
			xmlWriter.WriteEndElement(); // Style
			xmlWriter.WriteEndElement(); // Textbox
			if (!string.IsNullOrEmpty(colSpan))
			{
				xmlWriter.WriteElementString("ColSpan", colSpan.ToString());
			}
			xmlWriter.WriteEndElement(); // CellContents
			xmlWriter.WriteEndElement(); // TablixCell
		}

		private static void WriteDetailCell(XmlWriter xmlWriter, string detailValue, string styleDataSetName, string dataElementName, string format)
		{
			xmlWriter.WriteStartElement("TablixCell");
			xmlWriter.WriteStartElement("CellContents");
			xmlWriter.WriteStartElement("Textbox");
			xmlWriter.WriteAttributeString("Name", GetRandomTextboxName());
			xmlWriter.WriteElementString("DataElementName", dataElementName);
			xmlWriter.WriteElementString("CanGrow", "true");
			xmlWriter.WriteElementString("KeepTogether", "true");
			xmlWriter.WriteStartElement("Paragraphs");
			xmlWriter.WriteStartElement("Paragraph");
			xmlWriter.WriteStartElement("TextRuns");
			xmlWriter.WriteStartElement("TextRun");
			xmlWriter.WriteElementString("Value", detailValue);
			xmlWriter.WriteStartElement("Style");
			xmlWriter.WriteElementString("FontStyle", "Italic");
			xmlWriter.WriteElementString("FontFamily", "Verdana");
			xmlWriter.WriteElementString("FontSize", "8pt");
			xmlWriter.WriteElementString("Color", GetStyleDataColor(styleDataSetName));
			xmlWriter.WriteElementString("Format", format);
			xmlWriter.WriteEndElement(); // Style
			xmlWriter.WriteEndElement(); // TextRun
			xmlWriter.WriteEndElement(); // TextRuns
			xmlWriter.WriteStartElement("Style");
			xmlWriter.WriteElementString("TextAlign", "Left");
			xmlWriter.WriteEndElement(); // Style
			xmlWriter.WriteEndElement(); // Paragraph
			xmlWriter.WriteEndElement(); // Paragraphs
			xmlWriter.WriteStartElement("Style");
			xmlWriter.WriteStartElement("Border");
			xmlWriter.WriteElementString("Color", "LightGrey");
			xmlWriter.WriteElementString("Style", "Solid");
			xmlWriter.WriteEndElement(); // Border
			xmlWriter.WriteElementString("BackgroundColor", GetStyleRowFill(styleDataSetName));
			WritePaddings(xmlWriter, "2pt");
			xmlWriter.WriteEndElement(); // Style
			xmlWriter.WriteEndElement(); // Textbox
			xmlWriter.WriteEndElement(); // CellContents
			xmlWriter.WriteEndElement(); // TablixCell
		}

        private static void WriteCSVDetailCell(XmlWriter xmlWriter, string detailValue, string styleDataSetName, string dataElementName, string format)
        {
            xmlWriter.WriteStartElement("TablixCell");
            xmlWriter.WriteStartElement("CellContents");
            xmlWriter.WriteStartElement("Textbox");
            xmlWriter.WriteAttributeString("Name", GetRandomTextboxName());
            xmlWriter.WriteElementString("DataElementName", dataElementName);
            xmlWriter.WriteElementString("CanGrow", "true");
            xmlWriter.WriteElementString("KeepTogether", "true");
            xmlWriter.WriteStartElement("Paragraphs");
            xmlWriter.WriteStartElement("Paragraph");
            xmlWriter.WriteStartElement("TextRuns");
            xmlWriter.WriteStartElement("TextRun");
            xmlWriter.WriteElementString("Value", detailValue);
            xmlWriter.WriteStartElement("Style");
            xmlWriter.WriteEndElement(); // Style
            xmlWriter.WriteEndElement(); // TextRun
            xmlWriter.WriteEndElement(); // TextRuns
            xmlWriter.WriteStartElement("Style");
            xmlWriter.WriteElementString("TextAlign", "Left");
            xmlWriter.WriteEndElement(); // Style
            xmlWriter.WriteEndElement(); // Paragraph
            xmlWriter.WriteEndElement(); // Paragraphs
            xmlWriter.WriteStartElement("Style");
            xmlWriter.WriteEndElement(); // Style
            xmlWriter.WriteEndElement(); // Textbox
            xmlWriter.WriteEndElement(); // CellContents
            xmlWriter.WriteEndElement(); // TablixCell
        }

        private static void WriteCSVGrandTotalDetailCell(XmlWriter xmlWriter, string detailValue, string styleDataSetName, string dataElementName, string dataElementOutput)
        {
            xmlWriter.WriteStartElement("TablixCell");
            xmlWriter.WriteStartElement("CellContents");
            xmlWriter.WriteStartElement("Textbox");
            xmlWriter.WriteAttributeString("Name", GetRandomTextboxName());
            if (!string.IsNullOrEmpty(dataElementName))
            {
                xmlWriter.WriteElementString("DataElementName", dataElementName);
            }
            xmlWriter.WriteElementString("DataElementOutput", dataElementOutput);
            xmlWriter.WriteElementString("CanGrow", "true");
            xmlWriter.WriteElementString("KeepTogether", "true");
            xmlWriter.WriteStartElement("Paragraphs");
            xmlWriter.WriteStartElement("Paragraph");
            xmlWriter.WriteStartElement("TextRuns");
            xmlWriter.WriteStartElement("TextRun");
            xmlWriter.WriteElementString("Value", detailValue);
            xmlWriter.WriteStartElement("Style");
            xmlWriter.WriteEndElement(); // Style
            xmlWriter.WriteEndElement(); // TextRun
            xmlWriter.WriteEndElement(); // TextRuns
            xmlWriter.WriteStartElement("Style");
            xmlWriter.WriteElementString("TextAlign", "Left");
            xmlWriter.WriteEndElement(); // Style
            xmlWriter.WriteEndElement(); // Paragraph
            xmlWriter.WriteEndElement(); // Paragraphs
            xmlWriter.WriteStartElement("Style");
            xmlWriter.WriteEndElement(); // Style
            xmlWriter.WriteEndElement(); // Textbox
            xmlWriter.WriteEndElement(); // CellContents
            xmlWriter.WriteEndElement(); // TablixCell
        }

		private static void WriteFooterDetailCell(XmlWriter xmlWriter, string detailValue, string styleDataSetName, string dataElementName, string format, string colSpan)
		{
			xmlWriter.WriteStartElement("TablixCell");
			xmlWriter.WriteStartElement("CellContents");
			xmlWriter.WriteStartElement("Textbox");
			xmlWriter.WriteAttributeString("Name", GetRandomTextboxName());
			if (!string.IsNullOrEmpty(dataElementName))
			{
				xmlWriter.WriteElementString("DataElementName", dataElementName);
			}
			xmlWriter.WriteElementString("DataElementOutput", "NoOutput");
			xmlWriter.WriteElementString("CanGrow", "true");
			xmlWriter.WriteElementString("KeepTogether", "true");
			xmlWriter.WriteStartElement("Paragraphs");
			xmlWriter.WriteStartElement("Paragraph");
			xmlWriter.WriteStartElement("TextRuns");
			xmlWriter.WriteStartElement("TextRun");
			xmlWriter.WriteElementString("Value", detailValue);
			xmlWriter.WriteStartElement("Style");
			xmlWriter.WriteElementString("FontFamily", "Verdana");
			xmlWriter.WriteElementString("FontSize", "8pt");
			xmlWriter.WriteElementString("FontWeight", "Bold");
			xmlWriter.WriteElementString("Color", GetStyleDataColor(styleDataSetName));
			if (!string.IsNullOrEmpty(format))
			{
				xmlWriter.WriteElementString("Format", format);
			}
			xmlWriter.WriteEndElement(); // Style
			xmlWriter.WriteEndElement(); // TextRun
			xmlWriter.WriteEndElement(); // TextRuns
			xmlWriter.WriteStartElement("Style");
			xmlWriter.WriteElementString("TextAlign", "Left");
			xmlWriter.WriteEndElement(); // Style
			xmlWriter.WriteEndElement(); // Paragraph
			xmlWriter.WriteEndElement(); // Paragraphs
			xmlWriter.WriteStartElement("Style");
			xmlWriter.WriteStartElement("Border");
			xmlWriter.WriteElementString("Color", "LightGrey");
			xmlWriter.WriteElementString("Style", "Solid");
			xmlWriter.WriteEndElement(); // Border
			xmlWriter.WriteElementString("BackgroundColor", GetStyleGroupBackColor(styleDataSetName));
			WritePaddings(xmlWriter, "2pt");
			xmlWriter.WriteEndElement(); // Style
			xmlWriter.WriteEndElement(); // Textbox
			if (!string.IsNullOrEmpty(colSpan))
			{
				xmlWriter.WriteElementString("ColSpan", colSpan.ToString());
			}
			xmlWriter.WriteEndElement(); // CellContents
			xmlWriter.WriteEndElement(); // TablixCell
		}

		private static void WriteGrandTotalDetailCell(XmlWriter xmlWriter, string detailValue, string styleDataSetName, string dataElementName, string format, string colSpan)
		{
			xmlWriter.WriteStartElement("TablixCell");
			xmlWriter.WriteStartElement("CellContents");
			xmlWriter.WriteStartElement("Textbox");
			xmlWriter.WriteAttributeString("Name", GetRandomTextboxName());
			if (!string.IsNullOrEmpty(dataElementName))
			{
				xmlWriter.WriteElementString("DataElementName", dataElementName);
			}
            xmlWriter.WriteElementString("DataElementOutput", "NoOutput");
			xmlWriter.WriteElementString("CanGrow", "true");
			xmlWriter.WriteElementString("KeepTogether", "true");
			xmlWriter.WriteStartElement("Paragraphs");
			xmlWriter.WriteStartElement("Paragraph");
			xmlWriter.WriteStartElement("TextRuns");
			xmlWriter.WriteStartElement("TextRun");
			xmlWriter.WriteElementString("Value", detailValue);
			xmlWriter.WriteStartElement("Style");
			xmlWriter.WriteElementString("FontFamily", "Verdana");
			xmlWriter.WriteElementString("FontSize", "8pt");
			xmlWriter.WriteElementString("FontWeight", "Bold");
			xmlWriter.WriteElementString("Color", GetStyleDataColor(styleDataSetName));
			if (!string.IsNullOrEmpty(format))
			{
				xmlWriter.WriteElementString("Format", format);
			}
			xmlWriter.WriteEndElement(); // Style
			xmlWriter.WriteEndElement(); // TextRun
			xmlWriter.WriteEndElement(); // TextRuns
			xmlWriter.WriteStartElement("Style");
			xmlWriter.WriteElementString("TextAlign", "Left");
			xmlWriter.WriteEndElement(); // Style
			xmlWriter.WriteEndElement(); // Paragraph
			xmlWriter.WriteEndElement(); // Paragraphs
			xmlWriter.WriteStartElement("Style");
			xmlWriter.WriteStartElement("Border");
			xmlWriter.WriteElementString("Color", "LightGrey");
			xmlWriter.WriteElementString("Style", "Solid");
			xmlWriter.WriteEndElement(); // Border
			xmlWriter.WriteElementString("BackgroundColor", GetStyleGrandTotalBackColor(styleDataSetName));
			WritePaddings(xmlWriter, "2pt");
			xmlWriter.WriteEndElement(); // Style
			xmlWriter.WriteEndElement(); // Textbox
			if (!string.IsNullOrEmpty(colSpan))
			{
                xmlWriter.WriteElementString("ColSpan", colSpan.ToString());
			}
			xmlWriter.WriteEndElement(); // CellContents
			xmlWriter.WriteEndElement(); // TablixCell
		}

		private static void WritePaddings(XmlWriter xmlWriter, string padding)
		{
			xmlWriter.WriteElementString("PaddingLeft", padding);
			xmlWriter.WriteElementString("PaddingRight", padding);
			xmlWriter.WriteElementString("PaddingTop", padding);
			xmlWriter.WriteElementString("PaddingBottom", padding);
		}

		private static void WriteSpacerCells(XmlWriter xmlWriter, int count, string styleDataSetName)
		{
			for (var i = 0; i < count; i++)
			{
				xmlWriter.WriteStartElement("TablixCell");
				xmlWriter.WriteStartElement("CellContents");
				xmlWriter.WriteStartElement("Textbox");
				xmlWriter.WriteAttributeString("Name", GetRandomTextboxName());
				xmlWriter.WriteElementString("CanGrow", "true");
				xmlWriter.WriteElementString("KeepTogether", "true");
				xmlWriter.WriteStartElement("Paragraphs");
				xmlWriter.WriteStartElement("Paragraph");
				xmlWriter.WriteStartElement("TextRuns");
				xmlWriter.WriteStartElement("TextRun");
				xmlWriter.WriteStartElement("Value");
				xmlWriter.WriteEndElement(); // Value
				xmlWriter.WriteStartElement("Style");
				xmlWriter.WriteElementString("Color", GetStyleDataColor(styleDataSetName));
				xmlWriter.WriteEndElement(); // Style
				xmlWriter.WriteEndElement(); // TextRun
				xmlWriter.WriteEndElement(); // TextRuns
				xmlWriter.WriteStartElement("Style");
				xmlWriter.WriteEndElement(); // Style
				xmlWriter.WriteEndElement(); // Paragraph
				xmlWriter.WriteEndElement(); // Paragraphs
				xmlWriter.WriteStartElement("Style");
				xmlWriter.WriteStartElement("Border");
				xmlWriter.WriteElementString("Color", "LightGrey");
				xmlWriter.WriteElementString("Style", "None");
				xmlWriter.WriteEndElement(); // Border
				WritePaddings(xmlWriter, "2pt");
				xmlWriter.WriteEndElement(); // Style
				xmlWriter.WriteEndElement(); // Textbox
				xmlWriter.WriteEndElement(); // CellContents
				xmlWriter.WriteEndElement(); // TablixCell
			}
		}

		private static void WriteHeaderCell(XmlWriter xmlWriter, String label, String value, int colSpan, string styleDataSetName, string dataElementName)
		{
			xmlWriter.WriteStartElement("TablixCell");
			xmlWriter.WriteStartElement("CellContents");
			xmlWriter.WriteStartElement("Textbox");
			xmlWriter.WriteAttributeString("Name", GetRandomTextboxName());
			xmlWriter.WriteElementString("DataElementName", dataElementName);
			xmlWriter.WriteElementString("DataElementOutput", "NoOutput");
			xmlWriter.WriteElementString("CanGrow", "true");
			xmlWriter.WriteElementString("KeepTogether", "true");
			xmlWriter.WriteStartElement("Paragraphs");
			xmlWriter.WriteStartElement("Paragraph");
			xmlWriter.WriteStartElement("TextRuns");
			xmlWriter.WriteStartElement("TextRun");
			xmlWriter.WriteElementString("Value", label);
			xmlWriter.WriteStartElement("Style");
			xmlWriter.WriteElementString("FontFamily", "Verdana");
			xmlWriter.WriteElementString("FontSize", "8pt");
			xmlWriter.WriteElementString("Color", GetStyleDataColor(styleDataSetName));
			xmlWriter.WriteEndElement(); // Style
			xmlWriter.WriteEndElement(); // TextRun
            xmlWriter.WriteStartElement("TextRun");
            xmlWriter.WriteElementString("Value", ":");
            xmlWriter.WriteStartElement("Style");
            xmlWriter.WriteElementString("FontFamily", "Verdana");
            xmlWriter.WriteElementString("FontSize", "8pt");
            xmlWriter.WriteElementString("Color", GetStyleDataColor(styleDataSetName));
            xmlWriter.WriteEndElement(); // Style
            xmlWriter.WriteEndElement(); // TextRun
			xmlWriter.WriteStartElement("TextRun");
			xmlWriter.WriteElementString("Value", value);
			xmlWriter.WriteStartElement("Style");
			xmlWriter.WriteElementString("FontFamily", "Verdana");
			xmlWriter.WriteElementString("FontSize", "8pt");
			xmlWriter.WriteElementString("FontWeight", "Bold");
			xmlWriter.WriteElementString("Color", GetStyleDataColor(styleDataSetName));
			xmlWriter.WriteEndElement(); // Style
			xmlWriter.WriteEndElement(); // TextRun
			xmlWriter.WriteEndElement(); // TextRuns
			xmlWriter.WriteStartElement("Style");
			xmlWriter.WriteEndElement(); // Style
			xmlWriter.WriteEndElement(); // Paragraph
			xmlWriter.WriteEndElement(); // Paragraphs
			xmlWriter.WriteStartElement("Style");
			xmlWriter.WriteStartElement("Border");
			xmlWriter.WriteElementString("Color", "LightGrey");
			xmlWriter.WriteElementString("Style", "Solid");
			xmlWriter.WriteEndElement(); // Border
			xmlWriter.WriteElementString("BackgroundColor", GetStyleGroupBackColor(styleDataSetName));
			WritePaddings(xmlWriter, "2pt");
			xmlWriter.WriteEndElement(); // Style
			xmlWriter.WriteEndElement(); // Textbox
			xmlWriter.WriteElementString("ColSpan", colSpan.ToString());
			xmlWriter.WriteEndElement(); // CellContents
			xmlWriter.WriteEndElement(); // TablixCell
		}

        private static void WriteCSVHeaderCell(XmlWriter xmlWriter, String label, String value, int colSpan, string styleDataSetName, string dataElementName, string dataElementOutput)
        {
            xmlWriter.WriteStartElement("TablixCell");
            xmlWriter.WriteStartElement("CellContents");
            xmlWriter.WriteStartElement("Textbox");
            xmlWriter.WriteAttributeString("Name", GetRandomTextboxName());
            xmlWriter.WriteElementString("DataElementName", dataElementName);
            xmlWriter.WriteElementString("DataElementOutput", dataElementOutput);
            xmlWriter.WriteElementString("CanGrow", "true");
            xmlWriter.WriteElementString("KeepTogether", "true");
            xmlWriter.WriteStartElement("Paragraphs");
            xmlWriter.WriteStartElement("Paragraph");
            xmlWriter.WriteStartElement("TextRuns");
            xmlWriter.WriteStartElement("TextRun");
            xmlWriter.WriteElementString("Value", value);
            xmlWriter.WriteStartElement("Style");
            xmlWriter.WriteEndElement(); // Style
            xmlWriter.WriteEndElement(); // TextRun
            xmlWriter.WriteEndElement(); // TextRuns
            xmlWriter.WriteStartElement("Style");
            xmlWriter.WriteElementString("TextAlign", "Right");
            xmlWriter.WriteEndElement(); // Style
            xmlWriter.WriteEndElement(); // Paragraph
            xmlWriter.WriteEndElement(); // Paragraphs
            xmlWriter.WriteEndElement(); // Textbox
            xmlWriter.WriteEndElement(); // CellContents
            xmlWriter.WriteEndElement(); // TablixCell
        }

		private static void WriteFooterLabelCell(XmlWriter xmlWriter, String value, int colSpan, string styleDataSetName, string dataElementName)
		{
			xmlWriter.WriteStartElement("TablixCell");
			xmlWriter.WriteStartElement("CellContents");
			xmlWriter.WriteStartElement("Textbox");
			xmlWriter.WriteAttributeString("Name", GetRandomTextboxName());
			xmlWriter.WriteElementString("DataElementName", dataElementName);
			xmlWriter.WriteElementString("DataElementOutput", "NoOutput");
			xmlWriter.WriteElementString("CanGrow", "true");
			xmlWriter.WriteElementString("KeepTogether", "true");
			xmlWriter.WriteStartElement("Paragraphs");
			xmlWriter.WriteStartElement("Paragraph");
			xmlWriter.WriteStartElement("TextRuns");
			xmlWriter.WriteStartElement("TextRun");
			xmlWriter.WriteElementString("Value", "Totals For ");
			xmlWriter.WriteStartElement("Style");
			xmlWriter.WriteElementString("FontFamily", "Verdana");
			xmlWriter.WriteElementString("FontSize", "8pt");
			xmlWriter.WriteElementString("Color", GetStyleDataColor(styleDataSetName));
			xmlWriter.WriteEndElement(); // Style
			xmlWriter.WriteEndElement(); // TextRun
			xmlWriter.WriteStartElement("TextRun");
			xmlWriter.WriteElementString("Value", value);
			xmlWriter.WriteStartElement("Style");
			xmlWriter.WriteElementString("FontFamily", "Verdana");
			xmlWriter.WriteElementString("FontSize", "8pt");
			xmlWriter.WriteElementString("FontWeight", "Bold");
			xmlWriter.WriteElementString("Color", GetStyleDataColor(styleDataSetName));
			xmlWriter.WriteEndElement(); // Style
			xmlWriter.WriteEndElement(); // TextRun
			xmlWriter.WriteEndElement(); // TextRuns
			xmlWriter.WriteStartElement("Style");
			xmlWriter.WriteEndElement(); // Style
			xmlWriter.WriteEndElement(); // Paragraph
			xmlWriter.WriteEndElement(); // Paragraphs
			xmlWriter.WriteStartElement("Style");
			xmlWriter.WriteStartElement("Border");
			xmlWriter.WriteElementString("Color", "LightGrey");
			xmlWriter.WriteElementString("Style", "Solid");
			xmlWriter.WriteEndElement(); // Border
			xmlWriter.WriteElementString("BackgroundColor", GetStyleGroupBackColor(styleDataSetName));
			WritePaddings(xmlWriter, "2pt");
			xmlWriter.WriteEndElement(); // Style
			xmlWriter.WriteEndElement(); // Textbox
			if (colSpan > 1)
			{
				xmlWriter.WriteElementString("ColSpan", colSpan.ToString());
			}
			xmlWriter.WriteEndElement(); // CellContents
			xmlWriter.WriteEndElement(); // TablixCell
		}

		private static void WriteGrandTotalLabelCell(XmlWriter xmlWriter, String value, int colSpan, string styleDataSetName)
		{
			xmlWriter.WriteStartElement("TablixCell");
			xmlWriter.WriteStartElement("CellContents");
			xmlWriter.WriteStartElement("Textbox");
			xmlWriter.WriteAttributeString("Name", GetRandomTextboxName());
			xmlWriter.WriteElementString("CanGrow", "true");
			xmlWriter.WriteElementString("KeepTogether", "true");
			xmlWriter.WriteElementString("DataElementOutput", "NoOutput");
			xmlWriter.WriteStartElement("Paragraphs");
			xmlWriter.WriteStartElement("Paragraph");
			xmlWriter.WriteStartElement("TextRuns");
			xmlWriter.WriteStartElement("TextRun");
			xmlWriter.WriteElementString("Value", value);
			xmlWriter.WriteStartElement("Style");
			xmlWriter.WriteElementString("FontFamily", "Verdana");
			xmlWriter.WriteElementString("FontSize", "8pt");
			xmlWriter.WriteElementString("FontWeight", "Bold");
			xmlWriter.WriteElementString("Color", GetStyleDataColor(styleDataSetName));
			xmlWriter.WriteEndElement(); // Style
			xmlWriter.WriteEndElement(); // TextRun
			xmlWriter.WriteEndElement(); // TextRuns
			xmlWriter.WriteStartElement("Style");
			xmlWriter.WriteEndElement(); // Style
			xmlWriter.WriteEndElement(); // Paragraph
			xmlWriter.WriteEndElement(); // Paragraphs
			xmlWriter.WriteStartElement("Style");
			xmlWriter.WriteStartElement("Border");
			xmlWriter.WriteElementString("Color", "LightGrey");
			xmlWriter.WriteElementString("Style", "Solid");
			xmlWriter.WriteEndElement(); // Border
			xmlWriter.WriteElementString("BackgroundColor", GetStyleGrandTotalBackColor(styleDataSetName));
			WritePaddings(xmlWriter, "2pt");
			xmlWriter.WriteEndElement(); // Style
			xmlWriter.WriteEndElement(); // Textbox
			xmlWriter.WriteElementString("ColSpan", colSpan.ToString());
			xmlWriter.WriteEndElement(); // CellContents
			xmlWriter.WriteEndElement(); // TablixCell
		}
	}
}

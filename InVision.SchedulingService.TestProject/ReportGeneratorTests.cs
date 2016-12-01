using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Emdat.InVision.Generator;
using System.Xml.Linq;
using System.Xml.XPath;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace InVision.SchedulingService.TestProject
{
	[TestClass]
	public class ReportGeneratorTests
	{
		private TestContext testContextInstance;
		public TestContext TestContext
		{
			get { return testContextInstance; }
			set { testContextInstance = value; }
		}


		[TestMethod]
		[DeploymentItem("data")]
		[DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\ReportGeneratorTests.csv", "ReportGeneratorTests#csv", DataAccessMethod.Sequential)]
		public void get_placeholder_elements_from_rdl_template()
		{
			string filename = TestContext.DataRow["filename"] as string;
			XElement rdlTemplate = XElement.Load(filename);

			var tablix = ReportGenerator.GetPlaceHolderTablixElement(rdlTemplate);
			Assert.IsNotNull(tablix);

			var page = ReportGenerator.GetPageElement(rdlTemplate);
			Assert.IsNotNull(page);

			var bodyWidth = ReportGenerator.GetBodyWidthElement(rdlTemplate);
			Assert.IsNotNull(bodyWidth);

			var parametersTextBox = ReportGenerator.GetParametersTextBoxElement(rdlTemplate);
			Assert.IsNotNull(parametersTextBox);

			var timezoneTextBox = ReportGenerator.GetTimeZoneNoteTextBoxElement(rdlTemplate);
			Assert.IsNotNull(timezoneTextBox);

			var reportNameTextBox = ReportGenerator.GetReportNameTexBoxElement(rdlTemplate);
			Assert.IsNotNull(reportNameTextBox);

			var pageNumberTextBox = ReportGenerator.GetPageNumberTextBoxElement(rdlTemplate);
			Assert.IsNotNull(pageNumberTextBox);
		}

		[TestMethod]
		[DeploymentItem("data\\generate_rdl_with_no_grouping_generates_valid_rdl.options.xml")]
		[DeploymentItem("data\\generate_rdl_with_no_grouping_generates_valid_rdl.template.xml")]
		[DeploymentItem("schemas")]
		public void generate_rdl_with_no_grouping_generates_valid_rdl()
		{
			XElement rdlTemplate = XElement.Load("generate_rdl_with_no_grouping_generates_valid_rdl.template.xml");
			XElement options = XElement.Load("generate_rdl_with_no_grouping_generates_valid_rdl.options.xml");
			XElement reportColumns = options.Element("ReportColumns");

			var rdlBytes = ReportGenerator.GenerateReportDefinition(rdlTemplate, reportColumns, false, "EXCEL");
			AssertRdlIsValid(rdlBytes);

			var csvRdlBytes = ReportGenerator.GenerateReportDefinition(rdlTemplate, reportColumns, false, "CSV");
			AssertRdlIsValid(csvRdlBytes);
		}

		private static void AssertRdlIsValid(byte[] rdl)
		{
			//execute the report using the new RDL
			using (var ms = new MemoryStream(rdl))
			{
				XDocument rdlDocument = XDocument.Load(ms);

				var xmlns = rdlDocument.Root.GetDefaultNamespace();
				var schemaFileName = xmlns.NamespaceName.Replace('/', '_').Replace(':', '_') + ".xsd";
				Assert.IsTrue(File.Exists(schemaFileName), $"Schema file does not exist: {schemaFileName}");
				using (var schemaReader = XmlReader.Create(schemaFileName))
				{
					XmlSchema schema = XmlSchema.Read(schemaReader, (s,e) => { });
					XmlSchemaSet schemaSet = new XmlSchemaSet();
					schemaSet.Add(schema);
					rdlDocument.Validate(schemaSet, (s, e) =>
					{
						Assert.Fail($"Schema is not valid: {e.Message}");
					});					
				}
			}
		}
	}
}

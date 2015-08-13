using InVision.SchedulingService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Emdat.InVision.SSRSExecution;
using System;
using System.IO;
using System.Diagnostics;
using Emdat.InVision;
using Emdat;
using System.Linq;
namespace InVision.SchedulingService.TestProject
{


    /// <summary>
    ///This is a test class for ReportExecutionTest and is intended
    ///to contain all ReportExecutionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ReportExecutionTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for Execute
        ///</summary>
        [TestMethod()]
        public void ExecuteTestInValidParameter()
        {
            ReportExecution target = new ReportExecution();
            target.ReportPath = "/Inquiry Reports/Client Billing Detail Report";
            target.Parameters = new List<ParameterValue>(new ParameterValue[]
            {
                new ParameterValue
                {
                    Name = "Client_ID",
                    Value = "325"
                },
                new ParameterValue
                {
                    Name = "User_ID",
                    Value = "6404"
                },
                new ParameterValue
                {
                    Name = "Start_Date",
                    Value = "1/1/2007"
                },
                new ParameterValue
                {
                    Name = "End_Date",
                    Value = "12/31/2007"
                },
                new ParameterValue
                {
                    Name = "Location_ID",
                    Value = null
                },
                new ParameterValue
                {
                    Name = "Document_ID",
                    Value = null
                },
                new ParameterValue
                {
                    Name = "SortType",
                    Value = "0" //this is an invalid value
                },
                new ParameterValue
                {
                    Name = "DetailShow",
                    Value = "True"
                }
            });
            target.Format = "CSV";
            target.DeviceInfo = null;
            target.Execute(null, TimeSpan.FromMinutes(2));
            Assert.IsTrue(target.Data == null);
            Assert.IsTrue(target.Error is InvalidOperationException);
        }

        /// <summary>
        ///A test for Execute
        ///</summary>
        [TestMethod()]
        public void ExecuteCSVTest()
        {
            ReportExecution target = new ReportExecution();
            target.ReportPath = "/Inquiry Reports/Client Billing Detail Report";
            target.State = ReportExecutionStateEnum.Queued;
            target.Parameters = new List<ParameterValue>(new ParameterValue[]
            {
                new ParameterValue
                {
                    Name = "Client_ID",
                    Value = "325"
                },
                new ParameterValue
                {
                    Name = "User_ID",
                    Value = "6404"
                },
                new ParameterValue
                {
                    Name = "Start_Date",
                    Value = "1/1/2007"
                },
                new ParameterValue
                {
                    Name = "End_Date",
                    Value = "12/31/2007"
                },
                new ParameterValue
                {
                    Name = "Location_ID",
                    Value = null
                },
                new ParameterValue
                {
                    Name = "Document_ID",
                    Value = null
                },
                new ParameterValue
                {
                    Name = "SortType",
                    Value = "1"
                },
                new ParameterValue
                {
                    Name = "DetailShow",
                    Value = "True"
                }
            });
            target.Format = "CSV";
            target.DeviceInfo = null;
            target.Execute(null, TimeSpan.FromMinutes(2));
            Assert.IsTrue(target.Data != null);
            Assert.IsTrue(target.Data.Length > 0);            
        }

        /// <summary>
        ///A test for Execute
        ///</summary>
        [TestMethod()]
        public void ExecuteHTMLTest()
        {
            ReportExecution target = new ReportExecution();
            target.ReportPath = "/Inquiry Reports/Client Billing Detail Report";
            target.State = ReportExecutionStateEnum.Queued;
            target.Parameters = new List<ParameterValue>(new ParameterValue[]
            {
                new ParameterValue
                {
                    Name = "Client_ID",
                    Value = "325"
                },
                new ParameterValue
                {
                    Name = "User_ID",
                    Value = "6404"
                },
                new ParameterValue
                {
                    Name = "Start_Date",
                    Value = "3/1/2007"
                },
                new ParameterValue
                {
                    Name = "End_Date",
                    Value = "5/30/2007"
                },
                new ParameterValue
                {
                    Name = "Location_ID",
                    Value = null
                },
                new ParameterValue
                {
                    Name = "Document_ID",
                    Value = null
                },
                new ParameterValue
                {
                    Name = "SortType",
                    Value = "1"
                },
                new ParameterValue
                {
                    Name = "DetailShow",
                    Value = "True"
                }
            });
            target.Format = "HTML4.0";
            target.DeviceInfo = "<DeviceInfo><Toolbar>false</Toolbar><Section>0</Section><ExpandContent>true</ExpandContent><HTMLFragment>true</HTMLFragment><OnlyVisibleStyles>false</OnlyVisibleStyles><StyleStream>true</StyleStream></DeviceInfo>";
            target.Execute(null, TimeSpan.FromMinutes(2));
            Assert.IsTrue(target.Data != null);
            Assert.IsTrue(target.Data.Length > 0);
            File.WriteAllBytes(@"C:\temp\reports\htmltest.html", target.Data);
        }

        /// <summary>
        /// GetNextRunDateTimeZoneTest was used to repro and fix a bug in the Next Run Date calculation (TFS WI #2885)
        /// </summary>        
        [TestMethod]        
        public void GetNextRunDateEasternStandardTimeZoneTest()
        {
            DateTime scheduledStartDate  = new DateTime(2010, 1, 31, 23, 0, 0); 
            string baseTranCoTimeZone = "Eastern Standard Time";
            string serverTimeZone = "Central Standard Time";
            Schedule sched = new Schedule(
                2, //weekly
                4, //Monday
                1, //every 1 week
                null,
                (DateTime?)TimeZoneInfoExtension.SafeConvertTimeBySystemTimeZoneId(scheduledStartDate, serverTimeZone, baseTranCoTimeZone),
                "000000",
                "000000");
            DateTime? nextRun = sched.GetNextRunDate(TimeZoneInfoExtension.SafeConvertTimeBySystemTimeZoneId(scheduledStartDate, serverTimeZone, baseTranCoTimeZone));            
            DateTime? expectedNextRun = new DateTime(2010, 2, 8, 0, 0, 0);
            Assert.AreEqual(expectedNextRun.Value, nextRun.Value);            
        }

        /// <summary>
        /// GetNextRunDateTimeZoneTest was used to repro and fix a bug in the Next Run Date calculation (TFS WI #2885)
        /// </summary>        
        [TestMethod]
        public void GetNextRunDatePacificStandardTimeZoneTest()
        {
            DateTime scheduledStartDate = new DateTime(2010, 2, 1, 2, 0, 0);
            string baseTranCoTimeZone = "Pacific Standard Time";
            string serverTimeZone = "Central Standard Time";
            Schedule sched = new Schedule(
                2, //weekly
                4, //Monday
                1, //every 1 week
                null,
                (DateTime?)TimeZoneInfoExtension.SafeConvertTimeBySystemTimeZoneId(scheduledStartDate, serverTimeZone, baseTranCoTimeZone),
                "000000",
                "000000");
            DateTime? nextRun = sched.GetNextRunDate(TimeZoneInfoExtension.SafeConvertTimeBySystemTimeZoneId(scheduledStartDate, serverTimeZone, baseTranCoTimeZone));
            DateTime? expectedNextRun = new DateTime(2010, 2, 8, 0, 0, 0);
            Assert.AreEqual(expectedNextRun.Value, nextRun.Value);
        }

        /// <summary>
        /// TFS #3449: Date field variables using incorrect time zone (from InTrack #42499)
        ///</summary>
        [TestMethod()]
        public void GetReportExecutionFromRow_EasternStandardTimeTest()
        {
            string destTimeZone = "Eastern Standard Time";
            GetExecutionFromQueueRow execRow = new GetExecutionFromQueueRow
            {
                EndDate = null,
                ErrorCount = 0,
                ErrorDescription = null,
                ModifiedDate = new DateTime(2010, 4, 20, 0, 0, 0),
                ModifiedUser = "mock",
                Name = "Client Clinician 16 thru last day",
                Parameters = "<Parameters><ParameterValue><Name>Company_ID</Name><Value>250</Value></ParameterValue><ParameterValue><Name>User_ID</Name><Value>11959</Value></ParameterValue><ParameterValue><Name>Locale_ID</Name><Value>en-US</Value></ParameterValue><ParameterValue><Name>Client_ID</Name></ParameterValue><ParameterValue><Name>Start_Date</Name><Value>2010-04-16</Value></ParameterValue><ParameterValue><Name>End_Date</Name><Value>2010-04-30</Value></ParameterValue><ParameterValue><Name>SortAndGroupOption</Name><Value>0</Value></ParameterValue><ParameterValue><Name>HideDetail</Name><Value>True</Value></ParameterValue></Parameters>",
                ReportExecutionErrorID = null,
                ReportExecutionID = 13431,
                ReportExecutionStatusID = 4,
                ReportFormatID = 2,
                ReportID = 138,
                ReportSubscriptionID = 3085,
                ScheduledStartDate = new DateTime(2010, 4, 30, 23, 0, 0),
                StartDate = null,
                UsedHistory = false
            };
            GetFormatRow formatRow = new GetFormatRow
            {
                Description = null,
                Extension = "CSV",
                HttpContentType = "text/csv",
                IsActive = true,
                Name = "CSV",
                ReportFormatID = 2,
                SSRSDeviceInfo = null,
                SSRSFormat = "CSV"
            };
            GetReportRow reportRow = new GetReportRow
            {
                CategoryID = 13,
                CategoryName = "Production Reports",
                CategorySortOrder = 14,
                Description = "Report of line production for Clients that are directly contracted with you. This report will NOT list outsourced work done by your company.",
                Name = "Client Production Report",
                ReportID = 138,
                ReportParametersUrl = null,
                ReportPath = "/Production/InCommand Reports/Client Production Report",
                SortOrder = 2
            };
            GetSubscriptionRow subscriptionRow = new GetSubscriptionRow
            {
                ReportSubscriptionID = 3085,
                ReportID = 138,
                Name = "Client Clinician 16 thru last day",
                IsActive = true,
                ReportFormatID = 2,
                Parameters = "<Parameters><ParameterValue><Name>Company_ID</Name><Value>250</Value></ParameterValue><ParameterValue><Name>User_ID</Name><Value>11959</Value></ParameterValue><ParameterValue><Name>Locale_ID</Name><Value>en-US</Value></ParameterValue><ParameterValue><Name>Client_ID</Name></ParameterValue><ParameterValue><Name>Start_Date</Name><Value>@FirstOfPreviousMonth+15</Value></ParameterValue><ParameterValue><Name>End_Date</Name><Value>@LastOfPreviousMonth</Value></ParameterValue><ParameterValue><Name>SortAndGroupOption</Name><Value>0</Value></ParameterValue><ParameterValue><Name>HideDetail</Name><Value>True</Value></ParameterValue></Parameters>",
                ModifiedUser = "djacks",
                ModifiedDate = new DateTime(2010, 5, 4, 8, 27, 20),
                ScheduleFrequencyID = 3,
                FrequencyInterval = 0,
                FrequencyRecurrenceFactor = 1,
                FrequencyRelativeInterval = "1",
                StartTime = "000000",
                EndTime = "000000"
            };
            ReportExecution expected = new ReportExecution
            {
                FileType = "CSV",
                Format = "CSV",
                FormatId = 2,
                Id = "13431",
                Name = "Client Clinician 16 thru last day",
                NextScheduledRunParameters = new List<Emdat.InVision.SSRS.ParameterValue>(new Emdat.InVision.SSRS.ParameterValue[]
                {
                    new Emdat.InVision.SSRS.ParameterValue
                    {
                        Name = "Company_ID",
                        Value= "250"
                    },
                    new Emdat.InVision.SSRS.ParameterValue
                    {
                        Name = "User_ID",
                        Value= "11959"
                    },
                    new Emdat.InVision.SSRS.ParameterValue
                    {
                        Name = "Locale_ID",
                        Value= "en-US"
                    },
                    new Emdat.InVision.SSRS.ParameterValue
                    {
                        Name = "Client_ID",
                        Value= null
                    },
                    new Emdat.InVision.SSRS.ParameterValue
                    {
                        Name = "Start_Date",
                        Value= "2010-05-16"
                    },
                    new Emdat.InVision.SSRS.ParameterValue
                    {
                        Name = "End_Date",
                        Value= "2010-05-31"
                    },
                    new Emdat.InVision.SSRS.ParameterValue
                    {
                        Name = "SortAndGroupOption",
                        Value= "0"
                    },
                    new Emdat.InVision.SSRS.ParameterValue
                    {
                        Name = "HideDetail",
                        Value= "True"
                    },
                }),
                NextScheduledRunTime = new DateTime(2010, 5, 31, 23, 0, 0),
                Parameters = new List<ParameterValue>(new ParameterValue[]
                {
                    new ParameterValue
                    {
                        Name = "Company_ID",
                        Value= "250"
                    },
                    new ParameterValue
                    {
                        Name = "User_ID",
                        Value= "11959"
                    },
                    new ParameterValue
                    {
                        Name = "Locale_ID",
                        Value= "en-US"
                    },
                    new ParameterValue
                    {
                        Name = "Client_ID",
                        Value= null
                    },
                    new ParameterValue
                    {
                        Name = "Start_Date",
                        Value= "2010-04-16"
                    },
                    new ParameterValue
                    {
                        Name = "End_Date",
                        Value= "2010-04-30"
                    },
                    new ParameterValue
                    {
                        Name = "SortAndGroupOption",
                        Value= "0"
                    },
                    new ParameterValue
                    {
                        Name = "HideDetail",
                        Value= "True"
                    }
                }),
                ReportId = 138,
                ReportPath = "/Production/InCommand Reports/Client Production Report",
                ScheduledRunTime = new DateTime(2010, 4, 30, 23, 0, 0),
                State = ReportExecutionStateEnum.Queued,
                SubscriptionId = 3085
            };
            ReportExecution actual;
            actual = ReportExecution.GetReportExecutionFromRow(execRow, formatRow, reportRow, subscriptionRow, destTimeZone);
                        
            Assert.AreEqual(expected.DeviceInfo, actual.DeviceInfo);
            Assert.AreEqual(expected.FileType, actual.FileType);
            Assert.AreEqual(expected.Format, actual.Format);
            Assert.AreEqual(expected.FormatId, actual.FormatId);
            Assert.AreEqual(expected.Name, actual.Name);
            var nextRunPrmsAssertions =
                from e in expected.NextScheduledRunParameters
                join a in actual.NextScheduledRunParameters on e.Name equals a.Name
                select new { Expected = e, Actual = a };
            Assert.AreEqual(expected.NextScheduledRunParameters.Count, nextRunPrmsAssertions.Count());
            foreach(var assertion in nextRunPrmsAssertions)
            {
                Assert.AreEqual(assertion.Expected.Name, assertion.Actual.Name);
                Assert.AreEqual(assertion.Expected.Label, assertion.Actual.Label);
                Assert.AreEqual(assertion.Expected.Value, assertion.Actual.Value);
            }
            Assert.AreEqual(expected.NextScheduledRunTime, actual.NextScheduledRunTime);
            Assert.AreEqual(expected.ParameterLanguage, actual.ParameterLanguage);
            var prmAssertions =
                from e in expected.Parameters
                join a in actual.Parameters on e.Name equals a.Name
                select new { Expected = e, Actual = a };
            Assert.AreEqual(expected.Parameters.Count, prmAssertions.Count());
            foreach (var assertion in prmAssertions)
            {
                Assert.AreEqual(assertion.Expected.Name, assertion.Actual.Name);
                Assert.AreEqual(assertion.Expected.Label, assertion.Actual.Label);
                Assert.AreEqual(assertion.Expected.Value, assertion.Actual.Value);
            }
            Assert.AreEqual(expected.ReportId, actual.ReportId);
            Assert.AreEqual(expected.ReportPath, actual.ReportPath);
            Assert.AreEqual(expected.ScheduledRunTime, actual.ScheduledRunTime);
            Assert.AreEqual(expected.SubscriptionId, actual.SubscriptionId);            
        }
    }
}
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
using Emdat.InVision.Sql;
using InVisionMvc.Infrastructure;

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
                (DateTime?)TimeZoneInfoExtensions.SafeConvertTimeBySystemTimeZoneId(scheduledStartDate, serverTimeZone, baseTranCoTimeZone),
                "000000",
                "000000");
            DateTime? nextRun = sched.GetNextRunDate(TimeZoneInfoExtensions.SafeConvertTimeBySystemTimeZoneId(scheduledStartDate, serverTimeZone, baseTranCoTimeZone));            
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
                (DateTime?)TimeZoneInfoExtensions.SafeConvertTimeBySystemTimeZoneId(scheduledStartDate, serverTimeZone, baseTranCoTimeZone),
                "000000",
                "000000");
            DateTime? nextRun = sched.GetNextRunDate(TimeZoneInfoExtensions.SafeConvertTimeBySystemTimeZoneId(scheduledStartDate, serverTimeZone, baseTranCoTimeZone));
            DateTime? expectedNextRun = new DateTime(2010, 2, 8, 0, 0, 0);
            Assert.AreEqual(expectedNextRun.Value, nextRun.Value);
        }        
    }
}
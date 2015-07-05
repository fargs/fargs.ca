using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using Microsoft.Data.Tools.Schema.Sql.UnitTesting;
using Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Orvosi.Test
{
    [TestClass()]
    public class AccountManagement : SqlDatabaseTestClass
    {

        public AccountManagement()
        {
            InitializeComponent();
        }

        [TestInitialize()]
        public void TestInitialize()
        {
            base.InitializeTest();
        }
        [TestCleanup()]
        public void TestCleanup()
        {
            base.CleanupTest();
        }

        [TestMethod()]
        public void TimeFrameInWeeks()
        {
            SqlDatabaseTestActions testActions = this.TimeFrameInWeeksData;
            // Execute the pre-test script
            // 
            System.Diagnostics.Trace.WriteLineIf((testActions.PretestAction != null), "Executing pre-test script...");
            SqlExecutionResult[] pretestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PretestAction);
            // Execute the test script
            // 
            System.Diagnostics.Trace.WriteLineIf((testActions.TestAction != null), "Executing test script...");
            SqlExecutionResult[] testResults = TestService.Execute(this.ExecutionContext, this.PrivilegedContext, testActions.TestAction);
            // Execute the post-test script
            // 
            System.Diagnostics.Trace.WriteLineIf((testActions.PosttestAction != null), "Executing post-test script...");
            SqlExecutionResult[] posttestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PosttestAction);
        }
        [TestMethod()]
        public void TimeframeInDays()
        {
            SqlDatabaseTestActions testActions = this.TimeframeInDaysData;
            // Execute the pre-test script
            // 
            System.Diagnostics.Trace.WriteLineIf((testActions.PretestAction != null), "Executing pre-test script...");
            SqlExecutionResult[] pretestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PretestAction);
            try
            {
                // Execute the test script
                // 
                System.Diagnostics.Trace.WriteLineIf((testActions.TestAction != null), "Executing test script...");
                SqlExecutionResult[] testResults = TestService.Execute(this.ExecutionContext, this.PrivilegedContext, testActions.TestAction);
            }
            finally
            {
                // Execute the post-test script
                // 
                System.Diagnostics.Trace.WriteLineIf((testActions.PosttestAction != null), "Executing post-test script...");
                SqlExecutionResult[] posttestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PosttestAction);
            }
        }
        [TestMethod()]
        public void TimeframeInMonths()
        {
            SqlDatabaseTestActions testActions = this.TimeframeInMonthsData;
            // Execute the pre-test script
            // 
            System.Diagnostics.Trace.WriteLineIf((testActions.PretestAction != null), "Executing pre-test script...");
            SqlExecutionResult[] pretestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PretestAction);
            try
            {
                // Execute the test script
                // 
                System.Diagnostics.Trace.WriteLineIf((testActions.TestAction != null), "Executing test script...");
                SqlExecutionResult[] testResults = TestService.Execute(this.ExecutionContext, this.PrivilegedContext, testActions.TestAction);
            }
            finally
            {
                // Execute the post-test script
                // 
                System.Diagnostics.Trace.WriteLineIf((testActions.PosttestAction != null), "Executing post-test script...");
                SqlExecutionResult[] posttestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PosttestAction);
            }
        }



        #region Designer support code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction TimeFrameInWeeks_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ExpectedSchemaCondition expectedSchemaCondition1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AccountManagement));
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition rowCountCondition1;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction TimeframeInDays_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction TimeframeInMonths_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ExpectedSchemaCondition expectedSchemaCondition2;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition rowCountCondition2;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ExpectedSchemaCondition expectedSchemaCondition3;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition rowCountCondition3;
            this.TimeFrameInWeeksData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            this.TimeframeInDaysData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            this.TimeframeInMonthsData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            TimeFrameInWeeks_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            expectedSchemaCondition1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ExpectedSchemaCondition();
            rowCountCondition1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            TimeframeInDays_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            TimeframeInMonths_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            expectedSchemaCondition2 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ExpectedSchemaCondition();
            rowCountCondition2 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            expectedSchemaCondition3 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ExpectedSchemaCondition();
            rowCountCondition3 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.RowCountCondition();
            // 
            // TimeFrameInWeeks_TestAction
            // 
            TimeFrameInWeeks_TestAction.Conditions.Add(expectedSchemaCondition1);
            TimeFrameInWeeks_TestAction.Conditions.Add(rowCountCondition1);
            resources.ApplyResources(TimeFrameInWeeks_TestAction, "TimeFrameInWeeks_TestAction");
            // 
            // TimeFrameInWeeksData
            // 
            this.TimeFrameInWeeksData.PosttestAction = null;
            this.TimeFrameInWeeksData.PretestAction = null;
            this.TimeFrameInWeeksData.TestAction = TimeFrameInWeeks_TestAction;
            // 
            // expectedSchemaCondition1
            // 
            expectedSchemaCondition1.Enabled = true;
            expectedSchemaCondition1.Name = "expectedSchemaCondition1";
            resources.ApplyResources(expectedSchemaCondition1, "expectedSchemaCondition1");
            expectedSchemaCondition1.Verbose = true;
            // 
            // rowCountCondition1
            // 
            rowCountCondition1.Enabled = true;
            rowCountCondition1.Name = "rowCountCondition1";
            rowCountCondition1.ResultSet = 1;
            rowCountCondition1.RowCount = 131;
            // 
            // TimeframeInDaysData
            // 
            this.TimeframeInDaysData.PosttestAction = null;
            this.TimeframeInDaysData.PretestAction = null;
            this.TimeframeInDaysData.TestAction = TimeframeInDays_TestAction;
            // 
            // TimeframeInDays_TestAction
            // 
            TimeframeInDays_TestAction.Conditions.Add(expectedSchemaCondition2);
            TimeframeInDays_TestAction.Conditions.Add(rowCountCondition2);
            resources.ApplyResources(TimeframeInDays_TestAction, "TimeframeInDays_TestAction");
            // 
            // TimeframeInMonthsData
            // 
            this.TimeframeInMonthsData.PosttestAction = null;
            this.TimeframeInMonthsData.PretestAction = null;
            this.TimeframeInMonthsData.TestAction = TimeframeInMonths_TestAction;
            // 
            // TimeframeInMonths_TestAction
            // 
            TimeframeInMonths_TestAction.Conditions.Add(expectedSchemaCondition3);
            TimeframeInMonths_TestAction.Conditions.Add(rowCountCondition3);
            resources.ApplyResources(TimeframeInMonths_TestAction, "TimeframeInMonths_TestAction");
            // 
            // expectedSchemaCondition2
            // 
            expectedSchemaCondition2.Enabled = true;
            expectedSchemaCondition2.Name = "expectedSchemaCondition2";
            resources.ApplyResources(expectedSchemaCondition2, "expectedSchemaCondition2");
            expectedSchemaCondition2.Verbose = true;
            // 
            // rowCountCondition2
            // 
            rowCountCondition2.Enabled = true;
            rowCountCondition2.Name = "rowCountCondition2";
            rowCountCondition2.ResultSet = 1;
            rowCountCondition2.RowCount = 915;
            // 
            // expectedSchemaCondition3
            // 
            expectedSchemaCondition3.Enabled = true;
            expectedSchemaCondition3.Name = "expectedSchemaCondition3";
            resources.ApplyResources(expectedSchemaCondition3, "expectedSchemaCondition3");
            expectedSchemaCondition3.Verbose = true;
            // 
            // rowCountCondition3
            // 
            rowCountCondition3.Enabled = true;
            rowCountCondition3.Name = "rowCountCondition3";
            rowCountCondition3.ResultSet = 1;
            rowCountCondition3.RowCount = 30;
        }

        #endregion


        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        #endregion

        private SqlDatabaseTestActions TimeFrameInWeeksData;
        private SqlDatabaseTestActions TimeframeInDaysData;
        private SqlDatabaseTestActions TimeframeInMonthsData;
    }
}

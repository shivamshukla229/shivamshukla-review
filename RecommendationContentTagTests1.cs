using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Selenium2;
using OpenQA.Selenium;
using NUnit.Framework;
using Selenium2.Meridian;
using System.Threading;
using TestAutomation.Meridian.Regression_Objects;
using NUnit.Framework.Interfaces;
using AventStack.ExtentReports;
using Selenium2.Meridian.P1.MyResponsibilities.Training;
using TestAutomation.Suite.Responsibilities.ProfessionalDevelopment;
using System.Text.RegularExpressions;
using Selenium2.Meridian.Suite.P1.MyResponsibilities.Training;
using TestAutomation.Suite1.Administration.AdministrationConsole;
using Selenium2.Meridian.Suite.Administration.AdministrationConsole;

namespace Selenium2.Meridian.Suite
{
    [TestFixture("ffbs", Category = "firefox")]
    [TestFixture("chbs", Category = "chrome")]
    [TestFixture("iebs", Category = "iexplorer")]
    [TestFixture("safari", Category = "safari")]
    [TestFixture("edge", Category = "edge")]
    [TestFixture("anybrowser", Category = "local")]

    [Parallelizable(ParallelScope.Fixtures)]
    class RecommendationContentTagTest1 : TestBase
    {
        string browserstr = string.Empty;

        public RecommendationContentTagTest1(string browser)
            : base(browser)
        {
            //   browserstr = browser + "apo";
             
            // driver.Manage().Window.Maximize();
            Driver.Instance = driver;
            InitializeBase(driver);
            LoginPage.GoTo();
            //    LoginPage.LoginClick();
            LoginPage.LoginAs("").WithPassword("").Login();

        }

        string tagname = "Tag_Reg" + Meridian_Common.globalnum;
        string generalcoursetitle = "GC_Reg" + Meridian_Common.globalnum;
        string classroomcoursetitle= "ClassRoomCourseTitle" + Meridian_Common.globalnum;
        string scormtitle = "scorm" + Meridian_Common.globalnum;
        bool TC_34081 = false;
        bool TC_34082 = false;
        string user = "User" + Meridian_Common.globalnum;

        [SetUp]
        public void starttest()
        {
            string tagname = "Tag_Reg" + Meridian_Common.globalnum;
            _test = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            int ix1 = TestContext.CurrentContext.Test.FullName.LastIndexOf('.');
            int ix2 = ix1 > 0 ? TestContext.CurrentContext.Test.FullName.LastIndexOf('.', ix1 - 1) : -1;
            Meridian_Common.CurrentTestName = TestContext.CurrentContext.Test.FullName.Remove(0, ix2 + 1);


           
        }
        [Test, Order(1)]
        public void A01_Learner_Set_Their_Interest_from_HomePage_33520()
        {
             CommonSection.CreateLink.GeneralCourse();
            _test.Log(Status.Info, "Goto Content Creation Page");
            GeneralCoursePage.CreateGeneralCourse(generalcoursetitle, generalcoursetitle);
            _test.Log(Status.Info, "Content Created");
           String expectedtagname = GeneralCoursePage.CreateTags(tagname);
            _test.Log(Status.Info, "Tag Created");
            StringAssert.AreEqualIgnoringCase(tagname, expectedtagname);
            _test.Log(Status.Info, "Assertion Pass as Tag Has been created successfully.");
            DocumentPage.ClickButton_CheckIn();
            CommonSection.Learn.Home();
            _test.Log(Status.Info, "Navigating to HomePage");
            driver.LogoutUser(ObjectRepository.LogoutHoverLink, ObjectRepository.HoverMainLink);
            _test.Log(Status.Info, "Logout from admin");
            LoginPage.LoginAs("ssuser1").WithPassword("password").Login();
            _test.Log(Status.Info, "Login with Learner");
            Assert.IsTrue(HomePage.selectInterest(tagname, generalcoursetitle));
            _test.Log(Status.Info, "Assertion Pass as learner selected interest from homepage successfully");
            driver.LogoutUser(ObjectRepository.LogoutHoverLink, ObjectRepository.HoverMainLink);
            LoginPage.LoginAs("").WithPassword("").Login();

        }

        [Test, Order(2)]
        public void A02_Learner_See_Only_Those_Tags_which_are_Linked_with_a_Content_for_which_Learner_Have_Access_34079()
        {
            // This test case depends on above Test Case 33520
            CommonSection.Manage.Training();
            _test.Log(Status.Info, "Navigating to Manage Training");
            TrainingPage.SearchRecord(generalcoursetitle);
            _test.Log(Status.Info, "Searching the record");
            ManageContentPage.ClickContentTitle(generalcoursetitle);
            _test.Log(Status.Info, "Clicking on searched record");
            DocumentPage.ClickButton_CheckOut();
            ManageCompetencyPage.removePermission();
            _test.Log(Status.Info, "Removing Permission of learner from Content");
            DocumentPage.ClickButton_CheckIn();
            driver.LogoutUser(ObjectRepository.LogoutHoverLink, ObjectRepository.HoverMainLink);
            _test.Log(Status.Info, "Logout from Admin");
            LoginPage.LoginAs("ssuser1").WithPassword("password").Login();
            _test.Log(Status.Info, "Login as Learner");
            Assert.IsFalse(Driver.Instance.IsElementVisible(By.XPath("//h4[contains(text(),'" + generalcoursetitle + "')]")));
            _test.Log(Status.Info, "Assertion Pass as learner do not have access to those content tags for which learner do not have access");
            driver.LogoutUser(ObjectRepository.LogoutHoverLink, ObjectRepository.HoverMainLink);
            LoginPage.LoginAs("").WithPassword("").Login();
        }

        [Test, Order(3)]
        public void A03_Admin_Add_New_Tag_into_a_Content_on_ManageContent_Page_34080()
        {
            // This test case depends on above two test cases 34079 and 33520
            CommonSection.Manage.Training();
            _test.Log(Status.Info, "Navigating to Manage Training");
            TrainingPage.SearchRecord(generalcoursetitle);
            _test.Log(Status.Info, "Searching the record");
            ManageContentPage.ClickContentTitle(generalcoursetitle);
            _test.Log(Status.Info, "Clicking on searched record");
            DocumentPage.ClickButton_CheckOut();
            ManageCompetencyPage.addPermission();
            _test.Log(Status.Info, "Adding Permission into Content");
            DocumentPage.ClickButton_CheckIn();
            CommonSection.Manage.Training();
            _test.Log(Status.Info, "Navigating to Manage Training");
            TrainingPage.SearchRecord(generalcoursetitle);
            _test.Log(Status.Info, "Searching the record");
            Assert.IsTrue(TrainingPage.removeTag_ManageContentSearchPage(tagname));
            TC_34082 = true;
            _test.Log(Status.Info, "Assertion Pass as User removed tag from content from Manage Content Search Page");
            Assert.IsTrue(TrainingPage.addTag_ManageContentSearchPage(tagname));
            TC_34081 = true;
            _test.Log(Status.Info, "Assertion Pass as User added tag into content from Manage Content Search Page");

        }

        [Test,Order(4)]
        public void A04_Admin_Add_Existing_Tag_into_Content_On_Manage_Content_Page_34081()
        {
            Assert.IsTrue(TC_34081); // This test case alredy covered into TC 34080
            _test.Log(Status.Info, "Assertion Pass as User able to add existing tags into content from manage content search page");
        }

        [Test, Order(5)]
        public void A05_Admin_Remove_Tag_from_Content_Item_on_Manage_Content_Page_34082()
        {
            Assert.IsTrue(TC_34082); // This test case alredy covered into TC 34080
            _test.Log(Status.Info, "Assertion Pass as User able to remove tags from content from manage content search page");
        }


        [Test, Order(6)]
        public void A06_To_Test_Admin_User_Manages_Recommendations_Settings_by_Domain_33896()
        {
            // This test case scope is very less as this is somthing related to domain setting. We can not complete this test case as there is no child domain in external site.
            CommonSection.Manage.Recommendations();
            Assert.IsTrue(RecommendationsPage.checkDomainDropdown());
            _test.Log(Status.Info, "Assertion Pass as Select Domain Dropdown displaying to admin user under recommendation setting page ");
        }

        [Test, Order(7)]
        public void A07_Learner_See_Recently_Added_Contents_Recommendation_On_Homepage_33469()
        {
            CommonSection.CreateLink.GeneralCourse();
            _test.Log(Status.Info, "Goto Content Creation Page");
            GeneralCoursePage.CreateGeneralCourse(generalcoursetitle+"TC33469", generalcoursetitle);
            _test.Log(Status.Info, "Content Created");
            String expectedtagname = GeneralCoursePage.CreateTags(tagname);
            _test.Log(Status.Info, "Tag Created");
            StringAssert.AreEqualIgnoringCase(tagname, expectedtagname);
            _test.Log(Status.Info, "Assertion Pass as Tag Has been created successfully.");
            DocumentPage.ClickButton_CheckIn();
            RecommendationsPage.selectMostRecent_RecentlyAdded();
            CommonSection.Learn.Home();
            _test.Log(Status.Info, "Navigate to Homepage");
            Assert.IsTrue(Driver.Instance.IsElementVisible(By.XPath("//h4[contains(text(),'"+generalcoursetitle+"TC33469"+"')]")));
            _test.Log(Status.Info, "Assert recently added content display on homepage : Pass");

        }

        [Test, Order(8)]
        public void A08_Admin_Merges_Users_that_Both_Have_Selected_Interests_33987()
        {
            Driver.CreateNewAccount_Specific(user + "User1");
            LoginPage.LoginAs(user + "User1").WithPassword("").Login();
            CommonSection.Avatar.Account();
            AccountPage.ClickProfiletab();
            string primaryuser1_tags = AccountPage.addInterest();
            Driver.CreateNewAccount_Specific(user + "User2");
            LoginPage.LoginAs(user + "User2").WithPassword("").Login();
            CommonSection.Avatar.Account();
            AccountPage.ClickProfiletab();
            string primaryuser2_tags = AccountPage.addInterest();
            LoginPage.LoginAs("").WithPassword("").Login();
            CommonSection.Administer.People.MergeUser();
            MergeUsersPage.mergeUsers(user + "User1", user + "User2");

            driver.LogoutUser(ObjectRepository.LogoutHoverLink, ObjectRepository.HoverMainLink);
            LoginPage.LoginAs(user + "User1").WithPassword("").Login();
            CommonSection.Avatar.Account();
            AccountPage.ClickProfiletab();
            string expectedTag = Driver.Instance.GetElement(By.XPath("//a[@class='btn btn-add-remove btn-outline-primary']")).Text;
            StringAssert.AreEqualIgnoringCase(primaryuser1_tags, expectedTag);


        }


        //[TearDown]
        public void stoptest()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var stacktrace = string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace)
                    ? ""
                    : string.Format("{0}", TestContext.CurrentContext.Result.StackTrace);
            var errorMessage = TestContext.CurrentContext.Result.Message;
            Status logstatus;

            switch (status)
            {
                case TestStatus.Failed:
                    logstatus = Status.Fail;
                    string screenShotPath = ScreenShot.Capture(driver, browserstr);
                    _test.Log(Status.Info, "Assertion has been Failed, Please Take a Look to Below Screenshot and StackTrace");
                    _test.Log(Status.Info, stacktrace + errorMessage);
                    _test.Log(Status.Info, "Snapshot below: " + _test.AddScreenCaptureFromPath(screenShotPath));
                    screenShotPath = screenShotPath.Replace(@"C:\Users\admin\Desktop\Automation\Regression Scripts\18.2\", @"Z:\");
                    _test.Log(Status.Info, "MailSnapshot below: " + _test.AddScreenCaptureFromPath(screenShotPath));

                    break;
                case TestStatus.Inconclusive:
                    logstatus = Status.Warning;
                    break;
                case TestStatus.Skipped:
                    logstatus = Status.Skip;
                    break;
                default:
                    logstatus = Status.Pass;
                    break;
            }

            _test.Log(logstatus, "Test ended with " + logstatus + stacktrace);
            //  _extent.Flush();
            if (Meridian_Common.islog == true)
            {
                driver.LogoutUser(ObjectRepository.LogoutHoverLink, ObjectRepository.HoverMainLink);
            }
            else
            {
                CommonSection.Learn.Home();
             //   Driver.Instance.Navigate_to_TrainingHome();
                //  TrainingHomeobj.lnk_TrainingManagement_click(By.XPath("//a[contains(.,'Administer')]"), By.XPath("//a[contains(.,'Training Management')]"));
                //if (!driver.GetElement(By.Id("lbUserView")).Displayed)
                //{
                //    driver.Navigate().Refresh();
                //}
                //    driver.Navigate().Refresh();
            }
        }



    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Selenium2;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using NUnit.Framework;
using Selenium2.Meridian;
using System.Threading;
using TestAutomation.Meridian.Regression_Objects;
using System.Configuration;
using System.Diagnostics;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Configuration;
using Selenium2.Meridian.P1.MyResponsibilities.Training;
using System.IO;
using NUnit.Framework.Interfaces;

//using RelevantCodes.ExtentReports;

//using TestAutomation.PageFactoryTests;

namespace Selenium2
{


    public class TestBase : Selenium2TestBase
    {
        public IWebDriver driver;
        protected ExtentReports _extent;
        protected ExtentTest _test;
        string reportdir = string.Empty;
        string url = string.Empty;
       

        [OneTimeSetUp]
        protected void Setup()
        {
          
            var dir = TestContext.CurrentContext.TestDirectory + "\\";
            var fileName = this.GetType().ToString() + ".html";

            reportdir = dir + "reports";
                var htmlReporter = new ExtentHtmlReporter(dir + fileName);
            htmlReporter.Configuration().Theme = Theme.Dark;

            htmlReporter.Configuration().DocumentTitle = "TestReport";

            htmlReporter.Configuration().ReportName = "MeridianReportfor 18.1";
           
            _extent = new ExtentReports();
            _extent.AttachReporter(htmlReporter);

            ICapabilities cap = ((RemoteWebDriver)Driver.Instance).Capabilities;
            
           
                _extent.AddSystemInfo("Environment", ConfigurationManager.AppSettings["MeridianTestEnvironment"]);
            
                _extent.AddSystemInfo("Browser", ConfigurationManager.AppSettings["Selenium2Browserch"]);

                _extent.AddSystemInfo("Browser Name", cap.BrowserName);
                _extent.AddSystemInfo("Browser Version", cap.Version);
               // _extent.AddSystemInfo("Environment", cap.Platform.MajorVersion.ToString());



        }
        string browserstr = string.Empty;
        internal string passwd(string browser)
        {
            if (browser == "1")
            {
                Meridian_Common.SmokeTestSite=browser;
                return "LM$@dm1n";
            }
            else if (browser == "2")
            {
                Meridian_Common.SmokeTestSite = browser;
                return "b";
            }
           else if (browser == "3")
            {
                Meridian_Common.SmokeTestSite = browser;
                return "c";
            }
            else if (browser == "4")
            {
                Meridian_Common.SmokeTestSite = browser;
                return "d";
            }
            else if (browser == "5")
            {
                Meridian_Common.SmokeTestSite = browser;

                return "e";
            }
            else
                return "finish";
        }

        internal string username(string browser)
        {
            if (browser == "1")
                return "siteadmin";
            else if (browser == "2")
                return "b";
            else if (browser == "3")
                return "c";
            else if (browser == "4")
                return "d";
            else if (browser == "5")
                return "e";
            else
                return "finish";
        }

        internal string rootfolder(string browser)
        {
            if (browser == "1")
                return "a";
            else if (browser == "2")
                return "b";
            else if (browser == "3")
                return "c";
            else if (browser == "4")
                return "d";
            else if (browser == "5")
                return "e";
            else
                return "finish";
        }
        
        internal string Url(string browser)
        {
            if (browser == "1")
            {
              browser=Meridian_Common.SmokeTestSite;
                ConfigurationManager.AppSettings["MeridianTestEnvironment"] = "https://prdct-mg-18-2.mksi-lms.net/";
                url = ConfigurationManager.AppSettings["MeridianTestEnvironment"];

                return url;
            }
            else if (browser == "2")
            {
                browser = Meridian_Common.SmokeTestSite ;
                ConfigurationManager.AppSettings["MeridianTestEnvironment"] = "https://www.medifastuniversity.com";
                url = ConfigurationManager.AppSettings["MeridianTestEnvironment"];
                return url;
            }
            else if (browser == "3")
            {
                browser = Meridian_Common.SmokeTestSite ;
                ConfigurationManager.AppSettings["MeridianTestEnvironment"] = "https://lms.runzheimer.com";
                url = ConfigurationManager.AppSettings["MeridianTestEnvironment"];

                return url;
            }
            else if (browser == "4")
            {
                browser = Meridian_Common.SmokeTestSite ;
                ConfigurationManager.AppSettings["MeridianTestEnvironment"] = "https://nyceitraining.mkscloud.com/";
                url = ConfigurationManager.AppSettings["MeridianTestEnvironment"];

                return url;
            }
            else if (browser == "5")
            {
                browser = Meridian_Common.SmokeTestSite;
                ConfigurationManager.AppSettings["MeridianTestEnvironment"] = "https://prdct-mg-18-2.mksi-lms.net/";
                url = ConfigurationManager.AppSettings["MeridianTestEnvironment"];

                return url;
            }
            else
                return "finish";

        }
        [TearDown]
        public void stoptest()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var stacktrace = string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace)
                    ? ""
                    : string.Format("{0}", TestContext.CurrentContext.Result.StackTrace);
            Status logstatus;
            var errorMessage = TestContext.CurrentContext.Result.Message;
            switch (status)
            {
                case TestStatus.Failed:
                    logstatus = Status.Fail;
                    string screenShotPath = ScreenShot.Capture(driver, browserstr);
                    _test.Log(Status.Info, stacktrace + errorMessage);
                    _test.Log(Status.Info, "Snapshot below: " + _test.AddScreenCaptureFromPath(screenShotPath));
                    screenShotPath = screenShotPath.Replace(@"C:\RegressionSuite\Regression Scripts\Somnath\TestAutomation\ErrorScreenshots", @"Z:\");
                    _test.Log(Status.Info, "MailSnapshot below: " + _test.AddScreenCaptureFromPath(screenShotPath));
                    if (driver.Title == "Object reference not set to an instance of an object.")
                    {
                        driver.Navigate_to_TrainingHome();
                        Driver.focusParentWindow();
                        CommonSection.Avatar.Logout();
                        LoginPage.LoginClick();
                        LoginPage.LoginAs("siteadmin").WithPassword("password").Login();
                    }
                    //if (!Meridian_Common.isadminlogin)
                    //{
                    //    CommonSection.Logout();
                    //    LoginPage.LoginAs("").WithPassword("").Login();
                    //}


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
               // driver.Navigate_to_TrainingHome();
                //  TrainingHomeobj.lnk_TrainingManagement_click(By.XPath("//a[contains(.,'Administer')]"), By.XPath("//a[contains(.,'Training Management')]"));
                //if (!driver.GetElement(By.Id("lbUserView")).Displayed)
                //{
                //    driver.Navigate().Refresh();
                //}
                //    driver.Navigate().Refresh();
            }
        }
        [OneTimeTearDown]
        protected void TearDown()
        {
           
            _extent.Flush();
            String Todaysdate = DateTime.Now.ToString("dd-MMM-yyyy");

            string destpath = string.Empty;
            string pth = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
            string otherpath = pth.Substring(0, pth.LastIndexOf("bin")) + "reports/" + Todaysdate;
            destpath = new Uri(otherpath).LocalPath;
            string fileName = string.Empty;
            string destFile = string.Empty;
            //  pth.Substring(0, pth.LastIndexOf("bin"))
            if (!Directory.Exists(otherpath))
            {

                // otherpath1 = new Uri(otherpath).LocalPath;
                Directory.CreateDirectory(destpath);


            }
            
            string sourcepath = TestContext.CurrentContext.TestDirectory;
            string[] files1 = Directory.GetFiles(sourcepath, "*.html", SearchOption.TopDirectoryOnly);
            //string[] files = System.IO.Directory.GetFiles(root);
            foreach(var filename in files1)
            {
                fileName = System.IO.Path.GetFileName(filename);
                fileName = Meridian_Common.SmokeTestSite + fileName;
                destFile = System.IO.Path.Combine(destpath, fileName);
               // File.Move(filename, destFile);
            }
            Driver.Instance.Quit();
          //  Driver.Instance.Close(); ;
        }

        //  string[] localbrowser = new string[] { "firefox", "iexplore", "chrome" };

        public TestBase(string browser)
        {
            Common.closeie();
            string singleExecution = "yes";
            if (ConfigurationManager.AppSettings["Selenium2Browser"] == null)
            {
                if (ConfigurationManager.AppSettings["Selenium2Browserff"] == browser)
                {
                    driver = StartBrowser(browser);
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
                }
                if (ConfigurationManager.AppSettings["Selenium2Browserch"] == browser)
                {
                    driver = StartBrowser(browser);
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
                }
                if (ConfigurationManager.AppSettings["Selenium2Browserie"] == browser)
                {
                  //  Meridian_Common.MeridianTestbrowser = browser;
                    driver = StartBrowser(browser);
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
                }
                if (ConfigurationManager.AppSettings["Selenium2Browsersa"] == browser)
                {
                 //   Meridian_Common.MeridianTestbrowser = browser;
                    driver = StartBrowser(browser);
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
                }
                if (ConfigurationManager.AppSettings["Selenium2Browsered"] == browser)
                {
                  //  Meridian_Common.MeridianTestbrowser = browser;
                    driver = StartBrowser(browser);
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
                }
                //if (ConfigurationManager.AppSettings["Selenium2Browserff"] == browser)
                //{
                //    driver = StartBrowser(browser);
                //}
                if ("anybrowser" == browser)
                {
                    driver = StartBrowser(ConfigurationManager.AppSettings["selenium2browser"]);
                    //driver.Manage().Window.Maximize();
                    if(ConfigurationManager.AppSettings["selenium2browser"]=="chrome")
                    {
                        CloseChromeDialog();
                    }
                    if (ConfigurationManager.AppSettings["selenium2browser"] == "IE11")
                    {
                        Meridian_Common.MeridianTestbrowser = "IE11";
                    }
                    if (ConfigurationManager.AppSettings["selenium2browser"] == "iexplore")
                    {
                        Meridian_Common.MeridianTestbrowser = "IE";
                    }

                }
                //  driver = StartBrowser(browser);

                // }

            }
            else if (ConfigurationManager.AppSettings["Selenium2Browser"] != null)
            {
                if (singleExecution == "yes")
                {
                    if (ConfigurationManager.AppSettings["Selenium2Browser"] == "iexplore")
                    {
                        Meridian_Common.MeridianTestbrowser = "IE";
                        driver = StartBrowser(ConfigurationManager.AppSettings["Selenium2Browser"]);
                        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
                    }
                    else
                    {
                        driver = StartBrowser(ConfigurationManager.AppSettings["Selenium2Browser"]);
                        singleExecution = "no";
                        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
                    }
                }
                else
                {
                    OneTimeTearDown();
                }
            }

            else
            {
                OneTimeTearDown();
                //Assert.Ignore("checking");
            }





            ExtractDataExcel.fillalldic();



        }
        public void CloseChromeDialog()
        {
            System.Threading.Thread.Sleep(5000);
            Process.Start(@".\closechromewarning.exe");
        }
        public void OneTimeTearDown()
        {
            //  Meridian_Common.checklocal = true;
            Meridian_Common.islog = false;
            //   driver.Quit();
            //  Assert.Inconclusive("Checking");
            //try
            //{
            //    driver.Quit();
            Assert.Ignore("Quitting test as it is not configured");
            ////}
            //    catch(NUnit.Framework.IgnoreException)
            //{
            //    driver.Quit();
            //}
            //catch(Exception ex)
            //{
            //    ex.InnerException.Message.ToString();
            //}
        }
        //internal MdnHomePage LoginPage;
        //internal MdnLoginPage1 LoginPage1;
        //internal MdnHomePage HomePage;
        //internal MdnCommonPage CommonPage;
        #region internal olds
        internal ClassroomCourse classroomcourse;
        internal My_Responsibilities MyResponsibilitiesobj;
        internal ContentSearch ContentSearchobj;
        internal Details detailspage;
        internal TrainingHomes TrainingHomeobj;
        internal AdminstrationConsole AdminstrationConsoleobj;
        internal Tests Testsobj;
        internal Details Detailsobj;
        internal EditSummary EditSummaryobj;
        internal Scorm1_2 Scorm1_2obj;
        internal EditQuestion EditQuestionobj;
        internal EditQuestionGroup EditQuestionGroupobj;
        internal AddUsers AddUsrObj;
        internal GeneralCourse generalcourseobj;
        internal MyTeachingSchedule myteachingscheduleobj;
        internal ProfessionalDevelopments professionaldevelopmentobj;
        internal Document documentobj;
        internal ManageUsers manageuserobj;
        internal Create Createobj;
        internal Summary summaryobj;
        internal RequiredTrainingConsoles reauiredtrainingconsoleobj;
        internal RequiredTraining requiredtrainingobj;
        internal ManageUsers ManageUsersobj;
        internal CreateNewAccount CreateNewAccountobj;
        

        internal Training Trainingobj;
        internal Login Loginobj;
        internal Content Contentobj;
        internal Credits Creditsobj;
        internal AddContent AddContentobj;
        internal Summary Summaryobj;
        internal ScheduleAndManageSection ScheduleAndManageSectionobj;
        internal SearchResults SearchResultsobj;
        internal CreateNewCourseSectionAndEventPage CourseSectionobj;
        internal Transcripts Transcriptsobj;
        internal Products Productsobj;
        internal BrowseTrainingCatalog BrowseTrainingCatalogobj;
        internal ShoppingCarts ShoppingCartsobj;
        internal ProfessionalDevelopments ProfessionalDevelopmentsobj;
        internal Createnewproficencyscale Createnewproficencyscaleobj;
        internal Createnewcompetency Createnewcompetencyobj;
        internal CreateNewSucessProfile CreateNewSucessProfileobj;
        internal SucessProfile SucessProfileobj;
        internal Search Searchobj;
        internal TrainingActivities TrainingActivitiesobj;
        internal ProfessionalDevelopments_learner ProfessionalDevelopments_learnerobj;
        internal Organization Organizationobj;
        internal DevelopmentPlans DevelopmentPlansobj;
        internal AddDevelopmentActivities AddDevelopmentActivitiesobj;
        internal MyAccount MyAccountobj;
        internal UsersUtil UsersUtilobj;
        internal MyCalenders MyCalendersobj;
        internal MyReports MyReportsobj;
        internal Config_Reports Config_Reportsobj;
        internal ConfigurationConsole ConfigurationConsoleobj;
        internal ApprovalPath ApprovalPathobj;
        internal MyMessages MyMessageobj;
        internal MessageUtil MessageUtilobj;
        internal MyRequests MyRequestsobj;
        internal Blogs Blogsobj;
        internal CollabarationSpaces CollabarationSpacesobj;
        internal Faqs Faqsobj;
        internal HomePageFeed HomePageFeedobj;
        internal ProductTypes ProductTypesobj;
        internal Surveys Surveysobj;
        internal SurveyScales SurveyScalesobj;
        internal AuditingConsoles AuditingConsolesobj;
        internal Category Categoryobj;
        internal Trainings Trainingsobj;
        internal VirtualMeetings VirtualMeetingsobj;
        internal CreditType CreditTypeobj;
        internal AssignedUser AssignedUserobj;
        internal AddUsers AddUsersobj;
        internal CustomField CustomFieldobj;
        internal CreateNewCustomField CreateNewCustomFieldobj;
        internal EditField EditFieldobj;
        internal EducationLevel EducationLevelobj;
        internal EditOrganization EditOrganizationobj;
        internal SelectManager SelectManagerobj;
        internal Role Roleobj;
        internal SelectTrainingPOC SelectTrainingPOCobj;
        internal Complex Complexobj;
        internal AccountCodes AccountCodesobj;
        internal AccountCodeTypes AccountCodeTypesobj;
        internal DiscountCodes DiscountCodesobj;
        internal ManageTaxRates ManageTaxRatesobj;
        internal TaxItemCategories TaxItemCategoriesobj;
        internal Certificates Certificatesobj;
        internal CourseProviders CourseProvidersobj;
        internal ExternalLearnings ExternalLearningsobj;
        internal ExternalLearningConsoles ExternalLearningConsolesobj;
        internal ExternalLearningtypes ExternalLearningtypesobj;
        internal RequiredTrainingConsoles RequiredTrainingConsolesobj;
        internal SelectProfile SelectProfileobj;
        internal TrainingProfiles TrainingProfilesobj;
        internal EditTrainingProfile EditTrainingProfileobj;
        internal MergeUsers MergeUsersobj;
        internal UserGroup UserGroupobj;
        internal SelectCertificate SelectCertificateobj;
        internal EditPermission EditPermissionobj;
        internal Announcements_l Announcements_lobj;
        internal FAQ_l FAQ_lobj;
        internal ManageProficencyScale ManageProficencyScaleobj;
        internal ArchivedProficencyScale ArchivedProficencyScaleobj;
        internal MappedContent MappedContentobj;
        internal MappedCompetency MappedCompetencyobj;
        internal ManageSuccessProfile ManageSuccessProfileobj;
        internal Instructor instructors;
        internal Instructorspof Instructorsobj;
        internal ManageGradebook ManageGradebookobj;
        internal ManagePricingSchedule ManagePricingScheduleobj;
        // internal ManageEnrollmentForOnlineCourses ManageEnrollmentForOnlineCoursesobj;
        internal Approvalrequestobject approvalrequest;
        internal JobTitles JobTitlesobj;
        internal ManageJobTitle ManageJobTitleobj;
        internal DomainConsole DomainConsoleobj;
        internal ExternalLearningSearch ExternalLearningSearchobj;
        internal url urlobj;
        internal CheckOut CheckOutobj;
        internal skin skinobj;
        internal MyOwnLearningUtils MyOwnLearningobj;
        internal CurrentTrainings CurrentTrainingsobj;
        internal Scorm12 scormobj;
        internal AICC aicccourse;
        internal OJT ojtcourse;
        internal TrainingCatalogUtil TrainingCatalogobj;
        internal AccessKeys accesskeys;
        internal TrainingHomes objTrainingHome;
        internal Create objCreate;
        internal CreateCurriculum objCurriculum;
        internal ScreenShot takeScreenhsot;

        #endregion
        public void InitializeBase(IWebDriver objDriver)
        {
            
            //LoginPage = new MdnHomePage(driver);
            //LoginPage1 = new MdnLoginPage1(driver);
            //HomePage = new MdnHomePage(driver);
            //CommonPage = new MdnCommonPage(driver);

            #region initialize old
            driver = objDriver;
            CheckOutobj = new CheckOut(driver);
            takeScreenhsot = new ScreenShot(driver);
            approvalrequest = new Approvalrequestobject(driver);
            instructors = new Instructor(driver);
            approvalrequest = new Approvalrequestobject(driver);
            DomainConsoleobj = new DomainConsole(driver);
            ManageGradebookobj = new ManageGradebook();
            Instructorsobj = new Instructorspof();
            MyResponsibilitiesobj = new My_Responsibilities(driver);
            manageuserobj = new ManageUsers(driver);
            objTrainingHome = new TrainingHomes(driver);
            objCurriculum = new CreateCurriculum(driver);
            classroomcourse = new ClassroomCourse(driver);
            ContentSearchobj = new ContentSearch(driver);
            objCreate = new Create(driver);
            detailspage = new Details(driver);
           

            TrainingHomeobj = new TrainingHomes(driver);
            AdminstrationConsoleobj = new AdminstrationConsole(driver);
            Testsobj = new Tests(driver);
            Detailsobj = new Details(driver);
            EditSummaryobj = new EditSummary(driver);
            Scorm1_2obj = new Scorm1_2(driver);
            EditQuestionobj = new EditQuestion(driver);
            EditQuestionGroupobj = new EditQuestionGroup(driver);

            AddUsrObj = new AddUsers(driver);

            generalcourseobj = new GeneralCourse(driver);
            myteachingscheduleobj = new MyTeachingSchedule();
            professionaldevelopmentobj = new ProfessionalDevelopments(driver);
            documentobj = new Document(driver);
            CreateNewAccountobj = new CreateNewAccount(driver);
            ManageUsersobj = new ManageUsers(driver);
            Createobj = new Create(driver);
            summaryobj = new Summary(driver);
            reauiredtrainingconsoleobj = new RequiredTrainingConsoles(driver);
            requiredtrainingobj = new RequiredTraining(driver);
            Trainingobj = new Training(driver);
            Loginobj = new Login(driver);
            Contentobj = new Content(driver);
            Creditsobj = new Credits(driver);
            AddContentobj = new AddContent(driver);
            Summaryobj = new Summary(driver);
            ScheduleAndManageSectionobj = new ScheduleAndManageSection(driver);
            SearchResultsobj = new SearchResults(driver);
            CourseSectionobj = new CreateNewCourseSectionAndEventPage(driver);
            Transcriptsobj = new Transcripts(driver);
            Productsobj = new Products(driver);
            BrowseTrainingCatalogobj = new BrowseTrainingCatalog(driver);
            ShoppingCartsobj = new ShoppingCarts(driver);
            ProfessionalDevelopmentsobj = new ProfessionalDevelopments(driver);
            Createnewproficencyscaleobj = new Createnewproficencyscale(driver);
            Createnewcompetencyobj = new Createnewcompetency(driver);
            CreateNewSucessProfileobj = new CreateNewSucessProfile(driver);
            SucessProfileobj = new SucessProfile(driver);
            Searchobj = new Search(driver);
            TrainingActivitiesobj = new TrainingActivities(driver);
            ProfessionalDevelopments_learnerobj = new ProfessionalDevelopments_learner(driver);
            Organizationobj = new Organization(driver);
            DevelopmentPlansobj = new DevelopmentPlans(driver);
            AddDevelopmentActivitiesobj = new AddDevelopmentActivities(driver);
            MyAccountobj = new MyAccount(driver);
            UsersUtilobj = new UsersUtil(driver);
            MyCalendersobj = new MyCalenders(driver);
            MyReportsobj = new MyReports(driver);
            Config_Reportsobj = new Config_Reports(driver);
            ConfigurationConsoleobj = new ConfigurationConsole(driver);
            ApprovalPathobj = new ApprovalPath(driver);
            MyMessageobj = new MyMessages(driver);
            MessageUtilobj = new MessageUtil(driver);
            MyRequestsobj = new MyRequests(driver);
            Blogsobj = new Blogs(driver);
            CollabarationSpacesobj = new CollabarationSpaces(driver);
            Faqsobj = new Faqs(driver);
            HomePageFeedobj = new HomePageFeed(driver);
            ProductTypesobj = new ProductTypes(driver);
            Surveysobj = new Surveys(driver);
            SurveyScalesobj = new SurveyScales(driver);
            AuditingConsolesobj = new AuditingConsoles(driver);
            Categoryobj = new Category(driver);
            Trainingsobj = new Trainings(driver);
            VirtualMeetingsobj = new VirtualMeetings(driver);
            CreditTypeobj = new CreditType(driver);
            AssignedUserobj = new AssignedUser(driver);
            AddUsersobj = new AddUsers(driver);
            CustomFieldobj = new CustomField(driver);
            CreateNewCustomFieldobj = new CreateNewCustomField(driver);
            EditFieldobj = new EditField(driver);
            EducationLevelobj = new EducationLevel(driver);
            EditOrganizationobj = new EditOrganization(driver);
            SelectManagerobj = new SelectManager(driver);
            Roleobj = new Role(driver);
            SelectTrainingPOCobj = new SelectTrainingPOC(driver);
            Complexobj = new Complex(driver);
            AccountCodesobj = new AccountCodes(driver);
            AccountCodeTypesobj = new AccountCodeTypes(driver);
            DiscountCodesobj = new DiscountCodes(driver);
            ManageTaxRatesobj = new ManageTaxRates(driver);
            TaxItemCategoriesobj = new TaxItemCategories(driver);
            Certificatesobj = new Certificates(driver);
            CourseProvidersobj = new CourseProviders(driver);
            ExternalLearningsobj = new ExternalLearnings(driver);
            ExternalLearningConsolesobj = new ExternalLearningConsoles(driver);
            ExternalLearningtypesobj = new ExternalLearningtypes(driver);
            RequiredTrainingConsolesobj = new RequiredTrainingConsoles(driver);
            SelectProfileobj = new SelectProfile(driver);
            TrainingProfilesobj = new TrainingProfiles(driver);
            EditTrainingProfileobj = new EditTrainingProfile(driver);
            MergeUsersobj = new MergeUsers(driver);
            UserGroupobj = new UserGroup(driver);
            SelectCertificateobj = new SelectCertificate(driver);
            ManageProficencyScaleobj = new ManageProficencyScale(driver);
            ArchivedProficencyScaleobj = new ArchivedProficencyScale(driver);
            MappedContentobj = new MappedContent(driver);
            MappedCompetencyobj = new MappedCompetency(driver);
            ManageSuccessProfileobj = new ManageSuccessProfile(driver);
            FAQ_lobj = new FAQ_l(driver);
            Announcements_lobj = new Announcements_l(driver);
            JobTitlesobj = new JobTitles(driver);
            ManageJobTitleobj = new ManageJobTitle(driver);
            ManagePricingScheduleobj = new ManagePricingSchedule(driver);
            ExternalLearningSearchobj = new ExternalLearningSearch(driver);
            urlobj = new url(driver);
            skinobj = new skin(driver);
            MyOwnLearningobj = new MyOwnLearningUtils(driver);
            CurrentTrainingsobj = new CurrentTrainings(driver);
            scormobj = new Scorm12(driver);
            aicccourse = new AICC(driver);
            ojtcourse = new OJT(driver);
            TrainingCatalogobj = new TrainingCatalogUtil(driver);
            accesskeys = new AccessKeys(driver);
            #endregion
        }

        protected void StartReport()
        {
            //string pth = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
            //string actualpath = pth.Substring(0, pth.LastIndexOf("bin"));
            //string projectPath = new Uri(actualpath).LocalPath;

            //string reportPath = projectPath + "Reports\\MyOwnReport2.html";

            //extent = new ExtentReports(reportPath, true);


            //extent.AddSystemInfo("Host Name", "Saif")
            //    .AddSystemInfo("Environemnt", "QA")
            //    .AddSystemInfo("username", "saifafzal");
        }
    }
}

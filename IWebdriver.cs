using System;
using NUnit.Framework;
using Selenium2;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Remote;
using relativepath;
using System.Reflection;
using System.Collections.ObjectModel;
using Selenium2.Meridian;
using System.Linq;
using OpenQA.Selenium.Interactions;
using System.Drawing;
using System.Collections.Generic;
using TestAutomation.Meridian.Regression_Objects;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace OpenQA.Selenium
{
    public static class IWebElementExtensions
    {
        static string Browser = null;
        public static void ClickWithSpace(this IWebElement driver)
        {
            try
            {
                string text = GetBrwser(driver);
                if (text.Equals("IE"))
                {
                    Thread.Sleep(1000);
                    // driver.SendKeys(Keys.Enter); closed this for safari open it if not safari
                    driver.Click();
                }

                else
                {
                    //    Thread.Sleep(2000);
                    //  driver.SendKeys("");
                    driver.Click();
                }
            }
            catch (InvalidOperationException ex)
            {
                driver.Click();
            }
            catch (ElementNotVisibleException ex)
            {

            }
            catch (Exception ex)
            {

            }

        }
    
    
    public static void ClickAnchor(this IWebElement driver)
        {
            try
            {
                if (GetBrwser(driver).Equals("IE") || GetBrwser(driver).Equals("firefox"))
                {
                    Thread.Sleep(1000);
                    driver.SendKeys(Keys.Enter);
                }
                else
                {
                    Thread.Sleep(2000);
                    driver.SendKeys("");
                    driver.Click();
                }
            }
            catch (InvalidOperationException ex)
            {
                driver.Click();
            }

        }
        public static string GetBrwser(this IWebElement driver)
        {
      
          
          //string text =  CapabilityType.BrowserName.ToString();
            Browser = ((OpenQA.Selenium.Remote.RemoteWebElement)(driver)).WrappedDriver.ToString();
          if(Browser=="OpenQA.Selenium.Remote.RemoteWebDriver")
          {
              Browser=((OpenQA.Selenium.Remote.RemoteWebDriver)(((OpenQA.Selenium.Remote.RemoteWebElement)(driver)).WrappedDriver)).Capabilities.BrowserName.ToString();
          }
            if (Browser.ToLower().Contains("firefox"))
            { Browser = "firefox"; }
            else if (Browser.ToLower().Contains("chrome"))
            { Browser = "chrome"; }
            else if (Browser.ToLower().Contains("microsoftedge"))
            { Browser = "edge"; }
            else
                Browser = "IE";
            return Browser;
        }
        
        public static void ClickChkBox(this IWebElement driver)
        {
            
            Thread.Sleep(1000);
            driver.SendKeys("");
            driver.SendKeys(Keys.Space);
           
            //   driver.Click();
           // driver.Click();
        }
        public static void SendKeysWithSpace(this IWebElement driver, string text)
        {
            Thread.Sleep(2000);
            //  driver.SendKeys("");
            if (Meridian_Common.MeridianTestbrowser == "IE")
            {
                driver.SendKeys(text);
            }
            else
            {
                //    driver.Click();
                driver.SendKeys(text);
            }
        }

        public static void selectDropdownValue(this IWebDriver driver, By Element_id_of_Dropdown, string Text_to_Select)
        {
            IWebElement Element_id_of_Dropdown1 = driver.GetElement(Element_id_of_Dropdown);
            var selectElement = new SelectElement(Element_id_of_Dropdown1);
            selectElement.SelectByText(Text_to_Select);
        }
    }

    public static class IWebDriverExtensions
    {

        /// <summary>
        /// This method finds a select element and then selects the option element using the visible text
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="bylocatorForSelectElement"></param>
        /// <param name="text"></param>
        public static void FindSelectElement(this IWebDriver driver, By bylocatorForSelectElement, String text)
        {
            IWebElement selectElement = driver.GetElement(bylocatorForSelectElement);
            selectElement.FindElement(By.XPath("//option[contains(text(), '" + text + "')]")).ClickWithSpace();
        }
        public static void navigateAICCfile(this IWebDriver driver, string testpath, By by)
        {
            RelativeDirectory rd = new RelativeDirectory();
            string path = string.Empty;
            try
            {


                path = rd.Up(2) + testpath;
                // path = path.Replace("\\", "/");
                Thread.Sleep(1000);
                uploadFile(driver, path, by);
                //  Thread.Sleep(11000);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                driver.LogoutUser(ObjectRepository.LogoutHoverLink, ObjectRepository.HoverMainLink);

            }
        }

        public static void uploadFile(this IWebDriver driver, string path, By by)
        {
            try
            {
                //Thread.Sleep(6000);

                IAllowsFileDetection allowsDetection = (IAllowsFileDetection)driver;
                if (allowsDetection != null)
                {
                    allowsDetection.FileDetector = new LocalFileDetector();

                }
               driver.FindElement(by).SendKeys(path);

                Thread.Sleep(2000);
                //scorm12.GetElement(By.XPath("//td[@id='TabMenu_ML_BASE_TAB_UploadContent_TDElementUploadFile']/table/tbody/tr/td/input")).SendKeys(path);
            }
            catch (Exception ex)
            {
                //  System.Windows.Forms.SendKeys.SendWait("{ESC}");
                Thread.Sleep(6000);
                // path = string.Empty;
                // uploadfile(driver, path, by);
            }
        }

        public static void highlightElement(this IWebDriver driver, By by)
        {
            for (int i = 0; i < 10; i++)
            {
                IWebElement element = driver.FindElement(by);
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("arguments[0].setAttribute('style', arguments[1]);", element, "color: red; border: 2px solid red;");
                Thread.Sleep(1000);
                js.ExecuteScript("arguments[0].setAttribute('style', arguments[1]);", element, "");
            }
        }
        public static bool IsElementVisible(this IWebDriver driver, By elementby)
        {
            try
            {
                return driver.GetElement(elementby).Displayed && driver.GetElement(elementby).Enabled;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static IWebElement GetElement(this IWebDriver driver, By by)
        {
            String execScript = "return document.documentElement.scrollHeight>document.documentElement.clientHeight;";
            IJavaScriptExecutor scrollBarPresent = (IJavaScriptExecutor)driver;
            Boolean test = (Boolean)(scrollBarPresent.ExecuteScript(execScript));
            if (test == true)
            {
                IWebElement element = driver.FindElement(by);
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
                Thread.Sleep(500);
            }
            else if (test == false)
            {//commented the below code as it was not working with firefox in Browserstack
                //IWebElement element = driver.FindElement(by);
                //Actions action = new Actions(driver);
                //action.MoveToElement(element).Perform();
                //Console.WriteLine("Scroll bar not present");
            }


            for (int i = 1; i <= 10; i++)
            {
                try
                {
                    return driver.FindElement(by);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception was raised on locating element: " + ex.Message);

                }

            }
            throw new ElementNotVisibleException(by.ToString());



        }
        public static string getSuccessMessage(this IWebDriver driver)
        {
           // driver.WaitForElement(By.XPath("//div[@class='alert alert-dismissible alert-fixed alert-success']"));
            int n = 0;
            while (n == 0)
                try
                {
                    driver.FindElement(By.XPath("//div[@id='content']/div/div")).Click();

                    break;
                }
                catch (StaleElementReferenceException e)
                {
                }
            string rettext = driver.GetElement(By.XPath("//div[@id='content']/div/div")).Text; //driver.GetElement(By.XPath("//div[@class='alert alert-dismissible alert-fixed alert-success']")).Text;
            return rettext;
        }
        public static bool comparePartialString(this IWebDriver driver,string text, string actual)
        {
            string act1 = Regex.Replace(text, @"\s+", "");
            string act2 = Regex.Replace(actual, @"\s+", "");
            bool res = act2.Contains(act1);
            return res;
        }
        public static void ClickEleJs(this IWebDriver driver, By by)
        {
            try
            {
                if (Meridian_Common.MeridianTestbrowser == "iebs" || Meridian_Common.MeridianTestbrowser == "IE" || Meridian_Common.MeridianTestbrowser == null || Meridian_Common.MeridianTestbrowser == "edge" || Meridian_Common.MeridianTestbrowser == "safari")
                {
                    driver.WaitForElement(by);
                    IWebElement element = driver.GetElement(by);
                    IJavaScriptExecutor executor = (IJavaScriptExecutor)driver;
                    executor.ExecuteScript("arguments[0].click();", element);
                }
                else
                {
                    driver.WaitForElement(by);
                    IWebElement element = driver.GetElement(by);
                    IJavaScriptExecutor executor = (IJavaScriptExecutor)driver;
                    executor.ExecuteScript("arguments[0].click();", element);
                }
            }
            catch(Exception ex)
            {

            }
        }
        public static bool Compareregexstring(this IWebDriver driver,string text, string actual)
        {
            string act1 = System.Text.RegularExpressions.Regex.Replace(text, @"\s+", "");
            string act2 = System.Text.RegularExpressions.Regex.Replace(actual, @"\s+", "");
        bool res=act2.Contains(act1);
        return res;
        }
        public static void ClickRB(this IWebDriver driver, By by)
        {
            try
            {
                if (GetBrwser(driver).Equals("IE"))
                {
                    IWebElement element = driver.FindElement(by);
                    IJavaScriptExecutor executor = (IJavaScriptExecutor)driver;
                    executor.ExecuteScript("arguments[0].click();", element);
                    Thread.Sleep(1000);

                }
                else
                {
                    Thread.Sleep(2000);
                    driver.GetElement(by).SendKeys("");
                    driver.GetElement(by).Click();
                }

            }
            catch (Exception ex)
            {

            }
        }
        public static void SwitchtoDefaultContent(this IWebDriver driver)
        {
            if (Meridian_Common.MeridianTestbrowser == "iebs")
            {
                string popupHandle = string.Empty;
                string existingWindowHandle = driver.CurrentWindowHandle;
                ReadOnlyCollection<string> windowHandles = driver.WindowHandles;

                foreach (string handle in windowHandles)
                {
                    if (handle == existingWindowHandle)
                    {
                        popupHandle = handle; break;
                    }
                    else
                    {
                        driver.SwitchTo().DefaultContent();
                    }
                }
                driver.SwitchTo().Window(popupHandle);



            }
            else
            {
                driver.SwitchTo().DefaultContent();
            }
            }
        

       public static string Browser = null;


        public static string GetBrwser(this IWebDriver driver)
        {
            Browser = driver.GetType().ToString();
            if (Browser.ToLower().Contains("firefox"))
            { Browser = "firefox"; }
            else if (Browser.ToLower().Contains("chrome"))
            { Browser = "chrome"; }
            else
                Browser = "IE";
            return Browser;
        }
        //public static bool UserTranscript(this IWebDriver driver)
        //{

        //    try
        //    {
        //        driver.GetElement(By.XPath("//*[@id='ctl00_SiteNavigationBar2_rdNavigationMenu']/ul/li[3]/a/span")).ClickWithSpace();
        //        Thread.Sleep(7000);

        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //    if (driver.Title == "Transcript")
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }

        //}
        public static int countelements(this IWebDriver driver, By by)
        {
            string result = string.Empty;
            int cnt = 0;
            try
            {
                //driver.WaitForElement(by);
                cnt = driver.FindElements(by).Count;
                return cnt;
            }
            catch (Exception ex)
            {
                return cnt;
            }
        }
        public static void SetDescription1(this IWebDriver driver, By htmleditor, By htmlcontrol, String desc)
        {
            //try
            //{


            //    if (driver.existsElement(htmleditor))
            //    {

            //        //driver.SwitchTo().DefaultContent();
            //        //driver.GetElement(htmleditor).ClickWithSpace();
            //        //driver.GetElement(htmleditor).SendKeysWithSpace(desc);
            //        driver.FindElement(By.XPath(".//*[@id='Editor']/div[2]/div/p"));
            //        driver.ClickEleJs(htmleditor);
            //        IWebElement a = driver.FindElement(htmleditor);

            //        ((IJavaScriptExecutor)driver).ExecuteScript("if($('.froala-view.froala-element.f-basic').length){$('.froala-view.froala-element.f-basic').removeClass('f-placeholder');}");
            //        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].innerHTML = '" + desc + "'", a);


            //    }
            //    else
            //    {
            //        driver.GetElement(htmlcontrol).SendKeys(desc);
            //    }


            //}
            //catch (Exception ex)
            //{
            //    ExceptionandLogging.ExceptionLogging(ex, Meridian_Common.CurrentTestName, "SetDescription", MethodBase.GetCurrentMethod().Name, "", driver);
            //}
        }
        //public static bool SelectFrame(this IWebDriver driver, By by)
        //{
        //    bool var = false;
        //    int j = 0;
        //    Exception finalex = null;
        //    for (j = 0; j <= 10; j++)
        //    {

        //        try
        //        {
        //            Thread.Sleep(4000);
        //            driver.SwitchTo().Frame(j);
        //            driver.WaitForElement(by);
        //            driver.GetElement(by);
        //            finalex = null;
        //            var = true;
        //            break;
        //            // driver.SwitchTo().DefaultContent();
        //        }
        //        catch (Exception ex)
        //        {
        //            finalex = ex;
        //            Console.WriteLine("Exception was raised on locating element: " + ex.Message);
        //            driver.SwitchTo().DefaultContent();
        //        }
                // }

          //  }
        //}
        //    if (finalex is NoSuchFrameException)
        //    {
        //        throw new NoSuchFrameException(j.ToString());
        //    }
        //    if (finalex is NoSuchElementException)
        //    {
        //        throw new NoSuchElementException(j.ToString());
        //    }

        //    return var;

        //}

        public static void SetDescription(this IWebDriver driver, String desc)
        {
            try
            {


                if (driver.existsElement(ObjectRepository.desc_htmleditor))
                {

                    //driver.SwitchTo().DefaultContent();
                    //driver.GetElement(htmleditor).ClickWithSpace();
                    //driver.GetElement(htmleditor).SendKeysWithSpace(desc);

                    driver.ClickEleJs(ObjectRepository.desc_htmleditor);
                    IWebElement a = driver.FindElement(ObjectRepository.desc_htmleditor);

                    ((IJavaScriptExecutor)driver).ExecuteScript("if($('.froala-view.froala-element.f-basic').length){$('.froala-view.froala-element.f-basic').removeClass('f-placeholder');}");
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].innerHTML = '" + desc + "'", a);


                }
                else
                {
                    driver.GetElement(ObjectRepository.desc_htmlcontrol).SendKeys(desc);
                }


            }
            catch (Exception ex)
            {
                ExceptionandLogging.ExceptionLogging(ex, Meridian_Common.CurrentTestName, "SetDescription", MethodBase.GetCurrentMethod().Name, "", driver);
            }
        }

        //public static void SelectFrame(this IWebDriver driver)
        //{
        //    int j = 0;
        //    Exception finalex = null;
        //    IList<IWebElement> iframes = driver.FindElements(By.TagName("iframe"));
        //    int noofframes = iframes.Count - 1;
        //    if (iframes.Count == 1)
        //    {
        //        noofframes = 0;
        //    }
        //    try
        //    {
        //        Thread.Sleep(3000);
        //        driver.SwitchTo().Frame(noofframes);
        //        finalex = null;
        //    }
        //    catch (Exception ex)
        //    {
        //        finalex = ex;
        //        Console.WriteLine("Exception was raised on locating element: " + ex.Message);
        //        driver.SwitchTo().DefaultContent();

        //    }

        //    if (finalex is NoSuchFrameException)
        //    {
        //        throw new NoSuchFrameException(j.ToString());
        //    }
        //    if (finalex is NoSuchElementException)
        //    {
        //        throw new NoSuchElementException(j.ToString());
        //    }

        //}
        public static void waitforframe(this IWebDriver driver, By elementtofind)
        {
           
            try
            {
               
                WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 20));
                wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(elementtofind));
            }
            catch(Exception ex)
            {
                
            }

        }

        //public static bool SelectFrame(this IWebDriver driver, By by)
        //{
        //    bool var = false;
        //    int j = 0;
        //    Exception finalex = null;
        //    driver.WaitForElement(by);
        //    for (j = 0; j <= 10; j++)
        //    {

        //        try
        //        {
        //            Thread.Sleep(5000);
        //            driver.SwitchTo().Frame(j);

        //            finalex = null;
        //            var = true;
        //            break;
        //            // driver.SwitchTo().DefaultContent();


        //        }
        //        catch (Exception ex)
        //        {
        //            finalex = ex;
        //            Console.WriteLine("Exception was raised on locating element: " + ex.Message);
        //            driver.SwitchTo().DefaultContent();

        //        }

        //    }

        //    if (finalex is NoSuchFrameException)
        //    {
        //        throw new NoSuchFrameException(j.ToString());
        //    }
        //    if (finalex is NoSuchElementException)
        //    {
        //        throw new NoSuchElementException(j.ToString());
        //    }

        //    return var;

        //}
        public static bool tempUserLogin(this IWebDriver driver, string type, string userid, string password, string browserstr)
        {
            try
            {
                Thread.Sleep(3000);
                driver.IsElementDisplayed_Generic_Login();
                if (driver.existsElement(ObjectRepository.main_login_button))
                    driver.ClickEleJs(ObjectRepository.main_login_button);

                switch (type)
                {

                    case "admin":
                        {
                            driver.WaitForElement(ObjectRepository.login_id);
                            driver.FindElement(ObjectRepository.login_id).Clear();
                            driver.FindElement(ObjectRepository.login_id).SendKeys(ExtractDataExcel.MasterDic_admin["Id"]);
                            driver.FindElement(ObjectRepository.login_password).SendKeys(ExtractDataExcel.MasterDic_admin["Password"]);
                            break;
                        }
                    case "admin1":
                        {
                            driver.WaitForElement(ObjectRepository.login_id);
                            driver.FindElement(ObjectRepository.login_id).Clear();
                            driver.FindElement(ObjectRepository.login_id).SendKeys(ExtractDataExcel.MasterDic_admin["Id"]);
                            driver.FindElement(ObjectRepository.login_password).SendKeys(ExtractDataExcel.MasterDic_admin["Password"]);
                            break;
                        }
                    //case "idpadmin":
                    //    {
                    //        driver.WaitForElement(ObjectRepository.login_id);
                    //        driver.GetElement(ObjectRepository.login_id).Clear();
                    //        driver.GetElement(ObjectRepository.login_id).SendKeys(ExtractDataExcel.MasterDic_idpadmin["Id"]+browserstr);
                    //        driver.GetElement(ObjectRepository.login_password).SendKeys(ExtractDataExcel.MasterDic_idpadmin["Password"]);
                    //        break;
                    //    }
                    case "user":
                        {
                            driver.IsElementDisplayed_Generic_Login();
                            //driver.WaitForElement(ObjectRepository.main_login_button);
                            //driver.FindElement(ObjectRepository.main_login_button).ClickWithSpace();
                            driver.WaitForElement(ObjectRepository.login_id_new);
                            driver.FindElement(ObjectRepository.login_id_new).Clear();
                            if (string.IsNullOrEmpty(userid))
                            {
                                driver.GetElement(ObjectRepository.login_id_new).SendKeys(ExtractDataExcel.MasterDic_newuser["Id"] + browserstr);
                            }
                            else
                            {
                                //driver.WaitForElement(ObjectRepository.main_login_button);
                                //driver.GetElement(ObjectRepository.main_login_button).ClickWithSpace();
                                //driver.WaitForElement(ObjectRepository.login_id_new);
                                driver.FindElement(ObjectRepository.login_id_new).SendKeysWithSpace(userid);
                                driver.WaitForElement(ObjectRepository.login_password_new);
                            }
                            driver.GetElement(ObjectRepository.login_password_new).SendKeysWithSpace(password);

                            break;
                        }
                    case "newuser":
                        {
                            /////////////////////////////////////////////////////////////
                            driver.IsElementDisplayed_Generic_Login();
                            driver.WaitForElement(ObjectRepository.main_login_button);
                            driver.FindElement(ObjectRepository.main_login_button).ClickWithSpace();
                            driver.WaitForElement(ObjectRepository.login_id_new);
                            driver.FindElement(ObjectRepository.login_id_new).Clear();
                            driver.FindElement(ObjectRepository.login_id_new).SendKeys(userid);
                            driver.WaitForElement(ObjectRepository.login_password_new);
                            driver.FindElement(ObjectRepository.login_password_new).SendKeys(password);
                            break;
                            ///////////////////////////////////////////////////////////////
                        }
                    case "userforregression":
                        {
                            /////////////////////////////////////////////////////////////
                            driver.IsElementDisplayed_Generic_Login();
                            driver.WaitForElement(ObjectRepository.main_login_button);
                            driver.FindElement(ObjectRepository.main_login_button).ClickWithSpace();
                            driver.WaitForElement(ObjectRepository.login_id_new);
                            driver.FindElement(ObjectRepository.login_id_new).Clear();
                            driver.FindElement(ObjectRepository.login_id_new).SendKeys(ExtractDataExcel.MasterDic_userforreg["Id"] + browserstr);
                            driver.WaitForElement(ObjectRepository.login_password_new);
                            driver.FindElement(ObjectRepository.login_password_new).SendKeys(password);
                            break;
                            ///////////////////////////////////////////////////////////////
                        }
                    case "userforregression1":
                        {
                            driver.FindElement(ObjectRepository.login_id).SendKeys(ExtractDataExcel.MasterDic_userforreg["Id"] + browserstr + 1);
                            driver.FindElement(ObjectRepository.login_password).SendKeys(password);
                            break;
                        }
                    case "sfuser":
                        {
                            driver.FindElement(ObjectRepository.login_id).SendKeys(userid);
                            driver.FindElement(ObjectRepository.login_password).SendKeys(password);
                            break;
                        }
                    case "instructor":
                        {
                            driver.IsElementDisplayed_Generic_Login();
                            driver.WaitForElement(ObjectRepository.main_login_button);
                            driver.FindElement(ObjectRepository.main_login_button).ClickWithSpace();
                            driver.WaitForElement(ObjectRepository.login_id_new);
                            driver.FindElement(ObjectRepository.login_id_new).Clear();
                            driver.FindElement(ObjectRepository.login_id_new).SendKeys(userid);
                            driver.FindElement(ObjectRepository.login_password_new).SendKeys(password);
                            break;
                        }
                    default:
                        {
                            driver.FindElement(ObjectRepository.login_id).SendKeys(ExtractDataExcel.MasterDic_admin["Id"]);
                            driver.FindElement(ObjectRepository.login_password).SendKeys(ExtractDataExcel.MasterDic_admin["Password"]);
                            break;
                        }

                }
                driver.ClickEleJs(ObjectRepository.signin_button_new);
                Thread.Sleep(5000);
                driver.WaitForElement(ObjectRepository.currentpassword);
                if (driver.Title.Contains("Edit Password"))
                {
                    driver.WaitForElement(ObjectRepository.currentpassword);
                    driver.FindElement(ObjectRepository.currentpassword).SendKeys(password);
                    driver.FindElement(ObjectRepository.editpassword).SendKeys(ExtractDataExcel.MasterDic_newuser["Password"]);
                    driver.FindElement(ObjectRepository.repeatpassword).SendKeys(ExtractDataExcel.MasterDic_newuser["Password"]);
                    driver.FindElement(ObjectRepository.saveeditpassword).ClickWithSpace();
                    Thread.Sleep(2000);
                    driver.WaitForElement(ObjectRepository.sucessmessage);

                }
                // Login.WaitForElement(By.XPath("/html/body/form/div[6]/div/div[2]/div/div/ul/li/a/span"));
                driver.WaitForElement(ObjectRepository.TrainingHome);
            }


            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            //return Login.GetElement(ObjectRepository.login_name)).Text;
            return true;
        }
        //public static void ClickWithSpace(this IWebDriver driver, By by)
        //{
        //    Thread.Sleep(2000);
        //    driver.GetElement(by).SendKeys("");
        //    driver.GetElement(by).ClickWithSpace();
        //    Thread.Sleep(2000);
        //}
        public static bool tempUserLogin1(this IWebDriver driver, string type, string userid, string password, string browserstr)
        {
            try
            {
                driver.WaitForElement(ObjectRepository.login_id);
                Thread.Sleep(2000);
                switch (type)
                {

                    case "admin":
                        {
                            driver.FindElement(ObjectRepository.login_id).SendKeys(ExtractDataExcel.MasterDic_admin["Id"]);
                            driver.FindElement(ObjectRepository.login_password).SendKeys(ExtractDataExcel.MasterDic_admin["Password"]);
                            break;
                        }
                    case "userforhelpdesk":
                        {
                            driver.FindElement(ObjectRepository.login_id).Clear();
                            driver.FindElement(ObjectRepository.login_id).SendKeys(ExtractDataExcel.MasterDic_userforhelpdesk["Id"] + browserstr);
                            driver.FindElement(ObjectRepository.login_password).SendKeys(password);
                            break;
                        }
                    case "userforhr":
                        {
                            driver.FindElement(ObjectRepository.login_id).Clear();
                            driver.FindElement(ObjectRepository.login_id).SendKeys(ExtractDataExcel.MasterDic_userforhr["Id"] + browserstr);
                            driver.FindElement(ObjectRepository.login_password).SendKeys(password);
                            break;
                        }

                    case "user":
                        {
                            driver.FindElement(ObjectRepository.login_id).SendKeys(ExtractDataExcel.MasterDic_newuser["Id"] + browserstr);
                            driver.FindElement(ObjectRepository.login_password).SendKeys(password);
                            break;
                        }
                    case "userforregression":
                        {
                            driver.FindElement(ObjectRepository.login_id).SendKeys(ExtractDataExcel.MasterDic_userforreg["Id"] + browserstr);
                            driver.FindElement(ObjectRepository.login_password).SendKeys(password);
                            break;
                        }
                    case "userforregression1":
                        {
                            driver.FindElement(ObjectRepository.login_id).SendKeys(ExtractDataExcel.MasterDic_userforreg["Id"] + browserstr + 1);
                            driver.FindElement(ObjectRepository.login_password).SendKeys(password);
                            break;
                        }
                    case "sfuser":
                        {
                            driver.FindElement(ObjectRepository.login_id).SendKeys(userid);
                            driver.FindElement(ObjectRepository.login_password).SendKeys(password);
                            break;
                        }
                    default:
                        {
                            driver.FindElement(ObjectRepository.login_id).SendKeys(ExtractDataExcel.MasterDic_admin["Id"]);
                            driver.FindElement(ObjectRepository.login_password).SendKeys(ExtractDataExcel.MasterDic_admin["Password"]);
                            break;
                        }

                }
                driver.FindElement(ObjectRepository.signin_button).ClickWithSpace();
                Thread.Sleep(5000);
                if (driver.Title.Contains("Edit Password"))
                {
                    driver.WaitForElement(ObjectRepository.currentpassword);
                    driver.GetElement(ObjectRepository.currentpassword).SendKeys(password);
                    driver.GetElement(ObjectRepository.editpassword).SendKeys(ExtractDataExcel.MasterDic_userforhr["Password"]);
                    driver.GetElement(ObjectRepository.repeatpassword).SendKeys(ExtractDataExcel.MasterDic_userforhr["Password"]);
                    driver.GetElement(ObjectRepository.saveeditpassword).ClickWithSpace();
                    driver.WaitForElement(ObjectRepository.sucessmessage);

                }
                // Login.WaitForElement(By.XPath("/html/body/form/div[6]/div/div[2]/div/div/ul/li/a/span"));
                driver.WaitForElement(ObjectRepository.TrainingHome);
            }


            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            //return Login.GetElement(ObjectRepository.login_name)).Text;
            return true;
        }
        //public static void SelectFrame(this IWebDriver driver)
        //{
        //    int j = 0;
        //    Exception finalex = null;
        //    for (j = 0; j <= 5; j++)
        //    {

        //        try
        //        {
        //            Thread.Sleep(3000);
        //            driver.SwitchTo().Frame(j);

        //            finalex = null;
        //            break;
        //            // driver.SwitchTo().DefaultContent();
        //        }
        //        catch (Exception ex)
        //        {
        //            finalex = e;
        //            Console.WriteLine("Exception was raised on locating element: " + e.Message);
        //            driver.SwitchTo().DefaultContent();

        //        }

        //    }

        //    if (finalex is NoSuchFrameException)
        //    {
        //        throw new NoSuchFrameException(j.ToString());
        //    }
        //    if (finalex is NoSuchElementException)
        //    {
        //        throw new NoSuchElementException(j.ToString());
        //    }



        //}
        // This is a basic wait for element not present a'la Selenium RC
        // but sharing the same timeout value as the driver
        public static void WaitForElementNotPresent(this IWebDriver driver, By bylocator)
        {
            int timeoutinteger = Common.DriverTimeout.Seconds;

            for (int second = 0; ; second++)
            {
                Thread.Sleep(3000);

                if (second >= timeoutinteger) Assert.Fail("Timeout: Element still visible at: " + bylocator);
                try
                {
                    IWebElement element = driver.GetElement(bylocator);
                }
                catch (NoSuchElementException)
                {
                    break;
                }
            }
        }
        //public static bool isPresentAndClick(this IWebDriver driver, By bylocator)
        //{
        //    bool variable = true;

        //    try
        //    {
        //        IWebElement element = driver.GetElement(bylocator);
        //        element.ClickWithSpace();
        //    }
        //    catch (NoSuchElementException) { variable = false; }
        //    return variable;
        //}// end of method
        public static bool Checkout(this IWebDriver driver)
        {

            int num = 0;
            bool val = false;
            if (driver.GetElement(By.Id("MainContent_header_FormView1_btnStatus")).Text=="Checkout")
            {
                while (num == 0)
                {
                    if (driver.GetElement(By.Id("MainContent_header_FormView1_btnStatus")).Enabled)
                    {
                        driver.ClickEleJs(By.Id("MainContent_header_FormView1_btnStatus"));
                        val = true;
                        break;
                    }
                    else
                    {
                        num = num + 1;
                        if (num == 10)
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                return val;
            }
            return val;

        }
        public static void select(this IWebDriver driver, By by, string text)
        {
            try
            {
                IWebElement select = driver.GetElement(by);
                IList<IWebElement> options = select.FindElements(By.TagName("option"));
                foreach (IWebElement option in options)
                {
                    if (option.Text.Contains(text))
                        option.Click();
                }
            }
            catch(StaleElementReferenceException)
            {

            }
            catch (NoSuchElementException)
            {
                by = null;

           
            }
        }
        public static bool selectOptionVisibility(this IWebDriver driver, By by, string text)
        {
            
                IWebElement select = driver.GetElement(by);
                IList<IWebElement> options = select.FindElements(By.TagName("li"));
                foreach (IWebElement option in options)
                {
                    if (option.Text.Contains(text))
                        return true;
                }
                return false;
           
        }
        public static bool WaitForPageLoad(this IWebDriver driver, By bylocator)
        {
            // int timeOut = 12000;
            bool var;
            var = true;
            try
            {
                // IWebElement element = driver.GetElement(bylocator);
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                //  WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeOut)).Until(ExpectedConditions.ElementExists(bylocator));
                //using lambda method

                wait.Until<IWebElement>(ExpectedConditions.ElementExists(bylocator));

                //     wait.Until<IWebElement>(ExpectedConditions.ElementIsVisible(bylocator));
                //   driver.GetElement(bylocator).SendKeys("");
            }
            catch (NoSuchElementException) { var = false; }
            catch (WebDriverTimeoutException) { var = false; }
            catch (TimeoutException) { var = false; }

            return var;
       }
        public static void WaitForElement(this IWebDriver driver, By bylocator)
        {
            // int timeOut = 12000;
            bool var;
            var = true;
           
                DefaultWait<IWebDriver> fluentWait = new DefaultWait<IWebDriver>(driver);
                fluentWait.Timeout = TimeSpan.FromSeconds(30);
                fluentWait.PollingInterval = TimeSpan.FromMilliseconds(250);
                fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
                IWebElement searchResult = fluentWait.Until(x => x.FindElement(bylocator));
          

           // return var;
        }

        public static void TakeScreenShot(this IWebDriver driver)
        {

            //// Create Screenshot folder
            //RelativeDirectory rd = new RelativeDirectory();
            //string createdFolderLocation = rd.Up(2) + "\\screenshot\\";

            //// Take the screenshot            
            //Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
            //string screenshot = ss.AsBase64EncodedString;
            //byte[] screenshotAsByteArray = ss.AsByteArray;
            ////string text = (MethodBase.GetCurrentMethod().DeclaringType).ToString();

            //StackTrace st = new StackTrace();
            //string text = st.GetFrame(1).GetMethod().Name;
            //StackFrame sf = st.GetFrame(0);

            //MethodBase currentMethodName = sf.GetMethod();
            //// Save the screenshot
            //string format = "Mdhmmyy";
            //ss.SaveAsFile(createdFolderLocation + text + DateTime.Now.ToString(format) + ".Jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
            //ss.ToString();

        }
        public static bool selectWindow(this IWebDriver driver, string Title)
        {
            bool variable = true;
            try
            {
                Thread.Sleep(5000);
                string BaseWindow = driver.CurrentWindowHandle;
                ReadOnlyCollection<string> handles = driver.WindowHandles;
                foreach (string handle in handles)
                {
                    if (handle != BaseWindow && handle != Meridian_Common.parentwdw)
                    {
                        driver.SwitchTo().Window(handle).Title.Equals(Title);
                        Thread.Sleep(5000);
                    }

                }

            }
            catch (NoSuchWindowException) { variable = false; }
            return variable;
        }// end of method

        public static bool selectWindow(this IWebDriver driver)
        {
            bool variable = true;
            try
            {
                Thread.Sleep(3000);
                string BaseWindow = driver.CurrentWindowHandle;
                ReadOnlyCollection<string> handles = driver.WindowHandles;
                foreach (string handle in handles)
                {
                    if (handle != BaseWindow)
                    {
                        driver.SwitchTo().Window(handle);
                        Thread.Sleep(5000);
                    }

                }

            }
            catch (NoSuchWindowException) { variable = false; }
            return variable;
        }
        public static IWebElement GetElementOnPage(this IWebDriver driver, By by)
        {
            RemoteWebElement element = (RemoteWebElement)driver.GetElement(by);
            var ele = element.LocationOnScreenOnceScrolledIntoView;
            return element;
        }
        public static bool isPresent(this IWebDriver driver, By bylocator)
        {
            bool variable = true;
            try
            {
                for (int x = 0; x < 3; x++)
                {
                    Thread.Sleep(500);
                    IWebElement element = driver.GetElement(bylocator);
                }
            }
            catch (NoSuchElementException)
            {
                variable = false;
                Assert.Fail("Timeout: No Such element found on page with locator " + bylocator);
            }
            return variable;
        }// end of method
        public static bool SelectWindowClose(this IWebDriver driver)
        {
            //int x;
            bool variable = true;
            try
            {
                string BaseWindow = driver.CurrentWindowHandle;
                ReadOnlyCollection<string> handles = driver.WindowHandles;
                foreach (string handle in handles)
                {
                    if (!handle.EndsWith(Meridian_Common.parentwdw))
                    {
                        //driver.SwitchTo().Window(handle);//.Title.Equals(Title);
                        Thread.Sleep(5000);
                        driver.Close();
                        Thread.Sleep(5000);
                        break;
                    }
                }
                driver.SwitchTo().Window(Meridian_Common.parentwdw);
            }
            catch (NoSuchWindowException) { variable = false; }
            return variable;
        }// end of method 

        public static bool SelectWindowClose1(this IWebDriver driver,string Title="")
        {
            //int x;
            bool variable = true;
            try
            {
                string BaseWindow = driver.CurrentWindowHandle;
                ReadOnlyCollection<string> handles = driver.WindowHandles;
                foreach (string handle in handles)
                {
                    if (handle != BaseWindow)
                    {
                        string tl = driver.SwitchTo().Window(handle).Title;
                        //driver.SwitchTo().Window(handle).Title.Equals(Title);
                        if (tl == Title)
                        {
                            Thread.Sleep(5000);
                            driver.Close();
                            Thread.Sleep(5000);
                            break;
                        }
                    }
                }
                driver.SwitchTo().Window(Meridian_Common.parentwdw);
            }
            catch (NoSuchWindowException) { variable = false; }
            return variable;
        }
        public static bool SelectWindowClose2(this IWebDriver driver, string WDWTitleCLose = "",string WDWTitleFocues="")
        {
            //int x;
            bool variable = true;
         
            try
            {
                string BaseWindow = Meridian_Common.parentwdw;
                ReadOnlyCollection<string> handles = driver.WindowHandles;
                int i = 0;
           
                foreach (string handle in handles)
                {
                    i = i + 1;
                    if (handle == BaseWindow||handle!=BaseWindow)
                    {
                       
                       
                      string tl = driver.SwitchTo().Window(handle).Title; 
                        //driver.SwitchTo().Window(handle).Title.Equals(Title);
                        if (tl==WDWTitleCLose && Meridian_Common.parentwdw!=handle)
                        {
                            
                            Thread.Sleep(5000);
                            driver.Close();
                            Thread.Sleep(5000);
                            break;
                        }
                    }
                }
                ReadOnlyCollection<string> handles1 = driver.WindowHandles;
                foreach (string handle in handles1)
                {
                    if (handle != BaseWindow||handle==BaseWindow)
                    {
                        string tl = driver.SwitchTo().Window(handle).Title;
                        //driver.SwitchTo().Window(handle).Title.Equals(Title);
                        if (tl == WDWTitleFocues||handle==Meridian_Common.parentwdw)
                        {
                            driver.SwitchTo().Window(handle);
                            break;
                        }
                        else if(handle== WDWTitleFocues)
                        {
                            driver.SwitchTo().Window(handle);
                        }

                    }
                }
               
            }
            catch (NoSuchWindowException) { variable = false; }
            return variable;
        }
        public static bool SelectWindowfocus(this IWebDriver driver, string WDWTitleFocues = "",params string[] wdwnames)
        {
            //int x;
            bool variable = true;
            Thread.Sleep(20000);
            try
            {
                string BaseWindow = driver.CurrentWindowHandle;
                ReadOnlyCollection<string> handles = driver.WindowHandles;
                int i = 0;

                foreach (string handle in handles)
                {
                    i = i + 1;
                    if (handle == BaseWindow || handle != BaseWindow)
                    {


                        string tl = driver.SwitchTo().Window(handle).Title;
                        var match = wdwnames.Where(p => p.Contains(tl)).FirstOrDefault();
                        //driver.SwitchTo().Window(handle).Title.Equals(Title);
                        if (match==null)
                        {

                            Thread.Sleep(5000);
                            driver.Close();
                            Thread.Sleep(5000);
                          //  break;
                        }
                    }
                }
                ReadOnlyCollection<string> handles1 = driver.WindowHandles;
                foreach (string handle in handles1)
                {
                    if (handle != BaseWindow || handle == BaseWindow)
                    {
                        string tl = driver.SwitchTo().Window(handle).Title;
                        //driver.SwitchTo().Window(handle).Title.Equals(Title);
                        if (tl == WDWTitleFocues)
                        {

                            break;
                        }
                    }
                }

            }
            catch (NoSuchWindowException) { variable = false; }
            return variable;
        }
        public static bool Checkin(this IWebDriver driver)
        {

            int num = 0;
            bool val = false;
            driver.WaitForElement(By.Id("MainContent_header_FormView1_btnStatus"));
            while (num == 0)
            {
                if (driver.GetElement(By.Id("MainContent_header_FormView1_btnStatus")).Text == "Check-in")
                {
                    driver.ClickEleJs(By.Id("MainContent_header_FormView1_btnStatus"));
                    val = true;
                    break;
                }
                else
                {
                    num = num + 1;
                    if (num == 10)
                    {
                        break;
                    }
                }
            }
            return val;

        }
        public static bool openadminconsolepage(this IWebDriver driver)
        {
            try
            {
                driver.GetElement(ObjectRepository.AdminTab).ClickAnchor();
                Thread.Sleep(7000);
                driver.selectWindow(ObjectRepository.AdminConsole);
                Thread.Sleep(2000);
                //driver.GetElement(By.XPath(ele.Admin_loc("document_link"))).ClickWithSpace();


            }
            catch (Exception ex)
            {
                return false;
            }
            if (driver.Title == "Administration Console")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //public static bool existsElement(this IWebDriver driver, String id)
        //{
        //    try
        //    {
        //        //  driver.WaitForElement(By.Id(id));
        //        driver.FindElement(By.Id(id));
        //    }
        //    catch (NoSuchElementException e)
        //    {
        //        return false;
        //    }
        //    return true;
        //}
        public static bool existsElement(this IWebDriver driver, By by)
        {
            try
            {
                Thread.Sleep(5000);
                if(driver.GetElement(by).Displayed)
                {
                    return true;
                }
            }
            catch (NoSuchElementException e)
            {
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        public static bool doubleClick(this IWebDriver driver, By bylocator)
        {
            bool variable = true;
            int x;
            try
            {
                IWebElement element = driver.GetElement(bylocator);
                element.ClickWithSpace();
                element.ClickWithSpace();
                Thread.Sleep(3000);
            }
            catch (NoSuchElementException) { variable = false; }
            return variable;
        }// e
        public static void findandacceptalert(this IWebDriver driver)
        {
            TimeSpan defaultTimeout = new TimeSpan(0, 0, 0, 10);
            var wait = new WebDriverWait(driver, defaultTimeout);
            Thread.Sleep(3000);
            IAlert alert = ExpectedConditions.AlertIsPresent().Invoke(driver);
            if (alert != null)
            {
                alert = wait.Until(drv => drv.SwitchTo().Alert());
                alert.Accept();
            }
            else
            {
                Console.WriteLine("alert not persent");
            }

        }
        //public static bool clicktraininghomelink(this IWebDriver driver)
        //{
        //    try
        //    {
        //        driver.GetElement(By.XPath("//span[contains(text(),'Training Home')]")).ClickWithSpace();
        //        driver.GetElement(ObjectRepository.CourseName_Ed);


        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //    if (driver.Title == "Training Home")
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }

        //}
        public static string searchcourseintraininghomepage(this IWebDriver driver, string input, string srchcriteria)
        {
            string actualresult = string.Empty;
            try
            {
                driver.WaitForElement(ObjectRepository.CourseName_Ed);
                driver.ClickEleJs(ObjectRepository.CourseName_Ed);
                //Thread.Sleep(3000);
                driver.GetElement(ObjectRepository.CourseName_Ed).SendKeysWithSpace(input);
                //Thread.Sleep(3000);
                //     driver.FindSelectElementnew(ObjectRepository.CourseName_Typ, srchcriteria);
                driver.GetElement(ObjectRepository.CourseName_Ed).SendKeys(Keys.Escape);
             //   driver.GetElement(By.XPath("//span[contains(.,'Search')]")).ClickWithSpace();
                //  Thread.Sleep(5000);
                driver.FindElement(By.XPath("//a[@onclick='ShowOverlay(); TrainingCatalogSearchRedirect(); return false;']")).ClickWithSpace();
                driver.WaitForElement(By.XPath("//a[contains(.,'"+input+"')]"));
               
                actualresult = driver.Title;
                return actualresult;
            }
            catch (Exception ex)
            {
                return actualresult;
            }


        }
        public static bool clicktableresult(this IWebDriver driver, string linktext)
        {
            bool actualresult = false;
            try
            {
                //driver.WaitForElement(ObjectRepository.DocumentResultTable);
                //driver.GetElement(ObjectRepository.DocumentResultTable).ClickWithSpace();
                driver.WaitForElement(By.XPath("//a[contains(text(),'" + linktext + "')]"));
                driver.ClickEleJs(By.XPath("//a[contains(text(),'" + linktext + "')]"));
               // driver.WaitForElement(ObjectRepository.TrainingHome);
                Thread.Sleep(3000);
                if (driver.Title.Contains(linktext))
                {
                    actualresult = true;
                }

            }
            catch (Exception ex)
            {
                driver.LogoutUser(ObjectRepository.LogoutHoverLink, ObjectRepository.HoverMainLink);

            }

            return actualresult;
        }
        public static string gettextofelement(this IWebDriver driver, By by)
        {
            string actualresult = string.Empty;
            try
            {
                driver.WaitForElement(by);
                actualresult = driver.GetElement(by).Text;
                return actualresult;
            }
            catch (Exception ex)
            {
                return "elemnt did not return the text";
            }


        }
        public static bool Returntextfromportlet(this IWebDriver driver, By by, string expectedresult)
        {
            try
            {
                IList<IWebElement> options = driver.FindElements(by);

                int cnt = options.Count;
                int k = -1;
                foreach (IWebElement option in options)
                {


                    string[] alltext = option.Text.Split();
                    foreach (string res in alltext)
                    {
                        if (res.Contains("Classroom_Course_0402445144instructor_sort"))
                        {
                            Assert.Pass("Strings matched");
                            //break;
                        }

                    }



                }
            }


            catch (Exception ex)
            {
                return false;
            }
            return false;
        }

        public static bool Returntextfrompage(this IWebDriver driver, By by, string expectedresult)
        {
            try
            {

                driver.GetElement(By.Id("MainContent_ucInstructorToolsSummary_Summary_lbAllMyTeaching")).ClickWithSpace();
                driver.WaitForElement(By.Id("MainContent_ucTabs_lbMyTeachingScheduleActive"));
                IList<IWebElement> options1 = driver.FindElements(by);

                foreach (IWebElement option1 in options1)
                {
                    string[] alltext1 = option1.Text.Split();
                    foreach (string res in alltext1)
                    {
                        if (res.Contains(expectedresult))
                        {
                            return true;
                            //break;
                        }

                    }


                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;



        }







        public static bool checktextispresent(this IWebDriver driver, By by)
        {
            bool var = true;
            try
            {

                driver.FindElement(by);

                return var;
            }
            catch (Exception ex)
            {
                return var = false;
            }


        }

        //public static string clicktableresult(this IWebDriver driver,string srchtex)
        //{
        //    By searchtext = By.XPath("//a[contains(text(),'" + srchtex + "')]");
        //    string actualresult = string.Empty;
        //    try
        //    {
        //        driver.WaitForElement(searchtext);
        //        driver.GetElement(searchtext).ClickWithSpace();
        //        driver.WaitForElement(ObjectRepository.TrainingHome);
        //        Thread.Sleep(3000);
        //        actualresult = driver.Title;
        //        return actualresult;
        //    }
        //    catch (Exception ex)
        //    {
        //        return actualresult;
        //    }


        //}

        public static bool UserLogin(this IWebDriver driver, string type, string browser, string customusername = "", string custompassword = "")
        {
            try
            {


                switch (type)
                {

                    case "admin":
                        {
                            /////////////////////////////////////////////////////////////
                            //driver.IsElementDisplayed_Generic_Login();
                            //driver.WaitForElement(ObjectRepository.main_login_button);
                            //driver.GetElement(ObjectRepository.main_login_button).ClickWithSpace();
                            // driver.IsElementDisplayed_Generic();
                            Meridian_Common.isadminlogin = true;
                            driver.WaitForElement(ObjectRepository.login_id_new);
                            driver.GetElement(ObjectRepository.login_id_new).Clear();


                            if (browser.Contains("ffbs"))
                            {
                                driver.GetElement(ObjectRepository.login_id_new).SendKeysWithSpace("regadminfirefox");
                            }
                            if (browser.Contains("chbs"))
                            {
                                driver.GetElement(ObjectRepository.login_id_new).SendKeysWithSpace("siteadmin");
                            }
                            if (browser.Contains("iebs"))
                            {
                                driver.GetElement(ObjectRepository.login_id_new).SendKeysWithSpace("regadminie");
                            }
                            if (browser.Contains("safari"))
                            {
                                driver.GetElement(ObjectRepository.login_id_new).SendKeysWithSpace("regadminsafari");
                            }
                            if (browser.Contains("edge"))
                            {
                                driver.GetElement(ObjectRepository.login_id_new).SendKeysWithSpace("regadminedge");
                            }
                            if (browser.Contains("anybrowser"))
                            {
                                driver.GetElement(ObjectRepository.login_id_new).SendKeysWithSpace(ExtractDataExcel.MasterDic_admin["Id"]);
                            }


                            driver.GetElement(ObjectRepository.login_password_new).SendKeysWithSpace(ExtractDataExcel.MasterDic_admin["Password"]);
                            break;
                            ///////////////////////////////////////////////////////////////


                            /*

                            driver.WaitForElement(ObjectRepository.login_id);
                            driver.GetElement(ObjectRepository.login_id).Clear();
                            driver.GetElement(ObjectRepository.login_id).SendKeys(ExtractDataExcel.MasterDic_admin["Id"]);
                            driver.GetElement(ObjectRepository.login_password).SendKeys(ExtractDataExcel.MasterDic_admin["Password"]);
                            break;
                             */
                        }
                    case "userforhelpdesk":
                        {
                            /////////////////////////////////////////////////////////////
                            //driver.IsElementDisplayed_Generic_Login();
                            //driver.WaitForElement(ObjectRepository.main_login_button);
                            //driver.GetElement(ObjectRepository.main_login_button).ClickWithSpace();
                            //driver.IsElementDisplayed_Generic();
                            driver.WaitForElement(ObjectRepository.login_id_new);
                            driver.GetElement(ObjectRepository.login_id_new).Clear();
                            // driver.GetElement(ObjectRepository.login_id_new).SendKeys(Meridian_Common.UserId);
                            if (browser.Contains("ffbs"))
                            {
                                driver.GetElement(ObjectRepository.login_id_new).SendKeysWithSpace("regadminfirefox");
                            }
                            if (browser.Contains("chbs"))
                            {
                                driver.GetElement(ObjectRepository.login_id_new).SendKeysWithSpace("siteadmin");
                            }
                            if (browser.Contains("iebs"))
                            {
                                driver.GetElement(ObjectRepository.login_id_new).SendKeysWithSpace(ExtractDataExcel.MasterDic_userforhelpdesk["Id"] + browser);
                            }
                            if (browser.Contains("safari"))
                            {
                                driver.GetElement(ObjectRepository.login_id_new).SendKeysWithSpace("regadminsafari");
                            }
                            if (browser.Contains("edge"))
                            {
                                driver.GetElement(ObjectRepository.login_id_new).SendKeysWithSpace("regadminedge");
                            }
                            if (browser.Contains("anybrowser"))
                            {
                                driver.GetElement(ObjectRepository.login_id_new).SendKeysWithSpace(ExtractDataExcel.MasterDic_userforhelpdesk["Id"] + browser);
                            }

                            driver.GetElement(ObjectRepository.login_password_new).SendKeys(ExtractDataExcel.MasterDic_admin1["Password"]);
                            break;
                            ///////////////////////////////////////////////////////////////


                            /*
                            driver.WaitForElement(ObjectRepository.login_id);
                            driver.GetElement(ObjectRepository.login_id).Clear();
                            driver.GetElement(ObjectRepository.login_id).SendKeys(ExtractDataExcel.MasterDic_admin1["Id"]);
                            driver.GetElement(ObjectRepository.login_password).SendKeys(ExtractDataExcel.MasterDic_admin1["Password"]);
                            break;
                             */
                        }
                    case "userforhr":
                        {
                            /////////////////////////////////////////////////////////////
                            //driver.IsElementDisplayed_Generic_Login();
                            //driver.WaitForElement(ObjectRepository.main_login_button);
                            //driver.GetElement(ObjectRepository.main_login_button).ClickWithSpace();
                            //driver.IsElementDisplayed_Generic();
                            driver.WaitForElement(ObjectRepository.login_id_new);
                            driver.GetElement(ObjectRepository.login_id_new).Clear();
                            // driver.GetElement(ObjectRepository.login_id_new).SendKeys(Meridian_Common.UserId);
                            if (browser.Contains("ffbs"))
                            {
                                driver.GetElement(ObjectRepository.login_id_new).SendKeysWithSpace("regadminfirefox");
                            }
                            if (browser.Contains("chbs"))
                            {
                                driver.GetElement(ObjectRepository.login_id_new).SendKeysWithSpace("siteadmin");
                            }
                            if (browser.Contains("iebs"))
                            {
                                driver.GetElement(ObjectRepository.login_id_new).SendKeysWithSpace(ExtractDataExcel.MasterDic_userforhr["Id"] + browser);
                            }
                            if (browser.Contains("safari"))
                            {
                                driver.GetElement(ObjectRepository.login_id_new).SendKeysWithSpace("regadminsafari");
                            }
                            if (browser.Contains("edge"))
                            {
                                driver.GetElement(ObjectRepository.login_id_new).SendKeysWithSpace("regadminedge");
                            }
                            if (browser.Contains("anybrowser"))
                            {
                                driver.GetElement(ObjectRepository.login_id_new).SendKeysWithSpace(ExtractDataExcel.MasterDic_userforhr["Id"] + browser);
                            }

                            driver.GetElement(ObjectRepository.login_password_new).SendKeys(ExtractDataExcel.MasterDic_admin1["Password"]);
                            break;
                            ///////////////////////////////////////////////////////////////


                            /*
                            driver.WaitForElement(ObjectRepository.login_id);
                            driver.GetElement(ObjectRepository.login_id).Clear();
                            driver.GetElement(ObjectRepository.login_id).SendKeys(ExtractDataExcel.MasterDic_admin1["Id"]);
                            driver.GetElement(ObjectRepository.login_password).SendKeys(ExtractDataExcel.MasterDic_admin1["Password"]);
                            break;
                             */
                        }
                    case "admin1":
                        {
                            /////////////////////////////////////////////////////////////
                            Meridian_Common.isadminlogin = true;
                            driver.IsElementDisplayed_Generic_Login();
                            if (driver.existsElement(ObjectRepository.main_login_button))
                            {
                                driver.ClickEleJs(ObjectRepository.main_login_button);
                            }
                            //driver.IsElementDisplayed_Generic();
                            if (!driver.existsElement(ObjectRepository.login_id_new))
                            {
                                driver.WaitForElement(ObjectRepository.login_id_new);
                                for (int i = 0; i < 30; i++)
                                    if (!driver.existsElement(ObjectRepository.login_id_new))
                                    {
                                        Thread.Sleep(1000);

                                    }
                                    else
                                    {
                                        break;
                                    }
                            }

                            //int n = 0;
                            //while (n == 0)
                            //    try
                            //    {
                            //        driver.FindElement(By.Id("username")).Click();

                            //        break;
                            //    }
                            //    catch (StaleElementReferenceException e)
                            //    {
                            //    }
                            driver.GetElement(ObjectRepository.login_id_new).Clear();
                            // driver.GetElement(ObjectRepository.login_id_new).SendKeys(Meridian_Common.UserId);
                            if (browser.Contains("ffbs"))
                            {
                                driver.FindElement(ObjectRepository.login_id_new).SendKeysWithSpace("regadminfirefox");
                            }
                            if (browser.Contains("chbs"))
                            {
                                driver.GetElement(ObjectRepository.login_id_new).SendKeysWithSpace("siteadmin");
                            }
                            if (browser.Contains("iebs"))
                            {
                                driver.GetElement(ObjectRepository.login_id_new).SendKeys("regadminie");
                            }
                            if (browser.Contains("safari"))
                            {
                                driver.GetElement(ObjectRepository.login_id_new).SendKeysWithSpace("regadminsafari");
                            }
                            if (browser.Contains("edge"))
                            {
                                driver.GetElement(ObjectRepository.login_id_new).SendKeysWithSpace("regadminedge");
                            }
                            if (browser.Contains("anybrowser"))
                            {

                                driver.GetElement(ObjectRepository.login_id_new).SendKeysWithSpace(ExtractDataExcel.MasterDic_admin1["Id"]);
                            }

                            driver.GetElement(ObjectRepository.login_password_new).SendKeys(ExtractDataExcel.MasterDic_admin1["Password"]);
                            break;
                            ///////////////////////////////////////////////////////////////


                            /*
                            driver.WaitForElement(ObjectRepository.login_id);
                            driver.GetElement(ObjectRepository.login_id).Clear();
                            driver.GetElement(ObjectRepository.login_id).SendKeys(ExtractDataExcel.MasterDic_admin1["Id"]);
                            driver.GetElement(ObjectRepository.login_password).SendKeys(ExtractDataExcel.MasterDic_admin1["Password"]);
                            break;
                             */
                        }

                    case "child1admin":
                        {
                            /////////////////////////////////////////////////////////////
                            //driver.IsElementDisplayed_Generic_Login();
                            //driver.WaitForElement(ObjectRepository.main_login_button);
                            //driver.GetElement(ObjectRepository.main_login_button).ClickWithSpace();
                            //driver.IsElementDisplayed_Generic();
                            driver.WaitForElement(ObjectRepository.login_id_new);
                            driver.GetElement(ObjectRepository.login_id_new).Clear();
                            driver.GetElement(ObjectRepository.login_id_new).SendKeys(ExtractDataExcel.MasterDic_child1admin["Id"]);
                            driver.GetElement(ObjectRepository.login_password_new).SendKeys(ExtractDataExcel.MasterDic_child1admin["Password"]);
                            break;
                            ///////////////////////////////////////////////////////////////


                            /*

                            driver.WaitForElement(ObjectRepository.login_id);
                            driver.GetElement(ObjectRepository.login_id).Clear();
                            driver.GetElement(ObjectRepository.login_id).SendKeys(ExtractDataExcel.MasterDic_child1admin["Id"]);
                            driver.GetElement(ObjectRepository.login_password).SendKeys(ExtractDataExcel.MasterDic_child1admin["Password"]);
                            break;
                             */
                        }

                    case "user":
                        {
                            /////////////////////////////////////////////////////////////
                            //driver.IsElementDisplayed_Generic_Login();
                            //driver.WaitForElement(ObjectRepository.main_login_button);
                            //driver.GetElement(ObjectRepository.main_login_button).ClickWithSpace();
                            //driver.IsElementDisplayed_Generic();
                            driver.WaitForElement(ObjectRepository.login_id_new);
                            driver.GetElement(ObjectRepository.login_id_new).Clear();
                            driver.GetElement(ObjectRepository.login_id_new).SendKeys(ExtractDataExcel.MasterDic_newuser["Id"] + browser);
                            driver.GetElement(ObjectRepository.login_password_new).SendKeys(ExtractDataExcel.MasterDic_newuser["Password"]);
                            break;
                            ///////////////////////////////////////////////////////////////


                            /*

                            driver.WaitForElement(ObjectRepository.login_id);
                            driver.GetElement(ObjectRepository.login_id).Clear();
                            driver.GetElement(ObjectRepository.login_id).SendKeys(ExtractDataExcel.MasterDic_newuser["Id"]+browserstr);
                            driver.GetElement(ObjectRepository.login_password).SendKeys(ExtractDataExcel.MasterDic_newuser["Password"]);
                            break;
                             */
                        }
                    case "custom":
                        {
                            /////////////////////////////////////////////////////////////
                            //driver.IsElementDisplayed_Generic_Login();
                            //driver.WaitForElement(ObjectRepository.main_login_button);
                            //driver.GetElement(ObjectRepository.main_login_button).ClickWithSpace();
                            //driver.IsElementDisplayed_Generic();
                            driver.WaitForElement(ObjectRepository.login_id_new);
                            driver.GetElement(ObjectRepository.login_id_new).Clear();
                            driver.GetElement(ObjectRepository.login_id_new).SendKeys(customusername);
                            driver.GetElement(ObjectRepository.login_password_new).SendKeys(custompassword);
                            break;
                            ///////////////////////////////////////////////////////////////


                            /*

                            driver.WaitForElement(ObjectRepository.login_id);
                            driver.GetElement(ObjectRepository.login_id).Clear();
                            driver.GetElement(ObjectRepository.login_id).SendKeys(customusername);
                            driver.GetElement(ObjectRepository.login_password).SendKeys(custompassword);
                            break;
                             */
                        }
                    case "testuser":
                        {

                            //driver.IsElementDisplayed_Generic_Login();
                            //driver.WaitForElement(ObjectRepository.main_login_button);
                            //driver.GetElement(ObjectRepository.main_login_button).ClickWithSpace();
                            //driver.IsElementDisplayed_Generic();
                            driver.WaitForElement(ObjectRepository.login_id_new);
                            driver.GetElement(ObjectRepository.login_id_new).Clear();
                            driver.GetElement(ObjectRepository.login_id_new).SendKeys(customusername);
                            driver.GetElement(ObjectRepository.login_password_new).SendKeys(custompassword);
                            break;

                        }
                    case "userforregression":
                        {
                            /////////////////////////////////////////////////////////////
                            //driver.IsElementDisplayed_Generic_Login();
                            //driver.WaitForElement(ObjectRepository.main_login_button);
                            //driver.GetElement(ObjectRepository.main_login_button).ClickWithSpace();
                            //driver.IsElementDisplayed_Generic();
                            driver.WaitForElement(ObjectRepository.login_id_new);
                            driver.GetElement(ObjectRepository.login_id_new).Clear();
                            driver.GetElement(ObjectRepository.login_id_new).SendKeys(ExtractDataExcel.MasterDic_userforreg["Id"] + browser);
                            driver.GetElement(ObjectRepository.login_password_new).SendKeys(ExtractDataExcel.MasterDic_userforreg["Password"]);
                            break;
                            ///////////////////////////////////////////////////////////////


                            /*

                            driver.WaitForElement(ObjectRepository.login_id);
                            driver.GetElement(ObjectRepository.login_id).Clear();
                            driver.GetElement(ObjectRepository.login_id).SendKeys(ExtractDataExcel.MasterDic_userforreg["Id"]+browserstr);
                            driver.GetElement(ObjectRepository.login_password).SendKeys(ExtractDataExcel.MasterDic_userforreg["Password"]);
                            break;
                             */
                        }
                    case "smokeuser":
                        {
                            /////////////////////////////////////////////////////////////
                            //driver.IsElementDisplayed_Generic_Login();
                            //driver.WaitForElement(ObjectRepository.main_login_button);
                            //driver.GetElement(ObjectRepository.main_login_button).ClickWithSpace();
                            //driver.IsElementDisplayed_Generic();
                            driver.WaitForElement(ObjectRepository.login_id_new);
                            driver.GetElement(ObjectRepository.login_id_new).Clear();
                            driver.GetElement(ObjectRepository.login_id_new).SendKeys(ExtractDataExcel.MasterDic_smokeuser["Id"] + browser);
                            driver.GetElement(ObjectRepository.login_password_new).SendKeys(ExtractDataExcel.MasterDic_smokeuser["Password"]);
                            break;
                            ///////////////////////////////////////////////////////////////


                            /*

                            driver.WaitForElement(ObjectRepository.login_id);
                            driver.GetElement(ObjectRepository.login_id).Clear();
                            driver.GetElement(ObjectRepository.login_id).SendKeys(ExtractDataExcel.MasterDic_smokeuser["Id"]+browserstr);
                            driver.GetElement(ObjectRepository.login_password).SendKeys(ExtractDataExcel.MasterDic_smokeuser["Password"]);
                            break;
                             */
                        }
                    case "Instructor":
                        {
                            /////////////////////////////////////////////////////////////
                            //driver.IsElementDisplayed_Generic_Login();
                            //driver.WaitForElement(ObjectRepository.main_login_button);
                            //driver.GetElement(ObjectRepository.main_login_button).ClickWithSpace();
                            //driver.IsElementDisplayed_Generic();
                            driver.WaitForElement(ObjectRepository.login_id_new);
                            driver.GetElement(ObjectRepository.login_id_new).Clear();
                            driver.GetElement(ObjectRepository.login_id_new).SendKeys(ExtractDataExcel.MasterDic_instructor["Id"] + browser);
                            driver.GetElement(ObjectRepository.login_password_new).SendKeys(ExtractDataExcel.MasterDic_instructor["Password"]);
                            break;
                            ///////////////////////////////////////////////////////////////


                            /*

                            driver.WaitForElement(ObjectRepository.login_id);
                            driver.GetElement(ObjectRepository.login_id).SendKeys(ExtractDataExcel.MasterDic_instructor["Id"]+browserstr);
                            driver.GetElement(ObjectRepository.login_password).SendKeys(ExtractDataExcel.MasterDic_instructor["Password"]);
                            break;
                             */
                        }
                    case "idpadmin":
                        {
                            /////////////////////////////////////////////////////////////
                            //driver.IsElementDisplayed_Generic_Login();
                            //driver.WaitForElement(ObjectRepository.main_login_button);
                            //driver.GetElement(ObjectRepository.main_login_button).ClickWithSpace();
                            //driver.IsElementDisplayed_Generic();
                            driver.WaitForElement(ObjectRepository.login_id_new);
                            driver.GetElement(ObjectRepository.login_id_new).Clear();
                            driver.GetElement(ObjectRepository.login_id_new).SendKeys(ExtractDataExcel.MasterDic_idpadmin["Id"] + browser);
                            //driver.GetElement(ObjectRepository.login_id_new).SendKeys("testidpadmin3");
                            driver.GetElement(ObjectRepository.login_password_new).SendKeys(ExtractDataExcel.MasterDic_idpadmin["Password"]);
                            break;
                            ///////////////////////////////////////////////////////////////


                            /*

                            driver.WaitForElement(ObjectRepository.login_id);
                            driver.GetElement(ObjectRepository.login_id).SendKeys(ExtractDataExcel.MasterDic_idpadmin["Id"]+browserstr);
                            driver.GetElement(ObjectRepository.login_password).SendKeys(ExtractDataExcel.MasterDic_idpadmin["Password"]);
                            break;
                             */
                        }
                    default:
                        {
                            Meridian_Common.isadminlogin = true;
                            /////////////////////////////////////////////////////////////
                            //driver.IsElementDisplayed_Generic_Login();
                            //driver.WaitForElement(ObjectRepository.main_login_button);
                            //driver.GetElement(ObjectRepository.main_login_button).ClickWithSpace();
                            //driver.IsElementDisplayed_Generic();
                            driver.WaitForElement(ObjectRepository.login_id_new);
                            driver.GetElement(ObjectRepository.login_id_new).Clear();
                            driver.GetElement(ObjectRepository.login_id_new).SendKeys(ExtractDataExcel.MasterDic_admin["Id"]);
                            driver.GetElement(ObjectRepository.login_password_new).SendKeys(ExtractDataExcel.MasterDic_admin["Password"]);
                            break;
                            ///////////////////////////////////////////////////////////////


                            /*

                            driver.WaitForElement(ObjectRepository.login_id);
                            driver.GetElement(ObjectRepository.login_id).Clear();
                            driver.GetElement(ObjectRepository.login_id).SendKeys(ExtractDataExcel.MasterDic_admin["Id"]);
                            driver.GetElement(ObjectRepository.login_password).SendKeys(ExtractDataExcel.MasterDic_admin["Password"]);
                            break;
                             */
                        }

                }

                driver.GetElement(ObjectRepository.signin_button_new).ClickWithSpace();
                //  driver.WaitForElement(By.XPath("//button[@aria-haspopup='true']"));
                if (driver.Title == "Edit Password")// (driver.WaitForTitle("Edit Password", new TimeSpan(0,0,30))) //changed for slow site response
                {
                    string pwdofuser = string.Empty;
                    if (type == "userforregression")
                    {
                        pwdofuser = ExtractDataExcel.MasterDic_userforreg["Password"];
                    }
                    else
                    {
                        pwdofuser = ExtractDataExcel.MasterDic_newuser["Password"];
                    }
                    if (type == "Instructor")
                    {
                        pwdofuser = ExtractDataExcel.MasterDic_admin1["Password"];
                    }
                    driver.WaitForElement(ObjectRepository.currentpassword);
                    driver.GetElement(ObjectRepository.currentpassword).SendKeys(pwdofuser);
                    driver.GetElement(ObjectRepository.editpassword).SendKeys(pwdofuser);
                    driver.GetElement(ObjectRepository.repeatpassword).SendKeys(pwdofuser);
                    driver.GetElement(ObjectRepository.saveeditpassword).ClickWithSpace();
                    driver.WaitForElement(ObjectRepository.sucessmessage);

                }

                if (type == "userforregression" && driver.Title != "Home")
                {

                    driver.Navigate().GoToUrl(Meridian_Common.MeridianTestSite);
                    UsersUtil userutt = new UsersUtil(driver);
                    userutt.CreateRegAccount(browser);

                }
                if (type == "testuser" && driver.Title != "Home")
                {

                    driver.Navigate().GoToUrl(Meridian_Common.MeridianTestSite);
                    UsersUtil userutt = new UsersUtil(driver);
                    userutt.CreateRegAccount(browser);

                }
                // Login.WaitForElement(By.XPath("/html/body/form/div[6]/div/div[2]/div/div/ul/li/a/span"));
                driver.WaitForElement(ObjectRepository.TrainingHome);
            }


            catch (Exception ex)
            {
                // ExceptionandLogging.ExceptionLogging(ex, Meridian_Common.CurrentTestName,"", MethodBase.GetCurrentMethod().Name, "", driver);
                //LogoutUser(driver, ObjectRepository.LogoutHoverLink, ObjectRepository.HoverMainLink);
                return false;
            }
            //return Login.GetElement(ObjectRepository.login_name)).Text;
            return true;
        }

        public static bool IsElementDisplayed_Generic(this IWebDriver driver,By by)
        {
            try
            {
                bool elementPresent = false;
                for (int i = 0; i < 2; i++)
                {
                    try { elementPresent = driver.FindElement(by).Displayed; }
                    catch (Exception) { }
                    if (elementPresent) { break; }
                    System.Threading.Thread.Sleep(500);
                }
                return elementPresent;
            }
            catch (Exception)
            { return false; }
        }

        public static bool IsElementDisplayed_Generic_Selectorg(this IWebDriver driverobj)
        {
            try
            {
                bool elementPresent = false;
                for (int i = 0; i < 10; i++)
                {
                    try { elementPresent = driverobj.FindElement(By.Id("MainContent_UC1_txtSearch")).Displayed; }
                    catch (Exception) { }
                    if (elementPresent) { break; }
                    System.Threading.Thread.Sleep(1000);
                }
                return elementPresent;
            }
            catch (Exception)
            { return false; }
        }

        public static bool IsElementDisplayed_Generic_Login(this IWebDriver driver)
        {
            try
            {
                bool elementPresent = false;
                for (int i = 0; i < 10; i++)
                {
                    try { elementPresent = driver.FindElement(ObjectRepository.main_login_button).Displayed; }
                    catch (Exception) { }
                    if (elementPresent) { break; }
                    System.Threading.Thread.Sleep(1000);
                }
                return elementPresent;
            }
            catch (Exception)
            { return false; }
        }
        public static void HoverLinkClick(this IWebDriver driver, By hoverlink,By linkclick)
        {
            if (GetBrwser(driver) == "IE")
            {
                try
                {
                    IWebElement a = driver.FindElement(hoverlink);
                    IWebElement b = driver.FindElement(linkclick);
                    String mouseOverScript = "if(document.createEvent){var evObj = document.createEvent('MouseEvents');evObj.initEvent('mouseover',true, false); arguments[0].dispatchEvent(evObj);} else if(document.createEventObject) { arguments[0].fireEvent('onmouseover');}";
                    ((IJavaScriptExecutor)driver).ExecuteScript(mouseOverScript, a);
                    Thread.Sleep(1000);
                    ((IJavaScriptExecutor)driver).ExecuteScript(mouseOverScript, b);
                    Thread.Sleep(1000);
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", b);


                }
                catch (Exception e)
                {
                    // TODO: handle exception
                }
            }
            else
            {
                IWebElement link = driver.GetElement(hoverlink);
                IWebElement linkli = driver.GetElement(linkclick);
                Actions action = new Actions(driver);
                Cursor.Position = new Point(0, 0);
                action.MoveToElement(link).Build().Perform();
                Thread.Sleep(2000);
                linkli.Click();

            }
        }

        public static void OpenToolbarItems(this IWebDriver driver, By hoverlink)
        {
            driver.WaitForElement(MainLink);
            IWebElement link = driver.GetElement(MainLink);
            link.ClickWithSpace();
            IWebElement linkli = driver.GetElement(hoverlink);
            
            Thread.Sleep(2000);
            linkli.ClickWithSpace();

        }
        public static void OrderClick(this IWebDriver driver, By OrdersHoverLink, By HoverMainLink)
        {
            driver.OpenToolbarItems(OrdersHoverLink);
            driver.WaitForElement(ObjectRepository.manageaccesskeys);
        }
        public static bool SelectFrame(this IWebDriver driver, By by)
        {
            bool var = false;
            int j = 0;
            Exception finalex = null;
            driver.WaitForElement(by);
            for (j = 0; j <= 10; j++)
            {

                try
                {
                    Thread.Sleep(5000);
                    driver.SwitchTo().Frame(j);

                    finalex = null;
                    var = true;
                    break;
                    // driver.SwitchTo().DefaultContent();


                }
                catch (Exception ex)
                {
                    finalex = ex;
                    Console.WriteLine("Exception was raised on locating element: " + ex.Message);
                    driver.SwitchTo().DefaultContent();

                }

            }

            if (finalex is NoSuchFrameException)
            {
                throw new NoSuchFrameException(j.ToString());
            }
            if (finalex is NoSuchElementException)
            {
                throw new NoSuchElementException(j.ToString());
            }

            return var;

        }
        //public static void OrderClick(this IWebDriver driver, By OrdersHoverLink, By HoverMainLink)
        //{
        //    driver.OpenToolbarItems(OrdersHoverLink);
        //    driver.WaitForElement(ObjectRepository.manageaccesskeys);

        //}

        public static void LogoutUser(this IWebDriver driver, By logouthoverlink, By HoverMainLink)
        {
            Meridian_Common.isadminlogin = false;
            driver.ClickEleJs(By.XPath("//a[@id='ph_avatar']"));
            driver.ClickEleJs(By.XPath("//a[contains(.,'Logout')]"));
            driver.WaitForElement(By.XPath("//input[@id='MainContent_UC5_btnLogin']"));
            driver.ClickEleJs(By.Id("MainContent_UC5_btnLogin"));
            //Thread.Sleep(5000);

            //string var = driver.Title;
            //switch (var)
            //{
            //    case "Training Home":
            //        {
            //            driver.SwitchTo().Window("");
            //            CheckLogout_SystemOptions(driver);
            //            driver.OpenToolbarItems(logouthoverlink);
            //            driver.WaitForElement(ObjectRepository.main_login_button);
            //            driver.GetElement(ObjectRepository.main_login_button).ClickWithSpace();
            //            break;
            //        }
            //    case "Home":
            //        {
            //            //  CheckLogout_SystemOptions(driver);
            //            driver.OpenToolbarItems(logouthoverlink);
            //            driver.WaitForElement(ObjectRepository.main_login_button);
            //            driver.GetElement(ObjectRepository.main_login_button).ClickWithSpace();
            //            break;
            //        }
            //    case "Transcript":
            //        {
            //            CheckLogout_SystemOptions(driver);
            //            driver.OpenToolbarItems(logouthoverlink);
            //            driver.WaitForElement(ObjectRepository.main_login_button);
            //            driver.GetElement(ObjectRepository.main_login_button).ClickWithSpace();
            //            break;
            //        }

            //    case "Required Training":
            //        {
            //            driver.Close();
            //            Thread.Sleep(2000);
            //            driver.SwitchTo().Window("");
            //            CheckLogout_SystemOptions(driver);
            //            driver.OpenToolbarItems(logouthoverlink);
            //            driver.WaitForElement(ObjectRepository.main_login_button);
            //            driver.GetElement(ObjectRepository.main_login_button).ClickWithSpace();
            //            break;
            //        }
            //    case "View All Attempts":
            //        {
            //            driver.Close();
            //            Thread.Sleep(2000);
            //            driver.SwitchTo().Window("");
            //            CheckLogout_SystemOptions(driver);
            //            driver.OpenToolbarItems(logouthoverlink);
            //            driver.WaitForElement(ObjectRepository.main_login_button);
            //            driver.GetElement(ObjectRepository.main_login_button).ClickWithSpace();
            //            break;
            //        }
            //    case "Required Training Console":
            //        {
            //            driver.Close();
            //            Thread.Sleep(2000);
            //            driver.SwitchTo().Window("");
            //            CheckLogout_SystemOptions(driver);
            //            driver.OpenToolbarItems(logouthoverlink);
            //            driver.WaitForElement(ObjectRepository.main_login_button);
            //            driver.GetElement(ObjectRepository.main_login_button).ClickWithSpace();
            //            break;
            //        }
            //    case "Roles":
            //        {
            //            //  driver.Close();
            //            Thread.Sleep(2000);
            //            driver.SelectWindowClose2("Roles", "Home");
            //            CheckLogout_SystemOptions(driver);
            //            driver.OpenToolbarItems(logouthoverlink);
            //            driver.WaitForElement(ObjectRepository.main_login_button);
            //            driver.GetElement(ObjectRepository.main_login_button).ClickWithSpace();
            //            break;
            //        }
            //    case "Schedule & Manage Sections":
            //        {

            //            CheckLogout_SystemOptions(driver);
            //            driver.OpenToolbarItems(logouthoverlink);
            //            driver.WaitForElement(ObjectRepository.main_login_button);
            //            driver.GetElement(ObjectRepository.main_login_button).ClickWithSpace();
            //            break;
            //        }
            //    case "Record Attendance, Status, and Scores":
            //        {
            //            CheckLogout_SystemOptions(driver);
            //            driver.OpenToolbarItems(logouthoverlink);
            //            driver.WaitForElement(ObjectRepository.main_login_button);
            //            driver.GetElement(ObjectRepository.main_login_button).ClickWithSpace();
            //            break;
            //        }
            //    case "My Responsibilities":
            //        {
            //            //driver.Close();
            //            //Thread.Sleep(2000);
            //            //driver.SwitchTo().Window("");
            //            //applogout.GetElement(By.Id(Object.logout_link)).ClickWithSpace();
            //            CheckLogout_SystemOptions(driver);
            //            driver.OpenToolbarItems(logouthoverlink);
            //            driver.WaitForElement(ObjectRepository.main_login_button);
            //            driver.GetElement(ObjectRepository.main_login_button).ClickWithSpace();
            //            break;
            //        }
            //    case "Documents":
            //        {
            //            driver.Close();
            //            Thread.Sleep(2000);
            //            driver.SwitchTo().Window("");
            //            CheckLogout_SystemOptions(driver);
            //            driver.OpenToolbarItems(logouthoverlink);
            //            driver.WaitForElement(ObjectRepository.main_login_button);
            //            driver.GetElement(ObjectRepository.main_login_button).ClickWithSpace();
            //            break;
            //        }
            //    case "General Course":
            //        {
            //            driver.Close();
            //            Thread.Sleep(2000);
            //            driver.SwitchTo().Window("");
            //            CheckLogout_SystemOptions(driver);
            //            driver.OpenToolbarItems(logouthoverlink);
            //            driver.WaitForElement(ObjectRepository.main_login_button);
            //            driver.GetElement(ObjectRepository.main_login_button).ClickWithSpace();
            //            break;
            //        }
            //    case "SCORM 1.2":
            //        {
            //            driver.Close();
            //            Thread.Sleep(2000);
            //            driver.SwitchTo().Window("");
            //            CheckLogout_SystemOptions(driver);
            //            driver.OpenToolbarItems(logouthoverlink);
            //            driver.WaitForElement(ObjectRepository.main_login_button);
            //            driver.GetElement(ObjectRepository.main_login_button).ClickWithSpace();
            //            break;
            //        }
            //    case "History":
            //        {
            //            driver.Close();
            //            Thread.Sleep(2000);
            //            driver.SwitchTo().Window("");
            //            CheckLogout_SystemOptions(driver);
            //            driver.OpenToolbarItems(logouthoverlink);
            //            driver.WaitForElement(ObjectRepository.main_login_button);
            //            driver.GetElement(ObjectRepository.main_login_button).ClickWithSpace();
            //            break;
            //        }
            //    case "Section Information":
            //        {
            //            driver.Close();
            //            Thread.Sleep(2000);
            //            driver.SwitchTo().Window("");
            //            CheckLogout_SystemOptions(driver);
            //            driver.OpenToolbarItems(logouthoverlink);
            //            driver.WaitForElement(ObjectRepository.main_login_button);
            //            driver.GetElement(ObjectRepository.main_login_button).ClickWithSpace();
            //            break;
            //        }
            //    default:
            //        {
            //            if (driver.existsElement(HoverMainLink))
            //            {
            //                CheckLogout_SystemOptions(driver);
            //                driver.OpenToolbarItems(By.XPath("//a[contains(.,'Logout')]"));
            //                driver.WaitForElement(ObjectRepository.main_login_button);
            //                driver.GetElement(ObjectRepository.main_login_button).ClickWithSpace();
            //            }
            //            else if (driver.existsElement(By.XPath("//a[contains(.,'Administer')]")))      //By.XPath("//a[contains(.,'Administration Console')]")
            //            {
            //                driver.Navigate_to_TrainingHome();
            //                //applogout.GetElement(By.Id(Object.logout_link)).ClickWithSpace();

            //                driver.Navigate().Refresh();
            //                driver.HoverLinkClick(By.XPath("//a[contains(.,'Learn')]"), By.XPath("//a[@href='/learnerpage.aspx']"));
            //                driver.OpenToolbarItems(By.XPath("//a[@class='dropdown-toggle firepath-matching-node']"));
            //                driver.WaitForElement(ObjectRepository.main_login_button);
            //                driver.GetElement(ObjectRepository.main_login_button).ClickWithSpace();
            //                break;
            //            }

            //            else
            //            {
            //                if (Meridian_Common.issmoketest)
            //                {
            //                    driver.Navigate().GoToUrl(Meridian_Common.SmokeTestSite);
            //                }
            //                else
            //                {
            //                    driver.Navigate().GoToUrl(Meridian_Common.MeridianTestSite);
            //                }

            //                driver.WaitForElement(ObjectRepository.main_login_button);
            //                driver.GetElement(ObjectRepository.main_login_button).ClickWithSpace();
            //                //this section will be deleted when logout bug is fixed
            //                //driver.UserLogin("admin1",browserstr);
            //                //driver.LogoutUser(ObjectRepository.LogoutHoverLink, ObjectRepository.HoverMainLink);
            //            }
            //            break;
            //        }
            //}





        }
        public static void checkdisplay(this IWebDriver driver,By by)
        {
            try{
                if (!driver.GetElement(by).Displayed)
                {
                    driver.Navigate().Refresh();
                }
            }
            catch(Exception ex)
            {
                driver.Navigate().Refresh();
            }

        }
        internal static void CheckLogout_SystemOptions(this IWebDriver driver)
        {
            //try
            //{
            //    if (!driver.IsElementVisible(By.Id("NavigationStrip1_lbUserView")))
            //    {
            //        driver.Navigate().Refresh();
            //    }
            //}
            //catch (Exception)
            //{
            //}
        }
        public static bool SwitchWindow(this IWebDriver driver, string title)
        {
            var currentWindow = driver.CurrentWindowHandle;
            Thread.Sleep(8000);
            var availableWindows = new List<string>(driver.WindowHandles);

            foreach (string w in availableWindows)
            {
                if (w != currentWindow)
                {

                    driver.SwitchTo().Window(w);
                    if (driver.Title.Contains(title))
                        return true;
                    else
                    {
                        driver.SwitchTo().Window(currentWindow);
                    }

                }
            }
            return false;
        }
        public static bool ElementNotPresent(this IWebDriver driver, By bylocator)
        {
            bool ispersent = false;
            int timeoutinteger = Common.DriverTimeout.Seconds;

            for (int second = 0; ; second++)
            {
                Thread.Sleep(500);

                // if (second >= timeoutinteger) //Assert.Fail("Timeout: Element still visible at: " + bylocator);
                try
                {
                    IWebElement element = driver.GetElement(bylocator);
                    break;
                }
                catch (Exception ex)
                {
                    if (ex is NoSuchElementException || ex is ElementNotVisibleException)
                    {
                        ispersent = true;
                        break;
                    }
                }

            }
            return ispersent;

        }

        public static void Navigate_to_TrainingHome(this IWebDriver driver)
        {
            try
            {
                Thread.Sleep(5000);
                if (driver.Title == "Object reference not set to an instance of an object.")
                {
                    driver.Navigate().GoToUrl(Meridian_Common.MeridianTestSite);
                    Meridian_Common.islog = true;
                }
                //string BaseWindow = driver.CurrentWindowHandle; code could be used for edge.
                //ReadOnlyCollection<string> handles = driver.WindowHandles;

                //if (handles.Count > 1)
                //{
                //    string text1 = driver.Title;
                //    foreach (string handle in handles)
                //    {
                //        Boolean a = driver.SwitchTo().Window(handle).Url.Contains(text1);
                //        if (a != true)
                //        {
                //            driver.SwitchTo().Window(handle);
                //            break;
                //        }
                //    }
                //}
                //string text = driver.Title;
                int i = 0;
                if (Meridian_Common.MeridianTestbrowser == "iebs" || Meridian_Common.MeridianTestbrowser == null || Meridian_Common.MeridianTestbrowser == "iexplore" || Meridian_Common.MeridianTestbrowser == "IE" || Meridian_Common.MeridianTestbrowser == "safari" || Meridian_Common.MeridianTestbrowser == "edge")
                {
                    
                    string childWindow = driver.CurrentWindowHandle;
                    ReadOnlyCollection<string> handles = driver.WindowHandles;
                    foreach (string handle in handles)
                    {
                        if (handle == childWindow&&handles.Count!=1||handle!=childWindow)
                        {
                          //  driver.SwitchTo().Window(handle);//.Title.Equals(Title);
                            Thread.Sleep(5000);
  
                            driver.Close();
                            Thread.Sleep(5000);
                            i = i + 2;
                            break;
                        }
                        i = i + 1;
                    }
                    ReadOnlyCollection<string> handles1 = driver.WindowHandles;
                    foreach (string handle in handles1)
                    {
                        if (i > 1)
                        {
                            driver.SwitchTo().Window(handle);
                         //   break;//.Title.Equals(Title);
                        }
                    }
                
                    
                 //   driver.SwitchtoDefaultContent();
                }
                else
                {
                    if (driver.existsElement(By.XPath(".//*[@id='utility-nav']/ul[1]/li/span")))
                    {
                        driver.Close();
                        Thread.Sleep(2000);
                        driver.SwitchtoDefaultContent();
                        driver.SwitchTo().Window("");
                        Thread.Sleep(2000);
                        //  driver.GetElement(By.XPath("//span[@class='fa fa-times']")).ClickWithSpace(); closed this line as it is being used in istraining home function


                    }
                    else
                    {
                        driver.WaitForElement(lnk_MyOwnLearning);
                        driver.GetElement(lnk_MyOwnLearning).ClickWithSpace();
                    }
                }
                driver.Navigate().Refresh();
                if(driver.existsElement(By.TagName("iframe")))
                {
                    driver.Navigate().Refresh();
                }
            }
            catch (Exception ex)
            {


            }
        }
        

        public static string findidbytext(this IWebDriver driver, string textofcontrol)
        {
            IList<IWebElement> list = driver.FindElements(By.XPath("//*[@id='ctl00_MainContent_ucScheduledReport_mgScheduledReports_ctl00']/tbody/tr/td"));
            string parentctr = string.Empty;
            for (int i = 0; i < list.Count; i++)
            {
                if (textofcontrol == list[i].Text)
                {
                    parentctr = list[i].FindElement(By.XPath("parent::*")).GetAttribute("id");
                    break;
                }

            }
            return parentctr;

        }
        public static bool WaitForTitle(this IWebDriver driver, string title, TimeSpan timeout)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, timeout);
                wait.Until((d) => { return d.Title == title; });
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void ScrollToCoordinated(this IWebDriver driver, string x, string y)
        {
            //IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
            //jse.ExecuteScript("window.scrollBy('"+x+"','"+y+"')", "");
        }
        public static string deleteclassroom(this IWebDriver driver, string title, string statusfilter)
        {
            try
            {
                //  driver.UserLogin("admin");
                driver.GetElement(ObjectRepository.myResponsibilities).ClickAnchor();
                driver.WaitForElement(ObjectRepository.searchHome);
                driver.GetElement(ObjectRepository.myresponsibilitiesmycontenttitlelink).ClickWithSpace();
                driver.WaitForElement(ObjectRepository.contentdeletecontentbutton);
                driver.GetElement(ObjectRepository.contentdeletecontentbutton).ClickWithSpace();
                Thread.Sleep(5000);
                driver.SwitchTo().Alert().Accept();
                Thread.Sleep(5000);
            }
            catch (Exception ex)
            {
                return "";
            }
            return driver.GetElement(ObjectRepository.sucessmessage).Text;

        }

        public static IWebElement GetParent(IWebElement e)
        {
            return e.FindElement(By.XPath(".."));
        }
        public static int getintegerfromstring(this IWebDriver driver, string text)
        {
            try
            {
                string resultString = Regex.Match(text, @"\d+").Value;
                int number = Int32.Parse(resultString);
                return number;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static void FindSelectElementnew(this IWebDriver driver, By bylocatorForSelectElement, String text)
        {
            try
            {
                IWebElement element = driver.GetElement(bylocatorForSelectElement);
                SelectElement selector = new SelectElement(element);
                selector.SelectByText(text);
                Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
            }
        }
        public static void getcomboitemselected(this IWebDriver driver, By elementby, int index)
        {
            try
            {


                IWebElement element = driver.GetElement(elementby);
                SelectElement selector = new SelectElement(element);
                selector.SelectByIndex(index);
                Thread.Sleep(2000);
            }
            catch(Exception ex)
            {

            }
        }
        public static void OpenToolbarItemsforElement(this IWebDriver driver, By hoverlink)
        {

            IWebElement link = driver.GetElement(HoverConfigLink);
            IWebElement linkli = driver.GetElement(hoverlink);
            Actions action = new Actions(driver);
            Cursor.Position = new Point(0, 0);
            action.MoveToElement(link).Build().Perform();
            Thread.Sleep(2000);
            linkli.ClickWithSpace();

        }

        public static void CaptureScreenshotandLog(this IWebDriver driver, string functionName, string errormsg, string logstacktrace)
        {
            if (Meridian_Common.islog == true)
            {
                return;
            }
            //Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();

            //         byte[] screenshotAsByteArray = ss.AsByteArray;
            //         string dirscreenshot = string.Empty;
            //         //string dirlog = string.Empty;
            //         #region Old Code
            //         string currenttimepath = String.Format("{0:yyyyMMddhhmmssff}", DateTime.Now);
            //         //dirPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName;
            //         //dirscreenshot = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            //         //dirlog = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            //         //dirscreenshot = dirscreenshot + "\\screenshot\\" + ExtractDataExcel.token_for_reg;
            //         //dirlog = dirlog + "\\log\\" + ExtractDataExcel.token_for_reg;

            //         //bool isExistsscreenshot = System.IO.Directory.Exists(dirscreenshot);
            //         //bool isExistslog = System.IO.Directory.Exists(dirlog);

            //         //if (!isExistsscreenshot)
            //         //{
            //         //    System.IO.Directory.CreateDirectory(dirscreenshot);
            //         //}
            //         //if (!isExistslog)
            //         //{
            //         //    System.IO.Directory.CreateDirectory(dirlog);
            //         //}
            //         functionName = Regex.Replace(functionName, "[^0-9a-zA-Z]+", "_");
            //         string screenshotpath = CreateBaseDirectory()[0] + "\\" + currenttimepath + "_" + functionName + ".jpeg";
            //         string dirlog = CreateBaseDirectory()[1];
            //         //screenshotpath = screenshotpath.Replace("(", "_");
            //         //screenshotpath = screenshotpath.Replace(")", "_");
            //         //screenshotpath = screenshotpath.Replace("\"", "");
            //         #endregion

            //         ss.SaveAsFile(screenshotpath, OpenQA.Selenium.ScreenshotImageFormat.Jpeg);

            //         ss.ToString();
            //         LogWrite(errormsg, logstacktrace, dirlog + "\\" + ExtractDataExcel.token_for_reg + ".html", functionName, screenshotpath);
            //         Meridian_Common.islog = true;            }
            //         // Take the screenshot  


            ;
        }
        public static string[] CreateBaseDirectory()
        {
            try
            {
                string[] paths = new string[2] { "", "" };
                string baseFolder = "C:\\AutomationLogs\\";
                string screenShotsFolder = baseFolder + "Screenshots\\" + ExtractDataExcel.token_for_reg;
                string logsFolder = baseFolder + "Logs\\" + ExtractDataExcel.token_for_reg;//ExtractDataExcel.token_for_reg
                if (!Directory.Exists(baseFolder)) { Directory.CreateDirectory(baseFolder); }
                if (Directory.Exists(baseFolder))
                {
                    Directory.CreateDirectory(baseFolder + "Screenshots\\");
                    Directory.CreateDirectory(baseFolder + "Logs\\");
                    Directory.CreateDirectory(screenShotsFolder);
                    Directory.CreateDirectory(logsFolder);
                }
                paths[0] = screenShotsFolder; paths[1] = logsFolder;
                return paths;
            }
            catch (Exception)
            { return null; }
        }
        public static void LogWrite(string logMessage, string logstacktrace, string path, string functionname, string screenshotpath)
        {
            try
            {
                var uri = new System.Uri(screenshotpath);
                var screenshotpathurl = uri.AbsoluteUri;
                StringBuilder builder = new StringBuilder();
                if (!File.Exists(path))
                {
                    FileStream fileStr = File.Create(path);
                    fileStr.Close();

                    builder.AppendLine("<!DOCTYPE > <html> 	<head>		<title>Log for Automation Script Executed</title><style>" +
                        @"table.gridtable {
	                    font-family: verdana,arial,sans-serif;
	                    font-size:11px;
	                    color:#333333;
	                    border-width: 1px;
	                    border-color: #666666;
	                    border-collapse: collapse;
                        width:100%
                    }
                    table.gridtable th {
	                    border-width: 1px;
	                    padding: 8px;
	                    border-style: solid;
	                    border-color: #666666;
	                    background-color: #dedede;
                    }
                    table.gridtable tr:nth-child(odd){  background: #ffffff; }
                    table.gridtable tr:nth-child(even){  background: #dae5f4; }
                    table.gridtable td {
	                    border-width: 1px;
	                    padding: 8px;
	                    border-style: solid;
	                    border-color: #666666;
                        width:25%,
                        word-wrap:break-word;
	                 
                    }" +
                         @"</style>	</head>	<body><h3>LOG History:" + DateTime.Now.ToLongTimeString() + "-" + DateTime.Now.ToLongDateString() + "</h3>" +
                       @"<table class='gridtable'><thead><tr><th >Function</th><th >Error Message</th><th >Stack Trace</th><th >ScreenShot</th></tr></thead>" +
                       @"<input type='hidden'/></table></body></html>");
                    File.WriteAllText(path, builder.ToString());
                }
                string htmlFile = File.ReadAllText(path);
                builder.Clear();
                functionname = functionname.Replace("--", " ");
                //builder.AppendLine("<table class='users'>");
                //builder.AppendLine("<tbody>");
                //builder.AppendLine("<tr><td><h4>Function</h4></td> <td><h4>Stack Trace</h4></td></tr>");
                builder.AppendLine("<tr><td>" + functionname + "</td> <td >" + logMessage + "</td><td >" + logstacktrace + "</td><td ><a href='" + screenshotpathurl + "' target='_blank'>" + screenshotpathurl + "</a></td></tr>");
                //builder.AppendLine("</tbody>");
                //builder.AppendLine("</table>");
                builder.AppendLine("<input type='hidden'/>");
                string newFile = htmlFile.Replace("<input type='hidden'/>", builder.ToString());

                File.WriteAllText(path, newFile);

            }

            catch (Exception ex)
            {

            }
            //try
            //{
            //    using (StreamWriter w = File.AppendText(path))
            //    {
            //        Log(logMessage,logstacktrace, w,functionname);
            //    }
            //}
            //catch (Exception ex)
            //{
            //}
        }
        public static bool isSorted(this IWebDriver driver, String[] arrayS)
        {
            int n = arrayS.Length;
            for (int i = 0; i < n - 1; ++i)
            {
                if (arrayS[i].CompareTo(arrayS[i + 1]) > 0)
                {
                    return false;
                }
            }
            return true;

        }
        public static void Click_MyOwnLearning(this IWebDriver driver)
        {
            try
            {
                driver.WaitForElement(lnk_MyOwnLearning);
                driver.GetElement(lnk_MyOwnLearning).ClickWithSpace();
            }
            catch (Exception ex)
            {


            }

        }
        public static bool IsElementVisible(IWebElement element)
        {
            try
            {

            return element.Displayed && element.Enabled;
        }
            catch (Exception)
            {
                return false;
            }
        }

        internal static bool VerifySortingFunctionalityExternalLearning(this IWebDriver driver, string sortingType)
        {
            try
            {
                bool result_Sorting = true;
                Thread.Sleep(4000);
                driver.GetElement(By.XPath("//a[contains(text(),'Started/Begins')]")).Click();
                Thread.Sleep(6000);
                List<List<string>> allDates = new List<List<string>>();
                List<IWebElement> ele = driver.FindElements(AllStartTranings).ToList();
                for (int i = 0; i < ele.Count(); i++)
                {
                    List<string> specificRecord = new List<string>();
                    specificRecord.Add(ele[i].FindElement(AllStartTranings_Months).Text);
                    specificRecord.Add(ele[i].FindElement(AllStartTranings_Date).Text);
                    allDates.Add(specificRecord);
                }

                for (int i = 0; i < allDates.Count - 1; i++)
                {
                    int monthInDigit = DateTime.ParseExact(allDates[i][0], "MMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                    int monthInDigitNext = DateTime.ParseExact(allDates[i + 1][0], "MMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                    //Assending
                    if (sortingType.ToLower().Equals("ascending"))
                    {
                        if (monthInDigit > monthInDigitNext) { result_Sorting = false; break; }
                        if (monthInDigit == monthInDigitNext)
                        {
                            if (Convert.ToInt32(allDates[i][1]) > Convert.ToInt32(allDates[i + 1][1])) { result_Sorting = false; break; }
                        }
                    }
                    else
                    {
                        if (monthInDigit < monthInDigitNext) { result_Sorting = false; break; }
                        if (monthInDigit == monthInDigitNext)
                        {
                            if (Convert.ToInt32(allDates[i][1]) < Convert.ToInt32(allDates[i + 1][1])) { result_Sorting = false; break; }
                        }
                    }

                }
                return result_Sorting;
            }
            catch (Exception)
            {
                return false;
            }
        }

        internal static void ClickOn_TraningType(this IWebDriver driver, By locator, string browserstr, string courseType)
        {
            try
            {
                List<IWebElement> allTranings = driver.FindElements(By.CssSelector("tr[id*='ctl00_MainContent_UC3_RadGrid1_']")).ToList();
                foreach (IWebElement item in allTranings)
                {
                    if (IsCourseDisplayed_CurrentTranings(driver, item, browserstr, courseType))
                    {
                        item.Click();
                        if (driver.gettextofelement(By.XPath(".//*[@id='ctl00_MainContent_UC3_RadGrid1_ctl00__0']/td[4]"))=="View" && courseType == "Document")

                        {
                            driver.ClickEleJs(By.XPath("//a[contains(text(),'Open Item')]"));
                        }
                        else if (driver.gettextofelement(By.XPath(".//*[@id='ctl00_MainContent_UC3_RadGrid1_ctl00__0']/td[4]"))=="Enroll" && courseType == "AICC")
                        {
                            driver.ClickEleJs(By.XPath("//a[contains(text(),'AICC')]"));
                        }
                        else if (driver.gettextofelement(By.XPath(".//*[@id='ctl00_MainContent_UC3_RadGrid1_ctl00__0']/td[4]"))=="Resume" && courseType == "AICC")
                        {
                            driver.ClickEleJs(By.XPath(".//*[@id='ctl00_MainContent_UC3_RadGrid1_ctl00__0']/td[4]/div/a"));
                        }
                        else if (driver.gettextofelement(By.XPath(".//*[@id='ctl00_MainContent_UC3_RadGrid1_ctl00__0']/td[4]")) == "Enroll" && courseType == "General Course")
                        {
                            driver.ClickEleJs(By.XPath(".//*[@id='ctl00_MainContent_UC3_RadGrid1_ctl00__0']/td[4]/div/a"));
                        }
                        else if (driver.gettextofelement(By.XPath(".//*[@id='ctl00_MainContent_UC3_RadGrid1_ctl00__0']/td[4]")).Contains("Open Item") && courseType == "General Course")
                        {
                            if (locator == By.LinkText("Open Item"))
                            {
                                driver.ClickEleJs(By.XPath(".//*[@id='ctl00_MainContent_UC3_RadGrid1_ctl00__0']/td[4]/div/a"));
                            }
                            else
                            {
                                driver.ClickEleJs(By.XPath(".//*[@id='ctl00_MainContent_UC3_RadGrid1_ctl00__0']/td[4]/div/a[2]"));
                            }
                            }
                        else if (driver.gettextofelement(By.XPath(".//*[@id='ctl00_MainContent_UC3_RadGrid1_ctl00__0']/td[4]")) == "Resume" && courseType == "General Course")
                        {
                            driver.ClickEleJs(By.XPath(".//*[@id='ctl00_MainContent_UC3_RadGrid1_ctl00__0']/td[4]/div/a"));
                        }
                        else if (driver.gettextofelement(By.XPath(".//*[@id='ctl00_MainContent_UC3_RadGrid1_ctl00__0']/td[4]")) == "Enroll" && courseType == "SCORM")
                        {
                            driver.ClickEleJs(By.XPath(".//*[@id='ctl00_MainContent_UC3_RadGrid1_ctl00__0']/td[4]/div/a"));
                        }
                        else if (driver.gettextofelement(By.XPath(".//*[@id='ctl00_MainContent_UC3_RadGrid1_ctl00__0']/td[4]")).Contains("Open Item") && courseType == "SCORM")
                        {
                            if (locator == By.LinkText("Open Item"))
                            {
                                driver.ClickEleJs(By.XPath(".//*[@id='ctl00_MainContent_UC3_RadGrid1_ctl00__0']/td[4]/div/a"));
                            }
                            else
                            {
                                driver.ClickEleJs(By.XPath(".//*[@id='ctl00_MainContent_UC3_RadGrid1_ctl00__0']/td[4]/div/a[2]"));
                            }
                        }
                        else if (driver.gettextofelement(By.XPath(".//*[@id='ctl00_MainContent_UC3_RadGrid1_ctl00__0']/td[4]")).Contains("Access Item") && courseType == "Curriculum")
                        {
                            if (locator == By.LinkText("Access Item"))
                            {
                                driver.ClickEleJs(By.XPath(".//*[@id='ctl00_MainContent_UC3_RadGrid1_ctl00__0']/td[4]/div/a"));
                            }
                            else
                            {
                                driver.ClickEleJs(By.XPath(".//*[@id='ctl00_MainContent_UC3_RadGrid1_ctl00__0']/td[4]/div/a[2]"));
                            }
                        }
                        else if (driver.gettextofelement(By.XPath(".//*[@id='ctl00_MainContent_UC3_RadGrid1_ctl00__0']/td[4]")) == "Resume" && courseType == "SCORM")
                        {
                            driver.ClickEleJs(By.XPath(".//*[@id='ctl00_MainContent_UC3_RadGrid1_ctl00__0']/td[4]/div/a"));
                        }
                        else
                        {
                            driver.ClickEleJs(By.XPath("//a[contains(text(),'View')]"));
                        }
                  
                        break;
                    }
                }
                Thread.Sleep(2000);
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal static void ClickOn_HideButton(this IWebDriver driver, By locator, string browserstr, string courseType,By locator_Hide)
        {
            List<IWebElement> allTranings = driver.FindElements(By.CssSelector("tr[id*='ctl00_MainContent_UC3_RadGrid1_']")).ToList();
            foreach (var item in allTranings)
            {
                if (IsCourseDisplayed_CurrentTranings(driver, item, browserstr, courseType))
                {
                    item.FindElement(locator).ClickWithSpace();
                    item.FindElement(locator_Hide).ClickWithSpace();
                    break;
                }
            }
        }
        internal static bool IsCourseDisplayed_CurrentTranings(this IWebDriver driver, IWebElement item, string browserstr, string courseType)
        {
            try
            {

                if (courseType.Equals("AICC"))
                {
                    if (item.Text.Contains(ExtractDataExcel.MasterDic_aicc["Title"] + browserstr))
                    { return true; }
                    else return false;
                }
                else if (courseType.Equals("General Course"))
                {
                    if (item.Text.Contains(ExtractDataExcel.MasterDic_genralcourse["Title"] + browserstr))
                    { return true; }
                    else return false;
                }
                else if (courseType.Equals("ClassRoom Course"))
                    if (item.Text.Contains(ExtractDataExcel.MasterDic_classrommcourse["Title"] + browserstr))
                    { return true; }
                    else return false;
                else if (courseType.Equals("Curriculum"))
                    if (item.Text.Contains(Variables.curriculumTitle + browserstr))
                    { return true; }
                    else return false;
                else if (courseType.Equals("OJT"))
                    if (item.Text.Contains(ExtractDataExcel.MasterDic_ojt["Title"] + browserstr))
                    { return true; }
                    else return false;
                else if (courseType.Equals("Document"))
                    if (item.Text.Contains(ExtractDataExcel.MasterDic_document["Title"] + browserstr))
                    { return true; }
                    else return false;
                else if (courseType.Equals("Blog"))
                    if (item.Text.Contains("testblog_" + ExtractDataExcel.token_for_reg + browserstr))
                    { return true; }//
                    else return false;
                else if (courseType.Equals("FAQ"))
                    if (item.Text.Contains(ExtractDataExcel.MasterDic_FAQ["Question"] + browserstr + "adminfaq1"))
                    { return true; }
                    else return false;
                else if (courseType.Equals("SiteSurvey"))
                    if (item.Text.Contains(ExtractDataExcel.MasterDic_Survey["Title"] + browserstr))
                    { return true; }
                    else return false;
                else if (courseType.Equals("CollaborationSpace"))
                    if (item.Text.Contains(ExtractDataExcel.MasterDic_CSpace["Title"] + browserstr))
                    { return true; }
                    else return false;
                else
                    if (item.Text.Contains(ExtractDataExcel.MasterDic_scrom["Title"] + browserstr))
                    { return true; }
                    else return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        internal static void IsCorrectButtonDisplayed_AfterAcction(this IWebDriver driver, string ButtonText, By locator, string browserstr, string courseType)
        {
            try
            {

                string buttonText = string.Empty;
                bool recfound = false;
                if (driver.existsElement(By.Id("lbPrint")))
                {
                    List<IWebElement> allTraningss = new List<IWebElement>();
                    allTraningss = driver.FindElements(allTranings).ToList();
                    foreach (var item in allTraningss)
                    {
                        if (IsCourseDisplayed_CurrentTranings(driver, item, browserstr, courseType))
                        {
                            buttonText = driver.gettextofelement(locator);
                            if (buttonText == "" || buttonText == "elemnt did not return the text")
                            {
                                buttonText = item.Text;
                            }
                            // buttonText = item.Text;
                            recfound = true;
                            break;
                        }
                    }

                    if (recfound == false)
                    {
                        if (driver.gettextofelement(By.XPath(".//*[@id='ctl00_MainContent_UC3_RadGrid1_ctl00']/thead/tr[1]/td/div/div/nav/ol/li[3]/div/strong")) == "of 2")
                        {
                            driver.GetElement(By.XPath(".//*[@id='ctl00_MainContent_UC3_RadGrid1_ctl00']/thead/tr[1]/td/div/div/nav/ol/li[4]/a")).ClickWithSpace();
                            List<IWebElement> allTraningss1 = new List<IWebElement>();
                            allTraningss1 = driver.FindElements(allTranings).ToList();
                            foreach (var item in allTraningss1)
                            {
                                if (IsCourseDisplayed_CurrentTranings(driver, item, browserstr, courseType))
                                {
                                    //buttonText = driver.gettextofelement(locator);
                                    buttonText = item.FindElement(locator).Text;
                                    recfound = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            Throws ElementNotVisibleException;
                        }
                    }
                }
                else
                {
                    if (driver.existsElement(By.Id("MainContent_ucPrimaryActions_FormView1_LaunchAttemptFirst")))
                    {
                        buttonText = "Open Item";
                    }
                    else if (driver.existsElement(By.Id("MainContent_ucPrimaryActions_FormView1_LaunchCourseAttemptExisting")))
                    {
                        buttonText = "Resume";
                    }
                    else
                    {
                        buttonText = "Enroll";
                    }
                }
                if (!buttonText.Contains(ButtonText)) { throw new Exception("Default Button Text is not proper in current traning section for " + courseType + " course. Text should be -" + ButtonText); }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static By lnk_MyOwnLearning = By.Id("NavigationStrip1_lbUserView");

        public static By HoverConfigLink = By.XPath("//img[@alt='Configure']");
       // public static By MainLink = By.XPath(".//*[@id='utility-nav']/ul[2]/li[3]/a");
        public static By MainLink = By.XPath("//span[contains(@data-bind,'NAME.substring(0, 1)')]");
        public static By AllStartTranings = By.CssSelector("div[id*='_phStartDate']");
        public static By AllStartTranings_Months = By.CssSelector("span[id*='_lblStartMonth']");
        public static By AllStartTranings_Date = By.CssSelector("span[id*='_lblStartDay']");
        public static By enrollButton = By.CssSelector("a[id='DefaultBtn']");
        public static By openItemButton = By.CssSelector("a[id='DefaultComboBtn']");
        public static By cancellEnrollButton = By.CssSelector("a[id='btnCancelEnroll']");
        public static By resumeButton = By.CssSelector("a[id='DefaultBtn']");
        public static By allTranings = By.CssSelector("tr[id*='ctl00_MainContent_UC3_RadGrid1_']");
        private static readonly string tl;
    }
}

using OpenQA.Selenium;
using Selenium2.Meridian.P1.MyResponsibilities.Training;
using System;
using System.Threading;
using OpenQA.Selenium.Support.UI;

namespace Selenium2.Meridian.Suite.Administration.AdministrationConsole
{
    internal class RecommendationsPage
    {
        public static ContentTagsSectionCommand ContentTagsSection
        {
            get { return new ContentTagsSectionCommand(); }
        }

        internal static bool? RecommendationsPageElementVerifications()
        {
            bool result = false;

            try
            {
                Driver.Instance.FindElement(By.XPath("//a[@id='recentlyAddedSort']"));
                Driver.Instance.FindElement(By.XPath("//a[@id='recommendByTagsSort']"));
                Driver.Instance.FindElement(By.XPath("//a[@id='recentlyAddedTimePeriod']"));
                Driver.Instance.FindElement(By.XPath("//button[@class='multiselect dropdown-toggle btn btn-default']"));
           //     Driver.Instance.FindElement(By.XPath("//a[contains(.,'Manage Tags')]"));
          //      Driver.Instance.FindElement(By.XPath("//div[@class='bootstrap-switch bootstrap-switch-wrapper bootstrap-switch-on bootstrap-switch-small bootstrap-switch-id-recentlyAdded bootstrap-switch-animate']"));
          //      Driver.Instance.FindElement(By.XPath("//div[@class='bootstrap-switch bootstrap-switch-wrapper bootstrap-switch-on bootstrap-switch-small bootstrap-switch-id-recommendByTags bootstrap-switch-animate']"));
            
                result = true;
            }
            catch
            {

            }

            return result;
        }

        internal static void ClickToggle_ContentTag_Enable()
        {
            bool s = Driver.Instance.GetElement(By.XPath("//label[contains(.,'Content Tags')]/following::div[2]")).GetAttribute("class").Contains("on");
            if (s)
            {


            }
            else
            {
                Driver.clickEleJs(By.XPath("//div[@id='panelSettings']/div[2]/div[3]/div[2]/div/div/span[3]"));
            }

            //string s = Driver.GetElement(By.XPath("//div[2]/div/div/span")).Text;
            //if (s == "Enabled")
            //{
            //    Driver.clickEleJs(By.XPath("html/body/div[4]/div/div[2]/div/div[3]/div[2]/div/div/span[1]"));
            //    Thread.Sleep(5000);
            //    Driver.clickEleJs(By.XPath("html/body/div[4]/div/div[2]/div/div[3]/div[2]/div/div/span[3]"));
            //    Thread.Sleep(5000);
            //}
        }

        internal static void ClickToggle_RecentlyAdded_Enable()
        {
            bool s = Driver.Instance.GetElement(By.XPath("//label[contains(.,'Recently Added')]/following::div[2]")).GetAttribute("class").Contains("on");
            if (s)
            {


            }
            else
            {
                Driver.clickEleJs(By.XPath("//div[@id='panelSettings']/div[2]/div[2]/div[2]/div/div/span[3]"));
            }
            //string s1 = Driver.GetElement(By.XPath("//div[2]/div/div/span")).Text;
            //if (s1 == "Enabled")
            //{
            //    Driver.GetElement(By.XPath("//div[@id='content']/div[2]/div/div[2]/div[2]/div/div/span[2]")).ClickWithSpace();
            //    Thread.Sleep(5000);
            //    Driver.GetElement(By.XPath("//div[@id='content']/div[2]/div/div[3]/div[2]/div/div/span[2]")).ClickWithSpace();
            //    Thread.Sleep(5000);
            //}
        }

        internal static bool? Verify_RecentlyAddedPortlet()
        {
            bool result = false;
            try
            {
                Driver.Instance.FindElement(By.XPath("//div[@id='container-recently-added']"));

                result = true;
            }
            catch
            {

            }

            return result;
        }


        internal static bool? Verify_ContentTagPortlet()
        {
            bool result = false;
            try
            {
                Driver.Instance.FindElement(By.XPath("//div[@id='container-recommend-by-interest']"));

                result = true;
            }
            catch
            {

            }

            return result;
        }

        internal static string ClickShortingType_For_RecentlyAdded()
        {
            string s;
            s =  Driver.Instance.FindElement(By.XPath("//a[@id='recentlyAddedSort']")).Text;
            Driver.clickEleJs(By.XPath("//a[@id='recentlyAddedSort']"));
            return s;
        }

        internal static string ChangeShortingType_For_RecentlyAdded(string s)
        {
            string statusfinal = String.Empty;
            Driver.Instance.FindElement(By.XPath("//select[contains(@class,'form-control')]")).Click();
            if (s=="most recent")
            {
                string status = Driver.Instance.FindElement(By.XPath("//select[@class='form-control']//option[@value='2']")).Text;
                Driver.Instance.FindElement(By.XPath("//select[@class='form-control']//option[@value='2']")).Click();
                statusfinal = status;
                
            } else if (s=="random")
            {
                string status = Driver.Instance.FindElement(By.XPath("//select[@class='form-control']//option[@value='1']")).Text;
                Driver.Instance.FindElement(By.XPath("//select[@class='form-control']//option[@value='1']")).Click();
                statusfinal = status;
                
            }

            return statusfinal;
        }

        internal static string ClickTimeDuration_For_RecentlyAdded()
        {
            string s = Driver.Instance.FindElement(By.XPath("//a[@id='recentlyAddedTimePeriod']")).Text;
            Driver.clickEleJs(By.XPath("//a[@id='recentlyAddedTimePeriod']"));
            return s;
        }

        internal static string ChangeTimeDuration_For_RecentlyAdded(string s)
        {
            string value = string.Empty;
            switch(s)
            {
                case "within the last week":
                    {
                        value = Driver.Instance.FindElement(By.XPath("//select/option[@value='2']")).Text;
                        Driver.Instance.FindElement(By.XPath("//select[@class='form-control']")).Click();
                        Driver.Instance.FindElement(By.XPath("//select/option[@value='2']")).Click();
                        break;
                    }
                case "within the last month":
                    {
                        value = Driver.Instance.FindElement(By.XPath("//select/option[@value='3']")).Text;
                        Driver.Instance.FindElement(By.XPath("//select[@class='form-control']")).Click();
                        Driver.Instance.FindElement(By.XPath("//select/option[@value='3']")).Click();
                        break;
                    }
                case "within the last 3 month":
                    {
                        value = Driver.Instance.FindElement(By.XPath("//select/option[@value='4']")).Text;
                        Driver.Instance.FindElement(By.XPath("//select[@class='form-control']")).Click();
                        Driver.Instance.FindElement(By.XPath("//select/option[@value='4']")).Click();
                        break;
                    }

                case "within the last 6 month":
                    {
                        value = Driver.Instance.FindElement(By.XPath("//select/option[@value='5']")).Text;
                        Driver.Instance.FindElement(By.XPath("//select[@class='form-control']")).Click();
                        Driver.Instance.FindElement(By.XPath("//select/option[@value='5']")).Click();
                        break;
                    }

                case "within the last year":
                    {
                        value = Driver.Instance.FindElement(By.XPath("//select/option[@value='6']")).Text;
                        Driver.Instance.FindElement(By.XPath("//select[@class='form-control']")).Click();
                        Driver.Instance.FindElement(By.XPath("//select/option[@value='6']")).Click();
                        break;
                    }

                case "within a custom date range":
                    {
                        value = Driver.Instance.FindElement(By.XPath("//select/option[@value='7']")).Text;
                        Driver.Instance.FindElement(By.XPath("//select[@class='form-control']")).Click();
                        Driver.Instance.FindElement(By.XPath("//select/option[@value='7']")).Click();
                        break;
                    }

                case "since the beginning of time":
                    {
                        value = Driver.Instance.FindElement(By.XPath("//select/option[@value='1']")).Text;
                        Driver.Instance.FindElement(By.XPath("//select[@class='form-control']")).Click();
                        Driver.Instance.FindElement(By.XPath("//select/option[@value='1']")).Click();
                        break;
                    }



            }

            return value;
            
        }

        internal static void selectMostRecent_RecentlyAdded()
        {
            CommonSection.Manage.Recommendations();
            if (Driver.Instance.IsElementVisible(By.XPath("//a[contains(text(),'random')]")))
            {
                Driver.clickEleJs(By.XPath("//a[@id='recentlyAddedSort']"));
                var selectElement = new SelectElement(Driver.Instance.GetElement(By.XPath("//select[@class='form-control']")));
                selectElement.SelectByText("most recent");
                var selectElelemt2 = new SelectElement(Driver.Instance.GetElement(By.XPath("//div[3]/div[3]/p[1]/span[1]/div[1]/form[1]/div[1]/div[1]/div[1]/select[1]")));
                selectElelemt2.SelectByText("most recent");

            }
            else
            {

            }
        }

        internal static bool? checkDomainDropdown()
        {
            bool result = false;
            try
            {
                Driver.Instance.IsElementVisible(By.XPath("//button[@title='Meridian Global - Core Domain']"));
                result = true;
            }catch(Exception e)
            {

            }
            return result;
        }

        internal static string ClickShortingType_For_ContentTags()
        {
            string s;
            s = Driver.Instance.FindElement(By.XPath("//a[@id='recommendByTagsSort']")).Text;
            Driver.clickEleJs(By.XPath("//a[@id='recommendByTagsSort']"));
            return s;
        }

        internal static string ChangeShortingType_For_ContentTags(string s)
        {
            string statusfinal = String.Empty;
            Driver.Instance.FindElement(By.XPath("//select[contains(@class,'form-control')]")).Click();
            if (s == "most recent")
            {
                string status = Driver.Instance.FindElement(By.XPath("//select[@class='form-control']//option[@value='2']")).Text;
                Driver.Instance.FindElement(By.XPath("//select[@class='form-control']//option[@value='2']")).Click();
                statusfinal = status;

            }
            else if (s == "random")
            {
                string status = Driver.Instance.FindElement(By.XPath("//select[@class='form-control']//option[@value='1']")).Text;
                Driver.Instance.FindElement(By.XPath("//select[@class='form-control']//option[@value='1']")).Click();
                statusfinal = status;

            }

            return statusfinal;
            
        }

        internal static void ClickDropdown_Of_SelectingContentType()
        {
            Driver.clickEleJs(By.XPath("//button[@class='multiselect dropdown-toggle btn btn-default']"));
        }

        internal static void SelectContents_Checkbox()
        {
            Driver.Instance.FindElement(By.XPath("//input[@value='ML.BASE.COURSEWARE.ONLINE.AICC']")).Click();
        }

        internal static void Disable_RecentlyAddedPortlet()
        {
            Driver.clickEleJs(By.XPath("//div[@id='panelSettings']/div[2]/div[2]/div[2]/div/div/span"));
        }

        internal static void Enable_RecentlyAddedPortlet()
        {
            Driver.Instance.FindElement(By.XPath("//div[@id='panelSettings']/div[2]/div[2]/div[2]/div/div/span[3]")).Click();
        }

        internal static void Disable_ContentTagPortlet()
        {
            bool s = Driver.Instance.GetElement(By.XPath("//label[contains(.,'Content Tags')]/following::div[2]")).GetAttribute("class").Contains("on");
            if (s)
            {
                Driver.clickEleJs(By.XPath("//div[@id='panelSettings']/div[2]/div[3]/div[2]/div/div/span"));
                Thread.Sleep(1000);
            }
            else
            {
                
            }
            
        }

        internal static void Enable_ContentTagPortlet()
        {
            bool s = Driver.Instance.GetElement(By.XPath("//label[contains(.,'Content Tags')]/following::div[2]")).GetAttribute("class").Contains("on");
            if (s)
            {

                
            }
            else
            {
                Driver.clickEleJs(By.XPath("//div[@id='panelSettings']/div[2]/div[3]/div[2]/div/div/span[3]"));
            }
            
        }

        internal static string GetSuccessMessage()
        {
            return Driver.getSuccessMessage();
        }
    }

    public class ContentTagsSectionCommand
    {
        internal void ClickDisabled()
        {
            Driver.Instance.WaitForElement(By.XPath("//div[@id='content']/div[2]/div/div[3]/div/label"));
            Driver.GetElement(By.XPath("//div[@id='content']/div[2]/div/div[3]/div[2]/div/div/span")).ClickWithSpace();

        }

        internal void ClickEnabled()
        {
            Thread.Sleep(2000);
            Driver.GetElement(By.XPath("//div[@id='content']/div[2]/div/div[3]/div[2]/div/div/span[3]")).ClickWithSpace();
        }
    }
}
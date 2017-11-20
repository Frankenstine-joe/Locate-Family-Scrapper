using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Locate_Family_Scrapper
{
    class Program
    {
        static void Main(string[] args)
        {

            #region Declarations

            // Template is like http://www.locatefamily.com/Street-Lists/Pakistan/index-741.html
            string LocateFamilyLinkBase = "http://www.locatefamily.com/Street-Lists/";
            string Country = "Pakistan";
            string PageNumber = "1";
            int TotalPageNumber = 5;
            string LocateFamilyLink = LocateFamilyLinkBase + Country + "/index-" + PageNumber + ".html";

            // DB Class
            ContextClass cc = new ContextClass();

            #endregion

            for (int i = 1; i < TotalPageNumber; i++)
            {
                // Dynamic URL
                LocateFamilyLink = LocateFamilyLinkBase + Country + "/index-" + i + ".html";

                #region Browser Instantiating

                // Instantiate chrome browser
                IWebDriver driver = new ChromeDriver();

                // Open something with web browser
                driver.Url = LocateFamilyLink;

                //Thread.Sleep(4000);

                #endregion

                #region Data Gathering and storing

                IList<IWebElement> AllTables = driver.FindElements(By.ClassName("table"));

                foreach (var tables in AllTables)
                {
                    //Console.WriteLine(item.Text);

                    IList<IWebElement> AllRows = tables.FindElements(By.TagName("tr"));

                    foreach (var rows in AllRows)
                    {
                        // Console.WriteLine(rows.Text);
                        int count = 0;

                        // Instantiate person
                        Person person = new Person();

                        IWebElement element = rows.FindElement(By.TagName("th"));

                        person.LocateFamilyNumber = element.Text;

                        IList<IWebElement> AllTableData = rows.FindElements(By.TagName("td"));

                        foreach (var TableData in AllTableData)
                        {
                            if (count == 0)
                            {
                                person.Name = TableData.Text;
                            }
                            else if (count == 1)
                            {
                                person.Address = TableData.Text;
                            }
                            else
                            {
                                person.PhoneNumber = TableData.Text;
                            }
                            Console.WriteLine(TableData.Text);
                            count++;
                        }

                        // Save to DB
                        cc.Persons.Add(person);
                        cc.SaveChanges();

                        Console.WriteLine("******************************");
                    }
                    //Console.WriteLine("******************************");
                }

                driver.Close();

                #endregion

            }
        }
    }
}

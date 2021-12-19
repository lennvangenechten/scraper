using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumIndeed
{
    class Program
    {
        static void Main(string[] args)
        {
            void Youtube()
            {
                string csvPath = "C:/Users/defaultuser0/source/repos/SeleniumScraper/YoutubeResults.csv";
                StringBuilder stringOutput = new StringBuilder();
                stringOutput.AppendLine("link;title;channel;views");
                // ask the user for a searchterm
                Console.WriteLine("Your searchterm:");
                // read the input from the user
                String searchterm = Console.ReadLine();
                // set a reference for the browser
                IWebDriver driver = new ChromeDriver();
                // go to youtube
                driver.Navigate().GoToUrl("https://www.youtube.com/");
                // find and click the button to accept cookies
                IWebElement cookiesAccept = driver.FindElement(By.XPath("//*[text()='Ik ga akkoord']"));
                cookiesAccept.Click();
                // find and enter the searchbox
                IWebElement searchbox = driver.FindElement(By.Name("search_query"));
                searchbox.Click();
                // add the user input in the textbox and enter
                searchbox.SendKeys(searchterm + Keys.Enter);
                // wait for 4 seconds
                Thread.Sleep(4000);
                // find the filter button
                IWebElement filterButton = driver.FindElement(By.XPath("//*[text()='Filters']"));
                filterButton.Click();
                Thread.Sleep(2000);
                // find the label which filters on recent videos and click
                IWebElement recentFilter = driver.FindElement(By.XPath("//*[@id='label']/yt-formatted-string"));
                recentFilter.Click();
                var videos = driver.FindElements(By.TagName("ytd-video-renderer"));
                for (int i = 0; i < 5; i++)
                {
                    // get the link to the video
                    String link = videos[i].FindElement(By.TagName("a")).GetAttribute("href");
                    // get the title of the video
                    String title = videos[i].FindElement(By.TagName("h3")).Text;
                    // get the channel name
                    String channel = videos[i].FindElement(By.Id("channel-info")).Text;
                    // get the amount of views
                    IWebElement Views = videos[i].FindElement(By.Id("metadata-line"));
                    String views = Views.FindElement(By.XPath("./span[1]")).Text;
                    // create a string with the values we just got
                    String videoData = string.Join(";", link) + ";" + string.Join(";", title) + ";" + string.Join(";", channel) + ";" + string.Join(";", views);
                    stringOutput.AppendLine(videoData);
                };
                File.WriteAllText(csvPath, stringOutput.ToString());
            }
            void Indeed()
            {
                String csvPath = "C:/Users/defaultuser0/source/repos/SeleniumScraper/IndeedResults.csv";
                StringBuilder stringOutput = new StringBuilder();
                stringOutput.AppendLine("job;company;location;link");
                // ask the user for a searchterm
                Console.WriteLine("What kind of job are you looking for?");
                // read the input from the user
                String searchterm = Console.ReadLine();
                // set a reference for the browser
                IWebDriver driver = new ChromeDriver();
                // go to indeed
                driver.Navigate().GoToUrl("https://be.indeed.com/?r=us");
                // locate and click the cookies accept button
                Thread.Sleep(2000);
                IWebElement cookiesAccept = driver.FindElement(By.Id("onetrust-accept-btn-handler"));
                cookiesAccept.Click();
                // locate and click the searchbar
                IWebElement searchbar = driver.FindElement(By.Name("q"));
                searchbar.Click();
                // enter the searchterm and search for it
                searchbar.SendKeys(searchterm + Keys.Enter);
                // locate and click the filter button
                IWebElement filterButton = driver.FindElement(By.Id("filter-dateposted"));
                filterButton.Click();
                // find and click the element to filter on the last 3 days
                IWebElement lastThreeDaysFilter = driver.FindElement(By.XPath("//*[text()='Afgelopen 3 dagen']"));
                lastThreeDaysFilter.Click();
                // find and click the button to close the pop up
                Thread.Sleep(2000);
                IWebElement closePopUp = driver.FindElement(By.ClassName("popover-x-button-close"));
                Thread.Sleep(2000);
                closePopUp.Click();
                // select the results
                var jobs = driver.FindElements(By.ClassName("result"));
                IWebElement nextPage = null;
                Thread.Sleep(2000);
                bool jobsAvailable = true;
                while (jobsAvailable == true)
                {
                    try
                    {
                        for (int i = 0; i < jobs.Count - 1; i++)
                        {
                            // get the title
                            IWebElement title = jobs[i].FindElement(By.ClassName("jobTitle"));
                            title = title.FindElement(By.XPath("./span"));
                            // get the company
                            IWebElement company = jobs[i].FindElement(By.ClassName("companyName"));
                            //get the location
                            IWebElement location = jobs[i].FindElement(By.ClassName("companyLocation"));
                            // get the link
                            String link = jobs[i].GetAttribute("href");
                            // make a string
                            String jobData = string.Join(";", title.Text) + ";" + string.Join(";", company.Text) + ";" + string.Join(";", location.Text) + ";" + string.Join(";", link);
                            stringOutput.AppendLine(jobData);
                        }
                        nextPage = driver.FindElement(By.CssSelector("[aria-label = 'Volgende']"));
                        nextPage.Click();
                        Thread.Sleep(1000);
                        jobs = driver.FindElements(By.ClassName("result"));
                    }
                    catch
                    {
                        File.WriteAllText(csvPath, stringOutput.ToString());
                        Thread.Sleep(20000);
                        jobsAvailable = false;
                    }
                }
            }
            void Weather()
            {
                string csvPath = "C:/Users/defaultuser0/source/repos/SeleniumScraper/WeatherResults.csv";
                StringBuilder stringOutput = new StringBuilder();
                stringOutput.AppendLine("date;weather;minimum;maximum");
                // ask the user for a searchterm
                Console.WriteLine("Location:");
                // read the input from the user
                String searchterm = Console.ReadLine();
                // set a reference for the browser
                IWebDriver driver = new ChromeDriver();
                // go to youtube
                driver.Navigate().GoToUrl("https://openweathermap.org/");
                Thread.Sleep(8000);
                // accept cookies
                IWebElement cookiesAccept = driver.FindElement(By.XPath("//*[text()='Allow all']"));
                Thread.Sleep(2000);
                cookiesAccept.Click();
                // look for the given city
                IWebElement searchbox = driver.FindElement(By.Name("q"));
                searchbox.Click();
                searchbox.SendKeys(searchterm + Keys.Enter);
                Thread.Sleep(2000);
                // go to the data for the next 10 days
                IWebElement selectCity = driver.FindElement(By.XPath("//*[@id='forecast_list_ul']/table/tbody/tr[1]/td[2]/b[1]/a"));
                selectCity.Click();
                Thread.Sleep(4000);
                IWebElement daysList = driver.FindElement(By.ClassName("day-list"));
                var days = daysList.FindElements(By.TagName("li"));
                for (int i = 0; i < days.Count - 1; i++)
                {
                    // get temperatures
                    IWebElement temperatures = days[i].FindElement(By.XPath("div/div/span"));
                    String temperaturesString = temperatures.Text;
                    String temperatureNumbers = new String(temperaturesString.Where(Char.IsDigit).ToArray());
                    // count the number of negative temperatures
                    int negatives = temperaturesString.Count(f => (f == '-'));
                    // select the weather description
                    IWebElement weather = days[i].FindElement(By.XPath("div/span"));
                    // select the date
                    IWebElement date = days[i].FindElement(By.XPath("span"));
                    // variables for the maximum and minimum 
                    String max = "";
                    String min = "";
                    // split the string into two temperatures
                    if (temperatureNumbers.Length == 2)
                    {
                        max = temperatureNumbers[0].ToString();
                        min = temperatureNumbers[1].ToString();
                    }
                    else if (temperatureNumbers.Length == 3)
                    {
                        max = temperatureNumbers.Substring(0, 2);
                        min = temperatureNumbers[2].ToString();
                    }
                    else
                    {
                        max = temperatureNumbers.Substring(0, 2);
                        min = temperatureNumbers.Substring(2, 2);
                    }
                    // add a dash in front of the negative
                    if (negatives == 1)
                    {
                        min = "-" + min;
                    }
                    else if (negatives == 2)
                    {
                        min = "-" + min;
                        max = "-" + max;
                    }
                    // create a string with the variables
                    String weatherData = string.Join(";", date.Text) + ";" + String.Join(";", weather.Text) + ";" + String.Join(";", min) + ";" + String.Join(";", max);
                    // append the new string to the output
                    stringOutput.AppendLine(weatherData);
                }
                // add the lines to the csv
                File.WriteAllText(csvPath, stringOutput.ToString());
            }
            String choice = "";
            while (true)
            {
                Console.WriteLine("What would you like to scrape? (youtube, indeed or weather)");
                choice = Console.ReadLine();
                if (choice == "youtube" || choice == "indeed" || choice == "weather")
                {
                    break;
                }
            }
            Console.WriteLine("\n");
            if (choice == "youtube")
            {
                Youtube();
            }
            else if (choice.ToLower() == "indeed")
            {
                Indeed();
            }
            else
            {
                Weather();
            }
        }
    }
}
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Threading.Tasks;

namespace traderbot
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(50, 10);
            var profileManager = new FirefoxProfileManager();
            FirefoxProfile profile = profileManager.GetProfile("default-release");
            var options = new FirefoxOptions();
            options.Profile = profile;
            IWebDriver driver = new FirefoxDriver(options);
            driver.Navigate().GoToUrl("https://www.tradingview.com/chart/lQPStCCx/#"); //live-use
            //driver.Navigate().GoToUrl("https://www.tradingview.com/chart/dl7UKhYQ/"); //test-use

            int tp = 10;
            int sl = 40;

            int fetchRate = 200;

            string currentSpread = "";

            string alert = "";

            string time = "";

            Task alertChecker = new Task(t =>
            {
                while (true)
                {
                    //read alert
                    if (alert.Equals("sltp") || alert.Equals(""))
                    {
                        try
                        {
                            By byXpath = By.XPath("//div[contains(@class,'secondaryRow-9x4llTtJ')]");
                            alert = driver.FindElement(byXpath).Text;
                        }
                        catch (Exception)
                        {
                        }
                    }

                    //reconnect if have to
                    try
                    {
                        By byXpath = By.XPath("//span[contains(@class,'content-p9ma7wH8')]");
                        driver.FindElement(byXpath).Click();
                        Console.WriteLine("Reconnected");
                    }
                    catch (Exception)
                    {
                    }

                    //exchange time
                    try
                    {
                        By byXpath = By.XPath("//span[contains(@class,'inner-WhrIKIq9')]");
                        time = driver.FindElement(byXpath).Text;
                    }
                    catch (Exception)
                    {
                        time = "couldn't fetch time.";
                    }

                    //current spread
                    try
                    {
                        By byXpath = By.XPath("//div[contains(@class,'apply-common-tooltip spread-LlcFLhWF')]");
                        currentSpread = driver.FindElement(byXpath).Text;
                    }
                    catch (Exception)
                    {
                        currentSpread = "couldn't fetch spread.";
                    }

                    Console.Clear();
                    Console.WriteLine($"ExchangeTime: {time}");
                    Console.WriteLine($"CurrentSpread: {currentSpread}");
                    Console.WriteLine($"Alert: {alert}");
                    System.Threading.Thread.Sleep(fetchRate);
                }
            }, TaskCreationOptions.LongRunning);

            Task order = new Task(t =>
            {
                bool foundElement = false;
                while (true)
                {
                    if (alert.Equals("buy") || alert.Equals("sell"))
                    {
                        //close alert
                        try
                        {
                            By byXpath = By.XPath("//button[contains(@class,'button-YKkCvwjV size-small-YKkCvwjV color-brand-YKkCvwjV variant-primary-YKkCvwjV')]");
                            driver.FindElements(byXpath)[1].Click();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }

                        //close previous trade
                        try
                        {
                            By byXpath = By.XPath("//div[contains(@class,'icon-R6hgQk56 button-9pA37sIi apply-common-tooltip isInteractive-9pA37sIi newStyles-9pA37sIi')]");
                            driver.FindElements(byXpath)[1].Click();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }

                        //buy
                        foundElement = false;
                        if (alert.Equals("buy"))
                        {
                            while (!foundElement)
                            {
                                try
                                {
                                    By byXpath = By.XPath("//div[contains(@class,'apply-common-tooltip button-LlcFLhWF buyButton-LlcFLhWF')]");
                                    driver.FindElements(byXpath)[0].Click();
                                    foundElement = true;
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                    System.Threading.Thread.Sleep(fetchRate / 2);
                                }
                            }
                        }

                        //sell
                        foundElement = false;
                        if (alert.Equals("sell"))
                        {
                            while (!foundElement)
                            {
                                try
                                {
                                    By byXpath = By.XPath("//div[contains(@class,'apply-common-tooltip button-LlcFLhWF sellButton-LlcFLhWF')]");
                                    driver.FindElements(byXpath)[0].Click();
                                    foundElement = true;
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                    System.Threading.Thread.Sleep(fetchRate / 2);
                                }
                            }
                        }

                        //click on set tp/sl
                        foundElement = false;
                        while (!foundElement)
                        {
                            try
                            {
                                By byXpath = By.XPath("//div[contains(@class,'icon-R6hgQk56 button-9pA37sIi apply-common-tooltip isInteractive-9pA37sIi newStyles-9pA37sIi')]");
                                driver.FindElements(byXpath)[0].Click();
                                foundElement = true;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                System.Threading.Thread.Sleep(fetchRate / 2);
                            }
                        }

                        //click on the left box
                        foundElement = false;
                        while (!foundElement)
                        {
                            try
                            {
                                By byXpath = By.XPath("//span[contains(@class,'box-5Xd5conM check-5Xd5conM noOutline-5Xd5conM')]");
                                driver.FindElements(byXpath)[0].Click();
                                foundElement = true;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                System.Threading.Thread.Sleep(fetchRate / 2);
                            }
                        }

                        //click on the right box
                        foundElement = false;
                        while (!foundElement)
                        {
                            try
                            {
                                By byXpath = By.XPath("//span[contains(@class,'box-5Xd5conM check-5Xd5conM noOutline-5Xd5conM')]");
                                driver.FindElements(byXpath)[1].Click();
                                foundElement = true;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                System.Threading.Thread.Sleep(fetchRate / 2);
                            }
                        }

                        //set left pip
                        foundElement = false;
                        while (!foundElement)
                        {
                            try
                            {
                                By byXpath = By.XPath("//input[contains(@class,'input-uGWFLwEy with-end-slot-uGWFLwEy')]");
                                driver.FindElements(byXpath)[2].SendKeys(tp.ToString());
                                foundElement = true;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                System.Threading.Thread.Sleep(fetchRate / 2);
                            }
                        }

                        //set right pip
                        foundElement = false;
                        while (!foundElement)
                        {
                            try
                            {
                                By byXpath = By.XPath("//input[contains(@class,'input-uGWFLwEy with-end-slot-uGWFLwEy')]");
                                driver.FindElements(byXpath)[6].SendKeys(sl.ToString());
                                foundElement = true;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                System.Threading.Thread.Sleep(fetchRate / 2);
                            }
                        }

                        //press modify button
                        foundElement = false;
                        while (!foundElement)
                        {
                            //blue modify
                            try
                            {
                                By byXpath = By.XPath("//button[contains(@class,'button-AUtedaek buy-AUtedaek popup-AUtedaek')]");
                                driver.FindElements(byXpath)[0].Click();
                                foundElement = true;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                System.Threading.Thread.Sleep(fetchRate / 2);
                            }

                            //red modify
                            try
                            {
                                By byXpath = By.XPath("//button[contains(@class,'button-AUtedaek sell-AUtedaek popup-AUtedaek')]");
                                driver.FindElements(byXpath)[0].Click();
                                foundElement = true;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                System.Threading.Thread.Sleep(fetchRate / 2);
                            }
                        }

                        alert = "";
                    }
                    System.Threading.Thread.Sleep(fetchRate);
                }
            }, TaskCreationOptions.LongRunning);


            order.Start();
            alertChecker.Start();

            while (true)
            {
                System.Threading.Thread.Sleep(1000);
            }

            driver.Dispose();

        }
    }
}

using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System.Threading.Tasks;

namespace traderbot
{
    class Program
    {
        static void Main(string[] args)
        {
            var profileManager = new FirefoxProfileManager();
            FirefoxProfile profile = profileManager.GetProfile("default-release");
            var options = new FirefoxOptions();
            options.Profile = profile;
            IWebDriver driver = new FirefoxDriver(options);
            driver.Navigate().GoToUrl("https://www.tradingview.com/chart/lQPStCCx/#");

            int tp = 10;
            int sl = 40;

            string alert = "";

            Task alertChecker = new Task(t =>
            {

                while (true)
                {
                    //read alert
                    if (alert.Equals("sltp") || alert.Equals(""))
                    {
                        Console.Clear();
                        try
                        {
                            By byXpath = By.XPath("//div[contains(@class,'secondaryRow-9x4llTtJ')]");
                            alert = driver.FindElement(byXpath).Text;
                        }
                        catch (Exception e)
                        {
                        }
                    }
                    Console.WriteLine($"alert msg: {alert}");
                    Console.WriteLine("press q to quit");
                    System.Threading.Thread.Sleep(200);
                }
            }, TaskCreationOptions.LongRunning);


            Task order = new Task(t =>
            {

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
                        if (alert.Equals("buy"))
                        {
                            try
                            {
                                By byXpath = By.XPath("//div[contains(@class,'apply-common-tooltip button-LlcFLhWF buyButton-LlcFLhWF')]");
                                driver.FindElements(byXpath)[0].Click();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                        }

                        //sell
                        if (alert.Equals("sell"))
                        {
                            try
                            {
                                By byXpath = By.XPath("//div[contains(@class,'apply-common-tooltip button-LlcFLhWF sellButton-LlcFLhWF')]");
                                driver.FindElements(byXpath)[0].Click();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                        }

                        //click on set tp/sl
                        System.Threading.Thread.Sleep(2000);
                        try
                        {
                            By byXpath = By.XPath("//div[contains(@class,'icon-R6hgQk56 button-9pA37sIi apply-common-tooltip isInteractive-9pA37sIi newStyles-9pA37sIi')]");
                            driver.FindElements(byXpath)[0].Click();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }

                        //click on the left box
                        System.Threading.Thread.Sleep(500);
                        try
                        {
                            By byXpath = By.XPath("//span[contains(@class,'box-5Xd5conM check-5Xd5conM noOutline-5Xd5conM')]");
                            driver.FindElements(byXpath)[0].Click();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }

                        //click on the right box
                        System.Threading.Thread.Sleep(100);
                        try
                        {
                            By byXpath = By.XPath("//span[contains(@class,'box-5Xd5conM check-5Xd5conM noOutline-5Xd5conM')]");
                            driver.FindElements(byXpath)[1].Click();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }

                        //set left pip
                        System.Threading.Thread.Sleep(100);
                        try
                        {
                            By byXpath = By.XPath("//input[contains(@class,'input-uGWFLwEy with-end-slot-uGWFLwEy')]");
                            driver.FindElements(byXpath)[2].SendKeys(tp.ToString());
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }

                        //set right pip
                        System.Threading.Thread.Sleep(100);
                        try
                        {
                            By byXpath = By.XPath("//input[contains(@class,'input-uGWFLwEy with-end-slot-uGWFLwEy')]");
                            driver.FindElements(byXpath)[6].SendKeys(sl.ToString());
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }

                        //press modify button
                        //blue modify
                        System.Threading.Thread.Sleep(100);
                        try
                        {
                            By byXpath = By.XPath("//button[contains(@class,'button-AUtedaek buy-AUtedaek popup-AUtedaek')]");
                            driver.FindElements(byXpath)[0].Click();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        //red modify
                        try
                        {
                            By byXpath = By.XPath("//button[contains(@class,'button-AUtedaek sell-AUtedaek popup-AUtedaek')]");
                            driver.FindElements(byXpath)[0].Click();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }

                        alert = "";
                    }
                    System.Threading.Thread.Sleep(200);
                }
            }, TaskCreationOptions.LongRunning);


            order.Start();
            alertChecker.Start();

            string param = "";
            while (!param.Equals("q"))
            {
                param = Console.ReadLine();
            }

            driver.Dispose();

        }
    }
}

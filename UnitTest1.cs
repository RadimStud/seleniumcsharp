using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;

namespace SeleniumTests
{
    public class MainMenuTests
    {
        private IWebDriver? driver;  // Nullable oprava CS8618
        private readonly string baseUrl = "https://radimstudeny.cz";

        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }

        [Test]
        public void TestMainMenuNavigation()
        {
            driver?.Navigate().GoToUrl(baseUrl);
            Thread.Sleep(2000); // Počkej na načtení stránky

            string[] xpaths =
            {
                "//*[@id='modal-1-content']/ul/li[1]/a/span",
                "//*[@id='modal-1-content']/ul/li[2]/a/span",
                "//*[@id='modal-1-content']/ul/li[3]/a/span",
                "//*[@id='modal-1-content']/ul/li[4]/a/span"
            };

            foreach (var xpath in xpaths)
            {
                try
                {
                    IWebElement? menuItem = driver?.FindElement(By.XPath(xpath));
                    menuItem?.Click();
                    Thread.Sleep(2000); // Počkej na přesměrování
                    Console.WriteLine($"Kliknuto na menu: {xpath}");
                }
                catch (NoSuchElementException)
                {
                    Assert.Fail($"Element nebyl nalezen: {xpath}");
                }
            }
        }

        [TearDown]
        public void TearDown()
        {
            if (driver != null)
            {
                driver.Quit();  // Ukončí prohlížeč
                driver.Dispose(); // Uvolní prostředky
            }
        }
    }
}

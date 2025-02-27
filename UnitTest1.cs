using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Threading;

namespace SeleniumTests
{
    public class MainMenuTests
    {
        private IWebDriver? driver;
        private readonly string baseUrl = "https://radimstudeny.cz";
        private readonly string screenshotPath = "/home/runner/work/TestProject1/screenshots"; // GitHub Actions path

        [SetUp]
        public void SetUp()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--headless");
            chromeOptions.AddArgument("--no-sandbox");
            chromeOptions.AddArgument("--disable-dev-shm-usage");
            chromeOptions.AddArgument("--disable-gpu");
            chromeOptions.AddArgument("--remote-debugging-port=9222");

            driver = new ChromeDriver(chromeOptions);
            driver.Manage().Window.Maximize();

            // Vytvoření složky pro screenshoty
            Directory.CreateDirectory(screenshotPath);
        }

        [Test]
        public void TestMainMenuNavigation()
        {
            driver?.Navigate().GoToUrl(baseUrl);
            Thread.Sleep(2000);
            Console.WriteLine($"✅ Otevřena stránka: {baseUrl}");

            string[] xpaths =
            {
                "//*[@id='modal-1-content']/ul/li[1]/a/span",
                "//*[@id='modal-1-content']/ul/li[2]/a/span",
                "//*[@id='modal-1-content']/ul/li[3]/a/span",
                "//*[@id='modal-1-content']/ul/li[4]/a/span"
            };

            for (int i = 0; i < xpaths.Length; i++)
            {
                string xpath = xpaths[i];
                try
                {
                    IWebElement? menuItem = driver?.FindElement(By.XPath(xpath));
                    menuItem?.Click();
                    Thread.Sleep(2000);
                    
                    Console.WriteLine($"✅ Kliknuto na menu [{i + 1}]: {xpath}");

                    // Uložení screenshotu
                    string screenshotFile = Path.Combine(screenshotPath, $"screenshot_{i + 1}.png");
                    TakeScreenshot(screenshotFile);
                    Console.WriteLine($"📸 Screenshot uložen: {screenshotFile}");
                }
                catch (NoSuchElementException)
                {
                    Assert.Fail($"❌ Element nebyl nalezen: {xpath}");
                }
            }
        }

        private void TakeScreenshot(string filePath)
        {
            try
            {
                Screenshot screenshot = ((ITakesScreenshot)driver!).GetScreenshot();
                screenshot.SaveAsFile(filePath, OpenQA.Selenium.ScreenshotImageFormat.Png); // Oprava
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Chyba při ukládání screenshotu: {ex.Message}");
            }
        }

        [TearDown]
        public void TearDown()
        {
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
                Console.WriteLine("✅ Test dokončen, prohlížeč zavřen.");
            }
        }
    }
}

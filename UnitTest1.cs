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
        private readonly string screenshotPath = "/home/runner/work/screenshots"; // Pro GitHub Actions

        [SetUp]
        public void SetUp()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--headless");  // Běží bez UI
            chromeOptions.AddArgument("--no-sandbox");  // Oprava pro GitHub Actions
            chromeOptions.AddArgument("--disable-dev-shm-usage");  // Oprava pro CI/CD
            chromeOptions.AddArgument("--disable-gpu");  // Prevence problémů s vykreslováním
            chromeOptions.AddArgument("--remote-debugging-port=9222");  // Debugging mode

            driver = new ChromeDriver(chromeOptions);
            driver.Manage().Window.Maximize();

            // Vytvoření složky pro screenshoty, pokud neexistuje
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
                screenshot.SaveAsFile(filePath, ScreenshotImageFormat.Png);
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

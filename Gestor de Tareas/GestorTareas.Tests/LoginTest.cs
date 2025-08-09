using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;
using System.IO;
using OpenQA.Selenium;

namespace GestorTareas.Tests
{
    public class LoginTests : IDisposable
    {
        //Intento de capturar automaticas (echo con IA)
        public void SaveScreenshot(IWebDriver driver, string testName)
        {
            try
            {
                var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                var dir = Path.Combine(Directory.GetCurrentDirectory(), "TestResults", "Screenshots");
                Directory.CreateDirectory(dir);
                var fileName = $"{testName}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                var path = Path.Combine(dir, fileName);
            }
            catch { /* no bloquear por error en captura */ }
        }

        private readonly IWebDriver _driver;
        private readonly string _baseUrl = "https://localhost:7150";

        public LoginTests()
        {
            var options = new ChromeOptions();

            options.AddArgument("--ignore-certificate-errors"); 
            _driver = new ChromeDriver(options);
        }

        [Fact]
        public void Login_ShouldShowError_WhenCredentialsAreInvalid()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Login");

            _driver.FindElement(By.Id("email")).SendKeys("usuario@correo.com");
            _driver.FindElement(By.Id("password")).SendKeys("clave_incorrecta");
            _driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            System.Threading.Thread.Sleep(800);

            var errorAlert = _driver.FindElements(By.ClassName("alert-danger"));
            Assert.NotEmpty(errorAlert);
        }

        public void Dispose()
        {
            _driver.Quit();
        }
    }
}

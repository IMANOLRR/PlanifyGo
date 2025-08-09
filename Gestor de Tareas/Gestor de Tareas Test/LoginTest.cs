using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace GestorTareas.Tests
{
    public class LoginTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl = "https://localhost:5001";

        public LoginTests()
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless"); 
            _driver = new ChromeDriver(options);
        }

        [Fact]
        public void Login_ShouldRedirectToIndex_WhenCredentialsAreValid()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Login");

            _driver.FindElement(By.Id("email")).SendKeys("usuario@correo.com");
            _driver.FindElement(By.Id("password")).SendKeys("123456");
            _driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            Assert.Contains("/Index", _driver.Url);
        }

        public void Dispose()
        {
            _driver.Quit();
        }
    }

    internal class FactAttribute : Attribute
    {
    }
}

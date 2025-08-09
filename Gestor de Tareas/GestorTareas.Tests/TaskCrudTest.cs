using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using Xunit;

namespace GestorTareas.Tests
{
    public class TasksCrudTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl = "https://localhost:7150";

        public TasksCrudTests()
        {
            var options = new ChromeOptions();

            options.AddArgument("--ignore-certificate-errors");
            _driver = new ChromeDriver(options);

            // Login inicial
            _driver.Navigate().GoToUrl($"{_baseUrl}/Login");
            _driver.FindElement(By.Id("email")).SendKeys("usuario@correo.com");
            _driver.FindElement(By.Id("password")).SendKeys("0224546");
            _driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            System.Threading.Thread.Sleep(800);
        }

        //  Test Agregar, Editar y Eliminar Tareas
        [Fact]
        public void AddTask_ShouldAppearInList()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/AddTask");

            _driver.FindElement(By.Id("title")).SendKeys("Tarea Selenium");
            _driver.FindElement(By.Id("description")).SendKeys("Descripción de prueba");
            _driver.FindElement(By.Id("tags")).SendKeys("Test");
            var priority = _driver.FindElement(By.Id("priority"));
            priority.SendKeys("1");
            _driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            System.Threading.Thread.Sleep(800);

            _driver.Navigate().GoToUrl($"{_baseUrl}/ListTasks");
            Assert.True(_driver.PageSource.Contains("Tarea Selenium"));
        }

        [Fact]
        public void EditTask_ShouldChangeTitle()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ListTasks");

            var editButtons = _driver.FindElements(By.LinkText("Editar"));
            Assert.NotEmpty(editButtons);
            editButtons[0].Click();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            var titleInput = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("title")));

            titleInput.Clear();
            titleInput.SendKeys("Tarea Modificada Selenium");
            _driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            _driver.Navigate().GoToUrl($"{_baseUrl}/ListTasks");
            Assert.Contains("Tarea Modificada Selenium", _driver.PageSource);
        }


        [Fact]
        public void DeleteTask_ShouldRemoveFromList()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/ListTasks");

            var deleteButtons = _driver.FindElements(By.LinkText("Eliminar"));
            Assert.NotEmpty(deleteButtons);
            deleteButtons[0].Click();

            System.Threading.Thread.Sleep(500);

            // En la página de confirmación
            _driver.FindElement(By.CssSelector("button.btn-danger")).Click();
            System.Threading.Thread.Sleep(800);

            _driver.Navigate().GoToUrl($"{_baseUrl}/ListTasks");
            Assert.False(_driver.PageSource.Contains("Tarea Modificada Selenium"));
        }

        public void Dispose()
        {
            _driver.Quit();
        }
    }
}

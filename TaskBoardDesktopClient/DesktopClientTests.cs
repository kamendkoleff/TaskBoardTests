using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace TaskBoard.DesktopClientTests
{
    public class DesktopTest
    {
        private const string AppiumUrl = "http://127.0.0.1:4723/wd/hub";
        private const string ContactsBookUrl = "https://taskbook.nakov.repl.co";
        private const string appLocation = @"C:\Work\TaskBoard.DesktopClient-v1.0\TaskBoard.DesktopClient.exe";

        private WindowsDriver<WindowsElement> driver;
        private AppiumOptions options;

        [SetUp]
        public void StartApp()
        {
            options = new AppiumOptions() { PlatformName = "Windows" };
            options.AddAdditionalCapability("app", appLocation);

            driver = new WindowsDriver<WindowsElement>(new Uri(AppiumUrl), options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
        }


        [TearDown]
        public void CloseApp()
        {
            driver.Quit();
        }

        [Test]
        public void Test_AllTasks_FindResult()
        {
            // Arrange
            var urlField = driver.FindElementByAccessibilityId("textBoxApiUrl");
            urlField.Clear();
            urlField.SendKeys(ContactsBookUrl);

            var buttonConnect = driver.FindElementByAccessibilityId("buttonConnect");
            buttonConnect.Click();

            string windowsName = driver.WindowHandles[0];
            driver.SwitchTo().Window(windowsName);

            Thread.Sleep(12000);
            
            //var taskFound = driver.FindElementByAccessibilityId("listViewItem");
            //Assert.That(taskFound.Text, Is.EqualTo("Project skeleton"));

            var addtask = driver.FindElementByAccessibilityId("buttonAdd");
            addtask.Click();

            string windowsCreate = driver.WindowHandles[0];
            driver.SwitchTo().Window(windowsCreate);

            var titleField = driver.FindElementByAccessibilityId("textBoxTitle");
            titleField.SendKeys("Test Task");
            var descField = driver.FindElementByAccessibilityId("textBoxDescription");
            descField.SendKeys("Test description");
            var createButton = driver.FindElementByAccessibilityId("buttonCreate");
            createButton.Click();

           
            //var taskCreated = driver.FindElementByAccessibilityId("listViewItem-");
            //Assert.That(taskCreated.Text, Is.EqualTo("Test Task"));





        }
    }
}
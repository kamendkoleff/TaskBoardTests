using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TaskBoardWebDriverTests
{
    public class WebDriverTests
    {
        private WebDriver driver;
        private const string url = "https://taskboard.nakov.repl.co/";
        [OneTimeSetUp]
        public void Setup()
        {
            this.driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }
        [OneTimeTearDown]
        public void CloseBrowser()
        {
            this.driver.Quit();

        }


        [Test]
        public void Test_ListTasks_CheckFirst()
        {
            driver.Navigate().GoToUrl(url);
            var contactLinks = driver.FindElement(By.LinkText("Task Board"));
            contactLinks.Click();

            var sectionTitle = driver.FindElement(By.XPath("//h1[contains(text(),'Done')]"));
            var taskTitle = driver.FindElement(By.CssSelector("#task1 > tbody > tr.title > td")).Text;
            
            Assert.That(taskTitle, Is.EqualTo("Project skeleton"));
            Assert.That(sectionTitle.Text, Is.EqualTo("Done"));
        }
        [Test]
        public void Test_NavigateSearch_FindTask()
        {
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Search")).Click();

            var searchField = driver.FindElement(By.Id("keyword"));
            searchField.SendKeys("Home");
            driver.FindElement(By.Id("search")).Click();

            var firstResult = driver.FindElement(By.CssSelector("#task2 > tbody > tr.title > td")).Text;
            

            Assert.That(firstResult, Is.EqualTo("Home page"));
           
        }
        [Test]
        public void Test_NavigateSearch_InvalidInput
            ()
        {
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Search")).Click();

            var searchField = driver.FindElement(By.Id("keyword"));
            searchField.SendKeys("blablabla");
            driver.FindElement(By.Id("search")).Click();

            var result = driver.FindElement(By.Id("searchResult")).Text;
            Assert.That(result, Is.EqualTo("No tasks found."));
        }
        [Test]
        public void Test_CreateInvalidTask()
        {
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Create")).Click();

            var descriptionText = driver.FindElement(By.Id("description"));
            descriptionText.SendKeys("QA's Rock the Boat !!!");
            var buttonCreate = driver.FindElement(By.Id("create"));
            buttonCreate.Click();


            var errormsg = driver.FindElement(By.CssSelector("body > main > div")).Text;
            Assert.That(errormsg, Is.EqualTo("Error: Title cannot be empty!"));
        }
        [Test]
        public void Test_CreateValidContact()
        {
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Create")).Click();

            var titleInput = "QAs Rock" + DateTime.Now.Ticks;
            var descriptionInput = "Improve Your Code!!!" + DateTime.Now.Ticks;
            var BoardName = "done";
            
            driver.FindElement(By.Id("title")).SendKeys(titleInput);
            driver.FindElement(By.Id("Description")).SendKeys(descriptionInput);
            driver.FindElement(By.Id("boardName")).SendKeys(BoardName);
            
            var buttonCreate = driver.FindElement(By.Id("create"));
            buttonCreate.Click();


            var allTasks = driver.FindElements(By.CssSelector("table.task-entry"));
            var lastContact = allTasks.Last();


            var titleLastTask = lastContact.FindElement(By.CssSelector("tr.title > td")).Text;
            
            Assert.That(titleLastTask, Is.EqualTo(titleInput));
          
        }
    }
}
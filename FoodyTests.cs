using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Foody
{
    public class FoodyTests
    {
        protected IWebDriver driver;
        private Actions actions;

        private static readonly string BaseUrl = "http://softuni-qa-loadbalancer-2137572849.eu-north-1.elb.amazonaws.com:85/";
        private static string? lastCreatedTitle;
        private static string? lastCreatedDescription;
        private static string? lastCreatedTitleEdited;
        private static int? numberOfDisplayedCards;


        [OneTimeSetUp]
        public void Setup()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddUserProfilePreference("profile.password_manager_enabled", false);
            chromeOptions.AddArgument("--disable-search-engine-choice-screen");

            driver = new ChromeDriver(chromeOptions);
            actions = new Actions(driver);

            Find.Initialize(driver, TimeSpan.FromSeconds(10));
            Finds.Initialize(driver, TimeSpan.FromSeconds(10));

            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.Navigate().GoToUrl(BaseUrl);

            Login("alp", "123456");

        }

        [OneTimeTearDown]
        public void TearDown()
        {
            driver.Close();
            driver.Quit();
            driver.Dispose();
        }

        //Random string generator
        private string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        //Finds SINGLE element by selector.
        public static class Find
        {
            private static IWebDriver driver;
            private static WebDriverWait wait;

            public static void Initialize(IWebDriver webDriver, TimeSpan timeout)
            {
                driver = webDriver;
                wait = new WebDriverWait(driver, timeout);
            }

            private static IWebElement WaitForElement(Func<By> bySelector)
            {
                return wait.Until(ExpectedConditions.ElementIsVisible(bySelector()));
            }

            public static Func<string, IWebElement> Id => locator => WaitForElement(() => By.Id(locator));
            public static Func<string, IWebElement> Name => locator => WaitForElement(() => By.Name(locator));
            public static Func<string, IWebElement> Css => locator => WaitForElement(() => By.CssSelector(locator));
            public static Func<string, IWebElement> XPath => locator => WaitForElement(() => By.XPath(locator));
            public static Func<string, IWebElement> ClassName => locator => WaitForElement(() => By.ClassName(locator));
            public static Func<string, IWebElement> TagName => locator => WaitForElement(() => By.TagName(locator));
            public static Func<string, IWebElement> LinkText => locator => WaitForElement(() => By.LinkText(locator));
            public static Func<string, IWebElement> PartialLinkText => locator => WaitForElement(() => By.PartialLinkText(locator));
        }

        //Finds MULTIPLE elements by selector.

        public static class Finds
        {
            private static IWebDriver driver;
            private static WebDriverWait wait;

            public static void Initialize(IWebDriver webDriver, TimeSpan timeout)
            {
                driver = webDriver;
                wait = new WebDriverWait(driver, timeout);
            }

            private static IReadOnlyCollection<IWebElement> WaitForElements(Func<By> bySelector)
            {
                return wait.Until(drv => drv.FindElements(bySelector()));
            }

            public static Func<string, IReadOnlyCollection<IWebElement>> Id => locator => WaitForElements(() => By.Id(locator));
            public static Func<string, IReadOnlyCollection<IWebElement>> Name => locator => WaitForElements(() => By.Name(locator));
            public static Func<string, IReadOnlyCollection<IWebElement>> Css => locator => WaitForElements(() => By.CssSelector(locator));
            public static Func<string, IReadOnlyCollection<IWebElement>> XPath => locator => WaitForElements(() => By.XPath(locator));
            public static Func<string, IReadOnlyCollection<IWebElement>> ClassName => locator => WaitForElements(() => By.ClassName(locator));
            public static Func<string, IReadOnlyCollection<IWebElement>> TagName => locator => WaitForElements(() => By.TagName(locator));
            public static Func<string, IReadOnlyCollection<IWebElement>> LinkText => locator => WaitForElements(() => By.LinkText(locator));
            public static Func<string, IReadOnlyCollection<IWebElement>> PartialLinkText => locator => WaitForElements(() => By.PartialLinkText(locator));
        }
        
        //Login
        public void Login(string userName,string password)
        {
            Find.XPath("//a[@class='nav-link'][contains(.,'Log In')]").Click();

            var userNameInput = Find.Css("input#username");
            userNameInput.Clear();
            userNameInput.SendKeys(userName);

            var passwordInput = Find.Css("input#password");
            passwordInput.Clear();  
            passwordInput.SendKeys(password);

            var loginBtn = Find.Css("[type='submit']");
            loginBtn.Click();
        }

        //Create new item
        public void CreateItem(string foodName, string foodDescription)
        {
            var addFoodLink = Find.XPath("//a[@class='nav-link'][contains(.,'Add Food')]");
            addFoodLink.Click();

            var foodNameInput = Find.Css("input#name");
            foodNameInput.Clear();
            foodNameInput.SendKeys(foodName);

            var foodDescriptionInput = Find.Css("input#description");
            foodDescriptionInput.Clear();
            foodDescriptionInput.SendKeys(foodDescription);

            Find.Css("input#url").SendKeys("https://cdn.britannica.com/71/182071-050-4081A3AB/Poutine.jpg");

            var addBtn = Find.Css("[type='submit']");
            addBtn.Click();



        }
        
        //Get last item in the list
        public IWebElement GetLastCard()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var cards = wait.Until(driver => driver.FindElements(By.CssSelector(".row.gx-5.align-items-center")));
            Assert.That(cards.Count, Is.GreaterThan(0));
            return cards.Last();
        }

        //Search method
        public void Search(string searchCriteria)
        {
            var searchInput = Find.XPath("//input[contains(@type,'search')]");
            searchInput.Clear();
            searchInput.SendKeys(searchCriteria);

            var searchBtn = Find.Css(".btn.btn-primary.col-2.mt-5.rounded-pill > svg[role='img']");
            searchBtn.Click();

        }

        //Count displayed items
        public void CountDisplayedElements()
        {
            var elements = driver.FindElements(By.CssSelector(".row.gx-5.align-items-center"));
            numberOfDisplayedCards = elements.Count();
        }

        //Test #1 - Add item with INVALID data
        [Test, Order(1)]
        public void AddItemInvalidData()
        {
            driver.Navigate().GoToUrl(BaseUrl);
            CreateItem(" ", " ");

            string expectedUrl = $"{BaseUrl}Food/Add";
            Assert.That(driver.Url, Is.EqualTo(expectedUrl), "The URL after creation did not match the expected URL.");

            var mainErrorMsg = Find.Css(".text-danger.validation-summary-errors > ul > li");
            var titleErrorMsg = Find.Css("[data-valmsg-for='Name']");
            var descriptionErrorMsg = Find.Css("[data-valmsg-for='Description']");

            Assert.That(mainErrorMsg.Text, Is.EqualTo("Unable to add this food revue!"));
            Assert.That(titleErrorMsg.Text, Is.EqualTo("The Name field is required."));
            Assert.That(descriptionErrorMsg.Text, Is.EqualTo("The Description field is required."));
        }

        //Test #2 - Add item with VALID data
        [Test, Order(2)]
        public void AddItemValidData()
        {
            driver.Navigate().GoToUrl(BaseUrl);
            lastCreatedTitle = $"Food {GenerateRandomString(5)}";
            lastCreatedDescription = $"Description {GenerateRandomString(10)}";

            Find.XPath("//a[@class='nav-link'][contains(.,'Add Food')]").Click();

            CreateItem(lastCreatedTitle, lastCreatedDescription);
            Console.WriteLine($"Last created Title:{lastCreatedTitle}");
            Console.WriteLine($"Last created Description:{lastCreatedDescription}");

            string expectedUrl = $"{BaseUrl}";
            Assert.That(driver.Url, Is.EqualTo(expectedUrl), "The URL after creation did not match the expected URL.");

            var lastCard = GetLastCard();
            var lastCreatedTitleDisplayed = lastCard.FindElement(By.CssSelector("div.p-5>h2"));
            Console.WriteLine($"Title text of the last item in the list: {lastCreatedTitleDisplayed.Text}");
            Assert.That(lastCreatedTitleDisplayed.Text, Is.EqualTo(lastCreatedTitle));

            CountDisplayedElements();
            Console.WriteLine($"The number of displayed cards is: {numberOfDisplayedCards}");
        }

        //Test #3 - Edit the last created item
        [Test, Order(3)]
        public void EditLastAddedItem()
        {
            driver.Navigate().GoToUrl(BaseUrl);

            lastCreatedTitleEdited = $"Edited Title {GenerateRandomString(3)}";
            CountDisplayedElements();
            Console.WriteLine($"The number of displayed cards BEFORE EDIT is: {numberOfDisplayedCards}");


            var lastCard = GetLastCard();
            var lastEditBtn = lastCard.FindElement(By.CssSelector("a[href*='/Food/Edit']"));
            actions.MoveToElement(lastEditBtn).Click().Perform();

            CreateItem(lastCreatedTitleEdited, lastCreatedDescription);

            lastCard = GetLastCard();
            var lastCreatedTitleDisplayed = lastCard.FindElement(By.CssSelector("div.p-5>h2"));
            Console.WriteLine($"Title text of the last item in the list: {lastCreatedTitleDisplayed.Text}");
            CountDisplayedElements();

            var cards = driver.FindElements(By.CssSelector(".row.gx-5.align-items-center"));
            bool titleFound = cards.Any(card =>
            {
                var titleElement = card.FindElement(By.CssSelector("div.p-5 > h2"));
                return titleElement.Text.Equals(lastCreatedTitle, StringComparison.OrdinalIgnoreCase);
            });

            Assert.That(lastCreatedTitleDisplayed.Text, Is.EqualTo(lastCreatedTitleEdited));
            Assert.IsFalse(titleFound, "The card with the last created title was FOUND.");
            Console.WriteLine($"The number of displayed cards AFTER EDIT is: {numberOfDisplayedCards}");
            Console.WriteLine($"!!BUG DETECTED!!! Edit functionality creates new item, instead of editing the last created one.");


        }

        //Test #4 - Search for last created item
        [Test, Order(4)]
        public void SearchForLastCreatedItem() {
            driver.Navigate ().GoToUrl(BaseUrl);

            Search(lastCreatedTitle);
            CountDisplayedElements();
            Console.WriteLine($"The number of displayed cards is: {numberOfDisplayedCards}");
            Assert.That(numberOfDisplayedCards,Is.EqualTo(1));

            var lastCreatedTitleDisplayed = driver.FindElement(By.CssSelector("div.p-5>h2"));
            Console.WriteLine($"Title text of the last item in the SEARCH list: {lastCreatedTitleDisplayed.Text}");
            Assert.That(lastCreatedTitleDisplayed.Text, Is.EqualTo(lastCreatedTitle));


        }

        //Test #5 - Delete last created item
        [Test,Order(5)]
        public void DeleteLastItem()
        {
            driver.Navigate ().GoToUrl(BaseUrl);

            CountDisplayedElements();
            Console.WriteLine($"The number of displayed cards BEFORE delete is: {numberOfDisplayedCards}");
            var DisplayedCardsBefore = numberOfDisplayedCards;

            var lastCard = GetLastCard();
            var lastEditBtn = lastCard.FindElement(By.CssSelector("a[href*='/Food/Delete']"));
            actions.MoveToElement(lastEditBtn).Click().Perform();

            driver.Navigate ().GoToUrl(BaseUrl);
            CountDisplayedElements();
            Console.WriteLine($"The number of displayed cards AFTER delete is: {numberOfDisplayedCards}");
            var DisplayedCardsAfter = numberOfDisplayedCards;

            Assert.That(numberOfDisplayedCards, Is.EqualTo(DisplayedCardsBefore - 1));
        }

        //Test #6 - Perform search for last created item.
        [Test,Order(6)]
        public void SearchForDeletedItem()
        {
            driver.Navigate ().GoToUrl(BaseUrl);
            Search(lastCreatedTitleEdited);

            var searchMsg = Find.XPath("//h2[@class='display-4']");
            Assert.That(searchMsg.Text, Is.EqualTo("There are no foods :("));

            var addBtn = Find.XPath("//a[@class='btn btn-primary btn-xl rounded-pill mt-5'][contains(.,'Add food')]");
            Assert.IsTrue(addBtn.Displayed, "The 'Add food' button is not visible.");
        }
    }
}
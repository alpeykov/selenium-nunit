# Selenium N-UNIT Test Project

This project is a Selenium-based test automation framework using N-UNIT to automate the testing of web application. The project covers basic CRUD operations on items and tests various functionalities like login, search, and validation for input fields.

## Tested app
[Link](http://softuni-qa-loadbalancer-2137572849.eu-north-1.elb.amazonaws.com:85/)

## Prerequisites
- .NET SDK - latest version
- NUnit Framework
- Selenium WebDriver
- ChromeDriver (compatible version with Chrome browser)
- SeleniumExtras.WaitHelpers

## Setup
- git clone https://github.com/alpeykov/selenium-nunit
- Open Foody.csproj
- Build the project
- Run the tests

**Notes:**
- Test #5 passes, BUT there is a BUG.
Edit functionality creates new item, instead of editing the last created one.

**Methods:**
- **Setup**: Initializes the WebDriver, logs in with a predefined user, and sets up the test environment.
  
- **GenerateRandomString(int length)**: Generates a random alphanumeric string of specified length.

- **Find**: Provides utility functions to find a single element by different selectors (ID, Name, CSS, XPath, etc.). 
  - Example usage: `Find.Id("element-id")`

- **Finds**: Similar to `Find`, but for finding multiple elements that match a given selector.

- **Login(string userName, string password)**: Logs in a user with the specified username and password.

- **CreateItem(string foodName, string foodDescription)**: Creates a new item with the provided name and description.

- **GetLastCard()**: Returns the last displayed item card from the list on the UI.

- **Search(string searchCriteria)**: Searches for items based on the provided search criteria.

- **CountDisplayedElements()**: Counts and returns the number of item cards currently displayed on the page. All items are displayed on a single page. There is no paggination.

## Tests

1. **AddItemInvalidData**
   - Description: Attempts to create an item with invalid (empty) inputs.
   - Expected Outcome: Validation error messages are displayed, and the item is not supposed to be created successfully.

2. **AddItemValidData**
   - Description: Creates an item with valid data.
   - Expected Outcome: The item is successfully created and displayed on the page, where all items are displayed.

3. **EditLastCreatedItem**
   - Description: Edits the last created item.
   - Expected Outcome: The last item is updated with new data. The previous title should not be present in the list of all items or available via search.

4. **SearchForLastCreatedItem**
   - Description: Searches for the last created item using its title.
   - Expected Outcome: Only one item matching the search criteria is displayed.

5. **DeleteLastItem**
   - Description: Deletes the last created item.
   - Expected Outcome: The item is successfully deleted and the count decreases by one.

6. **SearchForDeletedItem**
   - Description: Searches for the item, that had just been deleted.
   - Expected Outcome: Displays a message indicating no items found, and the "Add Food" button is visible.

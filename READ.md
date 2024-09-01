# Selenium N-UNIT Test Project

This project is a Selenium-based test automation framework using N-UNIT to automate the testing of web application. The project covers basic CRUD operations (Create, Read, Update, Delete) on items and tests various functionalities like login, search, and validation for input fields.

## Prerequisites

- .NET SDK - latest version
- NUnit Framework
- Selenium WebDriver
- ChromeDriver (compatible version with Chrome browser)
- SeleniumExtras.WaitHelpers


**Methods:**
- **Setup**: Initializes the WebDriver, logs in with a predefined user, and sets up the test environment.
  
- **GenerateRandomString(int length)**: Generates a random alphanumeric string of specified length. Useful for creating unique test data.

- **Find**: Provides utility functions to find a single element by different selectors (ID, Name, CSS, XPath, etc.). 
  - Example usage: `Find.Id("element-id")`

- **Finds**: Similar to `Find`, but for finding multiple elements that match a given selector.

- **Login(string userName, string password)**: Logs in a user with the specified username and password.

- **CreateItem(string foodName, string foodDescription)**: Creates a new food item with the provided name and description.

- **GetLastCard()**: Returns the last displayed item card from the list on the UI.

- **Search(string searchCriteria)**: Searches for items based on the provided search criteria.

- **CountDisplayedElements()**: Counts and returns the number of item cards currently displayed on the page.

## Tests

1. **AddItemInvalidData**
   - Description: Attempts to create a food item with invalid (empty) inputs.
   - Expected Outcome: Validation error messages are displayed, and the item is not added.

2. **AddItemValidData**
   - Description: Creates a food item with valid data.
   - Expected Outcome: The item is successfully added, and the main page is displayed with the new item.

3. **EditLastCreatedItem**
   - Description: Edits the last created item.
   - Expected Outcome: The last item is updated with new data. The previous title should not be found.

4. **SearchForLastCreatedItem**
   - Description: Searches for the last created item using its title.
   - Expected Outcome: Only one item matching the search criteria is displayed.

5. **DeleteLastItem**
   - Description: Deletes the last created item.
   - Expected Outcome: The item count decreases by one.

6. **SearchForDeletedItem**
   - Description: Searches for a recently deleted item.
   - Expected Outcome: Displays a message indicating no items found, and the "Add Food" button is visible.
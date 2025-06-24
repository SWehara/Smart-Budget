[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-22041afd0340ce965d47ae6ef1cefeee28c7c493a6346c4f15d667ab976d596c.svg)](https://classroom.github.com/a/XEg8zYV8)
AF/22/0765
Feature list and instructions
- Main Window
    Opens first.
    Click to go to the Dashboard.
- Dashboard Window
    Main menu of the app.
    Click buttons to open:
       → Income
       → Expenses
       → Help
- Income Window
    Add your income (name, amount, date).
    View list of your income entries.
- Expenses Window
     Add your expenses (name, amount, date).
     View list of your expense entries.
- Help Window
   Shows how to use the app.


Documenting Final Features


"My Smart Budget" is a desktop finance application developed using C# and WPF.The application includes the following:

1. Login
- User login with credential validation
- Secured with SQL queries
- Redirects to dashboard on success

2. Dashboard
- Navigation buttons for all modules
- Personalized user greeting

3. Income
- Add, edit, and delete income entries
- Fields: source, amount, notes, and date
- Automatically filtered by selected month and year
- Updates total income for budget calculation

4. Budget
- Add and manage monthly budget categories
- Set category limits manually
- Real-time update of ‘Spent’ based on expenses
- Warnings when limits are exceeded or nearly reached (Highliting the row yellow when spendings near to budget, Highliting the row red when user exceeds the budget)
- Pie chart visualization using OxyPlot

5. Expenses
- Add, edit, and delete categorized expenses
- Category dropdown synced with budget categories
- Automatically updates ‘Spent’ in Budget module
- Filters by month/year 

 6. Goals
- Setting goals
- Track progress toward target savings

 7. Settings
- Change password with validation
- Delete account with confirmation

 8. Help
- Feature-based help documentation

 9. Month Report
- View summary of income, expenses, and remaining balance
- Export financial report as a PDF using QuestPDF

 10. Profile
- View and Edit personal information like email and phone number


Watch the demo video here (https://drive.google.com/file/d/1DY7cTFydmDnMsQG12mpWQF1shWW6Zi9y/view?usp=drive_link)

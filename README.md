# Blazor Bank Application
This is a small bank application made with Blazor WebAssembly.
It is part of a school assignment.
The app runs in the browser and uses LocalStorage to save data.

## ✨ What can the app do:
Create new accounts
Deposit money into an account
Withdraw money from an account
Transfer money between your own accounts
See a history of all transactions
Data is saved automatically

## ▶️ How to run the app:
Step 1. Open project in Visual Studio or whatever program you're using
Step 2. Make sure you have .NET 8 installed
Step 3. Run the project by pressing play or use command: dotnet run
Step 4. The browser will open automatically or you can use the link in the terminal

## 🏦 How to use the app:
You can create an acoount
Step 1. Go to Create account or /CreateAccount
Step 2. Name your account name 
Step 3. Choose what kind of account type bank account or save account
Step 4. Choose starting balance
Step 5. Click Create Account

### You can deposit or withdraw
Step 1. Go to Deposit/Withdrawal or /Deposit
Step 2. Select account
Step 3. Type the amount 
Step 4. Write description (optional)
Step 5. Choose type: deposit or withdraw
Step 5. Click the button Execute

### You can transfer money
Step 1. Go to Transfer or /Transaction
Step 2. Choose the account to send from 
Step 3. Choose the account to send to  
Step 4. Enter the amount
Step 5. Click transfer 

### You can view History
Step 1. Go to History or /History
Step 2. You can choose to all accounts or one account 
Step 3. You can see specific types Deposit, Withdrawal and Transfer 
Step 4. You can choose from a specific date 
Step 5. You can choose to a specific date 
Step 6. You can choose minimum amount of money 
Step 7 You can choose maximum amount of money 
Step 8 You can sort it by different types by date newest or oldest (Datum) or amount biggest and lowest

### You can view your accounts
Step 1. Go to My Accounts or /Accounts
Now you can see your accounts, you can also remove the accounts by pressing remove button but only if you have 0 amount of money on the account. 

## 💾 How is data saved?
Data is saved by using LocalStorage which means its only saved locally and when you open the page again the data will still be there. 

## 🧱 In Solution Explorer my different folder and programs
Domain/
BankAccount.cs   // Represents a bank account
Transaction.cs  // Represents a transaction

Interfaces/
IAccountService.cs  // Account service contract
IStorageService.cs // Storage contract

Services/
AccountService.cs   // Logic for accounts and saving data
StorageService.cs  // Talks to LocalStorage

Pages/
Accounts.razor       // Create and show accounts
Deposit.razor       // Deposit and withdraw page
Transaction.razor  // Transfer money page
History.razor     // View history page

## 📝 Using Console.WriteLine inside AccountService for important actions like:
When account is created
When money is deposited or withdrawn
When a transfer happens

## 🧭 Git commands i have been using:
For pushing code to Github
Git add .
Git commit -m "Write changes"
Git push

For pulling code between my laptop and computer
Git pull // You can use this to pull my code to your software but you need git installed on your computer 

For switching between my branch and main
Git checkout main
Git checkout featureaccount

Merging my branch to my main
Git merge featureaccount

## 💡VG addition
### Interest  
Savings accounts automatically earn 2% interest per month based on their current balance. I added this feature to simulate how real savings accounts work, and to make the application feel more realistic and useful. I created method in AccountService and IAccountService and implemented it in Accounts.Razor.
### PIN Lock 
The page starts locked. You enter a PIN. If the PIN is correct, the content shows. If it's wrong, an error is shown. The "Lock" button locks the page again. This is only for simple access, not real security.
### Categories
I added a new Category property to the transaction form. When transferring money, the user can now select a category (Food, Rent, Transport). The selected category is stored with the transaction and displayed in the description in history tab.


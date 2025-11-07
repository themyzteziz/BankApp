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
Step 1. Go to Skapa konto
Step 2. Name your account name (Kontonamn)
Step 3. Choose what kind of account type bank account or save account (Bankkonto / Sparkonto)
Step 4. Choose starting amount (Startsaldo)
Step 5. Click Skapa konto

### You can deposit or withdraw
Step 1. Go to Insättning/Uttag
Step 2. Choose an account (Välj konto:)
Step 3. Type the amount (Belopp:)
Step 4. Choose deposit or withdraw / Insätt eller Uttag
Step 5. Click the button (Utför)

### You can transfer money
Step 1. Go to Överföring (tab)
Step 2. Choose the account to send from (Från konto)
Step 3. Choose the account to send to  (Till konto)
Step 4. Enter the amount (Belopp)
Step 5. Click transfer (Överför)

### You can view History
Step 1. Go to Historik (tab)
Step 2. You can choose to all accounts or one account (Konto:)
Step 3. You can see specific types Deposit, Withdrawal and Transfer (Typ:)
Step 4. You can choose from a specific date (Från:)
Step 5. You can choose to a specific date (Till:)
Step 6. You can choose minimum amount of money (Min:)
Step 7 You can choose maximum amount of money (Max:)
Step 8 You can sort it by different types by date newest or oldest (Datum) or amount biggest and lowest (Belopp)

### You can view your accounts
Step 1. Go to Mina konton
Now you can see your accounts, you can also remove the accounts by pressing remove button (ta bort) but only if you have 0 amount of money on the account. 

## 💾 How is data saved?
Data is saved by using LocalStorage which means its only saved locally and when you open the page again the data will still be there. 

## 🧱 In Solution Explorer my different folder and programs properties
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
Savings accounts automatically earn 2% interest per month based on their current balance and also button to demonstrate 2% increase.
I added this feature to simulate how real savings accounts work, and to make the application feel more realistic and useful. I created method in AccountService and IAccountService and implemented it in Accounts.Razor.
### PIN Lock 
The page starts locked. You enter a PIN. If the PIN is correct, the content shows. If it's wrong, an error is shown. The "Lock" button locks the page again. This is only for simple access, not real security.

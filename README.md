BankApp – Blazor WebAssembly (.NET 8)

Features:

Create accounts (name, type, SEK)

View account list with balance and last updated

Deposit, withdraw, and transfer between accounts

Transaction history with filtering and sorting

Data persistence via browser LocalStorage

Tech & structure:

Object-oriented design with BankAccount, Transaction, AccountType

Interfaces and services (IAccountService, IStorageService)

Dependency Injection (configured in Program.cs)

Simple logging using Console.WriteLine

Error handling for invalid amounts and overdrafts

How to run:

Run dotnet restore

Run dotnet run

Open the link shown in the terminal

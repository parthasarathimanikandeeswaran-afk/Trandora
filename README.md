🛒 Trandora – ASP.NET Core MVC E-Commerce Application
📌 Overview

Trandora is a full-stack e-commerce web application built using ASP.NET Core MVC that demonstrates real-world online shopping functionality. The project is designed with clean MVC architecture and includes user authentication, product management, cart operations, checkout, and order processing. It uses ADO.NET with SQL Server for database interactions and cookie-based authentication for user session management.

🚀 Features

User Registration and Login

Cookie-based Authentication

Product Listing and Product Details

Add to Cart & Remove from Cart

Cart Management with Quantity Handling

Secure Checkout Process

Order Placement and Order History

Admin View for All Orders

Clean MVC Architecture (Controller, Model, Repository)

SQL Server Database Integration using ADO.NET

🛠️ Tech Stack

Frontend: Razor Views, HTML5, CSS3, Bootstrap

Backend: ASP.NET Core MVC

Database: SQL Server (LocalDB)

Data Access: ADO.NET

Authentication: Cookie-based Authentication

IDE: Visual Studio 2022

Version Control: Git & GitHub

🗂️ Project Structure
Trandora
│
├── Controllers
│   └── HomeController.cs
│
├── Models
│   ├── User.cs
│   ├── Product.cs
│   ├── CartItem.cs
│   ├── Order.cs
│   └── OrderItem.cs
│
├── Repositories
│   ├── UserRepository.cs
│   └── ProductRepository.cs
│
├── Views
│   ├── Home
│   ├── Products
│   ├── Cart
│   ├── Orders
│   └── Shared
│
├── wwwroot
│   ├── css
│   ├── js
│   └── images
│
├── appsettings.json
├── Program.cs
└── Trandora.csproj

⚙️ Database Configuration

The application uses SQL Server LocalDB.
Update your connection string in appsettings.json:

"ConnectionStrings": {
  "mycon": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TrandoraApp;Integrated Security=True"
}

▶️ How to Run the Project

Clone the repository:

git clone https://github.com/YOUR_USERNAME/Trandora.git


Open the project in Visual Studio 2022

Restore NuGet packages

Configure SQL Server and database tables

Run the project using IIS Express

🧠 Learning Outcomes

Understanding ASP.NET Core MVC architecture

Working with ADO.NET and SQL Server

Implementing cookie-based authentication

Building complete e-commerce workflows

Handling real-world CRUD operations

📸 Screenshots

(Add screenshots of Login, Products, Cart, Checkout, Orders)

👨‍💻 Author

Sarathi M
B.Sc Computer Science | ASP.NET Core MVC Developer

📄 License

This project is open-source and available for learning and educational purposes.

⭐ If you like this project, don’t forget to give it a star!

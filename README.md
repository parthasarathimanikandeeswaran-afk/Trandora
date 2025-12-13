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

Register Page
<img width="1916" height="916" alt="Register page" src="https://github.com/user-attachments/assets/412fa0fd-df5f-44b8-9fff-70c6c9282a53" />

Login Page
<img width="1918" height="977" alt="LoginPage" src="https://github.com/user-attachments/assets/48bad780-7932-4f00-ad90-da8b37f6e0fa" />

Home Page
<img width="1893" height="918" alt="Home page" src="https://github.com/user-attachments/assets/6c5586a0-0ba5-4ceb-8c37-a37a121b17d3" />

Products Page
<img width="1907" height="980" alt="Products page" src="https://github.com/user-attachments/assets/cb73afee-3be2-4d6c-a3a2-380fb9e5e581" />

Cart Page
<img width="1918" height="921" alt="Cart page" src="https://github.com/user-attachments/assets/d1af83f1-f889-4464-a4d4-0a2fd7063fc0" />

Checkout Page
<img width="1910" height="917" alt="Checkout page" src="https://github.com/user-attachments/assets/3e24d416-2dc3-4b4f-8b5a-f90e1c2f160c" />

Order Success Page
<img width="1912" height="918" alt="Order success page" src="https://github.com/user-attachments/assets/1287d0f7-fa5c-4ffd-a594-6ca365fb38b6" />

About Page
<img width="1896" height="905" alt="About page" src="https://github.com/user-attachments/assets/b3713a0e-643c-442a-9734-e6d31706b843" />

Contact Page
<img width="1897" height="912" alt="Contact page" src="https://github.com/user-attachments/assets/082bfe73-c9c3-4ede-aa69-140928af21c6" />


👨‍💻 Author

Sarathi M
B.Sc Computer Science | ASP.NET Core MVC Developer

📄 License

This project is open-source and available for learning and educational purposes.

⭐ If you like this project, don’t forget to give it a star!

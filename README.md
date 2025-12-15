# 🛒 Trandora – ASP.NET Core MVC E-Commerce Application

A full-stack e-commerce web application built using **ASP.NET Core MVC**, showcasing real-world online shopping functionality with secure authentication, cart management, checkout, and order processing.

---

## 📌 Overview

**Trandora** is a dynamic and scalable e-commerce platform designed using clean **MVC architecture**.  
It supports user authentication, product browsing, cart operations, checkout, and order management.  
The application uses **ADO.NET with SQL Server** for data access and **cookie-based authentication** for session handling.

---

## 🚀 Features

### 👤 User Features
- User Registration & Login
- Cookie-based Authentication
- Product Listing & Product Details
- Add to Cart / Remove from Cart
- Cart Quantity Management
- Secure Checkout Process
- Order Placement & Order History

### 🛠 Admin Features
- View all user orders
- Manage order data
- Centralized product handling

### 🧱 Architecture
- Clean **MVC Pattern**
- Repository-based data access
- Secure session and cookie handling

---

## 🛠️ Tech Stack

| Layer | Technology |
|-----|------------|
| Frontend | Razor Views, HTML5, CSS3, Bootstrap |
| Backend | ASP.NET Core MVC |
| Database | SQL Server (LocalDB) |
| Data Access | ADO.NET |
| Authentication | Cookie-based Authentication |
| IDE | Visual Studio 2022 |
| Version Control | Git & GitHub |

---

## 🗂️ Project Structure

Trandora
│
├── Controllers
│ └── HomeController.cs
│
├── Models
│ ├── User.cs
│ ├── Product.cs
│ ├── CartItem.cs
│ ├── Order.cs
│ └── OrderItem.cs
│
├── Repositories
│ ├── UserRepository.cs
│ └── ProductRepository.cs
│
├── Views
│ ├── Home
│ ├── Products
│ ├── Cart
│ ├── Orders
│ └── Shared
│
├── wwwroot
│ ├── css
│ ├── js
│ └── images
│
├── appsettings.json
├── Program.cs
└── Trandora.csproj

yaml
Copy code

---

## ⚙️ Database Configuration

The application uses **SQL Server LocalDB**.

Update the connection string in `appsettings.json`:

```json
"ConnectionStrings": {
  "mycon": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TrandoraApp;Integrated Security=True"
}
▶️ How to Run the Project
Clone the repository

bash
Copy code
git clone https://github.com/YOUR_USERNAME/Trandora.git
Open the project in Visual Studio 2022

Restore NuGet packages

Configure SQL Server & create database tables

Run the project using IIS Express

🧠 Learning Outcomes
Understanding ASP.NET Core MVC architecture

Working with ADO.NET and SQL Server

Implementing cookie-based authentication

Building complete e-commerce workflows

Handling real-world CRUD operations

Applying clean coding and layered architecture

👨‍💻 Author
Parthasarathi M
🎓 B.Sc Computer Science
💻 ASP.NET Core MVC Developer (Fresher)

📄 License
This project is open-source and intended for learning and educational purposes.

⭐ If you like this project, don’t forget to star the repository!







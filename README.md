# 📖 MyLibrary System

## 📌 Overview
The Library Management System is a **web-based application** developed using **ASP.NET Core MVC**.  
It provides an efficient way to manage **books, users, memberships, and borrowing operations** in a digital library environment.  
The system is designed with modern architecture patterns and is continuously evolving to include more features and better usability.

---

## 🏗 Versions

### ✅ Version 1.0
- Admin panel for system management.
- Full **CRUD operations** for **Books** and **Users**.
- Partial CRUD for other entities (e.g., **Categories**, **Borrowings**).
- **Image upload** support for book covers and user profile.
- **Reusable Delete Modal** for safe record deletion.
- Database integration with **SQL Server** and **Entity Framework Core**.
- **Unit of Work & Repository Pattern** for clean data access and separation of concerns.
- **Dependency Injection (DI)** for scalable architecture and service management.
- **Authentication & Validation** to ensure users exist before borrowing books.
- Real-time **borrowings, membership and user status updates**.

### 🔜 Version 2.0 (In Progress)
- Enhancing borrowing and membership features.
- Improved UI/UX.
- Integration of additional analytics.

---

## ✨ Features
- **📚 Books Management:** Add, read, edit, delete books; update availability status.
- **👤 Users Management:** Add, read, edit, delete members; active/inactive status based on borrowing activity.
- **🏷 Categories:** Organize books into categories.
- **🔄 Borrowings:** Track borrowed, overdue, and returned books; calculate fines automatically.
- **🖼 Image Upload:** Upload and display book covers and user profile photo using `IFormFile`.
- **🗑 Reusable Delete Modal:** Centralized deletion confirmation system.
- **🛡 Authentication & Validation:** Ensure only valid members can borrow books; manage sessions securely.

---

## 🛠 Tech Stack
- **Frontend:** HTML5, CSS3, Bootstrap 5, JavaScript.  
- **Backend:** ASP.NET Core MVC (.NET 8)  
- **Database:** Microsoft SQL Server  
- **ORM:** Entity Framework Core with LINQ  
- **Architecture Patterns:** Unit of Work & Repository, Dependency Injection  
- **Version Control:** Git & GitHub

> The One Reader Library is designed to be **scalable, maintainable, and user-friendly**, providing a smooth experience for both library admins and members.


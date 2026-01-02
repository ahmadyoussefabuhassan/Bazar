# 🛍️ BazarMaster API

![.NET](https://img.shields.io/badge/.NET-8.0-purple)
![SQL Server](https://img.shields.io/badge/Database-SQL%20Server-lightgrey)
![Status](https://img.shields.io/badge/Status-Completed-success)

**BazarMaster** is a comprehensive RESTful Web API designed for a Customer-to-Customer (C2C) e-commerce platform. It enables users to list used or new items for sale, browse categories, and communicate via an intelligent system.

This project was developed as a **Graduation Project**, showcasing modern backend architecture, security best practices, and AI integration.

---

## ✨ Key Features

*   **🔐 Secure Authentication:** Full Identity system using **JWT (JSON Web Tokens)** for secure login and registration.
*   **📦 Product Management:** CRUD operations for products with support for multiple image uploads.
*   **📂 Categories:** Organized product listing through dynamic categories.
*   **🤖 Smart AI Assistant:** Integrated Chatbot to assist users with prices, navigation, and FAQs (Powered by Google Gemini / Rule-Based Logic).
*   **🔍 Advanced Search:** Filter products by name, category, price range, and location.
*   **☁️ Cloud Ready:** configured for deployment on IIS servers (e.g., Somee/Azure).

---

## 🛠️ Tech Stack

*   **Framework:** ASP.NET Core Web API (.NET 8)
*   **Database:** SQL Server (Entity Framework Core - Code First)
*   **ORM:** Entity Framework Core
*   **Mapping:** AutoMapper
*   **Documentation:** Swagger UI (OpenAPI)
*   **Security:** ASP.NET Core Identity & JWT Bearer Authentication
*   **AI Integration:** Custom AI Service

---

## 🗂️ Database Schema (ER Diagram)

The database is designed to handle users, products, images, and categories efficiently. Below is an overview of the relationships based on the project architecture:

### 1. **User Table**
Stores user credentials and profile information.
*   **Attributes:** `FirstName`, `LastName`, `Email`, `Password (Hashed)`, `PhoneNumber`, `Location`, `Role`.
*   **Relationship:** A **User** can list multiple **Products**.

### 2. **Product Table**
The core entity representing items for sale.
*   **Attributes:** `Name`, `Price`, `Description`, `Condition`, `Location`, `ContactPhoneNumber`.
*   **Relationship:** 
    *   Belongs to a **User** (Seller).
    *   Belongs to a **Category**.
    *   Has many **Images**.

### 3. **Category Table**
Classifies products into groups (e.g., Electronics, Books).
*   **Attributes:** `Name`.
*   **Relationship:** A **Category** contains multiple **Products**.

### 4. **Image Table**
Handles media files associated with products.
*   **Attributes:** `FilePath`, `ContentType`, `FileSize`, `IsMain`.
*   **Relationship:** Linked to a specific **Product**.

---

## 🚀 Getting Started

### Prerequisites
*   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
*   SQL Server (LocalDB or Express)
*   Visual Studio 2022 or VS Code

### Installation

1.  **Clone the repository:**
    ```bash
    git clone https://github.com/YourUsername/BazarMaster-API.git
    ```
2.  **Configure Database:**
    Update the `appsettings.json` file with your connection string:
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=YOUR_SERVER;Database=BazarDb;Trusted_Connection=True;TrustServerCertificate=True"
    }
    ```
3.  **Run Migrations:**
    Open the Package Manager Console and run:
    ```bash
    Update-Database
    ```
4.  **Run the API:**
    ```bash
    dotnet run
    ```
5.  **Explore:**
    Navigate to `https://localhost:xxxx/swagger` to test the endpoints.

---

## 🔌 API Endpoints Overview

| Method | Endpoint | Description | Auth Required |
| :--- | :--- | :--- | :---: |
| `POST` | `/api/Auth/register` | Register a new user | ❌ |
| `POST` | `/api/Auth/login` | Login and get JWT Token | ❌ |
| `GET` | `/api/Product` | Get all products (with filters) | ❌ |
| `POST` | `/api/Product` | Add a new product with images | ✅ |
| `POST` | `/api/Chat/send` | Chat with AI Assistant | ❌ |
| `GET` | `/api/User/profile` | Get current user profile | ✅ |


---
*Developed with ❤️ for Graduation Project 2026*

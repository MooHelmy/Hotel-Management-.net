# 🏨 Hotel Management System

A modern **Hotel Management System REST API** built with **ASP.NET Core 10** and **C#**, designed to manage hotels, rooms, guests, employees, reservations, and payments through a clean and scalable backend architecture.

The project focuses on building a structured backend using **RESTful APIs**, **JWT Authentication**, **ASP.NET Identity**, **Entity Framework Core**, and **SQL Server**.

---

## 🚀 Features

### 🔐 Authentication & Authorization

* User registration and authentication
* JWT-based authentication
* Secure password management using ASP.NET Identity
* Role-based authorization
* Protected API endpoints

### 🏨 Hotel Management

* Create and manage hotels
* Retrieve hotel information
* Update hotel details
* Delete hotels

### 🛏️ Room Management

* Create and manage hotel rooms
* Room availability management
* Room status tracking
* Associate rooms with hotels

### 👤 Guest Management

* Guest registration and management
* Store guest information
* Retrieve guest details
* Manage guest records

### 📅 Reservation Management

* Create hotel reservations
* Manage reservation details
* Associate guests with rooms
* Track reservation status
* Manage check-in and check-out information

### 💳 Payment Management

* Record payments
* Associate payments with reservations
* Track payment information

### 👨‍💼 Employee Management

* Manage hotel employees
* Store employee information
* Associate employees with hotels

---

## 🏗️ Architecture

The project follows a **layered architecture** to keep responsibilities separated and make the application easier to maintain and extend.

```text
Hotel-Management-.net
│
├── Controllers
│   ├── AuthController
│   ├── EmployeeController
│   ├── GuestController
│   ├── HotelController
│   ├── PaymentController
│   ├── ReservationController
│   └── RoomController
│
├── Data
│   └── Database Context & Configuration
│
├── DependencyInjection
│   └── Service Registration
│
├── Dtos
│   └── Request & Response DTOs
│
├── Entities
│   ├── Amenity
│   ├── ApplicationUser
│   ├── Employee
│   ├── Guest
│   ├── Hotel
│   ├── Payment
│   ├── Reservation
│   └── Room
│
├── Exception
│   └── Exception Handling
│
├── Interfaces
│   └── Service Contracts
│
├── Services
│   └── Business Logic
│
├── Migrations
│   └── Entity Framework Core Migrations
│
├── Program.cs
├── appsettings.json
└── HotelManagement.csproj
```

The repository currently contains dedicated controllers for authentication, employees, guests, hotels, payments, reservations, and rooms, alongside separate entity, DTO, interface, service, and dependency-injection layers.

---

## 🛠️ Tech Stack

| Technology                    | Purpose                     |
| ----------------------------- | --------------------------- |
| **C#**                        | Backend development         |
| **ASP.NET Core 10**           | Web API framework           |
| **Entity Framework Core 10**  | ORM & database access       |
| **SQL Server**                | Relational database         |
| **ASP.NET Identity**          | User & identity management  |
| **JWT Bearer Authentication** | API authentication          |
| **Scalar**                    | API documentation & testing |
| **REST API**                  | Client-server communication |
| **Dependency Injection**      | Service management          |

The project targets **.NET 10** and uses ASP.NET Core JWT Bearer authentication, ASP.NET Identity Entity Framework Core, EF Core SQL Server, OpenAPI, and Scalar.

---

## 🔄 API Flow

```text
Client
   │
   ▼
ASP.NET Core Web API
   │
   ├── Authentication / Authorization
   │
   ├── Controllers
   │
   ├── DTOs
   │
   ├── Services
   │
   ├── Interfaces
   │
   └── Entity Framework Core
            │
            ▼
        SQL Server
```

---

## 🔑 Authentication

The API uses **JWT (JSON Web Tokens)** to secure protected endpoints.

Typical authentication flow:

```text
Register
   ↓
Login
   ↓
JWT Token
   ↓
Authorization Header
   ↓
Protected API Endpoints
```

Example:

```http
Authorization: Bearer <your-jwt-token>
```

---

## ⚙️ Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/MooHelmy/Hotel-Management-.net.git
```

### 2. Navigate to the project

```bash
cd Hotel-Management-.net
```

### 3. Configure SQL Server

Update the database connection string inside:

```text
appsettings.json
```

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=HotelManagement;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Replace `YOUR_SERVER` with your SQL Server instance.

### 4. Restore dependencies

```bash
dotnet restore
```

### 5. Apply Entity Framework migrations

```bash
dotnet ef database update
```

### 6. Run the application

```bash
dotnet run
```

The API will start using the configured ASP.NET Core environment.

---

## 📚 API Documentation

The project includes **OpenAPI support** and **Scalar** for exploring and testing the API endpoints.

After running the application, open the generated API documentation endpoint provided by the application configuration.

---

## 📌 Main Modules

```text
Authentication
     │
     ├── Users
     └── JWT

Hotel
     │
     ├── Rooms
     ├── Employees
     └── Amenities

Guest
     │
     └── Reservations
             │
             └── Payments
```

---

## 🎯 Project Goals

This project was built to demonstrate practical backend development concepts, including:

* RESTful API design
* Clean separation of responsibilities
* Authentication and authorization
* Database modeling
* Entity Framework Core
* Dependency Injection
* DTO-based API communication
* Service-oriented business logic
* SQL Server integration
* Scalable backend architecture

---

## 🔮 Future Improvements

Potential future improvements include:

* [ ] Online payment gateway integration
* [ ] Advanced booking availability validation
* [ ] Email notifications
* [ ] Admin dashboard
* [ ] Hotel analytics and reports
* [ ] Image upload for hotels and rooms
* [ ] Advanced search and filtering
* [ ] Pagination and sorting
* [ ] Unit & integration testing
* [ ] Docker support
* [ ] CI/CD pipeline
* [ ] Redis caching

---

## 👨‍💻 Author

**Mohamed Helmy**

Computer Science & Artificial Intelligence Graduate

Backend & Software Developer

### GitHub

[github.com/MooHelmy](https://github.com/MooHelmy)

---

## ⭐ Support

If you find this project useful or interesting, consider giving it a ⭐ on GitHub.

**Built with C#, ASP.NET Core, Entity Framework Core & SQL Server.**

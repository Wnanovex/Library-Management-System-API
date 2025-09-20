# Library Management System API

A full-stack **Library Management System** consisting of:

- 🖥️ ASP.NET Core **Web API** (.NET 8)
- 🌐 Simple **React.js Web UI** — displays all books
- 📱 Basic **Flutter Mobile App** — displays all books
- 💼 Modular backend with BLL, DAL, DTOs

> 🛠️ Frontend is currently limited to **viewing books only**. More features to come!

## 🛠️ Features

- 🔐 User authentication with SQL (login system)
- 📖 Manage books, authors, and categories
- 👤 Manage library members
- 📚 Issue/return books
- 💰 Fine calculation using SQL functions
- ⚡ High performance using **stored procedures**
- 🧾 Cleanly separated logic using **3-tier architecture**

## 🔗 API (ASP.NET Core)

### 📌 Path: `LibraryManagementSystem.API/`

- Built with ASP.NET Core 8
- Exposes REST API to manage books, members, issues, etc.

### ▶️ Run the API

```bash
cd LibraryManagementSystem.API
dotnet run
```

## React Web UI (Minimal)

### Path: library-web-ui/

- Displays a list of books fetched from the API

### Run the Web UI

```bash
cd library-web-ui
npm install
npm start
```

## Flutter App (Minimal)

### Path: library_app/

- Displays a list of books using API

- Uses Dart http package

### Run the Flutter App

```bash
cd library_app
flutter pub get
flutter run
```


## 🚀 Getting Started

### 🧱 Prerequisites

- Visual Studio (with .NET Desktop Development workload)
- SQL Server (any edition)
- SQL Server Management Studio (SSMS)
- .NET SDK (.NET 8)
- nodejs
- A code editor like Visual Studio Code
- Flutter SDK
- Android Studio
- Android Emulator or physical Android

---

### ⚙️ Setup Instructions

#### 1. **Clone the Repository**

```bash
git clone https://github.com/Wnanovex/Library-Management-System-API.git
cd Library-Management-System-API
```

#### 2. **Restore the database**

  1. Place the LibraryDB.bak file in a folder accessible by SQL Server (e.g., C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\Backup\)

  2. Open SQL Server Management Studio (SSMS) and connect to your server

  3. Click New Query

  4. Restore the database
 ```sql
    RESTORE DATABASE LibraryDB
    FROM DISK = 'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\Backup\LibraryDB.bak';
```

#### 3. **Open the solution**

Open LibraryManagementSystem.UI.sln in Visual Studio

#### 4. **Configure the database connection**

Edit the connection string in `LibraryManagementSystem.DAL\DataAccessSettings.cs`

```csharp
    string connectionString = "Server=YOUR_SERVER_NAME;Database=LibraryDB;User Id=your_user;Password=your_password;";
```

#### 5. **Build and run**

- Build/Run LibraryManagementSystem.API/

- Build/Run library-web-ui/
  
- Build/Run library_app/


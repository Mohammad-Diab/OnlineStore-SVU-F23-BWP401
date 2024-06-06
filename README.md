# Online Store Application

## Description

This is a web application developed as a homework project for the **Web Programming I  (BWP401)** course at **Syrian Virtual University**. The Online Store application allows users to browse, purchase or sell products online.

## Prerequisites

- Visual Studio 2022 or later
- SQL Server 2016 or later
- .NET SDK 6

## How to Run the Application

### Step 1: Extract the Project
extruct the content of the zip file to a folder lets say OnlineStore

### Step 2: Open in Visual Studio
1.  Open Visual Studio.
2.  Select `File -> Open -> Project/Solution`.
3.  Navigate to the folder and open the solution file (`OnlineStore.sln`).

### Step 3: Prepare SQL Server Database
1.  Open SQL Server Management Studio (SSMS).
2.  Connect to your SQL Server instance.
3.  Create a new database named `OnlineStoreDB`. 
4. Open a new query window and execute the SQL script to create tables and schema. You can find the script in `script.sql` file.

### Step 4: Fix Configurations

#### API Configuration

1.  Navigate to the `API` project in your solution.
2.  Open `config.json`.
3.  Update the connection string with your SQL Server details:
```
{
  "connectionString": null,
  "databaseServer": "127.0.0.1,1433",
  "databaseName": "BWP401F23",
  "databaseUsername": "sa",
  "databasePassword": "your_decrypted_password",
  "serverUrl": "http://localhost/API/",
  "clientUrl": "http://localhost/API/WebStore",
  "imageDirectory": "Upload\\Image"
}
```

#### UI Configuration
1.   Navigate to `wwwroot/script/common.js`.
2.  Update the API and Client URLs:
```
var ServerUrl = "http://localhost/API/";
var ClientUrl = "http://localhost/WebStore";
```

### Step 5: Build and Run the Application

#### API

1.  Set the API project as the startup project.
2.  Press `Ctrl+F5` to build and run the API.

#### UI 
*Ui does not need any kind of build*

### Step 6: Deploy and Start

1.  Deploy the API and UI to your hosting environment.
2.  Make sure the API is accessible to the UI by adjusting the API URL in the UI configuration file.
3.  Start the services and ensure they are running correctly.
# Product Management API

This is a simple RESTful API for managing products. It supports features such as retrieving all products or active products, getting a product by its ID, updating product descriptions, and toggling product active status. The API is documented using Swagger, and it uses an MSSQL database for storage.

## Features

- Get all products
- Get active products
- Get a product by its ID
- Update product descriptions
- Toggle product active status

## Prerequisites

Before running this project, ensure you have the following installed:

- **.NET 8.0** or later
- **SQL Server** (MSSQL)
- **Visual Studio 2022 or later** (with .NET development workload installed)
- **SQL Server Management Studio** (SSMS) or a similar database management tool for managing MSSQL databases (optional)
- **Swagger UI** (automatically available for API documentation)

## Setting Up the Project

### 1. Clone the repository:

First, clone the repository to your local machine using the following command:

```bash
git clone https://github.com/dcmxero/Eshop.git
```
Alternatively, you can open Git Bash or your terminal and run the same command.

### 2. Restore the dependencies:
Navigate to the cloned project folder and restore the NuGet packages:

```bash
cd Eshop
dotnet restore
```
### 3. Update the database connection string:
Example of appsettings.json file with your MSSQL database connection details.

Example configuration in appsettings.json for local database:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=eshop;Trusted_Connection=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  },
  "AllowedHosts": "*",
  "UseMockProductService": true
}
```
### 4. Apply migrations:
Apply the migrations to set up the database:

```bash
dotnet ef database update
```
Alternatively, you can use Visual Studio's Package Manager Console:

```bash
Update-Database
```

## Running the Application
### 1. Set the Startup Project to WebApi
Ensure that the WebApi project is set as the Startup Project in Visual Studio:

Right-click on the WebApi project in the Solution Explorer.
Select Set as StartUp Project.

### 2. Use IIS Express
By default, Visual Studio uses IIS Express for local development. To run the application with IIS Express:

In the Solution Explorer, ensure that your WebApi project is selected as the active project.

Press Ctrl + F5 or click on the Start Without Debugging button to run the application with IIS Express.

Alternatively, you can click the Start button (green play button) to run with debugging.

The application will start, and you will see the following output in the browser:

```arduino
https://localhost:5001
```
This URL indicates that the application is running on your local machine using IIS Express.

API Documentation with Swagger
Swagger is automatically integrated into the application. Once the application is running, you can access the Swagger UI by navigating to:

```bash
https://localhost:5001/swagger
```
Swagger will provide a user-friendly interface to explore and test the available API endpoints.

Available Endpoints
### 1. Get All Products
```url
GET /api/v1/Products/all
GET /api/v2/Products/all
```
Returns a list of all products.

### 2. Get Active Products
```url
GET /api/v1/Products/active
GET /api/v2/Products/active
```
Returns a list of all active products.

### 3. Get Product By ID
```url
GET /api/v1/Products/{id}
```
Returns a product by its ID.

### 4. Update Product Description
```url
PUT /api/v1/Products/{id}/description
```
Updates the description of a product by its ID.

### 5. Set Product active status
```url
PUT /api/v1/Products/{id}/activate
```
Activate product.
```url
PUT /api/v1/Products/{id}/deactivate
```
Deactivate prodcut.

## Running Unit Tests
To run the unit tests, you can use the following command:

```bash
dotnet test
```
This will execute the test cases for the application and display the results in the terminal.

Alternatively, you can run the unit tests directly within Visual Studio using the built-in test runner.

## Technologies Used
.NET 8.0 for API development
Entity Framework Core for database management
SQL Server (MSSQL) as the database provider
Swagger for API documentation
XUnit for unit testing
Visual Studio 2022+ for development

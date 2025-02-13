# ASP.NET Web Application - Customer Analysis API

## Prerequisites
Ensure you have the following installed before running the project:

1. **.NET SDK & Runtime**
   - Download and install the latest .NET SDK from [Microsoft .NET](https://dotnet.microsoft.com/en-us/download/dotnet)

2. **ASP.NET Core**
   - Install the necessary ASP.NET Core dependencies.

3. **PostgreSQL Database**
   - Download and install PostgreSQL from [PostgreSQL Official Site](https://www.postgresql.org/download/)

## Database Setup
1. Open **PostgreSQL** and create a new database.
2. Use the following connection string to configure the database:
   ```
   Host=localhost;Port=5432;Database=Tasks;Username=postgres;Password=postgres
   ```
3. Ensure the database is up and running before starting the application.

## Running the Application
1. Clone or download the project repository.
2. Open the solution (`.sln`) file in **Visual Studio**.
3. Restore dependencies and build the project.
4. Run the web application.

## API Endpoints
The main page displays all API endpoints and triggering options.

### Customer Analysis APIs
- **Total Number of Customers (Within a date range)**
- **Total Number of Orders (Within a date range)**
- **Average Order Value (Within a date range)**

### Sample API Requests
- **Refresh Data:**
  ```
  GET https://localhost:7048/api/action/RefreshData?isModifyExisting=true
  ```
- **Get Total Orders:**
  ```
  GET https://localhost:7048/api/action/GetTotalOrders?startDate=2024%2F02%2F01&EndTime=2025%2F04%2F01
  ```
- **Get Average Order Value:**
  ```
  GET https://localhost:7048/api/action/GetOrdersAverageValue?startDate=2024%2F01%2F02&EndTime=2025%2F02%2F01
  ```
- **Get Total Customers:**
  ```
  GET https://localhost:7048/api/action/GetTotalCustomers?startDate=2024%2F01%2F01&EndTime=2025%2F01%2F01
  ```

## Notes
- Ensure PostgreSQL is running and properly configured.
- Modify the connection string in `appsettings.json` if needed.
- Replace `7048` with the actual port your application is running on.

## License
This project is for internal use. Modify and distribute as needed.


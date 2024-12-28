# Rise_ContactsApp

## overview
this is is allows you to manage contacts. You can create, read, update, delete, and search for contacts. this repo is enable to getall the functionality of this app by using swagger to get the exposed endpoints.
If you want to use also in UI you can clone also this repository https://github.com/YoniThee/rise-contacts-app.git and connect easealy to this server.

### Introduction
Rise ContactsApp is designed to be a lightweight solution for managing contacts. Whether you are building a personal address book or a larger contact management system for a team, this API provides simple CRUD operations along with search functionality. It exposes the following endpoints for interacting with contact data:

* GET /Contacts: List all contacts with pagination
* POST /Contacts: Create a new contact
* GET /Contacts/{id}: Retrieve a contact by ID
* PUT /Contacts/{id}: Update a contact by ID
* DELETE /Contacts/{id}: Delete a contact by ID
* GET /Contacts/search: Search contacts by query

### Installation
Follow these steps to install and set up the Rise ContactsApp on your local machine.

Clone the Repository
Start by cloning the repository from GitHub:
'''
git clone https://github.com/YoniThee/Rise_ContactsApp.git
cd Rise_ContactsApp
'''
### Database Setup
Database Setup
The app is using Entity Framework Core to handle migrations and interact with a SQL Server database. Follow the steps below to configure the database and ensure it’s ready for use.:
#### 1. Create the Database in SQL Server
1. Open SQL Server Management Studio (SSMS) and connect to your SQL Server instance.
2. Right-click on the Databases node in SSMS and select New Database.
3. Name the database contacts_db and click OK.
#### 2.Configure the Connection String
Next, you need to configure your application to connect to the SQL Server database.

Open the appsettings.json file in your project and add a connection string for your SQL Server instance under the ConnectionStrings section.
currently there is default connectionstring there but you need to change it by your DB
EXAMPLE:
```
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=contacts_db;User Id=sa;Password=yourStrongPassword;"
  },
```
#### 3. Create the Migration
Entity Framework Core uses migrations to generate and update database schemas based on your models.

1. **Create a Migration:** Open a terminal or command prompt, navigate to the project folder, and run the following command:
```
dotnet ef migrations add InitialCreate
```
This will generate a migration file that contains the SQL commands to create the database schema for the Contact model (i.e., the Contacts table).

2. **Apply the Migration** to the Database: After generating the migration, you need to apply it to the database to create the required tables.
```
dotnet ef database update
```
This command will apply the migration to your contacts_db database, creating the Contacts table based on the Contact model.

#### 4.Verify Database Creation
After running the migration, you can verify the table creation by opening SQL Server Management Studio (SSMS) and checking the contacts_db database.

Expand your contacts_db database.
1. Right-click on Tables and select Refresh.
2. You should see a Contacts table listed.
3. You can also run the following SQL query in SSMS to check:

```
SELECT * FROM Contacts;
```
Since the table is empty initially, it should return no rows.

### Running the Application
Once the database is set up and connected, you can run the application locally.
```
dotnet run
```
OR run it from your IDE(visual studio for example)
This will start the application, and it will be accessible at https://localhost:5001 or http://localhost:5000 (depending on your settings - its can be another ports).

## Using Docker
Because this app is based on 2 servers(app and DB_server) its will be better to build the 2 servers into one container and enable thues servers to connect
fir this we have "Docker-compose" file
### 1.Build and Start the Containers:

You can use the docker-compose command to build the images and start the containers for both the app and the SQL Server database.

Run the following command in the project root directory (where the docker-compose.yml file is located):
```
docker-compose up --build
```
This command does the following:

* Builds the contacts_app image from the Dockerfile.
* Starts the contacts_app container and the sqlserver container.
* Maps the necessary ports:
  * The app will be available on port 8080 and 8081 on your localhost.
  * The SQL Server database will be available on port 1433.
* Initializes the database container with the provided environment variables.
### 2. Access the Application:
Once the containers are up and running, you can access the app in your browser at:
```
http://localhost:8080
```
The app will be running on port 8080 by default. You can also access the Swagger API documentation at:
```
http://localhost:8080/swagger
```
### 3. Database Connection:

The contacts_app service is set up to connect to the sqlserver service using Docker Compose's internal networking. The connection string is managed by environment variables and can be configured in the docker-compose.yml file if needed.

The SQL Server container (sqlserver) is set up with the following environment variables:

* ACCEPT_EULA=Y: Accepts the SQL Server license agreement.
* SA_PASSWORD=yourPassword: Sets the system administrator password for SQL Server.
The SQL Server database is exposed on port 1433, but you do not need to manually connect to it unless you want to access the database directly.

### 4. Verify Database Creation:

The application will automatically apply migrations to the SQL Server database when the containers start. However, you can also check the database status manually using SQL Server Management Studio (SSMS):

Connect to the database using localhost,1433 as the server, and use the following credentials:
* Username: sa
* Password: yourPassword
After logging in, you can verify that the database has been created and that the Contacts table is present.



## API Endpoints
this is how thw swagger is look like
![image](https://github.com/user-attachments/assets/d62f2b98-c77e-499a-a722-dcf61359c9b7)

### 1. Get All Contacts
Retrieve a paginated list of contacts.

Endpoint: /Contacts
Method: GET

Query Parameters:
* page (integer): The page number of contacts to retrieve (default: 1).
* pageSize (integer): The number of contacts to return per page (default: 10).
Response:
200 OK: Returns a list of contacts in JSON format.

**Example**
```
  GET /Contacts?page=1&pageSize=10
```

### 2. Create New Contact
Create a new contact.

Endpoint: /Contacts
Method: POST

Request Body:

{
  "firstName": "Yoni",
  "lastName": "Levi",
  "phoneNumber": "123456",
  "address": "Tel Aviv"
}
Response:
200 OK: Returns the created contact object in JSON format.
**Example:**
```
  POST /Contacts
```
### 3. Get Contact by ID
Retrieve a specific contact by its ID.

Endpoint: /Contacts/{id}
Method: GET

Path Parameter:
id (integer): The unique identifier of the contact.
Response:
200 OK: Returns the contact data for the specified ID.
**Example:**
```
  GET /Contacts/1
```
### 4. Update Contact by ID
Update an existing contact by its ID.

Endpoint: /Contacts/{id}
Method: PUT

Path Parameter:
id (integer): The unique identifier of the contact.
Request Body:

{
  "firstName": "Yoni",
  "lastName": "Levi",
  "phoneNumber": "123456",
  "address": "Shoam"
}
Response:
200 OK: Returns the updated contact object.
**Example:**
```
  PUT /Contacts/1
```
### 5. Delete Contact by ID
Delete a contact by its ID.

Endpoint: /Contacts/{id}
Method: DELETE

Path Parameter:
id (integer): The unique identifier of the contact.
Response:
200 OK: Successfully deletes the contact.
Example:
```
  DELETE /Contacts/1
```
### 6. Search Contacts
Search contacts using a query string.

Endpoint: /Contacts/search
Method: GET

Query Parameter:
query (string): The search term to filter contacts by (e.g., name, phone number, etc.).
Response:
200 OK: Returns a list of contacts that match the search query.
**Example:**
```
  GET /Contacts/search?query=Yoni
```

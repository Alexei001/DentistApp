## DentistApp

It's a small ASP.NET Core MVC application that allow scheduling for medical procedure in a dental office.Acces to the registration functionality is allowed only after the creation of the User account, or Login.Implementing client functionality and Administration functionality based on "Client" Role or "Admin" role.
###### Technologies Used
- **ASP.NET CORE MVC ver. 5.0**
- **Entity Framework Core**
  - Code First, Migration
  - MS SQL Server/T-SQL
- **.NET Identity**
  - User,Role Management
  - Register New Client
  - Admin Policy Authorization
- **Layered Architecture**
- **Quartz.Net library**
 - Scheduling job for Email Notify
- **MailKit, SmtpClinet for Email Services**
- **Bootstrap library ver 4.3.1**
    ###### Home Page ![Home Image](https://github.com/Alexei001/DentistApp/blob/master/PrntScrProject/HomePage.jpg)
###### Client Functionality
- Get Rezervation by User Id
- CRUD operation with client Rezervation
  - Entity Framework Core
- Email Notification for client after new Booking!
  - MailKit,Smtp Client with Google services
- By default has Client Role
  - Has acces only for Client Role Functionality on navBar menu

###### Admin Functionality
- Booking Management
  - All List of Clients ![Clients Image](https://github.com/Alexei001/DentistApp/blob/master/PrntScrProject/allClients%20page.png)
    - CRUD Operation with Entity Framework Core 
    - Searching/Sorting/Filters all list of Clients
    - Implementing Pagination with 3 items on page 
- Role Management
  - All list of Roles ![Role Image](https://github.com/Alexei001/DentistApp/blob/master/PrntScrProject/AllRoles.png)
     - CRUD operation with .NET Identity Edit user With roles ![Clients Image](https://github.com/Alexei001/DentistApp/blob/master/PrntScrProject/EditUserWithRolesManagement.jpg)
     - Add or Delete user from Roles
     - Add or Delete New Roles
     - By default has Admin && Client Roles 
- User Management
  - All list of User ![Users Image](https://github.com/Alexei001/DentistApp/blob/master/PrntScrProject/AllUsers.jpg)
    - CRUD operation with .NET Identity
    - Register new User is done By confirmaton Email with MailKit and SmtpClient
    - Delete User is done only after Removing all the Roles and Client Rezervation
- Scheduled Jobs
  - Implementing Email Notification one day Before Client Rezervation ![Schedule Image](https://github.com/Alexei001/DentistApp/blob/master/PrntScrProject/Scheduling%20Jobs.jpg
  - Job Scheduling system
    - Implementing Quartz.NET 
  - Trigger
    - Repeat Daily after 24 hours
    - Start at 23:55 PM 
- Doctor
- Procedure

###### Register Form ![Form Image](https://github.com/Alexei001/DentistApp/blob/master/PrntScrProject/RegisterForm.jpg)
- User Email
- User Name
- Password
- Confirma Password
- Model Validation
- Register new user is Done By Email Confirmation
  - Email confirmation With MailKit, SmptClient

###### Booking Form ![Booking Form](https://github.com/Alexei001/DentistApp/blob/master/PrntScrProject/BookForm.png "Image")
- Doctor
  - Choose from Enum list only 5 doctor
- Procedure
  - Choose frum Enum list only 5 procedure
- Available time
  - Choose on a calendar, DateTime format
- Client Name
- Client Email
  - Only Admin Role ,enabled to Change, by default is disabled
  - Based on User Registration Email
- Phone Number
- Comment
- Booking new Client
  - Email confirmation With MailKit, SmptClient

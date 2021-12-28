## DentistApp

It's a small ASP.NET Core MVC application that allow scheduling for medical procedure in a dental office.Acces to the registration functionality is allowed only after the creation of the User account, or Login.Implementing client functionality and Administration functionality based on "Client" Role or "Admin" role.
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
  - All List of Clients
    - CRUD Operation with Entity Framework Core
    - Searching/Sorting/Filters all list of Clients
    - Implementing Pagination with 3 items on page 
- Role Management
  - All list of Roles
     - CRUD operation with .NET Identity
     - Add or Delete user from Roles
     - Add or Delete New Roles
     - By default has Admin && Client Roles 
- User Management
  - All list of User
    - CRUD operation with .NET Identity
    - Register new User is done By confirmaton Email with MailKit and SmtpClient
    - Delete User is done only after Removing all the Roles and Client Rezervation
- Scheduled Jobs
  - Job Scheduling system
    - Implementing Quartz.NET 
  - Trigger
    - Repeat Daily after 24 hours
    - Start at 23:55 PM 
- Doctor
- Procedure

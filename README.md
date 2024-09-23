# InventorySystem App
# Content 
________________________________________________
- [Project description](#Project-description)
- [Who is this for?](#Who-is-this-for?)
- [Requirements](#Requirements)
- [What you will learn](#What-you-will-learn)
- [Technologies Used](#Technologies-Used)
- [Installation and Configuration Guide](#Installation-and-Configuration-Guide)
- [Testing and Validation](#Testing-and-Validation)
- [License and Credits](#License-and-Credits)
 ________________________________________________

 ![image](https://github.com/user-attachments/assets/3a7c88e8-b299-46c2-890e-f8b9dbb53c6c)
 ![image](https://github.com/user-attachments/assets/eddbcc50-1ada-40b0-91a6-618266443a4f)
 ![image](https://github.com/user-attachments/assets/db81a111-be61-4852-ac66-be5d0155580c)


## Project description
This project is a web application to manage RESTful properties, providing a series of endpoints that allow an efficient and flexible administration of both properties and the images associated with them. The microservice architecture is geared towards facilitating CRUD (Create, Read, Update, Delete) operations for properties and their images, as well as advanced queries that allow users to find properties according to specific criteria. 

## Who is it for?
- Anyone who wants to learn ASP NET MVC core from basics to advanced
- Anyone who wants to learn the latest changes with the new Microsoft framework
- Anyone interested in learning how to design large-scale projects
- Anyone interested in learning SOLID, Design Patterns and good practices

## Requirements
more than 12 months of knowledge of Cv, additionally prior knowledge in additional .Net Core (Dotnet), also with the Visual Studio 2022 IDE, with a database engine such as SQL Server Management Studio.

## What you will learn
- Learn the structure of the NET 8 project, Layered architecture
- Learn the basics of ASP NET Core
- Integrate with the Entity Framework and learn how to add more fields to users
- Integrate the Entity Framework along with early code migrations
- Authentication and authorization in ASP NET Core
- ASP NET Core sessions
- Create APIs and JSON

## Technologies Used
- .NET 8, Entity Framework Core, C# Language
- Swagger for API documentation
- Visual Studio and SQL Server applications

## Installation and Configuration Guide
- Clone the repository: https://github.com/WinDev-es/PropertiesIntegrations.git
- Configure the database in appsettings.json
- Open the PropertiesIntegrations.sln file, when you start in Visual Studio you deploy the PropertySystem Project and open the appsettings.json file and configure your connection string in the ConnectionStrings.DatabaseConnection session
- Open the package management console, put in the project "Percistence.Context" type "Add-Migration InitialMigration" Run
- Then type "Update-Database" Run
- You start Debugging (ctrl +F5)

## Testing and Validation
- Unit tests performed with xUnit.
- Integration testing using Postman.

## License and Credits
- MIT License
- Credits to Engineer Edelwin Antonio Molleda Marin


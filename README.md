# FilmAPI

## Introduction and Overview
This project involves using an EntityFramwork Code First workflow to create a database in SQL Server, as well as creating an ASP.NET Core Web API in C# for users to manipulate the data. The database consists of three tables; Franchise, Movie, Character. 

## Getting started
### Prerequisities
- SQL Server Management Studio (SSMS)

### Installation
1. Clone this repository
2. In `appsettings.json`, under "ConnectionStrings" change the "MoviesDb" data source to your SQL server name.
3. In the Package Manager Console type:
`add-migration initialDb`
`update-database`
This will create the database with dummy data from `MoviesDbContext.cs` in SMSS.

## Usage 
The API can be used to interact with the database through HTTP requests. There are different endpoints for the database tables that can be used perform GET, POST or PUT requests.

## Contributors
[Tobias Vetrhus](https://github.com/TobiasVetrhus)
[Ritwaan Hashi](https://github.com/ritwaan)
[Ine Bredesen](https://github.com/inemari)

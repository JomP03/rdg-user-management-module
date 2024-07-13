# README #

UMM - User Management Module.

In this module there we have the backend of the:

- User Management.


We follow the ONION architecture.

Here is a link for the project documentation:
(https://bitbucket.org/guilhermesousa03/rdg-data-administration-module/wiki/Home)[Documentation]


## Development


### Run the project ###

To run this project locally, you need to have a MySql database running. One way to do this is to use docker:

````bash
docker run --name mysql-dev -e MYSQL_ROOT_PASSWORD=rootpwd -d -p 3306:3306 mysql:latest
``````

If it's the first time you are going to use this database, use the update database command from 
EF Core (check next topic)

#### Migrations ####

For all the commands related to migrations, 
open the terminal on the Persistence project.

#### Update database ####

Either it's your first time using the database or you want to update it, with a new migration,
you need to run the following command:

````bash
dotnet ef database update
``````

#### Add migration ####

If you want to add a new migration, you need to run the following command:

````bash
dotnet ef migrations add <migration_name>
``````

#### Remove migration ####
If you want to remove the last migration, you need to run the following command:

````bash
dotnet ef migrations remove
``````


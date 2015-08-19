# ASP.NET-MVC
Here there are .NET Projects


## Catalogue Project

This is a project which shows a catalogue with products. There are three states to operate it:
* **NOT logged in users** */Visitors/* 
  * They would be able to see all uploaded products with comments and ratings over them.
* **Logged in users**
  * They are be able to see the products - their details. They can comment and rate them too.
* **Admin users**
  * They are able to add categories, add products, add their pictures. Also they are able to comment and rate them.



`Registration` - It requires to input *Username* and *Password*. They must be with length 6 and above. In addition 
username must not repeat with another written before. You are able to write *Full name* and *E-mail* but they are not
required for registering.

`Log In` - It works as it has to.  Registered user must write his/hers username and password to enter the prfoile.

`My Profile` - Additional button appears when a user loggs in. From there he/she is able to change password or
full name and e-mail

`Home` - This is the home page of the project. There you are able to see the last three uploaded products. On the 
left part you can see a menu with all written categories and a search input field. If you click at any of the 
categories you would be able to see the products with its category name. If you write a name in the search input
field you would be able to find any of the products with its category name.

`Products` - This is the place where all products are held. Every user is able to see and choose from it. If you 
logg in as *Admin* you are able to create and edit the products from the list.

`Categories` - This menu appears when *Admin* is logged in. From there the admin can add new category names and edit
previous written.

`Contacts` - This menu shows a google map location.


<hr />


> ADO.NET Entity Framework is used to make the connection between the Database and the MVC Controllers.

> MS SQL Server is used as a Database 

> You have to start SQL Server from the configuration manager to be able to start and check the project


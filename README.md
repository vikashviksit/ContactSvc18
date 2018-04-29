# ContactSvc18

This is a source code of a production ready application designed and implemented for maintaining contact information. 
 
Functionality:  - add a contact - list contacts - edit contact - delete contact 
 
Contact model: - First Name - Last Name - Email - Phone Number - Status (Possible values: Active/Inactive) 

Project has been developed as an Web API with MVC architecture  with web client in as a single page application (SPA).
The storage of records are done in the azure hosted SQL server database. 
The DataManger module component in the application provides server side caching mechanism to avoid round trips with database server.
The project has been developed using VS 2017 devlopment envirnonment on Windows.

Service is hosted on azure cloud at:
http://contactsvc18.azurewebsites.net/


Server side with full cache support for database is hosted at:
http://contactsvc20180427063825.azurewebsites.net/

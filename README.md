## SOLID issues

# Single Responsibility.
1. The code is stored in one class and doesn't use single responsibility principle. 
First of all we need to separate them. The database requests will be stored in the Repository. 
2. Data biding and other logic will be storesd in the Service. As a result:
- controller will have two DI initialisation in the constructor (service and logger)
- four lines in the SelectFromDBAndShow method 
			(log output "Get started", get data from the service, log output "Get ended", and the return result)

# Open/Closed
1. Doing the update or fix of the code we are changing almost all the code. 
To fix that we are going to separate the logic (as was described in the Single responsibility principle). 
We are going to use the Entity Framework as ORM. 
That is going to help to improve updating and adding new features. 
To use EF we are going to use the model bind instead of using the dynemic type.
Less code in classes - more closed to changes.
2. The dynamic type is a bad practice because it is wasting time at the runtime to check the type. 
The strong typing are checking at the compilation so it going to improve the speed and the code quality. 

# Dependency Inversion
1. We are going to implement the Onion architecture. It helps to separate the logic and split the code on more testable parts.
This architecture will help to improve this principle issues by not setting the high module to dependents on low modules.


## Use Entity Framework
1. What are the problems with the existing performance?
- There is no primary key. The primary key helps identify the row index and sets some unique identity etc.
- In some cases we need cleas ado.net (for big amount of data), but in current situation it's better to use model biding as it do EF.
- database connection settings in the controller's constructor - bad idea. It become not testable and hurd supported.
- Connection is opening and there is no connection close.
- In such type of work with database connections is better to free the memory after use.
- dynamic type!
2. What do you think needs to be improved?
- Change ado.net into EF.
- Add the primary key to the table
- Change the not strong typed type into strong typed
- Separate the logic into defferent tetable classes (use the solid principles to architect it).
3. How should test coverage be developed?
- All test need to have the successfule, failed and/or error results

### Expanding the capabilities of the controller
1. What response types and how can they be used in this project?
- Create a parameterized response type with hte error message
2. How do you make this clear to Swagger?
- Not to use a Dynamic type)))


### Good idea to create tests for the exception handler and the app load functionality. 
### But it might be too much for the homework.





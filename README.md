# Star Wars Fleet Intel


A full-stack implementation of the **Star Wars Fleet Intel** system, including:


1. **.NET API Backend**  
2. **Angular Web Frontend**


The system consumes data from the [Star Wars API (SWAPI)](https://swapi.dev/) and displays starship information through modern, scalable frontends.


---


## Project Structure


---


## Task 1: .NET API Backend


**Architecture & Design Patterns**


- Clean/Onion architecture with distinct Domain, Application, and Infrastructure layers
- **SwapiFacade** service simplifies SWAPI interactions
- **Chain of Responsibility** for starship data validation and enrichment
- **Strategy Pattern** for currency conversion
- **Decorator Pattern** for runtime modifications (Shield Boost, Targeting Computer)


**Performance & Reliability**


- Object Pooling for frequently instantiated classes
- Global exception handling using `ProblemDetails` (RFC 7807)
- Structured logging with `BeginScope` for request correlation


**Testing**


- Comprehensive unit and integration tests
- Request DTO validation using FluentValidation
- Test data generated using AutoFixture and Bogus


**Run Instructions**


1. Navigate to the `/backend` folder:  
```bash
>>cd backend


Restore packages:


>>dotnet restore




Run the API locally:


>>dotnet run


Access the API at:
https://localhost:5001 or http://localhost:5000


Hosted API URL:
http://starwars.runasp.net/index.html




Task 2: Angular Web Frontend


Architecture & Practices


Standalone Angular components


Angular Signals for state management


ChangeDetectionStrategy.OnPush for optimized rendering


Functionality


Displays a list of starships (name, model, manufacturer)


Reactive search filter for quick lookups


Detail view/modal for selected starships


Styling


Structured using 7-1 pattern


Follows BEM naming convention


Responsive and modern UI design


Testing


Component tests for user interactions and template rendering


Run Instructions


Navigate to /angular-frontend:


>>cd angular-frontend




Install dependencies:


>>npm install




Serve the application:


>>ng serve




Open browser at: http://localhost:4200


Run tests (default use chrome prowser to run tests):


>>ng test








# test-dennemeyer
This project is created to test my code skills. The .NET-based API supports managing items and facilitating a proposal system between different parties/companies.

# Prerequisites
Before running the application, ensure you have the following installed:
	•	.NET 9 SDK
	•	PostgreSQL
	•	Entity Framework Core
	•	Git

A tool to test the API: Swagger UI (built-in)

# Running the application
You can run project in with Visual Studio/Rider run or you can:
- clone the repository
  git clone test-dennemeyer
  cd TestProjectDennemeyer
- Update appsettings.json with your PostgreSQL connection string
- dotnet run
- Open [http://localhost:5133/swagger](http://localhost:5133/swagger/index.html) in your browser.

The data is seeded, database should be created on the first run of the project, but you can use dotnet ef database update.

# Testing the application

You can use other options like Postman, but the easiest way is to use Swagger by[ http://localhost:5133/swagger](http://localhost:5133/swagger/index.html). 

GET
/api/Proposal/History/{itemId}
Get proposal history for an item
---
POST
/api/Proposal
Create a new proposal
---
POST
/api/Proposal/{proposalId}
Create a counter-proposal
--
POST
/api/Proposal/{proposalId}/decision
Finalize decision (approve/reject proposal)
--

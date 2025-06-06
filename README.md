# Introduction

Porting over my sportsbook AI project that I generally did not put into source control. I decided to scrap that project, use the concepts I was using in that project, and build a better project from that. I am going to start with the WNBA and UFL seasons since I am dealing with a lot less teams and therefore, a lot less data.

I am also creating this to really get me used to fully making projects in VS Code and ween myself off Visual Studio 2022.

I so far have run these commands to create this project

```powershell
git init

# Pin to .NET 8
dotnet new globaljson --sdk-version 8.0.408

# Create the solution
dotnet new sln -n SportsBookAI

mkdir -p src/SportsBookAI.EntryConsole
mkdir -p src/SportsBookAI.Core
mkdir -p tests/SportsBookAI.Core.Tests
mkdir -p src/SportsBookAI.Core.Mongo
mkdir -p src/SportsBookAI.Api
mkdir -p src/SportsBookAI.Frontend.React.JS

dotnet new console -n SportsBookAI.EntryConsole -o src/SportsBookAI.EntryConsole
dotnet new classlib -n SportsBookAI.Core -o src/SportsBookAI.Core
dotnet new xunit -n SportsBookAI.Core.Tests -o tests/SportsBookAI.Core.Tests
dotnet new classlib -n SportsBookAI.Core.Mongo -o src/SportsBookAI.Core.Mongo
dotnet new webapi -n SportsBookAI.Api -o src/SportsBookAI.Api

dotnet sln add src/SportsBookAI.EntryConsole/SportsBookAI.EntryConsole.csproj
dotnet sln add src/SportsBookAI.Core/SportsBookAI.Core.csproj
dotnet sln add tests/SportsBookAI.Core.Tests/SportsBookAI.Core.Tests.csproj
dotnet add tests/SportsBookAI.Core.Tests/SportsBookAI.Core.Tests.csproj reference src/SportsBookAI.Core/SportsBookAI.Core.csproj
dotnet sln add src/SportsBookAI.Core.Mongo/SportsBookAI.Core.Mongo.csproj
dotnet add src/SportsBookAI.Core.Mongo/SportsBookAI.Core.Mongo.csproj reference src/SportsBookAI.Core/SportsBookAI.Core.csproj
dotnet add src/SportsBookAI.EntryConsole/SportsBookAI.EntryConsole.csproj reference src/SportsBookAI.Core/SportsBookAI.Core.csproj
dotnet add src/SportsBookAI.EntryConsole/SportsBookAI.EntryConsole.csproj reference src/SportsBookAI.Core.Mongo/SportsBookAI.Core.Mongo.csproj

dotnet sln add src/SportsBookAI.Api/SportsBookAI.Api.csproj
dotnet add src/SportsBookAI.Api/SportsBookAI.Api.csproj reference src/SportsBookAI.Core/SportsBookAI.Core.csproj
dotnet add src/SportsBookAI.Api/SportsBookAI.Api.csproj reference src/SportsBookAI.Core.Mongo/SportsBookAI.Core.Mongo.csproj

cd src/SportsBookAI.Frontend.React.JS
npm create vite@latest sportsbookai-frontend-react-js -- --template react
```

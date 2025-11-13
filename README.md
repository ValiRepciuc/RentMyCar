##Backend##
.NET 8 + EF Core 

Comenzi setup directorul backend

dotnet tool install --global dotnet-ef
dotnet restore
dotnet ef database update --project Domain --startup-project Presentation
dotnet run 


##Frontend##

Comenzi setup directorul frontend

npm i
npm run dev

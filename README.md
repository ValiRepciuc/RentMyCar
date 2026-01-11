##Backend##
.NET 8 + EF Core 

Comenzi setup directorul backend

dotnet tool install --global dotnet-ef
dotnet restore
dotnet ef database update --project Domain --startup-project Presentation
dotnet run

**Configurare:**
- Fișierul `backend/Presentation/appsettings.json` conține configurația pentru development
- Pentru production, copiați `appsettings.json.example` la `appsettings.Production.json` și modificați valorile:
  - ConnectionStrings: date de conectare la baza de date
  - JWT:SigningKey: o cheie secretă de minim 32 caractere (generați una sigură!)
  - JWT:Issuer: URL-ul backend-ului
  - JWT:Audience: URL-ul frontend-ului 


##Frontend##

Comenzi setup directorul frontend

npm i
npm run dev

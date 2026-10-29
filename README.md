# TP5 - Gestion d'une flotte d'ordinateurs
Safi Achraf - EFREI 

## Lancement du projet

Démarrer les 4 projets dans des terminaux séparés :

1. API principale (port 5247) :
dotnet run --project GestionOrdinateurs.API --urls "http://localhost:5247"

2. API Auth (port 5250) :
dotnet run --project AuthentificationApi --urls "http://localhost:5250"

3. Gateway YARP (port 5010) :
dotnet run --project GestionOrdinateurs.Gateway --urls "http://localhost:5010"

4. Client Blazor (port 5165) :
dotnet run --project GestionOrdinateurs.Client --urls "http://localhost:5165"

Le client est accessible sur http://localhost:5165

## Comptes pour tester

- user1 : Admin
- user2 : Gestionnaire
- user3 : Gestionnaire

Pas de mot de passe requis.

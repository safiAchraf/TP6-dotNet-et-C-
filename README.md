# TP6 - Gestion d'une flotte d'ordinateurs
Safi Achraf - EFREI

[Projet GitHub](https://github.com/safiAchraf/TP6-dotNet-et-C-)

## Lancement du projet

Au premier lancement, copier `.env.example` en `.env`. Si le fichier existe déjà, le conserver.

À la racine de la solution :

```bash
docker compose build
docker compose up -d
```

Attendre le démarrage de Keycloak, puis ouvrir http://localhost:5000.
Le realm et les comptes de test sont importés automatiquement au premier lancement.

- API principale : http://localhost:5247/swagger
- API Auth : http://localhost:5250/swagger
- Gateway : http://localhost:5010
- Keycloak : http://localhost:8180/admin/ (`admin` / `admin`)

## Comptes pour tester

- user1 : Admin
- user2 : Gestionnaire
- user3 : Gestionnaire

Mot de passe pour les trois comptes : `123`.

Pour lancer les tests : `dotnet test`.
Pour arrêter les services : `docker compose down`.

# TP6 - Gestion d'une flotte d'ordinateurs
Safi Achraf - EFREI

## Lancement avec Docker

Docker et Docker Compose doivent être installés. Les commandes se lancent à la racine de la solution.

Au premier lancement, copier `.env.example` en `.env` :

```bash
cp .env.example .env
docker compose build
docker compose up -d
```

Le fichier `.env` contient le compte Docker Hub et le secret du client Keycloak. Il n'est pas envoyé sur GitHub.
Le secret d'exemple sert au TP ; sa valeur doit être identique pour Keycloak et l'API d'authentification.
Si un `.env` existe déjà, le conserver avec son secret actuel.

Attendre la fin du démarrage de Keycloak, puis ouvrir http://localhost:5000.

| Service | Adresse |
| --- | --- |
| Client Blazor | http://localhost:5000 |
| Gateway YARP | http://localhost:5010 |
| API principale | http://localhost:5247/swagger |
| API d'authentification | http://localhost:5250/swagger |
| Keycloak | http://localhost:8180/admin/ |
| PostgreSQL | localhost:5432 |

Pour arrêter les services :

```bash
docker compose down
```

Sous WSL, exécuter les commandes dans Ubuntu. Si la commande disponible est `docker-compose`, l'utiliser à la place de `docker compose`.
Le navigateur Windows doit pouvoir joindre les ports 5000, 5010 et 8180 de WSL.

## Comptes pour tester

| Identifiant | Mot de passe | Rôle |
| --- | --- | --- |
| user1 | 123 | Admin |
| user2 | 123 | Gestionnaire |
| user3 | 123 | Gestionnaire |

L'Admin peut modifier le statut vendu et accéder aux réparations. Le Gestionnaire consulte les ordinateurs.
Pour la console d'administration Keycloak : `admin` / `admin`.

## Keycloak et données

Le fichier `keycloak/import/my-realm-realm.json` crée le realm `my-realm`, le client `my-client`, les rôles et les comptes de test au premier démarrage.
Un realm déjà présent est conservé. La configuration Keycloak est stockée dans PostgreSQL.

La page de connexion appelle `POST /api/authentication/login-keycloak` avec un identifiant et un mot de passe.
L'ancien endpoint `POST /api/authentication/login` est conservé comme demandé dans le TP, mais l'API principale utilise les tokens Keycloak.

Les ordinateurs et réparations restent dans SQLite. Le fichier `parcinfo.db` est copié dans la sortie de l'API et dans son image Docker.

## Tests et workflow GitHub

Avec le SDK .NET 10 :

```bash
dotnet test
```

Le workflow lance les tests, puis construit et publie les quatre images avec Docker Compose.
Les secrets GitHub nécessaires sont `DOCKER_ACCOUNT`, `DOCKER_USERNAME` et `DOCKER_PASSWORD` (token Docker Hub).

Avant le rendu, lancer `dotnet clean`, puis zipper le contenu de la solution.

Projekt startowany przez docker compose up -d (wymaga zainstalowanego dockera)
Powinny wystartować 3 kontenery. Powinniśmy mieć dostęp do:
- PG admina: http://localhost:5050/browser
- Widoku aplikacji: https://localhost:5001

Jeżeli chcemy w pgadminie dodać naszą bazę danych, klikamy Add New Server:
Name: [dowolna nazwa]
Host Name: db
Port: 5432
Maintenance Database: MusicDB [może zostać postgres]
Username: postgres
Password: postgres


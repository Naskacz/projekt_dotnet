Projekt startowany przez docker compose up -d (wymaga zainstalowanego dockera)
Powinny wystartować 4 kontenery. Powinniśmy mieć dostęp do:
- PG admina: http://localhost:5050/browser
- Frontend w vue: http://localhost:5173
Serwer powinien słuchać na localhost:8080, a baza danych localhost:5432. Obecnie jednak nic pod tymi adresami nie znajdziemy.


Jeżeli chcemy w pgadminie dodać naszą bazę danych, klikamy Add New Server:
Name: [dowolna nazwa]
Host Name: db
Port: 5432
Maintenance Database: MusicDB [może zostać postgres]
Username: postgres
Password: postgres


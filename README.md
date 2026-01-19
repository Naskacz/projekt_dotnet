Projekt jest budowany przez polecenie (wymaga zainstalowanego dockera):
- docker compose up --build -d      <-- flaga '--build': buduje projekt, głównie zalecany przy pierwszym odpaleniu projektu, później zazwyczaj można pomijać; '-d': powoduje, że w konsoli nie zasypie nas logami z kontenerów

Powinny wystartować 4 kontenery. Powinniśmy mieć dostęp do:
- PG admina: http://localhost:5050/browser
- Frontend w vue: http://localhost:5173
- Swagger: http://localhost:8080/swagger

Serwer powinien słuchać na localhost:8080, a baza danych localhost:5432.

W pg admin:

login: admin@admin.com

password: admin


Jeżeli chcemy w pgadminie dodać naszą bazę danych (opcjonalne), klikamy Add New Server:

Name: [dowolna nazwa]

Host Name: db

Port: 5432

Maintenance Database: MusicDB (może zostać postgres)

Username: postgres

Password: postgres



Aby zaaplikować migracje z plików do naszej bazy danych (jeżeli aplikacja sama tego nie zrobiła):
- docker compose exec -it dotnet_music_app bash
- dotnet ef database update
- exit (wyjście z kontenera)

Aby zatrzymać aplikację w dockerze należy wpisać:
- docker compose down (-v)      <-- dodanie '-v' jest opcjonalne, skutkuje m.in. skasowaniem danych przed ponownym uruchomieniem aplikacji w dockerze
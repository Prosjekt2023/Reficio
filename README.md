![Nøsted logo](https://raw.githubusercontent.com/Prosjekt2023/Reficio/main/bacit-dotnet.MVC/wwwroot/nlogo.png)

## Før du starter
## Husk! Docker-Desktop-installasjon:
* Besøk den offisielle Docker-nettsiden.
* For mer instruksjoner besøk: https://www.docker.com/products/docker-desktop/
* Last ned og følg veiledning.

## Før du kjører programmet:
* Lag en database i MariaDB
    * Du kan installere lokalt i Docker. Se veiledningen 'MariaDb database som Docker container'.
  

* Sett inn en connection string i filen `appsettings.json`, der du ser `ConnectionString.DefaultConnection`


* Denne skal følge dette formatet:
    * server=localhost; user=root; database=ReficioDB; port=3306; password=Reficio`
    * Dersom du kjører database og server på samme maskin, kan du bruke `localhost` eller `172.17.0.1` som IP-adresse
    * Det er anbefalt å bruke port 3306, da dette er standard for MySQL og MariaDB


### MariaDb database som Docker container

1. Opprett MariaDb container.


```docker
docker run --name Reficio -e MYSQL_ROOT_PASSWORD=Reficio -p 3306:3306 -d mariadb:latest
```

```
docker ps
```
2. Verifiser at container har status 'Running'

3. Koble til container og logg på som root.

  ```
  docker exec -it Reficio bash
  ```
```
  mariadb -u root -p 
  ```

4. Skriv inn PASSORD som ble satt ved opprettelse av konteiner. I dette tilfellet: Reficio

  ```
  Enter password: Reficio 
  ```
5. Bruk SQL kommando 

```
  USE DATABASE ReficioDB;
```
6. Opprett alle samsvarende tabeller for web-applikasjon:
```
  Kopier og lim inn koded fra Create.sql filen.
```
7. Ferdig! Du kan nå kjøre programmet.
```
 dotnet run
```

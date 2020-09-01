# WebApi

Crea el proyecto
```
dotnet new webapi -o ApiMovies
code -r ./api-movies
```

## For Connection

Agrega los EntityFramework para conectar con postgres
```
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package NpgSql.EntityFrameworkCore.PostgreSQL.Design
```

## For Migrations

Para agregar migraciones y actualizar en la base de datos
```
dotnet ef migrations add initialMigration
dotnet ef database update
```

## For Dtos

Agrega el AutoMapper para manejar los DTOs
```
dotnet add package AutoMapper
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection
```

## For Tokens
```
dotnet add package System.IdentityModel.Tokens.Jwt
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
```

# Documentatio

`dotnet add package Swashbuckle.AspNetCore`

# Postgis

Crea una base de datos con extención postgis desde la consola de postgres
`createdb -T template_postgis -U postgres movies`

# Postgres

`docker run --name some-postgres -e POSTGRES_PASSWORD=postgres -d postgres`
```
docker run --name some-postgres --volume postgres-data:/var/lib/postgresql/data -e POSTGRES_PASSWORD=postgres -d postgres
docker exec -it some-postgres sh

createdb -U postgres apidb
psql -U postgres apidb
CREATE TABLE products (id int, name varchar(100));
INSERT INTO products (id, name) VALUES (1, 'Wheel');
SELECT * FROM products;
\q
exit

docker rm -f some-postgres
docker run --name api-postgres --volume postgres-data:/var/lib/postgresql/data -e POSTGRES_PASSWORD=postgres -d postgres
docker exec -it api-postgres sh

psql -U postgres apidb
INSERT INTO products (id, name) VALUES (2, 'Chela');
SELECT * FROM products;
\q
exit

```
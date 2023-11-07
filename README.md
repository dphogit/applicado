# Applicado 🥑💼

Practicing REST API development in C# with the new Minimal API.

Converting my Google Sheets file to a simple CRUD service.

## Application Setup

Standard dotnet commands can be used to run the respective projects.

`.vscode` tasks and launch settings have been configured to automate standard
workflows including:

- build and debug/run for the API
- running tests

To setup the application to communicate with the database (see next section),
it needs a connection string. Use `dotnet user-secrets` to set the connection:

```bash
dotnet user-secrets set "Postgres:ConnectionString" "Host=localhost;Database=<POSTGRES_DB>;Username=<POSTGRES_USER>;Password=<POSTGRES_PASSWORD>;"
```

For more about storage of app secrets in development and how to use
`dotnet user-secrets`, see the [Microsoft Documentation](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets).

## Database Setup

This project uses PostgreSQL and PgAdmin4 is setup with docker compose. In the
`.env` file at the root, set these variables:

```env
# Postgres - should be consistent with the connection string
POSTGRES_USER=
POSTGRES_PASSWORD=
POSTGRES_DB=

# PgAdmin
PGADMIN_DEFAULT_EMAIL=
PGADMIN_DEFAULT_PASSWORD=
```

The `<POSTGRES_DB>`, `<POSTGRES_USER>`, and `<POSTGRES_PASSWORD>` should be
consistent with what is set in the connection string when setting up the
user secrets.

Then go to `localhost:5050` and setup a new server using the above connection
details. In the connection tab, the host name/address should be the IPv4
address of the postgres container with `docker inspect postgres` under
`NetworkSettings.Networks.applicado_default.IpAddress`.

# comparehare

For a general idea of the app and its architecture, please read [ARCHITECTURE.md](ARCHITECTURE.md)

This project was generated with the [Angular Full-Stack Generator](https://github.com/DaftMonk/generator-angular-fullstack) version 3.3.0.

## Getting Started

### Prerequisites

- Dotnet Core 2.2 installed
- MySQL docker image instantiated and configured
- Nuget package manager
- Yarn

### Developing (order is important)

1. `cd src/CompareHare.React` then run `yarn` to install dependencies.

2.  Run `yarn start` to start the development server. It should automatically open the client in your browser when ready.

3. Once the React app is up, you may then start the API, which both spins up the Web API server as well as the Hangfire background jobs, one of which (the offer loader job) will require a static resource that Webpack serves up for mocking purposes. (for now, in "production" it will scrape the legit URLs)

4. First, let's restore Nuget packages. **Open a new terminal tab** and `cd src` and then run `dotnet restore`

5. Now, lets run any migrations. `cd CompareHare.Api` then (making sure the specified MySQL database is running - locally, Docker image) `dotnet ef database update`

6. Finally, lets get this puppy running. Stay in the `CompareHare.Api` folder, then run `dotnet run`

## Migrations
I forgot how to do this, here's how:

1. Update all of the entities/relationships in `CompareHare.Domain`

2. If adding any new entities, you need to make sure the DbContext has an entry for them, or else Dotnet won't see them! Also, update DbContext with any relationship additions/changes.

3. In a terminal window, `cd src/CompareHare.Api` then run the command to generate a new migration based off a diff between the current schema and the changes:
`dotnet ef migrations add <YourMigrationNameHere>`

4. If you hit issues, tag a `--verbose` on the end to debug the issue (very common - fix your code!)

5. Once the migration Up/Down that was created is sufficient, run the migration: `dotnet ef database update`

6. If anything fails, you can select to rollback changes with `dotnet ef database update <FULL text name of last successful migration>` (includes the numbers in the migration file name)
## Testing

1. Ensure all packages are restored: `cd src` then `dotnet restore`

2. Then, you should be able to run all XUnit tests with `dotnet test`

3. TBD: Front end testing with Jest

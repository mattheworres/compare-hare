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

1.  Run `yarn` to install dependencies.

2.  Run `yarn start` to start the development server. It should automatically open the client in your browser when ready.

3. Once the React app is up, you may then start the API, which both spins up the Web API server as well as the Hangfire background jobs, one of which (the offer loader job) will require a static resource that Webpack serves up for mocking purposes.

4. First, let's restore Nuget packages. `cd src` and then run `dotnet restore`

5. Now, lets run any migrations. `cd CompareHare.Api` then `dotnet ef database update`

6. Finally, lets get this puppy running. Stay in the `CompareHare.Api` folder, then run `dotnet run`

## Testing

1. Ensure all packages are restored: `cd src` then `dotnet restore`

2. Then, you should be able to run all XUnit tests with `dotnet test`

3. TBD: Front end testing with Jest

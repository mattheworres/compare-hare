# Why is Serilog.Sinks.MySql a third party DLL? Why not use Nuget?

Well, good question. Answer: There are no Nuget packages available at the moment that matches these requirements:

- Works on netcoreapp2.0
- Uses the open source (and permissively licensed) [MySqlConnector](https://github.com/mysql-net/MySqlConnector) over Oracle's `MySql.Data` connector

The base repo/Nuget package from [saleem-mirza](https://github.com/saleem-mirza/serilog-sinks-mysql) does not target netcoreapp2.0, and uses the `MySql.Data` connector.

The fork from [reinaldocoelho](https://github.com/reinaldocoelho/serilog-sinks-mysql) adds support for netstandard2.0 (subset of netcoreapp2.0), but continues to use the `MySql.Data` connector.

A fork from [mysql-net](https://github.com/mysql-net/serilog-sinks-mysql/tree/mysqlconnector) (on the `mysqlconnector` branch) swaps the connector out, but is still based off of saleem-mirza's .NET 4.5 stack.

This DLL was compiled from a [combination fork that Matt created](https://github.com/mattheworres/serilog-sinks-mysql)!

1.  Open SLN in Visual Studio (VS2017 Pro was used)
2.  Restore Nuget packages.
3.  Set Solution Configuration to `Release` instead of `Debug`
4.  Go to `Build` -> `Build Solution`.
5.  Find DLL in the `bin` folder of the project. Voila!

# Update (Feb 2019):
Updated the SLN to use `netcoreapp22` and it seems to build A-OK on Mac using `dotnet build -c Release`?

# Update (Dec 2023):
So this may all be for nought: there is a Serilog provider IN the package: <https://github.com/mysql-net/MySqlConnector/tree/master/src/MySqlConnector.Logging.Serilog>

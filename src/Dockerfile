# FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
# WORKDIR /app

# # Copy everything
# COPY . ./
# # Restore as distinct layers
# RUN dotnet restore
# # Build and publish a release
# RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/sdk:2.2
# WORKDIR /app
# COPY /app/out .
ENTRYPOINT ["dotnet", "/app/publish/CompareHare.Api.dll"]


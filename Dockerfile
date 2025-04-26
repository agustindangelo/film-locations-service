# Use the official .NET 9.0 SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy everything and restore dependencies
COPY . ./
RUN dotnet restore

# Build and publish the app to the /publish folder
RUN dotnet publish ".\FilmLocations.Api\FilmLocations.Api.csproj" -c Release -o publish

# Use a lighter runtime image for production
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copy the published output
COPY --from=build /app/publish .

# Expose port 8080 (common for Azure App Service Linux)
EXPOSE 8080

# Start the app
ENTRYPOINT ["dotnet", "FilmLocations.Api.dll"]
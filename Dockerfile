# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copy solution and project files
COPY Care.sln .
COPY Care/Care.csproj Care/
RUN dotnet restore

# Copy the rest of the project
COPY . .

# Publish the project
RUN dotnet publish Care/Care.csproj -c Release -o out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app

# Copy the published output
COPY --from=build /app/out ./

# Copy SQLite database
COPY Care/app.db ./Care/app.db

# Expose port 8080 for Render
EXPOSE 8080

# Start the app
ENTRYPOINT ["dotnet", "Care.dll"]
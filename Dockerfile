# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copy only the project we care about
COPY Care/Care.csproj Care/
RUN dotnet restore Care/Care.csproj

# Copy the rest of the project
COPY . .

# Publish only the Care project
RUN dotnet publish Care/Care.csproj -c Release -o out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app

# Copy published output
COPY --from=build /app/out ./

# Copy SQLite DB
COPY Care/app.db ./Care/app.db

# Expose Render port
EXPOSE 8080

# Start the app
ENTRYPOINT ["dotnet", "Care.dll"]
# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 6000

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file
COPY ../GrpcExample.sln .

# Copy all project files
COPY ../GrpcContracts/ GrpcContracts/
COPY ../GrpcServer/ GrpcServer/
COPY ../GrpcClient/ GrpcClient/

# Restore only the server
RUN dotnet restore GrpcServer/GrpcServer.csproj

# Build and publish
WORKDIR /src/GrpcServer
RUN dotnet publish "GrpcServer.csproj" -c Release -o /app/publish

# Final image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "GrpcServer.dll"]

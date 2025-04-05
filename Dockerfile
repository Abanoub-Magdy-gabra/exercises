# Base image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the csproj file and restore dependencies
COPY Workout.csproj ./

# Restore dependencies (in case we missed it)
RUN dotnet restore "Workout.csproj"

# Copy the rest of the files into the container
COPY . .

# Set working directory to the project folder
WORKDIR /src

# Build the project
RUN dotnet build "Workout.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the project
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Workout.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image for running the app
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Workout.dll"]

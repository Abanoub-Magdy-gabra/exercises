FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# انسخ ملف المشروع من فولدر WebApplication1
COPY WebApplication1/Workout.csproj WebApplication1/

# استرجاع الحزم
RUN dotnet restore "WebApplication1/Workout.csproj"

# انسخ باقي الملفات
COPY . .

# ادخل على فولدر المشروع
WORKDIR /src/WebApplication1

# بناء المشروع
RUN dotnet build "Workout.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Workout.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Workout.dll"]

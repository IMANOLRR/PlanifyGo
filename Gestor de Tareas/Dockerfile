FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar solo el csproj primero (mejor cache)
COPY ["Gestor de Tareas.csproj", "./"]
RUN dotnet restore

# Copiar todo lo demás
COPY . .

RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Gestor de Tareas.dll"]
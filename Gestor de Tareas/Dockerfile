FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar solo el csproj primero para mejorar la cache.
COPY ["Gestor de Tareas.csproj", "./"]
RUN dotnet restore

# Copiar el resto del proyecto.
COPY . .

RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["sh", "-c", "dotnet 'Gestor de Tareas.dll' --urls http://0.0.0.0:${PORT:-8080}"]

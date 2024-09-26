# Usar la imagen de .NET SDK 8.0 para construir
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar el archivo .csproj y restaurar las dependencias
COPY ["EventManagementAPI.csproj", "./"]
RUN dotnet restore "./EventManagementAPI.csproj"

# Copiar el resto del proyecto y construir
COPY . .
RUN dotnet build "EventManagementAPI.csproj" -c Release -o /app/build

# Publicar la aplicación
RUN dotnet publish "EventManagementAPI.csproj" -c Release -o /app/publish

# Usar la imagen de ASP.NET Core 8.0 para ejecutar
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "EventManagementAPI.dll"]

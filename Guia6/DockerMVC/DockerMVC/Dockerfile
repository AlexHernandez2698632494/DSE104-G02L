# Establecer la imagen base .NET 8 SDK para construir la aplicacion
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia los archivos del proyecto y restaurar las dependencias
COPY *.csproj .
RUN dotnet restore

# Copiar el resto de los archivos y compilar la aplicacion
COPY --from=build /app .
RUN dotnet publish -c Release -o /out

# Establecer la imagen base para el entorno de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /out .

# Exponer el puerto en el que se ejecutará la aplicacion
EXPOSE 80

# Comando de inicio
ENTRYPOINT ["dotnet", "DockerMVC.dll"]

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["MascotasService/MascotasService/MascotasService.csproj", "MascotasService/MascotasService/"]
COPY ["MascotasService/MascotasService.Domain/MascotasService.Domain.csproj", "MascotasService/MascotasService.Domain/"]
COPY ["MascotasService/MascotasService.Application/MascotasService.Application.csproj", "MascotasService/MascotasService.Application/"]
COPY ["MascotasService/MascotasService.Infrastructure/MascotasService.Infrastructure.csproj", "MascotasService/MascotasService.Infrastructure/"]
RUN dotnet restore "MascotasService/MascotasService/MascotasService.csproj"
COPY . .
WORKDIR "/src/MascotasService/MascotasService"
RUN dotnet publish "MascotasService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "MascotasService.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["Gastos API/Gastos API.csproj", "Gastos API/"]
RUN dotnet restore "Gastos API/Gastos API.csproj"

COPY . .
WORKDIR "/src/Gastos API"
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Gastos API.dll"]

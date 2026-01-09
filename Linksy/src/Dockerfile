FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Linksy.API/Linksy.API.csproj", "Linksy.API/"]
RUN dotnet restore "Linksy.API/Linksy.API.csproj"
COPY . .
WORKDIR "/src/Linksy.API"
RUN dotnet build "Linksy.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Linksy.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Linksy.API.dll"]
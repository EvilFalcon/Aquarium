FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Aquarium/Aquarium.csproj", "Aquarium/"]
RUN dotnet restore "Aquarium/Aquarium.csproj"
COPY . .
WORKDIR "/src/Aquarium"
RUN dotnet build "Aquarium.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Aquarium.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Aquarium.dll"]

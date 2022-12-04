FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /build

COPY ["src/DemoApp.Shared/DemoApp.Shared.csproj", "src/DemoApp.Shared/DemoApp.Shared.csproj"]
COPY ["src/DemoApp.Publisher/DemoApp.Publisher.csproj", "src/DemoApp.Publisher/DemoApp.Publisher.csproj"]

RUN dotnet restore "src/DemoApp.Publisher/DemoApp.Publisher.csproj"

COPY . .
WORKDIR "/build/src/DemoApp.Publisher"

RUN dotnet build -c Release --no-restore

RUN dotnet publish -c Release -o /published-app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine
WORKDIR /app

COPY --from=build /published-app ./

ENTRYPOINT ["dotnet", "DemoApp.Publisher.dll"]
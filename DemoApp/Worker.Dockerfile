FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /build

COPY ["src/DemoApp.Shared/DemoApp.Shared.csproj", "src/DemoApp.Shared/DemoApp.Shared.csproj"]
COPY ["src/DemoApp.Worker/DemoApp.Worker.csproj", "src/DemoApp.Worker/DemoApp.Worker.csproj"]

RUN dotnet restore "src/DemoApp.Worker/DemoApp.Worker.csproj"

COPY . .
WORKDIR "/build/src/DemoApp.Worker"

RUN dotnet build -c Release --no-restore

RUN dotnet publish -c Release -o /published-app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine
WORKDIR /app

COPY --from=build /published-app ./

ENTRYPOINT ["dotnet", "DemoApp.Worker.dll"]
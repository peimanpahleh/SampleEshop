#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Services/Orders/Orders.Saga/Orders.Saga.csproj", "src/Services/Orders/Orders.Saga/"]
RUN dotnet restore "src/Services/Orders/Orders.Saga/Orders.Saga.csproj"
COPY . .
WORKDIR "/src/src/Services/Orders/Orders.Saga"
RUN dotnet build "Orders.Saga.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Orders.Saga.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Orders.Saga.dll"]
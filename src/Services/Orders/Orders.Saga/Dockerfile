#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["Orders.Saga/Orders.Saga.csproj", "Orders.Saga/"]
RUN dotnet restore "Orders.Saga/Orders.Saga.csproj"

COPY . .
WORKDIR "/src/Orders.Saga"
RUN dotnet build "Orders.Saga.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Orders.Saga.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Orders.Saga.dll"]
#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["Baskets.Domain/Baskets.Domain.csproj", "Baskets.Domain/"]
RUN dotnet restore "Baskets.Domain/Baskets.Domain.csproj"

COPY ["Baskets.Application/Baskets.Application.csproj", "Baskets.Application/"]
RUN dotnet restore "Baskets.Application/Baskets.Application.csproj"

COPY ["Baskets.Infrastructure/Baskets.Infrastructure.csproj", "Baskets.Infrastructure/"]
RUN dotnet restore "Baskets.Infrastructure/Baskets.Infrastructure.csproj"

COPY ["Baskets.Api/Baskets.Api.csproj", "Baskets.Api/"]
RUN dotnet restore "Baskets.Api/Baskets.Api.csproj"

COPY . .
WORKDIR "/src/Baskets.Api"
RUN dotnet build "Baskets.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Baskets.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Baskets.Api.dll"]
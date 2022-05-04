#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Services/Baskets/Baskets.Api/Baskets.Api.csproj", "src/Services/Baskets/Baskets.Api/"]
COPY ["src/Services/Baskets/Baskets.Infrastructure/Baskets.Infrastructure.csproj", "src/Services/Baskets/Baskets.Infrastructure/"]
COPY ["src/Services/Baskets/Baskets.Application/Baskets.Application.csproj", "src/Services/Baskets/Baskets.Application/"]
COPY ["src/Services/Baskets/Baskets.Domain/Baskets.Domain.csproj", "src/Services/Baskets/Baskets.Domain/"]
RUN dotnet restore "src/Services/Baskets/Baskets.Api/Baskets.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/Baskets/Baskets.Api"
RUN dotnet build "Baskets.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Baskets.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Baskets.Api.dll"]
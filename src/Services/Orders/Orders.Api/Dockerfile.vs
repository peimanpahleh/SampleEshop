#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Services/Orders/Orders.Api/Orders.Api.csproj", "src/Services/Orders/Orders.Api/"]
COPY ["src/Services/Orders/Orders.Infrastructure/Orders.Infrastructure.csproj", "src/Services/Orders/Orders.Infrastructure/"]
COPY ["src/Services/Orders/Orders.Application/Orders.Application.csproj", "src/Services/Orders/Orders.Application/"]
COPY ["src/Services/Orders/Orders.Domain/Orders.Domain.csproj", "src/Services/Orders/Orders.Domain/"]
RUN dotnet restore "src/Services/Orders/Orders.Api/Orders.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/Orders/Orders.Api"
RUN dotnet build "Orders.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Orders.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Orders.Api.dll"]
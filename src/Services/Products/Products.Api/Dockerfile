#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["Products.Domain/Products.Domain.csproj", "Products.Domain/"]
RUN dotnet restore "Products.Domain/Products.Domain.csproj"

COPY ["Products.Application/Products.Application.csproj", "Products.Application/"]
RUN dotnet restore "Products.Application/Products.Application.csproj"

COPY ["Products.Infrastructure/Products.Infrastructure.csproj", "Products.Infrastructure/"]
RUN dotnet restore "Products.Infrastructure/Products.Infrastructure.csproj"

COPY ["Products.Api/Products.Api.csproj", "Products.Api/"]
RUN dotnet restore "Products.Api/Products.Api.csproj"

COPY . .
WORKDIR "/src/Products.Api"
RUN dotnet build "Products.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Products.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Products.Api.dll"]
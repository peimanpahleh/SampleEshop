#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Services/Products/Products.Api/Products.Api.csproj", "src/Services/Products/Products.Api/"]
COPY ["src/Services/Products/Products.Infrastructure/Products.Infrastructure.csproj", "src/Services/Products/Products.Infrastructure/"]
COPY ["src/Services/Products/Products.Application/Products.Application.csproj", "src/Services/Products/Products.Application/"]
COPY ["src/Services/Products/Products.Domain/Products.Domain.csproj", "src/Services/Products/Products.Domain/"]
RUN dotnet restore "src/Services/Products/Products.Api/Products.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/Products/Products.Api"
RUN dotnet build "Products.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Products.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Products.Api.dll"]
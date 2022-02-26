#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["Payments.Domain/Payments.Domain.csproj", "Payments.Domain/"]
RUN dotnet restore "Payments.Domain/Payments.Domain.csproj"

COPY ["Payments.Application/Payments.Application.csproj", "Payments.Application/"]
RUN dotnet restore "Payments.Application/Payments.Application.csproj"

COPY ["Payments.Infrastructure/Payments.Infrastructure.csproj", "Payments.Infrastructure/"]
RUN dotnet restore "Payments.Infrastructure/Payments.Infrastructure.csproj"

COPY ["Payments.Api/Payments.Api.csproj", "Payments.Api/"]
RUN dotnet restore "Payments.Api/Payments.Api.csproj"

COPY . .
WORKDIR "/src/Payments.Api"
RUN dotnet build "Payments.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Payments.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Payments.Api.dll"]
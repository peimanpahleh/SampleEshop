#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Services/Payments/Payments.Api/Payments.Api.csproj", "src/Services/Payments/Payments.Api/"]
COPY ["src/Services/Payments/Payments.Infrastructure/Payments.Infrastructure.csproj", "src/Services/Payments/Payments.Infrastructure/"]
COPY ["src/Services/Payments/Payments.Application/Payments.Application.csproj", "src/Services/Payments/Payments.Application/"]
COPY ["src/Services/Payments/Payments.Domain/Payments.Domain.csproj", "src/Services/Payments/Payments.Domain/"]
RUN dotnet restore "src/Services/Payments/Payments.Api/Payments.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/Payments/Payments.Api"
RUN dotnet build "Payments.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Payments.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Payments.Api.dll"]
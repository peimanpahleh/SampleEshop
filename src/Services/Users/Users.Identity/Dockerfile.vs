#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Services/Users/Users.Identity/Users.Identity.csproj", "src/Services/Users/Users.Identity/"]
RUN dotnet restore "src/Services/Users/Users.Identity/Users.Identity.csproj"
COPY . .
WORKDIR "/src/src/Services/Users/Users.Identity"
RUN dotnet build "Users.Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Users.Identity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Users.Identity.dll"]
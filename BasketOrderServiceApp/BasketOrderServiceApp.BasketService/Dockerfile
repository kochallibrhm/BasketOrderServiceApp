#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BasketOrderServiceApp.BasketService/BasketOrderServiceApp.BasketService.csproj", "BasketOrderServiceApp.BasketService/"]
COPY ["BasketOrderServiceApp.Common/BasketOrderServiceApp.Common.csproj", "BasketOrderServiceApp.Common/"]
RUN dotnet restore "BasketOrderServiceApp.BasketService/BasketOrderServiceApp.BasketService.csproj"
COPY . .
WORKDIR "/src/BasketOrderServiceApp.BasketService"
RUN dotnet build "BasketOrderServiceApp.BasketService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BasketOrderServiceApp.BasketService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BasketOrderServiceApp.BasketService.dll"]
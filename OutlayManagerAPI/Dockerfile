#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["OutlayManagerAPI/OutlayManagerAPI.csproj", "OutlayManagerAPI/"]
COPY ["OutlayManager.Model/OutlayManager.Model.csproj", "OutlayManager.Model/"]
COPY ["OutlayManager.ExternalService/OutlayManager.ExternalService.csproj", "OutlayManager.ExternalService/"]
COPY ["OutlayManager.Infraestructure/OutlayManager.Infraestructure.csproj", "OutlayManager.Infraestructure/"]
COPY ["OutalyManager.Cache/OutalyManager.Cache.csproj", "OutalyManager.Cache/"]
COPY ["OutlayManager.Authentication/OutlayManager.Authentication.csproj", "OutlayManager.Authentication/"]
RUN dotnet restore "OutlayManagerAPI/OutlayManagerAPI.csproj"
COPY . .
WORKDIR "/src/OutlayManagerAPI"
RUN dotnet build "OutlayManagerAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OutlayManagerAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OutlayManagerAPI.dll"]
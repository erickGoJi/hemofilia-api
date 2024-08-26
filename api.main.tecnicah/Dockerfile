#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["api.main.tecnicah/api.main.tecnicah.csproj", "api.main.tecnicah/"]
COPY ["biz.main.tecnicah/biz.main.tecnicah.csproj", "biz.main.tecnicah/"]
COPY ["dal.main.tecnicah/dal.main.tecnicah.csproj", "dal.main.tecnicah/"]
RUN dotnet restore "api.main.tecnicah/api.main.tecnicah.csproj"
COPY . .
WORKDIR "/src/api.main.tecnicah"
RUN dotnet build "api.main.tecnicah.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "api.main.tecnicah.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "api.main.tecnicah.dll"]
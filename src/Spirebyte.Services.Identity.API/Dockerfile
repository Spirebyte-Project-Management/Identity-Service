#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Identity-Service/src/Spirebyte.Services.Identity.API/Spirebyte.Services.Identity.API.csproj", "src/Identity-Service/src/Spirebyte.Services.Identity.API/"]
COPY ["src/Identity-Service/src/Spirebyte.Services.Identity.Infrastructure/Spirebyte.Services.Identity.Infrastructure.csproj", "src/Identity-Service/src/Spirebyte.Services.Identity.Infrastructure/"]
COPY ["src/Identity-Service/src/Spirebyte.Services.Identity.Application/Spirebyte.Services.Identity.Application.csproj", "src/Identity-Service/src/Spirebyte.Services.Identity.Application/"]
COPY ["src/Identity-Service/src/Spirebyte.Services.Identity.Core/Spirebyte.Services.Identity.Core.csproj", "src/Identity-Service/src/Spirebyte.Services.Identity.Core/"]
RUN dotnet restore "src/Identity-Service/src/Spirebyte.Services.Identity.API/Spirebyte.Services.Identity.API.csproj"
COPY . .
WORKDIR "/src/src/Identity-Service/src/Spirebyte.Services.Identity.API"
RUN dotnet build "Spirebyte.Services.Identity.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Spirebyte.Services.Identity.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Spirebyte.Services.Identity.API.dll"]

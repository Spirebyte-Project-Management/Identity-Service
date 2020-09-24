#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Services/Identity/src/Spirebyte.Services.Identity.API/Spirebyte.Services.Identity.API.csproj", "Services/Identity/src/Spirebyte.Services.Identity.API/"]
COPY ["Services/Identity/src/Spirebyte.Services.Identity.Infrastructure/Spirebyte.Services.Identity.Infrastructure.csproj", "Services/Identity/src/Spirebyte.Services.Identity.Infrastructure/"]
COPY ["Services/Identity/src/Spirebyte.Services.Identity.Application/Spirebyte.Services.Identity.Application.csproj", "Services/Identity/src/Spirebyte.Services.Identity.Application/"]
COPY ["Services/Identity/src/Spirebyte.Services.Identity.Core/Spirebyte.Services.Identity.Core.csproj", "Services/Identity/src/Spirebyte.Services.Identity.Core/"]
RUN dotnet restore "Services/Identity/src/Spirebyte.Services.Identity.API/Spirebyte.Services.Identity.API.csproj"
COPY . .
WORKDIR "/src/Services/Identity/src/Spirebyte.Services.Identity.API"
RUN dotnet build "Spirebyte.Services.Identity.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Spirebyte.Services.Identity.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Spirebyte.Services.Identity.API.dll"]
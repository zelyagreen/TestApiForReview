#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 7001

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Services/Identity/Identity.API/Identity.API.csproj", "Services/Identity/Identity.API/"]
COPY ["TestApiForReview.Infrastructure/TestApiForReview.Infrastructure.csproj", "TestApiForReview.Infrastructure/"]
RUN dotnet restore "Services/Identity/Identity.API/Identity.API.csproj"
COPY . .
WORKDIR "/src/Services/Identity/Identity.API"
RUN dotnet build "Identity.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Identity.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.API.dll"]
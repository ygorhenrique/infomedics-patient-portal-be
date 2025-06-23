# Use the official .NET SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and restore as distinct layers
COPY InfomedicsPortal.sln ./
COPY InfomedicsPortal/InfomedicsPortal.csproj InfomedicsPortal/
COPY InfomedicsPortal.Core/InfomedicsPortal.Core.csproj InfomedicsPortal.Core/
COPY InfomedicsPortal.Infrastructure/InfomedicsPortal.Infrastructure.csproj InfomedicsPortal.Infrastructure/
COPY InfomedicsPortal.UnitTests/InfomedicsPortal.UnitTests.csproj InfomedicsPortal.UnitTests/
RUN dotnet restore

# Copy everything else and build
COPY . .
WORKDIR /src/InfomedicsPortal
RUN dotnet publish -c Release -o /app/publish 

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Production
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 5000

ENTRYPOINT ["dotnet", "InfomedicsPortal.dll"] 
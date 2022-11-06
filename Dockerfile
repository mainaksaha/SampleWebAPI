
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /source

COPY . .
RUN dotnet restore "./AppInsightService/AppInsightService.csproj"
RUN dotnet publish "./AppInsightService/AppInsightService.csproj" -c release -o /app --no-restore
    
# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal
WORKDIR /app
COPY --from=build /app ./

ENTRYPOINT ["dotnet", "AppInsightService.dll"]

FROM mcr.microsoft.com/dotnet/sdk:3.1-focal AS build
WORKDIR /source

COPY . .
RUN dotnet restore "./SampleWebAPI/SampleWebAPI.csproj"
RUN dotnet publish "./SampleWebAPI/SampleWebAPI.csproj" -c release -o /app --no-restore
    
# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal
WORKDIR /app
COPY --from=build /app ./
EXPOSE 5000
ENTRYPOINT ["dotnet", "SampleWebAPI.dll"]
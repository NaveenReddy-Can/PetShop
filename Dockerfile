# Use the .NET Framework 4.7.2 SDK image as the build environment
FROM mcr.microsoft.com/dotnet/framework/sdk:4.7.2 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN nuget restore

# Copy everything else and build
COPY . ./
RUN msbuild /p:Configuration=Release

# Build runtime image
FROM mcr.microsoft.com/dotnet/framework/aspnet:4.7.2
WORKDIR /app
COPY --from=build-env /app/bin/Release/net472/publish .
CMD ["PetShop.exe"]

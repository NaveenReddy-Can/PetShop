# Use an official Microsoft ASP.NET runtime as a parent image
FROM mcr.microsoft.com/dotnet/framework/aspnet:4.7.2

# Set the working directory to /app
WORKDIR /app

# Copy the contents of the PetShop application to the container
COPY . .

# Restore NuGet packages and build the application
RUN nuget restore && \
    msbuild /p:Configuration=Release

# Set the entry point for the container
ENTRYPOINT ["PetShop.exe"]

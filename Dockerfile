# Base image with .NET Framework 4.7.2 installed
FROM mcr.microsoft.com/dotnet/framework/runtime:4.7.2-windowsservercore-ltsc2019

# Set the working directory to the app directory
WORKDIR C:\app

# Copy the app files to the working directory
COPY . .

# Expose the port used by the app
EXPOSE 80

# Start the app
CMD ["PetShop.Web.dll"]

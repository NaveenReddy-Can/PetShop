# Use the microsoft/dotnet-framework image as the base image
FROM microsoft/dotnet-framework:4.7.2-sdk

# Set the working directory to the app directory
WORKDIR /app

# Copy the contents of the local directory to the container's app directory
COPY . .

# Build the PetShop application
RUN msbuild PetShop.sln /p:Configuration=Release

# Set the working directory to the output directory
WORKDIR /app/PetShop.Web/bin/Release

# Expose port 80 for the PetShop app
EXPOSE 80

# Start the PetShop app
CMD ["PetShop.Web.exe"]

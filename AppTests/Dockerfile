FROM mcr.microsoft.com/dotnet/sdk:8.0
# Create a shared volume for working with this container
VOLUME [ "/app" ]
# Go to working directory
WORKDIR /app
RUN dotnet tool install -g dotnet-reportgenerator-globaltool
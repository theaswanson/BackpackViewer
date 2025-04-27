# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:9.0@sha256:9b0a4330cb3dac23ebd6df76ab4211ec5903907ad2c1ccde16a010bf25f8dfde AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY BackpackViewer.API/*.csproj ./BackpackViewer.API/
COPY BackpackViewer.Core/*.csproj ./BackpackViewer.Core/
COPY BackpackViewer.Tests/*.csproj ./BackpackViewer.Tests/
RUN dotnet restore

# copy everything else and build app
COPY BackpackViewer.API/. ./BackpackViewer.API/
COPY BackpackViewer.Core/. ./BackpackViewer.Core/
COPY BackpackViewer.Tests/. ./BackpackViewer.Tests/
WORKDIR /source/BackpackViewer.API
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:9.0@sha256:c3aee4ea4f51369d1f906b4dbd19b0f74fd34399e5ef59f91b70fcd332f36566
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "BackpackViewer.API.dll"]
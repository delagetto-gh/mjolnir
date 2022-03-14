FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build-stage
WORKDIR /app
COPY Asgard.csproj .
RUN dotnet restore
COPY * .
RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:3.1.9 AS run-stage
WORKDIR /app
COPY --from=build-stage /app/out .
ENTRYPOINT [ "dotnet" , "Asgard.dll"]
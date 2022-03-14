FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /app
COPY *.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app/dist

FROM mcr.microsoft.com/dotnet/aspnet:3.1
WORKDIR /app
COPY --from=build /app/dist .
EXPOSE 80
ENV ASPNETCORE_URLS=http://*:80
ENTRYPOINT [ "dotnet" , "Asgard.dll"]
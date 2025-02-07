FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ["../StackOverflowTags.sln", "./"]
COPY ["../TagsService/TagsService.csproj", "./TagsService/"]

RUN dotnet restore TagsService/TagsService.csproj

COPY ["../TagsService/", "./TagsService/"]

WORKDIR /app/TagsService
RUN dotnet test --no-restore
RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT ["dotnet", "TagsService.dll"]

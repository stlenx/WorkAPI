FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["WorkAPI/WorkAPI.csproj", "WorkAPI/"]
RUN dotnet restore "WorkAPI/WorkAPI.csproj"
COPY . .
WORKDIR "/src/WorkAPI"
RUN dotnet build "WorkAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WorkAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WorkAPI.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MultitenantDbPoc/MultitenantDbPoc.csproj", "MultitenantDbPoc/"]
RUN dotnet restore "MultitenantDbPoc/MultitenantDbPoc.csproj"
COPY . .
WORKDIR "/src/MultitenantDbPoc"
RUN dotnet build "MultitenantDbPoc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MultitenantDbPoc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MultitenantDbPoc.dll"]

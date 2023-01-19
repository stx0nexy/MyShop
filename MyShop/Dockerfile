FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MyShop/MyShop/MyShop.csproj", "MyShop/"]
RUN dotnet restore "MyShop/MyShop.csproj"
COPY . .
WORKDIR "/src/MyShop"
RUN dotnet build "MyShop.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyShop.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyShop.dll"]

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/OrderManagementSystem/OrderManagementSystem.Web.Api/OrderManagementSystem.Web.Api.csproj", "src/OrderManagementSystem/OrderManagementSystem.Web.Api/"]
COPY ["src/OrderManagementSystem/OrderManagementSystem.Core/OrderManagementSystem.Core.csproj", "src/OrderManagementSystem/OrderManagementSystem.Core/"]
COPY ["src/OrderManagementSystem/OrderManagementSystem.Infrastructure/OrderManagementSystem.Infrastructure.csproj", "src/OrderManagementSystem/OrderManagementSystem.Infrastructure/"]
RUN dotnet restore "src/OrderManagementSystem/OrderManagementSystem.Web.Api/OrderManagementSystem.Web.Api.csproj"

COPY . .
RUN dotnet publish "src/OrderManagementSystem/OrderManagementSystem.Web.Api/OrderManagementSystem.Web.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "OrderManagementSystem.Web.Api.dll"]

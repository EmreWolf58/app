FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/OrderManagementSystem/OrderManagementSystem.Web.Mvc/OrderManagementSystem.Web.Mvc.csproj", "src/OrderManagementSystem/OrderManagementSystem.Web.Mvc/"]
COPY ["src/OrderManagementSystem/OrderManagementSystem.Core/OrderManagementSystem.Core.csproj", "src/OrderManagementSystem/OrderManagementSystem.Core/"]
RUN dotnet restore "src/OrderManagementSystem/OrderManagementSystem.Web.Mvc/OrderManagementSystem.Web.Mvc.csproj"

COPY . .
RUN dotnet publish "src/OrderManagementSystem/OrderManagementSystem.Web.Mvc/OrderManagementSystem.Web.Mvc.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "OrderManagementSystem.Web.Mvc.dll"]

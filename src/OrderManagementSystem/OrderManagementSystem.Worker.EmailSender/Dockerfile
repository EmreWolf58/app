FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# csproj'leri kopyala (cache için)
COPY ["src/OrderManagementSystem/OrderManagementSystem.Worker.EmailSender/OrderManagementSystem.Worker.EmailSender.csproj", "src/OrderManagementSystem/OrderManagementSystem.Worker.EmailSender/"]
COPY ["src/OrderManagementSystem/OrderManagementSystem.Core/OrderManagementSystem.Core.csproj", "src/OrderManagementSystem/OrderManagementSystem.Core/"]
COPY ["src/OrderManagementSystem/OrderManagementSystem.Infrastructure/OrderManagementSystem.Infrastructure.csproj", "src/OrderManagementSystem/OrderManagementSystem.Infrastructure/"]

RUN dotnet restore "src/OrderManagementSystem/OrderManagementSystem.Worker.EmailSender/OrderManagementSystem.Worker.EmailSender.csproj"

# tüm kodu kopyala
COPY . .

RUN dotnet publish "src/OrderManagementSystem/OrderManagementSystem.Worker.EmailSender/OrderManagementSystem.Worker.EmailSender.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "OrderManagementSystem.Worker.EmailSender.dll"]
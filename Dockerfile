FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["ApprovalSystem.Api/ApprovalSystem.Api.csproj", "ApprovalSystem.Api/"]
COPY ["ApprovalSystem.Application/ApprovalSystem.Application.csproj", "ApprovalSystem.Application/"]
COPY ["ApprovalSystem.Core/ApprovalSystem.Core.csproj", "ApprovalSystem.Core/"]
COPY ["ApprovalSystem.Infrastructure/ApprovalSystem.Infrastructure.csproj", "ApprovalSystem.Infrastructure/"]

RUN dotnet restore "ApprovalSystem.Api/ApprovalSystem.Api.csproj"

COPY . .
WORKDIR "/src/ApprovalSystem.Api"
RUN dotnet publish "ApprovalSystem.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "ApprovalSystem.Api.dll"]

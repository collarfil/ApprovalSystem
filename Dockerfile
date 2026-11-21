# Stage 1: Build the .NET 10 Clean Architecture solution
FROM ://microsoft.com AS build
WORKDIR /src

# Copy all project files first for faster Docker layer caching
COPY ["ApprovalSystem.Api/ApprovalSystem.Api.csproj", "ApprovalSystem.Api/"]
COPY ["ApprovalSystem.Application/ApprovalSystem.Application.csproj", "ApprovalSystem.Application/"]
COPY ["ApprovalSystem.Core/ApprovalSystem.Core.csproj", "ApprovalSystem.Core/"]
COPY ["ApprovalSystem.Infrastructure/ApprovalSystem.Infrastructure.csproj", "ApprovalSystem.Infrastructure/"]

RUN dotnet restore "ApprovalSystem.Api/ApprovalSystem.Api.csproj"

# Copy all source code and publish the API project
COPY . .
WORKDIR "/src/ApprovalSystem.Api"
RUN dotnet publish "ApprovalSystem.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Lightweight .NET 10 runtime image
FROM ://microsoft.com AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Render listens on port 8080 by default
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "ApprovalSystem.Api.dll"]

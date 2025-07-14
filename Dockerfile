FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
   WORKDIR /app
   EXPOSE 80

   FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
   WORKDIR /src
   COPY ["RadioScheduler.Api/RadioScheduler.Api.csproj", "RadioScheduler.Api/"]
   COPY ["RadioScheduler.Domain/RadioScheduler.Domain.csproj", "RadioScheduler.Domain/"]
   COPY ["RadioScheduler.Application/RadioScheduler.Application.csproj", "RadioScheduler.Application/"]
   COPY ["RadioScheduler.Infrastructure/RadioScheduler.Infrastructure.csproj", "RadioScheduler.Infrastructure/"]
   RUN dotnet restore "RadioScheduler.Api/RadioScheduler.Api.csproj"
   COPY . .
   WORKDIR "/src/RadioScheduler.Api"
   RUN dotnet build "RadioScheduler.Api.csproj" -c Release -o /app/build

   FROM build AS publish
   RUN dotnet publish "RadioScheduler.Api.csproj" -c Release -o /app/publish

   FROM base AS final
   WORKDIR /app
   COPY --from=publish /app/publish .
   ENTRYPOINT ["dotnet", "RadioScheduler.Api.dll"]
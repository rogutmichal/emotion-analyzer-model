FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

EXPOSE 8080


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src


COPY . .


RUN dotnet restore "EmotionAnalyzerAPI/EmotionAnalyzerAPI.csproj"


RUN dotnet publish "EmotionAnalyzerAPI/EmotionAnalyzerAPI.csproj" \
    -c Release \
    -o /app/publish



FROM base AS final
WORKDIR /app


ENV ASPNETCORE_URLS=http://0.0.0.0:8080


COPY --from=build /app/publish .


ENTRYPOINT ["dotnet", "EmotionAnalyzerAPI.dll"]
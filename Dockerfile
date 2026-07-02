FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY TodoListApi/TodoListApi.csproj TodoListApi/
RUN dotnet restore TodoListApi/TodoListApi.csproj
COPY TodoListApi/ TodoListApi/
RUN dotnet publish TodoListApi/TodoListApi.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT:-8080}
ENTRYPOINT ["dotnet", "TodoListApi.dll"]
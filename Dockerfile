FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

EXPOSE 5000:5000

# Copy
COPY . .

# Restore
RUN dotnet restore StreamingMovie.sln

# Build & publish
RUN dotnet publish StreamingMovie.Web/StreamingMovie.Web.csproj -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /out .

# Time
RUN apt-get update && apt-get install -y tzdata
ENV TZ=Asia/Ho_Chi_Minh
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

#Entry
ENTRYPOINT ["dotnet", "StreamingMovie.Web.dll"]

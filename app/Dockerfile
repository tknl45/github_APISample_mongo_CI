#此指令會初始化新的建置階段，並設定其餘指令的基底映像。
FROM microsoft/aspnetcore:2.0 AS base
#WORKDIR 指令會設定任何其餘 RUN、CMD、ENTRYPOINT、COPY 和 ADD Dockerfile 指令的工作目錄。 如果目錄不存在，則會建立它。 在此情況下，WORKDIR 會設定為應用程式目錄。
WORKDIR /app
#設定內部port
EXPOSE 80


#此指令會初始化新的建置階段，並設定其餘指令的基底映像。
FROM microsoft/aspnetcore-build:2.0 AS build
# 建立src資料夾
WORKDIR /src
# COPY 指令會從來源路徑中複製新檔案或目錄，並將它們新增至目的地容器檔案系統。 使用此指令，我們會將 C# 專案檔複製至容器。
COPY *.csproj ./
# 把相依程式下載安裝
RUN dotnet restore
# 複製全部的檔案
COPY . .
# 進入src資料夾
WORKDIR /src/
# 執行build
RUN dotnet build -c Release -o /app


FROM build AS publish
#執行publish 到 app資料夾 
RUN dotnet publish -c Release -o /app




# 最後一步
FROM base AS final
# 進入app 資料夾
WORKDIR /app
COPY --from=publish /app .
# 執行服務
ENTRYPOINT ["dotnet", "APISample.dll"]

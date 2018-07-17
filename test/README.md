# .NetCoreAndXunit
.NetCore + Xunit practice

# execute
> dotnet test

測試列表
> dotnet test -t

測試類別為API的test case
> dotnet test --filter Category=API


測試類別為API的test case 並留下紀錄
> dotnet test --filter Category=API --logger:trx
> dotnet test --filter Category=RestAPI --logger:trx

指令執行後會產生TestResults資料夾
資料夾中的有xxxx.trx檔，可藉由visual studio 2017打開
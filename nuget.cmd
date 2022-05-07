@echo off
::项目名称
set ProjectName=Json2DartClass
::Nuget发布的账号密码,中间用英文冒号隔开(account:password)
set ApiKey=bi7prk
::发布模式 Release/Debug
set PublishMode=Release
::Nuget发布地址
set SourceUrl=http://nuget.disfake.com/nuget

::这里开始禁止修改
del %ProjectName%*.nupkg /F /Q
nuget pack %ProjectName%/%ProjectName%.csproj -Build -Prop Configuration=%PublishMode%
nuget push "%ProjectName%*.nupkg" -Source %SourceUrl% -ApiKey %ApiKey%
del %ProjectName%*.nupkg /F /Q 
pause
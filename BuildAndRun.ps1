Get-ChildItem $SolutionDirectory -include bin,obj -Recurse | ForEach-Object ($_) { Remove-Item $_.FullName -Force -Recurse }

dotnet restore
dotnet build -c Release
.\HeadlessBrowserAudioVideoCapturingService\bin\Release\net6.0\win-x64\HeadlessBrowserAudioVideoCapturingService.exe
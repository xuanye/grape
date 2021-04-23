set -ex

cd $(dirname $0)/../

artifactsFolder="./artifacts"

if [ -d $artifactsFolder ]; then
  rm -R $artifactsFolder
fi

mkdir -p $artifactsFolder

dotnet build ./src/Grape.Captcha/Grape.Captcha.csproj -c Release

dotnet pack ./src/Grape.Captcha/Grape.Captcha.csproj -c Release -o $artifactsFolder

dotnet nuget push ./$artifactsFolder/Grape.Captcha.*.nupkg -k $NUGET_KEY -s https://www.nuget.org
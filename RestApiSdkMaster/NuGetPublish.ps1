Function CreateSubFolders($dir){
    $lib = $dir + '\lib\net45'
    New-Item -ItemType Directory -Force -Path  $lib
	Copy-Item RestApiSdkClassic\bin\Release\RestApiSdk.dll $lib\RestApiSdk.dll

    $lib = $dir + '\lib\portable-net40+sl5+wp80+win8+wpa81'
    New-Item -ItemType Directory -Force -Path  $lib
	Copy-Item RestApiSdkUniversal\bin\Release\RestApiSdk.dll $lib\RestApiSdk.dll

    $lib = $dir + '\lib\portable-net45+dnxcore50'
    New-Item -ItemType Directory -Force -Path  $lib
	Copy-Item RestApiSdkWin8\bin\Release\RestApiSdk.dll $lib\RestApiSdk.dll

    $lib = $dir + '\tools'
    New-Item -ItemType Directory -Force -Path  $lib
	Copy-Item install.ps1 $lib\install.ps1
}

Function RemoveSubFolders($dir){
    $lib = $dir + '\lib\net45'
    Remove-Item -Recurse -Force $lib
    $lib = $dir + '\lib\portable-net40+sl5+wp80+win8+wpa81'
    Remove-Item -Recurse -Force $lib
    $lib = $dir + '\lib\portable-net45+dnxcore50'
    Remove-Item -Recurse -Force $lib
}

try{
    $scriptpath = $MyInvocation.MyCommand.Path
    $dir = Split-Path $scriptpath
    Push-Location $dir

	#This file could change name with a NuGet update for the NuGet command line package!
    Copy-Item packages\NuGet.CommandLine.3.3.0\tools\NuGet.exe NuGet.exe

    CreateSubFolders($dir)

	& .\NuGet.exe pack RestApiSdk.nuspec

    RemoveSubFolders($dir)
}
finally{
    Remove-Item NuGet.exe
    Remove-Item RestApiSdk.1.0.0.nupkg
    RemoveSubFolders($dir)
}

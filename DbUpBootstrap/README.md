DbUp https://dbup.readthedocs.io/en/latest/

Need to install .net5 runtime at target machine deploy

powershell cmd >Invoke-WebRequest "https://dot.net/v1/dotnet-install.ps1" -OutFile "dotnet-install.ps1"

powershell cmd >./dotnet-install.ps1 -Runtime dotnet -Version 5.0.6 -InstallDir "C:\Program Files\dotnet"
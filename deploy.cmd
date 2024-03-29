@if "%SCM_TRACE_LEVEL%" NEQ "4" @echo off

:: ----------------------
:: KUDU Deployment Script
:: Version: 0.2.2
:: This script is highly customised for the specific deployment task
:: ----------------------

:: Prerequisites
:: -------------

:: Verify node.js installed
where node 2>nul >nul
IF %ERRORLEVEL% NEQ 0 (
  echo Missing node.js executable, please install node.js, if already installed make sure it can be reached from current environment.
  goto error
)

:: Setup
:: -----

setlocal enabledelayedexpansion

SET ARTIFACTS=%~dp0%..\artifacts

IF NOT DEFINED DEPLOYMENT_SOURCE (
  SET DEPLOYMENT_SOURCE=%~dp0%.
)

IF NOT DEFINED DEPLOYMENT_TARGET (
  SET DEPLOYMENT_TARGET=%ARTIFACTS%\wwwroot
)

IF NOT DEFINED NEXT_MANIFEST_PATH (
  SET NEXT_MANIFEST_PATH=%ARTIFACTS%\manifest

  IF NOT DEFINED PREVIOUS_MANIFEST_PATH (
    SET PREVIOUS_MANIFEST_PATH=%ARTIFACTS%\manifest
  )
)

IF NOT DEFINED KUDU_SYNC_CMD (
  :: Install kudu sync
  echo [%TIME%] Installing Kudu Sync
  call npm install kudusync -g --silent
  IF !ERRORLEVEL! NEQ 0 goto error

  :: Locally just running "kuduSync" would also work
  SET KUDU_SYNC_CMD=%appdata%\npm\kuduSync.cmd
)

IF NOT DEFINED DEPLOYMENT_TEMP (
  SET DEPLOYMENT_TEMP=%temp%\___deployTemp%random%
  SET CLEAN_LOCAL_DEPLOYMENT_TEMP=true
)

IF DEFINED CLEAN_LOCAL_DEPLOYMENT_TEMP (
  IF EXIST "%DEPLOYMENT_TEMP%" rd /s /q "%DEPLOYMENT_TEMP%"
  mkdir "%DEPLOYMENT_TEMP%"
)

IF NOT DEFINED MSBUILD_PATH (
  SET MSBUILD_PATH=%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe
)

:: Custom Options
IF NOT DEFINED NPM_INSTALL_ARGS (
  SET NPM_INSTALL_ARGS=--production
)

IF NOT DEFINED GULP_BUILD_ARGS (
  SET GULP_BUILD_ARGS=--production
)
 
IF NOT DEFINED SOLUTION_SOURCE (
  SET SOLUTION_SOURCE=%DEPLOYMENT_SOURCE%\Production\src\
)

IF NOT DEFINED SOLUTION_NAME (
  SET SOLUTION_NAME=%SOLUTION_SOURCE%\NssCorporateUmbraco.sln
)


goto Deployment

:: Utility Functions
:: -----------------

:SelectNodeVersion

IF DEFINED KUDU_SELECT_NODE_VERSION_CMD (
  :: The following are done only on Windows Azure Websites environment
  call %KUDU_SELECT_NODE_VERSION_CMD% "%DEPLOYMENT_SOURCE%" "%DEPLOYMENT_TARGET%" "%DEPLOYMENT_TEMP%"
  IF !ERRORLEVEL! NEQ 0 goto error

  IF EXIST "%DEPLOYMENT_TEMP%\__nodeVersion.tmp" (
    SET /p NODE_EXE=<"%DEPLOYMENT_TEMP%\__nodeVersion.tmp"
    IF !ERRORLEVEL! NEQ 0 goto error
  )
  
  IF EXIST "%DEPLOYMENT_TEMP%\__npmVersion.tmp" (
    SET /p NPM_JS_PATH=<"%DEPLOYMENT_TEMP%\__npmVersion.tmp"
    IF !ERRORLEVEL! NEQ 0 goto error
  )

  IF NOT DEFINED NODE_EXE (
    SET NODE_EXE=node
  )

  SET NPM_CMD="!NODE_EXE!" "!NPM_JS_PATH!"
) ELSE (
  SET NPM_CMD=npm
  SET NODE_EXE=node
)

goto :EOF

::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: Deployment
:: ----------

:Deployment
echo [%TIME%] Handling NHS NSS Corporate Umbraco deployment

:: 0. Select node version
call :SelectNodeVersion

:: 1. Install npm packages
IF EXIST "%DEPLOYMENT_SOURCE%\package.json" (
  echo [%TIME%] Installing NPM packages %NPM_INSTALL_ARGS%
  pushd "%DEPLOYMENT_SOURCE%"
  call :ExecuteCmd !NPM_CMD! install %NPM_INSTALL_ARGS%
  IF !ERRORLEVEL! NEQ 0 goto error
  popd
)

:: 2. Run gulp
IF EXIST "%DEPLOYMENT_SOURCE%\gulpfile.js" (
  echo [%TIME%] Running Gulp %GULP_BUILD_ARGS%
  pushd "%DEPLOYMENT_SOURCE%"
  call :Executecmd gulp %GULP_BUILD_ARGS%
  IF !ERRORLEVEL! NEQ 0 goto error
  popd
)

:: 3. Restore NuGet packages
IF /I %SOLUTION_NAME% NEQ "" (
  echo [%TIME%] Restoring Nuget Packages
  call :ExecuteCmd nuget restore "%SOLUTION_NAME%"
  IF !ERRORLEVEL! NEQ 0 goto error
)

:: 4. Build to the temporary path
IF /I "%IN_PLACE_DEPLOYMENT%" NEQ "1" (
  echo [%TIME%] Building in place
  call :ExecuteCmd "%MSBUILD_PATH%" "%SOLUTION_SOURCE%\NssCorporateUmbraco\NssCorporateUmbraco.csproj" /nologo /verbosity:m /t:Build /t:pipelinePreDeployCopyAllFilesToOneFolder /p:_PackageTempDir="%DEPLOYMENT_TEMP%";AutoParameterizationWebConfigConnectionStrings=false;Configuration=Release /p:SolutionDir="%SOLUTION_SOURCE%" %SCM_BUILD_ARGS%
) ELSE (
  echo [%TIME%] Building
  call :ExecuteCmd "%MSBUILD_PATH%" "%SOLUTION_SOURCE%\NssCorporateUmbraco\NssCorporateUmbraco.csproj" /nologo /verbosity:m /t:Build /p:AutoParameterizationWebConfigConnectionStrings=false;Configuration=Release /p:SolutionDir="%SOLUTION_SOURCE%" %SCM_BUILD_ARGS%
)

:: 5. KuduSync
IF /I "%IN_PLACE_DEPLOYMENT%" NEQ "1" (
  echo [%TIME%] Running KuduSync
  call :ExecuteCmd "%KUDU_SYNC_CMD%" --perf -q -f "%SOLUTION_SOURCE%\NssCorporateUmbraco\_\dist" -t "%DEPLOYMENT_TEMP%\_\dist" -n "%NEXT_MANIFEST_PATH%" -p "%PREVIOUS_MANIFEST_PATH%"
  IF !ERRORLEVEL! NEQ 0 goto error
)

:: 5. KuduSync
IF /I "%IN_PLACE_DEPLOYMENT%" NEQ "1" (
  echo [%TIME%] Running KuduSync
  call :ExecuteCmd "%KUDU_SYNC_CMD%" --perf -q -f "%DEPLOYMENT_TEMP%" -t "%DEPLOYMENT_TARGET%" -n "%NEXT_MANIFEST_PATH%" -p "%PREVIOUS_MANIFEST_PATH%" -i ".git;.hg;.deployment;deploy.cmd"
  IF !ERRORLEVEL! NEQ 0 goto error
)

::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

:: Post deployment stub
IF DEFINED POST_DEPLOYMENT_ACTION call "%POST_DEPLOYMENT_ACTION%"
IF !ERRORLEVEL! NEQ 0 goto error

goto end

:: Execute command routine that will echo out when error
:ExecuteCmd
setlocal
set _CMD_=%*
call %_CMD_%
if "%ERRORLEVEL%" NEQ "0" echo Failed exitCode=%ERRORLEVEL%, command=%_CMD_%
exit /b %ERRORLEVEL%

:error
endlocal
echo [%TIME%] An error has occurred during web site deployment.
call :exitSetErrorLevel
call :exitFromFunction 2>nul

:exitSetErrorLevel
exit /b 1

:exitFromFunction
()

:end
endlocal
echo [%TIME%] Finished successfully

Properties {
  $root_directory = Split-Path $psake.build_script_file

  $build_dir_name = "build"
  $solution_file_name = "WebApiMovieRestSolution.sln"
  $configuration = "release"
  
  $build_dir = "$root_directory\$build_dir_name"
  $packages_dir = "$root_directory\packages"
  $solution_file = "$root_directory\$solution_file_name"
  $tools_dir = "$root_directory\tools"
  $xunit_runner = @(gci $packages_dir -filter xunit.console.clr4.exe -recurse)[0].FullName

  $test_projects = @("WebApiMovieRest.Core.Tests.dll", "WebApiMovieRest.Infrastructure.Tests.dll", "WebApiMovieRest.IntegrationTests.dll")
}

FormatTaskName {
   param($taskName)
   write-host "`n $(('-'*25)) [$taskName] $(('-'*25))" -ForegroundColor Yellow
}

task Default -depends RunTests

task RunTests -depends Compile { 
  $test_projects | ForEach-Object { 
    Write-Host "Testing $_" -ForegroundColor Green
    $test_results_filename = "$build_dir\" + [IO.Path]::GetFileNameWithoutExtension($_) + ".Results.html"
    Exec { &"$xunit_runner" "$build_dir\$configuration\$_" /html "$test_results_filename"}
  	Write-Host ""
  }
}

task Compile -depends CreateOutputDirectories { 
  Write-Host "building $solution_file_name in $configuration mode" -ForegroundColor Green
  Write-Host "msbuild $solution_file /t:Build /p:Configuration=$configuration /v:quiet /p:OutDir=$build_dir\$configuration\" -ForegroundColor yellow
  Exec { msbuild $solution_file /t:Build /p:Configuration=$configuration /v:quiet /p:OutDir=$build_dir\$configuration\ } 
}

task CreateOutputDirectories -depends Clean {
  Write-Host "creating $build_dir directory" -ForegroundColor Green
  New-Item $build_dir -itemType directory -ErrorAction SilentlyContinue | Out-Null
}

task Clean -depends Variables { 
  Write-Host "removing $build_dir directory" -ForegroundColor Green
  Remove-Item -force -recurse $build_dir -ErrorAction SilentlyContinue

  Write-Host "msbuild $solution_file /t:Clean /p:Configuration=$configuration /v:quiet" -ForegroundColor yellow
  Exec { msbuild $solution_file /t:Clean /p:Configuration=$configuration /v:quiet }
} 

task Variables { 
  Write-Host "root_directory: $root_directory"
  Write-Host "configuration: $configuration"
  Write-Host "build_dir: $build_dir"
  Write-Host "packages_dir: $packages_dir"
  Write-Host "solution_file: $solution_file"
  Write-Host "tools_dir: $tools_dir"
  Write-Host "xunit_runner: $xunit_runner"
} 
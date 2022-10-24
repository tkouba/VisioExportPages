#requires -Version 2.0
<#PSScriptInfo
.VERSION 1.0
.GUID 81ab6892-33db-43e9-b942-11f3fdcb3d30
.AUTHOR Tomas Kouba
.DESCRIPTION This script can be used to batch covert Visio file to png file automatically. Based on Microsoft sample script.
.COMPANYNAME 
.COPYRIGHT (c) 2022 Tomas Kouba. All rights reserved.
.TAGS
.LICENSEURI https://github.com/tkouba/VisioExportPages/blob/master/LICENSE.md
.PROJECTURI https://github.com/tkouba/VisioExportPages
.ICONURI
.EXTERNALMODULEDEPENDENCIES
.REQUIREDSCRIPTS
.EXTERNALSCRIPTDEPENDENCIE
.RELEASENOTES
#>

<#
 	.SYNOPSIS
        This script can be used to batch covert Visio file to png file automatically.
    .DESCRIPTION
        This script can be used to batch covert Visio file to png file automatically. Based on Microsoft sample script.
    .PARAMETER  SourcePath
		Specifies the path of visio file.
    .PARAMETER IncludeBackground
        Include background pages. Standard do no export background pages.
    .EXAMPLE
        PS C:\Users\Administrator> C:\Script\Convert-VisioToPNG.ps1 -SourcePath C:\VisioFiles

        Convert to PNG                          File Name                               SourceFileName
        --------------                          ---------                               --------------
        Finished                                Page-1                                  MyVisio - 1.vsdx
        Finished                                Page-2                                  MyVisio - 1.vsdx
        Finished                                Page-3                                  MyVisio - 1.vsdx
        Finished                                Page-4                                  MyVisio - 1.vsdx
        Finished                                Page-5                                  MyVisio - 1.vsdx

    .EXAMPLE
        PS C:\Users\Administrator> C:\Script\Convert-VisioToPNG.ps1 -SourcePath 'C:\VisioFiles\MyVisio - 1.vsdx'

        Convert to PNG                          File Name                               SourceFileName
        --------------                          ---------                               --------------
        Finished                                Page-1                                  MyVisio - 1.vsdx
        Finished                                Page-2                                  MyVisio - 1.vsdx
        Finished                                Page-3                                  MyVisio - 1.vsdx
        Finished                                Page-4                                  MyVisio - 1.vsdx
        Finished                                Page-5                                  MyVisio - 1.vsdx
    .NOTES
        Version : 1.0, 2022-10-24
        Author : Tomas Kouba, based on Microsoft sample script
        File Name : Convert-VisioToPNG.ps1
        Requires : PowerShell 

    .LINK

#>
Param
(
    [Parameter(Mandatory = $true, ValueFromPipeline = $true, Position = 0)]
    [Alias('p')][String]$SourcePath,
    [Parameter(Mandatory = $false, ValueFromPipeline = $false)]
    [switch]$IncludeBackground
)

If (Test-Path -Path $SourcePath) {
    #get all related to Visio files
    $VisioFiles = Get-ChildItem -Path $SourcePath -Recurse -Include *.vsdx, *.vssx, *.vstx, *.vxdm, *.vssm, *.vstm, *.vsd, *.vdw, *.vss, *.vst

    If ($VisioFiles) {
        #Create the Visio application object
        $VisioApp = New-Object -ComObject Visio.Application
        $VisioApp.Visible = $false

        
        Foreach ($File in $VisioFiles) {
            $Objs = @()
            $FilePath = $File.FullName
            #get the directory of file
            $FileDirectory = $File.DirectoryName 
            $FileBaseName = $File.BaseName

            Write-Verbose "Opening $FilePath file."
            $Document = $VisioApp.Documents.Open($FilePath)
            $Pages = $VisioApp.ActiveDocument.Pages
            $PagesCount = $Pages.count
            $PageName
            $intPageNumber = 1
            Try {
                Foreach ($Page in $Pages) {
                    
                    Write-Progress -Activity "Converting Visio page file [$FileBaseName] to PNG ile" `
                        -Status "$intPageNumber of $PagesCount visio files - Finished" -PercentComplete $($intPageNumber / $PagesCount * 100)

                    Write-Verbose "Converting Visio page file '$FilePath' to '$FileDirectory' directory."
                    $intPageNumber++

                    If ($IncludeBackground -or ($Page.Background -eq $false)) {
                        # create PNG file name
                        $pngFileName = "$FileDirectory\$($FileBaseName) - $($Page.Name).png"
                        $Page.Export($pngFileName)                         

                        $Properties = @{'SourceFileName' = $File.Name
                            'Page Name'                  = $Page.Name
                            'Convert to PNG'             = If ($pngFileName) { "Finished" } Else { "Unfinished" }
                        }
                    }
                    else {
                        $Properties = @{'SourceFileName' = $File.Name
                            'Page Name'                  = $Page.Name
                            'Convert to PNG'             = "Background"
                        }
                    }
		            		
                    $objVisio = New-Object -TypeName PSObject -Property $Properties
                    $Objs += $objVisio
                }
                $Objs
                $Document.Close()
            }
            Catch {
                Write-Warning "A few visio page files have been lost in this converting. NO.$intPageNumber visio page file cannot convert to png file."
            }
        }
        
        $VisioApp.Quit()
    }
    Else {
        Write-Warning "Please make sure that at least one Visio file in the ""$Path""."
    }
}
Else {
    Write-Warning "The path does not exist, plese input the correct path."
}
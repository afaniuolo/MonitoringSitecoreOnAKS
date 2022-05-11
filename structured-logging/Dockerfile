# escape=`

ARG BASE_IMAGE
ARG BUILD_IMAGE

FROM ${BUILD_IMAGE} AS prep
SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Gather only artifacts necessary for NuGet restore, retaining directory structure
COPY *.sln nuget.config \nuget\
COPY src\ \temp\
RUN Invoke-Expression 'robocopy C:\temp C:\nuget\src /s /ndl /njh /njs *.csproj *.scproj packages.config'

FROM ${BUILD_IMAGE} AS builder

ARG BUILD_CONFIGURATION

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Create an empty working directory
WORKDIR C:\build

# Copy prepped NuGet artifacts, and restore as distinct layer to take better advantage of caching
COPY --from=prep .\nuget .\
RUN nuget restore

# Copy remaining source code
COPY src\ .\src\

# Build website with file publish
RUN msbuild .\src\Foundation\StructuredLogging\code\MonitoringSitecoreOnAKS.Foundation.StructuredLogging.csproj /p:Configuration=$env:BUILD_CONFIGURATION /p:DeployOnBuild=True /p:DeployDefaultTarget=WebPublish /p:WebPublishMethod=FileSystem /p:PublishUrl=C:\out\log4net

RUN Invoke-Expression 'robocopy C:\out\log4net C:\out\cm /s'; `
    (Get-Content C:\out\cm\App_Config\Include\Foundation\MonitoringSitecoreOnAKS.Foundation.StructuredLogging.config) -Replace '###ROLE###', 'CM' | Set-Content C:\out\cm\App_Config\Include\Foundation\MonitoringSitecoreOnAKS.Foundation.StructuredLogging.config

RUN Invoke-Expression 'robocopy C:\out\log4net C:\out\cd /s'; `
    (Get-Content C:\out\cd\App_Config\Include\Foundation\MonitoringSitecoreOnAKS.Foundation.StructuredLogging.config) -Replace '###ROLE###', 'CD' | Set-Content C:\out\cd\App_Config\Include\Foundation\MonitoringSitecoreOnAKS.Foundation.StructuredLogging.config

RUN Invoke-Expression 'robocopy C:\out\log4net C:\out\prc /s'; `
    (Get-Content C:\out\prc\App_Config\Include\Foundation\MonitoringSitecoreOnAKS.Foundation.StructuredLogging.config) -Replace '###ROLE###', 'PRC' | Set-Content C:\out\prc\App_Config\Include\Foundation\MonitoringSitecoreOnAKS.Foundation.StructuredLogging.config

# Install nuget package locally for Serilog.Formatting.Compact
RUN nuget install Serilog.Formatting.Compact -Version 1.1.0 -OutputDirectory C:\nuget\packages
RUN Invoke-Expression 'robocopy C:\nuget\packages\Serilog.Formatting.Compact.1.1.0\lib\netstandard2.0 C:\out\serilog'  

# Copy module files
COPY module\ C:\module\

FROM ${BASE_IMAGE}

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

WORKDIR C:\module

# Copy final build artifacts
COPY --from=builder C:\out\cm .\cm\content\
COPY --from=builder C:\out\cd .\cd\content\
COPY --from=builder C:\out\prc .\prc\content\

COPY --from=builder C:\out\serilog .\id\content\
COPY --from=builder C:\module\id\content .\id\content\
COPY --from=builder C:\out\serilog .\xdbautomationworker\content\
COPY --from=builder C:\module\xdbautomationworker\content .\xdbautomationworker\content\
COPY --from=builder C:\out\serilog .\cortexprocessing\content\
COPY --from=builder C:\module\cortexprocessing\content .\cortexprocessing\content\
COPY --from=builder C:\out\serilog .\cortexprocessingworker\content\
COPY --from=builder C:\module\cortexprocessingworker\content .\cortexprocessingworker\content\
COPY --from=builder C:\out\serilog .\cortexreporting\content\
COPY --from=builder C:\module\cortexreporting\content .\cortexreporting\content\
COPY --from=builder C:\out\serilog .\xdbautomation\content\
COPY --from=builder C:\module\xdbautomation\content .\xdbautomation\content\
COPY --from=builder C:\out\serilog .\xdbautomationrpt\content\
COPY --from=builder C:\module\xdbautomationrpt\content .\xdbautomationrpt\content\
COPY --from=builder C:\out\serilog .\xdbcollection\content\
COPY --from=builder C:\module\xdbcollection\content .\xdbcollection\content\
COPY --from=builder C:\out\serilog .\xdbrefdata\content\
COPY --from=builder C:\module\xdbrefdata\content .\xdbrefdata\content\
COPY --from=builder C:\out\serilog .\xdbsearch\content\
COPY --from=builder C:\module\xdbsearch\content .\xdbsearch\content\
COPY --from=builder C:\out\serilog .\xdbsearchworker\content\
COPY --from=builder C:\module\xdbsearchworker\content .\xdbsearchworker\content\
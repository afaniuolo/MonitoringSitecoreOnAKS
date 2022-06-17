# escape=`

ARG BASE_IMAGE
ARG BUILD_IMAGE

FROM ${BUILD_IMAGE} AS builder
SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Install nuget package locally
RUN nuget install Microsoft.ApplicationInsights -Version 2.20.0 -OutputDirectory C:\nuget\packages
RUN nuget install Microsoft.ApplicationInsights.Agent.Intercept -Version 2.4.0 -OutputDirectory C:\nuget\packages
RUN nuget install Microsoft.ApplicationInsights.DependencyCollector -Version 2.20.0 -OutputDirectory C:\nuget\packages
RUN nuget install Microsoft.ApplicationInsights.PerfCounterCollector -Version 2.20.0 -OutputDirectory C:\nuget\packages
RUN nuget install Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel -Version 2.20.0 -OutputDirectory C:\nuget\packages
RUN nuget install Microsoft.ApplicationInsights.Web -Version 2.20.0 -OutputDirectory C:\nuget\packages
RUN nuget install Microsoft.ApplicationInsights.WindowsServer -Version 2.20.0 -OutputDirectory C:\nuget\packages
RUN nuget install Microsoft.ApplicationInsights.TraceListener -Version 2.20.0 -OutputDirectory C:\nuget\packages
RUN nuget install Microsoft.AspNet.TelemetryCorrelation -Version 1.0.8 -OutputDirectory C:\nuget\packages
RUN nuget install System.Diagnostics.DiagnosticSource -Version 5.0.0 -OutputDirectory C:\nuget\packages

# Copy nuget packages dlls
RUN Invoke-Expression 'robocopy C:\nuget\packages\Microsoft.ApplicationInsights.2.20.0\lib\net452 C:\out\net452\bin'
RUN Invoke-Expression 'robocopy C:\nuget\packages\Microsoft.ApplicationInsights.2.20.0\lib\netstandard2.0 C:\out\netstandard\bin'
RUN Invoke-Expression 'robocopy C:\nuget\packages\Microsoft.ApplicationInsights.Agent.Intercept.2.4.0\lib\net45 C:\out\net452\bin'
RUN Invoke-Expression 'robocopy C:\nuget\packages\Microsoft.ApplicationInsights.Agent.Intercept.2.4.0\lib\net45 C:\out\netstandard\bin'
RUN Invoke-Expression 'robocopy C:\nuget\packages\Microsoft.ApplicationInsights.DependencyCollector.2.20.0\lib\net452 C:\out\net452\bin'
RUN Invoke-Expression 'robocopy C:\nuget\packages\Microsoft.ApplicationInsights.DependencyCollector.2.20.0\lib\netstandard2.0 C:\out\netstandard\bin'
RUN Invoke-Expression 'robocopy C:\nuget\packages\Microsoft.ApplicationInsights.PerfCounterCollector.2.20.0\lib\net452 C:\out\net452\bin'
RUN Invoke-Expression 'robocopy C:\nuget\packages\Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.2.20.0\lib\net452 C:\out\net452\bin'
RUN Invoke-Expression 'robocopy C:\nuget\packages\Microsoft.ApplicationInsights.Web.2.20.0\lib\net452 C:\out\net452\bin'
RUN Invoke-Expression 'robocopy C:\nuget\packages\Microsoft.ApplicationInsights.WindowsServer.2.20.0\lib\net452 C:\out\net452\bin'
RUN Invoke-Expression 'robocopy C:\nuget\packages\Microsoft.ApplicationInsights.TraceListener.2.20.0\lib\net452 C:\out\net452\bin'
RUN Invoke-Expression 'robocopy C:\nuget\packages\Microsoft.AspNet.TelemetryCorrelation.1.0.8\lib\net45 C:\out\net452\bin'
RUN Invoke-Expression 'robocopy C:\nuget\packages\System.Diagnostics.DiagnosticSource.5.0.0\lib\net45 C:\out\net452\bin'
RUN Invoke-Expression 'robocopy C:\nuget\packages\System.Diagnostics.DiagnosticSource.5.0.0\lib\netstandard1.3 C:\out\netstandard\bin'

FROM ${BASE_IMAGE}

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Copy module files
COPY module\ C:\module\

WORKDIR C:\module

# Copy final build artifacts
COPY --from=builder C:\out\net452 .\cm\content\
COPY --from=builder C:\out\net452 .\cd\content\
COPY --from=builder C:\out\net452 .\prc\content\
COPY --from=builder C:\out\netstandard .\xdbcollection\content\

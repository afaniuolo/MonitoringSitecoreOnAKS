using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MonitoringSitecoreOnAKS.Foundation.StructuredLogging.Settings
{
	public static class LoggingSettings
	{
		public static string LogType => Sitecore.Configuration.Settings.GetSetting("StructuredLogging.LogType", "Sitecore");
		public static string Role => Sitecore.Configuration.Settings.GetSetting("StructuredLogging.Role", "Single");
	}
}
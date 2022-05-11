using System;
using System.Collections.Generic;
using System.Reflection;
using log4net.Layout;
using log4net.spi;
using MonitoringSitecoreOnAKS.Foundation.StructuredLogging.Constants;
using MonitoringSitecoreOnAKS.Foundation.StructuredLogging.Extensions;
using MonitoringSitecoreOnAKS.Foundation.StructuredLogging.Settings;
using Newtonsoft.Json;

namespace MonitoringSitecoreOnAKS.Foundation.StructuredLogging.Layouts
{
	public class JsonLayout : LayoutSkeleton
	{
		public JsonLayout()
        {
            IgnoresException = false;
        }

        public override void ActivateOptions()
        {
        }

        public override string Format(LoggingEvent loggingEvent)
        {
            var dic = new Dictionary<string, object>();

            SetInstrinsics(dic, loggingEvent);
            SetExceptionData(dic, loggingEvent);

            return $"{JsonConvert.SerializeObject(dic)}\n";
        }

        public override bool IgnoresException { get; }

        private void SetInstrinsics(Dictionary<string, object> dictionary, LoggingEvent loggingEvent)
        {
            dictionary.Add(MessageProperties.Timestamp, loggingEvent.TimeStamp.ToUnixTimeMilliseconds());
            dictionary.Add(MessageProperties.ThreadName, loggingEvent.ThreadName);
            dictionary.Add(MessageProperties.MessageText, loggingEvent.RenderedMessage);
            dictionary.Add(MessageProperties.LogLevel, loggingEvent.Level.Name);
            dictionary.Add(MessageProperties.LoggerName, loggingEvent.LoggerName);
            dictionary.Add(MessageProperties.LogType, LoggingSettings.LogType);
            dictionary.Add(MessageProperties.ServerRole, LoggingSettings.Role);
        }

        private void SetExceptionData(Dictionary<string, object> dictionary, LoggingEvent loggingEvent)
        {
            if (!String.IsNullOrEmpty(loggingEvent.GetExceptionStrRep()))
            {
                var exception = GetException(loggingEvent);
                if (!string.IsNullOrEmpty(exception.Message))
                {
                    dictionary.Add(MessageProperties.ErrorMessage, exception.Message);
                }

                if (!string.IsNullOrEmpty(exception.StackTrace))
                {
                    dictionary.Add(MessageProperties.ErrorStack, exception.StackTrace);
                }

                dictionary.Add(MessageProperties.ErrorClass, loggingEvent.MessageObject.GetType().ToString());
            }
        }

        private static Exception GetException(LoggingEvent loggingEvent)
        {
            var exception = GetInstanceField(typeof(LoggingEvent), loggingEvent, "m_thrownException");
            return exception as Exception;
        }

        internal static object GetInstanceField(Type type, object instance, string fieldName)
        {
            BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                                     | BindingFlags.Static;
            FieldInfo field = type.GetField(fieldName, bindFlags);
            return field.GetValue(instance);
        }
    }
}

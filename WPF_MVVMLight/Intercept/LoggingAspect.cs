using AspectInjector.Broker;
using Newtonsoft.Json;
using NLog;
using System;
using System.IO;
using System.Reflection;

namespace WPF_MVVMLight_CRUD.Intercept
{
    // 使用 AspectInjector 織入 Log
    [Aspect(Scope.Global)]
    public class LoggingAspect
    {

        //透過程式設定 NLog 時，要留意 statci logger 取得物件時機
        //要排在 SetupNLog() 之後
        static ILogger logger = null;

        static void SetupNLog()
        {
            var config = new NLog.Config.LoggingConfiguration();
            //偵測程式是不是被放在桌面
            var onDesktop = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) == Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            var consoleTarget = new NLog.Targets.ConsoleTarget();
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, consoleTarget);

            LogManager.Configuration = config;
            logger = LogManager.GetLogger("debug");
        }

        public static void ConfigureNLog(LogLevel consoleLogLevel, LogLevel fileLogLevel)
        {
            // Step 1. Create configuration object
            var config = new NLog.Config.LoggingConfiguration();

            // Step 2. Create targets and add them to the configuration
            var consoleTarget = new NLog.Targets.ConsoleTarget();
            config.AddTarget("console", consoleTarget);

            var fileTarget = new NLog.Targets.FileTarget();
            config.AddTarget("file", fileTarget);

            // Step 3. Set target properties
            consoleTarget.Layout = @"${message} ${onexception:EXCEPTION\:${exception:format=tostring}}";

            fileTarget.Layout = @"${date:format=yyyy-MM-dd HH\:mm\:ss.fff} [${level:uppercase=true}] ${message}  ${onexception:EXCEPTION\:${exception:format=tostring}}";
            fileTarget.FileName = "C:\\temp\\logs\\SpiderOpenQuant.${date:format=yyyy-MM-dd hh}.log";
            fileTarget.ConcurrentWrites = false;
            fileTarget.KeepFileOpen = true;
            fileTarget.OpenFileCacheTimeout = 60;

            // Step 4. Define rules
            var rule1 = new NLog.Config.LoggingRule("*", consoleLogLevel, consoleTarget);
            config.LoggingRules.Add(rule1);

            var rule2 = new NLog.Config.LoggingRule("*", fileLogLevel, fileTarget);
            config.LoggingRules.Add(rule2);

            // Step 5. Activate the configuration
            LogManager.Configuration = config;
        }

        /// <summary>
        /// 執行前
        /// </summary>
        /// <param name="name">方法名</param>
        /// <param name="arguments">參數</param>
        [Advice(Kind.Before, Targets = Target.Method)]
        public void Before([Argument(Source.Name)]string name, [Argument(Source.Arguments)]object[] arguments)
        {
            SetupNLog();
            logger.Debug($"Before，調用方法：'{name}'，調用參數：{JsonConvert.SerializeObject(arguments)}");
        }

        /// <summary>
        /// 執行後
        /// </summary>
        /// <param name="name">方法名</param>
        /// <param name="arguments">參數</param>
        /// <param name="retrrnValue">返回值</param>
        [Advice(Kind.After, Targets = Target.Method)]
        public void After([Argument(Source.Name)] string name, [Argument(Source.Arguments)] object[] arguments, [Argument(Source.ReturnValue)] object returnValue)
        {
            logger.Debug($"After，調用方法：'{name}'，調用參數：{JsonConvert.SerializeObject(arguments)}，返回值為：{JsonConvert.SerializeObject(returnValue)}");
        }

        [Advice(Kind.Around, Targets = Target.Method)]
        public object Around(
            [Argument(Source.Name)] string name,
            [Argument(Source.Arguments)] object[] arguments,
            [Argument(Source.Target)] Func<object[], object> target)
        {
            var result = target(arguments);

            return result;
        }
    }

    [Injection(typeof(LoggingAspect))]
    public class LoggingAttribute : Attribute
    {
        
    }

    [Injection(typeof(LoggingAspect))]
    public class LoggingMethodAttribute : Attribute
    {
        
    }
}

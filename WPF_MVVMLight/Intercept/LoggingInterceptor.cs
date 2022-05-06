using Castle.DynamicProxy;
using Newtonsoft.Json;
using NLog;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace WPF_MVVMLight_CRUD.Intercept
{
    // 使用 Castle 織入 Log
    public class LoggingInterceptor : IInterceptor
    {
        /// <summary>
        /// 日誌記錄器
        /// </summary>
        private Logger _logger;

        private void SetupNLog()
        {
            var config = new NLog.Config.LoggingConfiguration();
            //偵測程式是不是被放在桌面
            var onDesktop = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) == Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            var consoleTarget = new NLog.Targets.ConsoleTarget();
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, consoleTarget);

            LogManager.Configuration = config;
            _logger = LogManager.GetLogger("debug");
        }

        public LoggingInterceptor()
        {
            SetupNLog();
        }

        public void Intercept(IInvocation invocation)
        {
            var dataIntercept = $"【當前執行方法】：{ invocation.Method.Name} \r\n" +
                                $"【傳入的參數有】： {JsonConvert.SerializeObject(invocation.Arguments)}\r\n";

            try
            {
                //執行當前方法   
                invocation.Proceed();

                var returnType = invocation.Method.ReturnType;
                //非同步方法
                if (IsAsyncMethod(invocation.Method))
                {

                    if (returnType != null && returnType == typeof(Task))
                    {
                        //等待方法返回的Task
                        async Task res() => await (Task)invocation.ReturnValue;

                        invocation.ReturnValue = res();
                    }
                    else //Task<TResult>
                    {
                        var returnType2 = invocation.Method.ReflectedType;//獲取返回類型

                        if (returnType2 != null)
                        {
                            var resultType = invocation.Method.ReturnType.GetGenericArguments()[0];

                            MethodInfo methodInfo = typeof(LoggingInterceptor).GetMethod("HandleAsync", BindingFlags.Instance | BindingFlags.Public);

                            var mi = methodInfo.MakeGenericMethod(resultType);
                            invocation.ReturnValue = mi.Invoke(this, new[] { invocation.ReturnValue });
                        }
                    }

                    var type = invocation.Method.ReturnType;
                    var resultProperty = type.GetProperty("Result");

                    if (resultProperty != null)
                        dataIntercept += ($"【執行完成結果】：{JsonConvert.SerializeObject(resultProperty.GetValue(invocation.ReturnValue))}");
                }
                //同步方法
                else
                {
                    if (returnType != null && returnType == typeof(void))
                    {

                    }
                    else
                        dataIntercept += ($"【執行完成結果】：{JsonConvert.SerializeObject(invocation.ReturnValue)}");
                }

                _logger.Debug(dataIntercept);
            }
            catch (Exception ex)
            {
                LogEx(ex, dataIntercept);
            }
        }

        //構造等待返回值的非同步方法
        public async Task<T> HandleAsync<T>(Task<T> task)
        {
            var t = await task;

            return t;
        }

        private void LogEx(Exception ex, string dataIntercept)
        {
            if (ex != null)
            {
                //執行的 service 中，捕獲異常
                dataIntercept += ($"【執行完成結果】：方法中出現異常：{ex.Message + ex.InnerException}\r\n");

                // 異常日誌裡有詳細的堆疊資訊
                Parallel.For(0, 1, e =>
                {
                    _logger.Debug(dataIntercept);
                });
            }
        }

        /// <summary>
        /// 判斷是否非同步方法
        /// </summary>
        public static bool IsAsyncMethod(MethodInfo method)
        {
            return (method.ReturnType == typeof(Task) || (method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>)));
        }
    }
    
}

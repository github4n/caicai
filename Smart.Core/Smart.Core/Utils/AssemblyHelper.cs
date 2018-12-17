using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Smart.Core.Utils
{
    public class AssemblyHelper
    {
        /// <summary>
        /// 黑名单
        /// </summary>
        private static string[] BlackList = { "System", "FluentValidation", "StackExchange", "Microsoft", "Autofac", "Quartz", "EntityFramework", "MySql", "MongoDB", "log4net", "AutoMapper", "NPOI", "CrystalQuartz", "Gma.QrCodeNet", "HtmlAgilityPack", "Common.Logging", "NetPay", "ServiceStack", "Newtonsoft.Json", "LitJson", "Robots", "CsQuery" };

        private static object lockObj = new object();
        private static IEnumerable<Type> modelList;

        static AssemblyHelper()
        {
            lock (lockObj)
            {
                if (modelList == null)
                {
                    lock (lockObj)
                    {
                        //var logger = Modules.ModuleManager.Resolve<ILogger>();
                        var pathList = new List<string> { AppDomain.CurrentDomain.BaseDirectory };
                        List<Assembly> AssemblyList = new List<Assembly>();
                        string path = AppDomain.CurrentDomain.BaseDirectory;
                        if (!path.Contains("bin") && Directory.Exists(path))//如果当前程序目录没有bin，就添加bin，并且需要这个bin真实存在
                        {
                            pathList.Add(Path.Combine(path, "bin"));
                        }

                        try
                        {
                            foreach (var _path in pathList)
                            {
                                foreach (var dir in Directory.GetFiles(_path)
                                                 .Where(i => i.EndsWith("dll", StringComparison.CurrentCultureIgnoreCase)
                                                 || i.EndsWith("exe", StringComparison.CurrentCultureIgnoreCase)))
                                {
                                    var dll = Assembly.LoadFrom(dir);
                                    if (BlackList.Where(j => dll.FullName.StartsWith(j, StringComparison.CurrentCultureIgnoreCase)).Count() == 0)
                                        AssemblyList.Add(dll);
                                }
                            }
                            modelList = AssemblyList.SelectMany(a => a.GetTypes()).Where(i => i.IsClass && !i.IsAbstract);
                        }
                        catch (Exception ex)
                        {
                            //logger.Logger_Debug("Plugin error\r\n" + ex.Message + ex.StackTrace);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 某个类型是否派生于别一个类型，支持泛型接口
        /// </summary>
        /// <param name="subType"></param>
        /// <param name="basicType"></param>
        /// <returns></returns>
        private static bool IsAssignableToGenericType(Type subType, Type basicType)
        {
            if (basicType.IsAssignableFrom(subType))
                return true;

            var interfaceTypes = subType.GetInterfaces();

            foreach (var it in interfaceTypes)
            {
                if (it.IsGenericType && it.GetGenericTypeDefinition() == basicType)
                    return true;
            }

            if (subType.IsGenericType && subType.GetGenericTypeDefinition() == basicType)
                return true;

            Type baseType = subType.BaseType;
            if (baseType == null) return false;

            return false;
        }

        /// <summary>
        /// 得到BIN下面加载程序集里，实现某接口的类型集合
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetTypesByInterfaces(Type @interface)
        {
            return modelList.Where(x => IsAssignableToGenericType(x, @interface));
        }

        /// <summary>
        /// 得到BIN下面加载程序集里，实现某接口的类型名称的集合
        /// </summary>
        /// <param name="interface"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetTypeNamesByInterfaces(Type @interface)
        {
            return GetTypesByInterfaces(@interface).Select(i => i.Name);
        }

        /// <summary>
        /// 根据接口，返回程序运行时里所有实现它的类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="interface"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetCurrentTypesByInterfaces<T>(Type @interface)
        {
            var handlers = @interface.Assembly.GetExportedTypes()
                .Where(x => x.GetInterfaces()
                    .Any(a => a.IsGenericType && a.GetGenericTypeDefinition() == @interface))
                    .Where(h => h.GetInterfaces().Any(ii => ii.GetGenericArguments()
                        .Any(aa => aa == typeof(T))))
                        .ToList();
            return handlers;
        }
    }
}

using EntityModel.Model;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace readXml
{
    class Program
    {
        static void Main(string[] args)
        {
            //普通模式
            var csredis = new CSRedis.CSRedisClient("127.0.0.1:6379,defaultDatabase=1,poolsize=50,ssl=false,writeBuffer=10240");
            //初始化 RedisHelper
            RedisHelper.Initialization(csredis);
            //Install-Package Caching.CSRedis (本篇不需要) 
            //注册mvc分布式缓存
            //services.AddSingleton<IDistributedCache>(new Microsoft.Extensions.Caching.Redis.CSRedisCache(RedisHelper.Instance));
            Test();
            Console.ReadKey();
        }

        static void Test()
        {

            RedisHelper.Set("name", "祝雷");//设置值。默认永不过期
            //RedisHelper.SetAsync("name", "祝雷");//异步操作
            Console.WriteLine(RedisHelper.Get<String>("name"));

            RedisHelper.Set("time", DateTime.Now, 1);
            Console.WriteLine(RedisHelper.Get<DateTime>("time"));
            Thread.Sleep(1100);
            Console.WriteLine(RedisHelper.Get<DateTime>("time"));

            // 列表
            RedisHelper.RPush("list", "第一个元素");
            RedisHelper.RPush("list", "第二个元素");
            RedisHelper.LInsertBefore("list", "第二个元素", "我是新插入的第二个元素！");
            Console.WriteLine($"list的长度为{RedisHelper.LLen("list")}");
            //Console.WriteLine($"list的长度为{RedisHelper.LLenAsync("list")}");//异步
            Console.WriteLine($"list的第二个元素为{RedisHelper.LIndex("list", 1)}");
            //Console.WriteLine($"list的第二个元素为{RedisHelper.LIndexAsync("list",1)}");//异步
            // 哈希
            RedisHelper.HSet("person", "name", "zhulei");
            RedisHelper.HSet("person", "sex", "男");
            RedisHelper.HSet("person", "age", "28");
            RedisHelper.HSet("person", "adress", "hefei");
            Console.WriteLine($"person这个哈希中的age为{RedisHelper.HGet<int>("person", "age")}");
            //Console.WriteLine($"person这个哈希中的age为{RedisHelper.HGetAsync<int>("person", "age")}");//异步


            // 集合
            RedisHelper.SAdd("students", "zhangsan", "lisi");
            RedisHelper.SAdd("students", "wangwu");
            RedisHelper.SAdd("students", "zhaoliu");
            Console.WriteLine($"students这个集合的大小为{RedisHelper.SCard("students")}");
            Console.WriteLine($"students这个集合是否包含wagnwu:{RedisHelper.SIsMember("students", "wangwu")}");


            //普通订阅
            RedisHelper.Subscribe(
              ("chan1", msg => Console.WriteLine(msg.Body)),
              ("chan2", msg => Console.WriteLine(msg.Body)));

            //模式订阅（通配符）
            RedisHelper.PSubscribe(new[] { "test*", "*test001", "test*002" }, msg => {
                Console.WriteLine($"PSUB   {msg.MessageId}:{msg.Body}    {msg.Pattern}: chan:{msg.Channel}");
            });
            //模式订阅已经解决的难题：
            //1、分区的节点匹配规则，导致通配符最大可能匹配全部节点，所以全部节点都要订阅
            //2、本组 "test*", "*test001", "test*002" 订阅全部节点时，需要解决同一条消息不可执行多次

            //发布
            
            Console.WriteLine(RedisHelper.Publish("chan1", "129993123123"));

           //Task taskTest = Task.Factory.StartNew(() =>
           //{
           //    for (int i = 0; i < 100; i++)
           //    {
           //        Console.WriteLine("第一种" + i);
           //    }

           //}, TaskCreationOptions.None);
           //taskTest.Wait();
           //Task taskTwo = Task.Factory.StartNew(() =>
           //{
           //    for (int i = 0; i < 100; i++)
           //    {
           //        Console.WriteLine("第二种" + i);
           //    }

           //}, TaskCreationOptions.None);
           //taskTwo.Wait();
           //Task taskTwo2 = Task.Factory.StartNew(() =>
           //{
           //    for (int i = 0; i < 100; i++)
           //    {
           //        Console.WriteLine("第三种" + i);
           //    }
           //}, TaskCreationOptions.None);

           //taskTwo2.Wait();


           // Console.WriteLine("123");
           //ThreadPool.QueueUserWorkItem(StartCode, taskTest);
           //    ThreadPool.QueueUserWorkItem(StartCode, taskTwo);
           //Task.ContinueWith(taskTest, taskTwo, taskTwo2);

           Console.WriteLine("等待taskTest和taskTwo执行后再执行");


          //  Task parent = new Task(() =>
          //  {
          //      CancellationTokenSource cts = new CancellationTokenSource(5000);
          //        //创建任务工厂
          //        TaskFactory tf = new TaskFactory(cts.Token, TaskCreationOptions.AttachedToParent,
          //      TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
          //        //添加一组具有相同状态的子任务
          //        Task[] task = new Task[]{
          //      tf.StartNew(() => { for (int i = 0; i < 100; i++)
          //      {
          //          Console.WriteLine("第一种" + i);
          //      } }),
          //      tf.StartNew(() => {     for (int i = 0; i < 100; i++)
          //      {
          //          Console.WriteLine("第二种"+i);
          //      } }),
          //      tf.StartNew(() => {     for (int i = 0; i < 100; i++)
          //      {
          //          Console.WriteLine("第三种"+i);
          //      } })
          //};
          //  });
          //  parent.Start();

            //Console.WriteLine("Time Job Start");
            //RunProgram().GetAwaiter().GetResult();
            //Console.WriteLine("Hello World!");
            Console.Read();
            Console.ReadKey();
        }

        private static void StartCode(object i)
        {
            Console.WriteLine("开始执行子线程...{0}", i);
            Thread.Sleep(1000);//模拟代码操作    
        }

        //读取xml
        public static void readXml()
        {
            //辽宁快乐12  广东快乐十分  广西快乐10分 重庆时时彩 是网页版
            //gdklsf(广东快乐十分)  bjsyxw(北京11选5)  kl8(北京快乐8)   bjkzhc(北京快中彩)  bjpkshi(北京PK拾) bjk3(北京快3)  tjsyxw(天津11选5)
            //klsf(天津快乐十分)  tjssc(天津时时彩)  hebsyxw(河北11选5)  hebk3(河北快3)   nmgsyxw(内蒙古11选5)  nmgk3(内蒙古快3)  lnsyxw(辽宁11选5)
            //jlsyxw(吉林11选5)  jlk3(吉林快3)  hljsyxw(黑龙江11选5)  hljklsf(黑龙江快乐十分)   shhsyxw(上海11选5)  shhk3(上海快3)  ssl(上海时时乐) 
            //jssyxw(江苏11选5)  jsk3(江苏快3)  zjsyxw(浙江11选5)  zjkl12(浙江快乐12)  ahsyxw(安微11选5) ahk3(安微快三)  fjsyxw(福建11选5)
            //fjk3(福建快3)   dlc(江西11选5)  jxssc(江西时时彩) jxk3(江西快3)  qyh(山东群英会) shdsyxw(山东十一运夺冠)  shdklpk3(山东快乐扑克3)
            //hensyxw(河南11选5)  henk3(河南快3)  henky481(河南快赢481)  hbsyxw(湖北11选5)  hbk3(湖北快3)  hnklsf(湖南快乐十分) xysc(湖南快乐赛车)
            //gdsyxw(广东11选5)   gxsyxw(广西11选5)  gxk3(广西快3)  chqklsf(重庆快乐十分)  sckl12(四川快乐12)  gzsyxw(贵州11选5)  
            //gzk3(贵州快3)  sxsyxw(陕西11选5)  sxklsf(陕西快乐十分)  gssyxw(甘肃11选5)  gsk3(甘肃快3)  qhk3(青海快3)  xjsyxw(新疆11选5)
            //xjssc(新疆时时彩)   xjxlc(新疆喜乐彩)  shxsyxw(山西11选5)  ytdj(山西泳坛夺金) shxklsf(山西快乐十分)  ynsyxw(云南11选5)
            //ynklsf(云南快乐10分)  ynssc(云南时时彩)
            string htmlCode;
            var gameCode = "gdklsf";
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            string date = DateTime.Now.AddDays(-2).ToString("yyyyMMdd");
            string Url = "http://kaijiang.500.com/static/info/kaijiang/xml/" + gameCode + "/" + date + ".xml";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "GET";
            request.ContentType = "text/xml";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.ContentEncoding.ToLower() == "gzip")//如果使用了GZip则先解压  
            {
                System.IO.Stream streamReceive = response.GetResponseStream();
                var zipStream = new System.IO.Compression.GZipStream(streamReceive, System.IO.Compression.CompressionMode.Decompress);
                StreamReader sr = new System.IO.StreamReader(zipStream, Encoding.GetEncoding("GB2312"));
                htmlCode = sr.ReadToEnd();
            }
            else
            {
                System.IO.Stream streamReceive = response.GetResponseStream();

                StreamReader sr = new System.IO.StreamReader(streamReceive, Encoding.GetEncoding("GB2312"));

                htmlCode = sr.ReadToEnd();
            }

            XmlDocument doc = new System.Xml.XmlDocument();//新建对象
            doc.LoadXml(htmlCode);
            List<DataModel> lists = new List<DataModel>();
            ArrayList arrNodeList = new ArrayList();
            XmlNodeList list = doc.SelectNodes("//row");
            foreach (XmlNode item in list)
            {
                arrNodeList.Add(item);
            }
            arrNodeList.Reverse();
            foreach (XmlNode item in arrNodeList)
            {

                DataModel cust = new DataModel();
                cust.expect = item.Attributes["expect"].Value;
                cust.opencode = item.Attributes["opencode"].Value;
                cust.opentime = Convert.ToDateTime(item.Attributes["opentime"].Value);
                Console.WriteLine("开奖期号:" + cust.expect + "    开奖号码:" + cust.opencode + "   开奖时间:" + cust.opentime);
            }

        }

        private static async Task RunProgram()
        {
            try
            {
                // Grab the Scheduler instance from the Factory  
                NameValueCollection props = new NameValueCollection
                {
                    { "quartz.serializer.type", "binary" }
                };
                StdSchedulerFactory factory = new StdSchedulerFactory(props);
                IScheduler scheduler = await factory.GetScheduler();


                // 启动任务调度器  
                await scheduler.Start();


                // 定义一个 Job  
                IJobDetail job = JobBuilder.Create<HelloJob>()
                    .WithIdentity("job1", "group1")
                    .Build();
                ISimpleTrigger trigger = (ISimpleTrigger)TriggerBuilder.Create()
                    .WithIdentity("trigger1") // 给任务一个名字  
                    .StartAt(DateTime.Now) // 设置任务开始时间  
                    .ForJob("job1", "group1") //给任务指定一个分组  
                    .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(10)  //循环的时间 1秒1次 
                    .RepeatForever())
                    .Build();


                // 等待执行任务  
                await scheduler.ScheduleJob(job, trigger);


                // some sleep to show what's happening  
                //await Task.Delay(TimeSpan.FromMilliseconds(2000));  
            }
            catch (SchedulerException se)
            {
                await Console.Error.WriteLineAsync(se.ToString());
            }
        }

        public class HelloJob : IJob
        {
            public Task Execute(IJobExecutionContext context)
            {
                return Console.Out.WriteLineAsync("Greetings from HelloJob!");
            }
        }

    }
}

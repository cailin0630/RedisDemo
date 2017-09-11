using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
namespace RedisDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var stopWatch=new Stopwatch();
            stopWatch.Start();
            var key = new string[1000];
            var value = new string[1000];
            for (var i = 0; i < 1000; i++)
            {
               
                key[i] = $"{i}";
                value[i] = $"-{i}";

                //SeRedisHelper.StringSet($"{i}", $"{i}{i}");
                //Console.WriteLine($"set {i} {i}{i}");
                //var result = SeRedisHelper.StringGet($"{i}");
                //if (!result.IsSuccess)
                //{
                //    Console.WriteLine($"get {i} {result.Message}");
                //}
                //else
                //{
                //    Console.WriteLine($"get {i} {result.Value}");
                //}
                
            }
            //SeRedisHelper.StringSetMany(key, value);
            //SeRedisHelper.EntitySet("jy", new
            //{
            //    name="jiangyi",
            //    age="26",
            //    sex="男"
            //},null,2);
            var ret= SeRedisHelper.EntityGet<object>("jy", 2);
            stopWatch.Stop();
            Console.WriteLine($"{stopWatch.ElapsedMilliseconds}\n{ret}");
            Console.ReadLine();
           
        }
    }
}

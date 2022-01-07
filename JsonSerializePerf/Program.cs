
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.IO;
using System.Json;
using System.Text;
using System.Text.Json;

namespace JsonSerializePerf
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            /*
            var data = new
            {
                name = "test",
                prop = "value",
            };
            ReadOnlyMemory<byte[]> response = new ReadOnlyMemory<byte[]>(data);
            */
            //var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
            //Console.WriteLine(summary);

            BenchmarkRunner.Run<JsonSerialize>();

        }
    }
}

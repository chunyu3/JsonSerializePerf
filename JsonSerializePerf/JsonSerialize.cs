using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Text;
using System.Text.Json;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace JsonSerializePerf
{
    [MemoryDiagnoser]
    public class JsonSerialize
    {
        //static string  someString = "{\"Id\":\"11111111-1111-1111-0000-000000000000\", \"Description\": \"some tool\", \"Count\":5}";
        //static SomeModel sm = new SomeModel(new Guid("11111111-1111-1111-0000-000000000000"), "some tool", 5);
        //static string someString = JsonSerializer.Serialize(sm);
        static string path = @"C:\MyProject\JsonSerializePerf\JsonSerializePerf\HDInsightOnDemandLinkedService.json";
        static string someString = File.ReadAllText(path);
        static byte[] data = Encoding.ASCII.GetBytes(someString);
        private readonly ReadOnlyMemory<byte> response = new ReadOnlyMemory<byte>(data);

        private readonly MemoryStream mstream = new(data);
        
        //mstream.Write(someString, 0, someString.size());

        [Benchmark]
        public SomeModel UseEnumerateObject()
        {
            using var doc = JsonDocument.Parse(response);
            var root = doc.RootElement;
            Guid id = Guid.Empty;
            string description = null;
            int count = 0;
            foreach (var property in root.EnumerateObject())
            {
                if (property.NameEquals("ID"))
                {
                    id = property.Value.GetGuid();
                    //id = Guid.NewGuid();
                }
                else if (property.NameEquals("Description"))
                {
                    description = property.Value.GetString();
                }
                else if (property.NameEquals("Count"))
                {
                    count = property.Value.GetInt32();
                }
            }

            if (id == Guid.Empty)
            {
                throw new ArgumentNullException();
            }

            return new SomeModel(id, description, count);
        }
        
        [Benchmark]
        public SomeModel UseTryGetProperty()
        {
            using var doc = JsonDocument.Parse(response);
            var root = doc.RootElement;
            return new SomeModel(
                root.TryGetProperty("ID", out var property) ? property.GetGuid() : throw new ArgumentNullException(),
                root.TryGetProperty("Description", out property) ? property.GetString() : null,
                root.TryGetProperty("Count", out property) ? property.GetInt32() : 0
            );
        }
        
        [Benchmark]
        public SomeModel UseJsonValue()
        {
            mstream.Seek(0, SeekOrigin.Begin);
            var root = (JsonObject)JsonValue.Load(mstream);
            return new SomeModel(
                root.TryGetValue("ID", out var property) ? new Guid(property) : throw new ArgumentNullException(),
                root.TryGetValue("Description", out property) ? (string)property : null,
                root.TryGetValue("Count", out property) ? property : 0
            );
        }
    }
}

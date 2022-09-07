using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedCacheLinkage.Package
{
    static class Json
    {
        internal static T FromJsonString<T>(string p) => Newtonsoft.Json.JsonConvert.DeserializeObject<T>(p);
        internal static string AsJsonString<T>(T p) where T : class
        =>
        Newtonsoft.Json.JsonConvert.SerializeObject(
            p,
            Newtonsoft.Json.Formatting.None,
            new Newtonsoft.Json.JsonSerializerSettings
            {
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,

            });
    }
}

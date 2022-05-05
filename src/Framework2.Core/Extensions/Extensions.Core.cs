using Newtonsoft.Json;

namespace Framework2.Core.Extensions
{
    public static partial class Extensions
    {
        public static T Tap<T>(this T @object, Action<T> action)
            where T : class
        {
            action(@object);
            return @object;
        }

        public static T Copy<T>(this T @object)
            where T : class
        {
            var serial = JsonConvert.SerializeObject(@object);
            return JsonConvert.DeserializeObject<T>(serial)!;
        }
    }
}

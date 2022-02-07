namespace Common.Domain.Core.Extensions
{
    public static partial class Extensions
    {
        public static T Tap<T>(this T @object, Action<T> action)
        {
            action(@object);
            return @object;
        }
    }
}

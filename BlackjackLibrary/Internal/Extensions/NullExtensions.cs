using System;

namespace BlackjackLibrary.Extensions
{
    internal static class NullExtensions
    {
        public static T GuardNotNull<T>(this T obj) where T : class
        {
            return obj ?? throw new ArgumentNullException(obj.GetType().Name + " is null");
        }

        public static bool NotNull<T>(this T obj) where T : class
        {
            return obj != null;
        }

        public static bool IsNull<T>(this T obj) where T : class
        {
            return obj == null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ProxyManager
{
    [DebuggerStepThrough()]
    internal static class ExtensionMethods
    {
        public static bool ElementsAreSame<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            var firstSet = new HashSet<T>();
            var count = 0;  // Used to keep track of how many elements are in both collections.
            // Count up to number of items in `first` and then count down in `second`.
            // The final result will be zero if they both have same number of elements.

            foreach (var item in first)
            {
                count += 1;
                firstSet.Add(item);
            }

            foreach (var item in second)
            {
                count -= 1;
                if (!firstSet.Remove(item)) // the item we're removing is not present in first set
                    return false;           // the two are not the same
            }

            return count == 0 && firstSet.Count == 0;
        }

        public static T As<T>(this object o)
        {
            try { return (T)o; }
            catch { return default(T); }
        }

        #region Enum Extensions

        public static T[] GetAllEnumValues<T>(this T value) where T : struct
        {
            // ensure it is an Enum we're going to be processing
            if (!typeof(T).IsEnum)
                throw new ArgumentException("Argument is not of type System.Enum");

            return (T[])Enum.GetValues(typeof(T));
        }

        public static T[] GetAllFlagsPresent<T>(this T value) where T : struct
        {
            // ensure it is an Enum we're going to be processing
            if (!typeof(T).IsEnum)
                throw new ArgumentException("Argument is not of type System.Enum");

            if (Convert.ToInt64(value) == 0) return new[] { value };

            var r = new List<T>();
            foreach (var f in value.GetAllEnumValues())
                if (Convert.ToInt64(f) != 0 && value.HasFlag(f))
                    r.Add(f);

            return r.ToArray();
        }

        public static bool HasFlag<T>(this T value, T flag) where T : struct
        {
            // ensure it is an Enum we're going to be processing
            if (!typeof(T).IsEnum)
                throw new ArgumentException("Argument is not of type System.Enum");

            // does not have flag attribute set
            if (!typeof(T).IsDefined(typeof(FlagsAttribute), false))
                return false;

            var e = Convert.ToInt64(value);
            var f = Convert.ToInt64(flag);

            return ((e & f) == f);
        }

        #endregion Enum Extensions

        public static bool IsNullOrBlank(this string s)
        {
            if (s != null)
                for (int i = 0; i < s.Length; i++)
                    if (!char.IsWhiteSpace(s[i]))
                        return false;

            return true;
        }

        public static bool None<T>(this IEnumerable<T> sequence, Predicate<T> predicate)
        {
            foreach (T local in sequence)
                if (predicate(local))
                    return false;

            return true;
        }

        public static TResult With<T, TResult>(this T o, Func<T, TResult> m)
        {
            return m.Invoke(o);
        }

        public static void With<T>(this T o, Action<T> m)
        {
            m.Invoke(o);
        }
    }
}
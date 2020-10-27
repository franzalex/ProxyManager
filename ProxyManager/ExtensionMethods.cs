using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ProxyManager
{
    [DebuggerStepThrough()]
    internal static class ExtensionMethods
    {
        #region Enum Extensions

        /// <summary>Gets all valid values of the <typeparamref name="T" /> enum.</summary>
        /// <typeparam name="T">The type of enum whose values are to be returned.</typeparam>
        /// <param name="value">The value from which all values are to be extracted.</param>
        /// <returns>Returns all the Enum values of the type <typeparamref name="T" />.</returns>
        /// <exception cref="System.ArgumentException">Argument is not of type System.Enum</exception>
        public static T[] GetAllEnumValues<T>(this T value) where T : struct
        {
            // ensure it is an Enum we're going to be processing
            if (!typeof(T).IsEnum)
                throw new ArgumentException("Argument is not of type System.Enum");

            return (T[])Enum.GetValues(typeof(T));
        }

        /// <summary>Gets all flags present in the specified value.</summary>
        /// <typeparam name="T">The type of enum whose flag values are to be returned.</typeparam>
        /// <param name="value">The value containing multiple flags.</param>
        /// <returns>
        /// Returns all flags of type <typeparamref name="T" /> in <paramref name="value" />.
        /// </returns>
        /// <exception cref="System.ArgumentException">Argument is not of type System.Enum</exception>
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

        /// <summary>Determines whether the specified Enum flag is present in the specified value.</summary>
        /// <typeparam name="T">The type of enum to be evaluated</typeparam>
        /// <param name="value">
        /// The value from which the presence of the specified flag is to be evaluated.
        /// </param>
        /// <param name="flag">The flag to check for in <paramref name="value" />.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="flag" /> is present in <paramref name="value" />;
        /// otherwise <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentException">Argument is not of type System.Enum</exception>
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

        /// <summary>
        /// Casts the specified object to another type or returns <c>null</c> if conversion fails.
        /// </summary>
        /// <typeparam name="T">The type to cast <paramref name="obj" /> to</typeparam>
        /// <param name="obj">The object to cast to another type.</param>
        /// <returns>
        /// <paramref name="obj" /> cast to <typeparamref name="T" /> otherwise the default value of
        /// <typeparamref name="T" />.
        /// </returns>
        public static T As<T>(this object obj)
        {
            try { return (T)obj; }
            catch { return default(T); }
        }

        /// <summary>Determines whether the elements of two sequences are the same.</summary>
        /// <typeparam name="T">The type of elements in the two sequences.</typeparam>
        /// <param name="first">The first sequence to check.</param>
        /// <param name="second">The second sequence to check.</param>
        /// <returns><c>true</c> if the elements of both sequences are the same; otherwise <c>false</c>.</returns>
        public static bool ElementsAreSame<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            var firstSet = new HashSet<T>();
            var count = 0;  // Used to keep track of how many elements are in both collections.
            // Count up to number of items in `first` and then count down in `second`. The final
            // result will be zero if they both have same number of elements.

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

        /// <summary>Ensures the value is greater or equal to the specified minimum.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="min">The minimum value.</param>
        /// <returns>
        ///   <paramref name="value" /> if it is greater than <paramref name="min" />; else <paramref name="min" />.
        /// </returns>
        public static T EnsureMin<T>(this T value, T min) where T : IComparable
        {
            if (value.CompareTo(min) < 0)
                return min;

            return value;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.String" /> instance is null or empty.
        /// </summary>
        /// <param name="s">The string to be checked.</param>
        /// <returns><c>true</c> if the string is null or empty; otherwise <c>false</c>.</returns>
        public static bool IsNullOrEmpty(this string s)
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

        /// <summary>Checks if two objects are not null and are equal.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o1">The first object to be compared.</param>
        /// <param name="o2">The second object to be compared.</param>
        /// <returns><c>true</c> if both objects are not null and are equal; otherwise <c>false</c>.</returns>
        /// <remarks>
        /// This function first checks that both objects being compared are not null before calling
        /// the object's Equal method. This extension method enables cleaner code where all
        /// null-checks in equality comparisons are shuttled away here.
        /// </remarks>
        public static bool NullCheckEquality<T>(this T o1, T o2)
        {
            if (o1 == null || o2 == null)
                return false;
            else
                return o1.Equals(o2);
        }

        /// <summary>Simulates a VB-style With statement which returns a result.</summary>
        /// <typeparam name="T">The type of object on which the method is to be performed.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="obj">The object on which <paramref name="method" /> will be run.</param>
        /// <param name="method">The method to run on the <paramref name="obj" /> on.</param>
        /// <returns>The result of the <paramref name="method" /> parameter.</returns>
        public static TResult With<T, TResult>(this T obj, Func<T, TResult> method)
        {
            return method.Invoke(obj);
        }

        /// <summary>Simulates a VB-style With statement with no return value.</summary>
        /// <typeparam name="T">The type of object on which the method is to be performed.</typeparam>
        /// <param name="obj">The object on which <paramref name="method" /> will be run.</param>
        /// <param name="method">The method to run on the <paramref name="obj" /> on.</param>
        public static void With<T>(this T obj, Action<T> method)
        {
            method.Invoke(obj);
        }
    }
}
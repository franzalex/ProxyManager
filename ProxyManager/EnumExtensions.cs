using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace ProxyManager
{
    [System.Diagnostics.DebuggerStepThrough()]
    internal static class EnumInternals<T> where T : struct
    {
        internal static readonly bool IsFlags;
        internal static readonly Func<T, T, T> Or;
        internal static readonly Func<T, T, T> And;
        internal static readonly Func<T, T> Not;
        internal static Func<T, T, bool> Equality;
        internal static readonly Func<T, bool> IsEmpty;
        internal static readonly Type UnderlyingType;

        static EnumInternals()
        {
            UnderlyingType = Enum.GetUnderlyingType(typeof(T));
            IsFlags = typeof(T).IsDefined(typeof(FlagsAttribute), false);
            // Parameters for various expression trees
            ParameterExpression param1 = Expression.Parameter(typeof(T), "x");
            ParameterExpression param2 = Expression.Parameter(typeof(T), "y");
            Expression convertedParam1 = Expression.Convert(param1, UnderlyingType);
            Expression convertedParam2 = Expression.Convert(param2, UnderlyingType);
            Equality = Expression.Lambda<Func<T, T, bool>>(Expression.Equal(convertedParam1, convertedParam2), param1, param2).Compile();
            Or = Expression.Lambda<Func<T, T, T>>(Expression.Convert(Expression.Or(convertedParam1, convertedParam2), typeof(T)), param1, param2).Compile();
            And = Expression.Lambda<Func<T, T, T>>(Expression.Convert(Expression.And(convertedParam1, convertedParam2), typeof(T)), param1, param2).Compile();
            Not = Expression.Lambda<Func<T, T>>(Expression.Convert(Expression.Not(convertedParam1), typeof(T)), param1).Compile();
            IsEmpty = Expression.Lambda<Func<T, bool>>(Expression.Equal(convertedParam1,
                Expression.Constant(Activator.CreateInstance(UnderlyingType))), param1).Compile();
        }
    }

    /// <summary>
    /// Provides a set of static methods for use with enum types. Much of
    /// what's available here is already in System.Enum, but this class
    /// provides a strongly typed API.
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough()]
    public static class Enums
    {
        /// <summary>
        /// Returns an array of values in the enum.
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <returns>An array of values in the enum</returns>
        public static T[] AllEnumValues<T>() where T : struct
        {
            return (T[])Enum.GetValues(typeof(T));
        }

        public static IEnumerable<T> AllFlagsPresent<T>(this T value) where T : struct
        {
            foreach (var flag in AllEnumValues<T>())
            {
                if (value.ContainsFlag(flag))
                    yield return flag;
            }
        }

        public static bool ContainsFlag<T>(this T value, T flag) where T : struct
        {
            return EnumInternals<T>.IsFlags && EnumInternals<T>.Equality(flag, EnumInternals<T>.And(value, flag));
        }

        /// <summary>
        /// Returns an array of all enum values of the type specified.
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="value">A member of the desired enum type</param>
        /// <returns>An array of values in the enum.</returns>
        public static T[] GetAllEnumValues<T>(this T value) where T : struct
        {
            return AllEnumValues<T>();
        }

        /// <summary>
        /// Returns an array of names in the enum.
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <returns>An array of names in the enum</returns>
        public static string[] GetNamesArray<T>() where T : struct
        {
            return Enum.GetNames(typeof(T));
        }

        /// <summary>
        /// Checks whether the value is a named value for the type.
        /// </summary>
        /// <remarks>
        /// For flags enums, it is possible for a value to be a valid
        /// combination of other values without being a named value
        /// in itself. To test for this possibility, use IsValidCombination.
        /// </remarks>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="value">Value to test</param>
        /// <returns>True if this value has a name, False otherwise.</returns>
        public static bool IsNamedValue<T>(this T value) where T : struct
        {
            // TODO: Speed this up for big enums
            return AllEnumValues<T>().Contains(value);
        }

        /// <summary>
        /// Parses the name of an enum value.
        /// </summary>
        /// <remarks>
        /// This method only considers named values: it does not parse comma-separated
        /// combinations of flags enums.
        /// </remarks>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="name">Name to parse</param>
        /// <returns>The parsed value</returns>
        /// <exception cref="ArgumentException">The name could not be parsed.</exception>
        public static T ParseName<T>(string name) where T : struct
        {
            T value;
            if (!TryParseName(name, out value))
            {
                throw new ArgumentException("Unknown name", "name");
            }
            return value;
        }

        /// <summary>
        /// Attempts to find a value for the specified name.
        /// Only names are considered - not numeric values.
        /// </summary>
        /// <remarks>
        /// If the name is not parsed, <paramref name="value"/> will
        /// be set to the zero value of the enum. This method only
        /// considers named values: it does not parse comma-separated
        /// combinations of flags enums.
        /// </remarks>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="name">Name to parse</param>
        /// <param name="value">Enum value corresponding to given name</param>
        /// <returns>Whether the parse attempt was successful or not</returns>
        public static bool TryParseName<T>(string name, out T value) where T : struct
        {
            // TODO: Speed this up for big enums
            int index = (new List<string>(Enum.GetNames(typeof(T)))).IndexOf(name);
            if (index == -1)
            {
                value = default(T);
                return false;
            }
            value = AllEnumValues<T>()[index];
            return true;
        }

        /// <summary>
        /// Returns the underlying type for the enum
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <returns>The underlying type (Byte, Int32 etc) for the enum</returns>
        public static Type GetUnderlyingType<T>() where T : struct
        {
            return Enum.GetUnderlyingType(typeof(T));
        }
    }
}
using System.Collections.Generic;

namespace ProxyManager
{
    public static class Tuple
    {
        public static Tuple<T1, T2> Create<T1, T2>(T1 first, T2 second)
        {
            return new Tuple<T1, T2>(first, second);
        }
    }

    /// <summary>
    /// Represents a container with two objects
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public class Tuple<T1, T2> : IEqualityComparer<Tuple<T1, T2>>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        public Tuple(T1 first, T2 second)
        {
            Item1 = first;
            Item2 = second;
        }

        /// <summary>
        /// First item
        /// </summary>
        public T1 Item1 { get; private set; }

        /// <summary>
        /// Second item
        /// </summary>
        public T2 Item2 { get; private set; }

        /// <summary>
        /// Overrides the != operator
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Tuple<T1, T2> left, Tuple<T1, T2> right)
        {
            if (((object)left) == null && ((object)right) == null)
            {
                return false;
            }

            return !left.Equals(right);
        }

        /// <summary>
        /// Overrides the == operator
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Tuple<T1, T2> left, Tuple<T1, T2> right)
        {
            if (((object)left) == null && ((object)right) == null)
            {
                return true;
            }

            return left.Equals(right);
        }

        /// <summary>
        /// Checks for equality
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(Tuple<T1, T2> x, Tuple<T1, T2> y)
        {
            return EqualityComparer<T1>.Default.Equals(x.Item1, y.Item1)
          && EqualityComparer<T2>.Default.Equals(x.Item2, y.Item2);
        }

        /// <summary>
        /// Checks for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is Tuple<T1, T2> && Equals(this, (Tuple<T1, T2>)obj);
        }

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return EqualityComparer<T1>.Default.GetHashCode(Item1) ^
                   EqualityComparer<T2>.Default.GetHashCode(Item2);
        }

        /// <summary>Returns a hash code for this instance.</summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public int GetHashCode(Tuple<T1, T2> obj)
        {
            return obj.GetHashCode();
        }
    }
}
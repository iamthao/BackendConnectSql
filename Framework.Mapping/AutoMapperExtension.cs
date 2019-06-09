using System;
using System.Collections;
using System.Collections.Generic;
using AutoMapper;

namespace Framework.Mapping
{
    public static class AutoMapperExtension
    {
        /// <summary>
        ///     The map to.
        /// </summary>
        /// <param name="self">
        ///     The self enumurable.
        /// </param>
        /// <typeparam name="TResult">
        ///     The generic result.
        /// </typeparam>
        /// <returns>
        ///     The System.Collections.Generic.List`1[T -&gt; TResult].
        /// </returns>
        public static List<TResult> MapTo<TResult>(this IEnumerable self)
        {
            if (self == null)
            {
                throw new ArgumentNullException();
            }

            return (List<TResult>)Mapper.Map(self, self.GetType(), typeof(List<TResult>));
        }

        /// <summary>
        ///     Map entity to another entity.
        /// </summary>
        /// <param name="self">
        ///     The self object.
        /// </param>
        /// <typeparam name="TResult">
        ///     The generic result.
        /// </typeparam>
        /// <returns>
        ///     The ressult entity.
        /// </returns>
        public static TResult MapTo<TResult>(this object self)
        {
            if (self == null)
            {
                throw new ArgumentNullException();
            }

            return (TResult)Mapper.Map(self, self.GetType(), typeof(TResult));
        }

        /// <summary>
        ///     The map properties to instance.
        /// </summary>
        /// <param name="self">
        ///     The self object.
        /// </param>
        /// <param name="value">
        ///     The value to be mapped.
        /// </param>
        /// <typeparam name="TResult">
        ///     The generic result.
        /// </typeparam>
        /// <returns>
        ///     The TResult.
        /// </returns>
        public static TResult MapPropertiesToInstance<TResult>(this object self, TResult value)
        {
            if (self == null)
            {
                throw new ArgumentNullException();
            }

            return (TResult)Mapper.Map(self, value, self.GetType(), typeof(TResult));
        }

        /// <summary>
        ///     The dynamic map to.
        /// </summary>
        /// <param name="self">
        ///     The self object.
        /// </param>
        /// <typeparam name="TResult">
        ///     The generic result.
        /// </typeparam>
        /// <returns>
        ///     The TResult.
        /// </returns>
        public static TResult DynamicMapTo<TResult>(this object self)
        {
            if (self == null)
            {
                throw new ArgumentNullException();
            }

            return (TResult)Mapper.DynamicMap(self, self.GetType(), typeof(TResult));
        }
    }
}
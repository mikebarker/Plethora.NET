using System.Collections.Generic;

namespace Plethora.Cache
{
    /// <summary>
    /// Interface defining the methods required by the <see cref="CacheBase{TData,TArgument}"/> class.
    /// </summary>
    /// <typeparam name="TData">The data type returned by </typeparam>
    /// <typeparam name="TArgument">The implementation type of this interface.</typeparam>
    /// <example>
    /// To use this interface with the <see cref="CacheBase{TData,TArgument}"/> class, the
    /// generic parameter <typeparamref name="TArgument"/> should be set to the implementing
    /// class, e.g.:
    ///   <code>
    ///   <![CDATA[
    ///       public class FooBarArgument : IArgument<FooBar, FooBarArgument>
    ///       {
    ///           //...
    ///       }
    ///   ]]>
    ///   </code>
    /// </example>
    /// <remarks>
    /// <see cref="IArgument{TData,TArgument}"/> should be thought of as representing a set within the 
    /// key-space of the data class. That is if the data is uniquely identified by a single ID field,
    /// then the <see cref="IArgument{TData,TArgument}"/> implementation should describe the ID (or IDs)
    /// of data required. It may represent a single ID (e.g. 10564) or a range (e.g. 10200-10800).
    /// </remarks>
    public interface IArgument<TData, TArgument>
    {
        /// <summary>
        /// Gets a value indicating whether two arguments overlap in the key-space which they represent.
        /// </summary>
        /// <param name="B">
        /// An instance of <see cref="IArgument{TData,TArgument}"/> which is to be tested against this instance.
        /// </param>
        /// <param name="notInB">
        ///  <para>
        ///   The enumeration of <see cref="IArgument{TData,TArgument}"/> which represents the portion of
        ///   this which is not overlapped by <paramref name="B"/>.
        ///  </para>
        ///  <para>
        ///   In the case where this is entirely covered by <paramref name="B"/> the <paramref name="notInB"/>
        ///   argument can be returned as null or as an empty enumeration.
        ///  </para>
        ///  <para>
        ///   The return value is ignored in the case where false is returned, as there is no overlap.
        ///  </para>
        /// </param>
        /// <returns></returns>
        /// <remarks>
        ///  If one considers the aruments which represent the following sets over the key space:
        ///  <code>
        ///  <![CDATA[
        ///            (B)
        ///   +----------------------+
        ///   |                      |      this
        ///   |              +-------|-------------+
        ///   |              |       |#############|
        ///   |              |       |#############|
        ///   |              |       |#############|
        ///   |              |       |#############|
        ///   +----------------------+#############|
        ///                  |#####################|
        ///                  |#####################|
        ///                  +---------------------+
        ///  ]]>
        ///  </code>
        ///  In the above case <see cref="IsOverlapped"/> should return true (since this and B share
        ///  some of the key-space, and <paramref name="notInB"/> should represent the hashed area.
        /// </remarks>
        bool IsOverlapped(
            TArgument B,
            out IEnumerable<TArgument> notInB);


        /// <summary>
        /// A filtering function which returns a flag indicating whether data is represented by this
        /// argument instance.
        /// </summary>
        /// <param name="data">The data to be tested.</param>
        /// <returns>true if the <paramref name="data"/> parameter is represented by this instance.</returns>
        bool IsDataIncluded(TData data);
    }
}

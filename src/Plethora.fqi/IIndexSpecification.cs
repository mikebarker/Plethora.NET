using System.Collections.Generic;
using System.Linq.Expressions;

namespace Plethora.fqi
{
    /// <summary>
    /// Index specifiaction for a single index.
    /// </summary>
    public interface IIndexSpecification
    {
        /// <summary>
        /// A list of expressions which retrieve the indexed properties from the underlying data.
        /// </summary>
        IEnumerable<LambdaExpression> IndexExpressions { get; }

        /// <summary>
        /// true if the index is unique across the properties given by <see cref="IndexExpressions"/>.
        /// </summary>
        bool IsUnique { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Plethora.ExpressionAide
{
    /// <summary>
    /// Class which allows expressions to be duplicated and promotes closures to parameters.
    /// </summary>
    /// <remarks>
    ///  <para>
    ///   This technique of re-writing expression trees allows the resulting expression tree to be
    ///   compiled and the compiled version to be cached and re-used. Compiling an expression tree
    ///   is an expensive operation and so this an optimisation technique.
    ///  </para>
    ///  <para>
    ///   A "closure" is a constant reference to captured variables in an expressions scope. e.g.
    ///  <example>
    ///    <code>
    /// <![CDATA[
    ///      static Expression<Func<DateTime, bool>> IsYearFunc(int year)
    ///      {
    ///          return dt => dt.Year == year;
    ///      }
    /// ]]>
    ///    </code>
    ///   In this example the parameter 'year' is internally captured within a separate class. This
    ///   inner class is termed a "closure".
    ///  </example>
    ///  </para>
    ///  <para>
    ///   If an expression tree is duplicated without promoting closures to paramaters the resulting
    ///   delegate will always reference the orginal closure regardless of the context in which it is
    ///   invoked.
    ///  </para>
    /// </remarks>
    internal class ExpressionDuplicatorWithClosurePromotion : ExpressionDuplicator
    {
        #region Constants

        internal const string CLOSURE_CONSTANT_NAME = "<>c__DisplayClass";
        #endregion

        #region PromoteClosures

        public LambdaExecutor PromoteClosures(LambdaExpression expr)
        {
            IEnumerable<KeyValuePair<ParameterExpression, Step[]>> parameters;
            var dupe = this.DuplicateWithClosurePromotion(expr, out parameters);

            return new LambdaExecutor(dupe, parameters);
        }

        public LambdaExecutor<TResult> PromoteClosures<TResult>(Expression<Func<TResult>> expr)
        {
            IEnumerable<KeyValuePair<ParameterExpression, Step[]>> parameters;
            var dupe = this.DuplicateWithClosurePromotion(expr, out parameters);

            return new LambdaExecutor<TResult>(dupe, parameters);
        }

        public LambdaExecutor<T, TResult> PromoteClosures<T, TResult>(Expression<Func<T, TResult>> expr)
        {
            IEnumerable<KeyValuePair<ParameterExpression, Step[]>> parameters;
            var dupe = this.DuplicateWithClosurePromotion(expr, out parameters);

            return new LambdaExecutor<T, TResult>(dupe, parameters);
        }

        public LambdaExecutor<T1, T2, TResult> PromoteClosures<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> expr)
        {
            IEnumerable<KeyValuePair<ParameterExpression, Step[]>> parameters;
            var dupe = this.DuplicateWithClosurePromotion(expr, out parameters);

            return new LambdaExecutor<T1, T2, TResult>(dupe, parameters);
        }

        public LambdaExecutor<T1, T2, T3, TResult> PromoteClosures<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> expr)
        {
            IEnumerable<KeyValuePair<ParameterExpression, Step[]>> parameters;
            var dupe = this.DuplicateWithClosurePromotion(expr, out parameters);

            return new LambdaExecutor<T1, T2, T3, TResult>(dupe, parameters);
        }

        public LambdaExecutor<T1, T2, T3, T4, TResult> PromoteClosures<T1, T2, T3, T4, TResult>(Expression<Func<T1, T2, T3, T4, TResult>> expr)
        {
            IEnumerable<KeyValuePair<ParameterExpression, Step[]>> parameters;
            var dupe = this.DuplicateWithClosurePromotion(expr, out parameters);

            return new LambdaExecutor<T1, T2, T3, T4, TResult>(dupe, parameters);
        }
        #endregion

        #region Public Methods

        public LambdaExpression DuplicateWithClosurePromotion(LambdaExpression expr, out IEnumerable<KeyValuePair<ParameterExpression, Step[]>> parameters)
        {
            ParamDictionary parametersDic = new ParamDictionary();
            List<Step> path = new List<Step>();
            var dupe = (LambdaExpression)this.Duplicate(expr, new Step(Direction.This), parametersDic, path);

            parameters = parametersDic;
            return dupe;
        }
        #endregion

        #region Overrides

        protected override Expression DuplicateConstant(ConstantExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            if ((expression != null) && expression.Type.Name.StartsWith(CLOSURE_CONSTANT_NAME))
            {
                ParameterExpression rtn = null;

                //Test if the constant has been encountered before
                string expressionName = expression.Type.Name;
                foreach (ParameterExpression key in parameters.Keys)
                {
                    if (key.Name == expressionName)
                    {
                        rtn = key;
                        break;
                    }
                }

                if (rtn == null)
                {
                    //The constant has not been encountered before. Duplicate it.
                    rtn = Expression.Parameter(expression.Type, expression.Type.Name);
                    parameters.TryAdd(rtn, path.ToArray());
                }

                return rtn;
            }

            return base.DuplicateConstant(expression, parameters, path);
        }
        #endregion
    }
}

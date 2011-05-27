using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Plethora.ExpressionAide;

namespace Plethora.fqi
{
    internal static class ExpressionAnalyser
    {
        #region Constants

        private const string ParameterPseudonym = "<param>_";
        #endregion

        #region Public Methods

        public static IDictionary<string, INamedLateRange> GetMemberRestrictions(LambdaExpression lambda)
        {
            if (lambda.Body is BinaryExpression)
            {
                string[] parameters = lambda.Parameters.Select(r => r.Name).ToArray();
                var binExpression = (BinaryExpression)lambda.Body;
                var restrictions = GetMemberRestrictions(binExpression, parameters);

                return restrictions ?? new Dictionary<string, INamedLateRange>();
            }
            else
            {
                return new Dictionary<string, INamedLateRange>();
            }
        }

        public static string GetMemberName(LambdaExpression expression)
        {
            if (expression.Body is MemberExpression)
            {
                return ((MemberExpression)expression.Body).Member.Name;
            }
            else if (expression.Body is ParameterExpression)
            {
                var parameterExpression = (ParameterExpression)expression.Body;

                int i;
                for (i = 0; i < expression.Parameters.Count; i++)
                {
                    var parameter = expression.Parameters[i];

                    if (parameterExpression.Name == parameter.Name)
                        break;
                }
                return ParameterPseudonym + i.ToString();
            }

            return null;
        }
        #endregion

        #region Private Methods

        private static IDictionary<string, INamedLateRange> GetMemberRestrictions(
            BinaryExpression binaryExpression,
            IEnumerable<string> parameters)
        {
            if ((binaryExpression.NodeType == ExpressionType.And) ||
                (binaryExpression.NodeType == ExpressionType.AndAlso))
            {
                var leftBinaryExpression = binaryExpression.Left as BinaryExpression;
                var rightBinaryExpression = binaryExpression.Right as BinaryExpression;

                IDictionary<string, INamedLateRange> leftRestrictions = null;
                IDictionary<string, INamedLateRange> rightRestrictions = null;
                if (leftBinaryExpression != null)
                    leftRestrictions = GetMemberRestrictions(leftBinaryExpression, parameters);

                if (rightBinaryExpression != null)
                    rightRestrictions = GetMemberRestrictions(rightBinaryExpression, parameters);

                return LateRangeHelper.CombineDictionariesAND(leftRestrictions, rightRestrictions);
            }
            if ((binaryExpression.NodeType == ExpressionType.Or) ||
                (binaryExpression.NodeType == ExpressionType.OrElse))
            {
                var leftBinaryExpression = binaryExpression.Left as BinaryExpression;
                var rightBinaryExpression = binaryExpression.Right as BinaryExpression;

                IDictionary<string, INamedLateRange> leftRestrictions = null;
                IDictionary<string, INamedLateRange> rightRestrictions = null;
                if (leftBinaryExpression != null)
                    leftRestrictions = GetMemberRestrictions(leftBinaryExpression, parameters);

                if (rightBinaryExpression != null)
                    rightRestrictions = GetMemberRestrictions(rightBinaryExpression, parameters);

                return LateRangeHelper.CombineDictionariesOR(leftRestrictions, rightRestrictions);
            }
            else
            {
                var memberRestriction = GetMemberRestriction(binaryExpression, parameters);
                if (memberRestriction != null)
                {
                    var restrictions = new Dictionary<string, INamedLateRange> { { memberRestriction.Name, memberRestriction } };

                    return restrictions;
                }
            }

            return null;
        }

        private static INamedLateRange GetMemberRestriction(
            BinaryExpression binaryExpression,
            IEnumerable<string> parameters)
        {
            if ((binaryExpression.NodeType == ExpressionType.GreaterThan) ||
                (binaryExpression.NodeType == ExpressionType.GreaterThanOrEqual) ||
                (binaryExpression.NodeType == ExpressionType.LessThan) ||
                (binaryExpression.NodeType == ExpressionType.LessThanOrEqual) ||
                (binaryExpression.NodeType == ExpressionType.Equal))
            {
                bool indexableReferenceLocated = false;
                string memberName = null;
                Func<object> func = null;

                if (binaryExpression.Left is ParameterExpression)
                {
                    indexableReferenceLocated = GetParameterRestrictionValue(
                        (ParameterExpression)binaryExpression.Left,
                        binaryExpression.Right,
                        parameters,
                        out memberName, out func);
                }
                else if (binaryExpression.Right is ParameterExpression)
                {
                    indexableReferenceLocated = GetParameterRestrictionValue(
                        (ParameterExpression)binaryExpression.Right,
                        binaryExpression.Left,
                        parameters,
                        out memberName, out func);
                }
                else if (binaryExpression.Left is MemberExpression)
                {
                    indexableReferenceLocated = GetMemberRestrictionValues(
                        (MemberExpression)binaryExpression.Left,
                        binaryExpression.Right,
                        parameters,
                        out memberName, out func);
                }
                else if (binaryExpression.Right is MemberExpression)
                {
                    indexableReferenceLocated = GetMemberRestrictionValues(
                        (MemberExpression)binaryExpression.Right,
                        binaryExpression.Left,
                        parameters,
                        out memberName, out func);
                }

                if (indexableReferenceLocated)
                {
                    INamedLateRange restriction = CreateNamedRange(memberName);
                    switch (binaryExpression.NodeType)
                    {
                        case ExpressionType.GreaterThan:
                            restriction.MinFunc = func;
                            return restriction;
                        case ExpressionType.GreaterThanOrEqual:
                            restriction.MinFunc = func;
                            return restriction;
                        case ExpressionType.LessThan:
                            restriction.MaxFunc = func;
                            return restriction;
                        case ExpressionType.LessThanOrEqual:
                            restriction.MaxFunc = func;
                            return restriction;
                        case ExpressionType.Equal:
                            restriction.MinFunc = func;
                            restriction.MaxFunc = func;
                            return restriction;
                    }
                }
            }

            return null;
        }

        private static bool GetParameterRestrictionValue(ParameterExpression parameterExpression, Expression comparand,
            IEnumerable<string> parameters,
            out string parameterName,
            out Func<object> func)
        {
            bool rtn = false;
            parameterName = null;
            func = (() => null);

            int count = 0;
            foreach(var parameter in parameters)
            {
                //Found a reference to an input parameter.
                if (parameter == parameterExpression.Name)
                {
                    parameterName = ParameterPseudonym + count.ToString();

                    //Attempt to evaluate the comparad
                    bool result = GetExpressionFunc(comparand, out func);
                    rtn = result;

                    break;
                }

                count++;
            }
            return rtn;
        }

        private static bool GetMemberRestrictionValues(MemberExpression memberExpression, Expression comparand,
            IEnumerable<string> parameters,
            out string memberName,
            out Func<object> func)
        {
            bool rtn = false;
            memberName = null;
            func = (() => null);

            if (memberExpression.Expression is ParameterExpression)
            {
                var parameterExp = (ParameterExpression)memberExpression.Expression;
                if (parameters.Contains(parameterExp.Name))
                {
                    //Found a reference to an input value's member.
                    var memberInfo = memberExpression.Member;
                    memberName = memberInfo.Name;

                    //Attempt to evaluate the Right side
                    bool result = GetExpressionFunc(comparand, out func);
                    rtn = result;
                }
            }

            return rtn;
        }

        private static bool GetExpressionFunc(Expression expression, out Func<object> func)
        {
            //Short cut the ConstantExpression type to avoid unnecessary dynamic invoke
            if (expression is ConstantExpression)
            {
                var constant = (ConstantExpression)expression;
                func = (() => constant.Value);
                return true;
            }
            else
            {
                try
                {
                    var lambda = Expression.Lambda(expression);
                    func = (() => lambda.Execute());
                    return true;
                }
                catch (Exception)
                {
                    func = (() => null);
                    return false;
                }
            }

        }

        private static INamedLateRange CreateNamedRange(string name)
        {
            return new NamedLateRange(name);
        }
        #endregion
    }
}

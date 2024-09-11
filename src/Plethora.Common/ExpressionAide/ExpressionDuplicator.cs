using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;

namespace Plethora.ExpressionAide
{
    internal class ExpressionDuplicator
    {
        #region Public Methods

        public T Duplicate<T>(T expr)
            where T : LambdaExpression
        {
            ArgumentNullException.ThrowIfNull(expr);


            ParamDictionary parameters = new();
            List<Step> path = new();
            return (T)this.Duplicate(expr, new(Direction.This), parameters, path)!;
        }
        #endregion

        #region Protected Methods

        protected Expression? Duplicate(Expression? expression, Step step, ParamDictionary parameters, IEnumerable<Step> path)
        {
            if (expression is null)
                return null;

            path = path.Concat(Enumerable.Repeat(step, 1));

            Expression rtn;
            switch (expression.NodeType)
            {
                case ExpressionType.Add:
                    rtn = this.DuplicateAdd((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.AddChecked:
                    rtn = this.DuplicateAddChecked((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.And:
                    rtn = this.DuplicateAnd((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.AndAlso:
                    rtn = this.DuplicateAndAlso((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.ArrayIndex:
                    { 
                        if (expression is BinaryExpression binaryExpression)
                        {
                            rtn = this.DuplicateArrayIndex(binaryExpression, parameters, path);
                        }
                        else if (expression is MethodCallExpression methodCallExpression)
                        {
                            rtn = this.DuplicateArrayIndex(methodCallExpression, parameters, path);
                        }
                        else
                        {
                            throw new ArgumentException($"Unknown ArrayIndex expression type {expression.GetType()}", nameof(expression));
                        }
                    }
                    break;
                case ExpressionType.ArrayLength:
                    rtn = this.DuplicateArrayLength((UnaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.Call:
                    rtn = this.DuplicateCall((MethodCallExpression)expression, parameters, path);
                    break;
                case ExpressionType.Coalesce:
                    rtn = this.DuplicateCoalesce((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.Conditional:
                    rtn = this.DuplicateConditional((ConditionalExpression)expression, parameters, path);
                    break;
                case ExpressionType.Constant:
                    rtn = this.DuplicateConstant((ConstantExpression)expression, parameters, path);
                    break;
                case ExpressionType.Convert:
                    rtn = this.DuplicateConvert((UnaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.ConvertChecked:
                    rtn = this.DuplicateConvertChecked((UnaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.Divide:
                    rtn = this.DuplicateDivide((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.Equal:
                    rtn = this.DuplicateEqual((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.ExclusiveOr:
                    rtn = this.DuplicateExclusiveOr((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.GreaterThan:
                    rtn = this.DuplicateGreaterThan((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    rtn = this.DuplicateGreaterThanOrEqual((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.Invoke:
                    rtn = this.DuplicateInvoke((InvocationExpression)expression, parameters, path);
                    break;
                case ExpressionType.Lambda:
                    rtn = this.DuplicateLambda((LambdaExpression)expression, parameters, path);
                    break;
                case ExpressionType.LeftShift:
                    rtn = this.DuplicateLeftShift((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.LessThan:
                    rtn = this.DuplicateLessThan((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.LessThanOrEqual:
                    rtn = this.DuplicateLessThanOrEqual((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.ListInit:
                    rtn = this.DuplicateListInit((ListInitExpression)expression, parameters, path);
                    break;
                case ExpressionType.MemberAccess:
                    rtn = this.DuplicateMemberAccess((MemberExpression)expression, parameters, path);
                    break;
                case ExpressionType.MemberInit:
                    rtn = this.DuplicateMemberInit((MemberInitExpression)expression, parameters, path);
                    break;
                case ExpressionType.Modulo:
                    rtn = this.DuplicateModulo((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.Multiply:
                    rtn = this.DuplicateMultiply((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.MultiplyChecked:
                    rtn = this.DuplicateMultiplyChecked((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.Negate:
                    rtn = this.DuplicateNegate((UnaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.NegateChecked:
                    rtn = this.DuplicateNegateChecked((UnaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.New:
                    rtn = this.DuplicateNew((NewExpression)expression, parameters, path);
                    break;
                case ExpressionType.NewArrayBounds:
                    rtn = this.DuplicateNewArrayBounds((NewArrayExpression)expression, parameters, path);
                    break;
                case ExpressionType.NewArrayInit:
                    rtn = this.DuplicateNewArrayInit((NewArrayExpression)expression, parameters, path);
                    break;
                case ExpressionType.Not:
                    rtn = this.DuplicateNot((UnaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.NotEqual:
                    rtn = this.DuplicateNotEqual((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.Or:
                    rtn = this.DuplicateOr((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.OrElse:
                    rtn = this.DuplicateOrElse((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.Parameter:
                    rtn = this.DuplicateParameter((ParameterExpression)expression, parameters, path);
                    break;
                case ExpressionType.Power:
                    rtn = this.DuplicatePower((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.Quote:
                    rtn = this.DuplicateQuote((UnaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.RightShift:
                    rtn = this.DuplicateRightShift((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.Subtract:
                    rtn = this.DuplicateSubtract((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.SubtractChecked:
                    rtn = this.DuplicateSubtractChecked((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.TypeAs:
                    rtn = this.DuplicateTypeAs((UnaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.TypeIs:
                    rtn = this.DuplicateTypeIs((TypeBinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.UnaryPlus:
                    rtn = this.DuplicateUnaryPlus((UnaryExpression)expression, parameters, path);
                    break;

                default:
                    throw new ArgumentException(
                        $"Expression NodeType {expression.NodeType} not known.",
                        nameof(expression));
            }

            return rtn;
        }

        private IEnumerable<Expression?> DuplicateChild(Expression parent, Direction direction, ParamDictionary parameters, IEnumerable<Step> path)
        {
            //Single return expressions
            Expression? expr = null;
            bool assigned = false;
            switch (direction)
            {
                case Direction.This:
                    expr = parent;
                    assigned = true;
                    break;
                case Direction.Left:
                    expr = ((BinaryExpression)parent).Left;
                    assigned = true;
                    break;
                case Direction.Right:
                    expr = ((BinaryExpression)parent).Right;
                    assigned = true;
                    break;
                case Direction.Operand:
                    expr = ((UnaryExpression)parent).Operand;
                    assigned = true;
                    break;
                case Direction.Object:
                    expr = ((MethodCallExpression)parent).Object;
                    assigned = true;
                    break;
                case Direction.Body:
                    expr = ((LambdaExpression)parent).Body;
                    assigned = true;
                    break;
                case Direction.Test:
                    expr = ((ConditionalExpression)parent).Test;
                    assigned = true;
                    break;
                case Direction.IfTrue:
                    expr = ((ConditionalExpression)parent).IfTrue;
                    assigned = true;
                    break;
                case Direction.IfFalse:
                    expr = ((ConditionalExpression)parent).IfFalse;
                    assigned = true;
                    break;
                case Direction.Expression:
                    {
                        if (parent is InvocationExpression invocationExpression)
                        {
                            expr = invocationExpression.Expression;
                            assigned = true;
                        }
                        else if (parent is TypeBinaryExpression typeBinaryExpression)
                        {
                            expr = typeBinaryExpression.Expression;
                            assigned = true;
                        }
                        else if (parent is MemberExpression memberExpression)
                        {
                            expr = memberExpression.Expression;
                            assigned = true;
                        }
                    }
                    break;
                case Direction.NewExpression:
                    {
                        if (parent is ListInitExpression listInitExpression)
                        {
                            expr = listInitExpression.NewExpression;
                            assigned = true;
                        }
                        else if (parent is MemberInitExpression memberInitExpression)
                        {
                            expr = memberInitExpression.NewExpression;
                            assigned = true;
                        }
                    }
                    break;
            }
            if (assigned)
            {
                var dupe = this.Duplicate(expr, new(direction), parameters, path);
                return Enumerable.Repeat(dupe, 1);
            }

            //Multi return expressions
            IList<Expression>? exprs = null;
            switch (direction)
            {
                case Direction.Arguments:
                    {
                        if (parent is MethodCallExpression methodCallExpression)
                        {
                            exprs = methodCallExpression.Arguments;
                            assigned = true;
                        }
                        else if (parent is NewExpression newExpression)
                        {
                            exprs = newExpression.Arguments;
                            assigned = true;
                        }
                        else if (parent is InvocationExpression invocationExpression)
                        {
                            exprs = invocationExpression.Arguments;
                            assigned = true;
                        }
                    }
                    break;
                case Direction.Expressions:
                    {
                        if (parent is NewArrayExpression newArrayExpression)
                        {
                            exprs = newArrayExpression.Expressions;
                            assigned = true;
                        }
                    }
                    break;
            }
            if (assigned)
            {
                Debug.Assert(exprs != null);

                List<Expression?> dupes = new();
                int count = exprs.Count;
                for (int i = 0; i < count; i++)
                {
                    expr = exprs[i];

                    var dupe = this.Duplicate(expr, new(direction, i), parameters, path);

                    dupes.Add(dupe);
                }
                return dupes;
            }

            throw new ArgumentException($"Direction {direction} not recognised for Expression type {parent.GetType().Name}");
        }

        #region Duplicate Methods

        protected virtual Expression DuplicateAdd(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = this.DuplicateChild(expression, Direction.Left, parameters, path).Single();
            Debug.Assert(expLeft is not null);

            var expRight = this.DuplicateChild(expression, Direction.Right, parameters, path).Single();
            Debug.Assert(expRight is not null);

            var rtn = Expression.Add(expLeft, expRight, expression.Method);

            return rtn;
        }

        protected virtual Expression DuplicateAddChecked(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = this.DuplicateChild(expression, Direction.Left, parameters, path).Single();
            Debug.Assert(expLeft is not null);

            var expRight = this.DuplicateChild(expression, Direction.Right, parameters, path).Single();
            Debug.Assert(expRight is not null);

            var rtn = Expression.AddChecked(expLeft, expRight, expression.Method);
            
            return rtn;
        }

        protected virtual Expression DuplicateAnd(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = this.DuplicateChild(expression, Direction.Left, parameters, path).Single();
            Debug.Assert(expLeft is not null);

            var expRight = this.DuplicateChild(expression, Direction.Right, parameters, path).Single();
            Debug.Assert(expRight is not null);

            var rtn = Expression.And(expLeft, expRight, expression.Method);
            
            return rtn;
        }

        protected virtual Expression DuplicateAndAlso(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = this.DuplicateChild(expression, Direction.Left, parameters, path).Single();
            Debug.Assert(expLeft is not null);

            var expRight = this.DuplicateChild(expression, Direction.Right, parameters, path).Single();
            Debug.Assert(expRight is not null);

            var rtn = Expression.AndAlso(expLeft, expRight, expression.Method);
            
            return rtn;
        }

        protected virtual Expression DuplicateArrayIndex(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = this.DuplicateChild(expression, Direction.Left, parameters, path).Single();
            Debug.Assert(expLeft is not null);

            var expRight = this.DuplicateChild(expression, Direction.Right, parameters, path).Single();
            Debug.Assert(expRight is not null);

            var rtn = Expression.ArrayIndex(expLeft, expRight);
            
            return rtn;
        }

        protected virtual Expression DuplicateArrayIndex(MethodCallExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = this.DuplicateChild(expression, Direction.Object, parameters, path).Single();
            Debug.Assert(expLeft is not null);

            var expArguments = this.DuplicateChild(expression, Direction.Arguments, parameters, path);
            Debug.Assert(expArguments.All(exp => exp is not null));

            var rtn = Expression.ArrayIndex(expLeft, (IEnumerable<Expression>)expArguments);
            
            return rtn;
        }

        protected virtual Expression DuplicateArrayLength(UnaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expOperand = this.DuplicateChild(expression, Direction.Operand, parameters, path).Single();
            Debug.Assert(expOperand is not null);

            var rtn = Expression.ArrayLength(expOperand);
            
            return rtn;
        }

        protected virtual Expression DuplicateCall(MethodCallExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expObject = this.DuplicateChild(expression, Direction.Object, parameters, path).Single();

            var expArguments = this.DuplicateChild(expression, Direction.Arguments, parameters, path);
            Debug.Assert(expArguments.All(exp => exp is not null));

            var rtn = Expression.Call(expObject, expression.Method, (IEnumerable<Expression>)expArguments);
            
            return rtn;
        }

        protected virtual Expression DuplicateCoalesce(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = this.DuplicateChild(expression, Direction.Left, parameters, path).Single();
            Debug.Assert(expLeft is not null);

            var expRight = this.DuplicateChild(expression, Direction.Right, parameters, path).Single();
            Debug.Assert(expRight is not null);

            var rtn = Expression.Coalesce(expLeft, expRight, expression.Conversion);

            return rtn;
        }

        protected virtual Expression DuplicateConditional(ConditionalExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expTest = this.DuplicateChild(expression, Direction.Test, parameters, path).Single();
            Debug.Assert(expTest is not null);
            var expIfTrue = this.DuplicateChild(expression, Direction.IfTrue, parameters, path).Single();

            Debug.Assert(expIfTrue is not null);

            var expIfFalse = this.DuplicateChild(expression, Direction.IfFalse, parameters, path).Single();
            Debug.Assert(expIfFalse is not null);

            var rtn = Expression.Condition(expTest, expIfTrue, expIfFalse);

            return rtn;
        }

        protected virtual Expression DuplicateConstant(ConstantExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var rtn = Expression.Constant(expression.Value, expression.Type);

            return rtn;
        }

        protected virtual Expression DuplicateConvert(UnaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expOperand = this.DuplicateChild(expression, Direction.Operand, parameters, path).Single();
            Debug.Assert(expOperand is not null);

            var rtn = Expression.Convert(expOperand, expression.Type, expression.Method);

            return rtn;
        }

        protected virtual Expression DuplicateConvertChecked(UnaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expOperand = this.DuplicateChild(expression, Direction.Operand, parameters, path).Single();
            Debug.Assert(expOperand is not null);

            var rtn = Expression.ConvertChecked(expOperand, expression.Type, expression.Method);

            return rtn;
        }

        protected virtual Expression DuplicateDivide(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = this.DuplicateChild(expression, Direction.Left, parameters, path).Single();
            Debug.Assert(expLeft is not null);

            var expRight = this.DuplicateChild(expression, Direction.Right, parameters, path).Single();
            Debug.Assert(expRight is not null);

            var rtn = Expression.Divide(expLeft, expRight, expression.Method);
            
            return rtn;
        }

        protected virtual Expression DuplicateEqual(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = this.DuplicateChild(expression, Direction.Left, parameters, path).Single();
            Debug.Assert(expLeft is not null);

            var expRight = this.DuplicateChild(expression, Direction.Right, parameters, path).Single();
            Debug.Assert(expRight is not null);

            var rtn = Expression.Equal(expLeft, expRight, expression.IsLiftedToNull, expression.Method);
            
            return rtn;
        }

        protected virtual Expression DuplicateExclusiveOr(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = this.DuplicateChild(expression, Direction.Left, parameters, path).Single();
            Debug.Assert(expLeft is not null);

            var expRight = this.DuplicateChild(expression, Direction.Right, parameters, path).Single();
            Debug.Assert(expRight is not null);

            var rtn = Expression.ExclusiveOr(expLeft, expRight, expression.Method);
            
            return rtn;
        }

        protected virtual Expression DuplicateGreaterThan(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = this.DuplicateChild(expression, Direction.Left, parameters, path).Single();
            Debug.Assert(expLeft is not null);

            var expRight = this.DuplicateChild(expression, Direction.Right, parameters, path).Single();
            Debug.Assert(expRight is not null);

            var rtn = Expression.GreaterThan(expLeft, expRight, expression.IsLiftedToNull, expression.Method);
            
            return rtn;
        }

        protected virtual Expression DuplicateGreaterThanOrEqual(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = this.DuplicateChild(expression, Direction.Left, parameters, path).Single();
            Debug.Assert(expLeft is not null);

            var expRight = this.DuplicateChild(expression, Direction.Right, parameters, path).Single();
            Debug.Assert(expRight is not null);

            var rtn = Expression.GreaterThanOrEqual(expLeft, expRight, expression.IsLiftedToNull, expression.Method);
            
            return rtn;
        }

        protected virtual Expression DuplicateInvoke(InvocationExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expExpression = this.DuplicateChild(expression, Direction.Expression, parameters, path).Single();
            Debug.Assert(expExpression is not null);

            var expArguments = this.DuplicateChild(expression, Direction.Arguments, parameters, path);
            Debug.Assert(expArguments.All(exp => exp is not null));

            var rtn = Expression.Invoke(expExpression, (IEnumerable<Expression>)expArguments);
            
            return rtn;
        }

        protected virtual Expression DuplicateLambda(LambdaExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expBody = this.DuplicateChild(expression, Direction.Body, parameters, path).Single();
            Debug.Assert(expBody is not null);

            IEnumerable<Step> pathToThis = Enumerable.Repeat(new Step(Direction.This), 1);
            foreach (var parameter in expression.Parameters)
            {
                parameters.TryAdd(parameter, pathToThis.ToArray());
            }

            var lambdaParameters = parameters.SortBy(expression.Parameters).Select(p => p.Key).ToArray();
            var rtn = Expression.Lambda(expBody, lambdaParameters);

            return rtn;
        }

        protected virtual BinaryExpression DuplicateLeftShift(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = this.DuplicateChild(expression, Direction.Left, parameters, path).Single();
            Debug.Assert(expLeft is not null);

            var expRight = this.DuplicateChild(expression, Direction.Right, parameters, path).Single();
            Debug.Assert(expRight is not null);

            var rtn = Expression.LeftShift(expLeft, expRight, expression.Method);

            return rtn;
        }

        protected virtual BinaryExpression DuplicateLessThan(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = this.DuplicateChild(expression, Direction.Left, parameters, path).Single();
            Debug.Assert(expLeft is not null);

            var expRight = this.DuplicateChild(expression, Direction.Right, parameters, path).Single();
            Debug.Assert(expRight is not null);

            var rtn = Expression.LessThan(expLeft, expRight, expression.IsLiftedToNull, expression.Method);

            return rtn;
        }

        protected virtual BinaryExpression DuplicateLessThanOrEqual(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = this.DuplicateChild(expression, Direction.Left, parameters, path).Single();
            Debug.Assert(expLeft is not null);

            var expRight = this.DuplicateChild(expression, Direction.Right, parameters, path).Single();
            Debug.Assert(expRight is not null);

            var rtn = Expression.LessThanOrEqual(expLeft, expRight, expression.IsLiftedToNull, expression.Method);
            
            return rtn;
        }

        protected virtual ListInitExpression DuplicateListInit(ListInitExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expNewExpression = (NewExpression?)this.DuplicateChild(expression, Direction.NewExpression, parameters, path).Single();
            Debug.Assert(expNewExpression is not null);

            var rtn = Expression.ListInit(expNewExpression, expression.Initializers);
            
            return rtn;
        }

        protected virtual MemberExpression DuplicateMemberAccess(MemberExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expExpression = this.DuplicateChild(expression, Direction.Expression, parameters, path).Single();
            var rtn = Expression.MakeMemberAccess(expExpression, expression.Member);
            
            return rtn;
        }

        protected virtual MemberInitExpression DuplicateMemberInit(MemberInitExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expNewExpression = (NewExpression?)this.DuplicateChild(expression, Direction.NewExpression, parameters, path).Single();
            Debug.Assert(expNewExpression is not null);

            var rtn = Expression.MemberInit(expNewExpression, expression.Bindings);
            
            return rtn;
        }

        protected virtual BinaryExpression DuplicateModulo(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = this.DuplicateChild(expression, Direction.Left, parameters, path).Single();
            Debug.Assert(expLeft is not null);

            var expRight = this.DuplicateChild(expression, Direction.Right, parameters, path).Single();
            Debug.Assert(expRight is not null);

            var rtn = Expression.Modulo(expLeft, expRight, expression.Method);
            
            return rtn;
        }

        protected virtual BinaryExpression DuplicateMultiply(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = this.DuplicateChild(expression, Direction.Left, parameters, path).Single();
            Debug.Assert(expLeft is not null);

            var expRight = this.DuplicateChild(expression, Direction.Right, parameters, path).Single();
            Debug.Assert(expRight is not null);

            var rtn = Expression.Multiply(expLeft, expRight, expression.Method);
            
            return rtn;
        }

        protected virtual BinaryExpression DuplicateMultiplyChecked(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = this.DuplicateChild(expression, Direction.Left, parameters, path).Single();
            Debug.Assert(expLeft is not null);

            var expRight = this.DuplicateChild(expression, Direction.Right, parameters, path).Single();
            Debug.Assert(expRight is not null);

            var rtn = Expression.MultiplyChecked(expLeft, expRight, expression.Method);
            
            return rtn;
        }

        protected virtual UnaryExpression DuplicateNegate(UnaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expOperand = this.DuplicateChild(expression, Direction.Operand, parameters, path).Single();
            Debug.Assert(expOperand is not null);

            var rtn = Expression.Negate(expOperand, expression.Method);
            
            return rtn;
        }

        protected virtual UnaryExpression DuplicateNegateChecked(UnaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expOperand = this.DuplicateChild(expression, Direction.Operand, parameters, path).Single();
            Debug.Assert(expOperand is not null);

            var rtn = Expression.NegateChecked(expOperand, expression.Method);
            
            return rtn;
        }

        protected virtual NewExpression DuplicateNew(NewExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expArguments = this.DuplicateChild(expression, Direction.Arguments, parameters, path);
            Debug.Assert(expArguments.All(exp => exp is not null));

            var rtn = Expression.New(expression.Constructor!, (IEnumerable<Expression>)expArguments, expression.Members);
            
            return rtn;
        }

        protected virtual NewArrayExpression DuplicateNewArrayBounds(NewArrayExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expExpressions = this.DuplicateChild(expression, Direction.Expressions, parameters, path);
            Debug.Assert(expExpressions.All(exp => exp is not null));

            var rtn = Expression.NewArrayBounds(expression.Type.GetElementType()!, (IEnumerable<Expression>)expExpressions);
            
            return rtn;
        }

        protected virtual NewArrayExpression DuplicateNewArrayInit(NewArrayExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expExpressions = this.DuplicateChild(expression, Direction.Expressions, parameters, path);
            Debug.Assert(expExpressions.All(exp => exp is not null));

            var rtn = Expression.NewArrayInit(expression.Type.GetElementType()!, (IEnumerable<Expression>)expExpressions);
            
            return rtn;
        }

        protected virtual UnaryExpression DuplicateNot(UnaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expOperand = this.DuplicateChild(expression, Direction.Operand, parameters, path).Single();
            Debug.Assert(expOperand is not null);

            var rtn = Expression.Not(expOperand, expression.Method);
            
            return rtn;
        }

        protected virtual BinaryExpression DuplicateNotEqual(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = this.DuplicateChild(expression, Direction.Left, parameters, path).Single();
            Debug.Assert(expLeft is not null);

            var expRight = this.DuplicateChild(expression, Direction.Right, parameters, path).Single();
            Debug.Assert(expRight is not null);

            var rtn = Expression.NotEqual(expLeft, expRight, expression.IsLiftedToNull, expression.Method);
            
            return rtn;
        }

        protected virtual BinaryExpression DuplicateOr(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = this.DuplicateChild(expression, Direction.Left, parameters, path).Single();
            Debug.Assert(expLeft is not null);

            var expRight = this.DuplicateChild(expression, Direction.Right, parameters, path).Single();
            Debug.Assert(expRight is not null);

            var rtn = Expression.Or(expLeft, expRight, expression.Method);
            
            return rtn;
        }

        protected virtual BinaryExpression DuplicateOrElse(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = this.DuplicateChild(expression, Direction.Left, parameters, path).Single();
            Debug.Assert(expLeft is not null);

            var expRight = this.DuplicateChild(expression, Direction.Right, parameters, path).Single();
            Debug.Assert(expRight is not null);

            var rtn = Expression.OrElse(expLeft, expRight, expression.Method);
            
            return rtn;
        }

        protected virtual ParameterExpression DuplicateParameter(ParameterExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            ParameterExpression? rtn = null;

            //Test if the constant has been encountered before
            string expressionName = expression.Name!;
            foreach (ParameterExpression key in parameters.Keys)
            {
                if (string.Equals(key.Name, expressionName, StringComparison.Ordinal))
                {
                    rtn = key;
                    break;
                }
            }

            if (rtn is null)
            {
                //The constant has not been encountered before. Duplicate it.
                rtn = Expression.Parameter(expression.Type, expression.Name);
                parameters.TryAdd(rtn, path.ToArray());
            }

            return rtn;
        }

        protected virtual BinaryExpression DuplicatePower(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = this.DuplicateChild(expression, Direction.Left, parameters, path).Single();
            Debug.Assert(expLeft is not null);

            var expRight = this.DuplicateChild(expression, Direction.Right, parameters, path).Single();
            Debug.Assert(expRight is not null);

            var rtn = Expression.Power(expLeft, expRight, expression.Method);
            
            return rtn;
        }

        protected virtual UnaryExpression DuplicateQuote(UnaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expOperand = this.DuplicateChild(expression, Direction.Operand, parameters, path).Single();
            Debug.Assert(expOperand is not null);

            var rtn = Expression.Quote(expOperand);
            
            return rtn;
        }

        protected virtual BinaryExpression DuplicateRightShift(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = this.DuplicateChild(expression, Direction.Left, parameters, path).Single();
            Debug.Assert(expLeft is not null);

            var expRight = this.DuplicateChild(expression, Direction.Right, parameters, path).Single();
            Debug.Assert(expRight is not null);

            var rtn = Expression.RightShift(expLeft, expRight, expression.Method);
            
            return rtn;
        }

        protected virtual BinaryExpression DuplicateSubtract(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = this.DuplicateChild(expression, Direction.Left, parameters, path).Single();
            Debug.Assert(expLeft is not null);

            var expRight = this.DuplicateChild(expression, Direction.Right, parameters, path).Single();
            Debug.Assert(expRight is not null);

            var rtn = Expression.Subtract(expLeft, expRight, expression.Method);
            
            return rtn;
        }

        protected virtual BinaryExpression DuplicateSubtractChecked(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = this.DuplicateChild(expression, Direction.Left, parameters, path).Single();
            Debug.Assert(expLeft is not null);

            var expRight = this.DuplicateChild(expression, Direction.Right, parameters, path).Single();
            Debug.Assert(expRight is not null);

            var rtn = Expression.SubtractChecked(expLeft, expRight, expression.Method);
            
            return rtn;
        }

        protected virtual UnaryExpression DuplicateTypeAs(UnaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expOperand = this.DuplicateChild(expression, Direction.Operand, parameters, path).Single();
            Debug.Assert(expOperand is not null);

            var rtn = Expression.TypeAs(expOperand, expression.Type);
            
            return rtn;
        }

        protected virtual TypeBinaryExpression DuplicateTypeIs(TypeBinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expExpression = this.DuplicateChild(expression, Direction.Expression, parameters, path).Single();
            Debug.Assert(expExpression is not null);

            var rtn = Expression.TypeIs(expExpression, expression.TypeOperand);
            
            return rtn;
        }

        protected virtual UnaryExpression DuplicateUnaryPlus(UnaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expOperand = this.DuplicateChild(expression, Direction.Operand, parameters, path).Single();
            Debug.Assert(expOperand is not null);

            var rtn = Expression.UnaryPlus(expOperand, expression.Method);
            
            return rtn;
        }
        #endregion
        #endregion

        protected class ParamDictionary : IDictionary<ParameterExpression, Step[]>
        {
            #region Fields

            readonly Dictionary<string, KeyValuePair<ParameterExpression, Step[]>> innerDictionary = new();
            #endregion

            #region Implementation of IEnumerable

            public IEnumerator<KeyValuePair<ParameterExpression, Step[]>> GetEnumerator()
            {
                return this.innerDictionary.Values.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
            #endregion

            #region Implementation of ICollection<KeyValuePair<ParameterExpression, Step[]>>

            void ICollection<KeyValuePair<ParameterExpression, Step[]>>.Add(KeyValuePair<ParameterExpression, Step[]> item)
            {
                this.innerDictionary.Add(item.Key.Name!, item);
            }

            void ICollection<KeyValuePair<ParameterExpression, Step[]>>.Clear()
            {
                this.innerDictionary.Clear();
            }

            bool ICollection<KeyValuePair<ParameterExpression, Step[]>>.Contains(KeyValuePair<ParameterExpression, Step[]> item)
            {
                return this.ContainsKey(item.Key);
            }

            void ICollection<KeyValuePair<ParameterExpression, Step[]>>.CopyTo(KeyValuePair<ParameterExpression, Step[]>[] array, int arrayIndex)
            {
                this.innerDictionary.Values.CopyTo(array, arrayIndex);
            }

            bool ICollection<KeyValuePair<ParameterExpression, Step[]>>.Remove(KeyValuePair<ParameterExpression, Step[]> item)
            {
                return this.Remove(item.Key);
            }

            public int Count
            {
                get { return this.innerDictionary.Count; }
            }

            bool ICollection<KeyValuePair<ParameterExpression, Step[]>>.IsReadOnly
            {
                get
                {
                    return ((ICollection<KeyValuePair<string, KeyValuePair<ParameterExpression, Step[]>>>)this.innerDictionary).IsReadOnly;
                }
            }
            #endregion

            #region Implementation of IDictionary<ParameterExpression, Step[]>

            public bool ContainsKey(ParameterExpression key)
            {
                return this.innerDictionary.ContainsKey(key.Name!);
            }

            public void Add(ParameterExpression key, Step[] value)
            {
                KeyValuePair<ParameterExpression, Step[]> pair = new(key, value);
                this.innerDictionary.Add(key.Name!, pair);
            }

            public bool TryAdd(ParameterExpression key, Step[] value)
            {
                if (this.innerDictionary.ContainsKey(key.Name!))
                    return false;

                KeyValuePair<ParameterExpression, Step[]> pair = new(key, value);
                this.innerDictionary.Add(key.Name!, pair);

                return true;
            }

            public bool Remove(ParameterExpression key)
            {
                return this.innerDictionary.Remove(key.Name!);
            }

            public bool TryGetValue(ParameterExpression key, [MaybeNullWhen(false)] out Step[] value)
            {
                if (this.innerDictionary.TryGetValue(key.Name!, out var pair))
                {
                    value = pair.Value;
                    return true;
                }
                else
                {
                    value = null;
                    return false;
                }
            }

            public Step[] this[ParameterExpression key]
            {
                get { return this.innerDictionary[key.Name!].Value; }
                set
                {
                    KeyValuePair<ParameterExpression, Step[]> pair = new(key, value);
                    this.innerDictionary[key.Name!] = pair;
                }
            }

            public ICollection<ParameterExpression> Keys
            {
                get { return this.innerDictionary.Values.Select(r => r.Key).ToList(); }
            }

            public ICollection<Step[]> Values
            {
                get { return this.innerDictionary.Values.Select(r => r.Value).ToList(); }
            }
            #endregion

            #region Public Methods

            public KeyValuePair<ParameterExpression, Step[]> GetPair(ParameterExpression expression)
            {
                return this.innerDictionary[expression.Name!];
            }

            public void Combine(ParamDictionary dictionary)
            {
                foreach (var pair in dictionary)
                {
                    ParameterExpression exp = pair.Key;
                    Step[] path = pair.Value;
                    if (!this.ContainsKey(exp))
                    {
                        this.Add(exp, path);
                    }
                }
            }

            public IEnumerable<KeyValuePair<ParameterExpression, Step[]>> SortBy(
                IEnumerable<ParameterExpression> expressions)
            {
                List<KeyValuePair<ParameterExpression, Step[]>> rtn = new();
                foreach (var expression in expressions)
                {
                    var pair = this.GetPair(expression);
                    rtn.Add(new(pair.Key, pair.Value));
                }

                //Add remaining elements
                foreach (var pair in this)
                {
                    if (!ContainsKey(rtn, pair.Key))
                        rtn.Add(new(pair.Key, pair.Value));
                }

                return rtn;
            }
            #endregion

            #region Private Methods

            private static bool ContainsKey(IEnumerable<KeyValuePair<ParameterExpression, Step[]>> list, ParameterExpression key)
            {
                foreach (var pair in list)
                {
                    if (pair.Key.Name == key.Name)
                        return true;
                }
                return false;
            }
            #endregion
        }

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
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
            ParamDictionary parameters = new ParamDictionary();
            List<Step> path = new List<Step>();
            return (T)Duplicate(expr, new Step(Direction.This), parameters, path);
        }
        #endregion

        #region Protected Methods

        protected Expression Duplicate(Expression expression, Step step, ParamDictionary parameters, IEnumerable<Step> path)
        {
            if (expression == null)
                return null;

            path = path.Concat(Enumerable.Repeat(step, 1));

            Expression rtn;
            switch (expression.NodeType)
            {
                case ExpressionType.Add:
                    rtn = DuplicateAdd((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.AddChecked:
                    rtn = DuplicateAddChecked((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.And:
                    rtn = DuplicateAnd((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.AndAlso:
                    rtn = DuplicateAndAlso((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.ArrayIndex:
                    if (expression is BinaryExpression)
                    {
                        rtn = DuplicateArrayIndex((BinaryExpression)expression, parameters, path);
                    }
                    else if (expression is MethodCallExpression)
                    {
                        rtn = DuplicateArrayIndex((MethodCallExpression)expression, parameters, path);
                    }
                    else
                    {
                        throw new ArgumentException(string.Format("Unknown ArrayIndex expression type {0}", expression.GetType()), "expression");
                    }
                    break;
                case ExpressionType.ArrayLength:
                    rtn = DuplicateArrayLength((UnaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.Call:
                    rtn = DuplicateCall((MethodCallExpression)expression, parameters, path);
                    break;
                case ExpressionType.Coalesce:
                    rtn = DuplicateCoalesce((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.Conditional:
                    rtn = DuplicateConditional((ConditionalExpression)expression, parameters, path);
                    break;
                case ExpressionType.Constant:
                    rtn = DuplicateConstant((ConstantExpression)expression, parameters, path);
                    break;
                case ExpressionType.Convert:
                    rtn = DuplicateConvert((UnaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.ConvertChecked:
                    rtn = DuplicateConvertChecked((UnaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.Divide:
                    rtn = DuplicateDivide((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.Equal:
                    rtn = DuplicateEqual((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.ExclusiveOr:
                    rtn = DuplicateExclusiveOr((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.GreaterThan:
                    rtn = DuplicateGreaterThan((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    rtn = DuplicateGreaterThanOrEqual((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.Invoke:
                    rtn = DuplicateInvoke((InvocationExpression)expression, parameters, path);
                    break;
                case ExpressionType.Lambda:
                    rtn = DuplicateLambda((LambdaExpression)expression, parameters, path);
                    break;
                case ExpressionType.LeftShift:
                    rtn = DuplicateLeftShift((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.LessThan:
                    rtn = DuplicateLessThan((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.LessThanOrEqual:
                    rtn = DuplicateLessThanOrEqual((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.ListInit:
                    rtn = DuplicateListInit((ListInitExpression)expression, parameters, path);
                    break;
                case ExpressionType.MemberAccess:
                    rtn = DuplicateMemberAccess((MemberExpression)expression, parameters, path);
                    break;
                case ExpressionType.MemberInit:
                    rtn = DuplicateMemberInit((MemberInitExpression)expression, parameters, path);
                    break;
                case ExpressionType.Modulo:
                    rtn = DuplicateModulo((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.Multiply:
                    rtn = DuplicateMultiply((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.MultiplyChecked:
                    rtn = DuplicateMultiplyChecked((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.Negate:
                    rtn = DuplicateNegate((UnaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.NegateChecked:
                    rtn = DuplicateNegateChecked((UnaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.New:
                    rtn = DuplicateNew((NewExpression)expression, parameters, path);
                    break;
                case ExpressionType.NewArrayBounds:
                    rtn = DuplicateNewArrayBounds((NewArrayExpression)expression, parameters, path);
                    break;
                case ExpressionType.NewArrayInit:
                    rtn = DuplicateNewArrayInit((NewArrayExpression)expression, parameters, path);
                    break;
                case ExpressionType.Not:
                    rtn = DuplicateNot((UnaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.NotEqual:
                    rtn = DuplicateNotEqual((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.Or:
                    rtn = DuplicateOr((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.OrElse:
                    rtn = DuplicateOrElse((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.Parameter:
                    rtn = DuplicateParameter((ParameterExpression)expression, parameters, path);
                    break;
                case ExpressionType.Power:
                    rtn = DuplicatePower((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.Quote:
                    rtn = DuplicateQuote((UnaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.RightShift:
                    rtn = DuplicateRightShift((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.Subtract:
                    rtn = DuplicateSubtract((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.SubtractChecked:
                    rtn = DuplicateSubtractChecked((BinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.TypeAs:
                    rtn = DuplicateTypeAs((UnaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.TypeIs:
                    rtn = DuplicateTypeIs((TypeBinaryExpression)expression, parameters, path);
                    break;
                case ExpressionType.UnaryPlus:
                    rtn = DuplicateUnaryPlus((UnaryExpression)expression, parameters, path);
                    break;

                default:
                    throw new ArgumentException(
                        string.Format("Expression NodeType {0} not known. ", expression.NodeType),
                        "expression");
            }

            return rtn;
        }

        private IEnumerable<Expression> DuplicateChild(Expression parent, Direction direction, ParamDictionary parameters, IEnumerable<Step> path)
        {
            //Single return expressions
            Expression expr = null;
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
                    if (parent is InvocationExpression)
                    {
                        expr = ((InvocationExpression)parent).Expression;
                        assigned = true;
                    }
                    else if (parent is TypeBinaryExpression)
                    {
                        expr = ((TypeBinaryExpression)parent).Expression;
                        assigned = true;
                    }
                    else if (parent is MemberExpression)
                    {
                        expr = ((MemberExpression)parent).Expression;
                        assigned = true;
                    }
                    break;
                case Direction.NewExpression:
                    if (parent is ListInitExpression)
                    {
                        expr = ((ListInitExpression)parent).NewExpression;
                        assigned = true;
                    }
                    else if (parent is MemberInitExpression)
                    {
                        expr = ((MemberInitExpression)parent).NewExpression;
                        assigned = true;
                    }
                    break;
            }
            if (assigned)
            {
                Expression dupe = Duplicate(expr, new Step(direction), parameters, path);
                return Enumerable.Repeat(dupe, 1);
            }

            //Multi return expressions
            IList<Expression> exprs = null;
            switch (direction)
            {
                case Direction.Arguments:
                    if (parent is MethodCallExpression)
                    {
                        exprs = ((MethodCallExpression)parent).Arguments;
                        assigned = true;
                    }
                    else if (parent is NewExpression)
                    {
                        exprs = ((NewExpression)parent).Arguments;
                        assigned = true;
                    }
                    else if (parent is InvocationExpression)
                    {
                        exprs = ((InvocationExpression)parent).Arguments;
                        assigned = true;
                    }
                    break;
                case Direction.Expressions:
                    if (parent is NewArrayExpression)
                    {
                        exprs = ((NewArrayExpression)parent).Expressions;
                        assigned = true;
                    }
                    break;
            }
            if (assigned)
            {
                var dupes = new List<Expression>();
                int count = exprs.Count();
                for (int i = 0; i < count; i++)
                {
                    expr = exprs[i];

                    var dupe = Duplicate(expr, new Step(direction, i), parameters, path);

                    dupes.Add(dupe);
                }
                return dupes;
            }

            throw new ArgumentException(string.Format("Direction {0} not recognised for Expression type {1}", direction, parent.GetType().Name));
        }

        #region Duplicate Methods

        protected virtual Expression DuplicateAdd(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = DuplicateChild(expression, Direction.Left, parameters, path).Single();
            var expRight = DuplicateChild(expression, Direction.Right, parameters, path).Single();
            var rtn = Expression.Add(expLeft, expRight, expression.Method);

            return rtn;
        }

        protected virtual Expression DuplicateAddChecked(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = DuplicateChild(expression, Direction.Left, parameters, path).Single();
            var expRight = DuplicateChild(expression, Direction.Right, parameters, path).Single();
            var rtn = Expression.AddChecked(expLeft, expRight, expression.Method);
            
            return rtn;
        }

        protected virtual Expression DuplicateAnd(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = DuplicateChild(expression, Direction.Left, parameters, path).Single();
            var expRight = DuplicateChild(expression, Direction.Right, parameters, path).Single();
            var rtn = Expression.And(expLeft, expRight, expression.Method);
            
            return rtn;
        }

        protected virtual Expression DuplicateAndAlso(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = DuplicateChild(expression, Direction.Left, parameters, path).Single();
            var expRight = DuplicateChild(expression, Direction.Right, parameters, path).Single();
            var rtn = Expression.AndAlso(expLeft, expRight, expression.Method);
            
            return rtn;
        }

        protected virtual Expression DuplicateArrayIndex(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = DuplicateChild(expression, Direction.Left, parameters, path).Single();
            var expRight = DuplicateChild(expression, Direction.Right, parameters, path).Single();
            var rtn = Expression.ArrayIndex(expLeft, expRight);
            
            return rtn;
        }

        protected virtual Expression DuplicateArrayIndex(MethodCallExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = DuplicateChild(expression, Direction.Object, parameters, path).Single();
            var expArguments = DuplicateChild(expression, Direction.Arguments, parameters, path);
            var rtn = Expression.ArrayIndex(expLeft, expArguments);
            
            return rtn;
        }

        protected virtual Expression DuplicateArrayLength(UnaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expOperand = DuplicateChild(expression, Direction.Operand, parameters, path).Single();
            var rtn = Expression.ArrayLength(expOperand);
            
            return rtn;
        }

        protected virtual Expression DuplicateCall(MethodCallExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expObject = DuplicateChild(expression, Direction.Object, parameters, path).Single();
            var expArguments = DuplicateChild(expression, Direction.Arguments, parameters, path);
            var rtn = Expression.Call(expObject, expression.Method, expArguments);
            
            return rtn;
        }

        protected virtual Expression DuplicateCoalesce(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = DuplicateChild(expression, Direction.Left, parameters, path).Single();
            var expRight = DuplicateChild(expression, Direction.Right, parameters, path).Single();
            var rtn = Expression.Coalesce(expLeft, expRight, expression.Conversion);

            return rtn;
        }

        protected virtual Expression DuplicateConditional(ConditionalExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expTest = DuplicateChild(expression, Direction.Test, parameters, path).Single();
            var expIfTrue = DuplicateChild(expression, Direction.IfTrue, parameters, path).Single();
            var expIfFalse = DuplicateChild(expression, Direction.IfFalse, parameters, path).Single();
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
            var expOperand = DuplicateChild(expression, Direction.Operand, parameters, path).Single();
            var rtn = Expression.Convert(expOperand, expression.Type, expression.Method);

            return rtn;
        }

        protected virtual Expression DuplicateConvertChecked(UnaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expOperand = DuplicateChild(expression, Direction.Operand, parameters, path).Single();
            var rtn = Expression.ConvertChecked(expOperand, expression.Type, expression.Method);

            return rtn;
        }

        protected virtual Expression DuplicateDivide(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = DuplicateChild(expression, Direction.Left, parameters, path).Single();
            var expRight = DuplicateChild(expression, Direction.Right, parameters, path).Single();
            var rtn = Expression.Divide(expLeft, expRight, expression.Method);
            
            return rtn;
        }

        protected virtual Expression DuplicateEqual(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = DuplicateChild(expression, Direction.Left, parameters, path).Single();
            var expRight = DuplicateChild(expression, Direction.Right, parameters, path).Single();
            var rtn = Expression.Equal(expLeft, expRight, expression.IsLiftedToNull, expression.Method);
            
            return rtn;
        }

        protected virtual Expression DuplicateExclusiveOr(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = DuplicateChild(expression, Direction.Left, parameters, path).Single();
            var expRight = DuplicateChild(expression, Direction.Right, parameters, path).Single();
            var rtn = Expression.ExclusiveOr(expLeft, expRight, expression.Method);
            
            return rtn;
        }

        protected virtual Expression DuplicateGreaterThan(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = DuplicateChild(expression, Direction.Left, parameters, path).Single();
            var expRight = DuplicateChild(expression, Direction.Right, parameters, path).Single();
            var rtn = Expression.GreaterThan(expLeft, expRight, expression.IsLiftedToNull, expression.Method);
            
            return rtn;
        }

        protected virtual Expression DuplicateGreaterThanOrEqual(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = DuplicateChild(expression, Direction.Left, parameters, path).Single();
            var expRight = DuplicateChild(expression, Direction.Right, parameters, path).Single();
            var rtn = Expression.GreaterThanOrEqual(expLeft, expRight, expression.IsLiftedToNull, expression.Method);
            
            return rtn;
        }

        protected virtual Expression DuplicateInvoke(InvocationExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expExpression = DuplicateChild(expression, Direction.Expression, parameters, path).Single();
            var expArguments = DuplicateChild(expression, Direction.Arguments, parameters, path);
            var rtn = Expression.Invoke(expExpression, expArguments);
            
            return rtn;
        }

        protected virtual Expression DuplicateLambda(LambdaExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expBody = DuplicateChild(expression, Direction.Body, parameters, path).Single();

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
            var expLeft = DuplicateChild(expression, Direction.Left, parameters, path).Single();
            var expRight = DuplicateChild(expression, Direction.Right, parameters, path).Single();
            var rtn = Expression.LeftShift(expLeft, expRight, expression.Method);

            return rtn;
        }

        protected virtual BinaryExpression DuplicateLessThan(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = DuplicateChild(expression, Direction.Left, parameters, path).Single();
            var expRight = DuplicateChild(expression, Direction.Right, parameters, path).Single();
            var rtn = Expression.LessThan(expLeft, expRight, expression.IsLiftedToNull, expression.Method);

            return rtn;
        }

        protected virtual BinaryExpression DuplicateLessThanOrEqual(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = DuplicateChild(expression, Direction.Left, parameters, path).Single();
            var expRight = DuplicateChild(expression, Direction.Right, parameters, path).Single();
            var rtn = Expression.LessThanOrEqual(expLeft, expRight, expression.IsLiftedToNull, expression.Method);
            
            return rtn;
        }

        protected virtual ListInitExpression DuplicateListInit(ListInitExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expNewExpression = (NewExpression)DuplicateChild(expression, Direction.NewExpression, parameters, path).Single();
            var rtn = Expression.ListInit(expNewExpression, expression.Initializers);
            
            return rtn;
        }

        protected virtual MemberExpression DuplicateMemberAccess(MemberExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expExpression = DuplicateChild(expression, Direction.Expression, parameters, path).Single();
            var rtn = Expression.MakeMemberAccess(expExpression, expression.Member);
            
            return rtn;
        }

        protected virtual MemberInitExpression DuplicateMemberInit(MemberInitExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expNewExpression = (NewExpression)DuplicateChild(expression, Direction.NewExpression, parameters, path).Single();
            var rtn = Expression.MemberInit(expNewExpression, expression.Bindings);
            
            return rtn;
        }

        protected virtual BinaryExpression DuplicateModulo(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = DuplicateChild(expression, Direction.Left, parameters, path).Single();
            var expRight = DuplicateChild(expression, Direction.Right, parameters, path).Single();
            var rtn = Expression.Modulo(expLeft, expRight, expression.Method);
            
            return rtn;
        }

        protected virtual BinaryExpression DuplicateMultiply(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = DuplicateChild(expression, Direction.Left, parameters, path).Single();
            var expRight = DuplicateChild(expression, Direction.Right, parameters, path).Single();
            var rtn = Expression.Multiply(expLeft, expRight, expression.Method);
            
            return rtn;
        }

        protected virtual BinaryExpression DuplicateMultiplyChecked(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = DuplicateChild(expression, Direction.Left, parameters, path).Single();
            var expRight = DuplicateChild(expression, Direction.Right, parameters, path).Single();
            var rtn = Expression.MultiplyChecked(expLeft, expRight, expression.Method);
            
            return rtn;
        }

        protected virtual UnaryExpression DuplicateNegate(UnaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expOperand = DuplicateChild(expression, Direction.Operand, parameters, path).Single();
            var rtn = Expression.Negate(expOperand, expression.Method);
            
            return rtn;
        }

        protected virtual UnaryExpression DuplicateNegateChecked(UnaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expOperand = DuplicateChild(expression, Direction.Operand, parameters, path).Single();
            var rtn = Expression.NegateChecked(expOperand, expression.Method);
            
            return rtn;
        }

        protected virtual NewExpression DuplicateNew(NewExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expArguments = DuplicateChild(expression, Direction.Arguments, parameters, path);
            var rtn = Expression.New(expression.Constructor, expArguments, expression.Members);
            
            return rtn;
        }

        protected virtual NewArrayExpression DuplicateNewArrayBounds(NewArrayExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expExpressions = DuplicateChild(expression, Direction.Expressions, parameters, path);
            var rtn = Expression.NewArrayBounds(expression.Type.GetElementType(), expExpressions);
            
            return rtn;
        }

        protected virtual NewArrayExpression DuplicateNewArrayInit(NewArrayExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expExpressions = DuplicateChild(expression, Direction.Expressions, parameters, path);
            var rtn = Expression.NewArrayInit(expression.Type.GetElementType(), expExpressions);
            
            return rtn;
        }

        protected virtual UnaryExpression DuplicateNot(UnaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expOperand = DuplicateChild(expression, Direction.Operand, parameters, path).Single();
            var rtn = Expression.Not(expOperand, expression.Method);
            
            return rtn;
        }

        protected virtual BinaryExpression DuplicateNotEqual(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = DuplicateChild(expression, Direction.Left, parameters, path).Single();
            var expRight = DuplicateChild(expression, Direction.Right, parameters, path).Single();
            var rtn = Expression.NotEqual(expLeft, expRight, expression.IsLiftedToNull, expression.Method);
            
            return rtn;
        }

        protected virtual BinaryExpression DuplicateOr(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = DuplicateChild(expression, Direction.Left, parameters, path).Single();
            var expRight = DuplicateChild(expression, Direction.Right, parameters, path).Single();
            var rtn = Expression.Or(expLeft, expRight, expression.Method);
            
            return rtn;
        }

        protected virtual BinaryExpression DuplicateOrElse(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = DuplicateChild(expression, Direction.Left, parameters, path).Single();
            var expRight = DuplicateChild(expression, Direction.Right, parameters, path).Single();
            var rtn = Expression.OrElse(expLeft, expRight, expression.Method);
            
            return rtn;
        }

        protected virtual ParameterExpression DuplicateParameter(ParameterExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            ParameterExpression rtn = null;

            //Test if the constant has been encountered before
            string expressionName = expression.Name;
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
                rtn = Expression.Parameter(expression.Type, expression.Name);
                parameters.TryAdd(rtn, path.ToArray());
            }

            return rtn;
        }

        protected virtual BinaryExpression DuplicatePower(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = DuplicateChild(expression, Direction.Left, parameters, path).Single();
            var expRight = DuplicateChild(expression, Direction.Right, parameters, path).Single();
            var rtn = Expression.Power(expLeft, expRight, expression.Method);
            
            return rtn;
        }

        protected virtual UnaryExpression DuplicateQuote(UnaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expOperand = DuplicateChild(expression, Direction.Operand, parameters, path).Single();
            var rtn = Expression.Quote(expOperand);
            
            return rtn;
        }

        protected virtual BinaryExpression DuplicateRightShift(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = DuplicateChild(expression, Direction.Left, parameters, path).Single();
            var expRight = DuplicateChild(expression, Direction.Right, parameters, path).Single();
            var rtn = Expression.RightShift(expLeft, expRight, expression.Method);
            
            return rtn;
        }

        protected virtual BinaryExpression DuplicateSubtract(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = DuplicateChild(expression, Direction.Left, parameters, path).Single();
            var expRight = DuplicateChild(expression, Direction.Right, parameters, path).Single();
            var rtn = Expression.Subtract(expLeft, expRight, expression.Method);
            
            return rtn;
        }

        protected virtual BinaryExpression DuplicateSubtractChecked(BinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expLeft = DuplicateChild(expression, Direction.Left, parameters, path).Single();
            var expRight = DuplicateChild(expression, Direction.Right, parameters, path).Single();
            var rtn = Expression.SubtractChecked(expLeft, expRight, expression.Method);
            
            return rtn;
        }

        protected virtual UnaryExpression DuplicateTypeAs(UnaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expOperand = DuplicateChild(expression, Direction.Operand, parameters, path).Single();
            var rtn = Expression.TypeAs(expOperand, expression.Type);
            
            return rtn;
        }

        protected virtual TypeBinaryExpression DuplicateTypeIs(TypeBinaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expExpression = DuplicateChild(expression, Direction.Expression, parameters, path).Single();
            var rtn = Expression.TypeIs(expExpression, expression.TypeOperand);
            
            return rtn;
        }

        protected virtual UnaryExpression DuplicateUnaryPlus(UnaryExpression expression, ParamDictionary parameters, IEnumerable<Step> path)
        {
            var expOperand = DuplicateChild(expression, Direction.Operand, parameters, path).Single();
            var rtn = Expression.UnaryPlus(expOperand, expression.Method);
            
            return rtn;
        }
        #endregion
        #endregion

        protected class ParamDictionary : IDictionary<ParameterExpression, Step[]>
        {
            #region Fields

            readonly Dictionary<string, KeyValuePair<ParameterExpression, Step[]>> innerDictionary = new Dictionary<string, KeyValuePair<ParameterExpression, Step[]>>();
            #endregion

            #region Implementation of IEnumerable

            public IEnumerator<KeyValuePair<ParameterExpression, Step[]>> GetEnumerator()
            {
                return innerDictionary.Values.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
            #endregion

            #region Implementation of ICollection<KeyValuePair<ParameterExpression, Step[]>>

            void ICollection<KeyValuePair<ParameterExpression, Step[]>>.Add(KeyValuePair<ParameterExpression, Step[]> item)
            {
                innerDictionary.Add(item.Key.Name, item);
            }

            void ICollection<KeyValuePair<ParameterExpression, Step[]>>.Clear()
            {
                innerDictionary.Clear();
            }

            bool ICollection<KeyValuePair<ParameterExpression, Step[]>>.Contains(KeyValuePair<ParameterExpression, Step[]> item)
            {
                return this.ContainsKey(item.Key);
            }

            void ICollection<KeyValuePair<ParameterExpression, Step[]>>.CopyTo(KeyValuePair<ParameterExpression, Step[]>[] array, int arrayIndex)
            {
                innerDictionary.Values.CopyTo(array, arrayIndex);
            }

            bool ICollection<KeyValuePair<ParameterExpression, Step[]>>.Remove(KeyValuePair<ParameterExpression, Step[]> item)
            {
                return this.Remove(item.Key);
            }

            public int Count
            {
                get { return innerDictionary.Count; }
            }

            bool ICollection<KeyValuePair<ParameterExpression, Step[]>>.IsReadOnly
            {
                get
                {
                    return ((ICollection<KeyValuePair<string, KeyValuePair<ParameterExpression, Step[]>>>)innerDictionary).IsReadOnly;
                }
            }
            #endregion

            #region Implementation of IDictionary<ParameterExpression, Step[]>

            public bool ContainsKey(ParameterExpression key)
            {
                return innerDictionary.ContainsKey(key.Name);
            }

            public void Add(ParameterExpression key, Step[] value)
            {
                KeyValuePair<ParameterExpression, Step[]> pair = new KeyValuePair<ParameterExpression, Step[]>(key, value);
                innerDictionary.Add(key.Name, pair);
            }

            public bool TryAdd(ParameterExpression key, Step[] value)
            {
                if (innerDictionary.ContainsKey(key.Name))
                    return false;

                KeyValuePair<ParameterExpression, Step[]> pair = new KeyValuePair<ParameterExpression, Step[]>(key, value);
                innerDictionary.Add(key.Name, pair);

                return true;
            }

            public bool Remove(ParameterExpression key)
            {
                return innerDictionary.Remove(key.Name);
            }

            public bool TryGetValue(ParameterExpression key, out Step[] value)
            {
                KeyValuePair<ParameterExpression, Step[]> pair;
                bool result = innerDictionary.TryGetValue(key.Name, out pair);
                value = result ? pair.Value : null;
                return result;
            }

            public Step[] this[ParameterExpression key]
            {
                get { return innerDictionary[key.Name].Value; }
                set
                {
                    KeyValuePair<ParameterExpression, Step[]> pair = new KeyValuePair<ParameterExpression, Step[]>(key, value);
                    innerDictionary[key.Name] = pair;
                }
            }

            public ICollection<ParameterExpression> Keys
            {
                get { return innerDictionary.Values.Select(r => r.Key).ToList(); }
            }

            public ICollection<Step[]> Values
            {
                get { return innerDictionary.Values.Select(r => r.Value).ToList(); }
            }
            #endregion

            #region Public Methods

            public KeyValuePair<ParameterExpression, Step[]> GetPair(ParameterExpression expression)
            {
                return innerDictionary[expression.Name];
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
                var rtn = new List<KeyValuePair<ParameterExpression, Step[]>>();
                foreach (var expression in expressions)
                {
                    var pair = this.GetPair(expression);
                    rtn.Add(new KeyValuePair<ParameterExpression, Step[]>(pair.Key, pair.Value));
                }

                //Add remaining elements
                foreach (var pair in this)
                {
                    if (!ContainsKey(rtn, pair.Key))
                        rtn.Add(new KeyValuePair<ParameterExpression, Step[]>(pair.Key, pair.Value));
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

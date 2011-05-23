using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Plethora.ExpressionAide
{
    internal static class LambdaKeyer
    {
        /// <summary>
        /// Optimised version of the Expression .ToString method.
        /// </summary>
        public static string GetKey(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            StringBuilder sb = new StringBuilder(1024);
            BuildString(expression, sb);
            string str = sb.ToString();
            return str;
        }


        private static void BuildString(Expression expression, StringBuilder builder)
        {
            if (expression is BinaryExpression)
                BuildString((BinaryExpression)expression, builder);
            else if (expression is ConstantExpression)
                BuildString((ConstantExpression)expression, builder);
            else if (expression is ConditionalExpression)
                BuildString((ConditionalExpression)expression, builder);
            else if (expression is InvocationExpression)
                BuildString((InvocationExpression)expression, builder);
            else if (expression is LambdaExpression)
                BuildString((LambdaExpression)expression, builder);
            else if (expression is ListInitExpression)
                BuildString((ListInitExpression)expression, builder);
            else if (expression is MemberExpression)
                BuildString((MemberExpression)expression, builder);
            else if (expression is MemberInitExpression)
                BuildString((MemberInitExpression)expression, builder);
            else if (expression is MethodCallExpression)
                BuildString((MethodCallExpression)expression, builder);
            else if (expression is NewArrayExpression)
                BuildString((NewArrayExpression)expression, builder);
            else if (expression is NewExpression)
                BuildString((NewExpression)expression, builder);
            else if (expression is ParameterExpression)
                BuildString((ParameterExpression)expression, builder);
            else if (expression is TypeBinaryExpression)
                BuildString((TypeBinaryExpression)expression, builder);
            else if (expression is UnaryExpression)
                BuildString((UnaryExpression)expression, builder);
            else
            {
                builder.Append(GetNodeType(expression.NodeType));
            }
        }

        private static void BuildString(BinaryExpression expression, StringBuilder builder)
        {
            if (expression.NodeType == ExpressionType.ArrayIndex)
            {
                BuildString(expression.Left, builder);
                BuildString(expression.Right, builder);
            }
            else
            {
                string @operator = GetOperator(expression);
                if (@operator != null)
                {
                    BuildString(expression.Left, builder);
                    builder.Append(@operator);
                    BuildString(expression.Right, builder);
                }
                else
                {
                    builder.Append(GetNodeType(expression.NodeType));
                    BuildString(expression.Left, builder);
                    BuildString(expression.Right, builder);
                }
            }
        }

        private static void BuildString(ConstantExpression expression, StringBuilder builder)
        {
            if (expression.Value == null)
                builder.Append("null");
            else
                builder.Append(expression.Value);
        }

        private static void BuildString(ConditionalExpression expression, StringBuilder builder)
        {
            builder.Append("IIF");
            BuildString(expression.Test, builder);
            BuildString(expression.IfTrue, builder);
            BuildString(expression.IfFalse, builder);
        }

        private static void BuildString(InvocationExpression expression, StringBuilder builder)
        {
            builder.Append("Invoke");
            BuildString(expression.Expression, builder);
            int num = 0;
            int count = expression.Arguments.Count;
            while (num < count)
            {
                BuildString(expression.Arguments[num], builder);
                num++;
            }
        }

        private static void BuildString(LambdaExpression expression, StringBuilder builder)
        {
            if (expression.Parameters.Count == 1)
            {
                BuildString(expression.Parameters[0], builder);
            }
            else
            {
                foreach (ParameterExpression parameter in expression.Parameters)
                {
                    BuildString(parameter, builder);
                }
            }
            builder.Append("=>");
            BuildString(expression.Body, builder);
        }

        private static void BuildString(ListInitExpression expression, StringBuilder builder)
        {
            BuildString(expression.NewExpression, builder);
            foreach (ElementInit initializer in expression.Initializers)
            {
                BuildString(initializer, builder);
            }
        }

        private static void BuildString(MemberExpression expression, StringBuilder builder)
        {
            if (expression.Expression != null)
            {
                BuildString(expression.Expression, builder);
            }
            else
            {
                builder.Append(expression.Member.DeclaringType.Name);
            }
            builder.Append(".");
            builder.Append(expression.Member.Name);
        }

        private static void BuildString(MemberInitExpression expression, StringBuilder builder)
        {
            NewExpression newExpression = expression.NewExpression;
            if ((newExpression.Arguments.Count == 0) && newExpression.Type.Name.Contains("<"))
            {
                builder.Append("new");
            }
            else
            {
                BuildString(newExpression, builder);
            }
            foreach (MemberBinding binding in expression.Bindings)
            {
                BuildString(binding, builder);
            }
        }

        private static void BuildString(MethodCallExpression expression, StringBuilder builder)
        {
            Expression @object = expression.Object;
            if (Attribute.GetCustomAttribute(expression.Method, typeof(ExtensionAttribute)) != null)
            {
                @object = expression.Arguments[0];
            }
            if (@object != null)
            {
                BuildString(@object, builder);
            }
            builder.Append(expression.Method.Name);
            foreach (Expression argument in expression.Arguments)
            {
                BuildString(argument, builder);
            }
        }

        private static void BuildString(NewArrayExpression expression, StringBuilder builder)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.NewArrayInit:
                    {
                        builder.Append("new[]");
                        break;
                    }
                case ExpressionType.NewArrayBounds:
                    {
                        builder.Append("new");
                        builder.Append(expression.Type.ToString());
                        break;
                    }
            }
            foreach (Expression exp in expression.Expressions)
            {
                BuildString(exp, builder);
            }
        }

        private static void BuildString(NewExpression expression, StringBuilder builder)
        {
            Type type = (expression.Constructor == null) ? (expression.Type) : expression.Constructor.DeclaringType;
            builder.Append("new");
            int count = expression.Arguments.Count;
            builder.Append(type.Name);
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    if (expression.Members != null)
                    {
                        PropertyInfo info;
                        MemberInfo member = expression.Members[i];
                        if ((member.MemberType == MemberTypes.Method) && ((info = GetPropertyNoThrow((MethodInfo)member)) != null))
                        {
                            builder.Append(info.Name);
                        }
                        else
                        {
                            builder.Append(expression.Members[i].Name);
                        }
                        builder.Append("=");
                    }
                    BuildString(expression.Arguments[i], builder);
                }
            }
        }

        private static void BuildString(ParameterExpression expression, StringBuilder builder)
        {
            builder.Append(expression.Name ?? "<param>");
        }

        private static void BuildString(TypeBinaryExpression expression, StringBuilder builder)
        {
            BuildString(expression.Expression, builder);
            builder.Append("Is");
            builder.Append(expression.TypeOperand.Name);
        }

        private static void BuildString(UnaryExpression expression, StringBuilder builder)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                    builder.Append("-");
                    BuildString(expression.Operand, builder);
                    return;

                case ExpressionType.UnaryPlus:
                    builder.Append("+");
                    BuildString(expression.Operand, builder);
                    return;

                case ExpressionType.Not:
                    builder.Append("Not");
                    BuildString(expression.Operand, builder);
                    return;

                case ExpressionType.Quote:
                    BuildString(expression.Operand, builder);
                    return;

                case ExpressionType.TypeAs:
                    BuildString(expression.Operand, builder);
                    builder.Append("As");
                    builder.Append(expression.Type.Name);
                    return;
            }
            builder.Append(GetNodeType(expression.NodeType));
            BuildString(expression.Operand, builder);
        }


        private static void BuildString(ElementInit init, StringBuilder builder)
        {
            builder.Append(init.AddMethod);
            foreach (Expression expression in init.Arguments)
            {
                BuildString(expression, builder);
            }
        }


        private static void BuildString(MemberBinding binding, StringBuilder builder)
        {
            if (binding is MemberAssignment)
                BuildString((MemberAssignment)binding, builder);
            else if (binding is MemberListBinding)
                BuildString((MemberListBinding)binding, builder);
            else if (binding is MemberMemberBinding)
                BuildString((MemberMemberBinding)binding, builder);
        }

        private static void BuildString(MemberAssignment assignment, StringBuilder builder)
        {
            builder.Append(assignment.Member.Name);
            builder.Append("=");
            BuildString(assignment.Expression, builder);
        }

        private static void BuildString(MemberListBinding binding, StringBuilder builder)
        {
            builder.Append(binding.Member.Name);
            builder.Append("={");
            foreach (ElementInit initializer in binding.Initializers)
            {
                BuildString(initializer, builder);
            }
            builder.Append("}");
        }

        private static void BuildString(MemberMemberBinding binding, StringBuilder builder)
        {
            builder.Append(binding.Member.Name);
            builder.Append("={");
            foreach (MemberBinding memberBinding in binding.Bindings)
            {
                BuildString(memberBinding, builder);
            }
            builder.Append("}");
        }



        /// <summary>
        /// Optimised method to get the string of an <see cref="ExpressionType"/>.
        /// Faster than .ToString()
        /// </summary>
        private static string GetNodeType(ExpressionType nodeType)
        {
            switch (nodeType)
            {
                case ExpressionType.Add:
                    return "Add";
                case ExpressionType.AddChecked:
                    return "AddChecked";
                case ExpressionType.And:
                    return "And";
                case ExpressionType.AndAlso:
                    return "AndAlso";
                case ExpressionType.ArrayLength:
                    return "ArrayLength";
                case ExpressionType.ArrayIndex:
                    return "ArrayIndex";
                case ExpressionType.Call:
                    return "Call";
                case ExpressionType.Coalesce:
                    return "Coalesce";
                case ExpressionType.Conditional:
                    return "Conditional";
                case ExpressionType.Constant:
                    return "Constant";
                case ExpressionType.Convert:
                    return "Convert";
                case ExpressionType.ConvertChecked:
                    return "ConvertChecked";
                case ExpressionType.Divide:
                    return "Divide";
                case ExpressionType.Equal:
                    return "Equal";
                case ExpressionType.ExclusiveOr:
                    return "ExclusiveOr";
                case ExpressionType.GreaterThan:
                    return "GreaterThan";
                case ExpressionType.GreaterThanOrEqual:
                    return "GreaterThanOrEqual";
                case ExpressionType.Invoke:
                    return "Invoke";
                case ExpressionType.Lambda:
                    return "Lambda";
                case ExpressionType.LeftShift:
                    return "LeftShift";
                case ExpressionType.LessThan:
                    return "LessThan";
                case ExpressionType.LessThanOrEqual:
                    return "LessThanOrEqual";
                case ExpressionType.ListInit:
                    return "ListInit";
                case ExpressionType.MemberAccess:
                    return "MemberAccess";
                case ExpressionType.MemberInit:
                    return "MemberInit";
                case ExpressionType.Modulo:
                    return "Modulo";
                case ExpressionType.Multiply:
                    return "Multiply";
                case ExpressionType.MultiplyChecked:
                    return "MultiplyChecked";
                case ExpressionType.Negate:
                    return "Negate";
                case ExpressionType.UnaryPlus:
                    return "UnaryPlus";
                case ExpressionType.NegateChecked:
                    return "NegateChecked";
                case ExpressionType.New:
                    return "New";
                case ExpressionType.NewArrayInit:
                    return "NewArrayInit";
                case ExpressionType.NewArrayBounds:
                    return "NewArrayBounds";
                case ExpressionType.Not:
                    return "Not";
                case ExpressionType.NotEqual:
                    return "NotEqual";
                case ExpressionType.Or:
                    return "Or";
                case ExpressionType.OrElse:
                    return "OrElse";
                case ExpressionType.Parameter:
                    return "Parameter";
                case ExpressionType.Power:
                    return "Power";
                case ExpressionType.Quote:
                    return "Quote";
                case ExpressionType.RightShift:
                    return "RightShift";
                case ExpressionType.Subtract:
                    return "Subtract";
                case ExpressionType.SubtractChecked:
                    return "SubtractChecked";
                case ExpressionType.TypeAs:
                    return "TypeAs";
                case ExpressionType.TypeIs:
                    return "TypeIs";
            }
            return nodeType.ToString();
        }


        private static PropertyInfo GetPropertyNoThrow(MethodBase method)
        {
            if (method != null)
            {
                Type declaringType = method.DeclaringType;
                BindingFlags bindingAttr = BindingFlags.NonPublic | BindingFlags.Public;
                bindingAttr |= (BindingFlags)(method.IsStatic ? 8 : 4);
                foreach (PropertyInfo info in declaringType.GetProperties(bindingAttr))
                {
                    if (info.CanRead && (method == info.GetGetMethod(true)))
                    {
                        return info;
                    }
                    if (info.CanWrite && (method == info.GetSetMethod(true)))
                    {
                        return info;
                    }
                }
            }
            return null;
        }

        private static string GetOperator(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                    return "+";

                case ExpressionType.And:
                    if ((expression.Type != typeof(bool)) && (expression.Type != typeof(bool?)))
                    {
                        return "&";
                    }
                    return "And";

                case ExpressionType.AndAlso:
                    return "&&";

                case ExpressionType.Coalesce:
                    return "??";

                case ExpressionType.Divide:
                    return "/";

                case ExpressionType.Equal:
                    return "=";

                case ExpressionType.ExclusiveOr:
                    return "^";

                case ExpressionType.GreaterThan:
                    return ">";

                case ExpressionType.GreaterThanOrEqual:
                    return ">=";

                case ExpressionType.LeftShift:
                    return "<<";

                case ExpressionType.LessThan:
                    return "<";

                case ExpressionType.LessThanOrEqual:
                    return "<=";

                case ExpressionType.Modulo:
                    return "%";

                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                    return "*";

                case ExpressionType.NotEqual:
                    return "!=";

                case ExpressionType.Or:
                    if ((expression.Type != typeof(bool)) && (expression.Type != typeof(bool?)))
                    {
                        return "|";
                    }
                    return "Or";

                case ExpressionType.OrElse:
                    return "||";

                case ExpressionType.Power:
                    return "^";

                case ExpressionType.RightShift:
                    return ">>";

                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                    return "-";
            }
            return null;
        }
    }
}

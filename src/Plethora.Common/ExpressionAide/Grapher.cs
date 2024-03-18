using System.Linq.Expressions;
using System.Text;

namespace Plethora.ExpressionAide
{
    /// <summary>
    /// Helper class to construct a textual graph to diagnose expression trees.
    /// </summary>
    public class Grapher
    {
        public static string TextGraph(Expression expression)
        {
            StringBuilder sb = new StringBuilder();
            AddGraphNode(sb, 0, "", expression);
            return sb.ToString();
        }


        private static void AddGraphNode(StringBuilder sb, int depth, string relationDescription, Expression expression)
        {
            if (expression == null)
                return;

            string strBranch = string.Empty;
            for (int i = 0; i < depth; i++)
            {
                strBranch += "| ";
            }
            string strNode = strBranch + "+-";
            strBranch += "|     ";

            sb.AppendLine($"{strNode} {relationDescription}: {expression.NodeType} - {expression.GetType().Name} [{expression.GetHashCode()}]");


            sb.Append(strBranch);
            sb.AppendLine("Type = " + expression.Type);
            if (expression is BinaryExpression)
            {
                var exp = (BinaryExpression)expression;

                sb.AppendLine(strBranch + "Method = " + ((exp.Method == null) ? "<null>" : exp.Method.Name));
                sb.AppendLine(strBranch + "IsLifted = " + exp.IsLifted);
                sb.AppendLine(strBranch + "IsLiftedToNull = " + exp.IsLiftedToNull);
                AddGraphNode(sb, depth + 1, "Conversion", exp.Conversion!);
                AddGraphNode(sb, depth + 1, "Left", exp.Left);
                AddGraphNode(sb, depth + 1, "Right", exp.Right);
            }
            else if (expression is ConditionalExpression)
            {
                var exp = (ConditionalExpression)expression;

                AddGraphNode(sb, depth + 1, "Test", exp.Test);
                AddGraphNode(sb, depth + 1, "True", exp.IfTrue);
                AddGraphNode(sb, depth + 1, "False", exp.IfFalse);
            }
            else if (expression is ConstantExpression)
            {
                var exp = (ConstantExpression)expression;

                sb.Append(strBranch);
                sb.AppendLine("Value = " + exp.Value);
            }
            else if (expression is InvocationExpression)
            {
                var exp = (InvocationExpression)expression;

                int i = 0;
                foreach (var argument in exp.Arguments)
                {
                    AddGraphNode(sb, depth + 1, "Argument " + i, argument);
                    i++;
                }
                AddGraphNode(sb, depth + 1, "Expression", exp.Expression);
            }
            else if (expression is LambdaExpression)
            {
                var exp = (LambdaExpression)expression;

                sb.Append(strBranch);
                sb.AppendLine(exp.ToString());

                int i = 0;
                foreach (var parameter in exp.Parameters)
                {
                    AddGraphNode(sb, depth + 1, "Parameter " + i, parameter);
                    i++;
                }
                AddGraphNode(sb, depth + 1, "Body", exp.Body);
            }
            else if (expression is MemberExpression)
            {
                var exp = (MemberExpression)expression;

                sb.Append(strBranch);
                sb.AppendLine("Member = " + exp.Member.Name);
                AddGraphNode(sb, depth + 1, "Expression", exp.Expression!);
            }
            else if (expression is MethodCallExpression)
            {
                var exp = (MethodCallExpression)expression;

                var method = exp.Method;

                sb.Append(strBranch);
                sb.AppendLine("Method = " + (method.DeclaringType + "." + method.Name));
                AddGraphNode(sb, depth + 1, "Object", exp.Object!);

                int i = 0;
                foreach (var argument in exp.Arguments)
                {
                    AddGraphNode(sb, depth + 1, "Argument " + i, argument);
                    i++;
                }
            }
            else if (expression is NewExpression)
            {
                var exp = (NewExpression)expression;

                sb.Append(strBranch);
                sb.AppendLine("Constructor = " + exp.Constructor!.Name);

                int i = 0;
                foreach (var argument in exp.Arguments)
                {
                    AddGraphNode(sb, depth + 1, "Argument " + i, argument);
                    i++;
                }

                i = 0;
                foreach (var member in exp.Members!)
                {
                    sb.Append(strBranch);
                    sb.AppendLine("Member " + i + " = " + member.Name);
                    i++;
                }
            }
            else if (expression is NewArrayExpression)
            {
                var exp = (NewArrayExpression)expression;

                int i = 0;
                foreach (var express in exp.Expressions)
                {
                    AddGraphNode(sb, depth + 1, "Expression " + i, express);
                    i++;
                }
            }
            else if (expression is MemberInitExpression)
            {
                var exp = (MemberInitExpression)expression;

                int i = 0;
                foreach (var binding in exp.Bindings)
                {
                    sb.Append(strBranch);
                    sb.AppendLine("Binding " + i + ".BindingType = " + binding.BindingType);
                    sb.Append(strBranch);
                    sb.AppendLine("Binding " + i + ".Member = " + binding.Member.Name);


                    if (binding is MemberAssignment)
                    {
                        var bind = (MemberAssignment)binding;

                        AddGraphNode(sb, depth + 1, "Binding " + i + ".Expression", bind.Expression);
                    }
                    else if (binding is MemberMemberBinding)
                    {
                        var bind = (MemberMemberBinding)binding;

                        //AddGraphNode(sb, depth+1, "Binding " + i + ".Expression", bind.Bindings);
                    }
                    else if (binding is MemberListBinding)
                    {
                        var bind = (MemberListBinding)binding;

                        //AddGraphNode(sb, depth+1, "Binding " + i + ".Expression", bind.Initializers);
                    }

                    i++;
                }

                AddGraphNode(sb, depth + 1, "NewExpression", exp.NewExpression);
            }
            else if (expression is ListInitExpression)
            {
                var exp = (ListInitExpression)expression;

                int i = 0;
                foreach (var elementInit in exp.Initializers)
                {
                    sb.Append(strBranch);
                    sb.AppendLine("Initializer " + i + ".AddMethod = " + elementInit.AddMethod.Name);
                    int j = 0;
                    foreach (var argument in elementInit.Arguments)
                    {
                        AddGraphNode(sb, depth + 1, "Initializer " + i + ".Argument " + j, argument);
                        j++;
                    }
                    i++;
                }

                AddGraphNode(sb, depth + 1, "NewExpression", exp.NewExpression);
            }
            else if (expression is ParameterExpression)
            {
                var exp = (ParameterExpression)expression;

                sb.Append(strBranch);
                sb.AppendLine("Name = " + exp.Name);
            }
            else if (expression is TypeBinaryExpression)
            {
                var exp = (TypeBinaryExpression)expression;

                sb.Append(strBranch);
                sb.AppendLine("TypeOperand = " + exp.TypeOperand.Name);
                AddGraphNode(sb, depth + 1, "Expression", exp.Expression);
            }
            else if (expression is UnaryExpression)
            {
                var exp = (UnaryExpression)expression;

                sb.AppendLine(strBranch + "Method = " + ((exp.Method == null) ? "<null>" : exp.Method.Name));
                sb.AppendLine(strBranch + "IsLifted = " + exp.IsLifted);
                sb.AppendLine(strBranch + "IsLiftedToNull = " + exp.IsLiftedToNull);
                AddGraphNode(sb, depth + 1, "Operand", exp.Operand);
            }
        }
    }
}

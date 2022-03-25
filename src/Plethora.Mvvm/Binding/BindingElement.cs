using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Plethora.Mvvm.Binding
{
    public abstract class BindingElement
    {
        public abstract IBindingObserverElement CreateObserver(Type observedType);


        protected static object CreateGetter(
            Type observedType,
            Type valueType,
            Func<ParameterExpression, Expression> bodyExpressionFactory)
        {
            // Call Expression.Lambda<Func<T, TValue>>
            var parameterExp = Expression.Parameter(observedType);

            var bodyExpression = bodyExpressionFactory(parameterExp);

            var getterExpression = CreateLambdaExpression(observedType, valueType, bodyExpression, new[] { parameterExp });

            // Invoke the .Compile() method to obtain a Func<T, TValue>
            var compileMethod = getterExpression.GetType().GetMethod("Compile", new Type[0]);
            var getter = compileMethod.Invoke(getterExpression, Array.Empty<object>());

            return getter;
        }

        /// <summary>
        /// Gets the method 'Expression.Lambda<TDelegate>(Expression body, params ParameterExpression[] parameters)'
        /// where TDelegate is 'Func<T, TValue>'.
        /// </summary>
        /// <returns></returns>
        private static MethodInfo GetExpressionLambdaMethod()
        {
            var lambdaMethods = typeof(Expression).GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(m => m.Name == "Lambda")
                .Where(m => m.IsGenericMethodDefinition)
                .Where(m =>
                {
                    var parameters = m.GetParameters();
                    if ((parameters.Length == 2) &&
                        (parameters[0].ParameterType == typeof(Expression)) &&
                        (parameters[1].ParameterType == typeof(ParameterExpression[])))
                    {
                        return true;
                    }

                    return false;
                });

            var lambdaMethod = lambdaMethods.Single();
            return lambdaMethod;
        }

        /// <summary>
        /// Call 'Expression.Lambda<Func<T, TValue>>(bodyExpression, parameterExpressions)'
        /// </summary>
        /// <returns>
        /// An 'Expression<Func<T, TValue>>'
        /// </returns>
        private static object CreateLambdaExpression(Type input, Type output, Expression bodyExpression, ParameterExpression[] parameterExpressions)
        {
            // Create the type Func<T, TValue>
            var funcType = typeof(Func<,>).MakeGenericType(input, output);

            // Get the method 'Expression.Lambda<TDelegate>(Expression body, params ParameterExpression[] parameters)'
            var lambdaMethod = GetExpressionLambdaMethod();
            var lambda = lambdaMethod.MakeGenericMethod(funcType);

            // Invoke the Lambda method
            var funcExpression = lambda.Invoke(null, new object[] { bodyExpression, parameterExpressions });
            return funcExpression;
        }
    }
}

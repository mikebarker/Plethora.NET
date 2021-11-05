using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Reflection;

namespace Plethora.Test.Reflection
{
    [TestClass]
    public class Generic_Test
    {
        [TestMethod]
        public void SingleGenericMethod()
        {
            // Action
            MethodInfo method = typeof(SampleClass).GetGenericMethod(
                "SingleGenericMethod",
                new Type[]
                    {
                        typeof(Generic.Arg1),
                    });

            // Assert
            Assert.IsNotNull(method);
            Assert.IsTrue(method.IsGenericMethod);
            Assert.IsTrue(method.IsGenericMethodDefinition);
            Assert.AreEqual("SingleGenericMethod", method.Name);
            Assert.AreEqual(1, method.GetGenericArguments().Length);
            Assert.AreEqual(method.GetGenericArguments()[0], method.GetParameters()[0].ParameterType);
        }

        [TestMethod]
        public void StructRestrictedGenericMethod()
        {
            // Action
            MethodInfo method = typeof(SampleClass).GetGenericMethod(
                "StructRestrictedGenericMethod",
                new Type[]
                    {
                        typeof(Generic.Arg1),
                    });

            // Assert
            Assert.IsNotNull(method);
            Assert.IsTrue(method.IsGenericMethod);
            Assert.IsTrue(method.IsGenericMethodDefinition);
            Assert.AreEqual("StructRestrictedGenericMethod", method.Name);
            Assert.AreEqual(1, method.GetGenericArguments().Length);
            Assert.AreEqual(method.GetGenericArguments()[0], method.GetParameters()[0].ParameterType);
        }

        [TestMethod]
        public void ClassRestrictedGenericMethod()
        {
            // Action
            MethodInfo method = typeof(SampleClass).GetGenericMethod(
                "ClassRestrictedGenericMethod",
                new Type[]
                    {
                        typeof(Generic.Arg1),
                    });

            // Assert
            Assert.IsNotNull(method);
            Assert.IsTrue(method.IsGenericMethod);
            Assert.IsTrue(method.IsGenericMethodDefinition);
            Assert.AreEqual("ClassRestrictedGenericMethod", method.Name);
            Assert.AreEqual(1, method.GetGenericArguments().Length);
            Assert.AreEqual(method.GetGenericArguments()[0], method.GetParameters()[0].ParameterType);
        }

        [TestMethod]
        public void NonGenericMethod()
        {
            // Action
            MethodInfo method = typeof(SampleClass).GetGenericMethod(
                "NonGenericMethod",
                new Type[]
                    {
                        typeof(Generic.Arg1),
                        typeof(int)
                    });

            // Assert
            Assert.IsNotNull(method);
            Assert.IsTrue(method.IsGenericMethod);
            Assert.IsTrue(method.IsGenericMethodDefinition);
            Assert.AreEqual("NonGenericMethod", method.Name);
            Assert.AreEqual(1, method.GetGenericArguments().Length);
        }

        [TestMethod]
        public void QuadrupleGenericMethod()
        {
            // Action
            MethodInfo method = typeof(SampleClass).GetGenericMethod(
                "QuadrupleGenericMethod",
                new Type[]
                    {
                        typeof(Generic.Arg1),
                        typeof(Generic.Arg2),
                        typeof(Generic.Arg3),
                        typeof(Generic.Arg4),
                    });

            // Assert
            Assert.IsNotNull(method);
            Assert.IsTrue(method.IsGenericMethod);
            Assert.IsTrue(method.IsGenericMethodDefinition);
            Assert.AreEqual("QuadrupleGenericMethod", method.Name);
            Assert.AreEqual(4, method.GetGenericArguments().Length);
        }

        [TestMethod]
        public void NestedGenericMethod()
        {
            // Action
            MethodInfo method = typeof(SampleClass).GetGenericMethod(
                "NestedGenericMethod",
                new Type[]
                    {
                        typeof(IEnumerable<Generic.Arg1>),
                        typeof(Expression<Func<ICollection<Generic.Arg1>>>),
                    });

            // Assert
            Assert.IsNotNull(method);
            Assert.IsTrue(method.IsGenericMethod);
            Assert.IsTrue(method.IsGenericMethodDefinition);
            Assert.AreEqual("NestedGenericMethod", method.Name);
            Assert.AreEqual(1, method.GetGenericArguments().Length);
        }

        [TestMethod]
        public void OverloadedMethod()
        {
            // Action
            MethodInfo method = typeof(SampleClass).GetGenericMethod(
                "OverloadedMethod",
                new Type[]
                    {
                        typeof(Generic.Arg1),
                        typeof(Func<Generic.Arg1>),
                    });

            // Assert
            Assert.IsNotNull(method);
            Assert.IsTrue(method.IsGenericMethod);
            Assert.IsTrue(method.IsGenericMethodDefinition);
            Assert.AreEqual("OverloadedMethod", method.Name);
            Assert.AreEqual(1, method.GetGenericArguments().Length);
            Assert.IsTrue(method.GetParameters()[1].ParameterType.Name.StartsWith("Func"));
        }

        [TestMethod]
        public void DoesNotExist()
        {
            // Action
            MethodInfo method = typeof(SampleClass).GetGenericMethod(
                "DoesNotExist",
                new Type[]
                    {
                        typeof(Generic.Arg1),
                    });

            // Assert
            Assert.IsNull(method);
        }


        private class SampleClass
        {
            [TestMethod]
        public void SingleGenericMethod<T>(T t)
            {
            }

            [TestMethod]
        public void StructRestrictedGenericMethod<T>(T t)
                where T : struct
            {
            }

            [TestMethod]
        public void ClassRestrictedGenericMethod<T>(T t)
                where T : class
            {
            }

            [TestMethod]
        public void NonGenericMethod<T>(T t, int i)
            {
            }

            [TestMethod]
        public void QuadrupleGenericMethod<T1, T2, T3, T4>(T1 t1, T2 t2, T3 t3, T4 t4)
            {
            }

            [TestMethod]
        public void NestedGenericMethod<T>(IEnumerable<T> t, Expression<Func<ICollection<T>>> func)
            {
            }

            [TestMethod]
        public void OverloadedMethod<T>(T t, Func<T> func)
            {
            }

            [TestMethod]
        public void OverloadedMethod<T>(T t, Action<T> action)
            {
            }
        }
    }
}

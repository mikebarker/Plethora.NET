using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using NUnit.Framework;
using Plethora.Reflection;

namespace Plethora.Test.Reflection
{
    [TestFixture]
    public class Generic_Test
    {
        [Test]
        public void SingleGenericMethod()
        {
            //exec
            MethodInfo method = typeof(SampleClass).GetGenericMethod(
                "SingleGenericMethod",
                new Type[]
                    {
                        typeof(Generic.Arg1),
                    });

            //test
            Assert.IsNotNull(method);
            Assert.IsTrue(method.IsGenericMethod);
            Assert.IsTrue(method.IsGenericMethodDefinition);
            Assert.AreEqual("SingleGenericMethod", method.Name);
            Assert.AreEqual(1, method.GetGenericArguments().Length);
            Assert.AreEqual(method.GetGenericArguments()[0], method.GetParameters()[0].ParameterType);
        }

        [Test]
        public void StructRestrictedGenericMethod()
        {
            //exec
            MethodInfo method = typeof(SampleClass).GetGenericMethod(
                "StructRestrictedGenericMethod",
                new Type[]
                    {
                        typeof(Generic.Arg1),
                    });

            //test
            Assert.IsNotNull(method);
            Assert.IsTrue(method.IsGenericMethod);
            Assert.IsTrue(method.IsGenericMethodDefinition);
            Assert.AreEqual("StructRestrictedGenericMethod", method.Name);
            Assert.AreEqual(1, method.GetGenericArguments().Length);
            Assert.AreEqual(method.GetGenericArguments()[0], method.GetParameters()[0].ParameterType);
        }

        [Test]
        public void ClassRestrictedGenericMethod()
        {
            //exec
            MethodInfo method = typeof(SampleClass).GetGenericMethod(
                "ClassRestrictedGenericMethod",
                new Type[]
                    {
                        typeof(Generic.Arg1),
                    });

            //test
            Assert.IsNotNull(method);
            Assert.IsTrue(method.IsGenericMethod);
            Assert.IsTrue(method.IsGenericMethodDefinition);
            Assert.AreEqual("ClassRestrictedGenericMethod", method.Name);
            Assert.AreEqual(1, method.GetGenericArguments().Length);
            Assert.AreEqual(method.GetGenericArguments()[0], method.GetParameters()[0].ParameterType);
        }

        [Test]
        public void NonGenericMethod()
        {
            //exec
            MethodInfo method = typeof(SampleClass).GetGenericMethod(
                "NonGenericMethod",
                new Type[]
                    {
                        typeof(Generic.Arg1),
                        typeof(int)
                    });

            //test
            Assert.IsNotNull(method);
            Assert.IsTrue(method.IsGenericMethod);
            Assert.IsTrue(method.IsGenericMethodDefinition);
            Assert.AreEqual("NonGenericMethod", method.Name);
            Assert.AreEqual(1, method.GetGenericArguments().Length);
        }

        [Test]
        public void QuadrupleGenericMethod()
        {
            //exec
            MethodInfo method = typeof(SampleClass).GetGenericMethod(
                "QuadrupleGenericMethod",
                new Type[]
                    {
                        typeof(Generic.Arg1),
                        typeof(Generic.Arg2),
                        typeof(Generic.Arg3),
                        typeof(Generic.Arg4),
                    });

            //test
            Assert.IsNotNull(method);
            Assert.IsTrue(method.IsGenericMethod);
            Assert.IsTrue(method.IsGenericMethodDefinition);
            Assert.AreEqual("QuadrupleGenericMethod", method.Name);
            Assert.AreEqual(4, method.GetGenericArguments().Length);
        }

        [Test]
        public void NestedGenericMethod()
        {
            //exec
            MethodInfo method = typeof(SampleClass).GetGenericMethod(
                "NestedGenericMethod",
                new Type[]
                    {
                        typeof(IEnumerable<Generic.Arg1>),
                        typeof(Expression<Func<ICollection<Generic.Arg1>>>),
                    });

            //test
            Assert.IsNotNull(method);
            Assert.IsTrue(method.IsGenericMethod);
            Assert.IsTrue(method.IsGenericMethodDefinition);
            Assert.AreEqual("NestedGenericMethod", method.Name);
            Assert.AreEqual(1, method.GetGenericArguments().Length);
        }

        [Test]
        public void OverloadedMethod()
        {
            //exec
            MethodInfo method = typeof(SampleClass).GetGenericMethod(
                "OverloadedMethod",
                new Type[]
                    {
                        typeof(Generic.Arg1),
                        typeof(Func<Generic.Arg1>),
                    });

            //test
            Assert.IsNotNull(method);
            Assert.IsTrue(method.IsGenericMethod);
            Assert.IsTrue(method.IsGenericMethodDefinition);
            Assert.AreEqual("OverloadedMethod", method.Name);
            Assert.AreEqual(1, method.GetGenericArguments().Length);
            Assert.IsTrue(method.GetParameters()[1].ParameterType.Name.StartsWith("Func"));
        }

        [Test]
        public void DoesNotExist()
        {
            //exec
            MethodInfo method = typeof(SampleClass).GetGenericMethod(
                "DoesNotExist",
                new Type[]
                    {
                        typeof(Generic.Arg1),
                    });

            //test
            Assert.IsNull(method);
        }


        private class SampleClass
        {
            public void SingleGenericMethod<T>(T t)
            {
            }

            public void StructRestrictedGenericMethod<T>(T t)
                where T : struct
            {
            }

            public void ClassRestrictedGenericMethod<T>(T t)
                where T : class
            {
            }

            public void NonGenericMethod<T>(T t, int i)
            {
            }

            public void QuadrupleGenericMethod<T1, T2, T3, T4>(T1 t1, T2 t2, T3 t3, T4 t4)
            {
            }

            public void NestedGenericMethod<T>(IEnumerable<T> t, Expression<Func<ICollection<T>>> func)
            {
            }

            public void OverloadedMethod<T>(T t, Func<T> func)
            {
            }

            public void OverloadedMethod<T>(T t, Action<T> action)
            {
            }
        }
    }
}

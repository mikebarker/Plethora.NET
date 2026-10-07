using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Reflection;

namespace Plethora.Test.Reflection;

[TestClass]
public class Generic_Test
{
    [TestMethod]
    public void SingleGenericMethod()
    {
        // Action
        MethodInfo method = typeof(SampleClass).GetGenericMethod(
            "SingleGenericMethod",
            [
                typeof(Generic.Arg1),
            ]);

        // Assert
        Assert.IsNotNull(method);
        Assert.IsTrue(method.IsGenericMethod);
        Assert.IsTrue(method.IsGenericMethodDefinition);
        Assert.AreEqual("SingleGenericMethod", method.Name);
        Assert.HasCount(1, method.GetGenericArguments());
        Assert.AreEqual(method.GetGenericArguments()[0], method.GetParameters()[0].ParameterType);
    }

    [TestMethod]
    public void StructRestrictedGenericMethod()
    {
        // Action
        MethodInfo method = typeof(SampleClass).GetGenericMethod(
            "StructRestrictedGenericMethod",
            [
                typeof(Generic.Arg1),
            ]);

        // Assert
        Assert.IsNotNull(method);
        Assert.IsTrue(method.IsGenericMethod);
        Assert.IsTrue(method.IsGenericMethodDefinition);
        Assert.AreEqual("StructRestrictedGenericMethod", method.Name);
        Assert.HasCount(1, method.GetGenericArguments());
        Assert.AreEqual(method.GetGenericArguments()[0], method.GetParameters()[0].ParameterType);
    }

    [TestMethod]
    public void ClassRestrictedGenericMethod()
    {
        // Action
        MethodInfo method = typeof(SampleClass).GetGenericMethod(
            "ClassRestrictedGenericMethod",
            [
                typeof(Generic.Arg1),
            ]);

        // Assert
        Assert.IsNotNull(method);
        Assert.IsTrue(method.IsGenericMethod);
        Assert.IsTrue(method.IsGenericMethodDefinition);
        Assert.AreEqual("ClassRestrictedGenericMethod", method.Name);
        Assert.HasCount(1, method.GetGenericArguments());
        Assert.AreEqual(method.GetGenericArguments()[0], method.GetParameters()[0].ParameterType);
    }

    [TestMethod]
    public void WithNonGenericMethod()
    {
        // Action
        MethodInfo method = typeof(SampleClass).GetGenericMethod(
            "WithNonGenericMethod",
            [
                typeof(Generic.Arg1),
                typeof(int),
            ]);

        // Assert
        Assert.IsNotNull(method);
        Assert.IsTrue(method.IsGenericMethod);
        Assert.IsTrue(method.IsGenericMethodDefinition);
        Assert.AreEqual("WithNonGenericMethod", method.Name);
        Assert.HasCount(1, method.GetGenericArguments());
    }

    [TestMethod]
    public void QuadrupleGenericMethod()
    {
        // Action
        MethodInfo method = typeof(SampleClass).GetGenericMethod(
            "QuadrupleGenericMethod",
            [
                typeof(Generic.Arg1),
                typeof(Generic.Arg2),
                typeof(Generic.Arg3),
                typeof(Generic.Arg4),
            ]);

        // Assert
        Assert.IsNotNull(method);
        Assert.IsTrue(method.IsGenericMethod);
        Assert.IsTrue(method.IsGenericMethodDefinition);
        Assert.AreEqual("QuadrupleGenericMethod", method.Name);
        Assert.HasCount(4, method.GetGenericArguments());
    }

    [TestMethod]
    public void NestedGenericMethod()
    {
        // Action
        MethodInfo method = typeof(SampleClass).GetGenericMethod(
            "NestedGenericMethod",
            [
                typeof(IEnumerable<Generic.Arg1>),
                typeof(Expression<Func<ICollection<Generic.Arg1>>>),
            ]);

        // Assert
        Assert.IsNotNull(method);
        Assert.IsTrue(method.IsGenericMethod);
        Assert.IsTrue(method.IsGenericMethodDefinition);
        Assert.AreEqual("NestedGenericMethod", method.Name);
        Assert.HasCount(1, method.GetGenericArguments());
    }

    [TestMethod]
    public void OverloadedMethod()
    {
        // Action
        MethodInfo method = typeof(SampleClass).GetGenericMethod(
            "OverloadedMethod",
            [
                typeof(Generic.Arg1),
                typeof(Func<Generic.Arg1>),
            ]);

        // Assert
        Assert.IsNotNull(method);
        Assert.IsTrue(method.IsGenericMethod);
        Assert.IsTrue(method.IsGenericMethodDefinition);
        Assert.AreEqual("OverloadedMethod", method.Name);
        Assert.HasCount(1, method.GetGenericArguments());
        Assert.StartsWith("Func", method.GetParameters()[1].ParameterType.Name);
    }

    [TestMethod]
    public void DoesNotExist()
    {
        // Action
        MethodInfo method = typeof(SampleClass).GetGenericMethod(
            "DoesNotExist",
            [
                typeof(Generic.Arg1),
            ]);

        // Assert
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

        public void WithNonGenericMethod<T>(T t, int i)
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

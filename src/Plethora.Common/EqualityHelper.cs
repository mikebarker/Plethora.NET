using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Plethora
{
    /// <summary>
    /// Ensures the consistant application of properties when evaluating ToString, GetHashCode, Equals methods.
    /// </summary>
    /// <typeparam name="T">
    /// The object type for which the equality methods are required.
    /// </typeparam>
    /// <remarks>
    /// The intended usage is similar to:
    ///   <code>
    ///   <![CDATA[
    ///     public class Person
    ///     {
    ///         //Compare instances of 'Person' by Name and Age only. Ignor Height and Weight properties.
    ///         private static EqualityHelper<Person> equalityHelper = EqualityHelper<Person>.Create(
    ///             person => person.Name,
    ///             person => person.Age);
    ///         
    ///         
    ///         public string Name { get; set; }
    ///         public int Age { get; set; }
    ///         public double Height { get; set; }
    ///         public double Weight { get; set; }
    ///         
    ///         
    ///         public override bool Equals(object obj)
    ///         {
    ///             return equalityHelper.Equals(this, obj);
    ///         }
    ///         
    ///         public override int GetHashCode()
    ///         {
    ///             return equalityHelper.GetHashCode(this);
    ///         }
    ///         
    ///         public override string ToString()
    ///         {
    ///             return equalityHelper.GetToString(this);
    ///         }
    ///     }
    ///   ]]>
    ///   </code>
    /// </remarks>
    public abstract class EqualityHelper<T> : IEqualityComparer<T>
    {
        #region Fields

        private readonly string toStringPattern;

        #endregion

        #region Constructors

        protected EqualityHelper(params LambdaExpression[] expressions)
        {
            this.toStringPattern = GetToStringPattern(expressions);
        }
        #endregion

        #region Abstract Methods

        public abstract bool Equals(T x, T y);
        public abstract int GetHashCode(T t);
        public abstract string GetToString(T t);

        #endregion

        #region Methods

        private static string GetToStringPattern(params LambdaExpression[] expressions)
        {
            StringBuilder sb = new StringBuilder();

            int i = 0;
            foreach (LambdaExpression expression in expressions)
            {
                string propertyName = GetPropertyOrFieldName(expression);

                if (i != 0)
                    sb.Append("; ");

                sb.Append(propertyName + "={" + i + "}");
                i++;
            }

            return sb.ToString();
        }

        private static string GetPropertyOrFieldName(LambdaExpression expression)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            var body = expression.Body as MemberExpression;
            if (body == null)
                throw new ArgumentException("Invalid expression. Expression must represent a property or field.");

            var property = body.Member as PropertyInfo;
            if (property != null)
                return property.Name;

            var field = body.Member as FieldInfo;
            if (field != null)
                return field.Name;


            throw new ArgumentException("Invalid expression. Expression must represent a property or field.");
        }

        private string ToStringFormat(T t, params object[] values)
        {
            var toString = t.GetType().Name + " {" + string.Format(this.toStringPattern, values) + "}";

            return toString;
        }


        #endregion

        #region Factory Methods

        public static EqualityHelper<T> Create<T1>(
            Expression<Func<T, T1>> exp1)
        {
            return new Helper<T1>(
                exp1);
        }

        public static EqualityHelper<T> Create<T1, T2>(
            Expression<Func<T, T1>> exp1,
            Expression<Func<T, T2>> exp2)
        {
            return new Helper<T1, T2>(
                exp1,
                exp2);
        }

        public static EqualityHelper<T> Create<T1, T2, T3>(
            Expression<Func<T, T1>> exp1,
            Expression<Func<T, T2>> exp2,
            Expression<Func<T, T3>> exp3)
        {
            return new Helper<T1, T2, T3>(
                exp1,
                exp2,
                exp3);
        }

        public static EqualityHelper<T> Create<T1, T2, T3, T4>(
            Expression<Func<T, T1>> exp1,
            Expression<Func<T, T2>> exp2,
            Expression<Func<T, T3>> exp3,
            Expression<Func<T, T4>> exp4)
        {
            return new Helper<T1, T2, T3, T4>(
                exp1,
                exp2,
                exp3,
                exp4);
        }

        public static EqualityHelper<T> Create<T1, T2, T3, T4, T5>(
            Expression<Func<T, T1>> exp1,
            Expression<Func<T, T2>> exp2,
            Expression<Func<T, T3>> exp3,
            Expression<Func<T, T4>> exp4,
            Expression<Func<T, T5>> exp5)
        {
            return new Helper<T1, T2, T3, T4, T5>(
                exp1,
                exp2,
                exp3,
                exp4,
                exp5);
        }

        public static EqualityHelper<T> Create<T1, T2, T3, T4, T5, T6>(
            Expression<Func<T, T1>> exp1,
            Expression<Func<T, T2>> exp2,
            Expression<Func<T, T3>> exp3,
            Expression<Func<T, T4>> exp4,
            Expression<Func<T, T5>> exp5,
            Expression<Func<T, T6>> exp6)
        {
            return new Helper<T1, T2, T3, T4, T5, T6>(
                exp1,
                exp2,
                exp3,
                exp4,
                exp5,
                exp6);
        }

        public static EqualityHelper<T> Create<T1, T2, T3, T4, T5, T6, T7>(
            Expression<Func<T, T1>> exp1,
            Expression<Func<T, T2>> exp2,
            Expression<Func<T, T3>> exp3,
            Expression<Func<T, T4>> exp4,
            Expression<Func<T, T5>> exp5,
            Expression<Func<T, T6>> exp6,
            Expression<Func<T, T7>> exp7)
        {
            return new Helper<T1, T2, T3, T4, T5, T6, T7>(
                exp1,
                exp2,
                exp3,
                exp4,
                exp5,
                exp6,
                exp7);
        }

        public static EqualityHelper<T> Create<T1, T2, T3, T4, T5, T6, T7, T8>(
            Expression<Func<T, T1>> exp1,
            Expression<Func<T, T2>> exp2,
            Expression<Func<T, T3>> exp3,
            Expression<Func<T, T4>> exp4,
            Expression<Func<T, T5>> exp5,
            Expression<Func<T, T6>> exp6,
            Expression<Func<T, T7>> exp7,
            Expression<Func<T, T8>> exp8)
        {
            return new Helper<T1, T2, T3, T4, T5, T6, T7, T8>(
                exp1,
                exp2,
                exp3,
                exp4,
                exp5,
                exp6,
                exp7,
                exp8);
        }

        #endregion



        private class Helper<T1> : EqualityHelper<T>
        {
            #region Fields

            private readonly Func<T, T1> func1;

            #endregion

            #region Constructors

            public Helper(
                Expression<Func<T, T1>> exp1)
                : base(exp1)
            {
                //Validation
                if (exp1 == null)
                    throw new ArgumentNullException("exp1");


                this.func1 = exp1.Compile();
            }

            #endregion

            #region Overrides of EqualityHelper<T>

            public override bool Equals(T x, T y)
            {
                if (ReferenceEquals(x, y))
                    return true;

                if (x == null)
                    return false;

                if (y == null)
                    return false;

                bool result = true;
                result = result && EqualityComparer<T1>.Default.Equals(func1(x), func1(y));

                return result;
            }

            public override int GetHashCode(T t)
            {
                return HashCodeHelper.GetHashCode(
                    func1(t));
            }

            public override string GetToString(T t)
            {
                return ToStringFormat(
                    t,
                    func1(t));
            }

            #endregion
        }

        private class Helper<T1, T2> : EqualityHelper<T>
        {
            #region Fields

            private readonly Func<T, T1> func1;
            private readonly Func<T, T2> func2;

            #endregion

            #region Constructors

            public Helper(
                Expression<Func<T, T1>> exp1,
                Expression<Func<T, T2>> exp2)
                : base(exp1, exp2)
            {
                //Validation
                if (exp1 == null)
                    throw new ArgumentNullException("exp1");

                if (exp2 == null)
                    throw new ArgumentNullException("exp2");


                this.func1 = exp1.Compile();
                this.func2 = exp2.Compile();
            }

            #endregion

            #region Overrides of EqualityHelper<T>

            public override bool Equals(T x, T y)
            {
                if (ReferenceEquals(x, y))
                    return true;

                if (x == null)
                    return false;

                if (y == null)
                    return false;

                bool result = true;
                result = result && EqualityComparer<T1>.Default.Equals(func1(x), func1(y));
                result = result && EqualityComparer<T2>.Default.Equals(func2(x), func2(y));

                return result;
            }

            public override int GetHashCode(T t)
            {
                return HashCodeHelper.GetHashCode(
                    func1(t),
                    func2(t));
            }

            public override string GetToString(T t)
            {
                return ToStringFormat(
                    t,
                    func1(t),
                    func2(t));
            }

            #endregion
        }

        private class Helper<T1, T2, T3> : EqualityHelper<T>
        {
            #region Fields

            private readonly Func<T, T1> func1;
            private readonly Func<T, T2> func2;
            private readonly Func<T, T3> func3;

            #endregion

            #region Constructors

            public Helper(
                Expression<Func<T, T1>> exp1,
                Expression<Func<T, T2>> exp2,
                Expression<Func<T, T3>> exp3)
                : base(exp1, exp2, exp3)
            {
                //Validation
                if (exp1 == null)
                    throw new ArgumentNullException("exp1");

                if (exp2 == null)
                    throw new ArgumentNullException("exp2");

                if (exp3 == null)
                    throw new ArgumentNullException("exp3");


                this.func1 = exp1.Compile();
                this.func2 = exp2.Compile();
                this.func3 = exp3.Compile();
            }

            #endregion

            #region Overrides of EqualityHelper<T>

            public override bool Equals(T x, T y)
            {
                if (ReferenceEquals(x, y))
                    return true;

                if (x == null)
                    return false;

                if (y == null)
                    return false;

                bool result = true;
                result = result && EqualityComparer<T1>.Default.Equals(func1(x), func1(y));
                result = result && EqualityComparer<T2>.Default.Equals(func2(x), func2(y));
                result = result && EqualityComparer<T3>.Default.Equals(func3(x), func3(y));

                return result;
            }

            public override int GetHashCode(T t)
            {
                return HashCodeHelper.GetHashCode(
                    func1(t),
                    func2(t),
                    func3(t));
            }

            public override string GetToString(T t)
            {
                return ToStringFormat(
                    t,
                    func1(t),
                    func2(t),
                    func3(t));
            }

            #endregion
        }

        private class Helper<T1, T2, T3, T4> : EqualityHelper<T>
        {
            #region Fields

            private readonly Func<T, T1> func1;
            private readonly Func<T, T2> func2;
            private readonly Func<T, T3> func3;
            private readonly Func<T, T4> func4;

            #endregion

            #region Constructors

            public Helper(
                Expression<Func<T, T1>> exp1,
                Expression<Func<T, T2>> exp2,
                Expression<Func<T, T3>> exp3,
                Expression<Func<T, T4>> exp4)
                : base(exp1, exp2, exp3, exp4)
            {
                //Validation
                if (exp1 == null)
                    throw new ArgumentNullException("exp1");

                if (exp2 == null)
                    throw new ArgumentNullException("exp2");

                if (exp3 == null)
                    throw new ArgumentNullException("exp3");

                if (exp4 == null)
                    throw new ArgumentNullException("exp4");


                this.func1 = exp1.Compile();
                this.func2 = exp2.Compile();
                this.func3 = exp3.Compile();
                this.func4 = exp4.Compile();
            }

            #endregion

            #region Overrides of EqualityHelper<T>

            public override bool Equals(T x, T y)
            {
                if (ReferenceEquals(x, y))
                    return true;

                if (x == null)
                    return false;

                if (y == null)
                    return false;

                bool result = true;
                result = result && EqualityComparer<T1>.Default.Equals(func1(x), func1(y));
                result = result && EqualityComparer<T2>.Default.Equals(func2(x), func2(y));
                result = result && EqualityComparer<T3>.Default.Equals(func3(x), func3(y));
                result = result && EqualityComparer<T4>.Default.Equals(func4(x), func4(y));

                return result;
            }

            public override int GetHashCode(T t)
            {
                return HashCodeHelper.GetHashCode(
                    func1(t),
                    func2(t),
                    func3(t),
                    func4(t));
            }

            public override string GetToString(T t)
            {
                return ToStringFormat(
                    t,
                    func1(t),
                    func2(t),
                    func3(t),
                    func4(t));
            }
            
            #endregion
        }

        private class Helper<T1, T2, T3, T4, T5> : EqualityHelper<T>
        {
            #region Fields

            private readonly Func<T, T1> func1;
            private readonly Func<T, T2> func2;
            private readonly Func<T, T3> func3;
            private readonly Func<T, T4> func4;
            private readonly Func<T, T5> func5;

            #endregion

            #region Constructors

            public Helper(
                Expression<Func<T, T1>> exp1,
                Expression<Func<T, T2>> exp2,
                Expression<Func<T, T3>> exp3,
                Expression<Func<T, T4>> exp4,
                Expression<Func<T, T5>> exp5)
                : base(exp1, exp2, exp3, exp4, exp5)
            {
                //Validation
                if (exp1 == null)
                    throw new ArgumentNullException("exp1");

                if (exp2 == null)
                    throw new ArgumentNullException("exp2");

                if (exp3 == null)
                    throw new ArgumentNullException("exp3");

                if (exp4 == null)
                    throw new ArgumentNullException("exp4");

                if (exp4 == null)
                    throw new ArgumentNullException("exp4");

                if (exp5 == null)
                    throw new ArgumentNullException("exp5");


                this.func1 = exp1.Compile();
                this.func2 = exp2.Compile();
                this.func3 = exp3.Compile();
                this.func4 = exp4.Compile();
                this.func5 = exp5.Compile();
            }

            #endregion

            #region Overrides of EqualityHelper<T>

            public override bool Equals(T x, T y)
            {
                if (ReferenceEquals(x, y))
                    return true;

                if (x == null)
                    return false;

                if (y == null)
                    return false;

                bool result = true;
                result = result && EqualityComparer<T1>.Default.Equals(func1(x), func1(y));
                result = result && EqualityComparer<T2>.Default.Equals(func2(x), func2(y));
                result = result && EqualityComparer<T3>.Default.Equals(func3(x), func3(y));
                result = result && EqualityComparer<T4>.Default.Equals(func4(x), func4(y));
                result = result && EqualityComparer<T5>.Default.Equals(func5(x), func5(y));

                return result;
            }

            public override int GetHashCode(T t)
            {
                return HashCodeHelper.GetHashCode(
                    func1(t),
                    func2(t),
                    func3(t),
                    func4(t),
                    func5(t));
            }

            public override string GetToString(T t)
            {
                return ToStringFormat(
                    t,
                    func1(t),
                    func2(t),
                    func3(t),
                    func4(t),
                    func5(t));
            }

            #endregion
        }

        private class Helper<T1, T2, T3, T4, T5, T6> : EqualityHelper<T>
        {
            #region Fields

            private readonly Func<T, T1> func1;
            private readonly Func<T, T2> func2;
            private readonly Func<T, T3> func3;
            private readonly Func<T, T4> func4;
            private readonly Func<T, T5> func5;
            private readonly Func<T, T6> func6;

            #endregion

            #region Constructors

            public Helper(
                Expression<Func<T, T1>> exp1,
                Expression<Func<T, T2>> exp2,
                Expression<Func<T, T3>> exp3,
                Expression<Func<T, T4>> exp4,
                Expression<Func<T, T5>> exp5,
                Expression<Func<T, T6>> exp6)
                : base(exp1, exp2, exp3, exp4, exp5, exp6)
            {
                //Validation
                if (exp1 == null)
                    throw new ArgumentNullException("exp1");

                if (exp2 == null)
                    throw new ArgumentNullException("exp2");

                if (exp3 == null)
                    throw new ArgumentNullException("exp3");

                if (exp4 == null)
                    throw new ArgumentNullException("exp4");

                if (exp4 == null)
                    throw new ArgumentNullException("exp4");

                if (exp5 == null)
                    throw new ArgumentNullException("exp5");

                if (exp6 == null)
                    throw new ArgumentNullException("exp6");


                this.func1 = exp1.Compile();
                this.func2 = exp2.Compile();
                this.func3 = exp3.Compile();
                this.func4 = exp4.Compile();
                this.func5 = exp5.Compile();
                this.func6 = exp6.Compile();
            }

            #endregion

            #region Overrides of EqualityHelper<T>

            public override bool Equals(T x, T y)
            {
                if (ReferenceEquals(x, y))
                    return true;

                if (x == null)
                    return false;

                if (y == null)
                    return false;

                bool result = true;
                result = result && EqualityComparer<T1>.Default.Equals(func1(x), func1(y));
                result = result && EqualityComparer<T2>.Default.Equals(func2(x), func2(y));
                result = result && EqualityComparer<T3>.Default.Equals(func3(x), func3(y));
                result = result && EqualityComparer<T4>.Default.Equals(func4(x), func4(y));
                result = result && EqualityComparer<T5>.Default.Equals(func5(x), func5(y));
                result = result && EqualityComparer<T6>.Default.Equals(func6(x), func6(y));

                return result;
            }

            public override int GetHashCode(T t)
            {
                return HashCodeHelper.GetHashCode(
                    func1(t),
                    func2(t),
                    func3(t),
                    func4(t),
                    func5(t),
                    func6(t));
            }

            public override string GetToString(T t)
            {
                return ToStringFormat(
                    t,
                    func1(t),
                    func2(t),
                    func3(t),
                    func4(t),
                    func5(t),
                    func6(t));
            }

            #endregion
        }

        private class Helper<T1, T2, T3, T4, T5, T6, T7> : EqualityHelper<T>
        {
            #region Fields

            private readonly Func<T, T1> func1;
            private readonly Func<T, T2> func2;
            private readonly Func<T, T3> func3;
            private readonly Func<T, T4> func4;
            private readonly Func<T, T5> func5;
            private readonly Func<T, T6> func6;
            private readonly Func<T, T7> func7;

            #endregion

            #region Constructors

            public Helper(
                Expression<Func<T, T1>> exp1,
                Expression<Func<T, T2>> exp2,
                Expression<Func<T, T3>> exp3,
                Expression<Func<T, T4>> exp4,
                Expression<Func<T, T5>> exp5,
                Expression<Func<T, T6>> exp6,
                Expression<Func<T, T7>> exp7)
                : base(exp1, exp2, exp3, exp4, exp5, exp6, exp7)
            {
                //Validation
                if (exp1 == null)
                    throw new ArgumentNullException("exp1");

                if (exp2 == null)
                    throw new ArgumentNullException("exp2");

                if (exp3 == null)
                    throw new ArgumentNullException("exp3");

                if (exp4 == null)
                    throw new ArgumentNullException("exp4");

                if (exp4 == null)
                    throw new ArgumentNullException("exp4");

                if (exp5 == null)
                    throw new ArgumentNullException("exp5");

                if (exp6 == null)
                    throw new ArgumentNullException("exp6");

                if (exp7 == null)
                    throw new ArgumentNullException("exp7");


                this.func1 = exp1.Compile();
                this.func2 = exp2.Compile();
                this.func3 = exp3.Compile();
                this.func4 = exp4.Compile();
                this.func5 = exp5.Compile();
                this.func6 = exp6.Compile();
                this.func7 = exp7.Compile();
            }

            #endregion

            #region Overrides of EqualityHelper<T>

            public override bool Equals(T x, T y)
            {
                if (ReferenceEquals(x, y))
                    return true;

                if (x == null)
                    return false;

                if (y == null)
                    return false;

                bool result = true;
                result = result && EqualityComparer<T1>.Default.Equals(func1(x), func1(y));
                result = result && EqualityComparer<T2>.Default.Equals(func2(x), func2(y));
                result = result && EqualityComparer<T3>.Default.Equals(func3(x), func3(y));
                result = result && EqualityComparer<T4>.Default.Equals(func4(x), func4(y));
                result = result && EqualityComparer<T5>.Default.Equals(func5(x), func5(y));
                result = result && EqualityComparer<T6>.Default.Equals(func6(x), func6(y));
                result = result && EqualityComparer<T7>.Default.Equals(func7(x), func7(y));

                return result;
            }

            public override int GetHashCode(T t)
            {
                return HashCodeHelper.GetHashCode(
                    func1(t),
                    func2(t),
                    func3(t),
                    func4(t),
                    func5(t),
                    func6(t),
                    func7(t));
            }

            public override string GetToString(T t)
            {
                return ToStringFormat(
                    t,
                    func1(t),
                    func2(t),
                    func3(t),
                    func4(t),
                    func5(t),
                    func6(t),
                    func7(t));
            }

            #endregion
        }

        private class Helper<T1, T2, T3, T4, T5, T6, T7, T8> : EqualityHelper<T>
        {
            #region Fields

            private readonly Func<T, T1> func1;
            private readonly Func<T, T2> func2;
            private readonly Func<T, T3> func3;
            private readonly Func<T, T4> func4;
            private readonly Func<T, T5> func5;
            private readonly Func<T, T6> func6;
            private readonly Func<T, T7> func7;
            private readonly Func<T, T8> func8;

            #endregion

            #region Constructors

            public Helper(
                Expression<Func<T, T1>> exp1,
                Expression<Func<T, T2>> exp2,
                Expression<Func<T, T3>> exp3,
                Expression<Func<T, T4>> exp4,
                Expression<Func<T, T5>> exp5,
                Expression<Func<T, T6>> exp6,
                Expression<Func<T, T7>> exp7,
                Expression<Func<T, T8>> exp8)
                : base(exp1, exp2, exp3, exp4, exp5, exp6, exp7, exp8)
            {
                //Validation
                if (exp1 == null)
                    throw new ArgumentNullException("exp1");

                if (exp2 == null)
                    throw new ArgumentNullException("exp2");

                if (exp3 == null)
                    throw new ArgumentNullException("exp3");

                if (exp4 == null)
                    throw new ArgumentNullException("exp4");

                if (exp4 == null)
                    throw new ArgumentNullException("exp4");

                if (exp5 == null)
                    throw new ArgumentNullException("exp5");

                if (exp6 == null)
                    throw new ArgumentNullException("exp6");

                if (exp7 == null)
                    throw new ArgumentNullException("exp7");

                if (exp8 == null)
                    throw new ArgumentNullException("exp8");


                this.func1 = exp1.Compile();
                this.func2 = exp2.Compile();
                this.func3 = exp3.Compile();
                this.func4 = exp4.Compile();
                this.func5 = exp5.Compile();
                this.func6 = exp6.Compile();
                this.func7 = exp7.Compile();
                this.func8 = exp8.Compile();
            }

            #endregion

            #region Overrides of EqualityHelper<T>

            public override bool Equals(T x, T y)
            {
                if (ReferenceEquals(x, y))
                    return true;

                if (x == null)
                    return false;

                if (y == null)
                    return false;

                bool result = true;
                result = result && EqualityComparer<T1>.Default.Equals(func1(x), func1(y));
                result = result && EqualityComparer<T2>.Default.Equals(func2(x), func2(y));
                result = result && EqualityComparer<T3>.Default.Equals(func3(x), func3(y));
                result = result && EqualityComparer<T4>.Default.Equals(func4(x), func4(y));
                result = result && EqualityComparer<T5>.Default.Equals(func5(x), func5(y));
                result = result && EqualityComparer<T6>.Default.Equals(func6(x), func6(y));
                result = result && EqualityComparer<T7>.Default.Equals(func7(x), func7(y));
                result = result && EqualityComparer<T8>.Default.Equals(func8(x), func8(y));

                return result;
            }

            public override int GetHashCode(T t)
            {
                return HashCodeHelper.GetHashCode(
                    func1(t),
                    func2(t),
                    func3(t),
                    func4(t),
                    func5(t),
                    func6(t),
                    func7(t),
                    func8(t));
            }

            public override string GetToString(T t)
            {
                return ToStringFormat(
                    t,
                    func1(t),
                    func2(t),
                    func3(t),
                    func4(t),
                    func5(t),
                    func6(t),
                    func7(t),
                    func8(t));
            }

            #endregion
        }
    }
}

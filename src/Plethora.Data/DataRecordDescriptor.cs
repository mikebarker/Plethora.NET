using JetBrains.Annotations;
using Plethora.Collections;
using Plethora.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Plethora.Data
{
    /// <summary>
    /// A class which describes the fields of a <see cref="System.Data.IDataRecord"/>, based on .NET objects.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the DataRecord.</typeparam>
    public class DataRecordDescriptor<T>
    {
        private struct FieldDescriptor
        {
            public static FieldDescriptor Create<TField>(Func<T, TField> getValueFunc)
            {
                Func<T, object> getValueObjectFunc = t => (object)getValueFunc(t);

                var descriptor = new FieldDescriptor(typeof(TField), getValueObjectFunc);
                return descriptor;
            }

            private FieldDescriptor(Type type, Func<T, object> getValueFunc)
            {
                this.Type = type;
                this.GetValueFunc = getValueFunc;
            }

            public Type Type { get; }
            public Func<T, object> GetValueFunc { get; }
        }

        private readonly BidirectionalMap<string, int> nameToOrdinalMap = new BidirectionalMap<string, int>();
        private readonly List<FieldDescriptor> fieldDescriptors = new List<FieldDescriptor>();

        /// <summary>
        /// Adds a new field to the descriptor.
        /// </summary>
        /// <typeparam name="TField">The data type of the field.</typeparam>
        /// <param name="getValueExpression">The expression to retrieve the field's value from an item.</param>
        public void AddField<TField>([NotNull] Expression<Func<T, TField>> getValueExpression)
        {
            if (getValueExpression == null)
                throw new ArgumentNullException(nameof(getValueExpression));

            string name = ExpressionHelper.GetPropertyOrFieldName(getValueExpression);

            Func<T, TField> getValueFunc = getValueExpression.Compile();

            this.AddField(name, getValueFunc);
        }

        /// <summary>
        /// Adds a new field to the descriptor.
        /// </summary>
        /// <typeparam name="TField">The data type of the field.</typeparam>
        /// <param name="name">The name of the field.</param>
        /// <param name="getValueFunc">The callback function to retrieve the field's value from an item.</param>
        public void AddField<TField>([NotNull] string name, [NotNull] Func<T, TField> getValueFunc)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (getValueFunc == null)
                throw new ArgumentNullException(nameof(getValueFunc));

            if (this.nameToOrdinalMap.Contains(name))
                throw new ArgumentException(ResourceProvider.ArgAddingDuplicate(), nameof(name));


            int ordinal = this.fieldDescriptors.Count;

            this.nameToOrdinalMap.Add(name, ordinal);
            this.fieldDescriptors.Add(FieldDescriptor.Create(getValueFunc));
        }

        /// <summary>
        /// Gets the count of the number of fields.
        /// </summary>
        public int FieldCount => this.nameToOrdinalMap.Count;

        /// <summary>
        /// Gets the name of a field, given its ordinal position.
        /// </summary>
        /// <param name="i">The ordinal position of the field.</param>
        /// <returns>The name of the field.</returns>
        [NotNull]
        public string GetName(int i)
        {
            if (i > this.FieldCount - 1)
                throw new ArgumentOutOfRangeException(nameof(i), i, ResourceProvider.ArgMustBeBetween(nameof(i), "0", $"{nameof(FieldCount)}-1"));

            var name = this.nameToOrdinalMap.LookupReverse(i);
            return name;
        }


        /// <summary>
        /// Gets the ordinal position of a field, given its name.
        /// </summary>
        /// <param name="name">The name of the field.</param>
        /// <returns>The ordinal position of the field.</returns>
        public int GetOrdinal([NotNull] string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            var ordinal = this.nameToOrdinalMap.Lookup(name);
            return ordinal;
        }

        /// <summary>
        /// Gets the data type of a field, given its ordinal position.
        /// </summary>
        /// <param name="i">The ordinal position of the field.</param>
        /// <returns>The data type of the field.</returns>
        [NotNull]
        public Type GetType(int i)
        {
            if (i > this.FieldCount - 1)
                throw new ArgumentOutOfRangeException(nameof(i), i, ResourceProvider.ArgMustBeBetween(nameof(i), "0", $"{nameof(FieldCount)}-1"));

            FieldDescriptor fieldDescriptor = this.fieldDescriptors[i];
            Type type = fieldDescriptor.Type;
            return type;
        }

        /// <summary>
        /// Gets the value of a field for a item, given its ordinal position.
        /// </summary>
        /// <param name="item">The item for which the field's value is to be retrieved.</param>
        /// <param name="i">The ordinal position of the field.</param>
        /// <returns>The value of the field for the item.</returns>
        [CanBeNull]
        public object GetValue([NotNull] T item, int i)
        {
            if (item == null)
                throw new ArgumentNullException();

            if (i > this.FieldCount - 1)
                throw new ArgumentOutOfRangeException(nameof(i), i, ResourceProvider.ArgMustBeBetween(nameof(i), "0", $"{nameof(FieldCount)}-1"));

            FieldDescriptor fieldDescriptor = this.fieldDescriptors[i];
            Func<T, object> getValueFunc = fieldDescriptor.GetValueFunc;
            object value = getValueFunc(item);
            return value;
        }
    }
}

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Plethora.Data
{
    /// <summary>
    /// An <see cref="IDataReader"/> representation of a .NET list.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the list.</typeparam>
    /// <remarks>
    /// <example>
    /// In this example the customers list will be streamed to the database as a table parameter of the 'usp_InsertCustomers' stored procedure,
    /// with table parameter type containing the fields named "ID", "FamilyName", "GivenName", and "DateOfBirth":
    /// <code>
    /// <![CDATA[
    /// List<Person> customers = new List<Person>
    /// {
    ///     // ... SNIP ...
    /// }
    /// 
    /// DataRecordDescriptor<Person> descriptor = new DataRecordDescriptor<Person>();
    /// descriptor.AddField(dt => dt.ID);
    /// descriptor.AddField(dt => dt.FamilyName);
    /// descriptor.AddField(dt => dt.GivenName);
    /// descriptor.AddField(dt => dt.DateOfBirth);
    /// 
    /// DataReader<T> customersDataReader = new DataReader<T>(customers, descriptor);
    /// 
    /// using (SqlConnection connection = new SqlConnection(connectionString))
    /// {
    ///     connection.Open();
    /// 
    ///     // Configure the SqlCommand and SqlParameter.
    ///     SqlCommand insertCommand = new SqlCommand("usp_InsertCustomers", connection);
    ///     insertCommand.CommandType = CommandType.StoredProcedure;  
    ///     SqlParameter tvpParam = insertCommand.Parameters.AddWithValue("@tvpCustomers", customersDataReader);
    ///     tvpParam.SqlDbType = SqlDbType.Structured;  
    /// 
    ///     Execute the command.
    ///     insertCommand.ExecuteNonQuery();
    /// }
    /// ]]>
    /// </code>
    /// </example>
    /// <example>
    /// In this example the customers list will be streamed to the database and written directly into the dbo.Customers table, with
    /// the fields named "ID", "FamilyName", "GivenName", and "DateOfBirth":
    /// <code>
    /// <![CDATA[
    /// List<Person> customers = new List<Person>
    /// {
    ///     // ... SNIP ...
    /// }
    /// 
    /// DataRecordDescriptor<Person> descriptor = new DataRecordDescriptor<Person>();
    /// descriptor.AddField(dt => dt.ID);
    /// descriptor.AddField(dt => dt.FamilyName);
    /// descriptor.AddField(dt => dt.GivenName);
    /// descriptor.AddField(dt => dt.DateOfBirth);
    /// 
    /// DataReader<T> customersDataReader = new DataReader<T>(customers, descriptor);
    /// 
    /// using (SqlConnection connection = new SqlConnection(connectionString))
    /// {
    ///     connection.Open();
    ///     using (SqlBulkCopy cpy = new SqlBulkCopy(connection))
    ///     {
    ///         cpy.DestinationTableName = "dbo.Customers";
    ///         cpy.WriteToServer(customersDataReader);
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    /// </remarks>
    /// <seealso cref="DataRecordDescriptor{T}"/>
    public class DataReader<T> : IDataReader
    {
        private DataRecordDescriptor<T> descriptor;
        private IEnumerable<T> collection;
        private IEnumerator<T> enumerator;
        private bool isClosed = false;

        /// <summary>
        /// Initialise a new instance of the <see cref="DataReader"/> class.
        /// </summary>
        /// <param name="collection">The collection of elements.</param>
        /// <param name="descriptor">The <see cref="DataRecordDescriptor{T}"/> which describes the fields of this DataReader.</param>
        public DataReader(
            [NotNull] IEnumerable<T> collection,
            [NotNull] DataRecordDescriptor<T> descriptor)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));


            this.collection = collection;
            this.descriptor = descriptor;
        }

        #region Implementation of IDisposable

        /// <inheritdoc/>
        void IDisposable.Dispose()
        {
            if (!this.isClosed)
            {
                this.Close();
            }
        }

        #endregion

        #region Implementation of IDataRecord

        /// <inheritdoc/>
        public object this[int i]
        {
            get
            {
                if (this.isClosed)
                    throw new InvalidOperationException("IDataRecord is closed.");

                object value = this.GetValue(i);
                return value;
            }
        }

        /// <inheritdoc/>
        public object this[string name]
        {
            get
            {
                if (this.isClosed)
                    throw new InvalidOperationException("IDataRecord is closed.");

                int i = this.GetOrdinal(name);
                object value = this.GetValue(i);
                return value;
            }
        }

        /// <inheritdoc/>
        public int FieldCount => this.descriptor.FieldCount;

        /// <inheritdoc/>
        public string GetName(int i)
        {
            if (this.isClosed)
                throw new InvalidOperationException("IDataRecord is closed.");

            return this.descriptor.GetName(i);
        }

        /// <inheritdoc/>
        public int GetOrdinal(string name)
        {
            if (this.isClosed)
                throw new InvalidOperationException("IDataRecord is closed.");

            return this.descriptor.GetOrdinal(name);
        }

        /// <inheritdoc/>
        public object GetValue(int i)
        {
            if (this.isClosed)
                throw new InvalidOperationException("IDataRecord is closed.");

            if (i > this.FieldCount - 1)
                throw new ArgumentOutOfRangeException(nameof(i), i, ResourceProvider.ArgMustBeBetween(nameof(i), "0", $"{nameof(FieldCount)}-1"));

            if (this.enumerator == null)
                throw new InvalidOperationException(ResourceProvider.EnumeratorOpCantHappen());

            T element = this.enumerator.Current;
            object value = this.descriptor.GetValue(element, i);
            return value;
        }

        /// <inheritdoc/>
        int IDataRecord.GetValues(object[] values)
        {
            if (this.isClosed)
                throw new InvalidOperationException("IDataRecord is closed.");

            if (values.Length < this.descriptor.FieldCount)
                throw new ArgumentException("The array must be at least as long as the number of fields.", nameof(values));

            if (this.enumerator == null)
                throw new InvalidOperationException(ResourceProvider.EnumeratorOpCantHappen());

            T element = this.enumerator.Current;
            for (int i = 0; i < this.descriptor.FieldCount; i++)
            {
                object value = this.descriptor.GetValue(element, i);
                values[i] = value;
            }

            return this.descriptor.FieldCount;
        }

        /// <inheritdoc/>
        Type IDataRecord.GetFieldType(int i)
        {
            Type fieldType = this.descriptor.GetType(i);
            return fieldType;
        }

        /// <inheritdoc/>
        string IDataRecord.GetDataTypeName(int i)
        {
            Type fieldType = this.descriptor.GetType(i);
            string name = fieldType.Name;
            return name;
        }

        /// <inheritdoc/>
        bool IDataRecord.IsDBNull(int i)
        {
            var obj = ((IDataRecord)this).GetValue(i);
            return (obj == null);
        }


        /// <inheritdoc/>
        bool IDataRecord.GetBoolean(int i)
        {
            var obj = ((IDataRecord)this).GetValue(i);
            var result = (bool)obj;
            return result;
        }

        /// <inheritdoc/>
        byte IDataRecord.GetByte(int i)
        {
            var obj = ((IDataRecord)this).GetValue(i);
            var result = (byte)obj;
            return result;
        }

        /// <inheritdoc/>
        char IDataRecord.GetChar(int i)
        {
            var obj = ((IDataRecord)this).GetValue(i);
            var result = (char)obj;
            return result;
        }

        /// <inheritdoc/>
        DateTime IDataRecord.GetDateTime(int i)
        {
            var obj = ((IDataRecord)this).GetValue(i);
            var result = (DateTime)obj;
            return result;
        }

        /// <inheritdoc/>
        decimal IDataRecord.GetDecimal(int i)
        {
            var obj = ((IDataRecord)this).GetValue(i);
            var result = (decimal)obj;
            return result;
        }

        /// <inheritdoc/>
        double IDataRecord.GetDouble(int i)
        {
            var obj = ((IDataRecord)this).GetValue(i);
            var result = (double)obj;
            return result;
        }

        /// <inheritdoc/>
        float IDataRecord.GetFloat(int i)
        {
            var obj = ((IDataRecord)this).GetValue(i);
            var result = (float)obj;
            return result;
        }

        /// <inheritdoc/>
        Guid IDataRecord.GetGuid(int i)
        {
            var obj = ((IDataRecord)this).GetValue(i);
            var result = (Guid)obj;
            return result;
        }

        /// <inheritdoc/>
        short IDataRecord.GetInt16(int i)
        {
            var obj = ((IDataRecord)this).GetValue(i);
            var result = (short)obj;
            return result;
        }

        /// <inheritdoc/>
        int IDataRecord.GetInt32(int i)
        {
            var obj = ((IDataRecord)this).GetValue(i);
            var result = (int)obj;
            return result;
        }

        /// <inheritdoc/>
        long IDataRecord.GetInt64(int i)
        {
            var obj = ((IDataRecord)this).GetValue(i);
            var result = (long)obj;
            return result;
        }

        /// <inheritdoc/>
        string IDataRecord.GetString(int i)
        {
            var obj = ((IDataRecord)this).GetValue(i);
            var result = (string)obj;
            return result;
        }


        /// <inheritdoc/>
        long IDataRecord.GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc/>
        long IDataRecord.GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc/>
        IDataReader IDataRecord.GetData(int i)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region Implementation of IDataReader

        /// <inheritdoc/>
        int IDataReader.Depth => 1;

        /// <inheritdoc/>
        public bool IsClosed => this.isClosed;

        /// <inheritdoc/>
        int IDataReader.RecordsAffected => this.collection.Count();

        /// <inheritdoc/>
        public void Close()
        {
            if (this.isClosed)
                throw new InvalidOperationException("IDataRecord is closed.");

            this.descriptor = null;
            this.collection = null;
            this.enumerator?.Dispose();
            this.enumerator = null;
            this.isClosed = true;
        }

        /// <inheritdoc/>
        DataTable IDataReader.GetSchemaTable()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc/>
        bool IDataReader.NextResult()
        {
            if (this.isClosed)
                throw new InvalidOperationException("IDataRecord is closed.");

            this.enumerator?.Dispose();
            this.enumerator = null;
            return false;
        }

        /// <inheritdoc/>
        public bool Read()
        {
            if (this.isClosed)
                throw new InvalidOperationException("IDataRecord is closed.");

            if (this.enumerator == null)
            {
                this.enumerator = collection.GetEnumerator();
            }

            var result = this.enumerator.MoveNext();
            return result;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

using JetBrains.Annotations;

using Plethora.Data;
using Plethora.Logging;

namespace Plethora.Workflow.DAL
{
    public class SqlWorkAccessor : IWorkAccessor
    {
        private static readonly ILogger log = Logger.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly string connectionString;

        private readonly object staticDataLock = new object();
        private WorkflowStatic staticData = null;

        public SqlWorkAccessor(string connectionString)
        {
            if (connectionString == null)
                throw new ArgumentNullException(nameof(connectionString));


            this.connectionString = connectionString;
        }

        public long InitiateWorkflow(Guid systemId, string externalId, string workflowState, string description, IDictionary<string, string> data)
        {
            WorkflowStatic workflowStatic = this.StaticData;

            long initialBusinessStateId;
            try
            {
                initialBusinessStateId = workflowStatic.GetBusinessStateIdByName(workflowState);
            }
            catch (Exception ex)
            {
                log.Error(ex, ex.Message);
                throw;
            }

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Key", typeof(string));
            dataTable.Columns.Add("Value", typeof(string));

            foreach (KeyValuePair<string, string> pair in data)
            {
                dataTable.Rows.Add(pair.Key, pair.Value);
            }

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "work.InitiateWorkflow";
                    command.CommandType = CommandType.StoredProcedure;

                    SqlParameter externalIdParameter = command.Parameters.Add("@externalId", SqlDbType.VarChar);
                    externalIdParameter.Direction = ParameterDirection.Input;
                    externalIdParameter.Value = externalId;

                    SqlParameter descriptionParameter = command.Parameters.Add("@description", SqlDbType.VarChar);
                    descriptionParameter.Direction = ParameterDirection.Input;
                    descriptionParameter.Value = description;

                    SqlParameter initialBusinessStateIdParameter = command.Parameters.Add("@initialBusinessStateId", SqlDbType.Int);
                    initialBusinessStateIdParameter.Direction = ParameterDirection.Input;
                    initialBusinessStateIdParameter.Value = initialBusinessStateId;

                    SqlParameter dataParameter = command.Parameters.Add("@data", SqlDbType.Structured);
                    dataParameter.Direction = ParameterDirection.Input;
                    dataParameter.Value = dataTable;

                    SqlParameter workflowIdParameter = command.Parameters.Add("@workflowId", SqlDbType.BigInt);
                    workflowIdParameter.Direction = ParameterDirection.Output;


                    command.ExecuteNonQuery();


                    object workflowIdObj = workflowIdParameter.Value;
                    long workflowId = (long)workflowIdObj;

                    return workflowId;
                }
            }
        }

        public void CreateWorkItems(Guid systemId, IEnumerable<WorkItemLite> workItems)
        {
            WorkflowStatic workflowStatic = this.StaticData;

            DataTable workItemsTable = new DataTable();
            workItemsTable.Columns.Add("WorkflowId", typeof(long));
            workItemsTable.Columns.Add("Sequence", typeof(string));
            workItemsTable.Columns.Add("InitialBusinessStateId", typeof(long));
            workItemsTable.Columns.Add("DataGroupId", typeof(int));

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("DataGroupId", typeof(int));
            dataTable.Columns.Add("Key", typeof(string));
            dataTable.Columns.Add("Value", typeof(string));

            int dataGroupId = 0;
            foreach (WorkItemLite workItem in workItems)
            {

                long initialBusinessStateId;
                try
                {
                    initialBusinessStateId = workflowStatic.GetBusinessStateIdByName(workItem.State);
                }
                catch (Exception ex)
                {
                    log.Error(ex, ex.Message);
                    throw;
                }


                workItemsTable.Rows.Add(workItem.WorkflowId, workItem.Sequence, initialBusinessStateId, dataGroupId);

                foreach (KeyValuePair<string, object> pair in workItem.Data)
                {
                    string value = (pair.Value == null)
                        ? null
                        : pair.Value.ToString();

                    dataTable.Rows.Add(dataGroupId, pair.Key, value);
                }

                dataGroupId++;
            }


            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "work.CreateWorkItems";
                    command.CommandType = CommandType.StoredProcedure;

                    SqlParameter workItemsParameter = command.Parameters.Add("@workItems", SqlDbType.Structured);
                    workItemsParameter.Direction = ParameterDirection.Input;
                    workItemsParameter.Value = workItemsTable;

                    SqlParameter dataParameter = command.Parameters.Add("@data", SqlDbType.Structured);
                    dataParameter.Direction = ParameterDirection.Input;
                    dataParameter.Value = dataTable;


                    command.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<WorkItemLite> TryGetWork(Guid systemId, IEnumerable<string> businessStates, int maxWorkItems)
        {
            WorkflowStatic workflowStatic = this.StaticData;

            DataTable businessStateIdsTable = new DataTable();
            businessStateIdsTable.Columns.Add("Value", typeof(int));
            foreach (string businessState in businessStates)
            {
                long businessStateId;
                try
                {
                    businessStateId = workflowStatic.GetBusinessStateIdByName(businessState);
                }
                catch (Exception ex)
                {
                    log.Error(ex, ex.Message);
                    throw;
                }


                businessStateIdsTable.Rows.Add((int)businessStateId);
            }


            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "work.TryGetWork";
                    command.CommandType = CommandType.StoredProcedure;

                    SqlParameter systemIdParameter = command.Parameters.Add("@systemId", SqlDbType.UniqueIdentifier);
                    systemIdParameter.Direction = ParameterDirection.Input;
                    systemIdParameter.Value = systemId;

                    SqlParameter workflowStateIdsParameter = command.Parameters.Add("@workflowStateIds", SqlDbType.Structured);
                    workflowStateIdsParameter.Direction = ParameterDirection.Input;
                    workflowStateIdsParameter.Value = businessStateIdsTable;

                    SqlParameter maxWorkItemsParameter = command.Parameters.Add("@maxWorkItems", SqlDbType.Int);
                    maxWorkItemsParameter.Direction = ParameterDirection.Input;
                    maxWorkItemsParameter.Value = maxWorkItems;


                    DateTime lockAcquiredUtc = DateTime.UtcNow;

                    using (var reader = command.ExecuteReaderWithRetry())
                    {
                        int workflowIdOrdinal = reader.GetOrdinal("WorkflowId");
                        int workItemIdOrdinal = reader.GetOrdinal("WorkItemId");
                        int sequenceOrdinal = reader.GetOrdinal("Sequence");
                        int initialBusinessStateIdOrdinal = reader.GetOrdinal("InitialBusinessStateId");
                        int lockTimeoutUtcOrdinal = reader.GetOrdinal("LockTimeoutUtc");
                        int dataIdOrdinal = reader.GetOrdinal("DataId");

                        List<WorkItemRaw> workItemRawList = new List<WorkItemRaw>();
                        while (reader.Read())
                        {
                            long workflowId = reader.GetAs<long>(workflowIdOrdinal);
                            long workItemId = reader.GetAs<long>(workItemIdOrdinal);
                            string sequence = reader.GetAs<string>(sequenceOrdinal);
                            long initialBusinessStateId = reader.GetAs<long>(initialBusinessStateIdOrdinal);
                            DateTime lockTimeoutUtc = reader.GetAs<DateTime>(lockTimeoutUtcOrdinal);
                            long? dataId = reader.GetAs<long?>(dataIdOrdinal);

                            WorkItemRaw workItemRaw = new WorkItemRaw(
                                workflowId,
                                workItemId,
                                sequence,
                                initialBusinessStateId,
                                lockTimeoutUtc,
                                dataId);

                            workItemRawList.Add(workItemRaw);
                        }


                        // Data
                        reader.NextResult();

                        dataIdOrdinal = reader.GetOrdinal("DataId");
                        int keyOrdinal = reader.GetOrdinal("Key");
                        int valueOrdinal = reader.GetOrdinal("Value");

                        List<WorkItemDataRaw> dataRawList = new List<WorkItemDataRaw>();
                        while (reader.Read())
                        {
                            long dataId = reader.GetAs<long>(dataIdOrdinal);
                            string key = reader.GetAs<string>(keyOrdinal);
                            string value = reader.GetAs<string>(valueOrdinal);

                            WorkItemDataRaw dataRaw = new WorkItemDataRaw(
                                dataId,
                                key,
                                value);

                            dataRawList.Add(dataRaw);
                        }

                        Dictionary<long, IEnumerable<WorkItemDataRaw>> dataRawGrouped = dataRawList
                            .GroupBy(raw => raw.dataId)
                            .ToDictionary(group => group.Key, group => group.Select(r => r));

                        List<WorkItemLite> workItemLites = new List<WorkItemLite>();
                        foreach (WorkItemRaw workItemRaw in workItemRawList)
                        {
                            Dictionary<string, object> dataDictionary = new Dictionary<string, object>();
                            if (workItemRaw.dataId != null)
                            {
                                IEnumerable<WorkItemDataRaw> dataRawEnumerable;
                                if (dataRawGrouped.TryGetValue(workItemRaw.dataId.Value, out dataRawEnumerable))
                                {
                                    foreach (WorkItemDataRaw dataRaw in dataRawEnumerable)
                                    {
                                        dataDictionary.Add(dataRaw.key, dataRaw.value);
                                    }
                                }
                            }

                            string initialBusinessStateName =
                                workflowStatic.GetBusinessStateNameById(workItemRaw.initialBusinessStateId);

                            WorkItemLite workItemLite = new WorkItemLite(
                                workItemRaw.workflowId,
                                workItemRaw.workItemId,
                                workItemRaw.sequence,
                                initialBusinessStateName,
                                dataDictionary,
                                lockAcquiredUtc,
                                workItemRaw.lockTimeoutUtc);

                            workItemLites.Add(workItemLite);
                        }

                        return workItemLites;
                    }
                }
            }
        }

        public bool TryGetWorkItemLock(Guid systemId, long workItemId, out DateTime lockTimeoutUtc)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "work.TryGetWorkItemLock";
                    command.CommandType = CommandType.StoredProcedure;

                    SqlParameter systemIdParameter = command.Parameters.Add("@systemId", SqlDbType.UniqueIdentifier);
                    systemIdParameter.Direction = ParameterDirection.Input;
                    systemIdParameter.Value = systemId;

                    SqlParameter workflowStateIdsParameter = command.Parameters.Add("@workItemId", SqlDbType.BigInt);
                    workflowStateIdsParameter.Direction = ParameterDirection.Input;
                    workflowStateIdsParameter.Value = workItemId;

                    SqlParameter resultParameter = command.Parameters.Add("@result", SqlDbType.Bit);
                    resultParameter.Direction = ParameterDirection.Output;

                    SqlParameter timeoutParameter = command.Parameters.Add("@timeout", SqlDbType.DateTime);
                    timeoutParameter.Direction = ParameterDirection.Output;


                    command.ExecuteNonQuery();

                    object resultObj = resultParameter.Value;
                    bool result = (bool)resultObj;
                    if (result)
                    {
                        object timeoutObj = timeoutParameter.Value;

                        lockTimeoutUtc = (DateTime)timeoutObj;
                        return true;
                    }
                    else
                    {
                        lockTimeoutUtc = default(DateTime);
                        return false;
                    }
                }
            }
        }

        public bool TryRefreshWorkItemLock(Guid systemId, long workItemId, out DateTime lockTimeoutUtc)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "work.TryRefreshWorkItemLock";
                    command.CommandType = CommandType.StoredProcedure;

                    SqlParameter systemIdParameter = command.Parameters.Add("@systemId", SqlDbType.UniqueIdentifier);
                    systemIdParameter.Direction = ParameterDirection.Input;
                    systemIdParameter.Value = systemId;

                    SqlParameter workflowStateIdsParameter = command.Parameters.Add("@workItemId", SqlDbType.BigInt);
                    workflowStateIdsParameter.Direction = ParameterDirection.Input;
                    workflowStateIdsParameter.Value = workItemId;

                    SqlParameter resultParameter = command.Parameters.Add("@result", SqlDbType.Bit);
                    resultParameter.Direction = ParameterDirection.Output;

                    SqlParameter timeoutParameter = command.Parameters.Add("@timeout", SqlDbType.DateTime);
                    timeoutParameter.Direction = ParameterDirection.Output;


                    command.ExecuteNonQuery();

                    object resultObj = resultParameter.Value;
                    bool result = (bool)resultObj;
                    if (result)
                    {
                        object timeoutObj = timeoutParameter.Value;

                        lockTimeoutUtc = (DateTime)timeoutObj;
                        return true;
                    }
                    else
                    {
                        lockTimeoutUtc = default(DateTime);
                        return false;
                    }
                }
            }
        }

        public void CompleteWorkItem(Guid systemId, long workItemId, string finalWorkflowState, int startSequenceAt)
        {
            WorkflowStatic workflowStatic = this.StaticData;
            long finalBusinessStateId = workflowStatic.GetBusinessStateIdByName(finalWorkflowState);

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "work.MarkAsComplete";
                    command.CommandType = CommandType.StoredProcedure;

                    SqlParameter systemIdParameter = command.Parameters.Add("@systemId", SqlDbType.UniqueIdentifier);
                    systemIdParameter.Direction = ParameterDirection.Input;
                    systemIdParameter.Value = systemId;

                    SqlParameter workflowStateIdsParameter = command.Parameters.Add("@workItemId", SqlDbType.BigInt);
                    workflowStateIdsParameter.Direction = ParameterDirection.Input;
                    workflowStateIdsParameter.Value = workItemId;

                    SqlParameter finalBusinessStateIdParameter = command.Parameters.Add("@finalBusinessStateId", SqlDbType.Int);
                    finalBusinessStateIdParameter.Direction = ParameterDirection.Input;
                    finalBusinessStateIdParameter.Value = finalBusinessStateId;

                    SqlParameter startSequenceAtParameter = command.Parameters.Add("@startSequenceAt", SqlDbType.Int);
                    startSequenceAtParameter.Direction = ParameterDirection.Input;
                    startSequenceAtParameter.Value = startSequenceAt;

                    SqlParameter resultParameter = command.Parameters.Add("@result", SqlDbType.Bit);
                    resultParameter.Direction = ParameterDirection.Output;


                    command.ExecuteNonQuery();

                    // object resultObj = resultParameter.Value;
                    // bool result = (bool)resultObj;
                    // return result;
                }
            }
        }

        public void FailWorkItem(Guid systemId, long workItemId)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "work.MarkAsFailed";
                    command.CommandType = CommandType.StoredProcedure;

                    SqlParameter systemIdParameter = command.Parameters.Add("@systemId", SqlDbType.UniqueIdentifier);
                    systemIdParameter.Direction = ParameterDirection.Input;
                    systemIdParameter.Value = systemId;

                    SqlParameter workflowStateIdsParameter = command.Parameters.Add("@workItemId", SqlDbType.BigInt);
                    workflowStateIdsParameter.Direction = ParameterDirection.Input;
                    workflowStateIdsParameter.Value = workItemId;

                    SqlParameter resultParameter = command.Parameters.Add("@result", SqlDbType.Bit);
                    resultParameter.Direction = ParameterDirection.Output;


                    command.ExecuteNonQuery();

                    // object resultObj = resultParameter.Value;
                    // bool result = (bool)resultObj;
                    // return result;
                }
            }
        }


        [NotNull]
        private WorkflowStatic StaticData
        {
            get
            {
                if (this.staticData == null)
                {
                    lock (this.staticDataLock)
                    {
                        if (this.staticData == null)
                        {
                            this.staticData = this.GetStaticData();
                        }
                    }
                }

                return this.staticData;
            }
        }

        [NotNull]
        private WorkflowStatic GetStaticData()
        {
            List<BusinessStateRaw> businessStateRawList = new List<BusinessStateRaw>();

            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "work.GetAllWorkflowStatic";
                        command.CommandType = CommandType.StoredProcedure;


                        using (var reader = command.ExecuteReaderWithRetry())
                        {
                            int businessStateIdOrdinal = reader.GetOrdinal("BusinessStateId");
                            int nameOrdinal = reader.GetOrdinal("Name");
                            int timeoutMillisecondOrdinal = reader.GetOrdinal("TimeoutMillisecond");
                            int maxRetryCountOrdinal = reader.GetOrdinal("MaxRetryCount");
                            int descriptionOrdinal = reader.GetOrdinal("Description");

                            while (reader.Read())
                            {
                                long businessStateId = reader.GetAs<long>(businessStateIdOrdinal);
                                string name = reader.GetAs<string>(nameOrdinal);
                                int? timeoutMillisecond = reader.GetAs<int?>(timeoutMillisecondOrdinal);
                                int? maxRetryCount = reader.GetAs<int?>(maxRetryCountOrdinal);
                                string description = reader.GetAs<string>(descriptionOrdinal);

                                BusinessStateRaw businessStateRaw = new BusinessStateRaw(
                                    businessStateId,
                                    name,
                                    timeoutMillisecond,
                                    maxRetryCount,
                                    description);

                                log.Verbose("Acquired BusinessState [ID= {0}, Name= {1}, TimeoutMillisecond= {2}, MaxRetryCount= {3}, Description= {4}]",
                                    businessStateRaw.BusinessStateId,
                                    businessStateRaw.Name,
                                    businessStateRaw.TimeoutMillisecond,
                                    businessStateRaw.MaxRetryCount,
                                    businessStateRaw.Description);

                                businessStateRawList.Add(businessStateRaw);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error during GetStaticData.");
                throw;
            }

            return new WorkflowStatic(businessStateRawList);
        }


        private class WorkflowStatic
        {
            private readonly Dictionary<long, BusinessStateRaw> businessStatesById;
            private readonly Dictionary<string, BusinessStateRaw> businessStatesByName;

            public WorkflowStatic(IEnumerable<BusinessStateRaw> businessStates)
            {
                this.businessStatesById = new Dictionary<long, BusinessStateRaw>();
                this.businessStatesByName = new Dictionary<string, BusinessStateRaw>();

                foreach (BusinessStateRaw businessState in businessStates)
                {
                    this.businessStatesById.Add(businessState.BusinessStateId, businessState);
                    this.businessStatesByName.Add(businessState.Name, businessState);
                }
            }

            public bool TryGetBusinessStateIdByName(string name, out long businessStateId)
            {
                BusinessStateRaw businessState;
                bool result = this.businessStatesByName.TryGetValue(name, out businessState);

                if (result)
                {
                    businessStateId = businessState.BusinessStateId;
                    return true;
                }
                else
                {
                    businessStateId = default(long);
                    return false;
                }
            }

            public long GetBusinessStateIdByName(string name)
            {
                long businessStateId;
                bool result = this.TryGetBusinessStateIdByName(name, out businessStateId);

                if (!result)
                    throw new ArgumentException(string.Format("Unknown BusinessState [name = {0}]", name));

                return businessStateId;
            }

            public bool TryGetBusinessStateNameById(long businessStateId, out string name)
            {
                BusinessStateRaw businessState;
                bool result = this.businessStatesById.TryGetValue(businessStateId, out businessState);

                if (result)
                {
                    name = businessState.Name;
                    return true;
                }
                else
                {
                    name = default(string);
                    return false;
                }
            }

            public string GetBusinessStateNameById(long businessStateId)
            {
                string name;
                bool result = this.TryGetBusinessStateNameById(businessStateId, out name);

                if (!result)
                    throw new ArgumentException(string.Format("Unknown BusinessState [ID = {0}]", businessStateId));

                return name;
            }
        }


        private class BusinessStateRaw
        {
            public readonly long BusinessStateId;
            public readonly string Name;
            public readonly int? TimeoutMillisecond;
            public readonly int? MaxRetryCount;
            public readonly string Description;

            public BusinessStateRaw(long businessStateId, string name, int? timeoutMillisecond, int? maxRetryCount, string description)
            {
                this.BusinessStateId = businessStateId;
                this.Name = name;
                this.TimeoutMillisecond = timeoutMillisecond;
                this.MaxRetryCount = maxRetryCount;
                this.Description = description;
            }
        }

        private class WorkItemRaw
        {
            public readonly long workflowId;
            public readonly long workItemId;
            public readonly string sequence;
            public readonly long initialBusinessStateId;
            public readonly DateTime lockTimeoutUtc;
            public readonly long? dataId;

            public WorkItemRaw(long workflowId, long workItemId, string sequence, long initialBusinessStateId, DateTime lockTimeoutUtc, long? dataId)
            {
                this.workflowId = workflowId;
                this.workItemId = workItemId;
                this.sequence = sequence;
                this.initialBusinessStateId = initialBusinessStateId;
                this.lockTimeoutUtc = lockTimeoutUtc;
                this.dataId = dataId;
            }
        }

        private class WorkItemDataRaw
        {
            public readonly long dataId;
            public readonly string key;
            public readonly string value;

            public WorkItemDataRaw(long dataId, string key, string value)
            {
                this.dataId = dataId;
                this.key = key;
                this.value = value;
            }
        }
    }
}

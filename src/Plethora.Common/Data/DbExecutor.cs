using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Transactions;

namespace Plethora.Data
{
    public static class DbExecutor
    {
        private class Config
        {
            public int DefaultRetryCount { get; private set; }
            public Dictionary<string, string> SubstituteText { get; private set; }
            public Dictionary<string, int> SubstituteTimeout { get; private set; }
            public Dictionary<string, int> RetryCounts { get; private set; }

            public Config()
                : this(3, new Dictionary<string, string>(), new Dictionary<string, int>(), new Dictionary<string, int>())
            {
            }

            public Config(
                int defaultRetryCount,
                Dictionary<string, string> substituteText,
                Dictionary<string, int> substituteTimeout,
                Dictionary<string, int> retryCounts)
            {
                this.DefaultRetryCount = defaultRetryCount;
                this.SubstituteText = substituteText;
                this.SubstituteTimeout = substituteTimeout;
                this.RetryCounts = retryCounts;
            }
        }

        private static volatile Config config = new Config();
        private static string configFile;
        private static FileSystemWatcher configFileWatcher;

        #region RetryCommand Event

        /// <summary>
        /// Raised when a command is re-attempted.
        /// </summary>
        public static event DbCommandRetryEventHandler RetryCommand;

        /// <summary>
        /// Raises the <see cref="RetryCommand"/> event.
        /// </summary>
        private static void OnRetryCommand(DbCommandRetryEventArgs e)
        {
            var handler = RetryCommand;
            if (handler != null)
                handler(typeof(DbExecutor), e);
        }

        #endregion

        /// <summary>
        /// Gets and sets the <see cref="IExceptionTester"/> used to test if <see cref="IDbCommand"/>
        /// should be re-tried.
        /// </summary>
        public static IExceptionTester ExceptionTester { get; set; }

        /// <summary>
        /// Gets and sets the config file containing the data to allow provide overrides to the 
        /// standard database behaviour.
        /// </summary>
        /// <remarks>
        /// This config file will be monitored for changes, and the config reloaded when required.
        /// </remarks>
        public static string ConfigFile
        {
            get { return configFile; }
            set
            {
                if (string.Equals(configFile, value))
                    return;

                if (configFile != null)
                {
                    configFileWatcher.EnableRaisingEvents = false;

                    configFileWatcher.Deleted -= ConfigFileChanged;
                    configFileWatcher.Changed -= ConfigFileChanged;
                    configFileWatcher.Created -= ConfigFileChanged;

                    configFileWatcher.Dispose();
                    configFileWatcher = null;
                }

                configFile = value;

                if (configFile == null)
                {
                    ResetConfig();
                }
                else
                {
                    string directory = Path.GetDirectoryName(configFile) ?? string.Empty;
                    string fileName = Path.GetFileName(configFile);

                    configFileWatcher = new FileSystemWatcher(directory, fileName);
                    configFileWatcher.Created += ConfigFileChanged;
                    configFileWatcher.Changed += ConfigFileChanged;
                    configFileWatcher.Deleted += ConfigFileChanged;

                    ConfigFileChanged(null, new FileSystemEventArgs(WatcherChangeTypes.Changed, directory, fileName));

                    configFileWatcher.EnableRaisingEvents = true;
                }
            }
        }

        #region Public Extension Methods

        public static int ExecuteNonQueryWithRetry(this IDbCommand command)
        {
            return ExecuteWithRetry(command, cmd => cmd.ExecuteNonQuery());
        }

        public static object ExecuteScalarWithRetry(this IDbCommand command)
        {
            return ExecuteWithRetry(command, cmd => cmd.ExecuteScalar());
        }

        public static IDataReader ExecuteReaderWithRetry(this IDbCommand command)
        {
            return ExecuteWithRetry(command, cmd => cmd.ExecuteReader());
        }

        public static IDataReader ExecuteReaderWithRetry(this IDbCommand command, CommandBehavior commandBehavior)
        {
            return ExecuteWithRetry(command, cmd => cmd.ExecuteReader(commandBehavior));
        }

        #endregion

        private static T ExecuteWithRetry<T>(IDbCommand command, Func<IDbCommand, T> executeAction)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            if (command.CommandText == null)
                throw new InvalidOperationException("The command object does not have its command text property set.");


            //Keep an atomic local copy of the config, to prevent changes during execution.
            Config local_config = config;

            string commandText;
            if (local_config.SubstituteText.TryGetValue(command.CommandText, out commandText))
            {
                command.CommandText = commandText;
            }

            int timeout;
            if (local_config.SubstituteTimeout.TryGetValue(command.CommandText, out timeout))
            {
                command.CommandTimeout = timeout;
            }

            int retryCount = 1;
            IExceptionTester exceptionTester = null;

            //The retry logic can only be utilised if not already inside a transaction.
            if ((Transaction.Current == null) && (command.Transaction == null))
            {
                if (!local_config.RetryCounts.TryGetValue(command.CommandText, out retryCount))
                {
                    retryCount = local_config.DefaultRetryCount;
                }

                exceptionTester = ExceptionTester;
            }

            int i = 0;
            while (i < retryCount)
            {
                try
                {
                    T rtn = executeAction(command);
                    return rtn;
                }
                catch (Exception ex)
                {
                    TimeSpan retryWaitTime;

                    if ((i < (retryCount - 1)) &&
                        (exceptionTester != null) &&
                        (exceptionTester.TestForRetry(ex, out retryWaitTime)))
                    {
                        DbCommandRetryEventArgs e = new DbCommandRetryEventArgs(command, ex);
                        OnRetryCommand(e);
                        if (e.Cancel)
                            throw;

                        //Allow the re-try
                        Thread.Sleep(retryWaitTime);

                        //Attempt to re-open the connection if required.
                        if (command.Connection.State == ConnectionState.Closed)
                        {
                            command.Connection.Open();
                        }
                    }
                    else
                    {
                        throw;
                    }
                }

                i++;
            }

            //Should never occur. Either the code above will throw or return with-in the loop, or at worst throw on final execution
            return default(T);
        }

        private static void ResetConfig()
        {
            config = new Config();
        }

        private static void ConfigFileChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Deleted)
            {
                ResetConfig();
                return;
            }

            var configFilePath = e.FullPath;

            //Read the file and reset the dictionaries.
            try
            {
                DbExecutorConfig executorConfig;
                using (StreamReader sr = new StreamReader(configFilePath))
                {
                    executorConfig = DbExecutorConfigHelper.ReadConfig(sr);
                }

                int newDefaultRetryCount = executorConfig.defaultRetryCount;
                Dictionary<string, string> newSubstituteText = executorConfig.redirections.ToDictionary(r => r.commandText, r => r.substitute);
                Dictionary<string, int> newSubstituteTimeout = executorConfig.timeouts.ToDictionary(r => r.commandText, r => r.timeoutSec);
                Dictionary<string, int> newRetryCounts = executorConfig.retries.ToDictionary(r => r.commandText, r => r.retryCount);

                config = new Config(
                    newDefaultRetryCount,
                    newSubstituteText,
                    newSubstituteTimeout,
                    newRetryCounts);
            }
            catch (Exception)
            {
                ResetConfig();
            }
        }
    }
}
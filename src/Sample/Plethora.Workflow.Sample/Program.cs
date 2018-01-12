using System;
using System.Collections.Generic;

using Plethora.Logging;
using Plethora.Logging.log4net;
using Plethora.Workflow.DAL;

namespace Plethora.Workflow.Sample
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Logger.LoggerProvider = new log4netLoggerProvider();

            const string connectionString = @"Server=VAIO\SQLEXPRESS;Database=Workflow;Trusted_Connection=True;";


            ITrigger trigger = new TimerTrigger(5000);
            IWorkAccessor workAccessor = new SqlWorkAccessor(connectionString);

            TWorkProcessor processorStage1 = new TWorkProcessor("Example.Stage1", "Example.Stage1Complete");
            TWorkProcessor processorStage2 = new TWorkProcessor("Example.Stage2", "Example.Stage2Complete");
            TWorkProcessor processorStage3_1 = new TWorkProcessor("Example.Stage3-1", "Example.Stage3-1Complete");
            TWorkProcessor processorStage3_2 = new TWorkProcessor("Example.Stage3-2", "Example.Stage3-2Complete");

            Dictionary<string, IWorkProcessor> workProcessors = new Dictionary<string, IWorkProcessor>
            {
                {processorStage1.InitialState, processorStage1},
                {processorStage2.InitialState, processorStage2},
                {processorStage3_1.InitialState, processorStage3_1},
                {processorStage3_2.InitialState, processorStage3_2}
            };

            WorkEngine engine = new WorkEngine(
                1,
                trigger,
                workAccessor,
                workProcessors);

            engine.Start();

            Console.WriteLine();
            Console.WriteLine("Press any key...");
            Console.ReadKey(true);

            engine.Stop();

            engine.Dispose();
        }
    }

    internal class TWorkProcessor : IWorkProcessor
    {
        private readonly string initialState;
        private readonly string finalState;

        public TWorkProcessor(string initialState, string finalState)
        {
            this.initialState = initialState;
            this.finalState = finalState;
        }

        public string InitialState
        {
            get { return this.initialState; }
        }

        public void Process(WorkItem workItem)
        {
            workItem.Complete(this.finalState);
        }
    }
}

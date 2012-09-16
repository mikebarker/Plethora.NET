using System;
using System.Text;
using log4net.Layout;
using log4net.Util;

namespace Plethora.Logging.log4net.Extensions
{
    /// <summary>
    /// Dynamic log4net pattern layout used by <see cref="LogConfigurator.BasicConfiguration()"/>.
    /// </summary>
    public class DynamicPatternLayout : PatternLayout
    {
        #region Constructors

        static DynamicPatternLayout()
        {
            GlobalPattern = PatternElement.New().Message();            
        }

        public DynamicPatternLayout()
        {
            GlobalPatternChanged += this.GlobalPatternChangedHandler;
        }

        private void GlobalPatternChangedHandler(object sender, EventArgs e)
        {
            this.ActivateOptions();
        }
        #endregion

        #region PatternLayout Overrides

        /// <summary>
        /// Ignors the pattern provided and utilises the global pattern provided.
        /// </summary>
        protected override PatternParser CreatePatternParser(string pattern)
        {
            //Intercept the pattern
            pattern = GlobalPattern.GeneratePattern().Append("\n").ToString();
            return base.CreatePatternParser(pattern);
        }
        #endregion
        
        #region Static Implementation

        private static PatternElement globalPattern;

        public static PatternElement GlobalPattern
        {
            get { return globalPattern; }
            set
            {
                globalPattern = value;

                OnGlobalPatternChanged();
            }
        }

        private static void OnGlobalPatternChanged()
        {
            var handler = GlobalPatternChanged;
            if (handler != null)
                handler(null, EventArgs.Empty);
        }

        private static event EventHandler GlobalPatternChanged;
        #endregion
    }

    public class PatternElement
    {
        #region Fields

        private readonly PatternElement chain;
        private readonly string subPattern;
        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="PatternElement"/> class.
        /// </summary>
        private PatternElement()
        {
            this.chain = null;
            this.subPattern = string.Empty;
        }

        private PatternElement(PatternElement chain, string subPattern)
        {
            this.chain = chain;
            this.subPattern = subPattern;
        }
        #endregion

        #region Public Methods

        public static PatternElement New()
        {
            return new PatternElement();
        }


        public PatternElement Date()
        {
            return Date(0);
        }

        public PatternElement Date(int padding)
        {
            return Element("date", padding);
        }

        public PatternElement Date(string format)
        {
            return Date(format, 0);
        }

        public PatternElement Date(string format, int padding)
        {
            return Element(string.Format("date{{{0}}}", format), padding);
        }


        public PatternElement Thread() 
        {
            return Thread(0);
        }

        public PatternElement Thread(int padding)
        {
            return Element("thread", padding);
        }


        public PatternElement Level()
        {
            return Level(0);
        }

        public PatternElement Level(int padding)
        {
            return Element("level", padding);
        }


        public PatternElement Logger()
        {
            return Logger(0);
        }

        public PatternElement Logger(int padding)
        {
            return Element("logger", padding);
        }


        public PatternElement Context()
        {
            return Context(0);
        }
        public PatternElement Context(int padding)
        {
            return Element("ndc", padding);
        }


        public PatternElement Message()
        {
            return Message(0);
        }
        public PatternElement Message(int padding)
        {
            return Element("message", padding);
        }


        public PatternElement Space()
        {
            return Litteral(" ");
        }

        public PatternElement Tab()
        {
            return Litteral("\t");
        }

        public PatternElement NewLine()
        {
            return Litteral(Environment.NewLine);
        }


        public PatternElement Litteral(string litteral)
        {
            return new PatternElement(this, litteral);
        }
        #endregion
        
        #region Private Methods

        internal StringBuilder GeneratePattern()
        {
            if (chain == null)
                return new StringBuilder();

            var sb = this.chain.GeneratePattern();
            sb.Append(this.subPattern);
            return sb;
        }

        private PatternElement Element(string pattern, int padding)
        {
            return new PatternElement(this, string.Format("%{1}{0}",
                                                          pattern,
                                                          (padding==0) ? "" : padding.ToString()));
        }
        #endregion
    }
}

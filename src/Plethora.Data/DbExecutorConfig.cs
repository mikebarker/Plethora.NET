using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Plethora.Data
{
#pragma warning disable CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
#pragma warning disable IDE1006 // Naming Styles
    [Serializable]
    public sealed class DbExecutorConfig
    {
        [XmlAttribute]
        public int defaultRetryCount;

        public List<redirection> redirections { get; set; }

        public List<timeout> timeouts { get; set; }

        public List<retry> retries { get; set; }
    }

    [Serializable]
    public sealed class redirection
    {
        [XmlAttribute]
        public string commandText { get; set; }

        [XmlAttribute]
        public string substitute { get; set; }
    }

    [Serializable]
    public sealed class timeout
    {
        [XmlAttribute]
        public string commandText { get; set; }

        [XmlAttribute]
        public int timeoutSec { get; set; }
    }

    [Serializable]
    public sealed class retry
    {
        [XmlAttribute]
        public string commandText { get; set; }

        [XmlAttribute]
        public int retryCount { get; set; }
    }
#pragma warning restore CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
#pragma warning restore IDE1006 // Naming Styles

    public static class DbExecutorConfigHelper
    {
        public static DbExecutorConfig ReadConfig(StreamReader streamReader)
        {
            XmlSerializer serializer = new XmlSerializer(
                typeof(DbExecutorConfig),
                new[]
                {
                    typeof (redirection),
                    typeof (timeout),
                    typeof (retry)
                });

            object obj = serializer.Deserialize(streamReader);
            return obj as DbExecutorConfig;
        }
    }
}

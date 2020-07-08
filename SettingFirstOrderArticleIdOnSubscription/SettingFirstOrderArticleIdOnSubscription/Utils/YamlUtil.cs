using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SettingFirstOrderArticleIdOnSubscription.Utils
{
    public class YamlUtil
    {
        public class YamlImporter
        {
            public Models.ImportYaml.DeserializedObject Deserialize(string yamlName)
            {
                StreamReader sr = new StreamReader(yamlName);
                string text = sr.ReadToEnd();
                var input = new StringReader(text);
                var deserializer = new Deserializer();
                Models.ImportYaml.DeserializedObject deserializeObject = deserializer.Deserialize<Models.ImportYaml.DeserializedObject>(input);
                return deserializeObject;
            }
        }
    }
}

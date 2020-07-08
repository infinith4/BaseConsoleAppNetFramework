using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace SettingFirstOrderArticleIdOnSubscription.Models
{
    public class ImportYaml
    {
        public class DeserializedObject
        {
            [YamlMember(Alias = "basepaths")]
            public List<BasePath> basepaths { get; set; }

            public class BasePath
            {
                public string basepath { get; set; }
            }

            [YamlMember(Alias = "aspid_list")]
            public List<AspIdList> aspid_list { get; set; }

            public class AspIdList
            {
                public string aspid { get; set; }
            }
        }
    }
}

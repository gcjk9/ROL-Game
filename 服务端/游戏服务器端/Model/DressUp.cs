
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model
{
    class DressUp
    {
        public int Id { get; set; }
        public string Head { get; set; }
        public string Face { get; set; }
        public string Body { get; set; }

        /// <summary>
        /// 将装扮信息打包成json格式
        /// </summary>
        /// <returns></returns>
        public JObject GetJsonPackage()
        {
            JObject JDressUp = new JObject();
            
            JDressUp.Add("id", JToken.FromObject(Id.ToString()));
            JDressUp.Add("head", JToken.FromObject(Head));
            JDressUp.Add("face", JToken.FromObject(Face));
            JDressUp.Add("body", JToken.FromObject(Body));

            return JDressUp;
        }
    }
}

using UnityEngine;
using System.Collections;
using itfantasy.lmjson;
using itfantasy.unii.utils;

namespace itfantasy.unii
{
    public class BrushVo
    {
        /// <summary>
        /// id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public Color color { get; set; }

        /// <summary>
        /// 用户数据
        /// </summary>
        public string data { get; set; }

        public BrushVo()
        {
            id = 0;
            name = "";
            type = 0;
            color = Color.white;
            data = "";
        }

        public void Initialize(JsonData json)
        {
            this.id = json["id"].ToInt();
            this.name = json["name"].ToString();


            JsonUtil.LoadObject(json, this);
        }

        public JsonData Serialize()
        {
            JsonData json = JsonUtil.Serialize(this);
            return json;
        }
        
    }
}

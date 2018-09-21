using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using itfantasy.lmjson;

namespace itfantasy.unii.utils
{
    public class JsonUtil
    {

        public static string Encode(object csObj)
        {
            return Serialize(csObj).ToJson();
        }

        public static JsonData Serialize(object csObj)
        {
            JsonData json = new JsonData();
            if (csObj is IList)
            {
                foreach (object item in csObj as IList)
                {
                    if (item.GetType().IsValueType || item.GetType() == typeof(string))
                    {
                        json.Add(item);
                    }
                    else
                    {
                        json.Add(Serialize(item));
                    }
                }
            }
            else if (csObj is IDictionary)
            {
                foreach (object key in (csObj as IDictionary).Keys)
                {
                    object item = (csObj as IDictionary)[key];
                    if (item.GetType().IsValueType || item.GetType() == typeof(string))
                    {
                        json[key.ToString()] = new JsonData(item);
                    }
                    else
                    {
                        json[key.ToString()] = Serialize(item);
                    }
                }
            }
            else
            {
                json = SerializeJson(csObj);
            }
            return json;
        }

        static JsonData SerializeJson(object csObj)
        {
            JsonData json = new JsonData();
            List<string> properties = Comm.GetProperties(csObj);
            foreach (string key in properties)
            {
                object val = Comm.GetPropertyValue(csObj, key);
                if (val != null)
                {
                    if (val.GetType().IsValueType || val.GetType() == typeof(string))
                    {
                        json[key] = new JsonData(val);
                    }
                    else
                    {
                        json[key] = Serialize(val);
                    }
                }
            }
            return json;
        }

        public static JsonData Decode(string str)
        {
            return JSON.Parse(str);
        }

        public static T Decode<T>(string str) where T : new()
        {
            JsonData json = JSON.Parse(str);
            return LoadClass<T>(json);
        }

        public static List<T> DecodeList<T>(string str) where T : new()
        {
            return LoadList<T>(Decode(str));
        }

        public static T LoadClass<T>(JsonData json) where T : new()
        {
            return LoadObject(json, new T());
        }

        public static List<T> LoadList<T>(JsonData json) where T : new()
        {
            List<T> list = new List<T>();
            for (int i = 0; i < json.Count; i++)
            {
                list.Add(LoadClass<T>(json[i]));
            }
            return list;
        }

        public static T LoadObject<T>(JsonData json, T csObj)
        {
            List<string> properties = Comm.GetProperties(csObj);
            foreach (string key in properties)
            {
                if (json.Keys.Contains(key) && json[key] != null)
                {
                    if (json[key].IsArray)
                    {
                        ArrayList list = new ArrayList();
                        for (int i = 0; i < json[key].Count; i++)
                        {
                            JsonData item = json[key][i];
                            if (item.IsString)
                            {
                                list.Add(item.ToString());
                            }
                            else
                            {
                                list.Add(item.ToBinary());
                            }
                        }
                        Comm.SetPropertyValue(csObj, key, list);
                    }
                    else
                    {
                        Comm.SetPropertyValue(csObj, key, json[key]);
                    }
                }
            }
            return csObj;
        }
    }
}

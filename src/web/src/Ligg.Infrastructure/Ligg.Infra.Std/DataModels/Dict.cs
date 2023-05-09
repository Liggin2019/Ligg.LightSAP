using System.Collections.Generic;

namespace Ligg.Infrastructure.DataModels
{
    public class DictItem
    {
        private string _key;
        private string _value;
        public DictItem() { }
        public DictItem(string key, string value) { _key = key; _value = value; }
        public string Key { get { return _key; } set { _key = value; } }
        public string Value { get { return _value; } set { _value = value; } }
        public override string ToString() { return $"Key: {_key}, Value: {_value}"; }
    }

    public class GroupDict
    {
        public string Key;
        public List<DictItem> Value;
    }
    public class Dict
    {
        public List<DictItem> DictItems;
    }




}

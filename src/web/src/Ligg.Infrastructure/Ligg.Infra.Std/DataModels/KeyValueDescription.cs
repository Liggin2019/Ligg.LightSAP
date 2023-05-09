

namespace  Ligg.Infrastructure.DataModels
{
    public class KeyValueDescription
    {
        public KeyValueDescription(){ }
        public KeyValueDescription(string key, string val, string des)
        {
             Key = key; Value = val; Description = des;
        }

        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public override string ToString() { return $"Key: {Key}, Value: {Value}, Description: { Description}"; }
    }


}

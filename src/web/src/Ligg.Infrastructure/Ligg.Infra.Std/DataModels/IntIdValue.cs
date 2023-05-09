namespace  Ligg.Infrastructure.DataModels
{
    public class IntIdValue
    {
        public IntIdValue(){}
        public IntIdValue(int id, string value) { _id = id; _value = value; }

        private int _id;
        private string _value;

        public int Id { get { return _id; } set { _id = value; } }
        public string Value { get { return _value; } set { _value = value; } }
        public override string ToString() { return $"Id: {_id}, Value: {_value}"; }
    }
}

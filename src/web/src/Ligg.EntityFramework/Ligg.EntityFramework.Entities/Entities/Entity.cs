using Ligg.EntityFramework.Entities.Helpers;
using Newtonsoft.Json;
using System;

namespace Ligg.EntityFramework.Entities
{
    public abstract class Entity<TKey>
    {
        public Entity()
        {
            if (InitiateKey())
            {
                if (typeof(TKey) != typeof(long) &typeof(TKey) != typeof(int) & typeof(TKey) != typeof(Guid) & typeof(TKey) != typeof(string))
                    throw new Exception("Entity TKey can only be long, int, guid or string");
            }
        }

        public bool InitiateKey()
        {
            if (typeof(TKey) == typeof(long)) return true;
            if (typeof(TKey) == typeof(int)) return false;//seed, auto-increment
            else if (typeof(TKey) == typeof(Guid)) return true;
            else if (typeof(TKey) == typeof(string)) { return string.IsNullOrEmpty(Id.ToString()) ? true : false; } //string
            else throw new NotImplementedException();
        }

        public TKey GetDefaultKeyValue()
        {
            return default(TKey);
        }

        public void CreateKeyVal()
        {
            Id = EntityHelper.CreateKeyVal<TKey>();
        }

        [JsonConverter(typeof(StringJsonConverter))]
        public TKey Id { get; set; }

        public string CreatorId { get; set; }

        public DateTime CreationTime { get; set; }

        public string LastModifierId { get; set; }

        public DateTime? ModificationTime { get; set; }


    }
}
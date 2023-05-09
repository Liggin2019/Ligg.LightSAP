
using System;
using System.ComponentModel;

namespace Ligg.EntityFramework.Entities.Helpers
{
    public static class EntityHelper
    {
        public static TKey CreateKeyVal<TKey>()
        {

            if (typeof(TKey) == typeof(Guid))
            {
                var id = GuidGenerator.Instance.GetGuid();
                var conveter = TypeDescriptor.GetConverter(id);
                return (TKey)conveter.ConvertTo(id, typeof(TKey));
            }
            else if (typeof(TKey) == typeof(String))
            {
                var id = GuidGenerator.Instance.GetShortGuId();
                var conveter = TypeDescriptor.GetConverter(id);
                return (TKey)conveter.ConvertTo(id, typeof(TKey));
            }
            else if (typeof(TKey) == typeof(long))
            {
                //var id = new Random().Next();
                var id = SnowFlakeGenerator.Instance.GetId();
                var conveter = TypeDescriptor.GetConverter(id);
                return (TKey)conveter.ConvertTo(id, typeof(TKey));
            }
            //else if (typeof(TKey) == typeof(int))////seed, auto-increment
            //{
            //    return default(TKey);
            //}
            else throw new NotImplementedException();
        }

    }

}

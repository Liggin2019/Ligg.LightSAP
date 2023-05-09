using System;


namespace Ligg.EntityFramework.Entities.Helpers
{
    public class GuidGenerator
    {

        private static readonly GuidGenerator instance = new GuidGenerator();

        private GuidGenerator()
        {
        }
        public static GuidGenerator Instance
        {
            get
            {
                return instance;
            }
        }
        public string GetShortGuId()
        {
            var guid = Guid.NewGuid();
            return ToBase64String(guid);
        }

        public Guid GetGuid()
        {
            var guid = Guid.NewGuid();
            return guid;
        }


        public static string ToBase64String(Guid target)
        {
            if (target == Guid.Empty) throw new ArgumentException(target+" cannot be empty guid.");

            string base64 = Convert.ToBase64String(target.ToByteArray());

            string encoded = base64.Replace("/", "_").Replace("+", "-");

            return encoded.Substring(0, 22);
        }



    }


}

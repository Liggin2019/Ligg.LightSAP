using System;
using System.Collections;
using System.Collections.Generic;
using Ligg.Infrastructure.Extensions;
using Newtonsoft.Json;

namespace Ligg.Infrastructure.Helpers
{
    public static partial class ObjectHelper
    {

        public static string ParseToString(object obj)
        {
            try
            {
                if (obj == null)
                {
                    return string.Empty;
                }
                else
                {
                    return obj.ToString();
                }
            }
            catch
            {
                return string.Empty;
            }
        }

    }
}




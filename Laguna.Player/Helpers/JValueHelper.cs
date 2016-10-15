using System;
using Newtonsoft.Json.Linq;

namespace JhDeStip.Laguna.Player.Helpers
{
    public static class JValueHelper
    {
        public static int JValueToIntOrDefault(JValue value, int defaultValue)
        {
            if (value == null)
            {
                return defaultValue;
            }

            int intValue;
            if (int.TryParse(value.ToString(), out intValue))
            {
                return intValue;
            }

            return defaultValue;
        }

        public static string JValueToStringOrDefault(JValue value, string defaultValue)
        {
            if (value == null)
            {
                return defaultValue;
            }

            return value.ToString();
        }
    }
}

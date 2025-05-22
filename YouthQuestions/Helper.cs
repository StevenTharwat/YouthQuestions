namespace YouthQuestions
{
    static public class Helper
    {
        public static string customBase64Encode(string value)
        {
            string Base64 = "";
            try
            {
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(value);
                Base64= "df"+System.Convert.ToBase64String(plainTextBytes);
                Base64 = Base64.Replace("=", ";");
                Base64 = Base64 += "c=";
            }
            catch (Exception ex)
            {
            }
            return Base64;
        }

        public static string customBase64Decode(string value)
        {
            string original = "";
            try
            {
                if (value == null) return "NULL";
                if (value.Trim() == "" || value.Count() <= 4) return "NULL";
                value = value.Remove(value.Length - 2, 2);
                value = value.Remove(0, 2);
                value = value.Replace(";", "=");

                var base64EncodedBytes = System.Convert.FromBase64String(value);
                original= System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            }
            catch(Exception ex)
            {
            }
            return original;
        }

        internal static void logError(string className,string functionName, Exception ex)
        {
            //nlog
            var logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Error($"{className}|{functionName}|{ex.Message}");
        }
        
        internal static void logInfo(string className,string functionName,string message)
        {
            //nlog
            var logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Info($"{className}|{functionName}|{message}");
        }
    }
}

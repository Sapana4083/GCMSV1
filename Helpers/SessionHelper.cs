using Microsoft.AspNetCore.Http;

namespace GCMS.Helpers
{
    public class SessionHelper
    {
        public static void SetCaseId(HttpContext context, long caseId)
        {
            context.Session.SetString("CaseId", caseId.ToString());
        }

        public static long GetCaseId(HttpContext context)
        {
            string value = context.Session.GetString("CaseId");

            if (string.IsNullOrEmpty(value))
                return 0;

            return Convert.ToInt64(value);
        }

        public static void Clear(HttpContext context)
        {
            context.Session.Remove("CaseId");
        }
    }
}

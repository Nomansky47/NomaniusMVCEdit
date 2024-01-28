using Microsoft.AspNetCore.Http;
using System.Text;

namespace NomaniusMVC
{
    public static class DataSessions
    {
        public static void setAdmin(this HttpContext context, bool b)
        {
            if (b) context.Session.SetInt32("isAdmin", 0);
            else context.Session.SetInt32("isAdmin", 1);
        }
        public static bool? isAdmin(this HttpContext context)
        {
            if (context.Session.GetInt32("isAdmin") == 0)
                return true;
            else if (context.Session.GetInt32("isAdmin") == 1) return false;
            else return null;

        }
        public static void AddSession(this HttpContext context, string id, string content)
        {
            context.Session.SetString(id, content);
        }
        public static string? GetSession(this HttpContext context, string id)
        {
            return context.Session.GetString(id);
        }
        public static void DeleteAllSessions(this HttpContext context)
        {
            context.Session.Clear();
        }
    }
}

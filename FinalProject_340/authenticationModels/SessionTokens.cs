using FinalProject_340.Utilities;
using FinalProject_340.Models;

namespace FinalProject_340.Models
{
    public class SessionTokens
    {
        public static SessionTokens? getToken(string cookie)
        {
            if (string.IsNullOrEmpty(cookie)) return null;
            SqlDBConnection<SessionTokens> newConnection = new SqlDBConnection<SessionTokens>(Properties.Resource.appData);
            SessionTokens? newToken = newConnection.getFirstOrDefault(new Dictionary<string, string>()
            {
                {"SessionID", cookie }
            });
            return newToken;
        }

        public string? SessionID { get; set; }
        public string? accountHash { get; set; }
        public SessionTokens() { }
        public SessionTokens(string? UUID)
        {
            SessionID = (UUID + DateTime.Now.ToString()).toHash();
            accountHash = UUID;
        }
        public bool registerToken()
        {
            SqlDBConnection<SessionTokens> newConnection = new SqlDBConnection<SessionTokens>(Properties.Resource.appData);
            return newConnection.insertIntoTable(this);
        }
    }
}

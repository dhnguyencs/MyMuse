namespace FinalProject_340.Models
{
    public class SessionTokens
    {
        public string? SessionID { get; set; }
        public string? accountHash { get; set; }
        public SessionTokens() { }
        public SessionTokens(string? UUID)
        {
            SessionID = (UUID + DateTime.Now.ToString()).toHash();
            this.accountHash = UUID;
        }
        public bool registerToken()
        {
            SqlDBConnection<SessionTokens> newConnection = new SqlDBConnection<SessionTokens>("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=userDB_340;Connect Timeout=100;");
            return newConnection.insertIntoTable(this);
        }
    }
}

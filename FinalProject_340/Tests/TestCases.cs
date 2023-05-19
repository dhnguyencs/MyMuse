using FinalProject_340.Utilities;

namespace FinalProject_340.Tests
{
    public class TestCases
    {
        public TestCases(string ? connectionString = null)
        {
            connectDBString(connectionString);
        }

        public SqlDBConnection<Test> SqlDBConnection = new SqlDBConnection<Test>();
        public void connectDBString(string ? connectionString = null)
        {
            SqlDBConnection.createConnection(connectionString);
        }
        public bool testInsert(Test test)
        {
            return SqlDBConnection.insertIntoTable(test);
        }
        public void ____test_cases_1()
        {
            Test test = new Test()
            {
                c6 = true,
                c7 = true,
                c8 = true,
                c9 = true,
                c10 = true,
                c11 = true,
                c12 = false
            };
            testInsert(test);

            List<Test> query = SqlDBConnection.getList(new Dictionary<string, string>(), 1000);
        }
    }
}

namespace Framework.Utility
{
    public static class PersistenceHelper
    {
        public const string SqlConnectionStringFormat =
            "Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3};MultipleActiveResultSets=true;";

        public static string GenerateConnectionString(string server, string database,string userName,string password)
        {
            return string.Format(SqlConnectionStringFormat, server, database, userName, password);
        }
    }
}
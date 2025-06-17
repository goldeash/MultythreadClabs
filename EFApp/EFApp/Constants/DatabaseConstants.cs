namespace EFApp.Constants
{
    /// <summary>
    /// Contains constants for database configuration
    /// </summary>
    public static class DatabaseConstants
    {
        public const string CONNECTION_STRING = "workstation id=EFtesting.mssql.somee.com;packet size=4096;user id=goldeash_SQLLogin_1;pwd=rbwh9c17gi;data source=EFtesting.mssql.somee.com;persist security info=False;initial catalog=EFtesting;TrustServerCertificate=True";
        public const string MANUFACTURERS_TABLE_NAME = "EF_Manufacturers";
        public const string SHIPS_TABLE_NAME = "EF_Ships";
        public const int DEFAULT_NUMBER_OF_ENTRIES = 30;
    }
}
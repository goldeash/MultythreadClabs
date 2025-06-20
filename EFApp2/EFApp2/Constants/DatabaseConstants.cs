namespace EFApp.Constants
{
    /// <summary>
    /// Contains constant values used throughout the application.
    /// </summary>
    public static class AppConstants
    {
        /// <summary>
        /// The database connection string.
        /// </summary>
        public const string CONNECTION_STRING = "workstation id=EFtesting2.mssql.somee.com;packet size=4096;user id=goldeash_SQLLogin_1;pwd=rbwh9c17gi;data source=EFtesting2.mssql.somee.com;persist security info=False;initial catalog=EFtesting2;TrustServerCertificate=True";

        // --- User Interface Messages ---
        public const string INVALID_OPTION_MSG = "Invalid option. Press any key to continue...";
        public const string PRESS_ANY_KEY_MSG = "Press any key to continue...";
        public const string INVALID_ID_MSG = "Invalid ID format. Press any key to continue...";
        public const string INVALID_NUMBER_MSG = "Invalid number format. Press any key to continue...";
        public const string OPERATION_SUCCESS_MSG = "Operation completed successfully. Press any key to continue...";
        public const string NOT_FOUND_MSG = "Entity not found. Press any key to continue...";
        public const string ALL_FIELDS_REQUIRED_MSG = "All fields are required. Press any key to continue...";
        public const string ADD_MANUFACTURER_FIRST_MSG = "No manufacturers found. Please add a manufacturer first.";

        // --- Table Names ---
        public const string TPH_MANUFACTURERS_TABLE = "Lab8_TPH_Manufacturers";
        public const string TPH_SHIPS_TABLE = "Lab8_TPH_Ships";
        public const string TPT_MANUFACTURERS_TABLE = "Lab8_TPT_Manufacturers";
        public const string TPT_SHIPS_TABLE = "Lab8_TPT_Ships_Base";
        public const string TPT_BATTLESHIPS_TABLE = "Lab8_TPT_Battleships";
        public const string TPT_AIRCARRIERS_TABLE = "Lab8_TPT_Aircarriers";
        public const string TPT_CRUISERS_TABLE = "Lab8_TPT_Cruisers";
        public const string TPT_DESTROYERS_TABLE = "Lab8_TPT_Destroyers";
        public const string TPT_SUBMARINES_TABLE = "Lab8_TPT_Submarines";
        public const string TPC_MANUFACTURERS_TABLE = "Lab8_TPC_Manufacturers";
        public const string TPC_BATTLESHIPS_TABLE = "Lab8_TPC_Battleships";
        public const string TPC_AIRCARRIERS_TABLE = "Lab8_TPC_Aircarriers";
        public const string TPC_CRUISERS_TABLE = "Lab8_TPC_Cruisers";
        public const string TPC_DESTROYERS_TABLE = "Lab8_TPC_Destroyers";
        public const string TPC_SUBMARINES_TABLE = "Lab8_TPC_Submarines";
    }
}

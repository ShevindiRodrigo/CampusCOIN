namespace CampusCOIN
{
    public static class Constants
    {
        //Set database name
        public const string DatabaseFilename = "campuscoin.db3";

        //set database path
        public static string DatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
    }
}

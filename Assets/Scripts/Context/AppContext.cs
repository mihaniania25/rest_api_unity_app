namespace TABApps.TestTask
{
    public static class AppContext
    {
        public static AppConfigs Configs { get; private set; }
        public static AppModel Model { get; private set; }

        static AppContext()
        {
            Configs = new AppConfigs();
            Model = new AppModel();
        }
    }
}
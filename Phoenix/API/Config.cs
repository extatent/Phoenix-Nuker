namespace Phoenix
{
    public static class Config
    {
        public static string APIVersion = "10";
        public static bool IsBot = false;

        public enum Wait
        {
            Short = 0, Long = 1
        }

        public static void Sleep(Wait option)
        {
            if (option == Wait.Short)
                Thread.Sleep(200);
            else if (option == Wait.Long)
                Thread.Sleep(2000);
        }
    }
}
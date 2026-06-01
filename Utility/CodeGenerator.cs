namespace canbecheaperAPI.Utility
{
    public static class CodeGenerator
    {
        public static int Generate( )
        {
            Random random = new Random();

            int code = random.Next(100000, 10000000);
            return code;
        }
    }

    
}

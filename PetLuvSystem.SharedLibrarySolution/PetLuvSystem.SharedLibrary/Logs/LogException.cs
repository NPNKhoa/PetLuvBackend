using Serilog;

namespace PetLuvSystem.SharedLibrary.Logs
{
    public static class LogException
    {
        public static void LogExceptions(Exception ex)
        {
            Console.WriteLine("Original Exception: ");
            Console.WriteLine(ex.Message);
            Console.WriteLine("Exception Message: ");
            Log.Error(ex.Message);
        }

        public static void LogInformation(string message)
        {
            Console.WriteLine("Info Message: ");
            Console.WriteLine(message);
            Log.Information(message);
        }
    }
}

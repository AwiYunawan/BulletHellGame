using System;

namespace BulletHellGame
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                using (var game = new Game1())
                    game.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ ERROR: " + ex.Message);
                Console.WriteLine("📌 TRACE:\n" + ex.StackTrace);
                Console.ReadLine(); // agar terminal tidak langsung nutup
            }
        }
    }
}

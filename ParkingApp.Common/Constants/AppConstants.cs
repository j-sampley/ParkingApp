
namespace ParkingApp.Common.Constants
{
    public static class AppConstants
    {
        public static void ApiBanner()
        {
            string banner = @"
    ____             __   _                ___    ____  ____
   / __ \____ ______/ /__(_)___  ____ _   /   |  / __ \/  _/
  / /_/ / __ `/ ___/ //_/ / __ \/ __ `/  / /| | / /_/ // /  
 / ____/ /_/ / /  / ,< / / / / / /_/ /  / ___ |/ ____// /   
/_/    \__,_/_/  /_/|_/_/_/ /_/\__, /  /_/  |_/_/   /___/   
                              /____/                        
";
            Console.WriteLine(banner);
        }

        public static void GuiBanner()
        {
            string banner = @"
    ____             __   _                ________  ______
   / __ \____ ______/ /__(_)___  ____ _   / ____/ / / /  _/
  / /_/ / __ `/ ___/ //_/ / __ \/ __ `/  / / __/ / / // /  
 / ____/ /_/ / /  / ,< / / / / / /_/ /  / /_/ / /_/ // /   
/_/    \__,_/_/  /_/|_/_/_/ /_/\__, /   \____/\____/___/   
                              /____/                       
";
            Console.WriteLine(banner);
        }
    }
}
using MapHandleSpace;

namespace CSHARPRPG
{
    class RPGame
    {
        public static ActorStructureSpace.ActorStruc PlayerCharacter = new ActorStructureSpace.ActorStruc(){};
        public static Location PlayerLocation = new Location(){X = -1, Y = -1};
        public static int MapHeight = 40;
        public static int MapWidth = 40;
        public static int StartLevel = 5;
        static void Main(string[] args)
        {
            HandleStartUp();
            GameLoop();
        }


        static void HandleStartUp()
        {
            Random random = new Random();

            Console.WriteLine("-------------------------------------------------------------------------------------------------");
            Console.WriteLine("Welcome to ToJudge, An unfair Action roguelike, please wait while we generate a map just for you!");
            
            if (MapHandler.GenerateMap(MapHeight, MapWidth))
            {
                Console.WriteLine("Map has been successfully Generated! Have a look!");
                MapHandler.ReadAndDisplayMap(PlayerLocation, MapHandler.Map);
            }
            else
            {
                Console.WriteLine("Something has gone wrong...");
                Console.WriteLine("-------------------------------------------------------------------------------------------------");
                return;
            }

            Console.WriteLine("Now, who are you? What are your abilities? Do you want to choose a character or create one yourself?");
            Console.WriteLine("Type the corresponding number to make your decision. Let's give it a try!");
            Console.WriteLine("1: Create your own character");
            Console.WriteLine("2: Choose a pre-made character to get straight into the action");

            ConsoleKeyInfo choice = Console.ReadKey();
            Console.WriteLine();

            switch (choice.KeyChar)
            {
                case '1':
                    Console.WriteLine("Selected: Heading to Character Creation...");
                    PlayerCharacter = ActorStructureSpace.ActorHandle.GeneratePlayerCharacter();
                    break;

                case '2':
                    Console.WriteLine("Selected: Heading to Character Selection...");
                    PlayerCharacter = ActorStructureSpace.ActorHandle.GeneratePlayerSelection();
                    break;

                default:
                    Console.WriteLine("Invalid selection. Please restart the game and choose a valid option.");
                    Main(["Restart"]);
                    break;
            }
            Console.WriteLine($"Well done {PlayerCharacter.ActorName}! Now We will Generate random spawns for you to choose!");
            do
            {
                int GeneratedX = random.Next(0, MapWidth);
                int GeneratedY = random.Next(0, MapHeight);
                Console.WriteLine($"Hmmm how about X: {GeneratedX} Y: {GeneratedY}");
                Console.WriteLine("1: Yes \n 2: No");
                var LocationChoice = Console.ReadKey();
                if (LocationChoice.KeyChar == '1')
                {
                    PlayerLocation = new Location(){Y = GeneratedY, X = GeneratedX};
                }
            } while (PlayerLocation.X == -1 && PlayerLocation.Y == -1);
            Console.WriteLine("Startup complete! Have fun in the wasteland!");
        }
        public static void GameLoop()
        {
            while (PlayerCharacter.ActorHealth > 0)
            {
                MapHandler.GivePlayerMovementControls(PlayerLocation);
            }
            Console.WriteLine("Game Over");
        }
    }
}
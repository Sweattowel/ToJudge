using System.Runtime.Remoting;
using ActorStructureSpace;

namespace MapHandleSpace
{
    public class Location
    {
        public int X { get; set; }
        public int Y { get; set; }
    };
    public class MapDataStructure
    {
        public int LocationID { get; set; }
        public Location Location { get; set; }
        public bool CanPass { get; set; }
        public int WhatIsHereID { get; set; }
        public int WhatListToSearch { get; set; }
        public string WhatIsHereName { get; set; }
        public string WhatIsHereRepresentation { get; set; }
    };
    public class MapHandler
    {
        public static MapDataStructure[][] Map;
        public static void ReadAndDisplayMap(Location PlayerLocation, MapDataStructure[][] MapToRead)
        {
            for (int y = 0; y < MapToRead.Length; y++)
            {
                string MapLine = "";

                for (int x = 0; x < MapToRead[y].Length; x++)
                {
                    var CurrentPosition = MapToRead[y][x];
                    if (x == PlayerLocation.X && y == PlayerLocation.Y )
                    {
                        MapLine += "PL";
                        continue;
                    }
                    MapLine += CurrentPosition.WhatIsHereRepresentation; 
                }
                Console.WriteLine(MapLine);
            }
        }
        public static bool GenerateMap(int MapHeight, int MapWidth)
        {
            Random random = new Random();
            
            MapDataStructure[][] GeneratedMap = new MapDataStructure[MapHeight][];
            
            for (int y = 0; y < MapHeight; y++)
            {
                GeneratedMap[y] = new MapDataStructure[MapWidth];
                for (int x = 0; x < MapWidth; x++)
                {
                    int CurrentNoise = random.Next(0, 256);
                    MapDataStructure CurrentPosition = new(){};
                    /*
                        IDS
                        AIR 0
                        LAVA 1
                        STONE 2
                        MOUNTAIN 6
                        WATER 3
                        POSSESSEDWATER 5
                        RUBBLE 4
                    */
                    
                    switch (CurrentNoise)
                    {
                        
                        case < 2:
                            CurrentPosition = GenerateStore(random, x, y);
                            break;
                        case >= 2 and < 5:
                            CurrentPosition = GenerateEnemy(random, x, y);
                            break;
                        case > 5 and < 100:
                            CurrentPosition = GenerateSpecificObject(0 ,x ,y);
                            break;
                       case > 100 and < 150:
                            CurrentPosition = GenerateSpecificObject(2 ,x ,y);
                            break;
                       case > 150 and < 200:
                            CurrentPosition = GenerateSpecificObject(3 ,x ,y);
                            break;
                       case > 200 and < 220:
                            CurrentPosition = GenerateSpecificObject(6 ,x ,y);
                            break;                            
                        default:
                            CurrentPosition = GenerateRandomObject(random, x, y, true);
                            break;
                    }
                    GeneratedMap[y][x] = CurrentPosition;
                }                            
            }
            Map = GeneratedMap;
            return Map.Length == MapHeight && Map[0].Length == MapWidth;
        }
        // Need to make make that isnt completely random with no structure
        public static void GenerateMapTestNoise(int MapHeight, int MapWidth)
        {
            Random random = new Random();
            MapDataStructure[][] GeneratedMap = new MapDataStructure[MapHeight][];

            ReadAndDisplayMap(CSHARPRPG.RPGame.PlayerLocation, GeneratedMap);
        }
        public static MapDataStructure GenerateRandomObject(Random random, int X, int Y, bool MustBeClear)
        {   
            var Object = ObjectHandleSpace.ObjectStruc.GenerateObject(random, X, Y, false);

            MapDataStructure GeneratedMapData = new MapDataStructure(){
                LocationID = X + Y,
                CanPass = false,
                Location = new Location(){ X = X, Y = Y },
                WhatIsHereID = Object.ObjectID,
                WhatIsHereName = Object.ObjectName,
                WhatListToSearch = 0,
                WhatIsHereRepresentation = Object.ObjectRepresentation
            };

            return GeneratedMapData;
        }
        public static MapDataStructure GenerateSpecificObject(int WantedObjectID, int X, int Y)
        {
            var Object = ObjectHandleSpace.ObjectStruc.ObjectList.FirstOrDefault(_ => _.ObjectID == WantedObjectID);

            return new MapDataStructure(){
                LocationID = X + Y,
                CanPass = Object.CanPass,
                Location = new Location(){ X = X, Y = Y },
                WhatIsHereID = Object.ObjectID,
                WhatIsHereName = Object.ObjectName,
                WhatListToSearch = 0,
                WhatIsHereRepresentation = Object.ObjectRepresentation
            };
        }
        public static MapDataStructure GenerateEnemy(Random random, int X, int Y)
        {
            var GeneratedEnemy = ActorStructureSpace.ActorHandle.GenerateAndStoreActor(random);
            MapDataStructure GeneratedMapData = new MapDataStructure(){
                LocationID = X + Y,
                CanPass = false,
                Location = new Location(){ X = X, Y = Y },
                WhatIsHereID = GeneratedEnemy.ActorID,
                WhatIsHereName = GeneratedEnemy.ActorName,
                WhatListToSearch = 1,
                WhatIsHereRepresentation = GeneratedEnemy.ActorRepresentation
            };
            return GeneratedMapData;
        }

        public static MapDataStructure GenerateStore(Random random, int X, int Y)
        {
            var Store = StoreHandleSpace.StoreStruc.GenerateStore(random);

            MapDataStructure GeneratedMapData = new MapDataStructure(){
                LocationID = X + Y,
                CanPass = false,
                Location = new Location(){ X = X, Y = Y },
                WhatIsHereID = Store.StoreID,
                WhatIsHereName = Store.StoreName,
                WhatListToSearch = 2,
                WhatIsHereRepresentation = Store.StoreRepresentation
            };

            return GeneratedMapData;
        }
        public static void GivePlayerMovementControls(Location CurrentLocation)
        {
            ReadAndDisplayMap( CurrentLocation, Map); 
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("Press 4 to move left");
            Console.WriteLine("Press 8 to move up");
            Console.WriteLine("Press 6 to move right");
            Console.WriteLine("Press 5 to move down");
            Console.WriteLine("Press i to check Character");

            var keyPress = Console.ReadKey().KeyChar;
            MapDataStructure NewLocation = Map[CurrentLocation.Y][CurrentLocation.X]; 

            switch (keyPress)
            {
                case '4': 
                    if (CurrentLocation.X > 0)
                    {
                        NewLocation = Map[CurrentLocation.Y][CurrentLocation.X - 1];  
                        HandleLocation(NewLocation, CurrentLocation, [0,-1]);
                    }
                    else
                    {
                        Console.WriteLine("You can't move left. You're at the edge of the map.");
                    }
                    break;

                case '8':  
                    if (CurrentLocation.Y > 0)  
                    {
                        NewLocation = Map[CurrentLocation.Y - 1][CurrentLocation.X];
                        HandleLocation(NewLocation, CurrentLocation, [-1,0]);
                    }
                    else
                    {
                        Console.WriteLine("You can't move up. You're at the top edge of the map.");
                    }
                    break;

                case '6':
                    if (CurrentLocation.X < Map[CurrentLocation.Y].Length - 1)  
                    {
                        NewLocation = Map[CurrentLocation.Y][CurrentLocation.X + 1]; 
                        HandleLocation(NewLocation, CurrentLocation, [0,1]);
                    }
                    else
                    {
                        Console.WriteLine("You can't move right. You're at the edge of the map.");
                    }
                    break;

                case '5':  
                    if (CurrentLocation.Y < Map.Length - 1) 
                    {
                        NewLocation = Map[CurrentLocation.Y + 1][CurrentLocation.X];  
                        HandleLocation(NewLocation, CurrentLocation, [1,0]);
                    }
                    else
                    {
                        Console.WriteLine("You can't move down. You're at the bottom edge of the map.");
                    }
                    break;

                case 'i': 
                    Console.WriteLine("Character Info:");  
                    ActorHandle.DisplayActorData(CSHARPRPG.RPGame.PlayerCharacter, -1);
                    break;

                default:
                    Console.WriteLine("Invalid input, please try again.");
                    break;
            }
        }
        private static void HandleLocation(MapDataStructure NewLocationData, Location CurrentPlayerLocation, int[] Vector)
        {   
            var NewPlayerLocation = new Location(){
                X = CurrentPlayerLocation.X + Vector[1],
                Y = CurrentPlayerLocation.Y + Vector[0]
            };
            
            Console.WriteLine($"Player located at X:{NewPlayerLocation.X}Y:{NewPlayerLocation.Y}");
            switch (NewLocationData.WhatListToSearch)
            {
                case 0:
                    Console.WriteLine("Encountered Object");
                    if (SearchObjects(NewLocationData.WhatIsHereID, CurrentPlayerLocation, NewPlayerLocation))
                    {
                        CSHARPRPG.RPGame.PlayerLocation = NewPlayerLocation;
                    }
                    break;
                case 1:
                    Console.WriteLine("Encountered Enemy");
                    if (SearchActors(NewLocationData.WhatIsHereID, CurrentPlayerLocation, NewPlayerLocation))
                    {
                        CSHARPRPG.RPGame.PlayerLocation = NewPlayerLocation;
                    }
                    break;
                case 2:
                    Console.WriteLine("Encountered Store");
                    if (SearchStores(NewLocationData.WhatIsHereID, CurrentPlayerLocation, NewPlayerLocation))
                    {
                        CSHARPRPG.RPGame.PlayerLocation = NewPlayerLocation;
                    }
                    break;
                default:
                    Console.WriteLine("This location doesn't have anything to search.");
                    break;
            }
        }
        public static bool SearchActors(int ActorID, Location PrevLocation, Location AttemptLocation)
        {
            var result = ActorHandle.BeginFight(CSHARPRPG.RPGame.PlayerCharacter, ActorHandle.ActorList.Find(_ => _.ActorID == ActorID), PrevLocation, CSHARPRPG.RPGame.PlayerLocation);
            if (result)
            {
                ObjectHandleSpace.ObjectStruc.SetLocationToObject(AttemptLocation, 0);
                return true;
            }
            return false;
        }
        public static bool SearchObjects(int ObjectID, Location PrevLocation, Location AttemptLocation)
        {
            var Object = ObjectHandleSpace.ObjectStruc.ObjectList.FirstOrDefault(_ => _.ObjectID == ObjectID);
            Console.WriteLine(Object?.ObjectName);

            if (Object != null)
            {
                if (Object.ObjectIsDangerous && Object.ObjectHealth > 0)
                {
                    Console.WriteLine($"You step onto a {Object.ObjectName}. It's dangerous! \nIt deals {Object.ObjectStrength} damage.");
                    CSHARPRPG.RPGame.PlayerCharacter.ActorHealth -= Object.ObjectStrength;
                    Console.WriteLine($"You have {CSHARPRPG.RPGame.PlayerCharacter.ActorHealth} HP left!");
                }
                else if (!Object.CanPass && Object.ObjectHealth > 0)
                {
                    Console.WriteLine($"Your teeth are smashed by {Object.ObjectName}. Its strength is {Object.ObjectHealth}. \n1: Attack \n2: Leave");
                    var choice = Console.ReadKey().KeyChar;

                    if (choice == '1')
                    {
                        var Attack = CSHARPRPG.RPGame.PlayerCharacter.Attacks.MaxBy(_ => _.AttackStrength);
                        Console.WriteLine($"You use {Attack.AttackTitle.TitleName} {Attack.AttackName} against the {Object.ObjectName}.");
                        if (Attack.AttackStrength >= Object.ObjectHealth){
                            Console.WriteLine($"Destroyed the {Object.ObjectName} Reducing it to rubble!");

                            ObjectHandleSpace.ObjectStruc.SetLocationToObject(AttemptLocation, 4);
                            return true;
                        } else {
                            Console.WriteLine($"The {Object.ObjectName} is too tough! Grow stronger to take this down!");
                        }
                        
                    } 
                } else if (Object.CanPass || (Object.ObjectIsDangerous && Object.ObjectHealth <= 0)){
                    return true;
                }
                return false;
            }
            return false;
        }
        public static bool SearchStores(int StoreID, Location PrevLocation, Location AttemptLocation)
        {
            StoreHandleSpace.StoreStruc.StoreList.Find(_ => _.StoreID == StoreID);
            StoreHandleSpace.StoreStruc.ViewStoreItems(StoreID);
            return false;
        }
    };
}
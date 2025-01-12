namespace StoreHandleSpace
{   
    public class StorePotions
    {
        public int PotionsID { get; set; }
        public string PotionsTitle { get; set; }
        public string PotionsName { get; set; }
        public string PotionsEffect { get; set; }
        public string PotionsEffectTarget { get; set; }
        public int PotionsStrength { get; set; }
        public int PotionsCost { get; set; }

        public static Dictionary<int, Dictionary<string, string>> PotionTitles = new Dictionary<int, Dictionary<string, string>>
        {
            { 0, new Dictionary<string, string> { {"Virulent","Increase Effect by 10%"} } },
            { 1, new Dictionary<string, string> { {"Unsealed","Effect will affect user"} } },
            { 2, new Dictionary<string, string> { {"Diluted","Effect is diminished"} } },
            { 3, new Dictionary<string, string> { {"Unlicensed","Effect randomised with chance at extra effect"} } },
            { 4, new Dictionary<string, string> { {"Factory new","Gold value increased"} } },
        };
        public static Dictionary<int, Dictionary<string, int>> PotionEffects = new Dictionary<int, Dictionary<string, int>>
        {
            {0, new Dictionary<string, int> { {"IncreaseLevel", 1     } }},
            {1, new Dictionary<string, int> { {"IncreasePhysDefense",5} }},
            {2, new Dictionary<string, int> { {"IncreaseMagDefense",5 } }},
            {3, new Dictionary<string, int> { {"IncreaseAttacks",1    } }},
            {4, new Dictionary<string, int> { {"IncreaseStat",2       } }},
            {5, new Dictionary<string, int> { {"Teleport",10          } }},
            {6, new Dictionary<string, int> { {"Deal Random physical Damage",20          } }},
            {7, new Dictionary<string, int> { {"Deal Random physical Damage",20          } }},
            {8, new Dictionary<string, int> { {"Damage 10% enemy health",20         } }},
            {9, new Dictionary<string, int> { {"Put enemy attack on cooldown",20         } }},
        };
        public static Dictionary<int, Dictionary<string, string>> PotionsNames = new Dictionary<int, Dictionary<string, string>>
        {
            { 0, new Dictionary<string, string> { { "FlaskBottle", "TargetSelf" } } },
            { 1, new Dictionary<string, string> { { "DaggerBottle", "DamageTarget" } } },
            { 2, new Dictionary<string, string> { { "WispBottle", "TargetAll" } } }
        };
        
        public static StorePotions[] GeneratePotions(Random random)
        {
            List<StorePotions> GeneratedPotions = new List<StorePotions>();
            int Level = CSHARPRPG.RPGame.StartLevel;
            for ( int i = 0; i < Math.Min(8, Level * 2); i++)
            {
                var randomKeyTitle = random.Next(1, 5);
                var randomKeyName = random.Next(0, 3);
                var randomKeyEffect = random.Next(0, 10);

                var randomTitle = PotionTitles[randomKeyTitle];
                var randomBottle = PotionsNames[randomKeyName];
                var randomEffect = PotionEffects[randomKeyEffect];

                var title = randomTitle.Keys.First();

                var potionName = randomBottle.Keys.First();
                var potionEffectTarget = randomBottle.Values.First();

                var potionEffect = randomEffect.Keys.First();
                var potionEffectStrength = randomEffect.Values.First();
                

                GeneratedPotions.Add(new()
                {
                    PotionsID = i,
                    PotionsTitle = title,
                    PotionsName = potionName,
                    PotionsEffect = potionEffect,
                    PotionsEffectTarget = potionEffectTarget,
                    PotionsStrength = random.Next(1, potionEffectStrength * Level * 2),
                    PotionsCost = random.Next(10, potionEffectStrength * Level * 5),
                });                
            }


            return GeneratedPotions.ToArray();
        }
    }

    public class StoreStruc
    {
        public int StoreID { get; set; }
        public int StoreHealth { get; set; }
        public string StoreName { get; set; }
        public string StoreType { get; set; }
        public string StoreRepresentation { get; set; }
        public bool StoreIsDangerous { get; set; }
        public int StoreStrength { get; set; }
        public int StoreGold { get; set; }
        public static List<StoreStruc> StoreList = [];
        public StorePotions[] StorePotions { get; set; }

        public static string[] StoreNameList = { "Jerries", "Prabalast", "Arms Co", "General Store" };
        public static string[] StoreTypeList = { "UnwantedGoods", "Potions", "Crap", "Miscellaneous items for perplexing scenarios" };
        
        public static StoreStruc GenerateStore(Random random)
        {
            int Level = CSHARPRPG.RPGame.StartLevel;

            StoreStruc GeneratedStore = new StoreStruc(){
                StoreID = StoreList.Count(),
                StoreHealth = random.Next(5, Level * 2 + 5),
                StoreName = StoreNameList[random.Next(0, StoreNameList.Length)],
                StoreType = StoreTypeList[random.Next(0, StoreTypeList.Length)],
                StoreRepresentation = "ST",
                StoreIsDangerous = random.NextDouble() > 0.8,
                StoreStrength = random.Next(1, Level / 2),
                StoreGold = random.Next(100, Level * 100),
                StorePotions = StoreHandleSpace.StorePotions.GeneratePotions(random) 
            };
            StoreList.Add(GeneratedStore);
            
            return GeneratedStore;
        }
        public static void ViewStoreItems(int StoreID)
        {
            var Store = StoreList.FirstOrDefault(_ => _.StoreID == StoreID);
            bool WantLeave = false;
            if (Store != null)
            {            
                do
                {
                    Console.WriteLine("----------------------------------------------");
                    Console.WriteLine($"Welcome stranger to {Store.StoreName} {Store.StoreType} what can i do your mom for, Ge ha HARCK \n Type a potion ID to Buy Type L to Leave");
                    foreach (var Potion in Store.StorePotions)
                    {   
                        Console.WriteLine($"SELECTION ID: {Potion.PotionsID}");
                        Console.WriteLine($"-- {Potion.PotionsName} -- Effect {Potion.PotionsEffect} Targets {Potion.PotionsEffectTarget}");
                        Console.WriteLine($"-- Title {Potion.PotionsTitle} -- Strength {Potion.PotionsStrength} Cost {Potion.PotionsCost}");
                        Console.WriteLine();
                    }
                    Console.WriteLine("what ya buyin?");
                    var Choice = Console.ReadLine();

                    if (int.TryParse(Choice, out int potionID) && potionID >= 0 && potionID < Store.StorePotions.Length)
                    {
                        var selectedPotion = Store.StorePotions[potionID];
                        if (CSHARPRPG.RPGame.PlayerCharacter.ActorGold >= selectedPotion.PotionsCost)
                        {   
                            selectedPotion.PotionsID = CSHARPRPG.RPGame.PlayerCharacter.ActorPotions.Count();
                            CSHARPRPG.RPGame.PlayerCharacter.ActorPotions = CSHARPRPG.RPGame.PlayerCharacter.ActorPotions.Append(selectedPotion).ToArray();
                            CSHARPRPG.RPGame.PlayerCharacter.ActorGold -= selectedPotion.PotionsCost;
                            Store.StoreGold += selectedPotion.PotionsCost;
                            Console.WriteLine($"You bought {selectedPotion.PotionsName}. Happy drinking!");      
                            List<StorePotions> UpdatedStorePotions = new List<StorePotions>(){};        
                            foreach (var Potion in Store.StorePotions)
                            {
                                if (Potion.PotionsID != selectedPotion.PotionsID)
                                {
                                    Potion.PotionsID = UpdatedStorePotions.Count();
                                    UpdatedStorePotions.Append(Potion);
                                }
                            }              
                        } else {
                            Console.WriteLine("Cant afford that one... mate...");
                        }

                    }
                    else if (Choice.ToUpper() == "L")
                    {
                        Console.WriteLine("See you later!");
                        WantLeave = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice. Try again.");
                    }           
                } while (!WantLeave);
            }
            Console.WriteLine("Bye now!");
        }
    }
}
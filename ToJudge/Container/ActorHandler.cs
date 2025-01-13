using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting;
using MapHandleSpace;
using StoreHandleSpace;

namespace ActorStructureSpace
{
    public class DamageTypes
    {
        public int Slash { get; set; }
        public int Strike { get; set; }
        public int Pierce { get; set; }
        public int Fire { get; set; }
        public int Water { get; set; }
        public int Earth { get; set; }
        public int Wind { get; set; }
        public int TrueProtection { get; set; }
    }
    public class AttackStruc
    {   // ATTACK STRUCTURE, ATTACKS WILL HAVE TITLES THAT MATCH ACTORS I.E. Vile Kled Uses Vile Strike
        public int AttackID { get; set; }
        public string AttackType { get; set; }
        public string AttackName { get; set; }
        public int AttackStrength { get; set; }
        public int AttackRange { get; set; }
        public required int CurrentCool { get; set; }
        public int MaxCool { get; set; }
        public ActorTitleStruc AttackTitle { get; set; }
        private static string[] AttackNamesMelee = [
            "Slash",
            "Punch",
            "Kick",
            "Stab",
            "Strike",
            "Tackle",
            "Bite",
            "Chop",
            "Headbutt",
            "Swing",
            "Thrust",
            "Smash",
            "Pound",
            "Bash",
            "Slam",
            "Lunge"
        ];
        private static string[] AttackNamesRanged = 
        {
            "Throw",
            "Launch",
            "Chuck",
            "Shoot",
            "Hurl",
            "Fling",
            "Toss",
            "Fire",
            "Aim",
            "Pierce",
            "Strike",
            "Volley",
            "Arrow Shot",
            "Projectile",
            "Sniping",
            "Quick Shot",
            "Ricochet",
            "Blast Shot",
            "Arc Shot",
            "Precision Strike"
        };

        private static string[] AttackNamesMagicSorcery = 
        {
            "Blast",
            "Implosion",
            "Bolt",
            "Rain",
            "Explosion",
            "Rune",
            "Arcane Burst",
            "Magic Orb",
            "Mana Surge",
            "Spell Strike",
            "Frost Wave",
            "Flame Spiral",
            "Lightning Storm",
            "Void Tear",
            "Chaos Pulse",
            "Elemental Bolt",
            "Spirit Nova",
            "Mystic Ray",
            "Energy Surge",
            "Phantom Strike"
        };

        private static string[] AttackNamesHoly = 
        {
            "Guidance",
            "Miracle",
            "Intervention",
            "Holy Words",
            "AllMighty",
            "Divine Strike",
            "Sacred Flame",
            "Blessing Wave",
            "Angelic Strike",
            "Radiance",
            "Graceful Impact",
            "Sanctuary Blast",
            "Heavenly Slash",
            "Purity Ray",
            "Virtuous Blow",
            "Light Barrage",
            "Seraphic Strike",
            "Hallowed Burst",
            "Luminous Wrath",
            "Ethereal Guidance"
        };
        private static string[] DamageTypes = [
            "Slash",
            "Strike",
            "Pierce",
            "Fire",
            "Water",
            "Earth",
            "Wind",
            "TrueProtection",
        ];
        public static AttackStruc GenerateAttack(Random random, int AttackID, int ActorClass)
        {
            string[][] AttackList = [AttackNamesMelee, AttackNamesRanged, AttackNamesMagicSorcery, AttackNamesHoly];
            var AttackRangeMax = ActorClass == 1 ? 20 : 5;
            var ChosenAttackList = AttackList[ActorClass];

            int Level = CSHARPRPG.RPGame.StartLevel;
            AttackStruc GeneratedAttack = new AttackStruc(){
                AttackID = AttackID,
                AttackTitle = ActorTitleStruc.GenerateTitle(random),
                AttackName = ChosenAttackList[random.Next(0, ChosenAttackList.Length)],
                AttackRange = random.Next(1, AttackRangeMax + Level * 4),
                CurrentCool = 0,
                MaxCool = random.Next(2, 5),
                AttackStrength = random.Next(1, Level * 5),
                AttackType = DamageTypes[random.Next(0, DamageTypes.Length)]
            };
            return GeneratedAttack;
        }
    }
    public class ActorClassStruc
    {   // EFFECT DAMAGETYPES TOWARDS OTHER CLASSES
        public string ClassName { get; set; }
        public string ClassEffect { get; set; }
        public int ClassStrength { get; set; }
        public int BaseHealth { get; set; }
        public int BaseSpeed { get; set; }
        public int ClassType { get; set; }
        public DamageTypes BaseArmor { get; set; }

        public static ActorClassStruc GenerateClass(Random random)
        {
            ActorClassStruc GeneratedClass = ActorHandle.ClassList[random.Next(0, ActorHandle.ClassList.Length)];
            GeneratedClass.ClassStrength = random.Next(0,1);

            return GeneratedClass;
        }
    }
    public class ActorTitleStruc
    {   // EFFECT DAMAGE AND RESISTANCE IN RANDOM
        public string TitleName { get; set; }
        public string TitleEffect { get; set; }
        public int TitleStrength { get; set; }

        public static ActorTitleStruc GenerateTitle(Random random)
        {
            ActorTitleStruc GeneratedTitle = ActorHandle.TitleList[random.Next(0, ActorHandle.TitleList.Length)];
            GeneratedTitle.TitleStrength = random.Next(0,1);

            return GeneratedTitle;
        }
    }
    public class ActorStruc
    {
        public int ActorID { get; set; }
        public string ActorRepresentation { get; set; }
        public int ActorLevel { get; set; }
        public int CurrentXP { get; set; }
        public int XPToNextLevel { get; set; }
        public int ActorHealth { get; set; }
        public int ActorSpeed { get; set; }
        public int ActorGold { get; set; }
        public string ActorName { get; set; }
        public DamageTypes ActorArmour { get; set; }        
        public ActorClassStruc ActorClass { get; set; }
        public ActorTitleStruc ActorTitle { get; set; }
        public AttackStruc[] Attacks { get; set; }
        public StorePotions[] ActorPotions { get; set; }
        public static int CalculateDamage(AttackStruc ChosenAttack, ActorStruc ActorAttacker, ActorStruc ActorDefender)
        {   //Atk / (Def+100 / 100)
            var Damage = ChosenAttack.AttackStrength;
            
            if (ChosenAttack.AttackType == "TrueProtection")
            {
                Damage += ChosenAttack.AttackStrength - ActorDefender.ActorArmour.TrueProtection * ActorDefender.ActorLevel;
            }
            
            // CALCULATE ARMOUR DAMAGE REDUCTION
            switch (ChosenAttack.AttackType)
            {
                case "Slash": CalculateArmourReduction(Damage, ActorDefender.ActorArmour.Slash) ;  break;
                case "Strike": CalculateArmourReduction(Damage, ActorDefender.ActorArmour.Strike);  break;
                case "Pierce": CalculateArmourReduction(Damage, ActorDefender.ActorArmour.Pierce);  break;
                case "Earth": CalculateArmourReduction(Damage, ActorDefender.ActorArmour.Earth);  break;
                case "Wind": CalculateArmourReduction(Damage, ActorDefender.ActorArmour.Wind);  break;
                case "Fire": CalculateArmourReduction(Damage, ActorDefender.ActorArmour.Fire);  break;
                case "Water": CalculateArmourReduction(Damage, ActorDefender.ActorArmour.Water);  break;
            }
            int CalculateArmourReduction(int Damage, int Resist)
            { 
                int DamageLost = (Damage / 100) * Resist;
                return Damage -= DamageLost;
            }
            // CALCULATE CLASS CHANGES
            var classInteractions = new Dictionary<string, Dictionary<string, double>>
            {
                { "Warrior", new Dictionary<string, double> { { "Defender", 1.2 }, { "Rogue", 0.8 } } },
                { "Rogue", new Dictionary<string, double> { { "Defender", 1.5 }, { "Warrior", 0.8 } } },
                { "Defender", new Dictionary<string, double> { { "Warrior", 0.5 }, { "Rogue", 1.2 } } },
                { "Archer", new Dictionary<string, double> { { "Rogue", 1.3 }, { "Defender", 0.9 } } },
                { "Berserker", new Dictionary<string, double> { { "Archer", 1.5 }, { "Defender", 1.1 } } },
                { "Mage", new Dictionary<string, double> { { "Warrior", 1.4 }, { "Rogue", 0.8 } } },
                { "Priest", new Dictionary<string, double> { { "Demon", 1.5 }, { "Rogue", 0.8 } } },
                { "Demon", new Dictionary<string, double> { { "Priest", 1.5 }, { "Hero", 0.8 } } },
                { "Hero", new Dictionary<string, double> { { "Demon", 1.5 }, { "Rogue", 1.1 } } }
            };

            if (classInteractions.TryGetValue(ActorAttacker.ActorClass.ClassName, out var defenderEffects) &&
                defenderEffects.TryGetValue(ActorDefender.ActorClass.ClassName, out var effectMultiplier))
            {
                Damage = (int)Math.Floor(Damage * effectMultiplier);
            }
            // KURWA~!
            switch (ChosenAttack.AttackTitle.TitleName)
            {
                case "Brave" :
                    if (ActorDefender.ActorLevel > ActorAttacker.ActorLevel) {Damage = Damage / 100 * 90;};
                break;
                case "Strong" :
                    if (ChosenAttack.AttackRange == 1) {Damage = Damage / 100 * 110;};
                break;
                case "Swift" :
                    if (ChosenAttack.AttackRange == 1) {Damage = Damage * 2 / 100 * 110;};
                break;
                case "Resilient" :
                    ActorAttacker.ActorHealth += ActorAttacker.ActorHealth / 100 * 5;
                break;
                case "Vile" :
                    if (ActorDefender.ActorLevel < ActorAttacker.ActorLevel || ActorDefender.ActorGold < ActorAttacker.ActorGold){ Damage = Damage * 2 / 100 * 110;};
                break;
                case "Fierce" :
                    Random random = new Random();
                    if (random.Next(0,1) >= 0.9){
                        Damage *= 2;
                    };
                break;
                case "Bole" :
                    if (ActorDefender.ActorClass.ClassName == "Bole"){ Damage *= 2;};
                break;
                case "Iron" :
                    Damage += Damage * 2 / 100 * 110;
                break;
                case "Merciless" :
                    if (ActorDefender.ActorHealth <= ActorAttacker.ActorHealth / 2){ Damage = Damage * 2 / 100 * 110; };
                break;
                case "Steadfast" :  
                    if (ActorDefender.ActorSpeed > ActorAttacker.ActorSpeed ){ Damage = Damage * 2 / 100 * 110; };
                break;
            }
            return Damage;
        }
        public static void ApplyCooling(AttackStruc[] Attacks)
        {
            foreach (var Attack in Attacks)
            {
                Attack.CurrentCool = Math.Max(0, Attack.CurrentCool - 1);
            }
        }
        public static void HandleTeleport(Location Destination, ActorStruc Target, bool IsPlayer)
        {
            Random random = new Random();

            var TeleportDestinationY = Math.Min(CSHARPRPG.RPGame.MapHeight - 1, Destination.Y);
            var TeleportDestinationX = Math.Min(CSHARPRPG.RPGame.MapWidth - 1, Destination.X);

            if (Destination.Y < 0 || Destination.X < 0){
                TeleportDestinationY = random.Next(0, CSHARPRPG.RPGame.MapHeight);
                TeleportDestinationX = random.Next(0, CSHARPRPG.RPGame.MapWidth);
            }
            // Check if something is there that is dangerous or solid, if solid kill Target unless target is beastly
            var TargetCurrentLocation = MapHandler.Map.SelectMany(row => row).FirstOrDefault(cell => cell.WhatIsHereID == Target.ActorID && cell.WhatListToSearch == 1);
            if (TargetCurrentLocation == null && !IsPlayer)
            {
                Console.WriteLine("Target's current location could not be found.");
                return;
            }
            var WhatIsThere = MapHandler.Map[TeleportDestinationY][TeleportDestinationX];
            Location WhatIsThereLocation = WhatIsThere.Location;
            var TargetLocation = IsPlayer ? CSHARPRPG.RPGame.PlayerLocation : TargetCurrentLocation.Location;

            ObjectHandleSpace.ObjectStruc.SetLocationToObject(TargetLocation, 4);

            switch (WhatIsThere.WhatListToSearch)
            {
                case 0:
                    var ObjectHere = ObjectHandleSpace.ObjectStruc.ObjectList.First(_ => _.ObjectID == WhatIsThere.WhatIsHereID);  
                    if (ObjectHere.ObjectIsDangerous)
                    {
                        if (ObjectHere.ObjectStrength >= Target.ActorHealth)
                        {
                            Target.ActorHealth = 0;
                            Console.WriteLine($"{Target.ActorName} Died during teleport due to {ObjectHere.ObjectName}");
                        }
                        else 
                        {
                            
                            if (IsPlayer){
                                CSHARPRPG.RPGame.PlayerLocation = new Location(){X = TeleportDestinationX ,Y = TeleportDestinationY};
                            } else {
                                ActorHandle.SetLocationToActor(TargetLocation, Target.ActorID);
                            }
                            Console.WriteLine($"{Target.ActorName} Teleported to X{TargetLocation.X}Y{TargetLocation.Y}");
                        }
                    }     
                break;
                case 1:
                    var EnemyHere = ActorHandle.ActorList.First(_ => _.ActorID == WhatIsThere.WhatIsHereID); 
              
                    if (EnemyHere.ActorHealth >= Target.ActorHealth)
                    {
                        Target.ActorHealth = 0;
                        Console.WriteLine($"{Target.ActorName} Died during teleport due to {EnemyHere.ActorName}");
                    }
                    else 
                    {
                        if (IsPlayer){
                            CSHARPRPG.RPGame.PlayerLocation = new Location(){X = TeleportDestinationX ,Y = TeleportDestinationY};
                        } else {
                            ActorHandle.SetLocationToActor(TargetLocation, Target.ActorID);
                        }
                        Console.WriteLine($"{Target.ActorName} Teleported to X{TargetLocation.X}Y{TargetLocation.Y}");
                    }
                       
                break;
                case 2:
                    var StoreHere = StoreHandleSpace.StoreStruc.StoreList.FirstOrDefault(_ => _.StoreID == WhatIsThere.WhatIsHereID); 
                    if (StoreHere.StoreIsDangerous)
                    {
                        if (StoreHere.StoreStrength >= Target.ActorHealth)
                        {
                            Target.ActorHealth = 0;
                            Console.WriteLine($"{Target.ActorName} Died during teleport due to {StoreHere.StoreName}");
                        }
                        else 
                        {
                            if (IsPlayer){
                                CSHARPRPG.RPGame.PlayerLocation = new Location(){X = TeleportDestinationX ,Y = TeleportDestinationY};
                            } else {
                                ActorHandle.SetLocationToActor(TargetLocation, Target.ActorID);
                            }
                            Console.WriteLine($"{Target.ActorName} Teleported to X{TargetLocation.X}Y{TargetLocation.Y}");
                        }
                    }   
                break;
            }
        }
        public static bool UsePotion(StoreHandleSpace.StorePotions PotionSelected, ActorStruc PlayerCharacter, ActorStruc NpcDefender)
        {
            bool usedTeleport = true;
            switch (PotionSelected.PotionsEffectTarget)
            {
                case "TargetSelf":   usedTeleport = HandlePotionUse(PotionSelected, [PlayerCharacter], PlayerCharacter, true); break;
                case "DamageTarget": usedTeleport = HandlePotionUse(PotionSelected, [NpcDefender], PlayerCharacter, false); break;
                case "TargetAll":    usedTeleport = HandlePotionUse(PotionSelected, [PlayerCharacter, NpcDefender], PlayerCharacter, true); break;
            }
            return usedTeleport;
        }
        public static bool HandlePotionUse(StoreHandleSpace.StorePotions PotionSelected, ActorStruc[] Targets, ActorStruc PotionOwner, bool TargetPlayer)
        {
            Random random = new Random();
            bool usedTeleport = false;

            foreach (var Target in Targets)
            {
                switch (PotionSelected.PotionsEffect)
                {
                    case "IncreaseLevel": Target.ActorLevel += PotionSelected.PotionsStrength; break;
                    case "IncreasePhysDefense": 
                            Target.ActorArmour.Slash += PotionSelected.PotionsStrength / 4;
                            Target.ActorArmour.Strike += PotionSelected.PotionsStrength / 4;
                            Target.ActorArmour.Pierce += PotionSelected.PotionsStrength / 4;
                            Console.WriteLine($"{PotionSelected.PotionsName} Increased {Target.ActorName} Phy resist by {PotionSelected.PotionsStrength}!");
                    break;
                    case "IncreaseMagDefense":                             
                            Target.ActorArmour.Earth += PotionSelected.PotionsStrength / 4;
                            Target.ActorArmour.Fire += PotionSelected.PotionsStrength / 4;
                            Target.ActorArmour.Water += PotionSelected.PotionsStrength / 4;
                            Target.ActorArmour.Wind += PotionSelected.PotionsStrength / 4;
                            Console.WriteLine($"{PotionSelected.PotionsName} Increased {Target.ActorName} Mag resist by {PotionSelected.PotionsStrength}!");
                    break;
                    case "IncreaseAttacks": 
                        List<AttackStruc> NewAttacks = new List<AttackStruc>(){};
                        for (int i = 0; i < Target.Attacks.Length; i++)
                        {
                            NewAttacks.Add(Target.Attacks[i]);
                        }
                        NewAttacks.Add(AttackStruc.GenerateAttack(random, Target.ActorLevel * PotionSelected.PotionsStrength / 2, Target.ActorClass.ClassType));
                        Target.Attacks = NewAttacks.ToArray();
                        Console.WriteLine($"{PotionSelected.PotionsName} Gave {Target.Attacks[Target.Attacks.Length - 1].AttackName} to {Target.ActorName}!");
                    break;
                    case "IncreaseStat": 
                        if (Target.Attacks.Length > 0)
                        {
                            Target.Attacks[random.Next(0, Target.Attacks.Length)].AttackRange += PotionSelected.PotionsStrength;
                            Target.Attacks[random.Next(0, Target.Attacks.Length)].AttackStrength += PotionSelected.PotionsStrength;
                            Console.WriteLine($"{PotionSelected.PotionsName} Randomly increased attack stats!");
                        }
                        else
                        {
                            Console.WriteLine($"{Target.ActorName} has no attacks to modify!");
                        }
                    break;
                    case "Teleport": 
                        Location Destination = new Location(){ X = -1, Y = -1};
                        HandleTeleport(Destination, Target, TargetPlayer);
                        usedTeleport = true;
                        Console.WriteLine($"{PotionSelected.PotionsName} Teleports {Target.ActorName} randomly to a new location!");
                    break;
                    case "Deal Random physical Damage": 
                        var Damage = random.Next(5, PotionSelected.PotionsStrength * 2);
                        Target.ActorHealth -= Damage - Target.ActorArmour.TrueProtection;
                        Console.WriteLine($"{PotionSelected.PotionsName} Damages {Target.ActorName} by {Damage} Points reducing their health to {Target.ActorHealth}!");
                    break;
                    case "Damage 10% enemy health": 
                        var PercentHealth = Target.ActorHealth / 100 * 10;
                        Target.ActorHealth -= PercentHealth;
                        Console.WriteLine($"{PotionSelected.PotionsName} Takes 10% of {Target.ActorName}'s Health!");
                    break;
                    case "Put enemy attack on cooldown": 
                        Target.Attacks[random.Next(0, Target.Attacks.Length)].CurrentCool = Target.Attacks[random.Next(0, Target.Attacks.Length)].MaxCool;
                        Console.WriteLine($"{PotionSelected.PotionsName} Exhausts {Target.ActorName} randomly adding cooldown to their attacks!");
                    break;
                }                
            }
            Console.WriteLine($"{PotionSelected.PotionsName} Used!");
            // FAILS HERE FAILS HERE FAILS HERE FAILS HERE FAILS HERE
            List<StorePotions> NewPotions = new List<StorePotions>();
            foreach(var Potion in PotionOwner.ActorPotions)
            {
                if (Potion.PotionsID != PotionSelected.PotionsID)
                {
                    Potion.PotionsID = NewPotions.Count();
                    NewPotions.Add(Potion);
                }
            }
            PotionOwner.ActorPotions = NewPotions.ToArray();

            return usedTeleport;
        }   
    }

    public class ActorHandle
    {
        public static ActorTitleStruc[] TitleList = 
        {
            new()
            {
                TitleName = "Regular",
                TitleEffect = "No added effect",
                TitleStrength = 1
            },
            new()
            {
                TitleName = "Brave",
                TitleEffect = "Takes 10% less damage from stronger foes",
                TitleStrength = 1
            },
            new()
            {
                TitleName = "Strong",
                TitleEffect = "Deals 15% extra damage in close combat",
                TitleStrength = 1
            },
            new()
            {
                TitleName = "Swift",
                TitleEffect = "Increases speed by 20% during combat",
                TitleStrength = 1
            },
            new()
            {
                TitleName = "Resilient",
                TitleEffect = "Regenerates 5% health every turn",
                TitleStrength = 1
            },
            new()
            {
                TitleName = "Vile",
                TitleEffect = "Does 150% damage to weaker or unfortunate foes",
                TitleStrength = 1
            },
            new()
            {
                TitleName = "Fierce",
                TitleEffect = "Increases critical hit chance by 10%",
                TitleStrength = 1
            },
            new()
            {
                TitleName = "Bole",
                TitleEffect = "200% Damage to other Boles",
                TitleStrength = 1
            },
            new()
            {
                TitleName = "Iron",
                TitleEffect = "10% Damage Increase",
                TitleStrength = 1
            },
            new()
            {
                TitleName = "Bole",
                TitleEffect = "Extra damage against filthy Boles",
                TitleStrength = 1
            },
            new()
            {
                TitleName = "Merciless",
                TitleEffect = "Deals 20% extra damage to wounded enemies",
                TitleStrength = 1
            },
            new()
            {
                TitleName = "Steadfast",
                TitleEffect = "Takes 10% less damage from ranged attacks",
                TitleStrength = 1
            },
        };
        
        public static ActorClassStruc[] ClassList = 
        {
            new()
            {
                ClassName = "Regular",
                ClassEffect = "No effect",
                ClassStrength = 1,
                ClassType = 0,
                BaseHealth = 100,
                BaseSpeed = 10,
                BaseArmor = new DamageTypes()
                {
                    Slash = 20,
                    Strike = 20,
                    Pierce = 20,
                    Fire = 20,
                    Water = 20,
                    Earth = 20,
                    Wind = 20,
                    TrueProtection = 0,
                }
            },
            new()
            {
                ClassName = "Warrior",
                ClassEffect = "Deals 120% damage to Defenders, but takes 120% damage from Rogues",
                ClassStrength = 1,
                ClassType = 0,
                BaseHealth = 120,
                BaseSpeed = 8,
                BaseArmor = new DamageTypes()
                {
                    Slash = 30,
                    Strike = 30,
                    Pierce = 10,
                    Fire = 20,
                    Water = 10,
                    Earth = 15,
                    Wind = 10,
                    TrueProtection = 0,
                }
            },
            new()
            {
                ClassName = "Defender",
                ClassEffect = "Takes 50% less damage from Warriors, but deals 80% damage to Rogues",
                ClassStrength = 1,
                ClassType = 0,
                BaseHealth = 150,
                BaseSpeed = 6,
                BaseArmor = new DamageTypes()
                {
                    Slash = 40,
                    Strike = 50,
                    Pierce = 30,
                    Fire = 15,
                    Water = 10,
                    Earth = 20,
                    Wind = 10,
                    TrueProtection = 0,
                }
            },
            new()
            {
                ClassName = "Rogue",
                ClassEffect = "Deals 150% damage to Defenders, but takes 120% damage from Warriors",
                ClassStrength = 1,
                ClassType = 1,
                BaseHealth = 80,
                BaseSpeed = 15,
                BaseArmor = new DamageTypes()
                {
                    Slash = 10,
                    Strike = 10,
                    Pierce = 30,
                    Fire = 25,
                    Water = 20,
                    Earth = 10,
                    Wind = 20,
                    TrueProtection = 0,
                }
            },
            new()
            {
                ClassName = "Archer",
                ClassEffect = "Deals 130% damage to Rogues, but takes 90% damage from Defenders",
                ClassStrength = 1,
                ClassType = 1,
                BaseHealth = 100,
                BaseSpeed = 12,
                BaseArmor = new DamageTypes()
                {
                    Slash = 10,
                    Strike = 10,
                    Pierce = 30,
                    Fire = 20,
                    Water = 15,
                    Earth = 10,
                    Wind = 20,
                    TrueProtection = 0,
                }
            },
            new()
            {
                ClassName = "Berserker",
                ClassEffect = "Deals 150% damage to Archers, but takes 110% damage from Defenders",
                ClassStrength = 1,
                ClassType = 0,
                BaseHealth = 130,
                BaseSpeed = 10,
                BaseArmor = new DamageTypes()
                {
                    Slash = 25,
                    Strike = 20,
                    Pierce = 20,
                    Fire = 25,
                    Water = 10,
                    Earth = 15,
                    Wind = 20,
                    TrueProtection = 0,
                }
            },
            new()
            {
                ClassName = "Mage",
                ClassEffect = "Deals 140% damage to Warriors, but takes 120% damage from Rogues",
                ClassStrength = 1,
                ClassType = 2,
                BaseHealth = 70,
                BaseSpeed = 10,
                BaseArmor = new DamageTypes()
                {
                    Slash = 5,
                    Strike = 5,
                    Pierce = 10,
                    Fire = 40,
                    Water = 10,
                    Earth = 5,
                    Wind = 25,
                    TrueProtection = 0,
                }
            },
            new()
            {
                ClassName = "Priest",
                ClassEffect = "Deals 150% damage to Demons, but takes 120% damage from Rogues",
                ClassStrength = 1,
                ClassType = 3,
                BaseHealth = 90,
                BaseSpeed = 8,
                BaseArmor = new DamageTypes()
                {
                    Slash = 20,
                    Strike = 10,
                    Pierce = 15,
                    Fire = 10,
                    Water = 30,
                    Earth = 10,
                    Wind = 25,
                    TrueProtection = 1,
                }
            },
            new()
            {
                ClassName = "Demon",
                ClassEffect = "Deals 150% damage to Priests, but takes 120% damage from Heroes",
                ClassStrength = 1,
                ClassType = 3,
                BaseHealth = 120,
                BaseSpeed = 6,
                BaseArmor = new DamageTypes()
                {
                    Slash = 30,
                    Strike = 40,
                    Pierce = 20,
                    Fire = 50,
                    Water = 15,
                    Earth = 10,
                    Wind = 5,
                    TrueProtection = 5,
                }
            },
            new()
            {
                ClassName = "Hero",
                ClassEffect = "Deals 150% damage to Demons, but takes 110% damage from Rogues",
                ClassStrength = 1,
                ClassType = 0,
                BaseHealth = 100,
                BaseSpeed = 12,
                BaseArmor = new DamageTypes()
                {
                    Slash = 20,
                    Strike = 20,
                    Pierce = 20,
                    Fire = 30,
                    Water = 10,
                    Earth = 10,
                    Wind = 15,
                    TrueProtection = 10,
                }
            }
        };

        public static string[] ActorNames = 
        {
            "Mortis Grimm",
            "Velka Crowe",
            "Igor Black",
            "Lilith Moore",
            "Raspen Hollow",
            "Sable Dusk",
            "Vornick Grimes",
            "Thalice Wood",
            "Eryk Graves",
            "Zarnak Cross",
            "Drusilla Thorn",
            "Gorath Smithson",
            "Hester Glover",
            "Fenwick Marsh",
            "Morgo Crane"
        };
        public static List<ActorStruc> ActorList = new List<ActorStruc>();

        public static void SetLocationToActor(Location location, int ActorIDToReturn){
            var Actor = ActorList[ActorIDToReturn];

            MapHandler.Map[location.Y][location.X] = new(){
                LocationID = location.X + location.Y,
                CanPass = false,
                Location = new Location() { X = location.X, Y = location.Y },
                WhatIsHereID = Actor.ActorID,
                WhatIsHereName = Actor.ActorName,
                WhatListToSearch = 0,
                WhatIsHereRepresentation = "EN"
            };
        }
        public static ActorStruc GenerateAndStoreActor(Random random)
        {
            int MaxLevel = CSHARPRPG.RPGame.StartLevel;
            int Level = random.Next(0, MaxLevel + 5);
            List<AttackStruc> Attacks = new List<AttackStruc>(){};
            ActorClassStruc ActorClass = ActorClassStruc.GenerateClass(random);
            
            for (int i = 0; i < random.Next(2, 6); i++)
            {
                Attacks.Add(AttackStruc.GenerateAttack(random, Attacks.Count, ActorClass.ClassType));
            };

            ActorStruc GeneratedActor = new ActorStruc(){
                ActorID = ActorList.Count + 2,
                ActorLevel = Level,
                CurrentXP = 0,
                ActorRepresentation = "EN",
                ActorHealth = 0,
                ActorSpeed = 0,
                ActorGold = random.Next(20, 20 + Level * 100),
                ActorName = ActorNames[random.Next(0, ActorNames.Length)],
                ActorTitle = ActorTitleStruc.GenerateTitle(random),
                ActorClass = ActorClass,
                Attacks = [.. Attacks]
            };
            GeneratedActor.ActorArmour = new DamageTypes
            {
                Slash = GetRandomArmorValue(GeneratedActor.ActorClass.BaseArmor.Slash),
                Strike = GetRandomArmorValue(GeneratedActor.ActorClass.BaseArmor.Strike),
                Pierce = GetRandomArmorValue(GeneratedActor.ActorClass.BaseArmor.Pierce),
                Fire = GetRandomArmorValue(GeneratedActor.ActorClass.BaseArmor.Fire),
                Water = GetRandomArmorValue(GeneratedActor.ActorClass.BaseArmor.Water),
                Earth = GetRandomArmorValue(GeneratedActor.ActorClass.BaseArmor.Earth),
                Wind = GetRandomArmorValue(GeneratedActor.ActorClass.BaseArmor.Wind),
                TrueProtection = GetRandomArmorValue(GeneratedActor.ActorClass.BaseArmor.TrueProtection),
            };         
            int GetRandomArmorValue(double baseValue)
            {
                int min = (int)Math.Floor(baseValue / 1.5);
                int max = (int)Math.Ceiling(baseValue);

                return min < max ? random.Next(min, max) : max;
            }

            GeneratedActor.ActorHealth = random.Next(GeneratedActor.ActorClass.BaseHealth / 2 , GeneratedActor.ActorClass.BaseHealth);
            GeneratedActor.ActorSpeed = random.Next(GeneratedActor.ActorClass.BaseSpeed / 2 , GeneratedActor.ActorClass.BaseSpeed);
            GeneratedActor.XPToNextLevel = GetXpToNextLevel(GeneratedActor.ActorLevel);

            ActorList.Add(GeneratedActor);

            return GeneratedActor;
        }
        public static ActorStruc LevelActor(ActorStruc Actor, bool isPlayer)
        {
            return new (){
                ActorID = Actor.ActorID,
                ActorLevel = Actor.ActorLevel += 1,
                CurrentXP = Actor.CurrentXP,
                XPToNextLevel = GetXpToNextLevel(Actor.ActorLevel + 1),
                ActorRepresentation = isPlayer ? "PL" : "EN",
                ActorHealth = Actor.ActorHealth += GetPercentOf(Actor.ActorHealth, 10),
                ActorSpeed = Actor.ActorSpeed += GetPercentOf(Actor.ActorSpeed, 10) + 1,
                ActorGold = Actor.ActorGold += 200,
                ActorName = Actor.ActorName,
                ActorTitle = Actor.ActorTitle,
                ActorClass = Actor.ActorClass,
                Attacks = LevelAttacks(Actor.Attacks)
            };

        }
        public static AttackStruc[] LevelAttacks(AttackStruc[] Attacks)
        {
            for (int i = 0; i < Attacks.Length; i++)
            {
                Attacks[i].AttackStrength += GetPercentOf(Attacks[i].AttackStrength, 10);
                Console.WriteLine(Attacks[i].AttackStrength);
            }
            return Attacks;
        }
        public static int GetPercentOf(int OriginalValue, int PercentOfOriginalValue)
        {
            return (OriginalValue * PercentOfOriginalValue) / 100;
        }
        public static int GetXpToNextLevel(int CurrentLevel)
        {
            return (int)(Math.Pow(CurrentLevel, 1.5) * 50);
        }
        public static ActorStruc GeneratePlayerCharacter()
        {
            bool Confirmed = false;

            ActorStruc PlayerCharacter = new ActorStruc(){
                ActorID = 0,
                ActorRepresentation = "PL",
                ActorGold = 100,
                ActorLevel = 5,
                CurrentXP = 0,
                XPToNextLevel = 5
            };
            Console.WriteLine("Welcome to the Character creator! What is your Name!");
            
            do
            {
                PlayerCharacter.ActorName = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(PlayerCharacter.ActorName) || PlayerCharacter.ActorName.Length > 10)
                {
                    Console.WriteLine("Invalid name. Please enter a name that is not empty and is less than 10 characters.");
                }
            } while (PlayerCharacter.ActorName == "" || PlayerCharacter.ActorName.Length > 10 || PlayerCharacter.ActorName == null);

            Console.WriteLine($"Welcome {PlayerCharacter.ActorName}! Now we would like you to select a class! \n this will determine your speed and attributes as well as other hidden bonuses and weaknesses");
            do
            {
                for (int i = 0; i < ClassList.Length; i++)
                {   
                    var CurrentClass = ClassList[i];
                    Console.WriteLine($"{i}: {CurrentClass.ClassName}, {CurrentClass.ClassEffect}");
                    Console.WriteLine("---------------------------------------------------------------------------------------------");
                }
                var choice = Console.ReadLine();
                Console.WriteLine();
                
                var ChosenClass = ClassList[int.Parse(choice)];
                
                if (ChosenClass == null || int.Parse(choice) > ClassList.Length || int.Parse(choice).GetType() != typeof(int)){
                    Console.WriteLine("Class doesnt exist or invalid input, im a computer i dont have eyes");
                } else {
                    Console.WriteLine($"You have selected to become a {ChosenClass.ClassName} confirm? \n 1: Yes \n 2: No");
                    var check = Console.ReadKey().KeyChar;
                    if (check == '1'){
                        PlayerCharacter.ActorClass = ChosenClass;
                        PlayerCharacter.ActorArmour = ChosenClass.BaseArmor;
                        PlayerCharacter.ActorHealth  = ChosenClass.BaseHealth;
                        PlayerCharacter.ActorSpeed = ChosenClass.BaseSpeed;
                        Confirmed = true;
                    } else {
                        Console.WriteLine(" Selected - Resending choices");
                    }                    
                }  
            } while (!Confirmed);
            
            Console.WriteLine($"Selected - You are now a {PlayerCharacter.ActorClass.ClassName}, Now what shall your title be?");
            Confirmed = false;
    
            do
            {
                for (int i = 0; i < TitleList.Length; i++)
                {   
                    var CurrentTitle = TitleList[i];
                    Console.WriteLine($"{i}: {CurrentTitle.TitleName}, {CurrentTitle.TitleEffect}");
                    Console.WriteLine("---------------------------------------------------------------------------------------------");
                }
                var choice = Console.ReadLine();
                Console.WriteLine();
                
                var ChosenTitle = TitleList[int.Parse(choice)];
                
                if (ChosenTitle == null || int.Parse(choice) > TitleList.Length){
                    Console.WriteLine("Title doesnt exist?");
                } else {
                    Console.WriteLine($"You have selected to become a {ChosenTitle.TitleName} confirm? \n 1: Yes \n 2: No");
                    var check = Console.ReadKey().KeyChar;
                    if (check == '1'){
                        PlayerCharacter.ActorTitle = ChosenTitle;
                        Confirmed = true;
                    } else {
                        Console.WriteLine(" Selected - Resending choices");
                    }                    
                }  
            } while (!Confirmed);

            Console.WriteLine($"Your Title is now {PlayerCharacter.ActorTitle.TitleName}, Now we select your attacks and the rest will be auto generated according to your class \n The attacks will come in bunches of 4 that will be auto generated, but do not worry you may refresh for better results 3 times");
            Confirmed = false;

            do
            {
                Random random = new Random();

                AttackStruc[] Attacks = new AttackStruc[4];

                for (int i = 0; i < 4; i++)
                {
                    Attacks[i] = AttackStruc.GenerateAttack(random, i, PlayerCharacter.ActorClass.ClassType);
                }
                foreach (var attack in Attacks)
                {
                    Console.WriteLine($"     Name: {attack.AttackTitle.TitleName} {attack.AttackName} Damage: {attack.AttackStrength}");
                    Console.WriteLine($"     Range: {attack.AttackRange} Type: {attack.AttackType} CD: {attack.MaxCool}");
                    Console.WriteLine();
                }
                Console.WriteLine("Do you like this selection? \n 1: Yes \n 2: No");
                var choice = Console.ReadKey(true).KeyChar.ToString().Split("");
                if (int.Parse(choice[0]) == 1)
                {
                    PlayerCharacter.Attacks = Attacks;
                    Confirmed = true;
                }
            } while (!Confirmed);

            Confirmed = false;
            Console.WriteLine("Are you Happy with this character, or would you like to reset \n 1: Yes 2: No");
            var Choice = Console.ReadKey().KeyChar;
            if (Choice == '1'){
                Console.WriteLine("Welcome, Here is your character sheet");
                DisplayActorData(PlayerCharacter, -1);
                Random random = new Random();
                PlayerCharacter.ActorPotions = StorePotions.GeneratePotions(random);
                DisplayActorPotions(PlayerCharacter);
            } else {
                Console.WriteLine("Not our fault, Try again");
                GeneratePlayerCharacter();
            }

            return PlayerCharacter;
        }
        
        public static ActorStruc GeneratePlayerSelection()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("Welcome Lich, to the Character Selection menu!");
            Console.WriteLine("Here, you can choose someones body to steal to or refresh for new options.");
            Console.WriteLine("First, please provide your name milord:");
            
            string? characterName = "";
            ActorStruc PlayerCharacter = new ActorStruc();

            do
            {
                characterName = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(characterName))
                {
                    Console.WriteLine("Name cannot be empty. Please try again:");
                }
            } while (string.IsNullOrWhiteSpace(characterName));
            
            Console.WriteLine($"Welcome, {characterName}! Here are your character options:");

            ActorStruc[] playerChoices = new ActorStruc[4];
            bool confirmed = false;
            Random random = new Random();

            do
            {
                for (int i = 0; i < 4; i++)
                {
                    playerChoices[i] = GenerateAndStoreActor(random);
                    DisplayActorData(playerChoices[i], i);
                }

                Console.WriteLine("Press 'r' to refresh the selection or type the ID (0-3) of your chosen character:");
                var ChoiceKey = Console.ReadKey();
                Console.WriteLine();
                
                if (ChoiceKey.KeyChar == 'r')
                {
                    Console.WriteLine("Refreshing character options...");
                    continue;
                }

                if (char.IsDigit(ChoiceKey.KeyChar))
                {
                    int selectedId = int.Parse(ChoiceKey.KeyChar.ToString());

                    if (selectedId >= 0 && selectedId < 4)
                    {
                        
                        Console.WriteLine($"You selected: {playerChoices[selectedId].ActorTitle.TitleName} {playerChoices[selectedId].ActorName}.");
                        Console.WriteLine($"Steal their body?? \n 1: Yes \n 2: No");
                        if (Console.ReadLine()?.Trim() == "1")
                        {
                            PlayerCharacter = playerChoices[selectedId];
                            PlayerCharacter.ActorName = characterName;                            
                            confirmed = true;
                        } else 
                        {
                            Console.WriteLine("Resetting...");
                        }
                        
                    }
                    else
                    {
                        Console.WriteLine("Invalid selection. Please choose a valid ID (0-3).");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please press 'r' to refresh or choose a valid ID (0-3).");
                }
            } while (!confirmed);

            Console.WriteLine($"Character selection confirmed! Welcome, {PlayerCharacter.ActorTitle.TitleName} {PlayerCharacter.ActorName}!");
            DisplayActorData(PlayerCharacter, -1);
            PlayerCharacter.ActorPotions = StorePotions.GeneratePotions(random);
            DisplayActorPotions(PlayerCharacter);
            return PlayerCharacter;
        }

        public static void DisplayActorData(ActorStruc actor, int DisplayID )
        {
            Console.WriteLine("---------------------------------------------------------------");
            if (DisplayID > -1){
                Console.WriteLine($"SELECTIONID: {DisplayID}");
            }
            Console.WriteLine($"Name: {actor.ActorTitle.TitleName} {actor.ActorName} \n Class: {actor.ActorClass.ClassName} Level: {actor.ActorLevel} XP TO NEXT: {actor.XPToNextLevel}");
            Console.WriteLine($"HP: {actor.ActorHealth} Spd: {actor.ActorSpeed} Gld: {actor.ActorGold}");
            Console.WriteLine("Armour Values");
                
            Console.WriteLine($"     Slash: {actor.ActorArmour.Slash}");
            Console.WriteLine($"     Strike: {actor.ActorArmour.Strike}");
            Console.WriteLine($"     Pierce: {actor.ActorArmour.Pierce}");
            Console.WriteLine($"     Fire: {actor.ActorArmour.Fire}");
            Console.WriteLine($"     Water: {actor.ActorArmour.Water}");
            Console.WriteLine($"     Earth: {actor.ActorArmour.Earth}");
            Console.WriteLine($"     Wind: {actor.ActorArmour.Wind}");
            Console.WriteLine($"     TrueProtection: {actor.ActorArmour.TrueProtection}");
            
            Console.WriteLine("Attacks: ");
            foreach (var attack in actor.Attacks)
            {
                Console.WriteLine($"     Name: {attack.AttackTitle.TitleName} {attack.AttackName} Damage: {attack.AttackStrength}");
                Console.WriteLine($"     Range: {attack.AttackRange} Type: {attack.AttackType} CD: {attack.MaxCool}");
                Console.WriteLine();
            }
            Console.WriteLine("---------------------------------------------------------------");
        }
        public static void DisplayActorPotions(ActorStruc Actor)
        {
            if (Actor.ActorPotions.Length == 0){
                Console.WriteLine($"{Actor.ActorName} Has no Potions!");
            } else {
                Console.WriteLine("----------------------------------------------");
                foreach (var Potion in Actor.ActorPotions)
                {
                    Console.WriteLine($"SELECTION ID: {Potion.PotionsID}");
                    Console.WriteLine($"-- {Potion.PotionsName} -- Effect {Potion.PotionsEffect} Targets {Potion.PotionsEffectTarget}");
                    Console.WriteLine($"-- Title {Potion.PotionsTitle} -- Strength {Potion.PotionsStrength} Cost {Potion.PotionsCost}");
                    Console.WriteLine();
                }
            }
        }
        public static bool BeginFight(ActorStruc PlayerCharacter, ActorStruc NpcDefender, Location NpcLocation, Location CurrentPlayerLocation)
        {
            bool PlayerTurn = PlayerCharacter.ActorSpeed >= NpcDefender.ActorSpeed;
            bool ContinueFight = true;

            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine($"You encounter an enemy {(!PlayerTurn ? "They are Faster!" : "They are Slower!")}");
            DisplayActorData(NpcDefender, -1);

            while (ContinueFight && PlayerCharacter.ActorHealth > 0 && NpcDefender.ActorHealth > 0)
            {
                // Check for teleportation
                while (PlayerTurn)
                {
                    ActorStruc.ApplyCooling(PlayerCharacter.Attacks);
                    Console.WriteLine("---------------------------------------------------------------------");
                    Console.WriteLine("Select your attack by typing its ID!");
                    Console.WriteLine("Type 'insp' to see enemy sheet, 'inv' to view your own sheet, 'con' to use items, or 'flee' to run!");

                    for (int i = 0; i < PlayerCharacter.Attacks.Length; i++)
                    {
                        var attack = PlayerCharacter.Attacks[i];
                        Console.WriteLine($"ID: {i + 1}, Name: {attack.AttackTitle.TitleName}-{attack.AttackName}, Damage: {attack.AttackStrength}");
                        Console.WriteLine($"TYPE: {attack.AttackType} CD: {attack.MaxCool} {(attack.CurrentCool > 0 ? $"(Cooling: {attack.CurrentCool})" : "(Ready)")}");
                    }

                    var AttackChoice = Console.ReadLine()?.Trim().ToLower();

                    if (AttackChoice == "flee")
                    {
                        ContinueFight = false;
                        break;
                    }
                    else if (AttackChoice == "insp")
                    {
                        DisplayActorData(NpcDefender, -1);
                        continue;
                    }
                    else if (AttackChoice == "inv")
                    {
                        DisplayActorData(PlayerCharacter, -1);
                        continue;
                    }
                    else if (AttackChoice == "con")
                    {
                        DisplayActorPotions(PlayerCharacter);
                        Console.WriteLine("Choose a potion or type 'r' to cancel.");
                        var PotionChoice = Console.ReadLine()?.Trim().ToLower();

                        if (PotionChoice == "r") continue;

                        if (int.TryParse(PotionChoice, out int PotionIndex) && PotionIndex >= 0 && PotionIndex < PlayerCharacter.ActorPotions.Length)
                        {
                            if (!ActorStruc.UsePotion(PlayerCharacter.ActorPotions[PotionIndex], PlayerCharacter, NpcDefender))
                            {
                                PlayerTurn = false;
                            } else {
                                ContinueFight = false;
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid potion selection.");
                        }
                        continue;
                    }

                    if (int.TryParse(AttackChoice, out int ChosenIndex))
                    {
                        ChosenIndex--;
                        if (ChosenIndex >= 0 && ChosenIndex < PlayerCharacter.Attacks.Length)
                        {
                            var CurrentAttack = PlayerCharacter.Attacks[ChosenIndex];
                            if (CurrentAttack.CurrentCool == 0)
                            {
                                int Damage = ActorStruc.CalculateDamage(CurrentAttack, PlayerCharacter, NpcDefender);
                                Console.WriteLine($"You attack {NpcDefender.ActorName} with {CurrentAttack.AttackName}, dealing {Damage} damage!");
                                NpcDefender.ActorHealth -= Damage;
                                Console.WriteLine($"Their Health is now {NpcDefender.ActorHealth}!");

                                if (NpcDefender.ActorHealth <= 0)
                                {
                                    Console.WriteLine($"{NpcDefender.ActorName} has been defeated!");
                                    ContinueFight = false;
                                    break;
                                }

                                CurrentAttack.CurrentCool = CurrentAttack.MaxCool;
                                PlayerTurn = false;
                            }
                            else
                            {
                                Console.WriteLine("Attack is still cooling down!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid attack selection.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Try again.");
                    }
                }

                while (!PlayerTurn && ContinueFight)
                {

                    ActorStruc.ApplyCooling(NpcDefender.Attacks);
                    var ChosenAttack = NpcDefender.Attacks.FirstOrDefault(a => a.CurrentCool == 0);

                    if (ChosenAttack != null)
                    {
                        int Damage = ActorStruc.CalculateDamage(ChosenAttack, NpcDefender, PlayerCharacter);
                        Console.WriteLine($"{NpcDefender.ActorName} attacks you with {ChosenAttack.AttackName}, dealing {Damage} damage!");
                        PlayerCharacter.ActorHealth -= Damage;
                        Console.WriteLine($"Your Health is now {PlayerCharacter.ActorHealth}!");
                        if (PlayerCharacter.ActorHealth <= 0)
                        {
                            Console.WriteLine("You have been defeated!");
                            ContinueFight = false;
                            break;
                        }

                        ChosenAttack.CurrentCool = ChosenAttack.MaxCool;
                        PlayerTurn = true;
                    }
                    else
                    {
                        Console.WriteLine("Enemy blunders their turn!");
                        PlayerTurn = true;
                    }
                }
            }

            if (PlayerCharacter.ActorHealth > 0 && NpcDefender.ActorHealth <= 0)
            {
                Console.WriteLine($"Victory! You gain {NpcDefender.ActorGold} gold.");
                PlayerCharacter.ActorGold += NpcDefender.ActorGold;
                PlayerCharacter.CurrentXP += NpcDefender.ActorLevel * 50;
                if (PlayerCharacter.CurrentXP >= PlayerCharacter.XPToNextLevel)
                {
                    Console.WriteLine("LEVEL UP!");
                    PlayerCharacter = LevelActor(PlayerCharacter, true);
                }
                return true;
            }

            if (PlayerCharacter.ActorHealth <= 0)
            {
                Console.WriteLine("Game Over.");
            }

            return false;
        }

        public static bool CheckForTeleportation(ActorStruc Npc, Location Player, Location fightLocation)
        {
            var NPCLOCATION = MapHandler.Map
            .SelectMany(row => row)
            .FirstOrDefault(cell => cell.WhatIsHereID == Npc.ActorID && cell.WhatListToSearch == 1);
            if (NPCLOCATION == null)
            {
                Console.WriteLine("NPC location not found on the map.");
                return true;
            }
            return NPCLOCATION.Location.X != fightLocation.X || NPCLOCATION.Location.Y != fightLocation.Y || Player.X != fightLocation.X || Player.Y != fightLocation.Y;
        }
    }

}
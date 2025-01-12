using MapHandleSpace;

namespace ObjectHandleSpace
{
    public class ObjectStruc
    {
        public int ObjectID { get; set; }
        public int ObjectHealth { get; set; }
        public string ObjectName { get; set; }
        public string ObjectRepresentation { get; set; }
        public bool ObjectIsDangerous { get; set; }
        public int ObjectStrength { get; set; }
        public bool CanPass { get; set; }
        public string ObjectDescription { get; set; }

        public static ObjectStruc[] ObjectList = {
            new(){
                ObjectID = 0,
                ObjectHealth = 99999,
                ObjectName = "Air",
                ObjectRepresentation = "||",
                ObjectIsDangerous = false,
                ObjectStrength = 0,
                CanPass = true,
                ObjectDescription = "Quite breathable",
            }, 
            new(){
                ObjectID = 1,
                ObjectHealth = 99999,
                ObjectName = "Lava",
                ObjectRepresentation = "XX",
                ObjectIsDangerous = true,
                ObjectStrength = 99,
                CanPass = true,
                ObjectDescription = "Hotter then most things",
            }, 
            new(){
                ObjectID = 2,
                ObjectHealth = 20,
                ObjectName = "Stone",
                ObjectRepresentation = "[]",
                ObjectIsDangerous = false,
                ObjectStrength = 70,
                CanPass = false,
                ObjectDescription = "Watch your teeth",
            }, 
            new(){
                ObjectID = 3,
                ObjectHealth = 99999,
                ObjectName = "Water",
                ObjectRepresentation = "~~",
                ObjectIsDangerous = false,
                ObjectStrength = 0,
                CanPass = true,
                ObjectDescription = "Slow gentle current",
            },            
            new(){
                ObjectID = 4,
                ObjectHealth = 0,
                ObjectName = "Rubble",
                ObjectRepresentation = "&&",
                ObjectIsDangerous = false,
                ObjectStrength = 0,
                CanPass = true,
                ObjectDescription = "Destroyed remains of something",
            },
            new(){
                ObjectID = 5,
                ObjectHealth = 99999,
                ObjectName = "Possessed tide",
                ObjectRepresentation = "^^",
                ObjectIsDangerous = true,
                ObjectStrength = 10,
                CanPass = false,
                ObjectDescription = "An evil spirit haunts these waters",
            },   
            new(){
                ObjectID = 6,
                ObjectHealth = 99999,
                ObjectName = "Mountain",
                ObjectRepresentation = "MM",
                ObjectIsDangerous = true,
                ObjectStrength = 2,
                CanPass = true,
                ObjectDescription = "A tall mountain makes traversal Difficult",
            },   
        };
        public static void SetLocationToObject(Location location, int ObjectIDToReturn){
            var Object = ObjectList.FirstOrDefault(_ => _.ObjectID == ObjectIDToReturn);

            MapHandler.Map[location.X][location.Y] = new(){
                LocationID = location.X + location.Y,
                CanPass = Object.CanPass,
                Location = new Location() { X = location.X, Y = location.Y },
                WhatIsHereID = Object.ObjectID,
                WhatIsHereName = Object.ObjectName,
                WhatListToSearch = 0,
                WhatIsHereRepresentation = Object.ObjectRepresentation
            };
        }
        public static ObjectStruc GenerateObject(Random random, int X, int Y, bool MustBeClear)
        {
            if (MustBeClear) {
                return new ObjectStruc(){
                    ObjectID = 0,
                    ObjectHealth = 99999,
                    ObjectName = "Air",
                    ObjectRepresentation = "||",
                    ObjectIsDangerous = false,
                    ObjectStrength = 0,
                    CanPass = true,
                    ObjectDescription = "Quite breathable",
                };
            }
            ObjectStruc GeneratedObject = ObjectList[random.Next(0, ObjectList.Length)];
            
            return GeneratedObject;
        }
    }
}
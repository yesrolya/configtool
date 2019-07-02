using System;
using System.IO;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace ConfigTool
{ 
    [DataContract]
    public class Jason
    {
        [DataMember]
        public List<BranchConfig> Branches { get; } = new List<BranchConfig>();
        [DataMember]
        public List<KeyBuildingTierPackConfig> KeyBuildings { get; } = new List<KeyBuildingTierPackConfig>();
        [DataMember]
        public List<OfferConfig> Offers { get; } = new List<OfferConfig>();
        [DataMember]
        public List<LevelTierPackConfig> Levels { get; } = new List<LevelTierPackConfig>();
        [DataMember]
        public int Version;
        [DataMember]
        public List<string> KBCharacters = new List<string>();
        [DataMember]
        public string MainCharacter = "";

        public Jason() { }
        
        public void AddKB(int order, int unlockinglevel, int cost, string key = "key")
        {
            KeyBuildings.Add(new KeyBuildingTierPackConfig(order, unlockinglevel, cost, key));
        }

        public void AddOffer(int num, int price, int res1, int unlockinglevel)
        {
            Offers.Add(new OfferConfig(num, price, res1, unlockinglevel));
        }

        public void AddLevel(int order, string key = "key")
        {
            Levels.Add(new LevelTierPackConfig(order, key));
        }

        public void AddBranch(int unlockingLevel = 0, int levelsQuantity = 1)
        {
            Branches.Add(new BranchConfig(unlockingLevel, levelsQuantity));
        }

        [DataContract]
        public class KeyBuildingTierPackConfig : LevelTierPackConfig
        {
            [DataMember]
            public Resources Cost;

            [DataMember]
            public int UnlockingLevel;

            public KeyBuildingTierPackConfig (): base(0)
            {
                Cost = new Resources();
                UnlockingLevel = 0;
            }

            public KeyBuildingTierPackConfig(int order, int unlockinglevel, int cost, string key = "key") : base(order, key)
            {
                Cost = new Resources(ResourceType.Resource1, cost);
                UnlockingLevel = unlockinglevel;
            }
        }

        [DataContract]
        public class OfferConfig
        {
            [DataMember]
            public Resources Article;

            //[DataMember]
            //public int ArticleGold;

            [DataMember]
            //public Resources Price;
            public EmptyClass Price;

            [DataMember]
            public int PriceGold;

            [DataMember]
            public int UnlockingLevel;

            [DataMember]
            public string WidgetName = "Offer00";

            [DataMember]
            public string Char = "Etch";

            public OfferConfig() { }
            
            public OfferConfig(int num, int price, int res1, int unlockinglevel)
            {
                Article = new Resources(ResourceType.Resource1, res1);
                //ArticleGold = 10;
                Price = new EmptyClass();
                //Price = new Resources();
                PriceGold = price;
                UnlockingLevel = unlockinglevel;
                WidgetName = "Offer" + num.ToString("00");
            }
        }

        [DataContract]
        public class EmptyClass
        {
            public EmptyClass() { }
        }

        public enum ResourceType
        {
            Resource1,
            Resource2,
            Resource3,
        }

        [DataContract]
        public class Resources
        {
            public static readonly List<ResourceType> AllTypes = new List<ResourceType>((ResourceType[])Enum.GetValues(typeof(ResourceType)));

            [DataMember]
            public Dictionary<ResourceType, int> Values = new Dictionary<ResourceType, int>();

            public IEnumerable<KeyValuePair<ResourceType, int>> NonZeroResources => Values.Where(kv => kv.Value > 0);

            public bool IsEmpty => Values.Count == 0 || Values.All(kv => kv.Value == 0);

            public int this[ResourceType resource]
            {
                get => Values[resource];
                set => Values[resource] = value;
            }

            public Resources()
            {
                Clear();
            }

            public void Clear()
            {
                AllTypes.ForEach(e => Values[e] = 0);
            }

            public Resources(ResourceType resource, int amount) : this()
            {
                Add(resource, amount);
            }
            
            public void Add(ResourceType resource, int amount)
            {
                this[resource] += amount;
            }
        }

        [DataContract]
        public class BranchConfig
        {
            public BranchConfig() { }

            [DataMember]
            public List<LevelBranch> Levels = new List<LevelBranch>();
            [DataMember]
            public int UnlockingLevel = 5;

            public BranchConfig(int unlockingLevel = 0, int levelsQuantity = 1, string[] key = null) {
                for (int i = 0; i < levelsQuantity; i++) {
                    if (key != null && key.Length > i) {
                        Levels.Add(new LevelBranch(key[i]));
                    } else {
                        Levels.Add(new LevelBranch());
                    }
                }
                UnlockingLevel = unlockingLevel;
            }

            public void AddBranchLevel()
            {
                Levels.Add(new LevelBranch());
            }
        }

        [DataContract]
        public class LevelBranch
        {
            [DataMember]
            //public List<LevelInfo> Level = new List<LevelInfo>();
            public LevelInfo Level;

            public LevelBranch(string key = "key") {
               // Level.Add(new LevelInfo());
                Level = new LevelInfo(key);
            }
        }

        [DataContract]
        public class LevelInfo { 
            public LevelInfo(string key = "key") {
                Key = key;
            }
            [DataMember]
            public string Key = "key-key-key";
        }

        [DataContract]
        public class LevelTierPackConfig
        {
            [DataMember]
            public string AnOrder = "000";
            [DataMember]
            public LevelInfo Level;

            public LevelTierPackConfig() { }

            public LevelTierPackConfig(int order, string key = "key") {
                AnOrder = order.ToString("000");
                Level = new LevelInfo(key);
            }
        }
    }
}

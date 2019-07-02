using System;

namespace ConfigTool
{
    public class CityConfig
    {
        public string MainCharacter;
        public List<string> KbCharacters = new List<string>();
        public List<LevelTierPackConfig> Levels { get; } = new List<LevelTierPackConfig>();
        public List<KeyBuildingTierPackConfig> KeyBuildings { get; } = new List<KeyBuildingTierPackConfig>();
        public List<CollectibleDesc> Collectibles { get; } = new List<CollectibleDesc>();
        public List<OfferConfig> Offers { get; } = new List<OfferConfig>();
        public List<BranchConfig> Branches { get; } = new List<BranchConfig>();
        public int Version;

        public CityConfig() { }

        public class BranchConfig
        {
            public int UnlockingLevel;
            public List<LevelTierPackConfig> Levels { get; } = new List<LevelTierPackConfig>();
        }

        public class TierPackConfig
        {
            public string AnOrder;
            public LevelIdentifier Level;
        }

        public class OfferConfig
        {
            public Resources Article;
            public int ArticleGold;
            public Resources Price;
            public int PriceGold;
            public int UnlockingLevel;
            public string WidgetName;
            public string Char;
        }

        public class CollectibleDesc
        {
            public int KbId;
            public Resources Reward;
            public int RewardGold;
            public string WidgetName;
            public string QuestIcon;
            public string Char;
        }

        public class KeyBuildingTierPackConfig : TierPackConfig
        {
            public Resources Cost;
            public int UnlockingLevel;
        }
        public class LevelTierPackConfig : TierPackConfig
        {
            public int TierToUnlockKB = 0;
            public int KeyTier = -1;
        }
        public enum ResourceType
        {
            Resource1,
            Resource2,
            Resource3,
        }
        //?????????????????????????????????????????
        public class Resources
        {
            public static readonly List<ResourceType> AllTypes = Make.ListOfEnum<ResourceType>();

            [Serializable]
            public Dictionary<ResourceType, int> Values = new Dictionary<ResourceType, int>();

            public IEnumerable<KeyValuePair<ResourceType, int>> NonZeroResources => Values.Where(kv => kv.Value > 0);

            public bool IsEmpty => Values.Count == 0 || Values.All(kv => kv.Value == 0);

            public int this[ResourceType resource] {
                get => Values[resource];
                set => Values[resource] = value;
            }

            public Resources() {
                Clear();
            }

            public void Clear() {
                AllTypes.ForEach(e => Values[e] = 0);
            }

            public Resources(Dictionary<ResourceType, int> additional) : this() {
                Add(additional);
            }

            public Resources(ResourceType resource, int amount) : this() {
                Add(resource, amount);
            }

            public void Substract(Resources price) {
                AllTypes.ForEach(e => Values[e] -= price[e]);
            }

            public void Add(Resources additional) {
                AllTypes.ForEach(e => Values[e] += additional[e]);
            }

            public void Add(Dictionary<ResourceType, int> additional) {
                AllTypes.ForEach(e => {
                    if (additional.ContainsKey(e))
                        Values[e] += additional[e];
                });
            }

            public void Add(ResourceType resource, int amount) {
                this[resource] += amount;
            }

            public bool IsEnoughToBuy(Resources price) {
                return AllTypes.All(rt => Values[rt] >= price[rt]);
            }

            public override string ToString() {
                string result = null;
                foreach (var rt in AllTypes.Where(rt => this[rt] > 0)) {
                    result = result ?? "";
                    result += $"{rt}:{this[rt]} ";
                }
                return result ?? "<none>";
            }
        }





    }
    //public const string PoolConfigFilename = "LevelsPool.json";
}
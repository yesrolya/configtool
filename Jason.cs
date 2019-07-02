using System;

namespace ConfigTool
{
    [DataContract]
    public class Jason
    {
        [DataMember]
        public List<Branch> Branches;
        //public string KeyBuildings;
        //public string Levels;
        //public string Version;
        //public string KBCharacters;
        //public string MainCharacter;
        public Jason() { }

        public Jason(int qBranches) {
            Branches = new List<Branch>(qBranches);
        }

        [DataContract]
        public class Branch
        {
            public Branch() { }

            [DataMember]
            public List<LevelBranch> Levels = new List<LevelBranch>(1);
            [DataMember]
            public int UnlockingLevel = 5;

            public Branch(int levelsQuantity, int unlockingLevel) {
                Levels = new List<LevelBranch>(1);
            }

            [DataContract]
            public class LevelBranch
            {
                [DataMember]
                public LevelInfo Level = new LevelInfo(1);

                public LevelBranch() { }

                [DataContract]
                public struct LevelInfo
                {
                    public LevelInfo() { }
                    [DataMember]
                    public string Key = "key-key-key";
                }
            }
        }
    }
}

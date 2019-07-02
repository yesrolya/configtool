using System;

public class OutputClasses
{
    public enum DialogType {
        IntroDialogScenario,
        /*
            //Cue1
            Animation
            Text: []As you all know, Andy is coming home from camp today.
            Side: Left
            //Cue2
            Animation
            Text: []I commend you all for finding such… creative ways to spend your time while he was away, but we made a bit of a mess.
            Side: Left
            //Cue3
            Animation
            Text: []Now it’s time to pitch in and get the room back to the way it was.
            Side: Left
         */
        KbIntroDialogs,
        /*
            //KB1
            Cues:
            //Cue1
            Character: Woody
            Side: Left
            Text: []Sarge, we need to get those blocks in order before Andy comes home.
            //Cue2
            Character: ToySoldier_Cap
            Side: Right
            Text: []Roger that. I’ll gather my troops.
         */
        KbOutroDialogs,
        /*
            //KB5
            Cues:
            //Cue1
            Character: Buzz
            Side: Left
            Text: []Buzz Lightyear, reporting for duty.
            //Cue2
            Character: Woody
            Side: Right
            Text: []You’re just in time Buzz. Andy’s on his way.
         */
        OfferDialogScenarios
        /*
            //Scenario1
            Cue1: []Hammilton P. Moneybags, purveyor of fine goods and sundries, at your service.
            Cue2: []If you need batteries, I’m your pig!
            //Scenario2
            Cue1: []Coins weighing you down? Well, welcome to Hammilton P. Moneybags’ Battery Emporium!
            Cue2: []Exchange coins for batteries today and every day.
         */
    }

    public enum Character
    {
        None,
        Woody,
        ToySoldier_Cap,
        Slinky,
        Rex,
        Hamm,
        BoPeep,
        Buzz,
        Etch,
        Aliens,
        Bullseye,
        Jessie,
        RCCar,
        Lenny,
        Pricklepants,
        Dolly,
        Trixie,
        Peas,
        Alien,
        Wheezy,
        Rocky_Gibraltar,
        Zurg,
        Combat_Carl,
        Mr_Shark,
        Giggle,
        Pinball_Zebra,
        Pinball_Eagle,
        Forky,
        DukeCaboom
    }

    public enum Side
    {
        Left,
        Right
    }

    public class Cue
    {
        public readonly CharacterInfo Character;
        public readonly Side Side;
        public readonly string Text;
        public bool Skip;
        public Action<Widget> ShowHandler;

        public Cue(Character character, Side side, string text, bool skip = false, Action<Widget> showHandler = null) {
            Character = character;
            Side = side;
            Text = text;
            Skip = skip;
            ShowHandler = showHandler;
        }

        public Cue() { }
    }

    public class DialogConfig
    {
        public DialogType Type;
        public int CueNumber;
        public int ScenarioNumber;
        public Side side;
    }

    public class LevelConfig
    {
        public struct Level
        {
            public string Key;
        }

        [Serializable]
        public struct Branches
        {
            public List<Level> Levels;
            public int UnlockingLevel;
        }

        public struct KeyBuildings
        {
            public string AnOrder
        }

        public Branches Branchess;
        
    }

}

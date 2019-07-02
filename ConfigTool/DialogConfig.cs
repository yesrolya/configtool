using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigTool
{
    public class DialogConfig
    {
        public Dialog Intro = new Dialog();
        public List<Dialog> KBIntro = new List<Dialog>();
        public List<Dialog> KBOutro = new List<Dialog>();
        public List<OfferScenario> Offers = new List<OfferScenario>();
        
        public bool isIntroEmpty ()
        {
            if (Intro.cues.Count > 0)
                return false;
            else
                return true;
        }

        public bool isKBOutroEmpty()
        {
            if (KBOutro.Any(k => k.cues.Count > 0))
                return false;
            else
                return true;
        }

        public bool isKBIntroEmpty()
        {
            if (KBIntro.Any(k => k.cues.Count > 0))
                return false;
            else
                return true;
        }

        public bool isOfferEmpty()
        {
            if (Offers.Any(o => o.cues.Count > 0))
                return false;
            else
                return true;
        }

        public void AddEmptyKBDialog(bool intro, int order = -1)
        {
            if (intro) {
                KBIntro.Add(new Dialog(order));
            } else {
                KBOutro.Add(new Dialog(order));
            }
        }

        public void AddEmptyOffer(int order)
        {
            Offers.Add(new OfferScenario(order));
        }

        public DialogConfig () { }
        
        public class Dialog
        {
            public int order;
            public List<Cue> cues;
            
            public Dialog (int order = -1) {
                this.order = order;
                cues = new List<Cue>();
            }

            public void AddCue(int order, Side side, string text, string character)
            {
                cues.Add(new Cue(order, side, text, character));
            }

            public List<string> ToText(bool isIntro)
            {
                List<string> result = new List<string>();
                if (!isIntro) {
                    result.Add($"Cues:");
                }
                foreach (var c in cues) {
                    foreach (var str in c.ToText(isIntro))
                        result.Add(str);
                }
                
                return result;
            }
        }
        
        public enum Side
        {
            Left, Right
        }

        public class Cue
        {
            int order;
            public string character;
            Side side;
            string text;

            public Cue (int order, Side side, string text, string character = "None")
            {
                this.order = order;
                this.character = character;
                this.side = side;
                this.text = text;
            }

            public Cue(Cue cue, string newtext)
            {
                order = cue.order;
                character = cue.character;
                side = cue.side;
                text = newtext;
            }

            public List<string> ToText(bool isIntroDialog = false)
            {
                List<string> result = new List<string>();
                if (isIntroDialog) {
                    result.Add($"//Cue{order}");
                    result.Add("Animation");
                    result.Add($"Text: []{text}");
                    result.Add($"Side: {side.ToString()}");
                } else {
                    result.Add($"//Cue{order}");
                    result.Add($"Character: []{character}");
                    result.Add($"Side: {side.ToString()}");
                    result.Add($"Text: []{text}");
                }
                return result;
            }
        }

        public class OfferScenario
        {
            int order;
            public List<OfferCue> cues;
            
            public OfferScenario (int order, string text = null)
            {
                this.order = order;
                cues = new List<OfferCue>();
                if (text != null)
                    AddCue(text);
            }

            public void AddCue(string text)
            {
                int num = (cues == null ? 0 : cues.Count);
                cues.Add(new OfferCue(num + 1, text));
            }

            public List<string> ToText()
            {
                var result = new List<string>() { $"//Scenario{order}" };
                foreach (var c in cues)
                    result.Add(c.ToText());
                return result;
            }
        }

        public class OfferCue
        {
            int order;
            string text;

            public OfferCue (int order = 1, string text = "SOME_TEXT")
            {
                this.order = order;
                this.text = text;
            }

            public string ToText()
            {
                return $"Cue{order}: []{text}";
            }
        }
    }
}

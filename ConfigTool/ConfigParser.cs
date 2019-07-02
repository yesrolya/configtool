using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;

namespace ConfigTool
{
    public enum SheetNum
    {
        spec = 1,
        dialog = 2,
        full = 3,
        level = 4,
        kb = 5
    };

    public class ConfigFile
    {
        public List<ConfigList> Lists;
        public Jason file1;
        public DialogConfig file2;
        public string textfile2;
        bool isFullDesignLoaded = false;

        public ConfigFile(string fileInput)
        {
            Lists = new List<ConfigList>();
            file1 = new Jason();
            file2 = new DialogConfig();
            Excel.Application app;
            Excel.Workbook workbook;
            Excel.Worksheet worksheet;
            app = new Excel.Application();
            workbook = app.Workbooks.Open(fileInput);
            
            for (int i = 1; i <= workbook.Sheets.Count; i++) {
                worksheet = workbook.Sheets[i];
                int listNum = CheckName(worksheet.Name);
                if (listNum != -1) {
                    Lists.Add(new ConfigList(worksheet, listNum));
                }
            }
            
            workbook.Close(false);
            app.Quit();

            ReadInfo();
        }

        private void ReadInfo()
        {
            SelectProtagonist(Lists.FindIndex(x => x.Number == 1));
            ReadLevelsList(Lists.FindIndex(x => x.Number == 4));
            ReadOffersList(Lists.FindIndex(x => x.Number == 5));
            ReadDialogsCharacters(Lists.FindIndex(x => x.Number == 3));
            ReadDialogs(Lists.FindIndex(x => x.Number == 2));
        }

        public void WriteInfoDialog(string filepath)
        {
            using (StreamWriter sw = File.CreateText(filepath)) {
                if (!file2.isIntroEmpty()) {
                    sw.WriteLine("IntroDialogScenario:");
                    foreach (var str in file2.Intro.ToText(true)) {
                        sw.WriteLine(str);
                    }
                }

                if (!file2.isKBIntroEmpty()) {
                    sw.WriteLine();
                    sw.WriteLine("KbIntroDialogs:");
                    for (int i = 0; i < file2.KBIntro.Count; i++) {
                        if (file2.KBIntro[i].cues.Count != 0) {
                            sw.WriteLine($"//KB{i+1}");
                            foreach (var str in file2.KBIntro[i].ToText(false)) {
                                sw.WriteLine(str);
                            }
                            sw.WriteLine();
                        }
                    }
                }


                if (!file2.isKBOutroEmpty()) {
                    sw.WriteLine("KbOutroDialogs:");
                    for (int i = 0; i < file2.KBOutro.Count; i++) {
                        if (file2.KBOutro[i].cues.Count != 0) {
                            sw.WriteLine($"//KB{i + 1}");
                            foreach (var str in file2.KBOutro[i].ToText(false)) {
                                sw.WriteLine(str);
                            }
                            sw.WriteLine();
                        }
                    }
                }

                if (!file2.isOfferEmpty()) {
                    sw.WriteLine();
                    sw.WriteLine("OfferDialogScenarios:");
                    for (int i = 0; i < file2.Offers.Count; i++) {
                        if (file2.Offers[i].cues.Count != 0) {
                            foreach (var str in file2.Offers[i].ToText()) {
                                sw.WriteLine(str);
                            }
                        }
                    }
                }
            }
        }

        private void ReadDialogs(int listNum)
        {
            if (listNum == 0) return;
            //file2
            for (int i = 1; i < Lists[listNum].Cells.GetLength(0); ) {
                if (Tools.Contains(Lists[listNum][i, 0], "dialogue")) {
                    //Read Intro Dialog.
                    while (Tools.Contains(Lists[listNum][i, 1], "intro")) {
                        int introCue = file2.Intro.cues.Count;
                        file2.Intro.AddCue(introCue + 1, DialogConfig.Side.Left, Lists[listNum][i, 4], Lists[listNum][i, 2]);
                        i++;
                    }

                    if (Tools.Contains(Lists[listNum][i, 1], "kb")
                    && (Tools.Contains(Lists[listNum][i, 1], "before") || Tools.Contains(Lists[listNum][i, 1], "after"))) {
                        int kbNum = Tools.Number(Lists[listNum][i, 1]) - 1;
                        string character = Lists[listNum][i, 2];
                        string text = Lists[listNum][i, 4];
                        int cueNum;

                        DialogConfig.Side cSide = DialogConfig.Side.Right;
                        if (CheckCharLeftSide(kbNum, character)) {
                            cSide = DialogConfig.Side.Left;
                        }
                        if (Tools.Contains(Lists[listNum][i, 1], "before")) {
                            //Read KBIntro Dialog.
                            cueNum = file2.KBIntro[kbNum].cues.Count;
                            file2.KBIntro[kbNum].AddCue(cueNum + 1, cSide, text, character);
                        } else if (Tools.Contains(Lists[listNum][i, 1], "after")) {
                            //Read KBOutro Dialog.
                            cueNum = file2.KBOutro[kbNum].cues.Count;
                            file2.KBOutro[kbNum].AddCue(cueNum + 1, cSide, text, character);
                        }
                        
                    }
                    //} else if (Tools.Contains(Lists[listNum][i, 0], "offer")) {
                    //Пока в экселе я не находила.
                }
                i++;
            }
        }

        private void ChangeSide(ref DialogConfig.Side currentSide)
        {
            currentSide = currentSide == DialogConfig.Side.Left ? DialogConfig.Side.Right : DialogConfig.Side.Left ;
        }

        private void CheckCharacter (ref DialogConfig.Side currentSide, string currentCharacter, string previousCharacter)
        {
            if (previousCharacter != null && currentCharacter != previousCharacter)
                ChangeSide(ref currentSide);
        }

        string[] leftchar;
        private void ReadDialogsCharacters(int listNum)
        {
            if (listNum == 0) return;
            //Я начего не поняла со сторонами героев в диалоге.
            //leftrigthchar = new List<(string, string)>();

            var chars = from i in Enumerable.Range(0, Lists[listNum].Cells.GetLength(0))
                        where Tools.Contains(Lists[listNum][i, 1], "UI Left")
                        select Lists[listNum][i, 3];
            leftchar = chars.ToArray<string>();
        }

        private bool CheckCharLeftSide(int kbNumber, string character)
        {
            if (leftchar.Length <= kbNumber)
                return false;
            if (Tools.Contains(leftchar[kbNumber], character))
                return true;
            else
                return false;
        }

        private int CheckName(string name)
        {
            foreach (SheetNum s in Enum.GetValues(typeof(SheetNum))) {
                if (Tools.Contains(name, s.ToString())) {
                    return (int)s;
                }
            }
            return -1;
        }

        private void SelectProtagonist(int listNum)
        {
            if (listNum == 0) return;
            (int, int) index = Lists[listNum].FindIndex("PROTAGONIST");
            if (index.Item1 != -1) {
                file1.MainCharacter = Lists[listNum][index.Item1, index.Item2 + 1] ?? "None";
            }
        }

        private void ReadLevelsList(int listNum)
        {
            if (listNum == 0) return;
            int levelsQ = 0;
            //int kbQ = 0;
            
            (int, int) index = Lists[listNum].FindIndex("Level");

            int j = index.Item2;
            for (int i = index.Item1 + 1; i < Lists[listNum].Cells.GetLength(0); i++) {
                if (Lists[listNum][i,j] != null) {
                    if (Lists[listNum].CheckNum(i, j)) {
                        levelsQ++;
                    //} else if (Lists[listNum].CheckKB(i, j)) {
                    //    kbQ++;
                    } else if (Lists[listNum].CheckBranch(i, j) > 0) {
                        int l = Lists[listNum].CheckBranch(i, j);
                        int tempi = file1.Branches?.FindIndex(x => x.UnlockingLevel == l) ?? -1;
                        if (tempi != -1) {
                            file1.Branches[tempi].AddBranchLevel();
                        } else {
                            file1.AddBranch(l);
                        }
                    }
                }
            }
            
            if (levelsQ > 0) {
                for (int i = 0; i < levelsQ; i++)
                    file1.AddLevel(i + 1);
            }

        }

        private void ReadOffersList(int listNum)
        {
            if (listNum == 0) return;
            //Заполнение списка KB.
            var index = Lists[listNum].FindIndex("kb");
            int j = index.Item2;
            int order, ulevel, price;
            for (int i = index.Item1 + 1; i < Lists[listNum].Cells.GetLength(0) && Lists[listNum][i,j] != null; i++) {
                if (!int.TryParse(Lists[listNum][i, j], out order)) {
                    break;
                }
                if (!int.TryParse(Lists[listNum][i, j + 1], out ulevel)) {
                    //Если не указан уровень, то для достижения КБ необходимо пройти все.
                    ulevel = file1.Levels.Count; 
                }
                int.TryParse(Lists[listNum][i, j + 2], out price);
                file1.AddKB(order, ulevel, price);
            }

            //Заполнение списка Offers.
            index = Lists[listNum].FindIndex("so");
            j = index.Item2;
            //Unlocking level | Resource | Price
            // j+2
            int resource;
            int offerNum = 0;
            for (int i = index.Item1 + 1; i < Lists[listNum].Cells.GetLength(0) && Lists[listNum][i, j] != null; i++) {
                if (int.TryParse(Lists[listNum][i, j + 2], out ulevel)) {
                    int.TryParse(Lists[listNum][i, j + 3], out resource);
                    int.TryParse(Lists[listNum][i, j + 4], out price);
                    file1.AddOffer(++offerNum, price, resource, ulevel);
                }
            }

            //Заполнение пустыми диалогами второй файл по кол-ву КБ и Предложений
            for (int i = 0; i < file1.KeyBuildings.Count; i++) {
                file2.AddEmptyKBDialog(true, i + 1);
                file2.AddEmptyKBDialog(false, i + 1);
            }
            for (int i = 0; i < file1.Offers.Count; i++) {
                file2.AddEmptyOffer(i + 1);
            }
        }
    }

    public class ConfigList
    {
        public string[,] Cells;
        public int Number;

        public string this[int i, int j]
        {
            get {
                return Cells[i,j] ?? null;
            }
            set {
                Cells[i, j] = value;
            }
        }

        public ConfigList(Excel.Worksheet worksheet, int num)
        {
            Number = num;
            worksheet.Columns.ClearFormats();
            worksheet.Rows.ClearFormats();
            worksheet.Cells.ClearFormats();

            Excel.Range range;
            range = worksheet.UsedRange;

            var result = new string[range.Rows.Count, range.Columns.Count];

            foreach (Excel.Range cell in range.Cells) {
                result[cell.Row - 1, cell.Column - 1] = cell?.Value2?.ToString() ?? null;
            }

            Cells = result;
        }
        
        public (int,int) FindIndex (string str) {
            var result = from i in Enumerable.Range(0, Cells.GetLength(0))
                         from j in Enumerable.Range(0, Cells.GetLength(1))
                         where Tools.Contains(Cells[i, j], str)
                         select( i, j );
            return result?.First() ?? (-1, -1);
        }
        public bool CheckNum(int i, int j)
        {
            int temp;
            return int.TryParse(Cells[i, j], out temp);
        }
        public bool CheckKB(int i, int j, string str = "KB")
        {
            return Tools.Contains(Cells[i, j], str);
        }
        public int CheckBranch(int i, int j, char str = '-')
        {
            int result = -1;
            if (Tools.Contains(Cells[i, j], str.ToString())) {
                int.TryParse(Cells[i, j].Split(str)[0], out result);
            }
            return result;
        }
    }
    
    public class ConfigParser {

        public ConfigParser (string input, string output) {
            ConfigFile cfg = new ConfigFile(input);

            using (StreamWriter file = File.CreateText(output + "levels.json")) {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, cfg.file1);
            }

            cfg.WriteInfoDialog(output + "dialog.cfg");
        }
    }
}

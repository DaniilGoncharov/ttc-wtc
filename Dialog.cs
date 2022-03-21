using System;
using System.Collections.Generic;

namespace ttc_wtc
{
    [Serializable]
    public class Question
    {
        public Question(string question, List<Reaction> reactions)
        {
            Qstn = question;
            Reactions1 = reactions;
        }

        public string Qstn { get; set; }
        public List<Reaction> Reactions1 { get; set; }
    }

    [Serializable]
    public class Reaction
    {
        public Reaction(string answer, Question question)
        {
            Answer = answer;
            Question = question;
        }
        public string Answer { get; set; }
        public Question Question { get; set; }
    }

    [Serializable]
    class Dialog
    {
        public Dialog(Question startQuestion, int dialogLength)
        {
            LiveQuestion = startQuestion;
            DialogLength = dialogLength;
        }

        public Question LiveQuestion;
        public int DialogLength;
        public bool Completeness = false;
        public static string[] ToStrings(List<Reaction> reactions)
        {
            string[] ToString = new string[reactions.Count];
            for (int i = 0; i < reactions.Count; i++)
            {
                ToString[i] = reactions[i].Answer;
            }
            return ToString;
        }

        public static void Say(string Speach, int line)
        {
            int n = 0;
            for (int i = 0; i < Speach.Length; i++)
            {
                if (i + 17 >= 81)
                {
                    Console.SetCursorPosition(17 + n, line + 1);
                    n++;
                }
                Console.Write(Speach[i]);
                System.Threading.Thread.Sleep(25);
            }
        }

        public int GetDialog(NPC currentnpc)
        {
            int x = 0;
            for (int i = 0; i < DialogLength + 1; i++)
            {
                Reaction N;
                if (LiveQuestion.Reactions1 != null)
                {
                    Console.SetCursorPosition(17, i + 10 + x);
                    Console.Write(currentnpc.Name + ":");
                    Say(LiveQuestion.Qstn, i + 10);
                    Console.WriteLine();
                    Menu DialogMenu = new Menu(ToStrings(LiveQuestion.Reactions1));
                    N = LiveQuestion.Reactions1[DialogMenu.GetChoice(true, false, 28, 24)];
                    Console.SetCursorPosition(17, i + 11 + x);
                    x += 1;
                    Console.Write("Канеки :" + N.Answer);
                    LiveQuestion = N.Question;
                    DialogMenu.ClearMenu(28, 24, 1);
                }
                else
                {
                    Console.SetCursorPosition(17, i + 10 + x);
                    Console.Write(currentnpc.Name + ":");
                    Say(LiveQuestion.Qstn, i + 12);
                    Completeness = true;
                    break;
                }
            }
            string Bye = "Пока";
            string Fight = "Драться";
            List<string> vs = new List<string>();
            vs.Add(Bye);
            vs.Add(Fight);
            Menu resultMenu = new Menu(vs);
            return resultMenu.GetChoice(true, false, 31, 24);
        }
    }
}

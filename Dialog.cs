using System;
using System.Collections.Generic;
using System.Text;
namespace ttc_wtc
{   
    public class Wopros
    {
        public Wopros(string wopros,List<Reaction> reactions)
        {
            Wpr = wopros;
            reactions1 = reactions;
        }
        public string Wpr { get; set; }
        public List<Reaction> reactions1 { get; set; }               
    }
    public class Reaction
    {
        public Reaction(string answer,Wopros wopros) 
        {           
           antwort = answer;
           Woprosik = wopros;
        }
        public string antwort;
        public Wopros Woprosik;      
    }
     class Dialog
    {
        public Dialog(Wopros StartWopros,int dialogLength)
        {
            LiveWopros = StartWopros;
            DialogLength = dialogLength;
        }
        public Wopros LiveWopros;
        public int DialogLength;
        public bool Completeness=false;
        public static string[] ToStrings(List<Reaction> reactions)
        {
            string[] ToString = new string[reactions.Count];
            for (int i = 0; i < reactions.Count; i++)
            {
                ToString[i] = reactions[i].antwort;
            }
            return ToString;
        }
        public static void Saying (string Speach,int line)
        {
            int n = 0;

            for (int i = 0; i < Speach.Length; i++)
            {
               

                if (i+17>=81)
                {
                    Console.SetCursorPosition(17+n,line+1 );
                    n++;
                }
                Console.Write(Speach[i]);
                System.Threading.Thread.Sleep(25);
            }
        }
        public int GetDialog(NPC currentnpc)
        {
            int x = 0;
            for (int i = 0; i < DialogLength+1; i++)
            {             
                Reaction N;
                
                if (LiveWopros.reactions1!=null)
                {
                    Console.SetCursorPosition(17, i+10 + x);
                    Console.Write(currentnpc.Name + ":" );
                    Saying(LiveWopros.Wpr,i+10);
                    Console.WriteLine();
                    //Console.WriteLine(currentnpc.Name+":"+LiveWopros.Wpr);
                    Menu DialogMenu = new Menu(ToStrings(LiveWopros.reactions1));

                    N = LiveWopros.reactions1[DialogMenu.GetChoice(true, false, 28, 24)];
                    Console.SetCursorPosition(17, i+11+x);
                    x += 1;
                   
                    Console.Write("Канеки :"+N.antwort);
                    LiveWopros = N.Woprosik;
                    DialogMenu.ClearMenu(28, 24, 1);
                   
                }
                else
                {
                    Console.SetCursorPosition(17, i+10 + x);
                    Console.Write(currentnpc.Name + ":");
                    Saying(LiveWopros.Wpr,i+12);
                    Completeness = true;
                    // Console.WriteLine(currentnpc.Name + ":" + LiveWopros.Wpr);
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

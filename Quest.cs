using System;
using System.Collections.Generic;
using System.Text;

namespace ttc_wtc
{
    [Serializable]
     class Quest
     {
   
        public static Quest firstQest=new Quest   ("Узнайте как можно пройти через ворота");
        public static Quest secondQest = new Quest("Сделайте все чтобы получить ключ");
        public static Quest thirdQest = new Quest ("Пройдите через врата и убейте тварь");
        public static Quest fourthQest = new Quest("Вернитесь и Поговорите с Эрикой");
        public string questValue { get; set; }
        public bool trigger { get; set; }

        public Quest(string QuestValue) 
        { 
            questValue = QuestValue;
            trigger = false;
        }
        public static void QestChecking(Player player, NPC currentNPC ) 
        {
            string NPCname=currentNPC.Name;
            switch ( NPCname)
            {
                case "Эрика":
                    if (player.Have("Статуэтка чайки") && currentNPC != null && currentNPC.Dialog.Completeness )
                    {
                        currentNPC.Dialog = BasicDialogBuilder.EricaWithUminekoDialog.dialog;
                        player.AddItem(currentNPC.GetItemFromThisNPC("Ключ от старых ворот"));
                        currentNPC.NPCInventory.Add(player.DeleteFromInventory("Статуэтка чайки"));

                    }
                    else
                    {
                        if (currentNPC != null && currentNPC.Dialog.Completeness)
                        {
                            player.QuestNumber = 1;
                        }
                    }
                    break;
                case "Чайка":
                    if (player.Have("Статуэтка чайки") && currentNPC!= null && currentNPC.Dialog.Completeness)
                    {
                        currentNPC.Dialog = BasicDialogBuilder.UminekoWithStoneDialogBuilder.dialog;
                        player.AddItem(currentNPC.GetItemFromThisNPC("Ключ от старых ворот"));
                        currentNPC.NPCInventory.Add(player.DeleteFromInventory("Статуэтка чайки"));
                    }
                    break;
            }

           


        }
    }
   
}

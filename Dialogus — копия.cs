using System;
using System.Collections.Generic;
using System.Text;

namespace ttc_wtc
{
     class BasicDialogBuilder 
    {
        public Dialog dialog;
        public BasicDialogBuilder(string startWopros,string Reaction1,string Reaction2,string Wpr2,string Wpr1,string Reaction4,string Rection3,string Reaction6,string Reaction5,string Wpr3,string Wpr4,string Wpr5,string Wpr6)               
        {
            Wopros wpr3 = new Wopros(Wpr3, null);
            Wopros wpr4 = new Wopros(Wpr4, null);
            Reaction reaction3 = new Reaction(Rection3, wpr4);
            Reaction reaction4 = new Reaction(Reaction4, wpr3);
            List<Reaction> reactions3 = new List<Reaction>();
            reactions3.Add(reaction4);
            reactions3.Add(reaction3);
            Wopros wpr5 = new Wopros(Wpr5, null);
            Wopros wpr6 = new Wopros(Wpr6, null);
            Reaction reaction5 = new Reaction(Reaction5, wpr6);
            Reaction reaction6= new Reaction(Reaction6, wpr5);
            List<Reaction> reactions4 = new List<Reaction>();
            reactions4.Add(reaction6);
            reactions4.Add(reaction5);
            Wopros wpr = new Wopros(Wpr1, reactions4);
            Wopros wpr2 = new Wopros(Wpr2, reactions3);
            Reaction reaction = new Reaction(Reaction1, wpr2);
            Reaction reaction2 = new Reaction(Reaction2, wpr);
            List<Reaction> reactions2 = new List<Reaction>();
            reactions2.Add(reaction2);
            reactions2.Add(reaction);
            Wopros StartWopros2 = new Wopros(startWopros, reactions2);
            dialog = new Dialog(StartWopros2, 3);           
        }
          public BasicDialogBuilder(string startWopros,string Reaction1,string Reaction2,string Wpr1)               
        {
            Wopros wpr = new Wopros(Wpr1, null);
            Reaction reaction = new Reaction(Reaction1, wpr);
            Reaction reaction2 = new Reaction(Reaction2, wpr);
            List<Reaction> reactions2 = new List<Reaction>();
            reactions2.Add(reaction2);
            reactions2.Add(reaction);
            Wopros StartWopros2 = new Wopros(startWopros, reactions2);
            dialog = new Dialog(StartWopros2, 3);           
        }
        public static BasicDialogBuilder EricastartDialogBuilder = new BasicDialogBuilder("Привет как тебя зовут", "Мое имя Канеки и мне надо пройти через эти ворота", "Не твоего ума дело", "Тогда тебе понадобиться мой ключ,но мне тоже кое-что надо", "И что же тебе тогда от меня понадобилось?", "А может мне просто убить тебя", "И что же тебе от меня надо ?", "Мне нужен ключ от этих ворот", "Говори где ключ от ворот а иначе убъю", "Можешь попробовать но лучше принеси мне фигурку из подземелья ", "Принеси мне фигурку Чайки из подземелья и тогда ключ твой", "Ключ лежит в подземелье, удачи в поисках", "Ничего тебе говорить не собираюсь,лучше иди в водземелье");
        public static BasicDialogBuilder EricaWithUminekoDialog = new BasicDialogBuilder("ОООООООО ты смог достать статуэтку чайки", "Да и теперь герой жаждет своей награды", "Меня там чуть не убили", "Разумеется,держи свой ключ и можешь проваливать", "Я сама удивлена что ты выжил", "Спасибо,но для чего тебе нужна была эта статуэтка?", "Раньше ты была по добрее", "Отдай ключ и я уйду", "Раз такая умная сама бы забрала", "Статуэтка теперь моя так что что она делает не твоего ума дело,проваливай", "Теперь ты для меня бесполезный так что и говорить с тобой больше смысла нет", "Я уже отдала тебе ключ больше мне тебе помочь нечем", "Твой ключ у тебя так что прощай");
        public static BasicDialogBuilder UminekoStartDialogBuilder = new BasicDialogBuilder("Да-а-й", "ААААА говорящая чайка", "Ты умеешь говорить ?", "...");
        public static BasicDialogBuilder UminekoWithStoneDialogBuilder = new BasicDialogBuilder("Ура я наконец-то могу говорить и отдать тебе ключ от ворот", "ААА говорящая чайка", "Что ты такое?", "Не бойся я обычная девушка но в теле чайки ", "Я та чье тело сейчас у Эрики", "Как это возможно?", "Так значит я был прав люди и вправду поменялись телами", "Значит Эрика была чайкой?", " Как нам вернуть свои тела?", " Из-за котоклизма все люди поменялись телами, чтобы вернуть свое тело надо убить его нынешнего носителя", "Да, твое тело у той твари за вратами,а мое у Эрики,чтобы их вернуть надо их убить", "Эрика была человеком но тебе придется убить ее и тут тварь чтобы вернуть наши тела", "Нам надо убить нынешних владельцев наших тел");
    }

}

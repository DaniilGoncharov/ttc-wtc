using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// TODO: 1.Генерация карт в обычном режиме
///       2.Используемые предметы и список предметов
///       3.Предметы и сундуки
///       4.Сохранения и загрузка
/// </summary>
namespace ttc_wtc
{
    class Game
    {
        public enum Status
        {
            ClassMenu = 0,
            Closed = 1,
            InGame = 2,
            PauseMenu = 3,
            StartMenu = 4,
            Inventory = 5,
            SlotChoice = 6,
            ItemChoice = 7,
            ChestOpened = 8,
            CheatConsole = 9,
            InBattle = 10,
            InBattleForEntity = 11,
            InNPC = 12,
            InDialog=13,
            Theft=14
        }


        public Player Player;
        public List<Entity> Entities;
        public List<Chest> Chests;
        public NPC currentNPC;
        public bool mainQestisActive = true;

        public static Status GameStatus { get; set; }

        public Game(Player player, List<Entity> entities = null, List<Chest> chests = null)
        {
            this.Player = player;
            Entities = entities;
            Chests = chests;
        }

        public static void StartNewGame(bool endless = false, Status status = Status.StartMenu)
        {
            Program.CurrentGame = new Game(Program.GenerateStartPlayer(endless), endless ? null : Program.GenerateStartEntities(), endless ? null : Program.GenerateStartChests());
            GameStatus = status;
            Program.CurrentGame.Start(endless);
        }

        public static void StartGame(Player player, List<Entity> entities, List<Chest> chests, bool endless)
        {
            Program.CurrentGame = new Game(player, entities, chests);
            GameStatus = Status.InGame;
            Program.CurrentGame.Start(endless, false);
        }

        public static void GetStartMenuChoice()
        {
            bool gameStart = false; ;
            while(!gameStart)
            {
                int choice = Menu.StartMenu.GetChoice();
                switch (choice)
                {
                    case 0:
                        gameStart = true;
                        StartNewGame(false, Status.ClassMenu);
                        break;
                    case 1:
                        gameStart = true;
                        StartNewGame(true, Status.ClassMenu);
                        break;
                    case 2:
                        Player player = new Player();
                        List<Entity> entities = new List<Entity>();
                        List<Chest> chests = new List<Chest>();
                        if (SaveAndLoad.Load(ref player, ref entities, ref chests, out bool? endless))
                        {
                            gameStart = true;
                            StartGame(player, entities, chests, (bool)endless);
                        }
                        else
                        {
                            gameStart = false;
                        }
                        break;
                    case 3:
                        Environment.Exit(0);
                        break;
                }
            }
        }

        public void Start(bool endless = false, bool initialise = true)
        {
            if (!endless)
            {
                if (initialise)
                {
                    CollectedMaps.Initialise();
                }   
                foreach (Entity entity in Entities)
                {
                    CollectedMaps.SetEntity(entity.MapId, entity.X, entity.Y, entity);
                }
                foreach (Chest chest in Chests)
                {
                    CollectedMaps.SetChest(chest.MapId, chest.X, chest.Y, chest);
                }
            }
            else if (initialise)
            {
                CollectedMaps.EndlessInitialise();
            }

            CollectedMaps.SetEntity(Player.MapId, Player.X, Player.Y, Player);
            Draw.CurrentMapId = Player.MapId;
            int moveX = 0;
            int moveY = 0;
            do
            {
                if (Player.Have("Ключ от старых ворот"))
                {
                    Player.QuestNumber = 2;
                    CollectedMaps.InitialiseClosedDour(0);
                }
                switch (GameStatus)
                {
                    case Status.InGame:
                        InGameStatus(out moveX, out moveY, endless);
                        break;
                    case Status.ClassMenu:
                        TarotMenuStatus(endless);
                        break;
                    case Status.Inventory:
                        InventoryStatus(endless);
                        break;
                    case Status.ChestOpened:
                        ChestStatus(moveX, moveY, endless);
                        break;
                    case Status.InNPC:
                        NPCStatus(moveX,moveY);
                        break;
                    case Status.InBattle:
                        BattleStatus(endless);
                        break;
                    case Status.PauseMenu:
                        PauseStatus(endless);
                        break;
                    case Status.CheatConsole:
                        ConsoleStatus(endless);
                        break;
                    case Status.InDialog:
                        InDialog(moveX, moveY, endless);
                        break;
                    case Status.Theft:
                        InTheft(moveX, moveY);
                        break;
                    case Status.Closed:
                        Environment.Exit(0);
                        break;
                       

                }
            } while (true);
        }

        public void NPCStatus(int moveX,int moveY)
        {
            int choice = Menu.NPCMenu.GetChoice();
            switch (choice)
            {
                case 0:
                    GameStatus = Status.InDialog;
                    break;
                case 2:
                    currentNPC = (NPC)CollectedMaps.GetEntity(Player.MapId, Player.X + moveX, Player.Y + moveY);
                    Player.AddItems(CollectedMaps.GetAllItemsFromNPC(Player.MapId, Player.X + moveX, Player.Y + moveY));
                    currentNPC = new Enemy(currentNPC.Name, currentNPC.HP.MaximumHP, currentNPC.Damage.CurrentDamage,
                           currentNPC.Defense.CurrentDefense, currentNPC.MapId, currentNPC.X, y: currentNPC.Y);
                    CollectedMaps.SetEntity(currentNPC.MapId, currentNPC.X, currentNPC.Y, currentNPC);
                    GameStatus = Status.InBattle;
                    break;
                case 1:
                    GameStatus = Status.Theft;
                    break;
            }
        }
        public void InDialog(int moveX, int moveY, bool endlessGame = false) 
        {
           
            Console.Clear();
            currentNPC = (NPC)CollectedMaps.GetEntity(Player.MapId, Player.X + moveX, Player.Y + moveY);
            Quest.QestChecking(Player, currentNPC);
            if (currentNPC.Dialog.GetDialog(currentNPC) == 0)
            {
                if (currentNPC.Dialog.Completeness)
                {
                    Player.QuestNumber = 1;
                }
                GameStatus = Status.InGame;
            }
            else
            {
                Player.AddItems(CollectedMaps.GetAllItemsFromNPC(Player.MapId, Player.X + moveX, Player.Y + moveY));
                currentNPC = new Enemy(currentNPC.Name, currentNPC.HP.MaximumHP, currentNPC.Damage.CurrentDamage,
                       currentNPC.Defense.CurrentDefense, currentNPC.MapId, currentNPC.X, y: currentNPC.Y);
                CollectedMaps.SetEntity(currentNPC.MapId, currentNPC.X, currentNPC.Y, currentNPC);
                GameStatus = Status.InBattle;

            }
        }
        public void InTheft(int moveX, int moveY)
        {
            currentNPC = (NPC)CollectedMaps.GetEntity(Player.MapId, Player.X + moveX, Player.Y + moveY);
            Menu TiefsMenu = new Menu(currentNPC.GetTiefsItemNames());
            int theftChoice = TiefsMenu.GetChoice(true, true);
            if (theftChoice == currentNPC.GetTiefsItemNames().Count - 1)
            {
                GameStatus = Status.InGame;
            }
            else if (theftChoice < currentNPC.GetTiefsItemNames().Count - 2)
            {
                if (new Random().Next(0, 2) == 1)
                {
                    Player.AddItem(CollectedMaps.GetItemFromNPC(Player.MapId, Player.X + moveX, Player.Y + moveY, theftChoice));
                    GameStatus = Status.InGame;
                }
                else
                {
                    Player.AddItems(CollectedMaps.GetAllItemsFromNPC(Player.MapId, Player.X + moveX, Player.Y + moveY));
                    currentNPC = new Enemy(currentNPC.Name, currentNPC.HP.MaximumHP, currentNPC.Damage.CurrentDamage,
                           currentNPC.Defense.CurrentDefense, currentNPC.MapId, currentNPC.X, y: currentNPC.Y);
                    CollectedMaps.SetEntity(currentNPC.MapId, currentNPC.X, currentNPC.Y, currentNPC);
                    GameStatus = Status.InBattle;
                }
            }
            else if (theftChoice == currentNPC.GetTiefsItemNames().Count - 2)
            {
                if (new Random().Next(0, currentNPC.NPCInventory.Count) == 1)
                {
                    Player.AddItems(CollectedMaps.GetAllItemsFromNPC(Player.MapId, Player.X + moveX, Player.Y + moveY));
                    GameStatus = Status.InGame;
                }
                else
                {
                    Player.AddItems(CollectedMaps.GetAllItemsFromNPC(Player.MapId, Player.X + moveX, Player.Y + moveY));
                    currentNPC = new Enemy(currentNPC.Name, currentNPC.HP.MaximumHP, currentNPC.Damage.CurrentDamage,
                           currentNPC.Defense.CurrentDefense, currentNPC.MapId, currentNPC.X, y: currentNPC.Y);
                    CollectedMaps.SetEntity(currentNPC.MapId, currentNPC.X, currentNPC.Y, currentNPC);
                    GameStatus = Status.InBattle;
                }
            }
        }
        public void TarotMenuStatus(bool endlessGame = false)
        {
            int choice = Menu.TarotMenu.GetChoice();
            Player.SelectTarot(choice);
            GameStatus = Status.InGame;
        }

        public void InGameStatus(out int moveX, out int moveY, bool endlessGame = false)
        {
            Draw.ReDrawMap(CollectedMaps.GetDrawnMap(Player.MapId), Player.MapId);
            do
            {
                moveX = 0;
                moveY = 0;
                Draw.DrawMapInterface(Player, 53, 3);
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Escape:
                        GameStatus = Status.PauseMenu;
                        break;
                    case ConsoleKey.C:
                        GameStatus = Status.CheatConsole;
                        break;
                    case ConsoleKey.I:
                        GameStatus = Status.Inventory;
                        break;
                    case ConsoleKey.W:
                        moveY = -1;
                        break;
                    case ConsoleKey.A:
                        moveX = -1;
                        break;
                    case ConsoleKey.S:
                        moveY = 1;
                        break;
                    case ConsoleKey.D:
                        moveX = 1;
                        break;
                }
                if (((moveX != 0) || (moveY != 0)) && (Player.Move(moveX, moveY, endlessGame)))
                {
                    CollectedMaps.EnemyMovement(Player.MapId, Player.X, Player.Y);
                }             
                if (Entities!=null&&!Entities[0].Alive&&mainQestisActive&&Entities[1].Alive)
                {
                    Player.QuestNumber = 3;
                    Draw.ReDrawMap(CollectedMaps.GetDrawnMap(Player.MapId), Player.MapId);
                    NPC vilianNPC = (NPC)CollectedMaps.GetEntity(Entities[1].MapId,Entities[1].X,Entities[1].Y);
                    if (vilianNPC.Have("Статуэтка чайки"))
                    {
                        vilianNPC.HP=(2000,2000);
                        mainQestisActive = false;                       
                        Entities.RemoveAt(1);
                        CollectedMaps.DelEntity(vilianNPC.MapId, vilianNPC.X, vilianNPC.Y);
                       vilianNPC= new Enemy(vilianNPC.Name, vilianNPC.HP.MaximumHP, vilianNPC.Damage.CurrentDamage,
                       vilianNPC.Defense.CurrentDefense, vilianNPC.MapId, vilianNPC.X, y: vilianNPC.Y);
                        Entities.Add(vilianNPC);
                        CollectedMaps.SetEntity(vilianNPC.MapId, vilianNPC.X, vilianNPC.Y, vilianNPC);
                    }
                    else
                    {
                        mainQestisActive = false;                       
                        Entities.RemoveAt(1);
                        CollectedMaps.DelEntity(vilianNPC.MapId, vilianNPC.X, vilianNPC.Y);
                        vilianNPC = new Enemy(vilianNPC.Name, vilianNPC.HP.MaximumHP, vilianNPC.Damage.CurrentDamage,
                        vilianNPC.Defense.CurrentDefense, vilianNPC.MapId, vilianNPC.X, y: vilianNPC.Y);
                        Entities.Add(vilianNPC);
                        CollectedMaps.SetEntity(vilianNPC.MapId, vilianNPC.X, vilianNPC.Y, vilianNPC);
                    }

                    CollectedMaps.EnemyMovement(Player.MapId, Player.X, Player.Y);

                }
            } while (GameStatus == Status.InGame);
        }

        public void InventoryStatus(bool endlessGame = false)
        {
            do
            {
                List<string> inventoryItems = Player.GetInventorySlotNames();
                Menu inventoryMenu = new Menu(inventoryItems);
                int inventoryChoice = inventoryMenu.GetChoice();
                if (inventoryChoice == inventoryItems.Count - 1)
                {
                    GameStatus = Status.InGame;
                    Player.CountStatsByItems();
                    break;
                }
                Menu slotMenu = new Menu(Player.GetNamesBySlot(inventoryChoice));
                int slotChoice = slotMenu.GetChoice();
                if (slotChoice == 0)
                {
                    if (inventoryChoice != inventoryItems.Count - 2 && inventoryChoice != Consumable.ConsumableSlot)
                    {
                        Player.EquippedItems[inventoryChoice] = null;
                    }
                }
                else
                {
                    Player.ChangeItemByChoice(slotChoice, inventoryChoice);
                }
            } while (true);
        }

        public void ChestStatus(int moveX, int moveY, bool endlessGame = false)
        {
            string[] chestMenuItems = CollectedMaps.GetChestItems(Player.MapId, Player.X + moveX, Player.Y + moveY);
            Menu chestMenu = new Menu(chestMenuItems);
            int choice = chestMenu.GetChoice();
            if (choice < chestMenuItems.Length - 2)
            {
                Player.AddItem(CollectedMaps.GetItemFromChest(Player.MapId, Player.X + moveX, Player.Y + moveY, choice));
            }
            else if (choice == chestMenuItems.Length - 2)
            {
                Player.AddItems(CollectedMaps.GetAllItemsFromChest(Player.MapId, Player.X + moveX, Player.Y + moveY));
            }
            GameStatus = Status.InGame;
            if (CollectedMaps.GetChestItems(Player.MapId, Player.X + moveX, Player.Y + moveY).Length == 1)
            {
                CollectedMaps.DelChest(Player.MapId, Player.X + moveX, Player.Y + moveY);
            }
        }

        public void BattleStatus(bool endlessGame = false)
        {
            Battle currentBattle = new Battle(Player, CollectedMaps.GetNearEntities(Player.MapId, Player.X, Player.Y));
            currentBattle.Start();
        }

        public void PauseStatus(bool endlessGame = false)
        {
            int choice = Menu.PauseMenu.GetChoice();
            switch (choice)
            {
                case 0:
                    GameStatus = Status.InGame;
                    break;
                case 1:
                    SaveAndLoad.Save(Player, Entities, Chests, endlessGame);
                    break;
                case 2:
                    if (SaveAndLoad.Load(ref Player, ref Entities, ref Chests, out bool? endless))
                    {
                        StartGame(Player, Entities, Chests, (bool)endless);
                    }
                    break;
                case 3:
                    GetStartMenuChoice();
                    break;
            }
        }

        public void ConsoleStatus(bool endlessGame = false)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Введите команду: ");
            Console.CursorVisible = true;
            while (GameStatus == Status.CheatConsole)
            {
                string input = Console.ReadLine();
                string[] strings = input.Split(' ');
                switch (strings.Length)
                {
                    case 0:
                    case 1:
                        switch (strings[0])
                        {
                            case "help":
                                Console.WriteLine("help - список команд \n" +
                                              "kill - убить игрока \n" +
                                              "restart - запустить игру с самого начала \n" +
                                              "exit - выйти из консоли \n" +
                                              "closeapp - выйти из игры \n" +
                                              "give {id} - получить предмет по id \n" +
                                              "set {stat} {number} - изменить значение данной характеристики");
                                break;
                            case "kill":
                                Console.ResetColor();
                                Console.CursorVisible = false;
                                StartNewGame(endlessGame);
                                break;
                            case "restart":
                                Console.ResetColor();
                                Console.CursorVisible = false;
                                StartNewGame(endlessGame, Status.ClassMenu);
                                break;
                            case "exit":
                                Console.ResetColor();
                                Console.CursorVisible = false;
                                GameStatus = Status.InGame;
                                break;
                            case "closeapp":
                                Environment.Exit(0);
                                break;
                            default:
                                Console.WriteLine("Неизвестная команда. Введите help для просмотра списка команд.");
                                break;
                        }
                        break;
                    case 2:
                        switch (strings[0])
                        {
                            case "give":
                                if (int.TryParse(strings[1], out int id) && id >= 0 && id < Item.Items.Count)
                                {
                                    Player.AddItem(Item.Items[id]);
                                    Console.WriteLine("Успешно.");
                                }
                                else Console.WriteLine("Неверный id предмета. введите целое число от 0 до {0}.", Item.Items.Count - 1);
                                break;
                            default:
                                Console.WriteLine("Неизвестная команда. Введите help для просмотра списка команд.");
                                break;
                        }
                        break;
                    case 3:
                        if (strings[0] == "set")
                        {
                            bool error = true;
                            int value;
                            switch (strings[1])
                            {
                                case "hp":
                                    if (int.TryParse(strings[2], out value) && value > 0)
                                    {
                                        Player.HP = (value, Player.HP.MaximumHP > value ? Player.HP.MaximumHP : value);
                                        error = false;
                                    }
                                    break;
                                case "damage":
                                    if (int.TryParse(strings[2], out value) && value > 0)
                                    {
                                        Player.Damage = (value, value);
                                        error = false;
                                    }
                                    break;
                                case "defense":
                                    if (int.TryParse(strings[2], out value) && value > 0)
                                    {
                                        Player.Defense = (value, value);
                                        error = false;
                                    }
                                    break;
                                default:
                                    break;
                            }
                            if (error)
                            {
                                Console.WriteLine("Произошла ошибка. Проверьте правильность данных.");
                            }
                        }
                        else Console.WriteLine("Неизвестная команда. Введите help для просмотра списка команд.");
                        break;
                }
            }
        }
    }
}

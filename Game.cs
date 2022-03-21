using System;
using System.Collections.Generic;

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
            InDialog = 13,
            Theft = 14
        }

        public Player player;
        public List<Entity> entities;
        public List<Chest> chests;
        public NPC CurrentNPC { get; set; }
        public bool mainQuestIsActive = true;
        public bool supQestisActive = true;
        public static Status GameStatus { get; set; }

        public Game(Player player, List<Entity> entities = null, List<Chest> chests = null)
        {
            this.player = player;
            this.entities = entities;
            this.chests = chests;
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
            while (!gameStart)
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
                foreach (Entity entity in entities)
                {
                    CollectedMaps.SetEntity(entity.MapId, entity.X, entity.Y, entity);
                }
                foreach (Chest chest in chests)
                {
                    CollectedMaps.SetChest(chest.MapId, chest.X, chest.Y, chest);
                }
            }
            else if (initialise)
            {
                CollectedMaps.EndlessInitialise();
            }

            CollectedMaps.SetEntity(player.MapId, player.X, player.Y, player);
            Draw.CurrentMapId = player.MapId;
            int moveX = 0;
            int moveY = 0;
            do
            {
                if (player.Have("Ключ от старых ворот") && supQestisActive)
                {
                    player.QuestNumber = 2;
                    CollectedMaps.InitialiseClosedDour(0);
                    supQestisActive = false;
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
                        NPCStatus(moveX, moveY);
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

        public void NPCStatus(int moveX, int moveY)
        {
            int choice = Menu.NPCMenu.GetChoice();
            switch (choice)
            {
                case 0:
                    GameStatus = Status.InDialog;
                    break;
                case 2:
                    CurrentNPC = (NPC)CollectedMaps.GetEntity(player.MapId, player.X + moveX, player.Y + moveY);
                    player.AddItems(CollectedMaps.GetAllItemsFromNPC(player.MapId, player.X + moveX, player.Y + moveY));
                    CurrentNPC = new Enemy(CurrentNPC.Name, CurrentNPC.HP.MaximumHP, CurrentNPC.Damage.CurrentDamage,
                           CurrentNPC.Defense.CurrentDefense, CurrentNPC.MapId, CurrentNPC.X, y: CurrentNPC.Y);
                    CollectedMaps.SetEntity(CurrentNPC.MapId, CurrentNPC.X, CurrentNPC.Y, CurrentNPC);
                    GameStatus = Status.InBattle;
                    break;
                case 1:
                    GameStatus = Status.Theft;
                    break;
                case 3:
                    GameStatus = Status.InGame;
                    break;
            }
        }

        public void InDialog(int moveX, int moveY, bool endlessGame = false)
        {

            Console.Clear();
            CurrentNPC = (NPC)CollectedMaps.GetEntity(player.MapId, player.X + moveX, player.Y + moveY);
            Quest.QestChecking(player, CurrentNPC, entities[0]);
            if (CurrentNPC.Dialog.GetDialog(CurrentNPC) == 0)
            {
                if (CurrentNPC.Dialog.Completeness && CurrentNPC.Name == "Эрика")
                {
                    player.QuestNumber = 1;
                }
                GameStatus = Status.InGame;
            }
            else
            {
                player.AddItems(CollectedMaps.GetAllItemsFromNPC(player.MapId, player.X + moveX, player.Y + moveY));
                CurrentNPC = new Enemy(CurrentNPC.Name, CurrentNPC.HP.MaximumHP, CurrentNPC.Damage.CurrentDamage,
                       CurrentNPC.Defense.CurrentDefense, CurrentNPC.MapId, CurrentNPC.X, y: CurrentNPC.Y);
                CollectedMaps.SetEntity(CurrentNPC.MapId, CurrentNPC.X, CurrentNPC.Y, CurrentNPC);
                GameStatus = Status.InBattle;

            }
        }

        public void InTheft(int moveX, int moveY)
        {
            CurrentNPC = (NPC)CollectedMaps.GetEntity(player.MapId, player.X + moveX, player.Y + moveY);
            Menu TiefsMenu = new Menu(CurrentNPC.GetTiefsItemNames());
            int theftChoice = TiefsMenu.GetChoice(true, true);
            if (theftChoice == CurrentNPC.GetTiefsItemNames().Count - 1)
            {
                GameStatus = Status.InGame;
            }
            else if (theftChoice < CurrentNPC.GetTiefsItemNames().Count - 2)
            {
                if (new Random().Next(0, 2) == 1)
                {
                    player.AddItem(CollectedMaps.GetItemFromNPC(player.MapId, player.X + moveX, player.Y + moveY, theftChoice));
                    GameStatus = Status.InGame;
                }
                else
                {
                    player.AddItems(CollectedMaps.GetAllItemsFromNPC(player.MapId, player.X + moveX, player.Y + moveY));
                    CurrentNPC = new Enemy(CurrentNPC.Name, CurrentNPC.HP.MaximumHP, CurrentNPC.Damage.CurrentDamage,
                           CurrentNPC.Defense.CurrentDefense, CurrentNPC.MapId, CurrentNPC.X, y: CurrentNPC.Y);
                    CollectedMaps.SetEntity(CurrentNPC.MapId, CurrentNPC.X, CurrentNPC.Y, CurrentNPC);
                    GameStatus = Status.InBattle;
                }
            }
            else if (theftChoice == CurrentNPC.GetTiefsItemNames().Count - 2)
            {
                if (new Random().Next(0, CurrentNPC.NPCInventory.Count) == 1)
                {
                    player.AddItems(CollectedMaps.GetAllItemsFromNPC(player.MapId, player.X + moveX, player.Y + moveY));
                    GameStatus = Status.InGame;
                }
                else
                {
                    player.AddItems(CollectedMaps.GetAllItemsFromNPC(player.MapId, player.X + moveX, player.Y + moveY));
                    CurrentNPC = new Enemy(CurrentNPC.Name, CurrentNPC.HP.MaximumHP, CurrentNPC.Damage.CurrentDamage,
                           CurrentNPC.Defense.CurrentDefense, CurrentNPC.MapId, CurrentNPC.X, y: CurrentNPC.Y);
                    CollectedMaps.SetEntity(CurrentNPC.MapId, CurrentNPC.X, CurrentNPC.Y, CurrentNPC);
                    GameStatus = Status.InBattle;
                }
            }
        }

        public void TarotMenuStatus(bool endlessGame = false)
        {
            int choice = Menu.TarotMenu.GetChoice();
            player.SelectTarot(choice);
            GameStatus = Status.InGame;
        }

        public void InGameStatus(out int moveX, out int moveY, bool endlessGame = false)
        {
            Draw.ReDrawMap(CollectedMaps.GetDrawnMap(player.MapId), player.MapId);
            do
            {
                moveX = 0;
                moveY = 0;
                Draw.DrawMapInterface(player, 53, 3, endlessGame);
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
                if (((moveX != 0) || (moveY != 0)) && (player.Move(moveX, moveY, endlessGame)))
                {
                    CollectedMaps.EnemyMovement(player.MapId, player.X, player.Y);
                }
                if (entities != null && !entities[0].Alive && mainQuestIsActive && entities[1].Alive)
                {
                    player.QuestNumber = 3;
                    Draw.ReDrawMap(CollectedMaps.GetDrawnMap(player.MapId), player.MapId);
                    NPC vilianNPC = (NPC)CollectedMaps.GetEntity(entities[1].MapId, entities[1].X, entities[1].Y);
                    if (vilianNPC.Have("Статуэтка чайки"))
                    {
                        vilianNPC.HP = (6000, 6000);
                        vilianNPC.Damage = (500, 500);
                        vilianNPC.Defense = (50, 50);
                        mainQuestIsActive = false;
                        entities.RemoveAt(1);
                        CollectedMaps.DelEntity(vilianNPC.MapId, vilianNPC.X, vilianNPC.Y);
                        vilianNPC = new Enemy(vilianNPC.Name, vilianNPC.HP.MaximumHP, vilianNPC.Damage.CurrentDamage,
                        vilianNPC.Defense.CurrentDefense, vilianNPC.MapId, vilianNPC.X, y: vilianNPC.Y);
                        entities.Add(vilianNPC);
                        CollectedMaps.SetEntity(vilianNPC.MapId, vilianNPC.X, vilianNPC.Y, vilianNPC);
                    }
                    else
                    {
                        mainQuestIsActive = false;
                        entities.RemoveAt(1);
                        CollectedMaps.DelEntity(vilianNPC.MapId, vilianNPC.X, vilianNPC.Y);
                        vilianNPC = new Enemy(vilianNPC.Name, vilianNPC.HP.MaximumHP, vilianNPC.Damage.CurrentDamage,
                        vilianNPC.Defense.CurrentDefense, vilianNPC.MapId, vilianNPC.X, y: vilianNPC.Y);
                        entities.Add(vilianNPC);
                        CollectedMaps.SetEntity(vilianNPC.MapId, vilianNPC.X, vilianNPC.Y, vilianNPC);
                    }
                    CollectedMaps.EnemyMovement(player.MapId, player.X, player.Y);
                }
            } while (GameStatus == Status.InGame);
        }

        public void InventoryStatus(bool endlessGame = false)
        {
            do
            {
                List<string> inventoryItems = player.GetInventorySlotNames();
                Menu inventoryMenu = new Menu(inventoryItems);
                int inventoryChoice = inventoryMenu.GetChoice();
                if (inventoryChoice == inventoryItems.Count - 1)
                {
                    GameStatus = Status.InGame;
                    player.CountStatsByItems();
                    break;
                }
                Menu slotMenu = new Menu(player.GetNamesBySlot(inventoryChoice));
                int slotChoice = slotMenu.GetChoice();
                if (slotChoice == 0)
                {
                    if (inventoryChoice != inventoryItems.Count - 2 && inventoryChoice != Consumable.ConsumableSlot)
                    {
                        player.EquippedItems[inventoryChoice] = null;
                    }
                }
                else
                {
                    player.ChangeItemByChoice(slotChoice, inventoryChoice);
                }
            } while (true);
        }

        public void ChestStatus(int moveX, int moveY, bool endlessGame = false)
        {
            string[] chestMenuItems = CollectedMaps.GetChestItems(player.MapId, player.X + moveX, player.Y + moveY);
            Menu chestMenu = new Menu(chestMenuItems);
            int choice = chestMenu.GetChoice();
            if (choice < chestMenuItems.Length - 2)
            {
                player.AddItem(CollectedMaps.GetItemFromChest(player.MapId, player.X + moveX, player.Y + moveY, choice));
            }
            else if (choice == chestMenuItems.Length - 2)
            {
                player.AddItems(CollectedMaps.GetAllItemsFromChest(player.MapId, player.X + moveX, player.Y + moveY));
            }
            GameStatus = Status.InGame;
            if (CollectedMaps.GetChestItems(player.MapId, player.X + moveX, player.Y + moveY).Length == 1)
            {
                CollectedMaps.DelChest(player.MapId, player.X + moveX, player.Y + moveY);
            }
        }

        public void BattleStatus(bool endlessGame = false)
        {
            Battle currentBattle = new Battle(player, CollectedMaps.GetNearEnemies(player.MapId, player.X, player.Y));
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
                    SaveAndLoad.Save(player, entities, chests, endlessGame);
                    break;
                case 2:
                    if (SaveAndLoad.Load(ref player, ref entities, ref chests, out bool? endless))
                    {
                        StartGame(player, entities, chests, (bool)endless);
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
                                    player.AddItem(Item.Items[id]);
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
                                        player.HP = (value, player.HP.MaximumHP > value ? player.HP.MaximumHP : value);
                                        error = false;
                                        Console.WriteLine("Успешно.");
                                    }
                                    break;
                                case "damage":
                                    if (int.TryParse(strings[2], out value) && value > 0)
                                    {
                                        player.Damage = (value, value);
                                        error = false;
                                        Console.WriteLine("Успешно.");
                                    }
                                    break;
                                case "defense":
                                    if (int.TryParse(strings[2], out value) && value > 0)
                                    {
                                        player.Defense = (value, value);
                                        error = false;
                                        Console.WriteLine("Успешно.");
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

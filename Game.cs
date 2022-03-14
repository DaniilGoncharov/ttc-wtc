using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

        private Player player;
        private List<Entity> entities;
        private List<Chest> chests;
        public Status GameStatus { get; set; }

        public Game(Player Player, List<Entity> Entities, List<Chest> Chests)
        {
            player = Player;
            entities = Entities;
            chests = Chests;
        }

        public static void StartANewGame(Status status = Status.StartMenu)
        {
            Program.CurrentGame = new Game(Program.GenerateStartPlayer(), Program.GenerateStartEntities(), Program.GenerateStartChests());
            Program.CurrentGame.GameStatus = status;
            Program.CurrentGame.Start();
        }

        public void Start()
        {
            CollectedMaps.Initialise();
            CollectedMaps.SetEntity(player.MapId, player.X, player.Y, player);
            Draw.CurrentMapId = player.MapId;
            foreach (Entity entity in entities)
            {
                CollectedMaps.SetEntity(entity.MapId, entity.X, entity.Y, entity);
            }
            foreach (Chest chest in chests)
            {
                CollectedMaps.SetChest(chest.MapId, chest.X, chest.Y, chest);
            }
            int moveX = 0;
            int moveY = 0;
            int choice;
            do
            {
                switch (GameStatus)
                {
                    case Status.InGame:
                        Draw.ReDrawMap(CollectedMaps.GetDrawnMap(player.MapId), player.MapId);
                        do
                        {
                            moveX = 0;
                            moveY = 0;
                            Draw.DrawMapInterface(player, 53, 3);
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
                            if (((moveX != 0) || (moveY != 0)) && (player.Move(moveX, moveY)))
                            {
                                CollectedMaps.EnemyMovement(player.MapId, player.X, player.Y);
                            }
                        } while (GameStatus == Status.InGame);
                        break;
                    case Status.StartMenu:
                        choice = Menu.StartMenu.GetChoice();
                        switch (choice)
                        {
                            case 0:
                                GameStatus = Status.ClassMenu;
                                break;
                            case 1:
                                GameStatus = Status.Closed;
                                break;
                        }
                        break;
                    case Status.ClassMenu:
                        choice = Menu.TarotMenu.GetChoice();
                        player.SelectTarot(choice);
                        GameStatus = Status.InGame;
                        break;
                    case Status.Inventory:
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
                        break;
                    case Status.ChestOpened:
                        string[] chestMenuItems = CollectedMaps.GetChestItems(player.MapId, player.X + moveX, player.Y + moveY);
                        Menu chestMenu = new Menu(chestMenuItems);
                        choice = chestMenu.GetChoice();
                        if (choice < chestMenuItems.Length - 2)
                        {
                            player.AddItem(CollectedMaps.GetItemFromChest(player.MapId, player.X + moveX, player.Y + moveY, choice));
                        }
                        else if (choice == chestMenuItems.Length - 2)
                        {
                            player.AddItems(CollectedMaps.GetAllItemsFromChest(player.MapId, player.X + moveX, player.Y + moveY));
                        }
                        GameStatus = Status.InGame;
                        break;
                    case Status.InNPC:
                        choice = Menu.NPCMenu.GetChoice();
                        switch(choice)
                        {
                            case 0:
                                GameStatus = Status.InGame;
                                break;
                        }
                        break;
                    case Status.InBattle:
                        Battle currentBattle = new Battle(player, CollectedMaps.GetNearEntities(player.MapId, player.X, player.Y));
                        currentBattle.Start();
                        break;
                    case Status.PauseMenu:
                        choice = Menu.PauseMenu.GetChoice();
                        switch (choice)
                        {
                            case 0:
                                GameStatus = Status.InGame;
                                break;
                            case 1:
                                StartANewGame();
                                break;
                        }
                        break;
                    case Status.CheatConsole:
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
                                            StartANewGame();
                                            break;
                                        case "restart":
                                            Console.ResetColor();
                                            Console.CursorVisible = false;
                                            StartANewGame(Status.ClassMenu);
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
                                            if (int.TryParse(strings[1], out int id) && id >= 0 && id < Item.Items.Length)
                                            {
                                                player.AddItem(Item.Items[id]);
                                                Console.WriteLine("Успешно.");
                                            }
                                            else Console.WriteLine("Неверный id предмета. введите целое число от 0 до {0}.", Item.Items.Length - 1);
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
                                                if(int.TryParse(strings[2], out value) && value > 0)
                                                {
                                                    player.HP = (value, player.HP.MaximumHP > value ? player.HP.MaximumHP : value);
                                                    error = false;
                                                }
                                                break;
                                            case "damage":
                                                if (int.TryParse(strings[2], out value) && value > 0)
                                                {
                                                    player.Damage = (value, value);
                                                    error = false;
                                                }
                                                break;
                                            case "defense":
                                                if (int.TryParse(strings[2], out value) && value > 0)
                                                {
                                                    player.Defense = (value, value);
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
                        break;
                    case Status.Closed:
                        Environment.Exit(0);
                        break;
                }
            } while (true);
        }
    }
}

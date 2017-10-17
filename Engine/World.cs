using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public static class World
    {
        public static readonly List<Item> Items = new List<Item>();
        public static readonly List<Monster> Monsters = new List<Monster>();
        public static readonly List<Quest> Quests = new List<Quest>();
        public static readonly List<Location> Locations = new List<Location>();

        public const int ITEM_ID_RUSTY_SWORD = 1;
        public const int ITEM_ID_RAT_TAIL = 2;
        public const int ITEM_ID_PIECE_OF_FUR = 3;
        public const int ITEM_ID_SNAKE_FANG = 4;
        public const int ITEM_ID_SNAKESKIN = 5;
        public const int ITEM_ID_CLUB = 6;
        public const int ITEM_ID_HEALING_POTION = 7;
        public const int ITEM_ID_SPIDER_FANG = 8;
        public const int ITEM_ID_SPIDER_SILK = 9;
        public const int ITEM_ID_ADVENTURER_PASS = 10;
        public const int ITEM_ID_IRON_SWORD = 11; //added
        public const int ITEM_ID_WOLF_FUR = 12; //added
        public const int ITEM_ID_HELMET = 13; //added
        public const int ITEM_ID_SHIELD = 14; //added
        public const int ITEM_ID_GHEALING_POTION = 15; //added
        public const int ITEM_ID_LEATHER_VEST = 16; //added
        public const int ITEM_ID_WOLF_EAR = 17; //added
        
        public const int MONSTER_ID_RAT = 1;
        public const int MONSTER_ID_SNAKE = 2;
        public const int MONSTER_ID_GIANT_SPIDER = 3;
        public const int MONSTER_ID_WOLF = 4; //added

        public const int QUEST_ID_CLEAR_ALCHEMIST_GARDEN = 1;
        public const int QUEST_ID_CLEAR_FARMERS_FIELD = 2;
        public const int QUEST_ID_CLEAR_WOLVES_DEN = 3; //added

        public const int LOCATION_ID_HOME = 1;
        public const int LOCATION_ID_TOWN_SQUARE = 2;
        public const int LOCATION_ID_GUARD_POST = 3;
        public const int LOCATION_ID_ALCHEMIST_HUT = 4;
        public const int LOCATION_ID_ALCHEMISTS_GARDEN = 5;
        public const int LOCATION_ID_FARMHOUSE = 6;
        public const int LOCATION_ID_FARM_FIELD = 7;
        public const int LOCATION_ID_BRIDGE = 8;
        public const int LOCATION_ID_SPIDER_FIELD = 9;
        public const int LOCATION_ID_WOLVES_DEN = 10; //added
        public const int LOCATION_ID_CLIFFS = 11; //added

        static World()
        {
            PopulateItems();
            PopulateMonsters();
            PopulateQuests();
            PopulateLocations();
        }

        private static void PopulateItems()
        {
            Items.Add(new Weapon(ITEM_ID_RUSTY_SWORD, "Rusty sword", "Rusty swords", 0, 5));
            Items.Add(new Item(ITEM_ID_RAT_TAIL, "Rat tail", "Rat tails"));
            Items.Add(new Item(ITEM_ID_PIECE_OF_FUR, "Piece of fur", "Pieces of fur"));
            Items.Add(new Item(ITEM_ID_SNAKE_FANG, "Snake fang", "Snake fangs"));
            Items.Add(new Item(ITEM_ID_SNAKESKIN, "Snakeskin", "Snakeskins"));
            Items.Add(new Weapon(ITEM_ID_CLUB, "Club", "Clubs", 3, 10));
            Items.Add(new Weapon(ITEM_ID_IRON_SWORD, "Iron Sword", "Iron Swords", 5, 8)); //added
            Items.Add(new HealingPotion(ITEM_ID_HEALING_POTION, "Healing potion", "Healing potions", 5));
            Items.Add(new HealingPotion(ITEM_ID_GHEALING_POTION, "Greater healing potion", "Greater healing potions", 10)); //added
            Items.Add(new Item(ITEM_ID_SPIDER_FANG, "Spider fang", "Spider fangs"));
            Items.Add(new Item(ITEM_ID_SPIDER_SILK, "Spider silk", "Spider silks"));
            Items.Add(new Item(ITEM_ID_WOLF_FUR, "Wolf fur", "Wolf furs")); //added
            Items.Add(new Armor(ITEM_ID_HELMET, "Helmet", "Helmets",0,2,0));
            Items.Add(new Armor(ITEM_ID_SHIELD, "Shield", "Shields", 0, 2, 0));
            Items.Add(new Armor(ITEM_ID_LEATHER_VEST, "Leather vest", "Leather vests", 0, 1, 25));
            Items.Add(new Item(ITEM_ID_ADVENTURER_PASS, "Adventurer pass", "Adventurer passes"));
            Items.Add(new Item(ITEM_ID_WOLF_EAR, "Wolf's ear", "Wolf's ears")); //added

        }
        private static void PopulateMonsters()
        {
            Monster rat = new Monster(MONSTER_ID_RAT, "Rat", 5, 3, 10, 3, 3);
            rat.LootTable.Add(new LootItem(ItemByID(ITEM_ID_RAT_TAIL), 75, false));
            rat.LootTable.Add(new LootItem(ItemByID(ITEM_ID_PIECE_OF_FUR), 75, true));

            Monster snake = new Monster(MONSTER_ID_SNAKE, "Snake", 5, 3, 10, 3, 3);
            snake.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SNAKE_FANG), 75, false));
            snake.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SNAKESKIN), 75, true));

            Monster giantSpider = new Monster(MONSTER_ID_GIANT_SPIDER, "Giant spider", 20, 5, 40, 10, 10);
            giantSpider.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SPIDER_FANG), 75, true));
            giantSpider.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SPIDER_SILK), 75, false));

            Monster wolf = new Monster(MONSTER_ID_WOLF, "Wolf", 25, 6, 50, 14, 14);
            wolf.LootTable.Add(new LootItem(ItemByID(ITEM_ID_WOLF_FUR), 75, true));
            wolf.LootTable.Add(new LootItem(ItemByID(ITEM_ID_WOLF_EAR), 75, false));

            Monsters.Add(rat);
            Monsters.Add(snake);
            Monsters.Add(giantSpider);
            Monsters.Add(wolf);
        }

        private static void PopulateQuests()
        {
            Quest clearAlchemistGarden = new Quest(QUEST_ID_CLEAR_ALCHEMIST_GARDEN,
                "Clear the Alchemist's Garden",
                "Kill rats in the Alchemist's Garden and bring back 3 rat tails. " +
                "You will receive a healing potion and 10 gold pieces.", 20, 10);

            clearAlchemistGarden.QuestCompletionItems.Add(new QuestCompletionItem
                (ItemByID(ITEM_ID_RAT_TAIL), 3));

            clearAlchemistGarden.RewardItem = ItemByID(ITEM_ID_HEALING_POTION);

            Quest clearFarmersField = new Quest(QUEST_ID_CLEAR_FARMERS_FIELD,
                "Clear the Farmer's Field",
                "Kill snakes in the farmer's field and bring back 3 snake fangs. " +
                "You will receive an adventurer's pass and 20 gold pieces.", 20, 20);

            clearFarmersField.QuestCompletionItems.Add(new QuestCompletionItem(ItemByID(ITEM_ID_SNAKE_FANG), 3));

            clearFarmersField.RewardItem = ItemByID(ITEM_ID_ADVENTURER_PASS);

            Quest clearWolvesDen = new Quest(QUEST_ID_CLEAR_WOLVES_DEN,
                "Clear the Wolves' Den",
                "Kill the beasts in the Wolves' Den and bring back 3 wolf furs." +
                " You will receive a greater healing potion and 25 gold pieces.", 20, 25);

            clearWolvesDen.QuestCompletionItems.Add(new QuestCompletionItem(ItemByID(ITEM_ID_WOLF_FUR), 3));

            clearWolvesDen.RewardItem = ItemByID(ITEM_ID_GHEALING_POTION);

            Quests.Add(clearAlchemistGarden);
            Quests.Add(clearFarmersField);
            Quests.Add(clearWolvesDen);
        }

        private static void PopulateLocations()
        {
            //Create each location
            Location home = new Location(LOCATION_ID_HOME, "Home",
                "Your house. What a mess!");

            Location townSquare = new Location(LOCATION_ID_TOWN_SQUARE,
                "Town square", "You see a fountain and many market stalls.");

            Location alchemistHut = new Location(LOCATION_ID_ALCHEMIST_HUT,
                "Alchemist's hut", "There are many strange plants and research notes strewn about.");
            alchemistHut.QuestAvailableHere = QuestByID(QUEST_ID_CLEAR_ALCHEMIST_GARDEN);

            Location alchemistsGarden = new Location(LOCATION_ID_ALCHEMISTS_GARDEN,
                "Alchemist's garden", "Many plants are growing here.");
            alchemistsGarden.MonsterLivingHere = MonsterByID(MONSTER_ID_RAT);

            Location farmhouse = new Location(LOCATION_ID_FARMHOUSE,
                "Farmhouse", "There is a small farmhouse with chickens running amock.");
            farmhouse.QuestAvailableHere = QuestByID(QUEST_ID_CLEAR_FARMERS_FIELD);

            Location farmersField = new Location(LOCATION_ID_FARM_FIELD,
                "Farmer's field", "You see rows of corn as far as the eye can see.");
            farmersField.MonsterLivingHere = MonsterByID(MONSTER_ID_SNAKE);

            Location guardPost = new Location(LOCATION_ID_GUARD_POST,
                "Guard post", "There is a large, tough-looking guard here.",
                ItemByID(ITEM_ID_ADVENTURER_PASS));

            Location bridge = new Location(LOCATION_ID_BRIDGE,
                "Bridge", "A stone bridge crosses a rapid river.");

            Location spiderField = new Location(LOCATION_ID_SPIDER_FIELD,
                "Forest", "You see spider webs covering the trees in this forest.");
            spiderField.MonsterLivingHere = MonsterByID(MONSTER_ID_GIANT_SPIDER);

            Location cliffs = new Location(LOCATION_ID_CLIFFS, "Cliffs",
                "Rocky path hidden in shadows. You feel strong winds and hear ominous noises");

            Location wolvesden = new Location(LOCATION_ID_WOLVES_DEN, "Dark cave",
                "Large dark cave entrance. Howling is heard within.");
            wolvesden.MonsterLivingHere = MonsterByID(MONSTER_ID_WOLF);

            //link locations together

            home.LocationToNorth = townSquare;

            townSquare.LocationToNorth = alchemistHut;
            townSquare.LocationToSouth = home;
            townSquare.LocationToEast = guardPost;
            townSquare.LocationToWest = farmhouse;

            farmhouse.LocationToEast = townSquare;
            farmhouse.LocationToWest = farmersField;

            farmersField.LocationToEast = farmhouse;

            alchemistHut.LocationToSouth = townSquare;
            alchemistHut.LocationToNorth = alchemistsGarden;

            alchemistsGarden.LocationToSouth = alchemistHut;

            guardPost.LocationToEast = bridge;
            guardPost.LocationToWest = townSquare;

            bridge.LocationToWest = guardPost;
            bridge.LocationToEast = spiderField;
            bridge.LocationToNorth = cliffs;

            spiderField.LocationToWest = bridge;
            spiderField.LocationToNorth = cliffs;

            cliffs.LocationToSouth = bridge;
            cliffs.LocationToNorth = wolvesden;

            wolvesden.LocationToSouth = cliffs;

            //add locations to static list

            Locations.Add(home);
            Locations.Add(townSquare);
            Locations.Add(guardPost);
            Locations.Add(alchemistHut);
            Locations.Add(alchemistsGarden);
            Locations.Add(farmhouse);
            Locations.Add(farmersField);
            Locations.Add(bridge);
            Locations.Add(spiderField);
            Locations.Add(cliffs);
            Locations.Add(wolvesden);
        }

        public static Item ItemByID(int id)
        {
            foreach(Item item in Items)
            {
                if(item.ID == id)
                {
                    return item;
                }
            }
            return null;
        }

        public static Monster MonsterByID(int id)
        {
            foreach(Monster monster in Monsters)
            {
                if(monster.ID == id)
                {
                    return monster;
                }
            }
            return null;
        }

        public static Quest QuestByID(int id)
        {
            foreach(Quest quest in Quests)
            {
                if(quest.ID == id)
                {
                    return quest;
                }
            }
            return null;
        }

        public static Location LocationByID(int id)
        {
            foreach(Location location in Locations)
            {
                if(location.ID == id)
                {
                    return location;
                }
            }
            return null;
        }
    }
}

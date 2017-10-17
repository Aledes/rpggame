using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Player : LivingCreature
    {
        public int Gold { get; set; }
        public int ExperiencePoints { get; set; }
        public int Level
        {
            get { return ((ExperiencePoints / 100) + 1); }
        }

        public Location CurrentLocation { get; set; }
        public List<InventoryItem> Inventory { get; set; }
        public List<PlayerQuest> Quests { get; set; }

        public Player(int currentHitPoints, int maxmimumHitPoints, int gold,
            int experiencePoints)
                : base(currentHitPoints, maxmimumHitPoints)
        {
            Gold = gold;
            ExperiencePoints = experiencePoints;

            Inventory = new List<InventoryItem>();
            Quests = new List<PlayerQuest>();
        }

        public bool HasRequiredItemToEnterThisLocation(Location location)
        {
            if(location.ItemRequiredToEnter == null)
            {
                //no required item for this location, return true
                return true;
            }
            //see if the player has required item in inv
            return Inventory.Exists(ii => ii.Details.ID == location.ItemRequiredToEnter.ID);
        }

        public bool HasThisQuest(Quest quest)
        {
            return Quests.Exists(pq => pq.Details.ID == quest.ID);
        }

        public bool CompletedThisQuest(Quest quest)
        {
            foreach(PlayerQuest playerQuest in Quests)
            {
                if(playerQuest.Details.ID == quest.ID)
                {
                    return playerQuest.IsCompleted;
                }
            }

            return false;
        }

        public bool HasAllQuestCompletionItems(Quest quest)
        {
            //see if player has all items needed to complete quest here
            foreach(QuestCompletionItem qci in quest.QuestCompletionItems)
            {
                //check each item in inventory if they have it and enough of it
                if(!Inventory.Exists(ii=> ii.Details.ID ==
                    qci.Details.ID && ii.Quantity >= qci.Quantity))
                {
                    return false;
                }
            }
            //if here, player had all required items and enough of them to complete quest.
            return true;
        }

        public void RemoveQuestCompletionItems(Quest quest)
        {
            foreach(QuestCompletionItem qci in quest.QuestCompletionItems)
            {
                InventoryItem item = Inventory.SingleOrDefault(ii =>
                    ii.Details.ID == qci.Details.ID);

                if(item != null)
                {
                    //subtract quantity from player inv needed for quest
                    item.Quantity -= qci.Quantity;
                }
            }
        }

        public void AddItemToInventory(Item itemToAdd)
        {
            InventoryItem item = Inventory.SingleOrDefault(
                ii => ii.Details.ID == itemToAdd.ID);

            if (item == null)
            {
                //they didn't have the item, so add to their inventory, quantity: 1
                Inventory.Add(new InventoryItem(itemToAdd, 1));
            }
            else
            {
                //they have item in inventory, increase by one
                item.Quantity++;
            }
        }

        public void MarkQuestCompleted(Quest quest)
        {
            //find the quest in the player's quest list
            PlayerQuest playerQuest = Quests.SingleOrDefault(
                pq => pq.Details.ID == quest.ID);

            if(playerQuest != null)
            {
                playerQuest.IsCompleted = true;
            }
            
        }
    }
}

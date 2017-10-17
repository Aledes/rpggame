using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engine;

namespace RPG
{
    public partial class AdventureTime : Form
    {
        private Player _player;
        private Monster _currentMonster;

        public AdventureTime()
        {
            InitializeComponent();
            
            _player = new Player(10, 10, 20, 0);
            MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
            _player.Inventory.Add(new InventoryItem(
                World.ItemByID(World.ITEM_ID_RUSTY_SWORD), 1));

            lblHitPoints.Text = _player.CurrentHitPoints.ToString();
            lblGold.Text = _player.Gold.ToString();
            lblExperience.Text = _player.ExperiencePoints.ToString();
            lblLevel.Text = _player.Level.ToString();
        }
        //Armor labels
        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void btnNorth_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToNorth);
        }

        private void btnWest_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToWest);
        }

        private void btnEast_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToEast);
        }

        private void btnSouth_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToSouth);
        }

        private void btnUseWeapon_Click(object sender, EventArgs e)
        {
            // Get currently selected weapon from the cboWeapons Combobox
            Weapon currentWeapon = (Weapon)cboWeapons.SelectedItem;

            // Determine amount of daamge to do to monster
            int damageToMonster = RNG.NumberBetween(currentWeapon.MinimumDamage,
                currentWeapon.MaximumDamage);

            //Apply damage to monsters currentHP
            _currentMonster.CurrentHitPoints -= damageToMonster;

            //display msg
            rtbMessages.Text += "You've dealt " + damageToMonster.ToString() + "damage to "
                + _currentMonster.Name + "." + Environment.NewLine;
            ScrollToBottomOfMessages();

            //Check if monster is dead
            if(_currentMonster.CurrentHitPoints <= 0)
            {
                //monster is dead
                rtbMessages.Text += Environment.NewLine;
                rtbMessages.Text += _currentMonster.Name + " has been defeated!"
                    + Environment.NewLine;

                //Give player xp for killing monster
                _player.ExperiencePoints += _currentMonster.RewardExperiencePoints;
                rtbMessages.Text += _currentMonster.RewardExperiencePoints.ToString() +
                    " experience points received." + Environment.NewLine;

                //Give player gold for killing monster
                _player.Gold += _currentMonster.RewardGold;
                rtbMessages.Text += _currentMonster.RewardGold.ToString() +
                    " gold found!" + Environment.NewLine;

                ScrollToBottomOfMessages();

                //Get random loot items from monster
                List<InventoryItem> lootedItems = new List<InventoryItem>();

                //Add items to the lootedItems list, comapring random number to the drop %
                foreach(LootItem lootItem in _currentMonster.LootTable)
                {
                    if(RNG.NumberBetween(1,100) <= lootItem.DropPercentage)
                    {
                        lootedItems.Add(new InventoryItem(lootItem.Details, 1));
                    }
                }

                //if no items were randomly selected, add the default loot item(s).
                if(lootedItems.Count == 0)
                {
                    foreach(LootItem lootItem in _currentMonster.LootTable)
                    {
                        if (lootItem.IsDefaultItem)
                        {
                            lootedItems.Add(new InventoryItem(lootItem.Details, 1));
                        }
                    }
                }

                //Add the looted items to the player's inventory
                foreach(InventoryItem inventoryItem in lootedItems)
                {
                    _player.AddItemToInventory(inventoryItem.Details);

                    if(inventoryItem.Quantity == 1)
                    {
                        rtbMessages.Text += "You found " +
                            inventoryItem.Quantity.ToString() + " " +
                                inventoryItem.Details.Name + Environment.NewLine;
                    }
                    else
                    {
                        rtbMessages.Text += "You found " +
                            inventoryItem.Quantity.ToString() + " " +
                                inventoryItem.Details.NamePlural + Environment.NewLine;
                    }
                }
                ScrollToBottomOfMessages();

                UpdatePlayerStats();
                UpdateInventoryListInUI();
                UpdateWeaponListinUI();
                UpdatePotionListInUI();

                //Add blank line to msg box for appearance
                rtbMessages.Text += Environment.NewLine;

                //Move player to current location (heal player and create a new monster to fight)
                MoveTo(_player.CurrentLocation);
            }

            else
            {
                MonsterAttack();
            }
        }

        private void btnUsePotion_Click(object sender, EventArgs e)
        {
            //Get the currently selected potion from combobox
            HealingPotion potion = (HealingPotion)cboPotions.SelectedItem;

            //Add healing amount to player's current HP
            _player.CurrentHitPoints = (_player.CurrentHitPoints + potion.AmountToHeal);

            // Current HP cannot exceed max HP
            if(_player.CurrentHitPoints > _player.MaximumHitPoints)
            {
                _player.CurrentHitPoints = _player.MaximumHitPoints;
            }

            //Remove the potion from the player's inventory
            foreach(InventoryItem ii in _player.Inventory)
            {
                if(ii.Details.ID == potion.ID)
                {
                    ii.Quantity--;
                    break;
                }
            }

            //Display message
            rtbMessages.Text += "You drink a " + potion.Name + Environment.NewLine;

            //Monster gets their turn to attack
            MonsterAttack();
        }

        private void MoveTo(Location newLocation)
        {
            //Does location have required items
            if(!_player.HasRequiredItemToEnterThisLocation(newLocation))
            {
                rtbMessages.Text += "You need a " +
                    newLocation.ItemRequiredToEnter.Name +
                    " to enter here." + Environment.NewLine;
                ScrollToBottomOfMessages();
                return;
            }

            //Update player current location
            _player.CurrentLocation = newLocation;

            //show/hide available movement buttons
            btnNorth.Visible = (newLocation.LocationToNorth != null);
            btnEast.Visible = (newLocation.LocationToEast != null);
            btnSouth.Visible = (newLocation.LocationToSouth != null);
            btnWest.Visible = (newLocation.LocationToWest != null);

            //Display current location name and description
            rtbLocation.Text = newLocation.Name + Environment.NewLine;
            rtbLocation.Text += newLocation.Description + Environment.NewLine;

            // Heal the player
            _player.CurrentHitPoints = _player.MaximumHitPoints;

            // Update Hit Points in UI
            lblHitPoints.Text = _player.CurrentHitPoints.ToString();

            //Does Location have a quest?
            if(newLocation.QuestAvailableHere != null)
            {
                // see if player already has quest and if they've completed it
                bool playerAlreadyHasQuest =
                    _player.HasThisQuest(newLocation.QuestAvailableHere);
                bool playerAlreadyCompletedQuest =
                    _player.CompletedThisQuest(newLocation.QuestAvailableHere);

                //See if player already has the quest
                if(playerAlreadyHasQuest)
                {
                    //if player hasn't completed quest yet
                    if(!playerAlreadyCompletedQuest)
                    {
                        //See if player has all the items needed to complete the quest
                        bool playerHasAllItemsToCompleteQuest =
                            _player.HasAllQuestCompletionItems(newLocation.QuestAvailableHere);

                        //The player has all items required to complete the quest
                        if(playerHasAllItemsToCompleteQuest)
                        {
                            // display message
                            rtbMessages.Text += Environment.NewLine;
                            rtbMessages.Text += "You complete the " +
                                newLocation.QuestAvailableHere.Name +
                                " quest." + Environment.NewLine;

                            //remove quest items from inventory
                            _player.RemoveQuestCompletionItems(newLocation.QuestAvailableHere);

                            //Give Quest Rewards
                            rtbMessages.Text += "You receive: " + Environment.NewLine;
                            rtbMessages.Text +=
                                newLocation.QuestAvailableHere.RewardExperiencePoints.ToString()
                                + " experience points" + Environment.NewLine;
                            rtbMessages.Text +=
                                newLocation.QuestAvailableHere.RewardGold.ToString()
                                + "gold" + Environment.NewLine;
                            rtbMessages.Text +=
                                newLocation.QuestAvailableHere.RewardItem.Name +
                                    Environment.NewLine;
                            rtbMessages.Text += Environment.NewLine;

                            _player.ExperiencePoints +=
                                newLocation.QuestAvailableHere.RewardExperiencePoints;
                            _player.Gold += newLocation.QuestAvailableHere.RewardGold;

                            //Add reward item to player inventory
                            _player.AddItemToInventory(newLocation.QuestAvailableHere.RewardItem);

                            //Mark Quest as Completed
                            _player.MarkQuestCompleted(newLocation.QuestAvailableHere);

                            ScrollToBottomOfMessages();

                        }
                    }
                }
                else
                {
                    //The player does not already have the quest
                    //display messages
                    rtbMessages.Text += "You receive the " +
                        newLocation.QuestAvailableHere.Name +
                        " quest." + Environment.NewLine;
                    rtbMessages.Text += newLocation.QuestAvailableHere.Description +
                        Environment.NewLine;
                    rtbMessages.Text += "To complete it, return with:" +
                        Environment.NewLine;
                    foreach(QuestCompletionItem qci in 
                        newLocation.QuestAvailableHere.QuestCompletionItems)
                    {
                        if(qci.Quantity ==1)
                        {
                            rtbMessages.Text += qci.Quantity.ToString() + " " +
                                qci.Details.Name + Environment.NewLine;
                        }
                        else
                        {
                            rtbMessages.Text += qci.Quantity.ToString() + " " +
                                qci.Details.NamePlural + Environment.NewLine;
                        }
                    }
                    rtbMessages.Text += Environment.NewLine;
                    ScrollToBottomOfMessages();
                    //Add quest to player quest list
                    _player.Quests.Add(new PlayerQuest(newLocation.QuestAvailableHere));
                }
            }

            //Does Location have a Monster?
            if(newLocation.MonsterLivingHere !=  null)
            {
                rtbMessages.Text += "A " + newLocation.MonsterLivingHere.Name + "appeared!" +
                    Environment.NewLine;
                ScrollToBottomOfMessages();

                //Make a new monster, using values from the standard monster in the World.Monster list
                Monster standardMonster = World.MonsterByID(
                    newLocation.MonsterLivingHere.ID);

                _currentMonster = new Monster(standardMonster.ID, standardMonster.Name,
                    standardMonster.MaximumDamage, standardMonster.RewardExperiencePoints,
                    standardMonster.RewardGold, standardMonster.CurrentHitPoints,
                    standardMonster.MaximumHitPoints);

                foreach(LootItem lootItem in standardMonster.LootTable)
                {
                    _currentMonster.LootTable.Add(lootItem);
                }

                cboWeapons.Visible = true;
                cboPotions.Visible = true;
                btnUseWeapon.Visible = true;
                btnUsePotion.Visible = true;
            }
            else
            {
                _currentMonster = null;

                cboWeapons.Visible = false;
                cboPotions.Visible = false;
                btnUsePotion.Visible = false;
                btnUseWeapon.Visible = false;
            }

            // Refresh player's inventory list
            UpdateInventoryListInUI();

            //Refresh player quest list
            UpdateQuestListinUI();

            //Refresh player's weapons combobox
            UpdateWeaponListinUI();

            // Refresh player's potions combobox
            UpdatePotionListInUI();

        }

        //update inventory list in UI
        private void UpdateInventoryListInUI()
        {
            dgvInventory.RowHeadersVisible = false;

            dgvInventory.ColumnCount = 2;
            dgvInventory.Columns[0].Name = "Name";
            dgvInventory.Columns[0].Width = 197;
            dgvInventory.Columns[1].Name = "Quantity";

            dgvInventory.Rows.Clear();

            foreach (InventoryItem inventoryItem in _player.Inventory)
            {
                if (inventoryItem.Quantity > 0)
                {
                    dgvInventory.Rows.Add(new[]
                    {
                            inventoryItem.Details.Name,
                            inventoryItem.Quantity.ToString()
                        });
                }
            }
        }

        //Update quest list in UI
        private void UpdateQuestListinUI()
        {
            dgvQuests.RowHeadersVisible = false;

            dgvQuests.ColumnCount = 2;
            dgvQuests.Columns[0].Name = "Name";
            dgvQuests.Columns[0].Width = 197;
            dgvQuests.Columns[1].Name = "Done?";

            dgvQuests.Rows.Clear();

            foreach (PlayerQuest playerQuest in _player.Quests)
            {
                dgvQuests.Rows.Add(new[]
                {
                        playerQuest.Details.Name,
                        playerQuest.IsCompleted.ToString()
                    });
            }
        }

        //Update Weapon List in UI
        private void UpdateWeaponListinUI()
        {
            List<Weapon> weapons = new List<Weapon>();

            foreach (InventoryItem inventoryItem in _player.Inventory)
            {
                if (inventoryItem.Details is Weapon)
                {
                    if (inventoryItem.Quantity > 0)
                    {
                        weapons.Add((Weapon)inventoryItem.Details);
                    }
                }
            }

            if (weapons.Count == 0)
            {
                //player doesn't have any weapons, hide weapon combo box and "Use" button
                cboWeapons.Visible = false;
                btnUseWeapon.Visible = false;
            }
            else
            {
                cboWeapons.DataSource = weapons;
                cboWeapons.DisplayMember = "Name";
                cboWeapons.ValueMember = "ID";

                cboWeapons.SelectedIndex = 0;
            }
        }

        //Update Potion List in UI
        private void UpdatePotionListInUI()
        {
            List<HealingPotion> healingPotions = new List<HealingPotion>();

            foreach (InventoryItem inventoryItem in _player.Inventory)
            {
                if (inventoryItem.Details is HealingPotion)
                {
                    if (inventoryItem.Quantity > 0)
                    {
                        healingPotions.Add((HealingPotion)inventoryItem.Details);
                    }
                }
            }

            if (healingPotions.Count == 0)
            {
                // Player doesn't have any potions, so hide the potion combobox and
                // the "use" button
                cboPotions.Visible = false;
                btnUsePotion.Visible = false;
            }
            else
            {
                cboPotions.DataSource = healingPotions;
                cboPotions.DisplayMember = "Name";
                cboPotions.ValueMember = "ID";

                cboPotions.SelectedIndex = 0;
            }
        }

        //Monster Attack
        private void MonsterAttack()
        {
            //Determine amount of damage the monster does to the player
            int damageToPlayer =
                RNG.NumberBetween(0, _currentMonster.MaximumDamage);

            //display message
            rtbMessages.Text += _currentMonster.Name + " deals " + damageToPlayer.ToString()
                + " damage!" + Environment.NewLine;
            ScrollToBottomOfMessages();

            //Subtract Damage from player
            _player.CurrentHitPoints -= damageToPlayer;

            //Refresh player data in UI
            lblHitPoints.Text = _player.CurrentHitPoints.ToString();

            if (_player.CurrentHitPoints <= 0)
            {
                //display message
                rtbLocation.Text += "The " + _currentMonster.Name + " killed you." +
                    Environment.NewLine;
                ScrollToBottomOfMessages();

                //Move player to "Home"
                MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
            }

            //refresh player data in UI
            UpdatePlayerStats();
            UpdateInventoryListInUI();
            UpdatePotionListInUI();
        }

        //Update Player info and inventory controls
        private void UpdatePlayerStats()
        {
            lblHitPoints.Text = _player.CurrentHitPoints.ToString();
            lblGold.Text = _player.Gold.ToString();
            lblExperience.Text = _player.ExperiencePoints.ToString();
            lblLevel.Text = _player.Level.ToString();
        }

        //Scroll to bottom RichTextBox
        private void ScrollToBottomOfMessages()
        {
            rtbMessages.SelectionStart = rtbMessages.Text.Length;
            rtbMessages.ScrollToCaret();
        }
    }
}

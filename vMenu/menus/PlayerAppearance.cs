using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuAPI;
using Newtonsoft.Json;
using CitizenFX.Core;
using static CitizenFX.Core.UI.Screen;
using static CitizenFX.Core.Native.API;
using static vMenuClient.CommonFunctions;
using static vMenuShared.PermissionsManager;

namespace vMenuClient
{
    public class PlayerAppearance
    {
        private Menu menu;

        private Menu pedCustomizationMenu;
        private Menu savedPedsMenu;
        private Menu spawnPedsMenu;
        private Menu addonPedsMenu;
        private Menu mainPedsMenu = new Menu("Main Peds", "Spawn A Ped");
        private Menu animalsPedsMenu = new Menu("Animals", "Spawn A Ped");
        private Menu malePedsMenu = new Menu("Male Peds", "Spawn A Ped");
        private Menu femalePedsMenu = new Menu("Female Peds", "Spawn A Ped");
        private Menu otherPedsMenu = new Menu("Other Peds", "Spawn A Ped");

        public static Dictionary<string, uint> AddonPeds;

        public static int ClothingAnimationType { get; set; } = UserDefaults.PAClothingAnimationType;

        private Dictionary<MenuListItem, int> drawablesMenuListItems = new Dictionary<MenuListItem, int>();
        private Dictionary<MenuListItem, int> propsMenuListItems = new Dictionary<MenuListItem, int>();

        #region create the menu
        /// <summary>
        /// Creates the menu(s).
        /// </summary>
        private void CreateMenu()
        {
            // Create the menu.
            menu = new Menu(Game.Player.Name, "Player Appearance");
            savedPedsMenu = new Menu(Game.Player.Name, "Saved Peds");
            pedCustomizationMenu = new Menu(Game.Player.Name, "Customize Saved Ped");
            spawnPedsMenu = new Menu(Game.Player.Name, "Spawn Ped");
            addonPedsMenu = new Menu(Game.Player.Name, "Addon Peds");


            // Add the (submenus) to the menu pool.
            MenuController.AddSubmenu(menu, pedCustomizationMenu);
            MenuController.AddSubmenu(menu, savedPedsMenu);
            MenuController.AddSubmenu(menu, spawnPedsMenu);
            MenuController.AddSubmenu(spawnPedsMenu, addonPedsMenu);
            MenuController.AddSubmenu(spawnPedsMenu, mainPedsMenu);
            MenuController.AddSubmenu(spawnPedsMenu, animalsPedsMenu);
            MenuController.AddSubmenu(spawnPedsMenu, malePedsMenu);
            MenuController.AddSubmenu(spawnPedsMenu, femalePedsMenu);
            MenuController.AddSubmenu(spawnPedsMenu, otherPedsMenu);

            // Create the menu items.
            MenuItem pedCustomization = new MenuItem("Ped Customization", "Modify your ped's appearance.") { Label = "→→→" };
            MenuItem saveCurrentPed = new MenuItem("Save Ped", "Save your current ped. Note for the MP Male/Female peds this won't save most of their customization, just because that's impossible. Create those characters in the MP Character creator instead.");
            MenuItem savedPedsBtn = new MenuItem("Saved Peds", "Edit, rename, clone, spawn or delete saved peds.") { Label = "→→→" };
            MenuItem spawnPedsBtn = new MenuItem("Spawn Peds", "Change ped model by selecting one from the list or by selecting an addon ped from the list.") { Label = "→→→" };


            MenuItem spawnByNameBtn = new MenuItem("Spawn By Name", "Spawn a ped by entering it's name manually.");
            MenuItem addonPedsBtn = new MenuItem("Addon Peds", "Spawn a ped from the addon peds list.") { Label = "→→→" };
            MenuItem mainPedsBtn = new MenuItem("Main Peds", "Select a new ped from the main player-peds list.") { Label = "→→→" };
            MenuItem animalPedsBtn = new MenuItem("Animals", "Become an animal. ~r~Note this may crash your own or other players' game if you die as an animal, godmode can NOT prevent this.") { Label = "→→→" };
            MenuItem malePedsBtn = new MenuItem("Male Peds", "Select a male ped.") { Label = "→→→" };
            MenuItem femalePedsBtn = new MenuItem("Female Peds", "Select a female ped.") { Label = "→→→" };
            MenuItem otherPedsBtn = new MenuItem("Other Peds", "Select a ped.") { Label = "→→→" };

            List<string> walkstyles = new List<string>() { "Normal", "Injured", "Tough Guy", "Femme", "Gangster", "Posh", "Sexy", "Business", "Drunk", "Hipster" };
            MenuListItem walkingStyle = new MenuListItem("Walking Style", walkstyles, 0, "Change the walking style of your current ped. " +
                "You need to re-apply this each time you change player model or load a saved ped.");

            List<string> clothingGlowAnimations = new List<string>() { "On", "Off", "Fade", "Flash" };
            MenuListItem clothingGlowType = new MenuListItem("Illuminated Clothing Style", clothingGlowAnimations, ClothingAnimationType, "Set the style of the animation used on your player's illuminated clothing items.");

            // Add items to the menu.
            menu.AddMenuItem(pedCustomization);
            menu.AddMenuItem(saveCurrentPed);
            menu.AddMenuItem(savedPedsBtn);
            menu.AddMenuItem(spawnPedsBtn);

            menu.AddMenuItem(walkingStyle);
            menu.AddMenuItem(clothingGlowType);

            if (IsAllowed(Permission.PACustomize))
            {
                MenuController.BindMenuItem(menu, pedCustomizationMenu, pedCustomization);
            }
            else
            {
                menu.RemoveMenuItem(pedCustomization);
            }

            // always allowed
            MenuController.BindMenuItem(menu, savedPedsMenu, savedPedsBtn);
            MenuController.BindMenuItem(menu, spawnPedsMenu, spawnPedsBtn);

            Menu selectedSavedPedMenu = new Menu("Saved Ped", "renameme");
            MenuController.AddSubmenu(savedPedsMenu, selectedSavedPedMenu);
            MenuItem spawnSavedPed = new MenuItem("Spawn Saved Ped", "Spawn this saved ped.");
            MenuItem cloneSavedPed = new MenuItem("Clone Saved Ped", "Clone this saved ped.");
            MenuItem renameSavedPed = new MenuItem("Rename Saved Ped", "Rename this saved ped.") { LeftIcon = MenuItem.Icon.WARNING };
            MenuItem replaceSavedPed = new MenuItem("~r~Replace Saved Ped", "Repalce this saved ped with your current ped. Note this can not be undone!") { LeftIcon = MenuItem.Icon.WARNING };
            MenuItem deleteSavedPed = new MenuItem("~r~Delete Saved Ped", "Delete this saved ped. Note this can not be undone!") { LeftIcon = MenuItem.Icon.WARNING };

            if (!IsAllowed(Permission.PASpawnSaved))
            {
                spawnSavedPed.Enabled = false;
                spawnSavedPed.RightIcon = MenuItem.Icon.LOCK;
                spawnSavedPed.Description = "You are not allowed to spawn saved peds.";
            }

            selectedSavedPedMenu.AddMenuItem(spawnSavedPed);
            selectedSavedPedMenu.AddMenuItem(cloneSavedPed);
            selectedSavedPedMenu.AddMenuItem(renameSavedPed);
            selectedSavedPedMenu.AddMenuItem(replaceSavedPed);
            selectedSavedPedMenu.AddMenuItem(deleteSavedPed);

            KeyValuePair<string, PedInfo> savedPed = new KeyValuePair<string, PedInfo>();

            selectedSavedPedMenu.OnItemSelect += async (sender, item, index) =>
            {
                if (item == spawnSavedPed)
                {
                    await SetPlayerSkin(savedPed.Value.model, savedPed.Value, true);
                }
                else if (item == cloneSavedPed)
                {
                    string name = await GetUserInput($"Enter a clone name ({savedPed.Key.Substring(4)})", savedPed.Key.Substring(4), 30);
                    if (string.IsNullOrEmpty(name))
                    {
                        Notify.Error(CommonErrors.InvalidSaveName);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(GetResourceKvpString($"ped_{name}")))
                        {
                            Notify.Error(CommonErrors.SaveNameAlreadyExists);
                        }
                        else
                        {
                            if (StorageManager.SavePedInfo("ped_" + name, savedPed.Value, false))
                            {
                                Notify.Success($"Saved Ped has successfully been cloned. Clone name: ~g~<C>{name}</C>~s~.");
                            }
                            else
                            {
                                Notify.Error(CommonErrors.UnknownError, placeholderValue: " Could not save your cloned ped. Don't worry, your original ped is unharmed.");
                            }
                        }
                    }
                }
                else if (item == renameSavedPed)
                {
                    string name = await GetUserInput($"Enter a new name for: {savedPed.Key.Substring(4)}", savedPed.Key.Substring(4), 30);
                    if (string.IsNullOrEmpty(name))
                    {
                        Notify.Error(CommonErrors.InvalidSaveName);
                    }
                    else
                    {
                        if ("ped_" + name == savedPed.Key)
                        {
                            Notify.Error("You need to choose a different name, you can't use the same name as your existing ped.");
                            return;
                        }
                        if (StorageManager.SavePedInfo("ped_" + name, savedPed.Value, false))
                        {
                            Notify.Success($"Saved Ped has successfully been renamed. New ped name: ~g~<C>{name}</C>~s~.");
                            DeleteResourceKvp(savedPed.Key);
                            selectedSavedPedMenu.MenuSubtitle = name;
                            savedPed = new KeyValuePair<string, PedInfo>("ped_" + name, savedPed.Value);
                        }
                        else
                        {
                            Notify.Error(CommonErrors.SaveNameAlreadyExists);
                        }
                    }
                }
                else if (item == replaceSavedPed)
                {
                    if (item.Label == "Are you sure?")
                    {
                        item.Label = "";
                        bool success = await SavePed(savedPed.Key.Substring(4), overrideExistingPed: true);
                        if (!success)
                        {
                            Notify.Error(CommonErrors.UnknownError, placeholderValue: " Could not save your replaced ped. Don't worry, your original ped is unharmed.");
                        }
                        else
                        {
                            Notify.Success("Your saved ped has successfully been replaced.");
                            savedPed = new KeyValuePair<string, PedInfo>(savedPed.Key, StorageManager.GetSavedPedInfo(savedPed.Key));
                        }
                    }
                    else
                    {
                        item.Label = "Are you sure?";
                    }
                }
                else if (item == deleteSavedPed)
                {
                    if (item.Label == "Are you sure?")
                    {
                        DeleteResourceKvp(savedPed.Key);
                        Notify.Success("Your saved ped has been deleted.");
                        selectedSavedPedMenu.GoBack();
                    }
                    else
                    {
                        item.Label = "Are you sure?";
                    }
                }
            };

            void ResetSavedPedsMenu(bool refreshIndex)
            {
                foreach (var item in selectedSavedPedMenu.GetMenuItems())
                {
                    item.Label = "";
                }
                if (refreshIndex)
                {
                    selectedSavedPedMenu.RefreshIndex();
                }
            }

            selectedSavedPedMenu.OnIndexChange += (menu, newItem, oldItem, oldIndex, newIndex) => ResetSavedPedsMenu(false);
            selectedSavedPedMenu.OnMenuOpen += (menu) => ResetSavedPedsMenu(true);


            void UpdateSavedPedsMenu()
            {
                int size = savedPedsMenu.Size;

                Dictionary<string, PedInfo> savedPeds = StorageManager.GetSavedPeds();

                foreach (var ped in savedPeds)
                {
                    if (size < 1 || !savedPedsMenu.GetMenuItems().Any(e => ped.Key == e.ItemData.Key))
                    {
                        MenuItem btn = new MenuItem(ped.Key.Substring(4), "Click to manage this saved ped.") { Label = "→→→", ItemData = ped };
                        savedPedsMenu.AddMenuItem(btn);
                        MenuController.BindMenuItem(savedPedsMenu, selectedSavedPedMenu, btn);
                    }
                }

                if (savedPedsMenu.Size > 0)
                {
                    foreach (var d in savedPedsMenu.GetMenuItems())
                    {
                        if (!savedPeds.ContainsKey(d.ItemData.Key))
                        {
                            savedPedsMenu.RemoveMenuItem(d);
                        }
                        else
                        {
                            // Make sure the saved ped data is actually correct and up to date for this item.
                            var p = savedPeds.First(e => e.Key == d.ItemData.Key);
                            if (!string.IsNullOrEmpty(p.Key))
                            {
                                d.ItemData = p;
                            }
                        }
                    }
                }

                if (savedPedsMenu.Size > 0)
                {
                    savedPedsMenu.SortMenuItems((a, b) => a.Text.ToLower().CompareTo(b.Text.ToLower()));
                }

                // refresh index only if the size of the menu has changed.
                if (size != savedPedsMenu.Size)
                {
                    savedPedsMenu.RefreshIndex();
                }
            }

            savedPedsMenu.OnMenuOpen += (_) =>
            {
                UpdateSavedPedsMenu();
            };

            savedPedsMenu.OnItemSelect += (_, item, __) =>
            {
                savedPed = item.ItemData;
                selectedSavedPedMenu.MenuSubtitle = item.Text;
            };

            if (AddonPeds != null && AddonPeds.Count > 0 && IsAllowed(Permission.PAAddonPeds))
            {
                spawnPedsMenu.AddMenuItem(addonPedsBtn);
                MenuController.BindMenuItem(spawnPedsMenu, addonPedsMenu, addonPedsBtn);

                var addons = AddonPeds.ToList();

                addons.Sort((a, b) => a.Key.ToLower().CompareTo(b.Key.ToLower()));

                foreach (var ped in addons)
                {
                    string name = GetLabelText(ped.Key);
                    if (string.IsNullOrEmpty(name) || name == "NULL")
                    {
                        name = ped.Key;
                    }

                    MenuItem pedBtn = new MenuItem(ped.Key, "Click to spawn this model.") { Label = $"({name})" };

                    if (!IsModelInCdimage(ped.Value) || !IsModelAPed(ped.Value))
                    {
                        pedBtn.Enabled = false;
                        pedBtn.LeftIcon = MenuItem.Icon.LOCK;
                        pedBtn.Description = "This ped is not (correctly) streamed. If you are the server owner, please ensure that the ped name and model are valid!";
                    }

                    addonPedsMenu.AddMenuItem(pedBtn);
                }

                addonPedsMenu.OnItemSelect += async (sender, item, index) =>
                {
                    await SetPlayerSkin((uint)GetHashKey(item.Text), new PedInfo() { version = -1 }, true);
                };
            }

            if (IsAllowed(Permission.PASpawnNew))
            {
                spawnPedsMenu.AddMenuItem(spawnByNameBtn);
                spawnPedsMenu.AddMenuItem(mainPedsBtn);
                spawnPedsMenu.AddMenuItem(animalPedsBtn);
                spawnPedsMenu.AddMenuItem(malePedsBtn);
                spawnPedsMenu.AddMenuItem(femalePedsBtn);
                spawnPedsMenu.AddMenuItem(otherPedsBtn);

                MenuController.BindMenuItem(spawnPedsMenu, mainPedsMenu, mainPedsBtn);
                if (vMenuShared.ConfigManager.GetSettingsBool(vMenuShared.ConfigManager.Setting.vmenu_enable_animals_spawn_menu))
                {
                    MenuController.BindMenuItem(spawnPedsMenu, animalsPedsMenu, animalPedsBtn);
                }
                else
                {
                    animalPedsBtn.Enabled = false;
                    animalPedsBtn.Description = "This is disabled by the server owner, probably for a good reason because animals quite often crash the game.";
                    animalPedsBtn.LeftIcon = MenuItem.Icon.LOCK;
                }

                MenuController.BindMenuItem(spawnPedsMenu, malePedsMenu, malePedsBtn);
                MenuController.BindMenuItem(spawnPedsMenu, femalePedsMenu, femalePedsBtn);
                MenuController.BindMenuItem(spawnPedsMenu, otherPedsMenu, otherPedsBtn);

                foreach (var animal in animalModels)
                {
                    MenuItem animalBtn = new MenuItem(animal.Key, "Click to spawn this animal.") { Label = $"({animal.Value})" };
                    animalsPedsMenu.AddMenuItem(animalBtn);
                }

                foreach (var ped in mainModels)
                {
                    MenuItem pedBtn = new MenuItem(ped.Key, "Click to spawn this ped.") { Label = $"({ped.Value})" };
                    mainPedsMenu.AddMenuItem(pedBtn);
                }

                foreach (var ped in maleModels)
                {
                    MenuItem pedBtn = new MenuItem(ped.Key, "Click to spawn this ped.") { Label = $"({ped.Value})" };
                    malePedsMenu.AddMenuItem(pedBtn);
                }

                foreach (var ped in femaleModels)
                {
                    MenuItem pedBtn = new MenuItem(ped.Key, "Click to spawn this ped.") { Label = $"({ped.Value})" };
                    femalePedsMenu.AddMenuItem(pedBtn);
                }

                foreach (var ped in otherPeds)
                {
                    MenuItem pedBtn = new MenuItem(ped.Key, "Click to spawn this ped.") { Label = $"({ped.Value})" };
                    otherPedsMenu.AddMenuItem(pedBtn);
                }

                async void FilterMenu(Menu m, Control c)
                {
                    string input = await GetUserInput("Filter by ped model name, leave this empty to reset the filter");
                    if (!string.IsNullOrEmpty(input))
                    {
                        m.FilterMenuItems((mb) => mb.Label.ToLower().Contains(input.ToLower()) || mb.Text.ToLower().Contains(input.ToLower()));
                        Subtitle.Custom("Filter applied.");
                    }
                    else
                    {
                        m.ResetFilter();
                        Subtitle.Custom("Filter cleared.");
                    }
                }

                void ResetMenuFilter(Menu m)
                {
                    m.ResetFilter();
                }

                otherPedsMenu.OnMenuClose += ResetMenuFilter;
                malePedsMenu.OnMenuClose += ResetMenuFilter;
                femalePedsMenu.OnMenuClose += ResetMenuFilter;

                otherPedsMenu.InstructionalButtons.Add(Control.Jump, "Filter List");
                otherPedsMenu.ButtonPressHandlers.Add(new Menu.ButtonPressHandler(Control.Jump, Menu.ControlPressCheckType.JUST_RELEASED, new Action<Menu, Control>(FilterMenu), true));

                malePedsMenu.InstructionalButtons.Add(Control.Jump, "Filter List");
                malePedsMenu.ButtonPressHandlers.Add(new Menu.ButtonPressHandler(Control.Jump, Menu.ControlPressCheckType.JUST_RELEASED, new Action<Menu, Control>(FilterMenu), true));

                femalePedsMenu.InstructionalButtons.Add(Control.Jump, "Filter List");
                femalePedsMenu.ButtonPressHandlers.Add(new Menu.ButtonPressHandler(Control.Jump, Menu.ControlPressCheckType.JUST_RELEASED, new Action<Menu, Control>(FilterMenu), true));


                async void SpawnPed(Menu m, MenuItem item, int index)
                {

                    uint model = (uint)GetHashKey(item.Text);
                    if (m == animalsPedsMenu && !Game.PlayerPed.IsInWater)
                    {
                        switch (item.Text)
                        {
                            case "a_c_dolphin":
                            case "a_c_fish":
                            case "a_c_humpback":
                            case "a_c_killerwhale":
                            case "a_c_sharkhammer":
                            case "a_c_sharktiger":
			    case "a_c_stingray":
                                Notify.Error("This animal can only be spawned when you are in water, otherwise you will die immediately.");
                                return;
                            default: break;
                        }
                    }

                    if (IsModelInCdimage(model))
                    {
                        // for animals we need to remove all weapons, this is because animals have their own weapons which you can't normally get and/or select in the weapon wheel.
                        // so we clear the weapons to force that specific weapon to be equipped.
                        if (m == animalsPedsMenu)
                        {
                            Game.PlayerPed.Weapons.RemoveAll();
                            await SetPlayerSkin(model, new PedInfo() { version = -1 }, false);
                            await Delay(1000);
                            SetPedComponentVariation(Game.PlayerPed.Handle, 0, 0, 0, 0);
                            await Delay(1000);
                            SetPedComponentVariation(Game.PlayerPed.Handle, 0, 0, 1, 0);
                            await Delay(1000);
                            SetPedDefaultComponentVariation(Game.PlayerPed.Handle);
                        }
                        else
                        {
                            await SetPlayerSkin(model, new PedInfo() { version = -1 }, true);
                        }
                    }
                    else
                    {
                        Notify.Error(CommonErrors.InvalidModel);
                    }
                }

                mainPedsMenu.OnItemSelect += SpawnPed;
                malePedsMenu.OnItemSelect += SpawnPed;
                femalePedsMenu.OnItemSelect += SpawnPed;
                animalsPedsMenu.OnItemSelect += SpawnPed;
                otherPedsMenu.OnItemSelect += SpawnPed;

                spawnPedsMenu.OnItemSelect += async (sender, item, index) =>
                {
                    if (item == spawnByNameBtn)
                    {
                        string model = await GetUserInput("Ped Model Name", 30);
                        if (!string.IsNullOrEmpty(model))
                        {
                            await SetPlayerSkin(model, new PedInfo() { version = -1 }, true);
                        }
                        else
                        {
                            Notify.Error(CommonErrors.InvalidInput);
                        }
                    }
                };
            }


            // Handle list selections.
            menu.OnListItemSelect += (sender, item, listIndex, itemIndex) =>
            {
                if (item == walkingStyle)
                {
                    //if (MainMenu.DebugMode) Subtitle.Custom("Ped is: " + IsPedMale(Game.PlayerPed.Handle));
                    SetWalkingStyle(walkstyles[listIndex].ToString());
                }
                if (item == clothingGlowType)
                {
                    ClothingAnimationType = item.ListIndex;
                }
            };

            // Handle button presses.
            menu.OnItemSelect += async (sender, item, index) =>
            {
                if (item == pedCustomization)
                {
                    RefreshCustomizationMenu();
                }
                else if (item == saveCurrentPed)
                {
                    if (await SavePed())
                    {
                        Notify.Success("Successfully saved your new ped.");
                    }
                    else
                    {
                        Notify.Error("Could not save your current ped, does that save name already exist?");
                    }
                }
            };


            #region ped drawable list changes
            // Manage list changes.
            pedCustomizationMenu.OnListIndexChange += (sender, item, oldListIndex, newListIndex, itemIndex) =>
            {
                if (drawablesMenuListItems.ContainsKey(item))
                {
                    int drawableID = drawablesMenuListItems[item];
                    SetPedComponentVariation(Game.PlayerPed.Handle, drawableID, newListIndex, 0, 0);
                }
                else if (propsMenuListItems.ContainsKey(item))
                {
                    int propID = propsMenuListItems[item];
                    if (newListIndex == 0)
                    {
                        SetPedPropIndex(Game.PlayerPed.Handle, propID, newListIndex - 1, 0, false);
                        ClearPedProp(Game.PlayerPed.Handle, propID);
                    }
                    else
                    {
                        SetPedPropIndex(Game.PlayerPed.Handle, propID, newListIndex - 1, 0, true);
                    }
                    if (propID == 0)
                    {
                        int component = GetPedPropIndex(Game.PlayerPed.Handle, 0);      // helmet index
                        int texture = GetPedPropTextureIndex(Game.PlayerPed.Handle, 0); // texture
                        int compHash = GetHashNameForProp(Game.PlayerPed.Handle, 0, component, texture); // prop combination hash
                        if (N_0xd40aac51e8e4c663((uint)compHash) > 0) // helmet has visor. 
                        {
                            if (!IsHelpMessageBeingDisplayed())
                            {
                                BeginTextCommandDisplayHelp("TWOSTRINGS");
                                AddTextComponentSubstringPlayerName("Hold ~INPUT_SWITCH_VISOR~ to flip your helmet visor open or closed");
                                AddTextComponentSubstringPlayerName("when on foot or on a motorcycle and when vMenu is closed.");
                                EndTextCommandDisplayHelp(0, false, true, 6000);
                            }
                        }
                    }

                }
            };

            // Manage list selections.
            pedCustomizationMenu.OnListItemSelect += (sender, item, listIndex, itemIndex) =>
            {
                if (drawablesMenuListItems.ContainsKey(item)) // drawable
                {
                    int currentDrawableID = drawablesMenuListItems[item];
                    int currentTextureIndex = GetPedTextureVariation(Game.PlayerPed.Handle, currentDrawableID);
                    int maxDrawableTextures = GetNumberOfPedTextureVariations(Game.PlayerPed.Handle, currentDrawableID, listIndex) - 1;

                    if (currentTextureIndex == -1)
                        currentTextureIndex = 0;

                    int newTexture = currentTextureIndex < maxDrawableTextures ? currentTextureIndex + 1 : 0;

                    SetPedComponentVariation(Game.PlayerPed.Handle, currentDrawableID, listIndex, newTexture, 0);
                }
                else if (propsMenuListItems.ContainsKey(item)) // prop
                {
                    int currentPropIndex = propsMenuListItems[item];
                    int currentPropVariationIndex = GetPedPropIndex(Game.PlayerPed.Handle, currentPropIndex);
                    int currentPropTextureVariation = GetPedPropTextureIndex(Game.PlayerPed.Handle, currentPropIndex);
                    int maxPropTextureVariations = GetNumberOfPedPropTextureVariations(Game.PlayerPed.Handle, currentPropIndex, currentPropVariationIndex) - 1;

                    int newPropTextureVariationIndex = currentPropTextureVariation < maxPropTextureVariations ? currentPropTextureVariation + 1 : 0;
                    SetPedPropIndex(Game.PlayerPed.Handle, currentPropIndex, currentPropVariationIndex, newPropTextureVariationIndex, true);
                }
            };
            #endregion

        }


        #endregion

        #region get the menu
        /// <summary>
        /// Create the menu if it doesn't exist, and then returns it.
        /// </summary>
        /// <returns>The Menu</returns>
        public Menu GetMenu()
        {
            if (menu == null)
            {
                CreateMenu();
            }
            return menu;
        }
        #endregion

        #region Ped Customization Menu
        ///// <summary>
        ///// Refresh/create the ped customization menu.
        ///// </summary>
        private void RefreshCustomizationMenu()
        {
            drawablesMenuListItems.Clear();
            propsMenuListItems.Clear();
            pedCustomizationMenu.ClearMenuItems();

            #region Ped Drawables
            for (int drawable = 0; drawable < 12; drawable++)
            {
                int currentDrawable = GetPedDrawableVariation(Game.PlayerPed.Handle, drawable);
                int maxVariations = GetNumberOfPedDrawableVariations(Game.PlayerPed.Handle, drawable);
                int maxTextures = GetNumberOfPedTextureVariations(Game.PlayerPed.Handle, drawable, currentDrawable);

                if (maxVariations > 0)
                {
                    List<string> drawableTexturesList = new List<string>();

                    for (int i = 0; i < maxVariations; i++)
                    {
                        drawableTexturesList.Add($"Drawable #{i + 1} (of {maxVariations})");
                    }

                    MenuListItem drawableTextures = new MenuListItem($"{textureNames[drawable]}", drawableTexturesList, currentDrawable, $"Use ← & → to select a ~o~{textureNames[drawable]} Variation~s~, press ~r~enter~s~ to cycle through the available textures.");
                    drawablesMenuListItems.Add(drawableTextures, drawable);
                    pedCustomizationMenu.AddMenuItem(drawableTextures);
                }
            }
            #endregion

            #region Ped Props
            for (int tmpProp = 0; tmpProp < 5; tmpProp++)
            {
                int realProp = tmpProp > 2 ? tmpProp + 3 : tmpProp;

                int currentProp = GetPedPropIndex(Game.PlayerPed.Handle, realProp);
                int maxPropVariations = GetNumberOfPedPropDrawableVariations(Game.PlayerPed.Handle, realProp);

                if (maxPropVariations > 0)
                {
                    List<string> propTexturesList = new List<string>();

                    propTexturesList.Add($"Prop #1 (of {maxPropVariations + 1})");
                    for (int i = 0; i < maxPropVariations; i++)
                    {
                        propTexturesList.Add($"Prop #{i + 2} (of {maxPropVariations + 1})");
                    }


                    MenuListItem propTextures = new MenuListItem($"{propNames[tmpProp]}", propTexturesList, currentProp + 1, $"Use ← & → to select a ~o~{propNames[tmpProp]} Variation~s~, press ~r~enter~s~ to cycle through the available textures.");
                    propsMenuListItems.Add(propTextures, realProp);
                    pedCustomizationMenu.AddMenuItem(propTextures);

                }
            }
            pedCustomizationMenu.RefreshIndex();
            #endregion
        }

        #region Textures & Props
        private readonly List<string> textureNames = new List<string>()
        {
            "Head",
            "Mask / Facial Hair",
            "Hair Style / Color",
            "Hands / Upper Body",
            "Legs / Pants",
            "Bags / Parachutes",
            "Shoes",
            "Neck / Scarfs",
            "Shirt / Accessory",
            "Body Armor / Accessory 2",
            "Badges / Logos",
            "Shirt Overlay / Jackets",
        };

        private readonly List<string> propNames = new List<string>()
        {
            "Hats / Helmets", // id 0
            "Glasses", // id 1
            "Misc", // id 2
            "Watches", // id 6
            "Bracelets", // id 7
        };
        #endregion
        #endregion


        #region saved peds menus
        ///// <summary>
        ///// Refresh the spawn saved peds menu.
        ///// </summary>
        //private void RefreshSpawnSavedPedMenu()
        //{
        //    spawnSavedPedMenu.ClearMenuItems();
        //    int findHandle = StartFindKvp("ped_");
        //    List<string> savesFound = new List<string>();
        //    var i = 0;
        //    while (true)
        //    {
        //        i++;
        //        var saveName = FindKvp(findHandle);
        //        if (saveName != null && saveName != "" && saveName != "NULL")
        //        {
        //            // It's already the new format, so add it.
        //            savesFound.Add(saveName);
        //        }
        //        else
        //        {
        //            break;
        //        }
        //    }

        //    var items = new List<string>();
        //    foreach (var savename in savesFound)
        //    {
        //        if (savename.Length > 4)
        //        {
        //            var title = savename.Substring(4);
        //            if (!items.Contains(title))
        //            {
        //                MenuItem savedPedBtn = new MenuItem(title, "Spawn this saved ped.");
        //                spawnSavedPedMenu.AddMenuItem(savedPedBtn);
        //                items.Add(title);
        //            }
        //        }
        //    }

        //    // Sort the menu items (case IN-sensitive) by name.
        //    spawnSavedPedMenu.SortMenuItems((pair1, pair2) => pair1.Text.ToString().ToLower().CompareTo(pair2.Text.ToString().ToLower()));

        //    spawnSavedPedMenu.RefreshIndex();
        //    //spawnSavedPedMenu.UpdateScaleform();
        //}

        ///// <summary>
        ///// Refresh the delete saved peds menu.
        ///// </summary>
        //private void RefreshDeleteSavedPedMenu()
        //{
        //    deleteSavedPedMenu.ClearMenuItems();
        //    int findHandle = StartFindKvp("ped_");
        //    List<string> savesFound = new List<string>();
        //    while (true)
        //    {
        //        var saveName = FindKvp(findHandle);
        //        if (saveName != null && saveName != "" && saveName != "NULL")
        //        {
        //            savesFound.Add(saveName);
        //        }
        //        else
        //        {
        //            break;
        //        }
        //    }
        //    foreach (var savename in savesFound)
        //    {
        //        MenuItem deleteSavedPed = new MenuItem(savename.Substring(4), "~r~Delete ~s~this saved ped, this action can ~r~NOT~s~ be undone!")
        //        {
        //            LeftIcon = MenuItem.Icon.WARNING
        //        };
        //        deleteSavedPedMenu.AddMenuItem(deleteSavedPed);
        //    }

        //    // Sort the menu items (case IN-sensitive) by name.
        //    deleteSavedPedMenu.SortMenuItems((pair1, pair2) => pair1.Text.ToString().ToLower().CompareTo(pair2.Text.ToString().ToLower()));

        //    deleteSavedPedMenu.OnItemSelect += (sender, item, idex) =>
        //    {
        //        var name = item.Text.ToString();
        //        StorageManager.DeleteSavedStorageItem("ped_" + name);
        //        Notify.Success("Saved ped deleted.");
        //        deleteSavedPedMenu.GoBack();
        //    };

        //    deleteSavedPedMenu.RefreshIndex();
        //    //deleteSavedPedMenu.UpdateScaleform();
        //}
        #endregion

        //private List<string> stuff = new List<string>()
        //    {
        //        "csb_abigail",
        //    "csb_anita",
        //    "csb_anton",
        //    "csb_ballasog",
        //    "csb_bride",
        //    "csb_burgerdrug",
        //    "csb_car3guy1",
        //    "csb_car3guy2",
        //    "csb_chef",
        //    "csb_chin_goon",
        //    "csb_cletus",
        //    "csb_cop",
        //    "csb_customer",
        //    "csb_denise_friend",
        //    "csb_fos_rep",
        //    "csb_groom",
        //    "csb_grove_str_dlr",
        //    "csb_g",
        //    "csb_hao",
        //    "csb_hugh",
        //    "csb_imran",
        //    "csb_janitor",
        //    "csb_maude",
        //    "csb_mweather",
        //    "csb_ortega",
        //    "csb_oscar",
        //    "csb_porndudes",
        //    "csb_prologuedriver",
        //    "csb_prolsec",
        //    "csb_ramp_gang",
        //    "csb_ramp_hic",
        //    "csb_ramp_hipster",
        //    "csb_ramp_marine",
        //    "csb_ramp_mex",
        //    "csb_reporter",
        //    "csb_roccopelosi",
        //    "csb_screen_writer",
        //    "csb_stripper_01",
        //    "csb_stripper_02",
        //    "csb_tonya",
        //    "csb_trafficwarden",
        //    "g_f_y_ballas_01",
        //    "g_f_y_families_01",
        //    "g_f_y_lost_01",
        //    "g_f_y_vagos_01",
        //    "g_m_m_armboss_01",
        //    "g_m_m_armgoon_01",
        //    "g_m_m_armlieut_01",
        //    "g_m_m_chemwork_01",
        //    "g_m_m_chiboss_01",
        //    "g_m_m_chicold_01",
        //    "g_m_m_chigoon_01",
        //    "g_m_m_chigoon_02",
        //    "g_m_m_korboss_01",
        //    "g_m_m_mexboss_01",
        //    "g_m_m_mexboss_02",
        //    "g_m_y_armgoon_02",
        //    "g_m_y_azteca_01",
        //    "g_m_y_ballaeast_01",
        //    "g_m_y_ballaorig_01",
        //    "g_m_y_ballasout_01",
        //    "g_m_y_famca_01",
        //    "g_m_y_famdnf_01",
        //    "g_m_y_famfor_01",
        //    "g_m_y_korean_01",
        //    "g_m_y_korean_02",
        //    "g_m_y_korlieut_01",
        //    "g_m_y_lost_01",
        //    "g_m_y_lost_02",
        //    "g_m_y_lost_03",
        //    "g_m_y_mexgang_01",
        //    "g_m_y_mexgoon_01",
        //    "g_m_y_mexgoon_02",
        //    "g_m_y_mexgoon_03",
        //    "g_m_y_pologoon_01",
        //    "g_m_y_pologoon_02",
        //    "g_m_y_salvaboss_01",
        //    "g_m_y_salvagoon_01",
        //    "g_m_y_salvagoon_02",
        //    "g_m_y_salvagoon_03",
        //    "g_m_y_strpunk_01",
        //    "g_m_y_strpunk_02",
        //    "hc_driver",
        //    "hc_gunman",
        //    "hc_hacker",
        //    "ig_abigail",
        //    "ig_amandatownley",
        //    "ig_andreas",
        //    "ig_ashley",
        //    "ig_ballasog",
        //    "ig_bankman",
        //    "ig_barry",
        //    "ig_bestmen",
        //    "ig_beverly",
        //    "ig_brad",
        //    "ig_bride",
        //    "ig_car3guy1",
        //    "ig_car3guy2",
        //    "ig_casey",
        //    "ig_chef",
        //    "ig_chengsr",
        //    "ig_chrisformage",
        //    "ig_claypain",
        //    "ig_clay",
        //    "ig_cletus",
        //    "ig_dale",
        //    "ig_davenorton",
        //    "ig_denise",
        //    "ig_devin",
        //    "ig_dom",
        //    "ig_dreyfuss",
        //    "ig_drfriedlander",
        //    "ig_fabien",
        //    "ig_fbisuit_01",
        //    "ig_floyd",
        //    "ig_groom",
        //    "ig_hao",
        //    "ig_hunter",
        //    "ig_janet",
        //    "ig_jay_norris",
        //    "ig_jewelass",
        //    "ig_jimmyboston",
        //    "ig_jimmydisanto",
        //    "ig_joeminuteman",
        //    "ig_johnnyklebitz",
        //    "ig_josef",
        //    "ig_josh",
        //    "ig_kerrymcintosh",
        //    "ig_lamardavis",
        //    "ig_lazlow",
        //    "ig_lestercrest",
        //    "ig_lifeinvad_01",
        //    "ig_lifeinvad_02",
        //    "ig_magenta",
        //    "ig_manuel",
        //    "ig_marnie",
        //    "ig_maryann",
        //    "ig_maude",
        //    "ig_michelle",
        //    "ig_milton",
        //    "ig_molly",
        //    "ig_mrk",
        //    "ig_mrsphillips",
        //    "ig_mrs_thornhill",
        //    "ig_natalia",
        //    "ig_nervousron",
        //    "ig_nigel",
        //    "ig_old_man1a",
        //    "ig_old_man2",
        //    "ig_omega",
        //    "ig_oneil",
        //    "ig_orleans",
        //    "ig_ortega",
        //    "ig_paper",
        //    "ig_patricia",
        //    "ig_priest",
        //    "ig_prolsec_02",
        //    "ig_ramp_gang",
        //    "ig_ramp_hic",
        //    "ig_ramp_hipster",
        //    "ig_ramp_mex",
        //    "ig_roccopelosi",
        //    "ig_russiandrunk",
        //    "ig_screen_writer",
        //    "ig_siemonyetarian",
        //    "ig_solomon",
        //    "ig_stevehains",
        //    "ig_stretch",
        //    "ig_talina",
        //    "ig_tanisha",
        //    "ig_taocheng",
        //    "ig_taostranslator",
        //    "ig_tenniscoach",
        //    "ig_terry",
        //    "ig_tomepsilon",
        //    "ig_tonya",
        //    "ig_tracydisanto",
        //    "ig_trafficwarden",
        //    "ig_tylerdix",
        //    "ig_wade",
        //    "ig_zimbor",
        //    "mp_f_deadhooker",
        //    "mp_f_misty_01",
        //    "mp_f_stripperlite",
        //    "mp_g_m_pros_01",
        //    "mp_m_claude_01",
        //    "mp_m_exarmy_01",
        //    "mp_m_famdd_01",
        //    "mp_m_fibsec_01",
        //    "mp_m_marston_01",
        //    "mp_m_niko_01",
        //    "mp_m_shopkeep_01",
        //    "mp_s_m_armoured_01",
        //    "player_one",
        //    "player_two",
        //    "player_zero",
        //    "s_f_m_fembarber",
        //    "s_f_m_maid_01",
        //    "s_f_m_shop_high",
        //    "s_f_m_sweatshop_01",
        //    "s_f_y_airhostess_01",
        //    "s_f_y_bartender_01",
        //    "s_f_y_baywatch_01",
        //    "s_f_y_cop_01",
        //    "s_f_y_factory_01",
        //    "s_f_y_hooker_01",
        //    "s_f_y_hooker_02",
        //    "s_f_y_hooker_03",
        //    "s_f_y_migrant_01",
        //    "s_f_y_movprem_01",
        //    "s_f_y_ranger_01",
        //    "s_f_y_scrubs_01",
        //    "s_f_y_sheriff_01",
        //    "s_f_y_shop_low",
        //    "s_f_y_shop_mid",
        //    "s_f_y_stripperlite",
        //    "s_f_y_stripper_01",
        //    "s_f_y_stripper_02",
        //    "s_f_y_sweatshop_01",
        //    "s_m_m_ammucountry",
        //    "s_m_m_armoured_01",
        //    "s_m_m_armoured_02",
        //    "s_m_m_autoshop_01",
        //    "s_m_m_autoshop_02",
        //    "s_m_m_bouncer_01",
        //    "s_m_m_chemsec_01",
        //    "s_m_m_ciasec_01",
        //    "s_m_m_cntrybar_01",
        //    "s_m_m_dockwork_01",
        //    "s_m_m_doctor_01",
        //    "s_m_m_fiboffice_01",
        //    "s_m_m_fiboffice_02",
        //    "s_m_m_gaffer_01",
        //    "s_m_m_gardener_01",
        //    "s_m_m_gentransport",
        //    "s_m_m_hairdress_01",
        //    "s_m_m_highsec_01",
        //    "s_m_m_highsec_02",
        //    "s_m_m_janitor",
        //    "s_m_m_lathandy_01",
        //    "s_m_m_lifeinvad_01",
        //    "s_m_m_linecook",
        //    "s_m_m_lsmetro_01",
        //    "s_m_m_mariachi_01",
        //    "s_m_m_marine_01",
        //    "s_m_m_marine_02",
        //    "s_m_m_migrant_01",
        //    "s_m_m_movalien_01",
        //    "s_m_m_movprem_01",
        //    "s_m_m_movspace_01",
        //    "s_m_m_paramedic_01",
        //    "s_m_m_pilot_01",
        //    "s_m_m_pilot_02",
        //    "s_m_m_postal_01",
        //    "s_m_m_postal_02",
        //    "s_m_m_prisguard_01",
        //    "s_m_m_scientist_01",
        //    "s_m_m_security_01",
        //    "s_m_m_snowcop_01",
        //    "s_m_m_strperf_01",
        //    "s_m_m_strpreach_01",
        //    "s_m_m_strvend_01",
        //    "s_m_m_trucker_01",
        //    "s_m_m_ups_01",
        //    "s_m_m_ups_02",
        //    "s_m_o_busker_01",
        //    "s_m_y_airworker",
        //    "s_m_y_ammucity_01",
        //    "s_m_y_armymech_01",
        //    "s_m_y_autopsy_01",
        //    "s_m_y_barman_01",
        //    "s_m_y_baywatch_01",
        //    "s_m_y_blackops_01",
        //    "s_m_y_blackops_02",
        //    "s_m_y_busboy_01",
        //    "s_m_y_chef_01",
        //    "s_m_y_clown_01",
        //    "s_m_y_construct_01",
        //    "s_m_y_construct_02",
        //    "s_m_y_cop_01",
        //    "s_m_y_dealer_01",
        //    "s_m_y_devinsec_01",
        //    "s_m_y_dockwork_01",
        //    "s_m_y_doorman_01",
        //    "s_m_y_dwservice_01",
        //    "s_m_y_dwservice_02",
        //    "s_m_y_factory_01",
        //    "s_m_y_fireman_01",
        //    "s_m_y_garbage",
        //    "s_m_y_grip_01",
        //    "s_m_y_hwaycop_01",
        //    "s_m_y_marine_01",
        //    "s_m_y_marine_02",
        //    "s_m_y_marine_03",
        //    "s_m_y_mime",
        //    "s_m_y_pestcont_01",
        //    "s_m_y_pilot_01",
        //    "s_m_y_prismuscl_01",
        //    "s_m_y_prisoner_01",
        //    "s_m_y_ranger_01",
        //    "s_m_y_robber_01",
        //    "s_m_y_sheriff_01",
        //    "s_m_y_shop_mask",
        //    "s_m_y_strvend_01",
        //    "s_m_y_swat_01",
        //    "s_m_y_uscg_01",
        //    "s_m_y_valet_01",
        //    "s_m_y_waiter_01",
        //    "s_m_y_winclean_01",
        //    "s_m_y_xmech_01",
        //    "s_m_y_xmech_02",
        //    "u_f_m_corpse_01",
        //    "u_f_m_miranda",
        //    "u_f_m_promourn_01",
        //    "u_f_o_moviestar",
        //    "u_f_o_prolhost_01",
        //    "u_f_y_bikerchic",
        //    "u_f_y_comjane",
        //    "u_f_y_corpse_01",
        //    "u_f_y_corpse_02",
        //    "u_f_y_hotposh_01",
        //    "u_f_y_jewelass_01",
        //    "u_f_y_mistress",
        //    "u_f_y_poppymich",
        //    "u_f_y_princess",
        //    "u_f_y_spyactress",
        //    "u_m_m_aldinapoli",
        //    "u_m_m_bankman",
        //    "u_m_m_bikehire_01",
        //    "u_m_m_fibarchitect",
        //    "u_m_m_filmdirector",
        //    "u_m_m_glenstank_01",
        //    "u_m_m_griff_01",
        //    "u_m_m_jesus_01",
        //    "u_m_m_jewelsec_01",
        //    "u_m_m_jewelthief",
        //    "u_m_m_markfost",
        //    "u_m_m_partytarget",
        //    "u_m_m_prolsec_01",
        //    "u_m_m_promourn_01",
        //    "u_m_m_rivalpap",
        //    "u_m_m_spyactor",
        //    "u_m_m_willyfist",
        //    "u_m_o_finguru_01",
        //    "u_m_o_taphillbilly",
        //    "u_m_o_tramp_01",
        //    "u_m_y_abner",
        //    "u_m_y_antonb",
        //    "u_m_y_babyd",
        //    "u_m_y_baygor",
        //    "u_m_y_burgerdrug_01",
        //    "u_m_y_chip",
        //    "u_m_y_cyclist_01",
        //    "u_m_y_fibmugger_01",
        //    "u_m_y_guido_01",
        //    "u_m_y_gunvend_01",
        //    "u_m_y_hippie_01",
        //    "u_m_y_imporage",
        //    "u_m_y_justin",
        //    "u_m_y_mani",
        //    "u_m_y_militarybum",
        //    "u_m_y_paparazzi",
        //    "u_m_y_party_01",
        //    "u_m_y_pogo_01",
        //    "u_m_y_prisoner_01",
        //    "u_m_y_proldriver_01",
        //    "u_m_y_rsranger_01",
        //    "u_m_y_sbike",
        //    "u_m_y_staggrm_01",
        //    "u_m_y_tattoo_01",
        //    "u_m_y_zombie_01"
        //    };

        #region Model Names
        private Dictionary<string, string> mainModels = new Dictionary<string, string>()
        {
		["player_zero"] = "Michael",
		["player_one"] = "Franklin",
		["player_two"] = "Trevor",
		["mp_f_freemode_01"] = "Freemode Female",
		["mp_m_freemode_01"] = "Freemode Male",		
        };
        private Dictionary<string, string> animalModels = new Dictionary<string, string>()
        {
		["a_c_shepherd"] = "Australian Shepherd",
		["a_c_boar"] = "Boar",
		["a_c_cat_01"] = "Cat",
		["a_c_chimp"] = "Chimp",
		["a_c_chop"] = "Chop",
		["a_c_cormorant"] = "Cormorant",
		["a_c_cow"] = "Cow",
		["a_c_coyote"] = "Coyote",
		["a_c_crow"] = "Crow",
		["a_c_deer"] = "Deer",
		["a_c_dolphin"] = "Dolphin",
		["a_c_fish"] = "Fish",
		["a_c_sharkhammer"] = "Hammerhead Shark",
		["a_c_chickenhawk"] = "Hawk",
		["a_c_hen"] = "Hen",
		["a_c_humpback"] = "Humpback",
		["a_c_husky"] = "Husky",
		["a_c_killerwhale"] = "Killer Whale",
		["a_c_mtlion"] = "Mountain Lion",
		["a_c_panther"] = "Panther",
		["a_c_pig"] = "Pig",
		["a_c_pigeon"] = "Pigeon",
		["a_c_poodle"] = "Poodle",
		["a_c_pug"] = "Pug",
		["a_c_rabbit_01"] = "Rabbit",
		["a_c_rat"] = "Rat",
		["a_c_retriever"] = "Retriever",
		["a_c_rhesus"] = "Rhesus",
		["a_c_rottweiler"] = "Rottweiler",
		["a_c_seagull"] = "Seagull",
		["a_c_stingray"] = "Stingray",
		["a_c_sharktiger"] = "Tiger Shark",
		["a_c_westy"] = "Westie",
        };
        private Dictionary<string, string> maleModels = new Dictionary<string, string>()
        {
		["a_m_y_carclub_01"] = "Car Club",
		["a_m_y_tattoocust_01"] = "Tattoo Cust",
		["a_m_m_afriamer_01"] = "African American",
		["a_m_m_acult_01"] = "Altruist Cult Mid-Age",
		["a_m_o_acult_01"] = "Altruist Cult Old",
		["a_m_o_acult_02"] = "Altruist Cult Old 2",
		["a_m_y_acult_01"] = "Altruist Cult Young",
		["a_m_y_acult_02"] = "Altruist Cult Young 2",
		["a_m_m_beach_01"] = "Beach",
		["a_m_m_beach_02"] = "Beach 2",
		["a_m_y_musclbeac_01"] = "Beach Muscle",
		["a_m_y_musclbeac_02"] = "Beach Muscle 2",
		["a_m_m_mlcrisis_01"] = "Midlife Crisis Bikers",
		["a_m_y_gencaspat_01"] = "Casual Casino Guest",
		["a_m_y_smartcaspat_01"] = "Formel Casino Guest",
		["a_m_o_beach_01"] = "Beach Old",
		["a_m_o_beach_02"] = "Beach Old 2",
		["a_m_m_trampbeac_01"] = "Beach Tramp",
		["a_m_y_beach_01"] = "Beach Young",
		["a_m_y_beach_02"] = "Beach Young 2",
		["a_m_y_beach_03"] = "Beach Young 3",
		["a_m_y_beach_04"] = "Beach Young 4",
		["a_m_m_bevhills_01"] = "Beverly Hills",
		["a_m_m_bevhills_02"] = "Beverly Hills 2",
		["a_m_y_bevhills_01"] = "Beverly Hills Young",
		["a_m_y_bevhills_02"] = "Beverly Hills Young 2",
		["a_m_y_stbla_01"] = "Black Street",
		["a_m_y_stbla_02"] = "Black Street 2",
		["a_m_y_breakdance_01"] = "Breakdancer",
		["a_m_y_busicas_01"] = "Business Casual",
		["a_m_m_business_01"] = "Business",
		["a_m_y_business_01"] = "Business Young",
		["a_m_y_business_02"] = "Business Young 2",
		["a_m_y_business_03"] = "Business Young 3",
		["a_m_y_cyclist_01"] = "Cyclist",
		["a_m_y_dhill_01"] = "Downhill Cyclist",
		["a_m_y_downtown_01"] = "Downtown",
		["a_m_m_eastsa_01"] = "East SA",
		["a_m_m_eastsa_02"] = "East SA 2",
		["a_m_y_eastsa_01"] = "East SA Young",
		["a_m_y_eastsa_02"] = "East SA Young 2",
		["a_m_y_epsilon_01"] = "Epsilon",
		["a_m_y_epsilon_02"] = "Epsilon 2",
		["a_m_m_farmer_01"] = "Farmer",
		["a_m_m_fatlatin_01"] = "Fat Latino",
		["a_m_y_gay_01"] = "Gay",
		["a_m_y_gay_02"] = "Gay 2",
		["a_m_m_genfat_01"] = "General Fat",
		["a_m_m_genfat_02"] = "General Fat 2",
		["a_m_o_genstreet_01"] = "General Street Old",
		["a_m_y_genstreet_01"] = "General Street Young",
		["a_m_y_genstreet_02"] = "General Street Young 2",
		["a_m_m_golfer_01"] = "Golfer",
		["a_m_y_golfer_01"] = "Golfer Young",
		["a_m_m_hasjew_01"] = "Hasidic Jew",
		["a_m_y_hasjew_01"] = "Hasidic Jew Young",
		["a_m_y_hiker_01"] = "Hiker",
		["a_m_m_hillbilly_01"] = "Hillbilly",
		["a_m_m_hillbilly_02"] = "Hillbilly 2",
		["a_m_y_hippy_01"] = "Hippie",
		["a_m_y_hipster_01"] = "Hipster",
		["a_m_y_hipster_02"] = "Hipster 2",
		["a_m_y_hipster_03"] = "Hipster 3",
		["a_m_m_indian_01"] = "Indian",
		["a_m_y_indian_01"] = "Indian Young",
		["a_m_y_jetski_01"] = "Jetskier",
		["a_m_y_runner_01"] = "Jogger",
		["a_m_y_runner_02"] = "Jogger 2",
		["a_m_y_juggalo_01"] = "Juggalo",
		["a_m_m_ktown_01"] = "Korean",
		["a_m_o_ktown_01"] = "Korean Old",
		["a_m_y_ktown_01"] = "Korean Young",
		["a_m_y_ktown_02"] = "Korean Young 2",
		["a_m_m_stlat_02"] = "Latino Street 2",
		["a_m_y_stlat_01"] = "Latino Street Young",
		["a_m_y_latino_01"] = "Latino Young",
		["a_m_m_malibu_01"] = "Malibu",
		["a_m_y_methhead_01"] = "Meth Addict",
		["a_m_m_mexlabor_01"] = "Mexican Labourer",
		["a_m_m_mexcntry_01"] = "Mexican Rural",
		["a_m_y_mexthug_01"] = "Mexican Thug",
		["a_m_y_motox_01"] = "Motocross Biker",
		["a_m_y_motox_02"] = "Motocross Biker 2",
		["a_m_m_og_boss_01"] = "OG Boss",
		["a_m_m_paparazzi_01"] = "Paparazzi",
		["a_m_m_polynesian_01"] = "Polynesian",
		["a_m_y_polynesian_01"] = "Polynesian Young",
		["a_m_m_prolhost_01"] = "Prologue Host",
		["a_m_y_roadcyc_01"] = "Road Cyclist",
		["a_m_m_rurmeth_01"] = "Rural Meth Addict",
		["a_m_m_salton_01"] = "Salton",
		["a_m_m_salton_02"] = "Salton 2",
		["a_m_m_salton_03"] = "Salton 3",
		["a_m_m_salton_04"] = "Salton 4",
		["a_m_o_salton_01"] = "Salton Old",
		["a_m_y_salton_01"] = "Salton Young",
		["a_m_m_skater_01"] = "Skater",
		["a_m_y_skater_01"] = "Skater Young",
		["a_m_y_skater_02"] = "Skater Young 2",
		["a_m_m_skidrow_01"] = "Skid Row",
		["a_m_m_socenlat_01"] = "South Central Latino",
		["a_m_m_soucent_01"] = "South Central",
		["a_m_m_soucent_02"] = "South Central 2",
		["a_m_m_soucent_03"] = "South Central 3",
		["a_m_m_soucent_04"] = "South Central 4",
		["a_m_o_soucent_01"] = "South Central Old",
		["a_m_o_soucent_02"] = "South Central Old 2",
		["a_m_o_soucent_03"] = "South Central Old 3",
		["a_m_y_soucent_01"] = "South Central Young",
		["a_m_y_soucent_02"] = "South Central Young 2",
		["a_m_y_soucent_03"] = "South Central Young 3",
		["a_m_y_soucent_04"] = "South Central Young 4",
		["a_m_y_sunbathe_01"] = "Sunbather",
		["a_m_y_surfer_01"] = "Surfer",
		["a_m_m_tennis_01"] = "Tennis Player",
		["a_m_m_tourist_01"] = "Tourist",
		["a_m_m_tramp_01"] = "Tramp",
		["a_m_o_tramp_01"] = "Tramp Old",
		["a_m_m_tranvest_01"] = "Transvestite",
		["a_m_m_tranvest_02"] = "Transvestite 2",
		["a_m_y_beachvesp_01"] = "Vespucci Beach",
		["a_m_y_beachvesp_02"] = "Vespucci Beach 2",
		["a_m_y_vindouche_01"] = "Vinewood Douche",
		["a_m_y_vinewood_01"] = "Vinewood",
		["a_m_y_vinewood_02"] = "Vinewood 2",
		["a_m_y_vinewood_03"] = "Vinewood 3",
		["a_m_y_vinewood_04"] = "Vinewood 4",
		["a_m_y_stwhi_01"] = "White Street",
		["a_m_y_stwhi_02"] = "White Street 2",
		["a_m_y_yoga_01"] = "Yoga",
		["a_m_y_clubcust_01"] = "Club Customer 1",
		["a_m_y_clubcust_02"] = "Club Customer 2",
		["a_m_y_clubcust_03"] = "Club Customer 3",
		["a_m_y_clubcust_04"] = "Club Customer 4",
		["g_m_m_prisoners_01"] = "Gang Prisoner",
		["g_m_m_slasher_01"] = "Gang Slasher",
		["g_m_importexport_01"] = "Gang Import-Export",
		["g_m_m_armboss_01"] = "Armenian Boss",
		["g_m_m_armgoon_01"] = "Armenian Goon",
		["g_m_y_armgoon_02"] = "Armenian Goon 2",
		["g_m_m_armlieut_01"] = "Armenian Lieutenant",
		["g_m_y_azteca_01"] = "Azteca",
		["g_m_y_ballaeast_01"] = "Ballas East",
		["g_m_y_ballaorig_01"] = "Ballas Original",
		["g_m_y_ballasout_01"] = "Ballas South",
		["g_m_m_cartelguards_01"] = "Cartel Guard",
		["g_m_m_cartelguards_02"] = "Cartel Guard 2",
		["g_m_m_casrn_01"] = "Casino Guests?",
		["g_m_m_chemwork_01"] = "Chemical Plant Worker",
		["g_m_m_chiboss_01"] = "Chinese Boss",
		["g_m_m_chigoon_01"] = "Chinese Goon",
		["g_m_m_chigoon_02"] = "Chinese Goon 2",
		["g_m_m_chicold_01"] = "Chinese Goon Older",
		["g_m_y_famca_01"] = "Families CA",
		["g_m_y_famdnf_01"] = "Families DNF",
		["g_m_y_famfor_01"] = "Families FOR",
		["g_m_m_korboss_01"] = "Korean Boss",
		["g_m_y_korlieut_01"] = "Korean Lieutenant",
		["g_m_y_korean_01"] = "Korean Young",
		["g_m_y_korean_02"] = "Korean Young 2",
		["g_m_m_mexboss_01"] = "Mexican Boss",
		["g_m_m_mexboss_02"] = "Mexican Boss 2",
		["g_m_y_mexgang_01"] = "Mexican Gang Member",
		["g_m_y_mexgoon_01"] = "Mexican Goon",
		["g_m_y_mexgoon_02"] = "Mexican Goon 2",
		["g_m_y_mexgoon_03"] = "Mexican Goon 3",
		["g_m_y_pologoon_01"] = "Polynesian Goon",
		["g_m_y_pologoon_02"] = "Polynesian Goon 2",
		["g_m_y_salvaboss_01"] = "Salvadoran Boss",
		["g_m_y_salvagoon_01"] = "Salvadoran Goon",
		["g_m_y_salvagoon_02"] = "Salvadoran Goon 2",
		["g_m_y_salvagoon_03"] = "Salvadoran Goon 3",
		["g_m_y_strpunk_01"] = "Street Punk",
		["g_m_y_strpunk_02"] = "Street Punk 2",
		["g_m_y_lost_01"] = "The Lost MC",
		["g_m_y_lost_02"] = "The Lost MC 2",
		["g_m_y_lost_03"] = "The Lost MC 3",
		["s_m_m_Autoshop_03"] = "Autoshop Worker 3",
		["s_m_m_raceorg_01"] = "Racer Organisator",
		["s_m_m_tattoo_01"] = "Tattoo Artist 2",
		["s_m_y_airworker"] = "Air Worker",
		["s_m_m_movalien_01"] = "Alien",
		["s_m_y_ammucity_01"] = "Ammu-Nation City",
		["s_m_m_ammucountry"] = "Ammu-Nation Rural",
		["s_m_m_armoured_01"] = "Armoured Van Security",
		["s_m_m_armoured_02"] = "Armoured Van Security",
		["s_m_y_armymech_01"] = "Army Mechanic",
		["s_m_y_autopsy_01"] = "Autopsy Tech",
		["s_m_m_autoshop_01"] = "Autoshop Worker",
		["s_m_m_autoshop_02"] = "Autoshop Worker 2",
		["s_m_y_barman_01"] = "Barman",
		["s_m_m_cntrybar_01"] = "Bartender Rural",
		["s_m_y_baywatch_01"] = "Baywatch",
		["s_m_y_blackops_01"] = "Black Ops Soldier",
		["s_m_y_blackops_02"] = "Black Ops Soldier 2",
		["s_m_y_blackops_03"] = "Black Ops Soldier 3",
		["s_m_m_bouncer_01"] = "Bouncer",
		["s_m_m_bouncer_02"] = "Bouncer 2",
		["s_m_y_busboy_01"] = "Busboy",
		["s_m_o_busker_01"] = "Busker",
		["s_m_y_casino_01"] = "Casino Staff",
		["s_m_y_chef_01"] = "Chef",
		["s_m_m_chemsec_01"] = "Chemical Plant Security",
		["s_m_y_clown_01"] = "Clown",
		["s_m_y_construct_01"] = "construction Worker",
		["s_m_y_construct_02"] = "construction Worker 2",
		["s_m_y_cop_01"] = "Cop",
		["s_m_m_ccrew_01"] = "Crew Member",
		["s_m_y_dealer_01"] = "Dealer",
		["s_m_y_devinsec_01"] = "Devin's Security",
		["s_m_m_dockwork_01"] = "Dock Worker",
		["s_m_y_dockwork_01"] = "Dock Worker",
		["s_m_m_doctor_01"] = "Doctor",
		["s_m_y_doorman_01"] = "Doorman",
		["s_m_m_drugprocess_01"] = "Drug Processer",
		["s_m_y_dwservice_01"] = "DW Airport Worker",
		["s_m_y_dwservice_02"] = "DW Airport Worker 2",
		["s_m_y_factory_01"] = "Factory Worker",
		["s_m_m_fiboffice_01"] = "FIB Office Worker",
		["s_m_m_fiboffice_02"] = "FIB Office Worker 2",
		["s_m_m_fibsec_01"] = "FIB Security",
		["s_m_m_fieldworker_01"] = "Field Worker",
		["s_m_y_fireman_01"] = "Fireman",
		["s_m_m_gaffer_01"] = "Gaffer",
		["s_m_y_garbage"] = "Garbage Worker",
		["s_m_m_gardener_01"] = "Gardener",
		["s_m_y_grip_01"] = "Grip",
		["s_m_m_hairdress_01"] = "Hairdresser",
		["s_m_m_highsec_01"] = "High Security",
		["s_m_m_highsec_02"] = "High Security 2",
		["s_m_m_highsec_04"] = "High Security 4",
		["s_m_y_hwaycop_01"] = "Highway Cop",
		["s_m_m_ciasec_01"] = "IAA Security",
		["s_m_m_janitor"] = "Janitor",
		["s_m_m_lathandy_01"] = "Latino Handyman",
		["s_m_m_lifeinvad_01"] = "Life Invader",
		["s_m_m_linecook"] = "Line Cook",
		["s_m_m_lsmetro_01"] = "LS Metro Worker",
		["s_m_m_mariachi_01"] = "Mariachi",
		["s_m_m_marine_01"] = "Marine",
		["s_m_m_marine_02"] = "Marine 2",
		["s_m_y_marine_01"] = "Marine Young",
		["s_m_y_marine_02"] = "Marine Young 2",
		["s_m_y_marine_03"] = "Marine Young 3",
		["s_m_y_xmech_01"] = "Mechanic",
		["s_m_y_xmech_02"] = "MC Clubhouse Mechanic",
		["s_m_m_migrant_01"] = "Migrant",
		["s_m_y_mime"] = "Mime Artist",
		["s_m_m_movspace_01"] = "Movie Astronaut",
		["s_m_m_movprem_01"] = "Movie Premiere",
		["s_m_m_paramedic_01"] = "Paramedic",
		["s_m_y_pestcont_01"] = "Pest Control",
		["s_m_m_pilot_01"] = "Pilot",
		["s_m_y_pilot_01"] = "Pilot",
		["s_m_m_pilot_02"] = "Pilot 2",
		["s_m_m_postal_01"] = "Postal Worker",
		["s_m_m_postal_02"] = "Postal Worker 2",
		["s_m_m_prisguard_01"] = "Prison Guard",
		["s_m_y_prisoner_01"] = "Prisoner",
		["s_m_y_prismuscl_01"] = "Prisoner Muscular",
		["s_m_y_ranger_01"] = "Ranger",
		["s_m_y_robber_01"] = "Robber",
		["s_m_y_shop_mask"] = "Mask Salesman",
		["s_m_m_scientist_01"] = "Scientist",
		["s_m_m_security_01"] = "Security Guard",
		["s_m_y_sheriff_01"] = "Sheriff",
		["s_m_m_snowcop_01"] = "Snow Cop",
		["s_m_m_strperf_01"] = "Street Performer",
		["s_m_m_strpreach_01"] = "Street Preacher",
		["s_m_m_strvend_01"] = "Street Vendor",
		["s_m_y_strvend_01"] = "Street Vendor Young",
		["s_m_y_swat_01"] = "SWAT",
		["s_m_m_gentransport"] = "Transport Worker",
		["s_m_m_trucker_01"] = "Trucker",
		["s_m_m_ups_01"] = "UPS Driver",
		["s_m_m_ups_02"] = "UPS Driver 2",
		["s_m_y_uscg_01"] = "US Coastguard",
		["s_m_y_valet_01"] = "Valet",
		["s_m_y_waiter_01"] = "Waiter",
		["s_m_y_westsec_01"] = "Duggan Secruity",
		["s_m_y_winclean_01"] = "Window Cleaner",
		["s_m_y_clubbar_01"] = "Club Bartender",
		["s_m_y_waretech_01"] = "Warehouse Technician",
		["s_m_m_highsec_03"] = "High Security 3",
		["s_m_y_westsec_02"] = "Duggan Security 2",
		["u_m_y_abner"] = "Abner",
		["u_m_m_aldinapoli"] = "Al Di Napoli",
		["u_m_y_antonb"] = "Anton Beaudelaire",
		["u_m_y_juggernaut_01"] = "Avon Juggernaut",
		["u_m_y_babyd"] = "Baby D",
		["u_m_m_bankman"] = "Bank Manager",
		["u_m_m_bikehire_01"] = "Bike Hire Guy",
		["u_m_m_blane"] = "Blane",
		["u_m_m_curtis"] = "Curtis",
		["u_m_m_vince"] = "Vince",
		["u_m_o_dean"] = "Dean",
		["u_m_y_caleb"] = "Caleb",
		["u_m_y_croupthief_01"] = "Casino Thief",
		["u_m_y_gabriel"] = "Gabriel",
		["u_m_y_ushi"] = "Ushi",
		["u_m_y_burgerdrug_01"] = "Burger Drug Worker",
		["u_m_y_chip"] = "Chip",
		["u_m_y_cyclist_01"] = "Cyclist",
		["u_m_y_corpse_01"] = "Dead Courier",
		["u_m_m_doa_01"] = "DOA Man",
		["u_m_m_edtoh"] = "Ed Toh",
		["u_m_y_militarybum"] = "Ex-Mil Bum",
		["u_m_m_fibarchitect"] = "FIB Architect",
		["u_m_y_fibmugger_01"] = "FIB Mugger",
		["u_m_o_finguru_01"] = "Financial Guru",
		["u_m_m_glenstank_01"] = "Glen-Stank",
		["u_m_m_griff_01"] = "Griff",
		["u_m_y_guido_01"] = "Guido",
		["u_m_y_gunvend_01"] = "Gun Vendor",
		["u_m_y_smugmech_01"] = "Hangar Mechanic",
		["u_m_y_hippie_01"] = "Hippie",
		["u_m_m_streetart_01"] = "Street Art",
		["u_m_y_imporage"] = "Impotent Rage",
		["u_m_o_taphillbilly"] = "Jesco White Tapdancing",
		["u_m_m_jesus_01"] = "Jesus",
		["u_m_m_jewelthief"] = "Jewel Thief",
		["u_m_m_jewelsec_01"] = "Jeweller Security",
		["u_m_y_justin"] = "Justin",
		["u_m_y_baygor"] = "Kifflom Guy",
		["u_m_m_willyfist"] = "Love Fist Willy",
		["u_m_y_mani"] = "Mani",
		["u_m_m_markfost"] = "Mark Fostenburg",
		["u_m_m_filmdirector"] = "Movie Director",
		["u_m_o_filmnoir"] = "Movie Corpse Suited",
		["u_m_y_paparazzi"] = "Paparazzi Young",
		["u_m_m_partytarget"] = "Party Target",
		["u_m_y_party_01"] = "Partygoer",
		["u_m_y_pogo_01"] = "Pogo the Monkey",
		["u_m_y_prisoner_01"] = "Prisoner",
		["u_m_y_proldriver_01"] = "Prologue Driver",
		["u_m_m_promourn_01"] = "Prologue Mourner",
		["u_m_m_prolsec_01"] = "Prologue Security",
		["u_m_y_rsranger_01"] = "Republican Space Ranger",
		["u_m_m_rivalpap"] = "Rival Paparazzo",
		["u_m_y_sbike"] = "Sports Biker",
		["u_m_m_spyactor"] = "Spy Actor",
		["u_m_y_staggrm_01"] = "Stag Party Groom",
		["u_m_y_tattoo_01"] = "Tattoo Artist",
		["u_m_o_tramp_01"] = "Tramp Old",
		["u_m_y_zombie_01"] = "Zombie",
		["u_m_y_danceburl_01"] = "Club Dancer Burlesque",
		["u_m_y_dancelthr_01"] = "Club Dancer Leather",
		["u_m_y_dancerave_01"] = "Club Dancer Rave",
		["mp_s_m_armoured_01"] = "Armoured Van Security",
		["mp_m_cocaine_01"] = "Biker Cocaine",
		["mp_m_counterfeit_01"] = "Biker Counterfeit",
		["mp_m_forgery_01"] = "Biker Forgery",
		["mp_m_meth_01"] = "Biker Meth",
		["mp_m_weed_01"] = "Biker Weed",
		["mp_m_boatstaff_01"] = "Boat-Staff",
		["mp_m_exarmy_01"] = "Ex-Army",
		["mp_m_execpa_01"] = "Executive PA",
		["mp_m_famdd_01"] = "Families DD",
		["mp_m_freemode_01"] = "Freemode",
		["mp_m_securoguard_01"] = "Securoserve Guard",
		["mp_m_shopkeep_01"] = "Shopkeeper",
		["mp_m_waremech_01"] = "Warehouse Mechanic",
  		["mp_m_weapexp_01"] = "Weapon Exp",
  		["mp_m_weapwork_01"] = "Weapon Work",
        };
        private Dictionary<string, string> femaleModels = new Dictionary<string, string>()
        {
		["a_f_y_carclub_01"] = "Car Club",
		["a_f_m_beach_01"] = "Beach",
		["a_f_m_trampbeac_01"] = "Beach Tramp",
		["a_f_y_beach_01"] = "Beach Young",
		["a_f_y_beach_02"] = "Beach Young 2",
		["a_f_m_bevhills_01"] = "Beverly Hills",
		["a_f_m_bevhills_02"] = "Beverly Hills 2",
		["a_f_y_bevhills_01"] = "Beverly Hills Young",
		["a_f_y_bevhills_02"] = "Beverly Hills Young 2",
		["a_f_y_bevhills_03"] = "Beverly Hills Young 3",
		["a_f_y_bevhills_04"] = "Beverly Hills Young 4",
		["a_f_m_bodybuild_01"] = "Bodybuilder",
		["a_f_m_business_02"] = "Business 2",
		["a_f_y_business_01"] = "Business Young",
		["a_f_y_business_02"] = "Business Young 2",
		["a_f_y_business_03"] = "Business Young 3",
		["a_f_y_business_04"] = "Business Young 4",
		["a_f_m_downtown_01"] = "Downtown",
		["a_f_y_scdressy_01"] = "Dressy",
		["a_f_m_eastsa_01"] = "East SA",
		["a_f_m_eastsa_02"] = "East SA 2",
		["a_f_y_eastsa_01"] = "East SA Young",
		["a_f_y_eastsa_02"] = "East SA Young 2",
		["a_f_y_eastsa_03"] = "East SA Young 3",
		["a_f_y_epsilon_01"] = "Epsilon",
		["a_f_m_fatbla_01"] = "Fat Black",
		["a_f_m_fatcult_01"] = "Fat Cult",
		["a_f_m_fatwhite_01"] = "Fat White",
		["a_f_y_femaleagent"] = "Agent",
		["a_f_y_fitness_01"] = "Fitness",
		["a_f_y_fitness_02"] = "Fitness 2",
		["a_f_y_genhot_01"] = "General Hot Young",
		["a_f_o_genstreet_01"] = "General Street Old",
		["a_f_y_gencaspat_01"] = "Casual Casino Guest",
		["a_f_y_smartcaspat_01"] = "Formel Casino Guest",
		["a_f_y_golfer_01"] = "Golfer Young",
		["a_f_y_hiker_01"] = "Hiker",
		["a_f_y_hippie_01"] = "Hippie",
		["a_f_y_hipster_01"] = "Hipster",
		["a_f_y_hipster_02"] = "Hipster 2",
		["a_f_y_hipster_03"] = "Hipster 3",
		["a_f_y_hipster_04"] = "Hipster 4",
		["a_f_o_indian_01"] = "Indian Old",
		["a_f_y_indian_01"] = "Indian Young",
		["a_f_y_runner_01"] = "Jogger",
		["a_f_y_juggalo_01"] = "Juggalo",
		["a_f_m_ktown_01"] = "Korean",
		["a_f_m_ktown_02"] = "Korean 2",
		["a_f_o_ktown_01"] = "Korean Old",
		["a_f_m_prolhost_01"] = "Prologue Host",
		["a_f_y_rurmeth_01"] = "Rural Meth Addict",
		["a_f_m_salton_01"] = "Salton",
		["a_f_o_salton_01"] = "Salton Old",
		["a_f_y_skater_01"] = "Skater",
		["a_f_m_skidrow_01"] = "Skid Row",
		["a_f_m_soucent_01"] = "South Central",
		["a_f_m_soucent_02"] = "South Central 2",
		["a_f_m_soucentmc_01"] = "South Central MC",
		["a_f_o_soucent_01"] = "South Central Old",
		["a_f_o_soucent_02"] = "South Central Old 2",
		["a_f_y_soucent_01"] = "South Central Young",
		["a_f_y_soucent_02"] = "South Central Young 2",
		["a_f_y_soucent_03"] = "South Central Young 3",
		["a_f_y_tennis_01"] = "Tennis Player",
		["a_f_y_topless_01"] = "Topless",
		["a_f_m_tourist_01"] = "Tourist",
		["a_f_y_tourist_01"] = "Tourist Young",
		["a_f_y_tourist_02"] = "Tourist Young 2",
		["a_f_m_tramp_01"] = "Tramp",
		["a_f_y_vinewood_01"] = "Vinewood",
		["a_f_y_vinewood_02"] = "Vinewood 2",
		["a_f_y_vinewood_03"] = "Vinewood 3",
		["a_f_y_vinewood_04"] = "Vinewood 4",
		["a_f_y_yoga_01"] = "Yoga",
		["a_f_y_clubcust_01"] = "Club Customer 1",
		["a_f_y_clubcust_02"] = "Club Customer 2",
		["a_f_y_clubcust_03"] = "Club Customer 3",
		["a_f_y_clubcust_04"] = "Club Customer 4",
		["a_f_y_bevhills_05"] = "Beverly Hills Young 5",
		["g_f_importexport_01"] = "Gang Import-Export",
		["g_f_y_ballas_01"] = "Ballas",
		["g_f_y_families_01"] = "Families",
		["g_f_importexport_01"] = "Import Export",
		["g_f_y_lost_01"] = "The Lost MC",
		["g_f_y_vagos_01"] = "Vagos",
		["s_f_m_autoshop_01"] = "Autoshop Worker",
		["s_f_m_retailstaff_01"] = "Retailstaff",
		["s_f_y_airhostess_01"] = "Air Hostess",
		["s_f_m_fembarber"] = "Barber",
		["s_f_y_bartender_01"] = "Bartender",
		["s_f_y_baywatch_01"] = "Baywatch",
		["s_f_y_beachbarstaff_01"] = "Beach Bar Staff",
		["s_f_y_casino_01"] = "Casino Staff",
		["s_f_y_cop_01"] = "Cop",
		["s_f_y_factory_01"] = "Factory Worker",
		["s_f_y_hooker_01"] = "Hooker",
		["s_f_y_hooker_02"] = "Hooker 2",
		["s_f_y_hooker_03"] = "Hooker 3",
		["s_f_y_scrubs_01"] = "Hospital Scrubs",
		["s_f_m_maid_01"] = "Maid",
		["s_f_y_migrant_01"] = "Migrant",
		["s_f_y_movprem_01"] = "Movie Premiere",
		["s_f_y_ranger_01"] = "Ranger",
		["s_f_m_shop_high"] = "Sales Assistant High-End",
		["s_f_y_shop_low"] = "Sales Assistant Low-End",
		["s_f_y_shop_mid"] = "Sales Assistant Mid-Price",
		["s_f_y_sheriff_01"] = "Sheriff",
		["s_f_y_stripper_01"] = "Stripper",
		["s_f_y_stripper_02"] = "Stripper 2",
		["s_f_y_stripperlite"] = "Stripper Lite",
		["s_f_m_sweatshop_01"] = "Sweatshop Worker",
		["s_f_y_sweatshop_01"] = "Sweatshop Worker",
		["s_f_y_clubbar_01"] = "Club Bartender",
		["s_f_y_clubbar_02"] = "Club Bartender 2",
            	["u_f_y_bikerchic"] = "Biker Chic",
		["u_f_m_corpse_01"] = "Corpse",
		["u_f_m_casinocash_01"] = "Casino Cashier",
		["u_f_m_casinoshop_01"] = "Casino shop owner",
		["u_f_m_debbie_01"] = "Debbie Agatha Secretary",
		["u_f_o_carol"] = "Carol",
		["u_f_o_eileen"] = "Eileen",
		["u_f_y_beth"] = "Beth",
		["u_f_y_lauren"] = "Lauren",
		["u_f_y_taylor"] = "Taylor",
		["u_f_y_corpse_01"] = "Corpse Young",
		["u_f_y_corpse_02"] = "Corpse Young 2",
		["u_f_y_hotposh_01"] = "Hot Posh",
		["u_f_y_comjane"] = "Jane",
		["u_f_y_jewelass_01"] = "Jeweller Assistant",
		["u_f_m_miranda"] = "Miranda",
		["u_f_y_mistress"] = "Mistress",
		["u_f_o_moviestar"] = "Movie Star",
		["u_f_y_poppymich"] = "Poppy Mitchell",
		["u_f_y_princess"] = "Princess",
		["u_f_o_prolhost_01"] = "Prologue Host Old",
		["u_f_m_promourn_01"] = "Prologue Mourner",
		["u_f_y_spyactress"] = "Spy Actress",
		["u_f_m_miranda_02"] = "Miranda 2",
		["u_f_y_poppymich_02"] = "Poppy Mitchell 2",
		["u_f_y_danceburl_01"] = "Club Dancer Burlesque",
		["u_f_y_dancelthr_01"] = "Club Dancer Leather",
		["u_f_y_dancerave_01"] = "Club Dancer Rave",
		["mp_f_cocaine_01"] = "Biker Cocaine",
		["mp_f_counterfeit_01"] = "Biker Counterfeit",
		["mp_f_forgery_01"] = "Biker Forgery",
		["mp_f_meth_01"] = "Biker Meth", 
		["mp_f_weed_01"] = "Biker Weed",
		["mp_f_boatstaff_01"] = "Boat-Staff",
		["mp_f_chbar_01"] = "Clubhouse Bar",
		["mp_f_execpa_01"] = "Executive PA",
		["mp_f_execpa_02"] = "Executive PA 2",
		["mp_f_freemode_01"] = "Freemode",
		["mp_f_helistaff_01"] = "Heli-Staff",
		["mp_f_cardesign_01"] = "Office Garage Mechanic",
		["mp_f_stripperlite"] = "Stripper Lite",
		["mp_f_bennymech_01"] = "Benny Mechanic",
		
        };
        private Dictionary<string, string> otherPeds = new Dictionary<string, string>()
        {
		["ig_avischwartzman_02"] = "Avi Schawrtzman",
		["ig_benny_02"] = "Benny Los Santos Tuners",
		["ig_drugdealer"] = "Drugdealer",
		["ig_hao_02"] = "Hao",
		["ig_lildee"] = "Lil Dee",
		["ig_mimi"] = "Mimi",
		["ig_moodyman_02"] = "Moodyman",
		["ig_sessanta"] = "Sessanta",
		["player_zero"] = "Michael",
		["player_one"] = "Franklin",
		["player_two"] = "Trevor",
		["ig_abigail"] = "Abigail Mathers",
		["ig_agent"] = "Agent",
		["ig_agatha"] = "Agatha Baker",
		["ig_ary"] = "Ary",
		["ig_avery"] = "Avery Duggan",
		["ig_mp_agent14"] = "Agent 14",
		["ig_amandatownley"] = "Amanda De Santa",
		["ig_andreas"] = "Andreas Sanchez",
		["ig_ashley"] = "Ashley Butler",
		["ig_avon"] = "Avon Hertz",
		["ig_ballasog"] = "Ballas OG",
		["ig_benny"] = "Benny",
		["ig_bankman"] = "Bank Manager",
		["ig_barry"] = "Barry",
		["ig_bestmen"] = "Best Man",
		["ig_beverly"] = "Beverly Felton",
		["ig_brucie2"] = "Brucie Kibbutz",
		["ig_orleans"] = "Bigfoot",
		["ig_brad"] = "Brad",
		["ig_bride"] = "Bride",
		["ig_car3guy1"] = "Car 3 Guy 1",
		["ig_car3guy2"] = "Car 3 Guy 2",
		["ig_casey"] = "Casey",
		["ig_chef"] = "Chef",
		["ig_chef2"] = "Chef",
		["ig_claypain"] = "Clay Jackson The Pain Giver",
		["ig_clay"] = "Clay Simons The Lost",
		["ig_cletus"] = "Cletus",
		["ig_chrisformage"] = "Cris Formage",
		["ig_dale"] = "Dale",
		["ig_davenorton"] = "Dave Norton",
		["ig_denise"] = "Denise",
		["ig_devin"] = "Devin",
		["ig_popov"] = "Dima Popov",
		["ig_dom"] = "Dom Beasley",
		["ig_drfriedlander"] = "Dr. Friedlander",
		["ig_tomepsilon"] = "Epsilon Tom",
		["ig_fabien"] = "Fabien",
		["ig_ramp_gang"] = "Families Gang Member?",
		["ig_mrk"] = "Ferdinand Kerimov Mr. K",
		["ig_fbisuit_01"] = "FIB Suit",
		["ig_floyd"] = "Floyd Hebert",
		["ig_g"] = "Gerald",
		["ig_groom"] = "Groom",
		["ig_gustavo"] = "Gustavo",
		["ig_hao"] = "Hao",
		["ig_ramp_hic"] = "Hick",
		["ig_ramp_hipster"] = "Hipster",
		["ig_helmsmanpavel"] = "Helmsman Pavel",
		["ig_hunter"] = "Hunter",
		["ig_isldj_00"] = "Island Dj",
   		["ig_isldj_01"] = "Island Dj 1",
		["ig_isldj_02"] = "Island Dj 2",
		["ig_isldj_03"] = "Island Dj 3",
 		["ig_isldj_04"] = "Island Dj 4",
  		["ig_isldj_04_D_01"] = "Island Dj 4D",
  		["ig_isldj_04_D_02"] = "Island Dj 4D2",
   		["ig_isldj_04_E_01"] = "Island Dj 4E",
   		["ig_jackie"] = "Jackie",
		["ig_janet"] = "Janet",
		["ig_jay_norris"] = "Jay Norris",
		["ig_jewelass"] = "Jeweller Assistant",
		["ig_jimmyboston"] = "Jimmy Boston",
		["ig_jimmydisanto"] = "Jimmy De Santa",
       	["ig_jio"] = "Jio",
		["ig_johnnyklebitz"] = "Johnny Klebitz",
		["ig_josef"] = "Josef",
		["ig_josh"] = "Josh",
       	["ig_juanstrickler"] = "Juan Strickler",
       	["ig_kaylee"] = "Kaylee",
		["ig_karen_daniels"] = "Karen Daniels",
		["ig_kerrymcintosh"] = "Kerry McIntosh",
		["ig_lamardavis"] = "Lamar Davis",
		["ig_lazlow"] = "Lazlow",
		["ig_lestercrest"] = "Lester Crest",
		["ig_lestercrest_2"] = "Lester Crest Doomsday Heist",
		["ig_lifeinvad_01"] = "Life Invader",
		["ig_lifeinvad_02"] = "Life Invader 2",
		["ig_magenta"] = "Magenta",
		["ig_manuel"] = "Manuel",
		["ig_marnie"] = "Marnie Allen",
		["ig_maryann"] = "Mary-Ann Quinn",
		["ig_maude"] = "Maude",
		["ig_rashcosvki"] = "Maxim Rashkovsky",
		["ig_ramp_mex"] = "Mexican",
		["ig_michelle"] = "Michelle",
       	["ig_miguelmadrazo"] = "Miguel Madrazo",
		["ig_milton"] = "Milton McIlroy",
		["ig_joeminuteman"] = "Minuteman Joe",
    	["ig_malc"] = "Malc",
		["ig_molly"] = "Molly",
		["ig_money"] = "Money Man",
		["ig_mrsphillips"] = "Mrs. Phillips",
		["ig_mrs_thornhill"] = "Mrs. Thornhill",
       	["ig_mjo"] = "Mjo",
		["ig_natalia"] = "Natalia",
		["ig_nervousron"] = "Nervous Ron",
		["ig_nigel"] = "Nigel",
		["ig_old_man1a"] = "Old Man 1",
		["ig_old_man2"] = "Old Man 2",
		["ig_omega"] = "Omega",
        ["ig_oldrichguy"] = "Old Rich Guy",
		["ig_oneil"] = "O'Neil Brothers",
		["ig_ortega"] = "Ortega",
		["ig_paige"] = "Paige Harris",
		["ig_patricia"] = "Patricia",
        ["ig_patricia_02"] = "Patricia 2",
        ["ig_pilot"] = "Pilot",
		["ig_dreyfuss"] = "Peter Dreyfuss",
		["ig_priest"] = "Priest",
		["ig_prolsec_02"] = "Prologue Security 2",
		["ig_roccopelosi"] = "Rocco Pelosi",
		["ig_russiandrunk"] = "Russian Drunk",
		["ig_screen_writer"] = "Screenwriter",
		["ig_siemonyetarian"] = "Simeon Yetarian",
		["ig_solomon"] = "Solomon Richards",
        ["ig_sss"] = "Sss",
		["ig_stevehains"] = "Steve Haines",
		["ig_stretch"] = "Stretch",
		["ig_talina"] = "Talina",
		["ig_tanisha"] = "Tanisha",
		["ig_taocheng"] = "Tao Cheng",
		["ig_taocheng2"] = "Tao Cheng Casino",
		["ig_taostranslator"] = "Tao's Translator",
		["ig_taostranslator2"] = "Tao's Translator Casino",
		["ig_tenniscoach"] = "Tennis Coach",
		["ig_terry"] = "Terry",
		["ig_tonya"] = "Tonya",
		["ig_tracydisanto"] = "Tracey De Santa",
		["ig_trafficwarden"] = "Traffic Warden",
		["ig_tylerdix"] = "Tyler Dixon",
		["ig_paper"] = "United Paper Man",
		["ig_vagspeak"] = "Vagos Funeral Speaker",
		["ig_wade"] = "Wade",
		["ig_chengsr"] = "Wei Cheng",
		["ig_zimbor"] = "Zimbor",
		["ig_djblamadon"] = "DJ Black Madonna",
		["ig_djblamryans"] = "DJ Ryan S",
		["ig_djblamrupert"] = "DJ Rupert",
		["ig_djdixmanager"] = "DJ Dixon Manager",
		["ig_djsolfotios"] = "DJ Fotios",
		["ig_djsoljakob"] = "DJ Jakob",
		["ig_djsolmike"] = "DJ Mike T",
		["ig_djsolrobt"] = "DJ Rob T",
		["ig_djtalaurelia"] = " DJ Aurelia",
		["ig_djtalignazio"] = "DJ Ignazio",
		["ig_dix"] = "Dixon",
		["ig_englishdave"] = "English Dave",
        ["ig_englishdave_02"] = "English Dave",
		["ig_djgeneric_01"] = "Generic DJ",
		["ig_jimmyboston_02"] = "Jimmy Boston 2",
		["ig_kerrymcintosh_02"] = "Kerry McIntosh 2",
		["ig_lacey_jones_02"] = "Lacy Jones 2",
		["ig_lazlow_2"] = "Lazlow 2",
		["ig_sol"] = "Soloman",
		["ig_djsolmanager"] = "Soloman Manager",
		["ig_talcc"] = "Tale of Us 1",
		["ig_talmm"] = "Tale of Us 2",
		["ig_tylerdix_02"] = "Tyler Dixon 2",
		["ig_tonyprince"] = "Tony Prince",
		["ig_sacha"] = "Sacha Yetarian",
		["ig_thornton"] = "Thornton Duggan",
		["ig_tomcasino"] = "Tom Connors",
		["ig_vincent"] = "Vincent Casino",
		["ig_celeb_01"] = "Celeb 1",
		["ig_georginacheng"] = "Georgina Cheng",
		["ig_huang"] = "Huang",
		["ig_jimmydisanto2"] = "Jimmy De Santa 2",
		["ig_lestercrest_3"] = "Lester Crest 3",
		["ig_vincent_2"] = "Vincent Casino 2",
		["ig_wendy"] = "Wendy",
		["mp_m_avongoon"] = "Avon Goon",
		["mp_m_bogdangoon"] = "Bogdan Goon",
		["mp_m_claude_01"] = "Claude Speed",
		["mp_f_deadhooker"] = "Dead Hooker",
		["mp_m_fibsec_01"] = "FIB Security",
		["mp_m_marston_01"] = "John Marston",
		["mp_f_misty_01"] = "Misty",
		["mp_m_niko_01"] = "Niko Bellic",
		["mp_g_m_pros_01"] = "Pros",
		["mp_m_g_vagfun_01"] = "Vagos Funeral",
		["hc_driver"] = "Jewel Heist Driver",
		["hc_gunman"] = "Jewel Heist Gunman",
		["hc_hacker"] = "Jewel Heist Hacker",
        };


        #endregion

    }
}

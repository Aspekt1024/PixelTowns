#if TOOLS

using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace PixelTowns.Translation;

[Tool]
public partial class TranslationKeyEditorProperty : EditorProperty
{
    private MenuButton menuButton;
    private TranslationKey translationKey;
    
    private class Menu
    {
        private readonly PopupMenu popupMenu;
        private readonly List<string> items = new();
        private readonly Dictionary<string, Menu> subMenus = new();

        private readonly Action<string> itemSelectedCallback;
        
        public Menu(PopupMenu popupMenu, Action<string> itemSelectedCallback)
        {
            this.popupMenu = popupMenu;
            this.itemSelectedCallback = itemSelectedCallback;
            popupMenu.IndexPressed += index => itemSelectedCallback.Invoke(popupMenu.GetItemText((int)index));
        }

        public void AddItem(string item)
        {
            AddSubItem(item, item.Split("_"), 0);
        }

        public void PopulateMenus(bool addDefaultValue = false)
        {
            if (addDefaultValue)
            {
                popupMenu.AddItem("None");
                popupMenu.AddSeparator();
            }
            
            List<string> menuKeys = subMenus.Keys.OrderBy(k => k).ToList();
            foreach (var menuKey in menuKeys)
            {
                popupMenu.AddChild(subMenus[menuKey].popupMenu);
                popupMenu.AddSubmenuItem(menuKey, menuKey);
                
                subMenus[menuKey].PopulateMenus();
            }

            if (menuKeys.Any() && items.Any())
            {
                popupMenu.AddSeparator();
            }
            
            items.Sort();
            foreach (var item in items)
            {
                popupMenu.AddItem(item);
            }
            
        }

        /// <summary>
        /// Adds an item that belongs in a submenu.
        /// The index indicates how deep into the submenus we currently are (0 = parent level)
        /// </summary>
        private void AddSubItem(string item, string[] itemSplit, int index)
        {
            if (index == itemSplit.Length - 1)
            {
                items.Add(item);
            }
            else
            {
                Menu subMenu = subMenus.ContainsKey(itemSplit[index])
                    ? subMenus[itemSplit[index]]
                    : CreateSubMenu(itemSplit[index]);
                
                subMenu.AddSubItem(item, itemSplit, index + 1);
            }
        }

        private Menu CreateSubMenu(string name)
        {
            PopupMenu subPopup = new PopupMenu();
            subPopup.Name = name;
            
            Menu menu = new Menu(subPopup, itemSelectedCallback);
            subMenus.Add(name, menu);
            
            return menu;
        }
    }

    public TranslationKeyEditorProperty() : this("", null, null) {}
    
    public TranslationKeyEditorProperty(string name, TranslationKey tk, List<string> translationKeys)
    {
        if (string.IsNullOrEmpty(name) || tk == null || translationKeys == null || !translationKeys.Any()) return;
        
        Label = name;
        translationKey = tk;

        menuButton = new MenuButton();
        AddChild(menuButton);
        AddFocusable(menuButton);
        
        int selectedIndex = Mathf.Max(0, translationKeys.FindIndex(item => item == tk.Key));
        menuButton.Text = translationKeys[selectedIndex];
        
        Menu parentPopup = new Menu(menuButton.GetPopup(), OnNewKeySelected);
        foreach (var item in translationKeys)
        {
            parentPopup.AddItem(item);
        }
        
        parentPopup.PopulateMenus(true);
    }

    private void OnNewKeySelected(string newKey)
    {
        SetKey(newKey);
    }

    public override void _UpdateProperty()
    {
        var obj = GetEditedObject();
        var prop = GetEditedProperty();
        if (obj == null || string.IsNullOrEmpty(prop)) return;
        
        string newKey = (string)obj.Get(GetEditedProperty());
        if (newKey == translationKey.Key) return;

        SetKey(newKey);
    }

    private void SetKey(string newKey)
    {
        string prop = nameof(TranslationKey.Key);
        
        menuButton.Text = newKey;
        translationKey.Set(prop, newKey);
        EmitChanged(prop, newKey);
    }
}
#endif
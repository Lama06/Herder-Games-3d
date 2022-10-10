using System;
using System.Collections.Generic;
using System.Linq;
using HerderGames.Util;
using UnityEngine;

namespace HerderGames.Player
{
    [RequireComponent(typeof(Player))]
    public class Inventory : MonoBehaviour, PersistentDataContainer
    {
        public readonly Dictionary<Item, int> Items = new();

        public void AddItem(Item type)
        {
            Items[type] += 1;
        }

        public bool HasItem(Item type)
        {
            return Items[type] != 0;
        }

        public bool RemoveItem(Item type)
        {
            if (Items[type] == 0)
            {
                return false;
            }

            Items[type] -= 1;
            return true;
        }

        private string SaveKey => "player.inventory";

        public void LoadData()
        {
            foreach (var item in Enum.GetValues(typeof(Item)).Cast<Item>())
            {
                Items.Add(item, PlayerPrefs.GetInt($"{SaveKey}.items.{item.ToString()}", 0));
            }
        }

        public void SaveData()
        {
            foreach (var (type, amount) in Items)
            {
                PlayerPrefs.SetInt($"{SaveKey}.items.{type.ToString()}", amount);
            }
        }

        public void DeleteData()
        {
            foreach (var item in Enum.GetValues(typeof(Item)).Cast<Item>())
            {
                PlayerPrefs.DeleteKey($"{SaveKey}.items.{item.ToString()}");
            }
        }
    }
}

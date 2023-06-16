using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public string NameItem { get; set; }
    public int PriceItem { get; set; }

    public ItemData itemData = new();

    public GameObject[] allItems;
    private void Start()
    {
        if (PlayerPrefs.HasKey("SaveGame"))
        {
            Load();
        }
        else
        {
            itemData.money = 500;
            Save();
        }
    }

    private void Save()
    {
        PlayerPrefs.SetString("SaveGame", JsonUtility.ToJson(itemData));
    }

    private void Load()
    {
        itemData = JsonUtility.FromJson<ItemData>(PlayerPrefs.GetString("SaveGame"));

        for (int i = 0; i < itemData.buyItems.Count; i++)
        {
            for (int j = 0; j < allItems.Length; j++)
            {
                if (allItems[j].GetComponent<Item>().NameItem == itemData.buyItems[i])
                {
                    allItems[j].GetComponent<Item>().textPrice.text = "Good";
                    allItems[j].GetComponent<Item>().isBuy = true;
                }
            }
        }
    }

    public void BuyItem()
    {
        if (itemData.money >= PriceItem)
        {
            itemData.buyItems.Add((NameItem));
            itemData.money = itemData.money - PriceItem;

            Save();
            Load();
        }
    }
}

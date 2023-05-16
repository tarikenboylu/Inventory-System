using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    public const int Slot_Count = 100;
    
    public Dictionary<Attribute.Type, int> totalAttributes;

    [SerializeField] private Transform slotRoot;
    [SerializeField] private ItemSlotContainer containerPrefab;
    
    public ItemSlotContainer[] SlotList = new ItemSlotContainer[Slot_Count];

    private void Start()
    {
        SlotList = new ItemSlotContainer[Slot_Count];

        for (int i = 0; i < Slot_Count; i++)
        {
            InventorySlot slot = Instantiate(InventorySystemManager.slotPrefab, slotRoot).GetComponent<InventorySlot>();
            slot.inventory = this;
        }

        /*if (File.Exists("Inventory.save")) LoadInventory();*/
    }

    private void CalculateAttributes()
    {
        totalAttributes = new();

        for (int i = 0; i < Enum.GetNames(typeof(Attribute.Type)).Length; i++)
            totalAttributes.Add((Attribute.Type)i, 0);

        for (int i = 0; i < Slot_Count; i++)
        {
            if (SlotList[i].item == null) continue;

            if (SlotList[i].item.countable) continue;

            for (int j = 0; j < Slot_Count; j++)
                if (SlotList[i].item.attributes[j].activeWhile == Attribute.ActiveWhile.OnInventory)
                    totalAttributes[SlotList[i].item.attributes[j].attribute] += SlotList[i].item.attributes[j].value;
        }

        foreach (KeyValuePair<Attribute.Type, int> item in totalAttributes)
            Debug.Log(item.Key + " " + item.Value);
    }

    public void UpgradeItem(int index)
    {
        if (!SlotList[index].item.upgradable)
        {
            Debug.LogError("Item can't be upgraded");
            return;
        }

        SlotList[index].item.Upgrade();
    }

    //UIButton Function
    public void AddItem(ItemTemplate _item) => AddItem(_item, 1);

    /// <summary>
    /// After reorder Inventory
    /// </summary>
    public void AddItem(ItemTemplate template, int _amount = 1)
    {
        ItemContainerData data = new() { amount = _amount, item = template.CreateNewItem() };
        int index = ConvenientSlotIndex(data.item.ID, (Item.Rarity)data.item.rarity);

        if (index == -1)
        {
            index = ConvenientSlotIndex();

            if(index == -1)
                Debug.LogError("Inventory is Full");
            else
                SlotList[index].OverrideSlot(data);
        }
        else
            SlotList[index].AddToSlot(_amount);
    }

    public void RemoveItem(int _id, Item.Rarity _rarity, int _amount = 1)
    {
        Debug.Assert(ItemCount(_id, _rarity) < _amount, "Item is not enough ");

        for (int i = 0; i < Slot_Count; i++)
        {
            if (SlotList[i].Equality(_id, _rarity))
            {
                if (!SlotList[i].item.countable && _amount > 0)
                {
                    SlotList[i].AddToSlot(_amount);
                    _amount--;
                }
                else
                {
                    if (SlotList[i].amount >= _amount)
                    {
                        SlotList[i].RemoveItem(_amount);
                        _amount = 0;
                    }
                    else
                    {
                        SlotList[i].AddToSlot(_amount);
                        _amount -= SlotList[i].amount;
                    }
                }
                
                if (_amount == 0) return;
            }
        }
    }

    public void SortInventory()
    {
        for (int i = 0, deviation = 1; i + deviation < Slot_Count; i++)
            if (SlotList[i].item == null)
                for (; i + deviation < Slot_Count; deviation++)
                    if (SlotList[i + deviation].item != null)
                    {
                        SlotList[i].OverrideSlot(SlotList[i + deviation].GetContainerData());
                        SlotList[i + deviation].ClearSlot();
                        break;
                    }
    }

    /// <returns>First same countable item or empty slot index</returns>
    private int ConvenientSlotIndex(int _id, Item.Rarity _rarity)
    {
        //if item exist in inventory return item slot index
        for (int i = 0; i < Slot_Count; i++)
            if (SlotList[i].Equality(_id, _rarity))
                if (SlotList[i].Convenient) return i;

        return -1;
    }

    /// <returns>First empty slot index</returns>
    private int ConvenientSlotIndex()
    {
        for (int i = 0; i < Slot_Count; i++)
            if (SlotList[i].item == null)
                return i;

        return -1;
    }

    private int ItemCount(int _id, Item.Rarity _rarity)
    {
        int count = 0;

        for (int i = 0 ; i < Slot_Count; i++)
            if (SlotList[i].Equality(_id, _rarity))
                count += SlotList[i].amount;

        return count;
    }

    public void SaveInventory()
    {
        ItemContainerData[] slotData = new ItemContainerData[Slot_Count];

        for (int i = 0; i < Slot_Count; i++)
        {
            Item currentItem = SlotList[i].item != null ? SlotList[i].item : null;

            if (currentItem != null)
            {
                int amount = !currentItem.countable ? 1 : SlotList[i].amount;

                slotData[i] = new()
                {
                    amount = amount,
                    countable = currentItem.countable,
                    item = new()
                    {
                        rarity = (int)currentItem.rarity,
                        durability = currentItem.durability,
                        upgradeLevel = currentItem.upgradeLevel,
                        attributes = currentItem.attributes,
                        ID = currentItem.ID
                    }
                };
            }
            else
                slotData[i] = null;
        }

        SaveSystem.Save(slotData, "Inventory.save");
    }

    public void LoadInventory()
    {
        ItemContainerData[] slotsData = SaveSystem.Load<ItemContainerData[]>("Inventory.save");

        for (int i = 0; i < Slot_Count; i++)
            SlotList[i].OverrideSlot(slotsData[i]);
    }
}
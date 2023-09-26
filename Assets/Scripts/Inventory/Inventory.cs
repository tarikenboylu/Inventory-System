using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private bool isMainInventory;
    public const int Slot_Count = 100;
    public static Inventory MainInventory;
    //public Dictionary<Attribute.Type, int> totalAttributes;

    [SerializeField] private Transform slotRoot;
    private InventorySlot[] Slots = new InventorySlot[Slot_Count];
    private SlotData[] SlotList = new SlotData[Slot_Count];

    private void Start()
    {
        if (isMainInventory) MainInventory = this;

        SlotList = new SlotData[Slot_Count];
        Slots = new InventorySlot[Slot_Count];

        for (int i = 0; i < Slot_Count; i++)
        {
            InventorySlot slot = Instantiate(InventorySystemManager.slotPrefab, slotRoot).GetComponent<InventorySlot>();
            slot.inventory = this;
            SlotList[i] = new SlotData(null, 0);
            Slots[i] = slot;
            slot.index = i;
            RefreshSlot(i);
        }

        /*if (File.Exists("Inventory.save")) LoadInventory();*/
    }

    //private void CalculateAttributes()
    //{
    //    totalAttributes = new();

    //    for (int i = 0; i < Enum.GetNames(typeof(Attribute.Type)).Length; i++)
    //        totalAttributes.Add((Attribute.Type)i, 0);

    //    for (int i = 0; i < Slot_Count; i++)
    //    {
    //        if (SlotList[i].isEmpty) continue;

    //        //if (SlotList[i].item.countable) continue;

    //        for (int j = 0; j < Slot_Count; j++)
    //            if (SlotList[i].container.item.attributes[j].activeWhile == Attribute.ActiveWhile.OnInventory)
    //                totalAttributes[SlotList[i].item.attributes[j].attribute] += SlotList[i].item.attributes[j].value;
    //    }

    //    foreach (KeyValuePair<Attribute.Type, int> item in totalAttributes)
    //        Debug.Log(item.Key + " " + item.Value);
    //}

    public void SwapSlots(int index1, int index2)
    {
        (SlotList[index1], SlotList[index2]) = (SlotList[index2], SlotList[index1]);

        RefreshSlot(index1);
        RefreshSlot(index2);
    }

    //UIButton Function
    public void AddItem(ItemTemplate template) => AddItem(template.CreateNewItem());
    public void AddItemToSlot(ItemData data, int SlotIndex) => AddItemToSlot(new SlotData(data, 1) , SlotIndex);
    public void AddItem(ItemData data) => AddItem(new SlotData(data, 1));

    /// <summary>
    /// After reorder Inventory
    /// </summary>
    public void AddItem(SlotData data)
    {
        int RemainingAmount = data.stack;
        int index;

        //Fullfill half slots...
        while (HasConvenientStack(data.item, out index) && RemainingAmount > 0)
        {
            SlotList[index].AddStack(ref RemainingAmount);
            RefreshSlot(index);
        }

        //else empty slots..
        while (RemainingAmount > 0 && HasEmptySlot(out index))
        {
            SlotList[index].item = data.item;
            SlotList[index].AddStack(ref RemainingAmount);
            RefreshSlot(index);
        }

        if (RemainingAmount > 0) Debug.Log("Inventory is FULL");
    }

    public void AddItemToSlot(SlotData data, int SlotIndex)
    {
        int RemainingAmount = data.stack;

        if (SlotList[SlotIndex].item != null)
        {
            if (SlotList[SlotIndex].item.ItemTemplate != data.item.ItemTemplate)
            {
                AddItem(data);
                return;
            }
            else
            {
                SlotList[SlotIndex].AddStack(ref RemainingAmount);

                if (RemainingAmount > 0)
                    AddItem(new SlotData(data.item, RemainingAmount));

                return;
            }
        }


        SlotList[SlotIndex].item = data.item;
        SlotList[SlotIndex].AddStack(ref RemainingAmount);
        SlotList[SlotIndex] = data;
        RefreshSlot(SlotIndex);
    }

    public void ClearSlot(int index)
    {
        int amount = SlotList[index].stack;
        SlotList[index].item = null;
        SlotList[index].RemoveStack(ref amount);
        RefreshSlot(index);
    }

    public bool RemoveItem(SlotData slot)
    {
        if (ItemCount(slot.item) < slot.stack) return false;

        for (int i = 0; i < Slot_Count; i++)
        {
            //
        }
        return false;
    }

    private bool HasConvenientStack(ItemData data, out int slotIndex)
    {
        for (int i = 0; i < Slot_Count; i++)
        {
            SlotData slot = SlotList[i];
            
            if(slot.item == null) continue;

            if (slot.isMax) continue;

            if (SlotList[i].item.ItemTemplate == data.ItemTemplate)
            {
                slotIndex = i;
                return true;
            }
        }

        slotIndex = -1;
        return false;
    }

    private bool HasEmptySlot(out int SlotIndex)
    {
        for (int i = 0; i < Slot_Count; i++)
            if (SlotList[i].item == null)
            {
                SlotIndex = i;
                return true;
            }

        SlotIndex = -1;
        return false;
    }

    public void SortInventory()
    {
        //for (int i = 0, deviation = 1; i + deviation < Slot_Count; i++)
        //    if (SlotList[i].item == null)
        //        for (; i + deviation < Slot_Count; deviation++)
        //            if (SlotList[i + deviation].item != null)
        //            {
        //                SlotList[i].OverrideSlot(SlotList[i + deviation].GetContainerData());
        //                SlotList[i + deviation].ClearSlot();
        //                break;
        //            }
    }

    /// <returns>First same countable item or empty slot index</returns>
    private int ItemCount(ItemData data)
    {
        int count = 0;

        for (int slotIndex = 0; slotIndex < Slot_Count; slotIndex++)
        {
            SlotData slot = SlotList[slotIndex];

            if (slot.item == null) continue;
            
            if (slot.item.ItemTemplate == data.ItemTemplate) count += slot.stack;
        }

        return count;
    }

    /// <summary>
    ///Searchs item with UniqueID
    /// </summary>
    /// <returns>Result, SlotIndex</returns>
    private bool FindItem(ItemData data, out int SlotIndex)
    {
        for (int slotIndex = 0; slotIndex < Slot_Count; slotIndex++)
        {
            if (SlotList[slotIndex].item == null) continue;

            ItemData item = SlotList[slotIndex].item;

            if (item.ItemTemplate == data.ItemTemplate && item.UniqueID == data.UniqueID)
            {
                SlotIndex = slotIndex;
                return true;
            }
        }

        SlotIndex = -1;
        return false;
    }

    //private void RefreshInventory()
    //{
    //    for (int i = 0; i < Slot_Count; i++) RefreshSlot(i);
    //}

    private void RefreshSlot(int index)
    {
        SlotData slot = SlotList[index];

        Slots[index].RefreshSlot(slot.item, slot.stack);
    }

    public void SaveInventory()
    {
        SlotData[] slotData = new SlotData[Slot_Count];

        for (int i = 0; i < Slot_Count; i++)
        {
            //Item currentItem = SlotList[i].item != null ? SlotList[i].item : null;

            //if (currentItem != null)
            //{
            //    int amount = !currentItem.countable ? 1 : SlotList[i].amount;

            //    slotData[i] = new()
            //    {
            //        amount = amount,
            //        countable = currentItem.countable,
            //        item = new()
            //        {
            //            rarity = (int)currentItem.rarity,
            //            durability = currentItem.durability,
            //            upgradeLevel = currentItem.upgradeLevel,
            //            attributes = currentItem.attributes,
            //            ID = currentItem.ID
            //        }
            //    };
            //}
            //else
            //    slotData[i] = null;
        }

        SaveSystem.Save(slotData, "Inventory.save");
    }

    public void LoadInventory()
    {
        SlotData[] slotsData = SaveSystem.Load<SlotData[]>("Inventory.save");

        for (int i = 0; i < Slot_Count; i++)
            SlotList[i] = slotsData[i];
    }
}
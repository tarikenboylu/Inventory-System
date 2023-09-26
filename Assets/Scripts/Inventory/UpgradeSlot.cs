using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeSlot : Slot
{
    [SerializeField] UpgradeWindow window;
    ItemData itemData;
    int slotIndex;
    EquipmentManager equipmentManager;
    Inventory inventory;
    bool itemFromEquipment = false;

    public override void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent(out Item DroppedItem))
        {
            if (!DroppedItem.data.upgradable) return;

            if (DroppedItem.slot.TryGetComponent(out InventorySlot slot_Inventory))
            {
                itemFromEquipment = false;
                slotIndex = slot_Inventory.index;
                inventory = slot_Inventory.inventory;
                InsertItem(DroppedItem.data);
                slot_Inventory.inventory.ClearSlot(slot_Inventory.index);
            }
            else if (DroppedItem.slot.TryGetComponent(out EquipmentSlot slot_Equipment))
            {
                itemFromEquipment = true;
                slotIndex = slot_Equipment.index;
                equipmentManager = slot_Equipment.equipmentManager;
            }
        }
    }

    private void InsertItem(ItemData item)
    {
        RefreshSlot(item);
        itemData = item;
        window.Display(item);
    }

    public override void ClearSlot()
    {
        base.ClearSlot();
        window.Clear();
    }

    public void UpgradeItem()
    {
        if (itemData == null) return;
        //Cost and success Calculations...

        itemData.UpgradeLevel();

        if (!itemFromEquipment)
            inventory.AddItemToSlot(itemData, slotIndex);
        else
            equipmentManager.EquipItem(slotIndex, itemData);

        ClearSlot();
    }

    /// <summary>
    /// Item returns to last position
    /// </summary>
    public void RemoveItem()
    {
        if(itemData != null)
        {
            if (!itemFromEquipment)
                inventory.AddItemToSlot(itemData, slotIndex);
            else
                equipmentManager.EquipItem(slotIndex, itemData);

            ClearSlot();
        }
    }

}
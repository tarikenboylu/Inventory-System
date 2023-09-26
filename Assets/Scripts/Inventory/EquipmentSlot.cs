using UnityEngine.EventSystems;

public class EquipmentSlot : Slot
{
    public ItemType slotType;
    public EquipmentManager equipmentManager;
    public int index;
    private ItemData equippedItem;

    private void Awake()
    {
        equipmentManager = transform.GetComponentInParent<EquipmentManager>();
        index = transform.GetSiblingIndex();
    }

    public override void ClearSlot()
    {
        base.ClearSlot();
        equippedItem = null;
        equipmentManager.RemoveItem(index);
    }

    public void InsertItem(ItemData item)
    {
        equippedItem = item;
        equipmentManager.EquipItem(index, item);
    }

    public override void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        if (eventData.pointerDrag.TryGetComponent(out Item DroppedItem))
        {
            if (DroppedItem.data.ItemTemplate.type != slotType) return;

            if (DroppedItem.slot.TryGetComponent(out InventorySlot slot_Inventory))
            {
                if (equippedItem != null) slot_Inventory.inventory.AddItemToSlot(equippedItem, slot_Inventory.index);

                equipmentManager.EquipItem(index, DroppedItem.data);
                slot_Inventory.inventory.ClearSlot(slot_Inventory.index);
            }
            else if (DroppedItem.slot.TryGetComponent(out EquipmentSlot slot_Equipment))
            {
                equipmentManager.EquipItem(index, DroppedItem.data);
                slot_Equipment.equipmentManager.RemoveItem(slot_Equipment.index);
            }
            else if (DroppedItem.slot.TryGetComponent(out UpgradeSlot slot_Upgrade))
            {
                equipmentManager.EquipItem(index, DroppedItem.data);
                slot_Upgrade.ClearSlot();
            }
        }
    }
}
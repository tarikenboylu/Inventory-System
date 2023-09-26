using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static InventorySettings;
using TMPro;

public class InventorySlot : Slot
{
    public Inventory inventory;

    [SerializeField] private Image border;
    [SerializeField] private TextMeshProUGUI StackText;
    public bool isDraggable = true;
    public int index;

    /// <summary>
    /// Refresh UI
    /// </summary>
    public void RefreshSlot(ItemData _itemData, int Stack = 0)
    {
        base.RefreshSlot(_itemData);

        if (_itemData == null)
        {
            border.color = Color.clear;
            SetStackText(0);

            return;
        }

        border.color = RarityColor((Rarity)_itemData.rarity);
        SetStackText(Stack);
    }

    private void SetStackText(int amount) => StackText.text = amount > 1 ? amount.ToString() : "";

    public override void ClearSlot()
    {
        base.ClearSlot();
        SetStackText(0);
    }

    public override void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        if (eventData.pointerDrag.TryGetComponent(out Item DroppedItem))
        {
            if (DroppedItem.slot.TryGetComponent(out InventorySlot slot_Inventory))
            {
                if (slot_Inventory.inventory == inventory)
                    inventory.SwapSlots(slot_Inventory.index, index);
                else
                {
                    slot_Inventory.inventory.ClearSlot(index);
                    inventory.AddItemToSlot(DroppedItem.data, index);
                }
            }
            else if (DroppedItem.slot.TryGetComponent(out EquipmentSlot slot_Equipment))
            {
                inventory.AddItemToSlot(DroppedItem.data, index);
                slot_Equipment.ClearSlot();
            }
            else if (DroppedItem.slot.TryGetComponent(out UpgradeSlot slot_Upgrade))
            {
                inventory.AddItemToSlot(DroppedItem.data, index);
                slot_Upgrade.ClearSlot();
            }
        }
    }
}
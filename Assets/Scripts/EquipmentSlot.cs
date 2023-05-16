using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot : Slot
{
    private Item _item;
    public Item.ItemType slotType;

    public Item Item
    {
        get { return _item; }
        set
        {
            if (value.type == slotType)
                _item = value;
            else
                Debug.Log("Item type is not fit");
        }
    }

    public override void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            if (eventData.pointerDrag.TryGetComponent(out ItemSlotContainer DroppedItem))
            {
                if (DroppedItem.item.type == slotType)
                {
                    if (DroppedItem.slot.TryGetComponent(out InventorySlot _))
                    {
                        int index2 = DroppedItem.slot.transform.GetSiblingIndex();
                        int index1 = transform.GetSiblingIndex();
                        //PlayerDataBase.RemoveItem(Item , 1);
                    }
                    else if (DroppedItem.slot.TryGetComponent(out EquipmentSlot _))
                    {
                        int equipmentIndex = DroppedItem.slot.transform.GetSiblingIndex();
                        //PlayerDataBase.RemoveEquippedItem(equipmentIndex);
                    }
                    //PlayerDataBase.EquipItem();

                    Destroy(eventData.pointerDrag);
                    //Inventory.Instance.UpdateInventoryUI();
                }
                else
                {

                }
            }
        }
    }

    public override void InitSlot()
    {
        Debug.Log(Item.name + " equipped on " + slotType + " slot");
    }
}
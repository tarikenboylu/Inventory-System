using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : Slot
{
    private ItemSlotContainer container;

    public Inventory inventory;

    private IEnumerator Start()
    {
        if (container == null) container = Instantiate(InventorySystemManager.containerPrefab, transform);

        yield return new WaitForEndOfFrame();

        container.slot = this;
        inventory.SlotList[transform.GetSiblingIndex()] = container;
    }

    public override void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            Debug.Log(eventData.pointerDrag);

            if (eventData.pointerDrag.TryGetComponent(out ItemSlotContainer DroppedItem))
            {
                if (DroppedItem.slot.TryGetComponent(out InventorySlot _slot))
                {
                    int index2 = _slot.transform.GetSiblingIndex();
                    int index1 = transform.GetSiblingIndex();

                    Debug.Log(DroppedItem.item);

                    ItemContainerData tempData = container.GetContainerData();

                    inventory.SlotList[index1].OverrideSlot(DroppedItem.GetContainerData());
                    _slot.inventory.SlotList[index2].OverrideSlot(tempData);
                }
                /*else if (DroppedItem.slot.TryGetComponent(out EquipmentSlot _))
                {
                    int equipmentIndex = DroppedItem.slot.GetSiblingIndex();
                    //PlayerDataBase.RemoveEquippedItem(equipmentIndex);
                    //PlayerDataBase.AddItem(eventData.pointerDrag.GetComponent<InventoryItem>());
                }*/

                //Destroy(eventData.pointerDrag);
            }
        }
    }
}
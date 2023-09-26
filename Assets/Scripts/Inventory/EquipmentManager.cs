using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    [SerializeField] private EquipmentSlot[] equipmentSlots;
    private ItemData[] equipmentData;

    private void Awake() => equipmentData = new ItemData[equipmentSlots.Length];

    public void RemoveItem(int SlotIndex) => EquipItem(SlotIndex, null); 

    public void EquipItem(int SlotIndex, ItemData data)
    {
        equipmentData[SlotIndex] = data;
        equipmentSlots[SlotIndex].RefreshSlot(data);
        CalculateAttributes();
    }

    private void CalculateAttributes()
    {
        Debug.Log("Attributes recalculating...");
    }
}
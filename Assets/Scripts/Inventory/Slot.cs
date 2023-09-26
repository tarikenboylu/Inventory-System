using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    [SerializeField] protected Item item;
    [SerializeField] protected TextMeshProUGUI LevelText;

    public bool isEmpty => item == null;

    private void Awake()
    {
        if (item == null) item = Instantiate(InventorySystemManager.itemPrefab);
        item.slot = this;
    }

    public virtual void OnDrop(PointerEventData eventData) { }

    public void RefreshSlot(ItemData _itemData)
    {
        LevelText.text = _itemData != null ? "+" + _itemData.upgradeLevel.ToString() : "";
        item.Override(_itemData);
    }

    public virtual void ClearSlot()
    {
        RefreshSlot(null);
    }
}
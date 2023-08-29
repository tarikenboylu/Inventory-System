using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static InventorySettings;

[RequireComponent(typeof(CanvasGroup))]
public class ItemSlotContainer : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, /*IPointerDownHandler,*/ IPointerExitHandler
{
    [SerializeField] private Image border;
    /*[HideInInspector]*/ public InventorySlot slot;
    /*[HideInInspector]*/ public bool backToSlot;
    public int amount { get; private set; }
    public Item item { get; private set; }
    public TextMeshProUGUI amountText;
    public bool isDraggable = true;
    public bool Convenient => item.maxStackCount > amount && item.countable;
    public bool Equality(int id, Rarity rarity)
    {
        if (item == null) return false;

        return item.ID == id && item.rarity == rarity;
    }
    public ItemContainerData GetContainerData()
    {
        if(item == null) return null;
        else return new()
        {
            amount = this.amount,
            countable = item.countable,
            item = item != null ? item.GetItemData() : null
        };
    }

    private CanvasGroup canvas;

    private void Awake()
    {
        UpdateSlot();
        canvas = GetComponent<CanvasGroup>();
    }

    public void OverrideSlot(ItemContainerData data)
    {
        ClearSlot();

        if (data == null) return;

        item = Instantiate(InventorySystemManager.itemPrefab, transform);
        amount = data.amount;
        UpdateSlot();
    }

    public void AddToSlot(int _amount)
    {
        amount += _amount;
        UpdateSlot();
    }

    public void RemoveItem(int decrement)
    {
        if (decrement < amount)
            amount -= decrement;
        else if (decrement == amount)
            ClearSlot();
        else
            Debug.LogError("Logic Error : Not Enough items to delete in this slot");

        UpdateSlot();
    }

    /// <summary>
    /// Refresh UI
    /// </summary>
    private void UpdateSlot()
    {
        amountText.text = amount > 1 && item != null ? amount.ToString() : "";

        if (item == null)
        {
            border.color = Color.clear;
            return;
        }

        border.color = RarityColor(item.rarity);
    }

    public void ClearSlot()
    {
        if (item != null)
        {
            Destroy(item.gameObject);
            item = null;
        }

        amount = 0;
        UpdateSlot();
    }

    public void OnBeginDrag(PointerEventData _)
    {
        backToSlot = true;
        amountText.gameObject.SetActive(false);
        transform.SetParent(GameObject.Find("Canvas").transform);
        canvas.alpha = 0.6f;
        canvas.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        GetComponent<RectTransform>().anchoredPosition += eventData.delta / GameObject.Find("Canvas").GetComponent<Canvas>().scaleFactor;
    }

    public void OnEndDrag(PointerEventData _)
    {
        amountText.gameObject.SetActive(true);
        canvas.alpha = 1;
        canvas.blocksRaycasts = true;

        if (backToSlot) transform.SetParent(slot.transform);

        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

#region Info Window
    public void OnPointerEnter(PointerEventData _)
    {
        if (item != null)
        {
            CursorInfo.Initialize(item.type.ToString(), item.rarity, item.attributes);
            StartCoroutine(CursorInfo.TurnOnInfo(true));
        }
    }

    public void OnPointerExit(PointerEventData _) => StartCoroutine(CursorInfo.TurnOnInfo(false));
#endregion
}
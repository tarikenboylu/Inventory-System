using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image)), RequireComponent(typeof(CanvasGroup))]
public class Item : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, /*IPointerDownHandler,*/ IPointerExitHandler, IPointerEnterHandler
{
    public ItemData data { get; private set; }
    [SerializeField] private CanvasGroup ItemCanvas;
    [SerializeField] private Image itemImage;
    [HideInInspector] public Slot slot;
    [HideInInspector] public bool backToSlot;

    public void Override(ItemData _data)
    {
        if (_data == null)
        {
            data = null;
            itemImage.enabled = false;
            return;
        }

        itemImage.enabled = true;
        data = _data;
        itemImage.sprite = _data.sprite;
    }

    #region Drag-Drop

    public void OnBeginDrag(PointerEventData _)
    {
        backToSlot = true;
        transform.SetParent(GameObject.Find("Canvas").transform);
        ItemCanvas.alpha = 0.6f;
        ItemCanvas.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        GetComponent<RectTransform>().anchoredPosition += eventData.delta / GameObject.Find("Canvas").GetComponent<Canvas>().scaleFactor;
    }

    public void OnEndDrag(PointerEventData _)
    {
        ItemCanvas.alpha = 1;
        ItemCanvas.blocksRaycasts = true;

        if (backToSlot)
        {
            transform.SetParent(slot.transform);
            transform.SetSiblingIndex(1);
        }

        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    public void OnPointerEnter(PointerEventData _)
    {
        if (Input.GetMouseButton(0)) return;
        CursorInfo.Initialize(data);
        StartCoroutine(CursorInfo.TurnOnInfo(true));
    }

    public void OnPointerExit(PointerEventData _) => StartCoroutine(CursorInfo.TurnOnInfo(false));

    #endregion
}
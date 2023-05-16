using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class CursorInfo : MonoSingleton<CursorInfo>
{
    [SerializeField] private TextMeshProUGUI ItemNameHeader;
    [SerializeField] private Color CommonTextColor;
    [SerializeField] private Color UncommonTextColor;
    [SerializeField] private Color RareTextColor;
    [SerializeField] private Color UniqueTextColor;
    [SerializeField] private Color SetTextColor;
    [SerializeField] private Color LegendaryTextColor;

    private Rect panelRect => GetComponent<RectTransform>().rect;

    private static Color CommonItemColor;
    private static Color UncommonItemColor;
    private static Color RareItemColor;
    private static Color UniqueItemColor;
    private static Color SetItemColor;
    private static Color LegendaryItemColor;

    private static TextMeshProUGUI headerText;
    private static GameObject[] tempInfo;
    private static Transform InfoTransform;
    private static CanvasGroup canvas;

    private static string RarityColorHash(Item.Rarity rarity)
    {
        return rarity switch
        {
            Item.Rarity.Uncommon => ColorUtility.ToHtmlStringRGBA(UncommonItemColor),
            Item.Rarity.Rare => ColorUtility.ToHtmlStringRGBA(RareItemColor),
            Item.Rarity.Set => ColorUtility.ToHtmlStringRGBA(UniqueItemColor),
            Item.Rarity.Unique => ColorUtility.ToHtmlStringRGBA(SetItemColor),
            Item.Rarity.Legendary => ColorUtility.ToHtmlStringRGBA(LegendaryItemColor),
            _ => ColorUtility.ToHtmlStringRGBA(CommonItemColor)
        };
    }

    private void OnEnable()
    {
        CommonItemColor = CommonTextColor;
        UncommonItemColor = UncommonTextColor;
        RareItemColor = RareTextColor;
        UniqueItemColor = UniqueTextColor;
        SetItemColor = SetTextColor;
        LegendaryItemColor = LegendaryTextColor;

        canvas = GetComponent<CanvasGroup>();
        InfoTransform = transform;
        headerText = ItemNameHeader;
    }

    private void FixedUpdate()
    {
        if (GetComponent<CanvasGroup>().alpha > 0)
            transform.position = Input.mousePosition + Vector3.right * (10 + panelRect.width / 4) + Vector3.down * (2 + panelRect.height / 4);
    }
    
    public static IEnumerator TurnOnInfo(bool isOn, float _ShowOnTime = .6f)
    {
        float castTime = .5f;
        yield return new WaitForSeconds(castTime);

        float countDown = _ShowOnTime - castTime;

        while (countDown > 0)
        {
            canvas.alpha += (isOn ? Time.deltaTime : -Time.deltaTime) / (_ShowOnTime - castTime);
            countDown -= Time.deltaTime;

            yield return new WaitForSeconds(Time.deltaTime);
        }

        canvas.alpha = isOn ? 1 : 0;
        yield return null;
    }

    public static void Initialize(string ItemName, Item.Rarity rarity, Attribute[] attribute)
    {
        if (tempInfo != null)
            foreach (var info in tempInfo) Destroy(info);
        
        tempInfo = new GameObject[attribute.Length];

        Debug.Log(RarityColorHash(rarity));
        headerText.text = $"<color=#{RarityColorHash(rarity)}><b>{ItemName}</b></color>";

        for (int i = 0; i < attribute.Length; i++)
        {
            tempInfo[i] = Instantiate(InventorySystemManager.AttributeInfoPrefab.gameObject, InfoTransform);
            tempInfo[i].GetComponent<AttributeInfo>().Initialize(attribute[i]);
        }
    }
}
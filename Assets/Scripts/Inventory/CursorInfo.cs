using System.Collections;
using TMPro;
using UnityEngine;
using static InventorySettings;

public class CursorInfo : MonoSingleton<CursorInfo>
{
    [SerializeField] private TextMeshProUGUI ItemNameHeader;

    private static TextMeshProUGUI NameText;
    private static AttributeInfo[] tempInfo;
    private static Transform InfoTransform;
    private static CanvasGroup canvas;

    private static string RarityColorHash(Rarity rarity) => ColorUtility.ToHtmlStringRGBA(RarityColor(rarity));

    private void OnEnable()
    {
        transform.SetAsLastSibling();
        canvas = GetComponent<CanvasGroup>();
        InfoTransform = transform;
        NameText = ItemNameHeader;
    }

    private void FixedUpdate()
    {
        if (GetComponent<CanvasGroup>().alpha > 0) transform.position = Input.mousePosition;
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

    public static void Initialize(ItemData item)
    {
        if (tempInfo != null)
            foreach (var info in tempInfo) Destroy(info.gameObject);

        if (item.type == ItemType.OneHandWeapon || item.type == ItemType.TwoHandWeapon)
        {
            int minDamage = 0, maxDamage = 0;

            for (int i = 0; i < item.attributes.Length; i++)
            {
                if (item.attributes[i].type == Attribute.Type.MinDamage) minDamage = item.attributes[i].Value(item.upgradeLevel);
                if (item.attributes[i].type == Attribute.Type.MaxDamage) maxDamage = item.attributes[i].Value(item.upgradeLevel);
            }

            tempInfo = new AttributeInfo[item.attributes.Length - 1];

            tempInfo[item.attributes.Length - 2] = Instantiate(InventorySystemManager.AttributeInfoPrefab, InfoTransform);
            tempInfo[item.attributes.Length - 2].Edit("Damage", $"{minDamage}-{maxDamage}");
        }
        else
            tempInfo = new AttributeInfo[item.attributes.Length];

        NameText.text = $"<color=#{RarityColorHash((Rarity)item.rarity)}><b>{item.ItemTemplate.name} +{item.upgradeLevel}</b></color>";

        for (int i = 0, k = 0; i < item.attributes.Length; i++)
        {
            if (item.attributes[i].type == Attribute.Type.MinDamage || item.attributes[i].type == Attribute.Type.MaxDamage) continue;

            Attribute attribute = item.attributes[i];
            tempInfo[k] = Instantiate(InventorySystemManager.AttributeInfoPrefab, InfoTransform);
            tempInfo[k].Edit(attribute.type.ToString(), attribute.Value(item.upgradeLevel).ToString());
            k++;
        }
    }
}
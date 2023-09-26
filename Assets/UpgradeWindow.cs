using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using static InventorySettings;

public class UpgradeWindow : MonoBehaviour
{
    [SerializeField] private AttributeInfo attributeInfo;
    [SerializeField] private Transform layout;
    [SerializeField] private TextMeshProUGUI NameText;

    public void Display(ItemData item)
    {
        int itemLevel = item.upgradeLevel;

        if (item.type == ItemType.OneHandWeapon || item.type == ItemType.TwoHandWeapon)
        {
            int minDamage = 0, maxDamage = 0, minDamage2 = 0, maxDamage2 = 0;

            for (int i = 0; i < item.attributes.Length; i++)
            {
                if (item.attributes[i].type == Attribute.Type.MinDamage)
                {
                    minDamage = item.attributes[i].Value(itemLevel);
                    minDamage2 = item.attributes[i].Value(itemLevel + 1);
                }
                if (item.attributes[i].type == Attribute.Type.MaxDamage)
                {
                    maxDamage = item.attributes[i].Value(itemLevel);
                    maxDamage2 = item.attributes[i].Value(itemLevel + 1); 
                }
            }

            AttributeInfo DamageAttributeInfo = Instantiate(InventorySystemManager.AttributeInfoPrefab, layout);
            DamageAttributeInfo.Edit("Damage", $"{minDamage}-{maxDamage} --> {minDamage2}-{maxDamage2}");
        }


        NameText.text = $"<color=#{RarityColorHash((Rarity)item.rarity)}><b>{item.ItemTemplate.name} +{itemLevel}</b></color>";
        NameText.text += "<color=#fff> --> </color>";
        NameText.text += $"<color=#{RarityColorHash((Rarity)item.rarity)}><b>+{itemLevel + 1}</b></color>";

        for (int i = 0; i < item.attributes.Length; i++)
        {
            if (item.attributes[i].type == Attribute.Type.MinDamage || item.attributes[i].type == Attribute.Type.MaxDamage) continue;

            Attribute attribute = item.attributes[i];
            AttributeInfo tempInfo = Instantiate(InventorySystemManager.AttributeInfoPrefab, layout);

            string valueInfo = attribute.Value(itemLevel).ToString();

            if(attribute.Value(itemLevel) != attribute.Value(itemLevel + 1))
                valueInfo += "-->" + attribute.Value(itemLevel + 1).ToString();

            tempInfo.Edit(attribute.type.ToString(), valueInfo);
        }
    }
    private static string RarityColorHash(Rarity rarity) => ColorUtility.ToHtmlStringRGBA(RarityColor(rarity));

    public void Clear()
    {
        for (int i = layout.childCount - 1; i > 0; i--)
        {
            if (layout.GetChild(i) != NameText.transform)
                Destroy(layout.GetChild(i).gameObject);
        }

        NameText.text = string.Empty;
    }
}
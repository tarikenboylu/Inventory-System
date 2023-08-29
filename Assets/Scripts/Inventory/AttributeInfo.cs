using TMPro;
using UnityEngine;

public class AttributeInfo : MonoBehaviour
{
    //Cursor Info has as many as attributes that item has
    //This designs a attributes in cursor info window

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI valueText;

    [Header("Settings")]
    [SerializeField] private Color NormalItemColor = Color.white;
    [SerializeField] private Color SetItemColor = Color.green;
    [SerializeField] private Color UpgradableColor = Color.yellow;
    [SerializeField] private Color IncrementColor = Color.red;

    public void Initialize(Attribute attribute)
    {
        nameText.text = attribute.attribute.ToString();
        valueText.text = attribute.value.ToString();

        string hash = ColorUtility.ToHtmlStringRGBA(IncrementColor);

        if (attribute.upgradable)
        {
            string increment = attribute.upgradeIncrement == 0 ? attribute.upgradePercentIncrement.ToString() : attribute.upgradeIncrement.ToString();
            valueText.text += $"<color=#{hash}>(+{increment})</color>";
        }

        nameText.color = GetAttributeColor(attribute);
        valueText.color = GetAttributeColor(attribute);
    }

    private Color GetAttributeColor(Attribute attribute)
    {
        if(attribute == null) return NormalItemColor;

        if (attribute.activeWhile == Attribute.ActiveWhile.SetEquippedCount)
            return SetItemColor;
        else if (attribute.activeWhile == Attribute.ActiveWhile.SetEquipped)
            return SetItemColor;

        if(attribute.upgradable)
            return UpgradableColor;


        return NormalItemColor;
    }
}
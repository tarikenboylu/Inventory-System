using TMPro;
using UnityEngine;

public class AttributeInfo : MonoBehaviour
{
    //Cursor Info has as many as attributes that item has
    //This designs a attributes in cursor info window
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI valueText;

    public void Edit(string attribute, string value, Color color = default)
    {
        if (color == default) color = Color.white;

        nameText.text = attribute;
        valueText.text = value;
        nameText.color = color;
        valueText.color = color;
    }
}
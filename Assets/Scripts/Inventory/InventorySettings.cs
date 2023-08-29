using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System Settings", fileName = "Inventory System Custom Settings")]
public class InventorySettings : ScriptableObject
{
    private static InventorySettings _instance;
    public static InventorySettings Instance
    {
        get 
        { 
            if (_instance == null)
                _instance = Resources.Load("Inventory/Custom Settings") as InventorySettings;

            return _instance;
        }
    }

    [SerializeField, NonReorderable] private ItemRarity[] RarityBorderColors;
    public static Color RarityColor(Rarity rarity) => Instance.RarityBorderColors[(int)rarity].color;

    private void OnValidate()
    {
        ItemRarity[] temp = RarityBorderColors;
        RarityBorderColors = new ItemRarity[Enum.GetValues(typeof(Rarity)).Length];

        for (int i = 0; i < temp.Length; i++)
        {
            RarityBorderColors[i] = temp[i];
            RarityBorderColors[i].name = ((Rarity)i).ToString() + " Border Color";
        }
    }

    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Set,
        Unique,
        Legendary,
        Mythic
    }

    [Serializable]
    public class ItemRarity
    {
        [HideInInspector] public string name;
        public Color color;
    }
}
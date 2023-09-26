using System;
using UnityEngine;
using static InventorySettings;

[Serializable]
public class ItemData
{
    public Sprite sprite => ItemTemplate.itemSprite;
    public ItemType type => ItemTemplate.type;
    public bool upgradable => ItemTemplate.upgradable && upgradeLevel < 10;
    [field:SerializeField] public ItemTemplate ItemTemplate { get; private set; }
    [field:SerializeField] public int MaxStackCount { get; private set; }
    [field:SerializeField] public int MaxDurability { get; private set; }
    [field:SerializeField] public int Durability { get; private set; }
    [field:SerializeField] public int rarity { get; private set; }
    [field:SerializeField] public int upgradeLevel { get; private set; }
    [field:SerializeField] public int UniqueID { get; private set; }
    [field:SerializeField] public Attribute[] attributes { get; private set; }
    public void UpgradeLevel() => upgradeLevel++;

    public ItemData(ItemTemplate _template, int _MaxStackCount, int _MaxDurability, Rarity _rarity, int _upgradeLevel, int _UniqueID, Attribute[] _attributes)
    {
        ItemTemplate = _template;
        MaxStackCount = _MaxStackCount;
        MaxDurability = _MaxDurability;
        rarity = (int)_rarity; 
        upgradeLevel = _upgradeLevel;
        UniqueID = _UniqueID;
        attributes = _attributes;
        Durability = MaxDurability;
    }
}
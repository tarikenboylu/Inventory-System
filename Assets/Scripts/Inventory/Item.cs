using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ItemContainerData;
using static InventorySettings;

public class Item : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [field : SerializeField]
    public Attribute[] attributes { get; private set; }
    public int durability { get; private set; }
    public int upgradeLevel { get; private set; }
    public int ID { get; private set; }
    public Rarity rarity { get; private set; }
    public bool countable => template.countable;
    public bool upgradable => template.upgradable;
    public ItemType type => template.type;
    public int maxStackCount => template.maxStackCount;
    private ItemTemplate.ItemRarityTemplate template => InventorySystemManager.GetItemWithID(ID).RarityTemplate(rarity);

    public ItemData GetItemData() => new()
    {
        attributes = this.attributes,
        durability = this.durability,
        upgradeLevel = this.upgradeLevel,
        ID = this.ID,
        rarity = (int)this.rarity
    };

    public void OverrideItem(ItemData data)
    {
        attributes = data.attributes;
        durability = data.durability;
        upgradeLevel = data.upgradeLevel;
        ID = data.ID;
        rarity = (Rarity)data.rarity;
        UpdateItem();
    }

    private void UpdateItem()
    {
        GetComponent<Image>().color = template.itemUIColor;
        levelText.text = "+" + upgradeLevel;
    }

    public void Upgrade(int amount = 1)
    {
        upgradeLevel += amount;
        UpdateItem();
    }

    public enum ItemType
    {
        OneHandWeapon,
        TwoHandWeapon,
        Chest,
        Head,
        Boot,
        Gloves,
        Pants,
        Ring,
        Amulet,
        Necklace,
        Belt,
        Earring,
        Consumable,
        Quest
    }
}
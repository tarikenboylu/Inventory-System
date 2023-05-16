using UnityEngine;
public static class InventorySystemManager
{
    private static ItemTemplate[] itemTemplates => Resources.LoadAll<ItemTemplate>("Item Datas");
    public static ItemSlotContainer containerPrefab => Resources.Load<ItemSlotContainer>("Item Container");
    public static InventorySlot slotPrefab => Resources.Load<InventorySlot>("Slot");
    public static Item itemPrefab => Resources.Load<Item>("Empty Item");
    public static AttributeInfo AttributeInfoPrefab => Resources.Load<AttributeInfo>("AttributeInfoTemplate");
    private static Sprite[] borders => Resources.LoadAll<Sprite>("Item Border Sprites");
    
    public static Sprite GetBorder(Item.Rarity rarity) => borders[(int)rarity]; 
    public static ItemTemplate GetItemWithID(int _id)
    {
        foreach (ItemTemplate itemTemplate in itemTemplates)
            if(itemTemplate.ID == _id)
                return itemTemplate;

        return null;
    }

    public static ItemContainerData ConvertContainerData(ItemSlotContainer container)
    {
        return new()
        {
            amount = container.amount,
            item = new()
            {
                attributes = container.item.attributes,
                durability = container.item.durability,
                ID = container.item.ID,
                rarity = (int)container.item.rarity,
                upgradeLevel = container.item.upgradeLevel
            }
        };
    }
}
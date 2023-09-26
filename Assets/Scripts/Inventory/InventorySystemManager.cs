using UnityEngine;

public static class InventorySystemManager
{
    //private static ItemTemplate[] itemTemplates => Resources.LoadAll<ItemTemplate>("Inventory/Item Datas");
    public static Slot slotPrefab => Resources.Load<Slot>("Inventory/Prefabs/Inventory Slot");
    public static Item itemPrefab => Resources.Load<Item>("Inventory/Prefabs/Empty Item");
    public static AttributeInfo AttributeInfoPrefab => Resources.Load<AttributeInfo>("Inventory/Prefabs/AttributeInfoTemplate");
    public static Sprite BorderSprite => Resources.Load<Sprite>("Inventory/Sprites/InventoryItemBorder");

    //public static ItemContainerData ConvertContainerData(ItemSlotContainer container)
    //{
    //    return new()
    //    {
    //        amount = container.amount,
    //        item = new()
    //        {
    //            attributes = container.item.attributes,
    //            durability = container.item.durability,
    //            ID = container.item.ID,
    //            rarity = (int)container.item.rarity,
    //            upgradeLevel = container.item.upgradeLevel
    //        }
    //    };
    //}
}
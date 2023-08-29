using System;

[Serializable]
public class ItemContainerData
{
    public bool countable;
    public int amount;
    public ItemData item;

    [Serializable]
    public class ItemData
    {
        public int rarity;
        public int upgradeLevel;
        public int durability;
        public int ID;
        public Attribute[] attributes;
    }
}
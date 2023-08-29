using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attribute
{
    public Type attribute;
    public string description;
    public ActiveWhile activeWhile;
    public bool upgradable;
    public int upgradeIncrement;//How much increases as upgrading
    public int upgradePercentIncrement;//How much increases as upgrading percent type
    public int value;

    public enum ActiveWhile
    {
        Equipped,
        OnInventory,
        [Tooltip("Set Items Count")] SetEquippedCount,
        [Tooltip("Exact Items Equipped")] SetEquipped
        //Consumable
    }

    public enum Type 
    { 
        Strength,
        Dexterity,
        Weight,
        Armor,
        MagicResistance,
        Health,
        Mana,
        MinDamage,
        MaxDamage,
        Intelligence
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attribute
{
    [HideInInspector] public string description;
    public Type type;
    public ActiveWhile activeWhile;
    [SerializeField] private int upgradeIncrement;//How much increases as upgrading
    [SerializeField] private int upgradePercentIncrement;//How much increases as upgrading percent type
    private int value;
    public int BaseValue => value;
    public int UpgradeIncrement => upgradeIncrement;
    public int UpgradePercentIncrement => upgradePercentIncrement;

    public int Value(int ItemLevel) => value + upgradeIncrement * ItemLevel + upgradePercentIncrement * value * ItemLevel;

    public Attribute(int _value, Type _type, ActiveWhile _activeWhile, int _upgradeIncrement, int _upgradePercentIncrement)
    {
        value = _value;
        type = _type;
        activeWhile = _activeWhile;
        upgradeIncrement = _upgradeIncrement;
        upgradePercentIncrement = _upgradePercentIncrement;
    }

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
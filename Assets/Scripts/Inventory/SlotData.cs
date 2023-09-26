using System;

[Serializable]
public class SlotData
{
    public int stack { get; private set; }
    public ItemData item;
    public bool isMax => stack == item.MaxStackCount;

    /// <summary>
    /// Adds amount returns what remaining is
    /// </summary>
    /// <param name="amount">Returns remaining</param>
    public void AddStack(ref int amount)
    {
        if (amount + stack >= item.MaxStackCount)
        {
            int remaining = amount + stack - item.MaxStackCount;
            stack = item.MaxStackCount;
            amount = remaining;
            return;
        }

        stack += amount;
        amount = 0;
    }

    public void RemoveStack(ref int amount)
    {
        if (amount - stack > 0)
        {
            amount -= stack;
            stack = 0;
            return;
        }

        stack -= amount;
        amount = 0;
    }

    public SlotData(ItemData itemData, int _stack)
    {
        item = itemData;
        stack = _stack;
    }
}
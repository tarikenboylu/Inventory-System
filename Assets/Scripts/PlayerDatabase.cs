using UnityEngine;

public static class PlayerDataBase
{
    #region SaveSystem
    ////inventory save file name = "inventory.save"
    //public static void SavePlayerInventory()
    //{
    //    PlayerInventory playerInventory = new PlayerInventory { itemCounts = new int[80], itemNames = new string[80] };

    //    for (int i = 0; i < 80; i++)
    //    {
    //        if (playerItems != null)
    //        {
    //            if (playerItems[i].item != null)
    //                playerInventory.itemNames[i] = playerItems[i].item.name;
    //            else
    //                playerInventory.itemNames[i] = "";

    //            playerInventory.itemCounts[i] = playerItems[i].count;
    //        }
    //        else
    //        {
    //            Debug.Log("Saving as Empty Slot");
    //            playerInventory.itemNames[i] = "";
    //            playerInventory.itemCounts[i] = 0;
    //        }
    //    }

    //    SaveSystem.Save<PlayerInventory>(playerInventory, "inventory.save");
    //}

    ////inventory save file name = "inventory.save"
    //public static void LoadPlayerInventory()
    //{
    //    PlayerInventory data = SaveSystem.Load<PlayerInventory>("inventory.save");

    //    playerItems = new ItemContainer[80];

    //    for (int i = 0; i < playerItems.Length; i++)
    //        playerItems[i] = new ItemContainer(null, 0);


    //    if (data.itemNames != null && data.itemCounts != null)
    //        for (int i = 0; i < 80; i++)
    //        {
    //            if (ItemDataBase.GetItemWithName(data.itemNames[i]) != null)
    //                playerItems[i].item = ItemDataBase.GetItemWithName(data.itemNames[i]);
    //            else
    //                playerItems[i].item = null;
    //            playerItems[i].count = data.itemCounts[i];
    //        }
    //    else
    //        Debug.LogError("Data is Empty");
    //}


    //[System.Serializable]
    //public class PlayerInventory
    //{
    //    public string[] itemNames;
    //    public int[] itemCounts;
    //}
    #endregion
}
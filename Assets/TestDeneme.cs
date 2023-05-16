using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDeneme : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform slotParent;
    [SerializeField] private ItemTemplate template;

    void Start()
    {
        Item item = Instantiate(itemPrefab, slotParent).GetComponent<Item>();
        var t = template.CreateNewItem();
        item.OverrideItem(t);
    }

}
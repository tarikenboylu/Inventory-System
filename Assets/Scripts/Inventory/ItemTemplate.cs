using UnityEngine;
using UnityEngine.AddressableAssets;
using static Item;
using static InventorySettings;
using UnityEditor;

//[CreateAssetMenu(fileName = "New Item Template", menuName = "Item")]
public class ItemTemplate : ScriptableObject
{
    public Sprite itemSprite;
    public ItemType type;
    public GameObject worldPrefab;
    public Rarity rarity;

    public Color itemObjectColor;
    public Color itemUIColor;

    public int durability_Max;
    public int maxStackCount;

    public int price;
    public int MarketPrice;

    public Randomization<int>.ObjectPossibility[] levelDropChance;
    public int UpgradeLevel_Max;

    public bool upgradable;
    public bool storable;
    public bool sellable;

    public int possibleAttributesMinCount;
    public int possibleAttributesMaxCount;
    public AttributeTemplate[] possibleAttributes;
    public AttributeTemplate[] characteristicAttributes;

    public int[] upgradeCost;//Probably this should be static values

    public int UniqueId
    {
        get
        {
            return 0;
        }
    }

#if UNITY_EDITOR
    [UnityEditor.MenuItem("Assets/Create/ItemTemplate/ItemTemplate", priority = 10)]
    private static void CreateWithRandomName()
    {
        ItemTemplate template = UnityEditor.ObjectFactory.CreateInstance<ItemTemplate>();
        string directory = "Assets/Resources/Inventory/Item Datas";
        int currentIndex = Resources.LoadAll<ItemTemplate>("Inventory/Item Datas").Length;
        string assetName = $"NewAssetName{currentIndex}.asset";
        string path = System.IO.Path.Combine(directory, assetName);
        //template.id = currentIndex;
        //Debug.Log("Scriptable object is awaken " + template.id);
        AssetDatabase.CreateAsset(template, path);
    }
#endif

    public ItemData CreateNewItem()
    {
        int extraAttributesCount = Random.Range(possibleAttributesMinCount, possibleAttributesMaxCount + 1);
        int attributeCount = extraAttributesCount + characteristicAttributes.Length;

        Attribute[] newAttributes = new Attribute[attributeCount];

        for (int i = 0; i < characteristicAttributes.Length; i++)
            newAttributes[i] = characteristicAttributes[i].Attribute();

        //It can roll same attribute more than one times... 
        for (int i = 0; i < extraAttributesCount; i++)
        {
            int rollAttribute = Random.Range(0, possibleAttributes.Length);
            newAttributes[characteristicAttributes.Length + i] = possibleAttributes[rollAttribute].Attribute();
        }

        int _upgradeLevel = Randomization<int>.RandomObject(levelDropChance);

        Debug.Log($"Created new item {name} {UniqueId}");
        //Item exists count : ... playerprefs?

        return new ItemData(this, maxStackCount, durability_Max, rarity, _upgradeLevel, UniqueId, newAttributes);
    }

    [System.Serializable]
    public class AttributeTemplate
    {
        public int minValue;
        public int maxValue;
        [SerializeField] private Attribute attribute;

        public Attribute Attribute() => new (Random.Range(minValue, maxValue), attribute.type, 
            attribute.activeWhile, attribute.UpgradeIncrement, attribute.UpgradePercentIncrement);
    }
}
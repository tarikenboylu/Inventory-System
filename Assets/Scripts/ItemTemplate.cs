using UnityEngine;
using static Item;
using static ItemContainerData;

//[CreateAssetMenu(fileName = "New Item Template", menuName = "Item")]
public class ItemTemplate : ScriptableObject
{
    [SerializeField] private ItemType type;

    [NonReorderable]
    public Randomization<ItemRarityTemplate>.ObjectPossibility[] itemPatterns;
    public Sprite itemSprite;

    public int ID => id;
    public ItemRarityTemplate RarityTemplate(Rarity _rarity) => itemPatterns[(int)_rarity].possibleObject;

    private int id;

#if UNITY_EDITOR
    [UnityEditor.MenuItem("Assets/Create/ItemTemplate/ItemTemplate", priority = 10)]
    private static void CreateWithRandomName()
    {
        ItemTemplate template = UnityEditor.ObjectFactory.CreateInstance<ItemTemplate>();
        string directory = "Assets/Resources/Item Datas";
        int currentIndex = Resources.LoadAll<ItemTemplate>("Item Datas").Length;
        string assetName = $"NewAssetName{currentIndex}.asset";
        string path = System.IO.Path.Combine(directory, assetName);
        template.id = currentIndex;
        Debug.Log("Scriptable object is awaken " + template.id);
        UnityEditor.AssetDatabase.CreateAsset(template, path);
    }
#endif

    public ItemData CreateNewItem()
    {
        ItemRarityTemplate rareItem = Randomization<ItemRarityTemplate>.RandomObject(itemPatterns);

        int extraAttributesCount = Random.Range(rareItem.possibleAttributesMinCount, rareItem.possibleAttributesMaxCount);
        int attributeCount = extraAttributesCount + rareItem.characteristicAttributes.Length;

        Attribute[] newAttributes = new Attribute[attributeCount];
        
        for (int i = 0; i < rareItem.characteristicAttributes.Length; i++)
            newAttributes[i] = rareItem.characteristicAttributes[i].Attribute();

        //It can roll same attribute more than one times... 
        for (int i = 0; i < extraAttributesCount; i++)
        {
            int rollAttribute = Random.Range(0, rareItem.possibleAttributes.Length);
            newAttributes[rareItem.characteristicAttributes.Length + i] = rareItem.possibleAttributes[rollAttribute].Attribute();
        }

        int _durability = Random.Range(0, rareItem.durability_Max);
        int _upgradeLevel = Randomization<int>.RandomObject(rareItem.levelDropChance);
        Debug.Log($"Created new item {rareItem.rareName} {id}");
        //Item exists count : ... playerprefs?
        return new() { attributes = newAttributes, durability = _durability, ID = id, rarity = (int)rareItem.rarity, upgradeLevel = _upgradeLevel };
    }

    #region Editor Design...
    private void OnValidate()
    {
        int rarityCount = System.Enum.GetValues(typeof(Rarity)).Length;
        Randomization<ItemRarityTemplate>.ObjectPossibility[] objectPossibilities = new Randomization<ItemRarityTemplate>.ObjectPossibility[rarityCount];

        for (int i = 0; i < rarityCount; i++)
        {
            if (itemPatterns.Length > i) objectPossibilities[i] = itemPatterns[i];
            else
                objectPossibilities[i] = new() { possibleObject = new() };

            objectPossibilities[i].possibleObject.rarity = (Rarity)i;
            objectPossibilities[i].possibleObject.type = type;
            objectPossibilities[i].possibleObject.rareName = objectPossibilities[i].possibleObject.rarity.ToString() + " " + objectPossibilities[i].possibleObject.type.ToString();
            objectPossibilities[i].objectName = objectPossibilities[i].possibleObject.rarity.ToString();
        }

        itemPatterns = objectPossibilities;
    }
    #endregion

    [System.Serializable]
    public class ItemRarityTemplate
    {
        [HideInInspector] public string rareName;
        [HideInInspector] public ItemType type;
        public GameObject worldPrefab;
        public Sprite ItemSprite;
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
        public bool countable;
        public bool storable;
        public bool sellable;

        public int possibleAttributesMinCount;
        public int possibleAttributesMaxCount;
        public AttributeTemplate[] possibleAttributes;
        public AttributeTemplate[] characteristicAttributes;

        public int[] upgradeCost;//Probably it should be static values
    }

    [System.Serializable]
    public class AttributeTemplate
    {
        public int minValue;
        public int maxValue;
        [SerializeField] private Attribute attribute;

        public Attribute Attribute()
        {
            attribute.value = Random.Range(minValue, maxValue);
            return attribute;
        }
    }
}
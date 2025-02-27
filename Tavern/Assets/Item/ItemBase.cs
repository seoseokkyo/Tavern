using UnityEngine;

[System.Serializable]
public struct ItemData
{
    public string itemName;
    public Texture2D itemIcon;
    public MeshFilter itemMeshFilter;
    public MeshRenderer itemMesh;
    public int itemID;
    public string itemDescription;
}

[System.Serializable]
public class ItemBase
{
    // 실제 아이템의 형태

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public enum EItemType
    {
        UseAble,
        NoUseAble,
        //Equipment, // 일단 퀵슬롯에서 사용하는 형태로
        EItemTypeMax
    }

    public EItemType ItemType = EItemType.NoUseAble;

    public ItemData CurrentItemData;

    public void RandDataSet()
    {
        int Size = ItemManager.Instance.items.Count;

        int RandNum = Random.Range(0, Size);

        CurrentItemData = ItemManager.Instance.items[RandNum];
    }

    public void SetItemData(ItemData Data)
    {
        CurrentItemData = Data;
    }
}

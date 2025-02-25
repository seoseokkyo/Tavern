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
    public MonoBehaviour test;
}

[System.Serializable]
public class ItemBase : MonoBehaviour
{
    // ���� �������� ����

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public enum EItemType
    {
        UseAble,
        NoUseAble,
        //Equipment, // �ϴ� �����Կ��� ����ϴ� ���·�
        EItemTypeMax
    }

    public EItemType ItemType = EItemType.NoUseAble;

    public ItemData CurrentItemData;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

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

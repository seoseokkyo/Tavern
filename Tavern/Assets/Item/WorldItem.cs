using UnityEngine;

public class WorldItem : Interactable
{
    // Scene��� �÷��̾�� ��ȣ�ۿ��� ������ �������� ����

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public ItemBase item;

    public MeshFilter WorldItemMeshFilter;
    public MeshRenderer WorldItemMesh;

    // ���߿� �޽� �ݸ��� ������ ��� �� �� �����غ���

    public bool bRandSet = true;

    public string InitItemName;

    void Start()
    {
        WorldItemMeshFilter = GetComponent<MeshFilter>();
        WorldItemMesh = GetComponent<MeshRenderer>();

        if (bRandSet)
        {
            item.RandDataSet();

            item = ItemManager.Instance.CastItemType(item);
        }
        else
        {
            item = ItemBase.ItemBaseCreator.CreateItemBase(ItemManager.Instance.GetItemDataByName(InitItemName));
        }

        SetItem(item);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public override string GetInteractingDescription() { return item.CurrentItemData.itemDescription; }

    public override void Interact()
    {
        if (interactPlayer)
        {
            if (interactPlayer.PlayerInventory.AddItem(ref item))
            {
                item = null;

                Destroy(gameObject);
            }
        }
    }

    public void SetItem(ItemBase inputItem)
    {
        item = inputItem;

        if (item.CurrentItemData.ItemPrefab)
        {
            GameObject child = Instantiate(item.CurrentItemData.ItemPrefab);
            child.transform.SetParent(transform, false);

            child.SetActive(true);
        }
        else
        {
            WorldItemMeshFilter.sharedMesh = item.CurrentItemData.itemMeshFilter.sharedMesh;
            WorldItemMesh.sharedMaterials = item.CurrentItemData.itemMesh.sharedMaterials;
        }
    }
}

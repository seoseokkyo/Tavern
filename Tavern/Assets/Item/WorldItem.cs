using Unity.VisualScripting;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using WebSocketSharp;
using static UnityEditor.Progress;

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

            WorldItemMeshFilter.sharedMesh = item.CurrentItemData.itemMeshFilter.sharedMesh;
            WorldItemMesh.sharedMaterials = item.CurrentItemData.itemMesh.sharedMaterials;
        }
        else
        {
            item.SetItemData(ItemManager.Instance.GetItemDataByName(InitItemName));
            SetItem(item);
        }
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

        WorldItemMeshFilter.sharedMesh = item.CurrentItemData.itemMeshFilter.sharedMesh;
        WorldItemMesh.sharedMaterials = item.CurrentItemData.itemMesh.sharedMaterials;
    }
}

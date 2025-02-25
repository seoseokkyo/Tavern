using Unity.VisualScripting;
using UnityEngine;
using WebSocketSharp;

public class WorldItem : Interactable
{
    // Scene등에서 플레이어와 상호작용이 가능한 아이템의 형태

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public ItemBase item;

    public MeshFilter WorldItemMeshFilter;
    public MeshRenderer WorldItemMesh;

    bool bTestInit = false;

    void Start()
    {
        item = new ItemBase();

        item.RandDataSet();

        WorldItemMeshFilter = GetComponent<MeshFilter>();
        WorldItemMesh = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!bTestInit)
        {
            // Test
            if(ItemManager.Instance.bReady)
            {
                WorldItemMeshFilter.sharedMesh = item.CurrentItemData.itemMeshFilter.sharedMesh;
                WorldItemMesh.sharedMaterials = item.CurrentItemData.itemMesh.sharedMaterials;
                
                bTestInit = true;
            }
        }
    }
    public override string GetInteractingDescription() { return item.CurrentItemData.itemDescription; }

    public override void Interact()
    {
        Debug.Log($"Interact Item : {item.CurrentItemData.itemName}");

        item.RandDataSet();

        WorldItemMeshFilter.sharedMesh = item.CurrentItemData.itemMeshFilter.sharedMesh;
        WorldItemMesh.sharedMaterials = item.CurrentItemData.itemMesh.sharedMaterials;
    }
}

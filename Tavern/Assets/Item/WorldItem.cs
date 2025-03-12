using NUnit.Framework.Constraints;
using System.Collections.Generic;
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
        }
        else
        {
            item.SetItemData(ItemManager.Instance.GetItemDataByName(InitItemName));
        }

        item = ItemManager.Instance.CastItemType(item);
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
        if (inputItem == null)
        {
            Debug.Log("SetItem(ItemBase inputItem) Null Input");
        }

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

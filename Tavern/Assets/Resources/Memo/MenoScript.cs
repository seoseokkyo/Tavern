using NUnit.Framework;
using Photon.Pun;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenoScript : WorldItem
{
    private List<string> foods = new List<string>();
    private string extraNote;

    public UnityEngine.UI.Image icon;
    public TextMeshPro contentText;

    public bool isAttached = false;

    public Transform attachPoint;    
    public MemoReviewUI memoUI;

    public GameObject obj;

    public void TryAttachMemo(Vector3 attachPosition, Quaternion attachRotation)
    {
        if (!isAttached && interactPlayer != null)
        {
            isAttached = true;
            interactPlayer.CurrentPlayer.DetachMemoItemFromRightHand();  
            photonView.RPC("RPC_AttachMemoItem", RpcTarget.AllBuffered, attachPosition, attachRotation);
        }
    }

    [PunRPC]
    void RPC_AttachMemoItem(Vector3 pos, Quaternion rotation)
    {
        transform.position = pos;
        transform.rotation = rotation;
        isAttached = true;

        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    public void Initialize(List<string> _foods, string extras)
    {
        if (foods.Count > 0) return;

        foods = _foods;
        extraNote = extras;

        icon.sprite = ItemManager.Instance.GetItemSpriteByName(foods[0]);
        memoUI.Initialize(foods, extras);
        memoUI.enabled = true;
    }

    [PunRPC]
    public void RPC_InitializeMemoData(string serializedFoods, string extraNote)
    {
         if (string.IsNullOrEmpty(serializedFoods)) return;

        List<string> foods = new List<string>(serializedFoods.Split('|'));
        var memoData = ItemManager.Instance.GetItemDataByName("Memo");

        MemoItemBase baseItem = new MemoItemBase(memoData, foods, extraNote);

        item = baseItem;
        InitItemName = baseItem.CurrentItemData.itemName;

        if (item.CurrentItemData.ItemPrefab)
        {
            MeshObj = Instantiate(item.CurrentItemData.ItemPrefab);
            MeshObj.transform.SetParent(transform, false);

            MeshObj.SetActive(false);
        }
        else
        {
            WorldItemMeshFilter.sharedMesh = item.CurrentItemData.itemMeshFilter.sharedMesh;
            WorldItemMesh.sharedMaterials = item.CurrentItemData.itemMesh.sharedMaterials;
        }

        if (foods.Count > 0 && icon != null)
        {
            icon.sprite = ItemManager.Instance.GetItemSpriteByName(foods[0]);
        }

        if (memoUI != null)
        {
            memoUI.Initialize(foods, extraNote);
        }
    }
}

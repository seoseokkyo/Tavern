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

    public bool isAttaching = false;
    public bool isAttached = false;

    public Transform attachPoint;
    public MemoReviewUI memoUI;

    public void TryAttachMemo(Vector3 attachPosition)
    {
        if (!isAttached && interactPlayer != null)
        {
            isAttached = true;
            interactPlayer.CurrentPlayer.ItemDetachFromRightHand();

            photonView.RPC("RPC_AttachMemoItem", RpcTarget.AllBuffered, attachPosition);
        }
    }

    [PunRPC]
    void RPC_AttachMemoItem(Vector3 pos)
    {
        transform.position = pos;
        transform.rotation = Quaternion.identity;
        isAttached = true;
        isAttaching = false;
    }

    public void OpenReviewUI()
    {
        MemoReviewUI ui = FindObjectOfType<MemoReviewUI>();
        if (ui != null && item is MemoItemBase memoData)
        {
            ui.Initialize(memoData.orderedFoods, memoData.extraNote);
            ui.OpenUI();
        }
    }

    public void Initialize(List<string> _foods, string extras)
    {
        foods = _foods;
        extraNote = extras;

        icon.sprite = ItemManager.Instance.GetItemSpriteByName(foods[0]);
        memoUI.Initialize(foods, extras);
    }

    [PunRPC]
    public void RPC_InitializeMemoData(string serializedFoods, string extraNote)
    {
        List<string> foods = new List<string>(serializedFoods.Split('|'));

        var memoData = ItemManager.Instance.GetItemDataByName("Memo");
        MemoItemBase memoItemBase = new MemoItemBase(memoData, foods, extraNote);

        SetItem(memoItemBase);

        // UI 
        Initialize(foods, extraNote);
    }
}

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


    public bool isHolding = false;
    public bool isAttaching = false;
    public bool isAttached = false;

    public Transform attachPoint;
    public MemoReviewUI memoUI;

    private void Update()
    {
        if(isHolding)
        {
            if(Input.GetMouseButtonDown(0))
            {
                isAttaching = true;
                TryAttachMemoItem();
            }

            if (Input.GetMouseButtonDown(1))
            {
                ShowMemoUI();
            }

            if (Input.GetMouseButtonUp(1))
            {
                HideMemoUI();
            }
        }
    }

    void TryAttachMemoItem()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                // 부착할 지점이 충돌한 위치
                AttachMemoItem(hit.point);
            }
        }
    }

    public void AttachMemoItem(Vector3 attachPosition)
    {
        transform.position = attachPosition;
        transform.rotation = Quaternion.identity; 

        isAttached = true;
        isHolding = false;  

        photonView.RPC("RPC_AttachMemoItem", RpcTarget.All, attachPosition);
        interactPlayer.CurrentPlayer.ItemDetachFromRightHand();
    }

    [PunRPC]
    void RPC_AttachMemoItem(Vector3 attachPosition)
    {
        transform.position = attachPosition;
        transform.rotation = Quaternion.identity;
        isAttached = true;
        isAttaching = false;
        isHolding = false;
    }

    void ShowMemoUI()
    {
        if (memoUI != null)
        {
            memoUI.OpenUI();
        }
    }

    void HideMemoUI()
    {
        if (memoUI != null)
        {
            memoUI.CloseUI();
        }
    }

    public void Initialize(List<string> _foods, string extras)
    {
        foods = _foods;
        extraNote = extras;

        icon.sprite = ItemManager.Instance.GetItemSpriteByName(foods[0]);
        memoUI.Initialize(foods, extras);
    }
}

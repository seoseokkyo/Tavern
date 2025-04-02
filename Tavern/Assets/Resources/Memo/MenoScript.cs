using NUnit.Framework;
using Photon.Pun;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenoScript : Interactable
{
    private List<string> foods = new List<string>();
    private string extraNote;

    public UnityEngine.UI.Image icon;
    public TextMeshPro contentText;

    public bool isHolding = false;
    public bool isAttached = false;

    public Transform attachPoint;
    private Transform playerHand;

    public MemoReviewUI memoUI;

    private void Update()
    {
        if(isHolding)
        {
            if(Input.GetMouseButtonDown(0))
            {
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

    void AttachMemoItem(Vector3 attachPosition)
    {
        transform.position = attachPosition;
        transform.rotation = Quaternion.identity; 

        isAttached = true;
        isHolding = false;  

        photonView.RPC("RPC_AttachMemoItem", RpcTarget.All, attachPosition);
    }

    [PunRPC]
    void RPC_AttachMemoItem(Vector3 attachPosition)
    {
        transform.position = attachPosition;
        transform.rotation = Quaternion.identity;
        isAttached = true;
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

    public override string GetInteractingDescription()
    {
        return "It's a memo";
    }

    public override void Interact()
    {
        if (interactPlayer != null)
        {
            // 오른손 부착.. 
            isHolding = true;
            photonView.RPC("RPC_SetIsHolding", RpcTarget.All, true);
        }

        if(isAttached && !isHolding)
        {
            PickupMemoItem();
        }
    }

    [PunRPC]
    void RPC_SetIsHolding(bool holdingStatus)
    {
        isHolding = holdingStatus;
    }

    void PickupMemoItem()
    {
        isAttached = false;
        isHolding = true;

        transform.SetParent(playerHand);
        transform.position = playerHand.position;
        transform.rotation = playerHand.rotation;

        photonView.RPC("RPC_PickupMemoItem", RpcTarget.All);
    }

    [PunRPC]
    void RPC_PickupMemoItem()
    {
        isAttached = false;
        isHolding = true;
    }

}

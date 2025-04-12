using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum ECookType
{
    Grill,
    //Slice,    얘는 일단 따로 하는걸로
    Boiling,
    Fry,
    ECookTypeMax
}

[Serializable]
public class PairTransformWorldItem
{
    public Transform Item1;
    public WorldItem Item2;
}

public class CookInterationObj : Interactable
{
    public string InteractDescription;

    private WorldItem PlayerHandleWorldItem = null;

    private List<WorldItem> CookingItem = new List<WorldItem>();

    // 초당 증가시킬 Cook Value
    [HideInInspector]
    public float CookingPerSecond = 1;

    public ECookType type;

    public List<Transform> ItemSockets;

    [HideInInspector]
    public List<PairTransformWorldItem> TransformAndUseState = new List<PairTransformWorldItem>();

    public CurrentCookingIngredientsUI UI_Prefab;

    private CurrentCookingIngredientsUI UI_Instantiate = null;

    public override string GetInteractingDescription()
    {
        return InteractDescription;
    }

    public override void Interact()
    {
        PlayerHandleWorldItem = interactPlayer.CurrentPlayer.RightHandItem;

        if (null == PlayerHandleWorldItem)
        {
            // 안에 들어있는 애들 상태랑 꺼내기 버튼등이 있는 UI출력
            if (null != UI_Instantiate)
            {
                Destroy(UI_Instantiate);
            }

            UI_Instantiate = Instantiate(UI_Prefab);
            UI_Instantiate.transform.SetParent(interactPlayer.PlayerCanvas.transform, false);

            UI_Instantiate.InteractPlayer = interactPlayer;
            UI_Instantiate.CookObj = this;

            UI_Instantiate.SetData(type, CookingItem);
        }
        else
        {
            if (ItemSockets.Count <= CookingItem.Count)
            {
                // 최대 동시 조리개수 초과

                return;
            }

            photonView.RPC("ClientToAll_ItemInputSend", RpcTarget.All, PlayerHandleWorldItem.photonView.ViewID);

            interactPlayer.CurrentPlayer.RightHandItem = null;

            //interactPlayer.CurrentPlayer.ItemDetachFromRightHand();
        }
    }

    [PunRPC]
    public void ClientToAll_ItemInputSend(int ItemViewID)
    {
        PhotonView view = PhotonView.Find(ItemViewID);
        var FindItem = view.GetComponentInParent<WorldItem>();

        foreach (var Pos in TransformAndUseState)
        {
            if (null == Pos.Item2)
            {
                Pos.Item2 = FindItem;

                FindItem.GetComponent<Collider>().enabled = false;
                FindItem.transform.SetParent(gameObject.transform, false);
                FindItem.transform.localPosition = Pos.Item1.localPosition;
                FindItem.transform.localRotation = Pos.Item1.localRotation;
                FindItem.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                CookingItem.Add(FindItem);

                break;
            }
        }
    }

    [PunRPC]
    public void ClientToServer_RequestState()
    {
        photonView.RPC("ServerToClient_ResponseState", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer);
    }

    [PunRPC]
    public void ServerToClient_ResponseState(Player TargetPlayer)
    {
        List<int> ViewList = new();

        foreach(var temp in TransformAndUseState)
        {
            ViewList.Add(null != temp.Item2 ? temp.Item2.photonView.ViewID : 0);
        }

        photonView.RPC("ReceiveState", TargetPlayer, ViewList.ToArray());
    }

    [PunRPC]
    public void ReceiveState(int[] ViewList)
    {
        for(int i = 0; i < TransformAndUseState.Count; i++)
        {
            if (ViewList[i] != 0)
            {
                PhotonView view = PhotonView.Find(ViewList[i]);
                var FindItem = view.GetComponentInParent<WorldItem>();

                FindItem.GetComponent<Collider>().enabled = false;
                FindItem.transform.SetParent(gameObject.transform, false);
                FindItem.transform.localPosition = TransformAndUseState[i].Item1.localPosition;
                FindItem.transform.localRotation = TransformAndUseState[i].Item1.localRotation;
                FindItem.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                CookingItem.Add(FindItem);

                TransformAndUseState[i].Item2 = FindItem;
            }
        }
    }

    public void IngredientTakeOut(WorldItem TargetItem)
    {
        int Count = CookingItem.Count;

        for (int i = 0; i < Count; i++)
        {
            if (CookingItem[i] == TargetItem)
            {
                interactPlayer.CurrentPlayer.ItemAttachToRightHand(TargetItem);

                foreach (var Pos in TransformAndUseState)
                {
                    if (Pos.Item2 == CookingItem[i])
                    {
                        Pos.Item2 = null;
                        break;
                    }
                }

                CookingItem.RemoveAt(i);
                break;
            }
        }

        UI_Instantiate.SetData(type, CookingItem);
    }

    private void Awake()
    {

    }

    public void Start()
    {
        CookingItem.Clear();

        foreach (var Pos in ItemSockets)
        {
            PairTransformWorldItem temp = new PairTransformWorldItem();
            temp.Item1 = Pos;
            temp.Item2 = null;

            TransformAndUseState.Add(temp);
        }

        if(!PhotonNetwork.IsMasterClient)
        {
            PhotonManager.Instance.OnJoinedRoomEndDelegate -= ClientToServer_RequestState;
            PhotonManager.Instance.OnJoinedRoomEndDelegate += ClientToServer_RequestState;
        }
    }

    void FixedUpdate()
    {
        foreach (var item in CookingItem)
        {
            if (item == null)
            {
                continue;
            }

            var Ingredient = item.GetComponent<IngredientComp>();

            float Value = CookingPerSecond * Time.fixedDeltaTime;

            switch (type)
            {
                case ECookType.Grill:
                    Ingredient.AccumulateGrilledValue(Value);
                    break;
                case ECookType.Boiling:
                    Ingredient.AccumulateBoiledValue(Value);
                    break;
                case ECookType.Fry:
                    Ingredient.AccumulateFriedValue(Value);
                    break;
            }
        }
    }
}

using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using System;

public class MemoDummyScript : Interactable
{
    public GameObject memoUI;
    private GameObject spawnedUI;

    public override string GetInteractingDescription()
    {
        return "Press [E] to Write Memo";
    }

    public override void Interact()
    {
        // 누가 사용하고 있으면 다른 사용자가 사용하지 못하게 막음
        if (!photonView.IsMine && photonView.IsSceneView)
        {
            photonView.RequestOwnership();
        }

        ModeController modeController = interactPlayer.GetComponent<ModeController>();
        if (modeController != null)
        {
            modeController.SetMode(true);
        }

        if (spawnedUI == null)
        {
            //GameObject canvas = interactPlayer.PlayerCanvas.gameObject;
            spawnedUI = Instantiate(memoUI);
            CreatingMemoUI ui = spawnedUI.GetComponent<CreatingMemoUI>();
            ui.MemoDummy = this;
            ui.Init(interactPlayer);
        }
        else
        {
            spawnedUI.SetActive(true);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void CreateMemoItem(string[] foods, string extra)
    {
        photonView.RPC("RPC_CreateMemoItem", RpcTarget.AllBuffered, foods, extra);
    }

    [PunRPC]
    void RPC_CreateMemoItem(string[] foods, string extra)
    {
        Debug.Log("Called CreateMemoItem RPC");

        List<FoodSelect> selected = new List<FoodSelect>();
        foreach (string f in foods)
        {
            ItemData curData = FindItemData(f);
            FoodSelect cur = gameObject.AddComponent<FoodSelect>();
            cur.Initialize(curData);
            selected.Add(cur);
        }

        GameObject memoItem = Resources.Load<GameObject>("Memo/Memo");
        if (memoItem == null)
        {
            Debug.LogError("Memo prefab is null");
            return;
        }

        GameObject instance = Instantiate(memoItem);
        MenoScript memo = instance.GetComponent<MenoScript>();
        if (memo == null)
        {
            Debug.LogError("MenoScript component is null");
            return;
        }
        memo.Initialize(selected, extra);
        //Vector3 loc = spawnLoc.transform.up * 5;

        Vector3 loc = transform.up * 5;

        memo.transform.position = loc;
    }


    public ItemData FindItemData(string name)
    {
        return ItemManager.Instance.GetItemDataByName(name);
    }
}

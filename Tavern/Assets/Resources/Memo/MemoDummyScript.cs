using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using System;
using UnityEditor;

public class MemoDummyScript : Interactable
{
    public GameObject memoUI;
    private GameObject spawnedUI;

    public GameObject memoPrefab;
    WorldItem spawnedMemo;

    public override string GetInteractingDescription()
    {
        return "Press [E] to Write Memo";
    }

    public override void Interact()
    {
        // ���� ����ϰ� ������ �ٸ� ����ڰ� ������� ���ϰ� ����
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
        List<string> selectedFoods = new List<string>(foods);

        GameObject memoObj = PhotonNetwork.Instantiate("Memo/Memo", transform.position + Vector3.up * 2f, Quaternion.identity);

        MenoScript memoItem = memoObj.GetComponent<MenoScript>();
        if (memoItem != null)
        {
            string serializedFoods = string.Join("|", selectedFoods); 
            memoItem.photonView.RPC("RPC_InitializeMemoData", RpcTarget.AllBuffered, serializedFoods, extra);
        }
    }

    public ItemData FindItemData(string name)
    {
        return ItemManager.Instance.GetItemDataByName(name);
    }
}

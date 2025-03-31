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
}

using Photon.Pun;
using Photon.Realtime;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    bool bSteamInit = false;
    bool bPhotonInit = false;
    bool bPhotonRpcReady = false;

    public void Awake()
    {

    }

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        if(false == SteamAPI.Init())
        {
            bSteamInit = false;
        }
        else
        {
            bPhotonInit = PhotonNetwork.ConnectUsingSettings();
            bSteamInit = true;
        }
    }

    void Update()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("PhotonNetwork.IsConnected == FALSE | Retrying");

            bPhotonInit = PhotonNetwork.ConnectUsingSettings();
            Debug.Log($"Retrying Result == {bPhotonInit}");
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            // ���⼭ ��� ���� ������ ���̺� ���̺� ���� �̸��� ���� ����ID�� ��� ����
            {

            }

            // �������� ��� �� �������� ����� ��� �÷��̾� Kick
            int PlayerNum = PhotonNetwork.PlayerList.Length;

            for (int i = 0; i < PlayerNum; i++)
            {
                // ���� ������Ŭ���̾�Ʈ�� �����÷��̾ ����Ʈ�� ���� ��� ���� ����
                if (PhotonNetwork.LocalPlayer != PhotonNetwork.PlayerList[i])
                {
                    PhotonNetwork.CloseConnection(PhotonNetwork.PlayerList[i]);
                }
            }
        }
    }

    private TaskCompletionSource<bool> _readyTaskCompletionSource;

    public async Task<bool> PhotonRpcReadyCheckAsync(float timeout = 5f)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            return true;
        }

        _readyTaskCompletionSource = new TaskCompletionSource<bool>();

        photonView.RPC("CheckRpcReceive", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer.ActorNumber);

        StartCoroutine(CheckTimeout(timeout));

        return await _readyTaskCompletionSource.Task;
    }

    private IEnumerator CheckTimeout(float timeout)
    {
        yield return new WaitForSeconds(timeout);

        if (_readyTaskCompletionSource != null && !_readyTaskCompletionSource.Task.IsCompleted)
        {
            _readyTaskCompletionSource.TrySetResult(false);
        }
    }

    [PunRPC]
    private void CheckRpcReceive(int senderID)
    {
        Player targetPlayer = PhotonNetwork.CurrentRoom.GetPlayer(senderID);
        if (targetPlayer != null)
        {
            photonView.RPC("CheckRpcResponse", targetPlayer);
        }
    }

    [PunRPC]
    private void CheckRpcResponse()
    {
        if (_readyTaskCompletionSource != null && !_readyTaskCompletionSource.Task.IsCompleted)
        {
            bPhotonRpcReady = true;
            _readyTaskCompletionSource.TrySetResult(true);
        }
    }
}

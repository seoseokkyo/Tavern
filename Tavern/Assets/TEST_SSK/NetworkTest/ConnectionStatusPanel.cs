using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class ConnectionStatusPanel : MonoBehaviour
{
    private readonly string connectionStatusMessage = "Connection Status: ";

    [Header("UI References")]
    public TMP_Text ConnectionStatusText;

    private ClientState tempState = 0;

    public void Update()
    {
        if (PhotonNetwork.NetworkClientState != tempState)
        {
            Debug.Log($"{tempState} => {PhotonNetwork.NetworkClientState}");
            tempState = PhotonNetwork.NetworkClientState;
        }

        ConnectionStatusText.text = connectionStatusMessage + PhotonNetwork.NetworkClientState;
    }
}

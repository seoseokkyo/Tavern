using Photon.Pun;
using UnityEngine;

public class TavernGameManager : MonoBehaviourPunCallbacks
{
    public GameObject PlayerPrefab;

    public Vector3 SpawnPos = new Vector3();

    private string debugText = "";

    private bool bPhotonRpcReadyCheck = false;
    float RpcReadyTimeCheck = 0.0f;

    private void Awake()
    {
        DefaultPool pool = PhotonNetwork.PrefabPool as DefaultPool;
        if (pool != null && this.PlayerPrefab != null)
        {
            pool.ResourceCache.Add(PlayerPrefab.name, PlayerPrefab);
        }
    }

    void Start()
    {
        Debug.Log($"{PhotonNetwork.CurrentRoom.Name}");

        Debug.Log($"PhotonNetwork.InRoom : {PhotonNetwork.InRoom}");
        Debug.Log($"PhotonNetwork.InLobby : {PhotonNetwork.InLobby}");
        Debug.Log($"PhotonNetwork.IsMasterClient : {PhotonNetwork.IsMasterClient}");
        Debug.Log($"PhotonNetwork.IsConnected : {PhotonNetwork.IsConnected}");
        Debug.Log($"PhotonNetwork.IsConnectedAndReady : {PhotonNetwork.IsConnectedAndReady}");

        RpcReadyTimeCheck = Time.time;
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 20;
        style.normal.textColor = Color.black;

        // 디버그 정보 출력
        GUI.Label(new Rect(10, 10, 500, 200), debugText, style);
    }

    private void FixedUpdate()
    {
        if (!bPhotonRpcReadyCheck && !PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            PhotonRpcReadyCheck();
        }

        debugText = $"PhotonNetwork.InRoom : {PhotonNetwork.InRoom}\n" + $"PhotonNetwork.InLobby : {PhotonNetwork.InLobby}\n" + $"PhotonNetwork.IsMasterClient : {PhotonNetwork.IsMasterClient}\n" + $"PhotonNetwork.IsConnected : {PhotonNetwork.IsConnected}\n" + $"PhotonNetwork.IsConnectedAndReady : {PhotonNetwork.IsConnectedAndReady}\n";
    }
    void DelayedRequestInstantiate()
    {
        SpawnPos = new Vector3(960f, 540f, -1.38f);

        SpawnPos.x += Random.Range(0, 5);
        SpawnPos.y += Random.Range(0, 5);

        RequestInstantiate(PlayerPrefab.name, SpawnPos, Quaternion.identity);
    }

    void RequestInstantiate(string prefabName, Vector3 position, Quaternion rotation)
    {
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("[Client] Requesting Instantiate from Server...");
            photonView.RequestOwnership();

            photonView.RPC("InstantiateOnMaster", RpcTarget.MasterClient, prefabName, position, rotation);
        }
    }

    [PunRPC]
    void InstantiateOnMaster(string prefabName, Vector3 position, Quaternion rotation)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        Debug.Log($"[Server] Instantiating {prefabName} at {position}");
        PhotonNetwork.Instantiate(prefabName, position, rotation);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 마우스 위치 가져오기 (스크린 좌표)
            Vector3 mousePosition = Input.mousePosition;

            // Ray 생성
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            // Raycast 실행
            if (Physics.Raycast(ray, out hit))
            {
                // 월드 좌표 얻기
                Vector3 worldPosition = hit.point;
                Debug.Log("World Position: " + worldPosition);
                worldPosition.z = -1.38f;

                PhotonNetwork.Instantiate(PlayerPrefab.name, worldPosition, Quaternion.identity);
            }
        }
    }

    void PhotonRpcReadyCheck()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            return;
        }

        photonView.RPC("CheckRpcReceive", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer.ActorNumber);
    }

    [PunRPC]
    void CheckRpcReceive(int senderID)
    {
        Photon.Realtime.Player targetPlayer = PhotonNetwork.CurrentRoom.GetPlayer(senderID);
        if (targetPlayer != null)
        {
            photonView.RPC("CheckRpcResponse", targetPlayer);
        }
    }

    [PunRPC]
    void CheckRpcResponse()
    {
        if(bPhotonRpcReadyCheck)
        {
            return;
        }

        bPhotonRpcReadyCheck = true;

        RpcReadyTimeCheck = Time.time - RpcReadyTimeCheck;

        Debug.Log($"TacTime:{RpcReadyTimeCheck}");

        RpcReadyTimeCheck = 0.0f;

        // 게임 시작 후 RPC가 연결되면 최초 1회 실행하는 Start대용 영역
        DelayedRequestInstantiate();
    }
}

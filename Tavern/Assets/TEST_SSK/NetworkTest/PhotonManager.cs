using Photon.Pun;
using Photon.Realtime;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PhotonManager : MonoBehaviourPunCallbacks
{
    [HideInInspector]
    public bool bPhotonRpcReady = false;

    public delegate void OnJoinedRoomEnd();
    public OnJoinedRoomEnd OnJoinedRoomEndDelegate;

    public List<GameObject> prefabsToCache;

    [HideInInspector]
    public PlayerController CurrentLocalPlayer = null;

    //<< Single
    protected static bool p_EverInitialized = false;

    protected static PhotonManager p_instance;
    public static PhotonManager Instance
    {
        get
        {
            if (p_instance == null)
            {
                return new GameObject("PhotonManager").AddComponent<PhotonManager>();
            }
            else
            {
                return p_instance;
            }
        }
    }

    protected bool m_bInitialized = false;
    public static bool Initialized
    {
        get
        {
            return Instance.m_bInitialized;
        }
    }

    public void Awake()
    {
        if (p_EverInitialized)
        {
            Debug.Log("Tried to Initialize the PhotonManager twice in one session!");
            throw new System.Exception("Tried to Initialize the PhotonManager twice in one session!");
        }

        if (p_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        p_instance = this;


        DontDestroyOnLoad(gameObject);

        DefaultPool pool = PhotonNetwork.PrefabPool as DefaultPool;
        if (pool != null)
        {
            foreach (GameObject prefab in prefabsToCache)
            {
                if (!pool.ResourceCache.ContainsKey(prefab.name))
                {
                    pool.ResourceCache.Add(prefab.name, prefab);
                }
            }
        }
    }
    //<<

    private void Start()
    {
        if (p_instance != FindFirstObjectByType<PhotonManager>())
        {
            p_instance = FindFirstObjectByType<PhotonManager>();
        }

        SceneManager.sceneLoaded += OnSceneLoaded;

        if (SteamAPI.IsSteamRunning())
        {
            PhotonInit();
        }
    }

    void Update()
    {
        //if (!PhotonNetwork.IsConnected)
        //{
        //    PhotonNetwork.ConnectUsingSettings();
        //}
    }

    void PhotonInit()
    {
        // Photon Init
        PhotonNetwork.LocalPlayer.NickName = SteamFriends.GetPersonaName();

        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "hk";
        PhotonNetwork.PhotonServerSettings.DevRegion = "hk";

        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.EnableCloseConnection = true;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            // 여기서 모든 유저 데이터 세이브 세이브 파일 이름은 스팀 고유ID를 사용 예정
            {

            }

            // 마스터일 경우 방 종료기능을 대신해 모든 플레이어 Kick
            if (PhotonNetwork.IsMasterClient)
            {
                foreach (Player player in PhotonNetwork.PlayerList)
                {
                    if (player != PhotonNetwork.LocalPlayer)
                    {
                        KickPlayer(player);
                    }
                }
            }
        }
    }

    public void KickPlayer(Player targetPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("LoadMainMenu", targetPlayer);

            PhotonNetwork.CloseConnection(targetPlayer);
        }
    }

    [PunRPC]
    public void LoadMainMenu()
    {
        if (null != CurrentLocalPlayer && CurrentLocalPlayer.photonView.IsMine)
        {
            PhotonNetwork.Destroy(CurrentLocalPlayer.gameObject);
        }

        PhotonNetwork.LoadLevel("MainMenuScene");
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
            _readyTaskCompletionSource.TrySetResult(true);
        }
    }

    public override async void OnJoinedRoom()
    {
        bPhotonRpcReady = await PhotonRpcReadyCheckAsync();

        Debug.Log($"PhotonRpcReadyCheckAsync : {bPhotonRpcReady}");
        if (bPhotonRpcReady)
        {
            if (null != OnJoinedRoomEndDelegate)
            {
                OnJoinedRoomEndDelegate();
            }
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "MainMenuScene" && scene.name != "ManagerSpawnScene")
        {
            if (bPhotonRpcReady)
            {
                RequestInstantiatePlayer();
            }
            else
            {
                OnJoinedRoomEndDelegate += RequestInstantiatePlayer;
            }
        }
    }

    int Count = 0;

    void RequestInstantiatePlayer()
    {
        Count++;

        if (CurrentLocalPlayer != null)
        {
            Debug.Log($"Destroy_CurrentLocalPlayer : {CurrentLocalPlayer}, Call Count :{Count}");

            PhotonNetwork.Destroy(CurrentLocalPlayer.gameObject);
            CurrentLocalPlayer = null;
        }

        // 마음에는 안드는데 일단은
        var StartPoint = GameObject.Find("StartPoint");

        var PlayerObj = PhotonNetwork.Instantiate("Player", StartPoint.transform.position, Quaternion.identity);

        CurrentLocalPlayer = PlayerObj.GetComponent<PlayerController>();

        Debug.Log($"PhotomManager_CurrentLocalPlayer : {CurrentLocalPlayer}, Call Count :{Count}");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.Disconnect();

        PhotonInit();

        Debug.Log("OnDisconnected");
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("MainMenuScene");
    }
}

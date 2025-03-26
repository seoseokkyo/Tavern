using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;
using System;
using UnityEngine.SceneManagement;

public class TavernGameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    private PhotonManager PhotonManager;

    [HideInInspector]
    public PlayerController CurrentLocalPlayer = null;

    public Vector3 SpawnPos = new Vector3();

    public string debugText = "";

    private float StartTime;

    public int MinutePerDay = 10;

    private float SecondsPerDay;

    public enum EWorkState
    {
        Init,
        Start,
        Working,
        Calculating,
        CalculateEnd,
        EWorkStateMax
    };

    public EWorkState CurrentState = EWorkState.Init;

    // Sync Variables
    [SerializeField]
    private bool _TavernOpen = false;

    public bool TavernOpen
    {
        get { return _TavernOpen; }
        set { photonView.RPC("SetTavernOpenFlag", RpcTarget.MasterClient, value); }
    }

    [PunRPC]
    public void SetTavernOpenFlag(bool bOpen)
    {
        _TavernOpen = bOpen;
    }

    [SerializeField]
    private float CommonUseGold = 0.0f;

    [PunRPC]
    public void AccumulateGold(float fValue)
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            CommonUseGold += fValue;
        }
    }

    [SerializeField]
    private float PassedTime = 0.0f;

    [SerializeField]
    private int PassedDay = 0;
    //<<


    //<< Single
    protected static bool t_EverInitialized = false;

    protected static TavernGameManager t_instance;
    public static TavernGameManager Instance
    {
        get
        {
            if (t_instance == null)
            {
                return new GameObject("TavernGameManager").AddComponent<TavernGameManager>();
            }
            else
            {
                return t_instance;
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
    //<<

    private void Awake()
    {
        if (t_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        t_instance = this;

        if (t_EverInitialized)
        {
            throw new System.Exception("Tried to Initialize the TavernGameManager twice in one session!");
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        SecondsPerDay = MinutePerDay * 60;
        StartTime = Time.time;

        SceneManager.sceneLoaded += OnSceneLoaded;
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
        //debugText = $"PhotonNetwork.InRoom : {PhotonNetwork.InRoom}\n" + $"PhotonNetwork.InLobby : {PhotonNetwork.InLobby}\n" + $"PhotonNetwork.IsMasterClient : {PhotonNetwork.IsMasterClient}\n" + $"PhotonNetwork.IsConnected : {PhotonNetwork.IsConnected}\n" + $"PhotonNetwork.IsConnectedAndReady : {PhotonNetwork.IsConnectedAndReady}\n";

        //debugText += $"\nTavernOpen : {TavernOpen}";

        if (!PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            MakeDailyResultDebugString(PassedDay - 1, out float UsedPrice);
        }

        debugText = $"Day : {PassedDay}\n WorkTime : {PassedTime}\n CurrentWorkState : {CurrentState}\n CommonUseGold : {CommonUseGold}\n Result : {DebugResult}";
    }


    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if (PassedTime >= SecondsPerDay)
            {
                CurrentState = EWorkState.Calculating;

                CalculateResult();

                PassedTime = 0.0f;

                PassedDay++;
            }

            if (CurrentState == EWorkState.Init)
            {
                DailyResultManager.Instance.ClearDailyUsedItem();

                CurrentState = EWorkState.Start;
            }
            else if (CurrentState == EWorkState.Working)
            {
                PassedTime += Time.time - StartTime;
            }
        }
    }

    string DebugResult = "";
    private void CalculateResult()
    {
        DailyResultManager.Instance.CalculateResult(PassedDay);

        MakeDailyResultDebugString(PassedDay, out float UsedPrice);

        CommonUseGold -= UsedPrice;

        CurrentState = EWorkState.CalculateEnd;
    }

    public void MakeDailyResultDebugString(int Day, out float UsedPrice)
    {
        DebugResult = "";

        DailyTotalResult Result;
        if (DailyResultManager.Instance.GetDailyTotalResultAt(Day, out Result))
        {
            DebugResult += $"Day : {Result.ResultDay}, DailyUsedCost : {Result.UsedItemPrice}, UseList : ";

            foreach (var item in Result.UsedItemSpecifications)
            {
                DebugResult += $"[{item.ItemName}/{item.Count}]";
            }
        }

        UsedPrice = Result.UsedItemPrice;
    }

    public void UserCheckedDailyResult()
    {
        CurrentState = EWorkState.Init;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "MainMenuScene" && scene.name != "ManagerSpawnScene")
        {
            if (PhotonManager.Instance.bPhotonRpcReady)
            {
                RequestInstantiatePlayer();
                RequestInstantiateResultManager();

                if (!PhotonNetwork.LocalPlayer.IsMasterClient)
                {
                    RequestDailyResultData();
                }
            }
            else
            {
                PhotonManager.Instance.OnJoinedRoomEndDelegate += RequestInstantiatePlayer;
                PhotonManager.Instance.OnJoinedRoomEndDelegate += RequestInstantiateResultManager;

                if (!PhotonNetwork.LocalPlayer.IsMasterClient)
                {
                    PhotonManager.Instance.OnJoinedRoomEndDelegate += RequestDailyResultData;
                }
            }

            // 호스트의 공통데이터 받아올것(GameDataInitialize)

            if (PhotonNetwork.IsMasterClient)
            {
                PassedDay = 0;
                PassedTime = 0;
                CommonUseGold = 0;
            }
        }
    }

    void RequestDailyResultData()
    {
        DailyResultManager.Instance.ReqSyncResultData();
    }

    void RequestInstantiateResultManager()
    {
        //var ResultManagerObj = PhotonNetwork.Instantiate("ResultManager", transform.position, Quaternion.identity);
    }

    void RequestInstantiatePlayer()
    {
        if (CurrentLocalPlayer != null)
        {
            Debug.Log($"Destroy_CurrentLocalPlayer : {CurrentLocalPlayer}");

            PhotonNetwork.Destroy(CurrentLocalPlayer.gameObject);
            CurrentLocalPlayer = null;
        }

        // 마음에는 안드는데 일단은
        var StartPoint = GameObject.Find("StartPoint");

        var PlayerObj = PhotonNetwork.Instantiate("Player", StartPoint.transform.position, Quaternion.identity);

        CurrentLocalPlayer = PlayerObj.GetComponent<PlayerController>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_TavernOpen);
            stream.SendNext(CommonUseGold);
            stream.SendNext(PassedTime);
            stream.SendNext(PassedDay);
        }
        else
        {
            _TavernOpen = (bool)stream.ReceiveNext();
            CommonUseGold = (float)stream.ReceiveNext();
            PassedTime = (float)stream.ReceiveNext();
            PassedDay = (int)stream.ReceiveNext();
        }
    }

    public void ClientToServerUseItem(string ItemName, int Count)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            UsedItem(ItemName, Count);
        }
        else
        {
            photonView.RPC("UsedItem", RpcTarget.MasterClient, ItemName, Count);
        }
    }

    [PunRPC]
    public void UsedItem(string ItemName, int Count)
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            DailyResultManager.Instance.UsedItem(ItemName, Count);
        }
    }

    public void TestFunction_PassedTimeToLimit()
    {
        PassedTime = SecondsPerDay;
    }
}

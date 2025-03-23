using Photon.Pun;
using UnityEngine;

public class TavernGameManager : MonoBehaviourPunCallbacks
{
    public PhotonManager PhotonManager;

    public Vector3 SpawnPos = new Vector3();

    public string debugText = "";

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
        debugText = $"PhotonNetwork.InRoom : {PhotonNetwork.InRoom}\n" + $"PhotonNetwork.InLobby : {PhotonNetwork.InLobby}\n" + $"PhotonNetwork.IsMasterClient : {PhotonNetwork.IsMasterClient}\n" + $"PhotonNetwork.IsConnected : {PhotonNetwork.IsConnected}\n" + $"PhotonNetwork.IsConnectedAndReady : {PhotonNetwork.IsConnectedAndReady}\n";
    }


    // Update is called once per frame
    void Update()
    {

    }
}

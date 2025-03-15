using Steamworks;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using System.Net;
using UnityEngine.Windows;
using System.IO;
using System.Text;
using Unity.Networking.Transport.Relay;
using Unity.VisualScripting;

public class TestUI : MonoBehaviour
{
    public SteamManager steamManager;
    public ScrollRect scrollRect;
    public SteamManager.RoomInfoStruct currentLobbyInfo;
    public TMP_InputField lobbyNameInput;
    public GameObject roomListUI;
    public Transform contentTransform;
    public int lastSelectedIndex;
    public CustomNetworkManager netManager;

    // Callback Funcs
    private Callback<LobbyMatchList_t> lobbyListCallback;
    private Callback<LobbyCreated_t> lobbyCreatedCallback;
    protected Callback<LobbyEnter_t> m_LobbyEnterCallback;
    private Callback<LobbyChatUpdate_t> m_LobbyChatUpdate;

    private string logFilePath;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // �ݹ� ���
        lobbyListCallback = Callback<LobbyMatchList_t>.Create(OnLobbyListReceived);
        lobbyCreatedCallback = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        m_LobbyEnterCallback = Callback<LobbyEnter_t>.Create(OnLobbyEnter);
        m_LobbyChatUpdate = Callback<LobbyChatUpdate_t>.Create(OnLobbyChatUpdate);

        StartCoroutine(ExecuteFunctionEveryXSeconds(2f, 3f));

        lobbyNameInput.onEndEdit.AddListener(OnSteamIDEntered);

        m_lobbyList.Clear();

        //<< Log
        logFilePath = Path.Combine(Application.persistentDataPath, "game_log.txt");

        Debug.Log($"Log Path : {logFilePath}");

        //GetExternalIP();
    }

    public string GetExternalIP()
    {
        using (WebClient webClient = new WebClient())
        {
            string externalIp = webClient.DownloadString("http://checkip.amazonaws.com").Trim();
            Debug.Log(externalIp);
            return externalIp;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (steamManager == null)
        {
            Debug.Log("SteamManager Is null");
        }
    }


    List<GameObject> m_lobbyList = new List<GameObject>();

    // �κ� ��� ���� �ݹ� �Լ�
    private void OnLobbyListReceived(LobbyMatchList_t callbackData)
    {
        //Debug.Log($"�κ� ��� ����, �� {callbackData.m_nLobbiesMatching}���� �κ� �ֽ��ϴ�.");

        //Debug.Log($"lobbyList Count : {m_lobbyList.Count}");

        for (int i = m_lobbyList.Count - 1; i >= 0; i--)
        {
            GameObject lobby = m_lobbyList[i];

            //Debug.Log($"lobby remove Index : {i}, lobby id : {lobby.name}");

            m_lobbyList.RemoveAt(i);
            Destroy(lobby);
        }

        // �κ� ��� ��ȸ
        for (int i = 0; i < callbackData.m_nLobbiesMatching; i++)
        {
            CSteamID lobbyID = SteamMatchmaking.GetLobbyByIndex(i);
            string lobbyName = SteamMatchmaking.GetLobbyData(lobbyID, "name");
            //Debug.Log($"�κ� {i}: {lobbyName} (ID: {lobbyID})");

            GameObject prefab = Instantiate(roomListUI);

            Debug.Log(prefab.name);

            RoomInfoUI roomInfo = prefab.GetComponent<RoomInfoUI>();

            m_lobbyList.Add(prefab);

            Debug.Log(roomInfo.name);

            CSteamID lobbyOwner = SteamMatchmaking.GetLobbyOwner(lobbyID);

            //SteamMatchmaking.GetLobbyMemberData(currentLobbyName, lobbyOwner, name);

            int iDataCount = SteamMatchmaking.GetLobbyDataCount(lobbyID);

            //SteamMatchmaking.GetLobbyData();
            int iCurrentMemeberNum = SteamMatchmaking.GetNumLobbyMembers(lobbyID);
            int iMemberLimit = SteamMatchmaking.GetLobbyMemberLimit(lobbyID);

            // Persona �̸� �������� (����Ʈ ���� ��ȯ)
            byte[] nameBytes = System.Text.Encoding.Default.GetBytes(SteamFriends.GetFriendPersonaName(lobbyOwner));
            string ownerPersonaName = System.Text.Encoding.UTF8.GetString(nameBytes);

            roomInfo.InitData(new SteamManager.RoomInfoStruct(i + 1, lobbyName, lobbyID.ToString(), ownerPersonaName, iCurrentMemeberNum, iMemberLimit, contentTransform));
        }
    }

    // �κ� ��� ���� ���� �ڵ鷯
    private void OnLobbyChatUpdate(LobbyChatUpdate_t callbackData)
    {
        // ����ڰ� �κ� ���� ���
        if (callbackData.m_rgfChatMemberStateChange == (uint)EChatMemberStateChange.k_EChatMemberStateChangeEntered)
        {
            Debug.Log($"New member entered the lobby: {callbackData.m_ulSteamIDUserChanged}");

            System.IO.File.AppendAllText(logFilePath, "\n" + callbackData.m_ulSteamIDUserChanged.ToString());            

            CSteamID steamIDUserChanged = new CSteamID(callbackData.m_ulSteamIDUserChanged);

            ulong.TryParse(currentLobbyInfo.roomID, out ulong steamId);

            if (SteamUser.GetSteamID() == SteamMatchmaking.GetLobbyOwner(new CSteamID(steamId)))
            {
                System.IO.File.AppendAllText(logFilePath, "\n" + SteamUser.GetSteamID().ToString() + " Is Host");

                //// P2P ���� ���¸� �����ɴϴ�.
                //P2PSessionState_t sessionState;

                //SteamNetworking.CreateP2PConnectionSocket(steamIDUserChanged, 0, 0, true);

                //bool isConnected = SteamNetworking.GetP2PSessionState(steamIDUserChanged, out sessionState);

                //if (isConnected)
                //{
                //    System.IO.File.AppendAllText(logFilePath, "\n" + steamIDUserChanged.ToString() + " Connected");

                //    Debug.Log($"P2P Session State for {steamIDUserChanged}:");
                //    Debug.Log($"Connected: {sessionState.m_bConnectionActive}");
                //    Debug.Log($"Using Relay: {sessionState.m_bUsingRelay}");
                //}
                //else
                //{
                //    System.IO.File.AppendAllText(logFilePath, "\n" + steamIDUserChanged.ToString() + " Connect Failed");

                //    Debug.Log("No active P2P session found for this SteamID.");
                //}
            }

        }
        // ����ڰ� �κ� ���� ���
        else if (callbackData.m_rgfChatMemberStateChange == (uint)EChatMemberStateChange.k_EChatMemberStateChangeLeft)
        {
            Debug.Log($"Member left the lobby: {callbackData.m_ulSteamIDUserChanged}");
        }
    }

    public void OnbnClickCreateButton()
    {
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, 4);


        if (SteamAPI.IsSteamRunning())
        {
            //// << ��Ƽ��
            //Debug.Log("SteamFriends.GetPersonaName : " + SteamFriends.GetPersonaName());

            //// << k_EPersonaStateOnline ���Ҹ��� ��?�� ���ϴ³������ ã�� �� ��
            //Debug.Log("SteamFriends.GetPersonaState : " + SteamFriends.GetPersonaState());

            //// << �̰� �׳� 1�� �����µ� ���ϴ³������ ã�� �� ��
            ////Debug.Log("SteamAPI.GetHSteamUser : " + SteamAPI.GetHSteamUser());
            ////Debug.Log("SteamAPI.GetHSteamPipe : " + SteamAPI.GetHSteamPipe());


            //Debug.Log("SteamUser.GetSteamID : " + SteamUser.GetSteamID());

            ////
            //SteamAPICall_t handle = SteamFriends.GetFollowerCount(SteamUser.GetSteamID());

            ////
            //Debug.Log("SteamFriends.GetFollowerCount : " + SteamFriends.GetFollowerCount(SteamUser.GetSteamID()));

            //Debug.Log("SteamMatchmaking.RequestLobbyList() : " + SteamMatchmaking.RequestLobbyList().ToString());
            //SteamFriends.GetPlayerNickname();
            //SteamFriends.HasFriend();

            //// ģ�� �� ��������
            //int friendCount = SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate);
            //Debug.Log($"ģ�� ��: {friendCount}");

            //for (int i = 0; i < friendCount; i++)
            //{
            //    // ģ�� ID ��������
            //    CSteamID friendSteamID = SteamFriends.GetFriendByIndex(i, EFriendFlags.k_EFriendFlagImmediate);
            //    EPersonaState friendState = SteamFriends.GetFriendPersonaState(friendSteamID);
            //    string friendName = SteamFriends.GetFriendPersonaName(friendSteamID);


            //    //if (friendState == EPersonaState.k_EPersonaStateOnline)
            //    //{
            //    //    string friendName = SteamFriends.GetFriendPersonaName(friendSteamID);
            //    //    Debug.Log($"�¶��� ģ��: {friendName} (SteamID: {friendSteamID.m_SteamID})");

            //    //}
            //}

            SteamMatchmaking.RequestLobbyList();
        }
    }


    // �κ� ���� �� �ݹ� �Լ�
    private void OnLobbyCreated(LobbyCreated_t callbackData)
    {
        if (callbackData.m_eResult == EResult.k_EResultOK)
        {
            Debug.Log("�κ� ���� ����!");

            CSteamID lobbyID = new CSteamID(callbackData.m_ulSteamIDLobby);

            currentLobbyInfo.roomID = lobbyID.ToString();
            string lobbyName = "My Game Lobby";

            // �κ� �̸� ����
            SteamMatchmaking.SetLobbyData(lobbyID, "name", lobbyName);
            Debug.Log($"�κ� ID: {lobbyID}, �̸�: {lobbyName}");

        }
        else
        {
            Debug.LogError("�κ� ���� ����: " + callbackData.m_eResult);
        }
    }

    // �Էµ� Steam ID ���� ó��
    void OnSteamIDEntered(string input)
    {
        if (ulong.TryParse(input, out ulong steamId))
        {
            // ��ȿ�� Steam ID (64��Ʈ ����)���, �̸� CSteamID�� ��ȯ
            //currentLobbyName = new CSteamID(steamId);
            //Debug.Log("�Է��� Steam ID: " + currentLobbyName);
        }
        else
        {
            Debug.LogError("��ȿ���� ���� Steam ID�Դϴ�.");
        }
    }


    public void OnbnClickJoinButton()
    {
        //Debug.Log($"Selected Index? : [{lastSelectedIndex}]");

        RoomInfoUI roomInfo = m_lobbyList[lastSelectedIndex].GetComponent<RoomInfoUI>();

        //Debug.Log($"Room Num : {roomInfo.roomInfo.roomNumber}, Room Host Name : {roomInfo.roomInfo.hostName}, Room User : {roomInfo.roomInfo.currentUserNum} / {roomInfo.roomInfo.roomUserLimit}");

        currentLobbyInfo = roomInfo.roomInfo;

        ulong.TryParse(currentLobbyInfo.roomID, out ulong steamID64);
        SteamMatchmaking.JoinLobby(new CSteamID(steamID64));
    }

    public void OnLobbyEnter(LobbyEnter_t pCallback)
    {
        if (pCallback.m_EChatRoomEnterResponse == (int)EChatRoomEnterResponse.k_EChatRoomEnterResponseSuccess)
        {
            Debug.Log("Successfully joined the lobby!");

            ulong.TryParse(currentLobbyInfo.roomID, out ulong steamID64);
            if (SteamUser.GetSteamID() != SteamMatchmaking.GetLobbyOwner(new CSteamID(steamID64)))
            {
                SteamNetworkingIdentity hostUser = new SteamNetworkingIdentity();
                hostUser.SetSteamID(SteamMatchmaking.GetLobbyOwner(new CSteamID(pCallback.m_ulSteamIDLobby)));

                netManager.ConnectToHostSteamP2P(hostUser.GetSteamID());
            }
        }
        else
        {
            Debug.LogError($"Failed to join the lobby : {EChatRoomEnterResponse.k_EChatRoomEnterResponseSuccess}");
        }
    }

    public void OnbnClickLeaveLobbyButton()
    {
        // ���ڿ��� ulong���� ��ȯ
        if (ulong.TryParse(currentLobbyInfo.roomID, out ulong steamID64))
        {
            SteamMatchmaking.LeaveLobby(new CSteamID(steamID64));
        }
    }

    public void OnbnClickCheckMyLobby()
    {
        if (ulong.TryParse(currentLobbyInfo.roomID, out ulong steamID64))
        {
            CSteamID roomID = new CSteamID(steamID64);

            CSteamID lobbyOwner = SteamMatchmaking.GetLobbyOwner(roomID);
            //SteamMatchmaking.GetLobbyMemberData(currentLobbyName, lobbyOwner, name);
            int iDataCount = SteamMatchmaking.GetLobbyDataCount(roomID);

            //SteamMatchmaking.GetLobbyData();
            int iCurrentMemeberNum = SteamMatchmaking.GetNumLobbyMembers(roomID);
            int iMemberLimit = SteamMatchmaking.GetLobbyMemberLimit(roomID);

            Debug.Log($"Lobby Name : {roomID}, Owner Name : {lobbyOwner}, Data Count : {iDataCount}, User : {iCurrentMemeberNum}/{iMemberLimit}");
        }
    }

    // ���� �ð����� ����Ǵ� �ڷ�ƾ �Լ�
    IEnumerator ExecuteFunctionEveryXSeconds(float delay, float repeatRate)
    {
        // ó�� ���� ���� ������ ����
        yield return new WaitForSeconds(delay);

        while (true)
        {
            // �ֱ������� �Լ� ����
            Debug.Log("�Լ��� ����Ǿ����ϴ�.");

            // �κ� ��� ��û
            SteamMatchmaking.RequestLobbyList();

            yield return new WaitForSeconds(repeatRate);
        }
    }

    // �ڷ�ƾ �����ϱ�
    void StopCoroutineFunction()
    {
        StopCoroutine("ExecuteFunctionEveryXSeconds");
    }
}

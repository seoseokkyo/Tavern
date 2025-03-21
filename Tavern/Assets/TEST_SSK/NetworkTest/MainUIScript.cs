using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Steamworks;
using System.Collections.Generic;
using UnityEngine;

public class MainUIScript : MonoBehaviourPunCallbacks
{
    // 메인 패널들
    public GameObject SelectionPanel;
    public GameObject NewGamePlayPanel;
    public GameObject ContinuePanel;
    public GameObject SearchRoomPanel;
    public GameObject SettingPanel;

    // 세부요소
    public GameObject GameRoomListItem;
    public GameObject CurrentRoomInfoListItem;

    // Transform
    public Transform GameRoomListContent;
    public Transform CurrentRoomInfoListContent;

    // Script Variables
    private Dictionary<string, RoomInfo> cachedRoomList;
    private Dictionary<string, GameObject> roomListEntries;
    private Dictionary<int, GameObject> playerListEntries;

    private bool bNeedCreateRoom = false;

    public string CurrentSelectedRoomName;

    public void Awake()
    {
        cachedRoomList = new Dictionary<string, RoomInfo>();
        roomListEntries = new Dictionary<string, GameObject>();
    }

    private void Start()
    {
        SetActivePanel(SelectionPanel.name);
    }

    private void Update()
    {
        if (PhotonNetwork.NetworkClientState == ClientState.JoinedLobby && bNeedCreateRoom && SteamAPI.Init())
        {
            bNeedCreateRoom = false;

            string roomName = SteamUser.GetSteamID().ToString();
            roomName = (roomName.Equals(string.Empty)) ? "Room " + Random.Range(1000, 10000) : roomName;

            byte maxPlayers = 4;
            //byte.TryParse(MaxPlayersInputField.text, out maxPlayers);
            //maxPlayers = (byte)Mathf.Clamp(maxPlayers, 2, 8);

            RoomOptions options = new RoomOptions { MaxPlayers = maxPlayers, PlayerTtl = 10000 };

            Hashtable roomProperties = new Hashtable();
            roomProperties["HostName"] = SteamFriends.GetPersonaName();
            roomProperties["ViewRoomName"] = $"{SteamFriends.GetPersonaName()}'s Room";
            options.CustomRoomProperties = roomProperties;

            // 로비에 표시할 프로퍼티 설정 (중요!)
            options.CustomRoomPropertiesForLobby = new string[] { "HostName", "ViewRoomName" };

            PhotonNetwork.CreateRoom(roomName, options, null);
        }
    }

    public override void OnCreatedRoom()
    {
        PhotonNetwork.CurrentRoom.SetMasterClient(PhotonNetwork.LocalPlayer);

        PhotonNetwork.CurrentRoom.IsOpen = true;
        PhotonNetwork.CurrentRoom.IsVisible = true;     
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearRoomListView();

        UpdateCachedRoomList(roomList);
        UpdateRoomListView();
    }

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            // Remove room from cached room list if it got closed, became invisible or was marked as removed
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
            {
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList.Remove(info.Name);
                }

                continue;
            }

            // Update cached room info
            if (cachedRoomList.ContainsKey(info.Name))
            {
                cachedRoomList[info.Name] = info;
            }
            // Add new room info to cache
            else
            {
                cachedRoomList.Add(info.Name, info);
            }
        }
    }

    private void ClearRoomListView()
    {
        foreach (GameObject entry in roomListEntries.Values)
        {
            Destroy(entry.gameObject);
        }

        roomListEntries.Clear();
    }

    private void UpdateRoomListView()
    {
        int RoomCount = 1;
        foreach (RoomInfo info in cachedRoomList.Values)
        {
            GameObject entry = Instantiate(GameRoomListItem);
            entry.transform.SetParent(GameRoomListContent);
            entry.transform.localScale = Vector3.one;

            entry.GetComponent<RoomListItem>()._MainUIScript = this;
            entry.GetComponent<RoomListItem>().Init(RoomCount++.ToString(), info, false);

            roomListEntries.Add(info.Name, entry);
        }
    }

    public void OnNewGamePlayButtonClicked()
    {
        SetActivePanel(NewGamePlayPanel.name);

        if (SteamAPI.IsSteamRunning())
        {
            if (!PhotonNetwork.InLobby)
            {
                PhotonNetwork.JoinLobby();
            }

            // 여기는 Scene Travel이후 실행
            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, 4);

            // 이게 현재 Player의 Steam닉네임
            SteamFriends.GetPersonaName();

            // 이건 현재 Player의 Steam고유아이디
            SteamUser.GetSteamID();

            bNeedCreateRoom = true;

        }
    }

    public void OnClickStartButton()
    {
        PhotonNetwork.LoadLevel("EmptyScene");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GameObject entry = Instantiate(CurrentRoomInfoListItem);
        entry.transform.SetParent(CurrentRoomInfoListContent);
        entry.transform.localScale = Vector3.one;
        entry.GetComponent<PlayerListItem>().Init(newPlayer.NickName);

        playerListEntries.Add(newPlayer.ActorNumber, entry);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Destroy(playerListEntries[otherPlayer.ActorNumber].gameObject);
        playerListEntries.Remove(otherPlayer.ActorNumber);
    }

    // 로비 생성 후 콜백 함수
    private void OnLobbyCreated(LobbyCreated_t callbackData)
    {
        if (callbackData.m_eResult == EResult.k_EResultOK)
        {
            Debug.Log("로비 생성 성공!");

            CSteamID lobbyID = new CSteamID(callbackData.m_ulSteamIDLobby);

            string lobbyName = "My Game Lobby";

            // 로비 이름 설정
            SteamMatchmaking.SetLobbyData(lobbyID, "name", lobbyName);
            Debug.Log($"로비 ID: {lobbyID}, 이름: {lobbyName}");

        }
        else
        {
            Debug.LogError("로비 생성 실패: " + callbackData.m_eResult);
        }
    }
    public override void OnJoinedRoom()
    {
        if (!PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("EmptyScene");
            Debug.Log("OnJoinedRoom");
        }
    }

    public void OnClickJoinRoom()
    {
        if (CurrentSelectedRoomName != "")
        {
            bool bCheck = PhotonNetwork.JoinRoom(CurrentSelectedRoomName);

            if (bCheck)
            {
                Debug.Log("OnClickJoinRoom TRUE");
            }
            else
            {
                Debug.Log("OnClickJoinRoom FALSE");
            }
        }
    }

    public void OnClickGameRoomItem()
    {
        if (playerListEntries == null)
        {
            playerListEntries = new Dictionary<int, GameObject>();
        }

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            GameObject entry = Instantiate(CurrentRoomInfoListItem);
            entry.transform.SetParent(CurrentRoomInfoListContent);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<PlayerListItem>().Init(SteamFriends.GetPersonaName());
            
            playerListEntries.Add(p.ActorNumber, entry);
        }
    }

    public override void OnLeftRoom()
    {
        SetActivePanel(SelectionPanel.name);

        foreach (GameObject entry in playerListEntries.Values)
        {
            Destroy(entry.gameObject);
        }

        playerListEntries.Clear();
        playerListEntries = null;
    }

    public void OnContinueButtonClicked()
    {
        SetActivePanel(ContinuePanel.name);
    }

    public void OnSearchRoomButtonClicked()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }

        SetActivePanel(SearchRoomPanel.name);
    }

    public void OnSettingButtonClicked()
    {
        SetActivePanel(SettingPanel.name);
    }

    public void OnBackButtonClicked()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }

        SetActivePanel(SelectionPanel.name);
    }

    private void SetActivePanel(string activePanel)
    {
        SelectionPanel.SetActive(activePanel.Equals(SelectionPanel.name));
        NewGamePlayPanel.SetActive(activePanel.Equals(NewGamePlayPanel.name));
        ContinuePanel.SetActive(activePanel.Equals(ContinuePanel.name));
        SearchRoomPanel.SetActive(activePanel.Equals(SearchRoomPanel.name));
        SettingPanel.SetActive(activePanel.Equals(SettingPanel.name));
    }
}

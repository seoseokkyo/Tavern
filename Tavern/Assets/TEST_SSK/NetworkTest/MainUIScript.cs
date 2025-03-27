using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Steamworks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // Buttons
    // Selection Panel's
    public Button NewGamePlayButton;
    public Button ContinueButton;
    public Button SearchButton;

    // NewGamePlay Panel's
    public Button StartButton;

    public void Awake()
    {
        cachedRoomList = new Dictionary<string, RoomInfo>();
        roomListEntries = new Dictionary<string, GameObject>();

        NewGamePlayButton.interactable = false;
        ContinueButton.interactable = false;
        SearchButton.interactable = false;

        StartButton.interactable = false;
    }

    private void Start()
    {
        SetActivePanel(SelectionPanel.name);

        NewGamePlayButton.interactable = false;
        ContinueButton.interactable = false;
        SearchButton.interactable = false;

        StartButton.interactable = false;

        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;

        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.EnableCloseConnection = true;
    }

    private void Update()
    {
        if (PhotonNetwork.NetworkClientState == ClientState.JoinedLobby && bNeedCreateRoom && SteamAPI.IsSteamRunning())
        {
            bNeedCreateRoom = false;

            //string roomName = SteamUser.GetSteamID().ToString();
            //roomName = (roomName.Equals(string.Empty)) ? "Room " + Random.Range(1000, 10000) : roomName;

            byte maxPlayers = 4;

            RoomOptions options = new RoomOptions { MaxPlayers = maxPlayers, PlayerTtl = 10000 };

            Hashtable roomProperties = new Hashtable();
            roomProperties["HostName"] = SteamFriends.GetPersonaName();
            roomProperties["ViewRoomName"] = $"{SteamFriends.GetPersonaName()}'s Room";
            options.CustomRoomProperties = roomProperties;

            options.CustomRoomPropertiesForLobby = new string[] { "HostName", "ViewRoomName" };

            bool bCheck = PhotonNetwork.CreateRoom(SteamUser.GetSteamID().ToString(), options, null);
            Debug.Log($"PhotonNetwork.CreateRoom : {bCheck}");
        }
    }

    public override void OnCreatedRoom()
    {
        PhotonNetwork.CurrentRoom.SetMasterClient(PhotonNetwork.LocalPlayer);

        PhotonNetwork.CurrentRoom.IsOpen = true;
        PhotonNetwork.CurrentRoom.IsVisible = true;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"방 생성 실패. 오류 코드: {returnCode}, 메시지: {message}");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearRoomListView();

        UpdateCachedRoomList(roomList);
        UpdateRoomListView();

        Debug.Log($"PhotonNetwork.CountOfRooms:{PhotonNetwork.CountOfRooms}");
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
            //SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, 4);

            bNeedCreateRoom = true;
            StartButton.interactable = false;
        }
    }

    public void OnClickStartButton()
    {
        PhotonNetwork.LoadLevel("tarvern");
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

    public override void OnJoinedRoom()
    {
        if (!PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("tarvern");
            Debug.Log("OnJoinedRoom");
        }
        else
        {
            StartButton.interactable = true;
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
            entry.GetComponent<PlayerListItem>().Init(p.NickName);

            playerListEntries.Add(p.ActorNumber, entry);
        }
    }

    public override void OnLeftRoom()
    {
        SetActivePanel(SelectionPanel.name);

        if (playerListEntries == null)
        {
            return;
        }

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

        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
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

    public override void OnConnectedToMaster()
    {
        NewGamePlayButton.interactable = true;
        ContinueButton.interactable = true;
        SearchButton.interactable = true;
    }

    public void OnClickQuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
}

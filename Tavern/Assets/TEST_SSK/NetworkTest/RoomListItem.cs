using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour, IPointerClickHandler
{
    public MainUIScript _MainUIScript;

    public TMP_Text RoomNumber;
    public TMP_Text RoomName;
    public TMP_Text HostName;
    public TMP_Text PlayerNumber;
    public Toggle PrivateCheckToggle;

    private string RoomCode = "";

    private void Awake()
    {

    }

    public void Init(string RoomNumber, RoomInfo Info, bool bPrivateCheck)
    {
        this.RoomNumber.text = RoomNumber;
        RoomCode = Info.Name;

        if (Info.CustomProperties.TryGetValue("HostName", out object hostNameObj))
        {
            this.HostName.text = hostNameObj as string;
        }

        if (Info.CustomProperties.TryGetValue("ViewRoomName", out object viewRoomNameObj))
        {
            this.RoomName.text = viewRoomNameObj as string;
        }

        this.PlayerNumber.text = $"{Info.PlayerCount}/{Info.MaxPlayers}";
        PrivateCheckToggle.isOn = bPrivateCheck;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _MainUIScript.CurrentSelectedRoomName = RoomCode;
    }

    public void SetPlayerNumber(int Number)
    {
        PlayerNumber.text = Number.ToString();
    }
}

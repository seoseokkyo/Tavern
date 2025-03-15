using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class RoomInfoUI : MonoBehaviour, IScrollHandler
{
    public TMP_InputField roomNumber;
    public TMP_InputField roomNameInput;
    //public TMP_InputField roomIDInput;
    public TMP_InputField hostNameInput;
    public TMP_InputField userNumberInput;

    public SteamManager.RoomInfoStruct roomInfo;



    void Start()
    {

    }
    void Update()
    {

    }

    public void OnPointerClickTest()
    {
        TestUI parent = GetComponentInParent<TestUI>();

        int index = transform.GetSiblingIndex();
        Debug.Log($"선택된 UI 요소 인덱스: {index}");

        parent.lastSelectedIndex = index;
    }

    public void OnScroll(PointerEventData eventData)
    {
        TestUI parent = GetComponentInParent<TestUI>();

        parent.scrollRect.OnScroll(eventData);
    }

    public void InitData(SteamManager.RoomInfoStruct roomData)
    {
        roomInfo = roomData;

        roomNumber.text = $"{roomData.roomNumber}";
        roomNameInput.text = roomData.roomName;
        //roomIDInput.text = roomData.roomID;
        hostNameInput.text = roomData.hostName;

        userNumberInput.text = roomData.currentUserNum + "/" + roomData.roomUserLimit;

        transform.SetParent(roomData.parentTransform);
    }
}

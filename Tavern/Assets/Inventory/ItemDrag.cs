using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class ItemDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //드래그 될때 이동되는 아이콘을 담는 static 변수
    //static으로 선언하는 이유는 Drop이벤트를 가진 슬롯 스크립트에서 접근하기 위함
    public static GameObject beingDraggedIcon;

    // 슬롯이 아닌 다른 오브젝트에 Icon을 드랍할 경우 원복할 위치 백업용
    Vector3 startPosition;

    // Drag중 UI 레이어에 비정상적으로 보이기 때문에
    // Icon 드래그중 변경할 부모 RactTransfom 변수
    [SerializeField] Transform onDragParent;

    // 슬롯이 아닌 다른 오브젝트에 Icon을 드랍할 경우 원복할 부모 백업용
    [HideInInspector] public Transform startParent;

    PlayerController Player = null;

    // 인터페이스 IBeginDragHandler를 상속 받았을 때 구현해야하는 콜백함수
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 드래그가 시작될때 대상 Icon의 게임오브젝트를 static 변수에 할당
        beingDraggedIcon = gameObject;

        // 백업용 포지션과 부모 트랜스폼을 백업 해둔다.
        startPosition = transform.position;
        startParent = transform.parent;

        // Drop이벤트를 정상적으로 감지하기 위해 Icon RectTransform을 무시 
        GetComponent<CanvasGroup>().blocksRaycasts = false;

        Player = FindFirstObjectByType<PlayerController>();

        onDragParent = Player.PlayerCanvas.transform;

        // 드래그 시작할때 부모transform을 변경
        transform.SetParent(onDragParent);
    }

    // 인터페이스 IDragHandler 상속 받았을 때 구현 해야하는 콜백함수
    public void OnDrag(PointerEventData eventData)
    {
        //드래그중에는 Icon을 마우스나 터치된 포인트의 위치로 이동시킨다.
        transform.position = Input.mousePosition;
    }

    // 인터페이스 IEndDragHandler 상속 받았을 때 구현 해야하는 콜백함수
    public void OnEndDrag(PointerEventData eventData)
    {
        // 드래그 대상을 지우고 해당 Icon에 이벤트감지를 허용한다.

        GetComponent<CanvasGroup>().blocksRaycasts = true;
        // 혹 드랍이벤트에 따라 부모가 변경되지 않고 
        // 이동중에 할당 되었던 부모 transform과 같다면
        // Icon의 부모와 위치를 원복한다.
        if (transform.parent == onDragParent && EventSystem.current.IsPointerOverGameObject())
        {
            transform.position = startPosition;
            transform.SetParent(startParent);
        }
        else
        {
            DropItemToWorld();
            Destroy(beingDraggedIcon.gameObject);
        }

        beingDraggedIcon = null;
    }

    private void DropItemToWorld()
    {
        Vector3 worldPosition = GetMouseWorldPosition();
        if (worldPosition != Vector3.zero)
        {
            ItemUI tempItem = beingDraggedIcon.GetComponent<ItemUI>();

            Vector3 direction = worldPosition - Player.transform.position;
            float distance = direction.magnitude;
            float maxDistance = 10f;

            Vector3 targetPosition = (distance > maxDistance)
                ? Player.transform.position + direction.normalized * maxDistance
                : worldPosition;

            targetPosition.y = worldPosition.y;

            ItemManager.Instance.ItemSpawn(tempItem.CurrentItemBase, targetPosition, Quaternion.identity);

            var tempSlot = startParent.GetComponentInParent<ItemSlotUI>();

            tempItem.CurrentItemBase.OwnerInventory.PopItem(tempSlot.SlotIndex);
        }

        Player.PlayerInventory.OnChanged();
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point; // 마우스가 닿은 위치 반환
        }
        return Vector3.zero; // 기본값 (드롭 불가능)
    }
}
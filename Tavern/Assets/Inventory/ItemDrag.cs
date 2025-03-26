using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class ItemDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject beingDraggedIcon;

    Vector3 startPosition;

    [SerializeField] Transform onDragParent;

    [HideInInspector] public Transform startParent;

    PlayerController Player = null;

    public void OnBeginDrag(PointerEventData eventData)
    {
        beingDraggedIcon = gameObject;

        var Temp = beingDraggedIcon.transform.root;
        var Temp2 = Temp.GetComponentInParent<PlayerController>();

        startPosition = transform.position;
        startParent = transform.parent;

        GetComponent<CanvasGroup>().blocksRaycasts = false;

        Player = TavernGameManager.Instance.CurrentLocalPlayer;
        Debug.Log($"OnBeginDrag_CurrentLocalPlayer : {Player}");

        onDragParent = Player.PlayerCanvas.transform;

        transform.SetParent(onDragParent);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;

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
            float maxDistance = 100f;

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
            return hit.point;
        }
        return Vector3.zero;
    }
}
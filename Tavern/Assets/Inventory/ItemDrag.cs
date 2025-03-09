using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class ItemDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //�巡�� �ɶ� �̵��Ǵ� �������� ��� static ����
    //static���� �����ϴ� ������ Drop�̺�Ʈ�� ���� ���� ��ũ��Ʈ���� �����ϱ� ����
    public static GameObject beingDraggedIcon;

    // ������ �ƴ� �ٸ� ������Ʈ�� Icon�� ����� ��� ������ ��ġ �����
    Vector3 startPosition;

    // Drag�� UI ���̾ ������������ ���̱� ������
    // Icon �巡���� ������ �θ� RactTransfom ����
    [SerializeField] Transform onDragParent;

    // ������ �ƴ� �ٸ� ������Ʈ�� Icon�� ����� ��� ������ �θ� �����
    [HideInInspector] public Transform startParent;

    PlayerController Player = null;

    // �������̽� IBeginDragHandler�� ��� �޾��� �� �����ؾ��ϴ� �ݹ��Լ�
    public void OnBeginDrag(PointerEventData eventData)
    {
        // �巡�װ� ���۵ɶ� ��� Icon�� ���ӿ�����Ʈ�� static ������ �Ҵ�
        beingDraggedIcon = gameObject;

        // ����� �����ǰ� �θ� Ʈ�������� ��� �صд�.
        startPosition = transform.position;
        startParent = transform.parent;

        // Drop�̺�Ʈ�� ���������� �����ϱ� ���� Icon RectTransform�� ���� 
        GetComponent<CanvasGroup>().blocksRaycasts = false;

        Player = FindFirstObjectByType<PlayerController>();

        onDragParent = Player.PlayerCanvas.transform;

        // �巡�� �����Ҷ� �θ�transform�� ����
        transform.SetParent(onDragParent);
    }

    // �������̽� IDragHandler ��� �޾��� �� ���� �ؾ��ϴ� �ݹ��Լ�
    public void OnDrag(PointerEventData eventData)
    {
        //�巡���߿��� Icon�� ���콺�� ��ġ�� ����Ʈ�� ��ġ�� �̵���Ų��.
        transform.position = Input.mousePosition;
    }

    // �������̽� IEndDragHandler ��� �޾��� �� ���� �ؾ��ϴ� �ݹ��Լ�
    public void OnEndDrag(PointerEventData eventData)
    {
        // �巡�� ����� ����� �ش� Icon�� �̺�Ʈ������ ����Ѵ�.

        GetComponent<CanvasGroup>().blocksRaycasts = true;
        // Ȥ ����̺�Ʈ�� ���� �θ� ������� �ʰ� 
        // �̵��߿� �Ҵ� �Ǿ��� �θ� transform�� ���ٸ�
        // Icon�� �θ�� ��ġ�� �����Ѵ�.
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
            return hit.point; // ���콺�� ���� ��ġ ��ȯ
        }
        return Vector3.zero; // �⺻�� (��� �Ұ���)
    }
}
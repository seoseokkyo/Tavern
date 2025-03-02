using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlotUI : MonoBehaviour, IDropHandler
{
    public int SlotIndex;

    GameObject Icon()
    {
        // ���Կ� ������(�ڽ� Ʈ������)�� ������ �������� gameobject�� ����
        // ���Կ� ������(�ڽ� Ʈ������)�� ���ٸ� null�� ����
        if (transform.childCount > 0)
            return transform.GetChild(0).gameObject;
        else
            return null;
    }

    // IDropHandler �������̽� ��ӽ� �����ؾ� �Ǵ� �ݹ� �Լ�
    // �� ��ũ��Ʈ�� ������Ʈ�� �߰� �� ���� ������Ʈ RactTransform����
    // ������ ����� �߻��ϸ� ����Ǵ� �ݹ��Լ�
    public void OnDrop(PointerEventData eventData)
    {
        ItemUI DragItemUI = ItemDrag.beingDraggedIcon.GetComponent<ItemUI>();

        // ������ ����ִٸ� Icon�� �ڽ����� �߰� ��ġ���� ��.
        if (Icon() == null)
        {
            ItemDrag.beingDraggedIcon.transform.SetParent(transform);
            ItemDrag.beingDraggedIcon.transform.position = transform.position;

            if (null != GetComponentInParent<InventoryUI>())
            {
                GetComponentInParent<InventoryUI>().SwapItemFromUI(SlotIndex, DragItemUI.ItemIndex);
                Debug.Log("InventoryUI");
            }
        }
        else
        {
            ItemUI CurrentItemUI =  Icon().gameObject.GetComponent<ItemUI>();

            if(null != CurrentItemUI && null != DragItemUI)
            {
                if (null != GetComponentInParent<InventoryUI>())
                {
                    GetComponentInParent<InventoryUI>().SwapItemFromUI(CurrentItemUI.ItemIndex, DragItemUI.ItemIndex);
                    Debug.Log("InventoryUI");
                }
            }
        }
    }
}

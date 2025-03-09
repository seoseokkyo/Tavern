using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlotUI : MonoBehaviour, IDropHandler
{
    public int SlotIndex;

    public InventoryComp OwnerInventory;

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
        if (null != ItemDrag.beingDraggedIcon)
        {
            ItemUI DragItemUI = ItemDrag.beingDraggedIcon.GetComponent<ItemUI>();

            if (null != DragItemUI)
            {
                GetComponentInParent<InventoryUI>().SwapItemFromUI(ref OwnerInventory, ref DragItemUI.CurrentItemBase.OwnerInventory, SlotIndex, DragItemUI.ItemIndex);

                Destroy(DragItemUI.gameObject);
            }
        }
    }
}

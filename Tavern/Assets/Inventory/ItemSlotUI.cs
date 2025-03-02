using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlotUI : MonoBehaviour, IDropHandler
{
    public int SlotIndex;

    GameObject Icon()
    {
        // 슬롯에 아이템(자식 트랜스폼)이 있으면 아이템의 gameobject를 리턴
        // 슬롯에 아이템(자식 트랜스폼)이 없다면 null을 리턴
        if (transform.childCount > 0)
            return transform.GetChild(0).gameObject;
        else
            return null;
    }

    // IDropHandler 인터페이스 상속시 구현해야 되는 콜백 함수
    // 이 스크립트가 컨포넌트로 추가 된 게임 오브젝트 RactTransform내에
    // 포인터 드랍이 발생하면 실행되는 콜백함수
    public void OnDrop(PointerEventData eventData)
    {
        ItemUI DragItemUI = ItemDrag.beingDraggedIcon.GetComponent<ItemUI>();

        // 슬롯이 비어있다면 Icon을 자식으로 추가 위치변경 함.
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

using UnityEngine;
using UnityEngine.EventSystems;

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
        beingDraggedIcon = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        // Ȥ ����̺�Ʈ�� ���� �θ� ������� �ʰ� 
        // �̵��߿� �Ҵ� �Ǿ��� �θ� transform�� ���ٸ�
        // Icon�� �θ�� ��ġ�� �����Ѵ�.
        if (transform.parent == startParent)
        {
            transform.position = startPosition;
            transform.SetParent(startParent);
        }
    }
}
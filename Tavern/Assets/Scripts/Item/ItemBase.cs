using UnityEngine;



public class ItemBase : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public enum EItemType
    {
        UseAble,
        NoUseAble,
        //Equipment, // �ϴ� �����Կ��� ����ϴ� ���·�
        EItemTypeMax
    }

    public EItemType ItemType = EItemType.NoUseAble;

    // �κ��丮 ��� ������ ���� ������ ���� ��� ��뿹���� ���� �������� �Ľ��ϰ�, main���� ���ø��� �Ǹ� ��ü���� ����ĭ�� ���� ���Ĵ� ���ȵ��� ������� ���� ���..?
    public string ItemDescription = "main:ex hp:Description1 Item:Description2 Item:Description3...";

    //public MonoBehaviour UsingPlayer { get; set; } = null;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

using UnityEngine;



public class ItemBase : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public enum EItemType
    {
        UseAble,
        NoUseAble,
        //Equipment, // 일단 퀵슬롯에서 사용하는 형태로
        EItemTypeMax
    }

    public EItemType ItemType = EItemType.NoUseAble;

    // 인벤토리 등에서 아이템 관련 설명을 적을 경우 사용예정인 변수 공백으로 파싱하고, main으로 스플릿이 되면 전체적인 설명칸에 들어가며 이후는 스탯등의 변경사항 설명에 사용..?
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

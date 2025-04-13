using Photon.Pun;
using System;
using UnityEngine;
using WebSocketSharp;

[Serializable]
public class IngredientData
{
    public string IngredientName;
    public string GrilledIngredientName;
    public string SlicedIngredientName;
    public string BoiledIngredientName;
    public string FriedIngredientName;


    public string PerfectFlow = "";

    // ex)
    /*
     *IngredientName = Potato;
     *GrilledIngredientName = Grilled Whole Potato; 
     *SlicedIngredientName = Sliced Potato; 
     *BoiledIngredientName = Boiled Whole Potato; -> 매쉬드 포테이토의 재료가 될 수 있음
     *FriedIngredientName = Fried Whole Potato;
     *PerfectFlow = Potato;
     *
     *IngredientName = Slice Potato;
     *GrilledIngredientName = Grilled Slice Potato; 
     *SlicedIngredientName = Julienne cut Potato;
     *BoiledIngredientName = Boiled Slice Potato; -> 매쉬드 포테이토의 재료가 될 수 있음
     *FriedIngredientName = Fried Slice Potato;
     *PerfectFlow = Potato,Slice;
     *
     *.... etc
    */

}


public class IngredientComp : MonoBehaviour, IPunObservable
{
    // 원본객체 접근용
    [HideInInspector]
    public WorldItem CurrentWorldItem = null;

    // 파생 아이템 이름들
    [HideInInspector]
    public IngredientData IngredientData = null;

    // 각 파생 전 후 값을 유지를 해야하나...
    // 굽고 난 다음 썰거나 튀긴다음 썰거나....
    [HideInInspector]
    public float GrilledValue = 0.0f;

    [HideInInspector]
    public float BoiledValue = 0.0f;

    [HideInInspector]
    public float FriedValue = 0.0f;

    [HideInInspector]
    private bool bTransformed = false;

    // 여기에 각 조리 단계를 적어놓는다??? 감자에서 시작해서 썰고 구우면 Potato,Slice,Grill 이런식으로?
    // 조리순서(조리법)을 지키지 않으면 맞는 음식이긴 한데 감점을 준다???
    private bool bInitialize = false;

    [HideInInspector]
    public string CookingFlowString = "";

    // 그럼 원래 의도된 순서도 적어놓아야겠네....?? 일단 인그리디언트 데이터에다가...

    // 일단 이렇게 각 식재료에 컴포넌트 붙여놓고 Plate에서 합쳐질때마다 Plate의 리스트에 Comp의 현재 식재료 이름과 각 조리값, 현재까지의 조리 Flow를 얹는다.....?

    public int WellGrilledStart = 40;
    public int HardGrilledStart = 60;

    public int WellBoiledStart = 40;
    public int HardBoiledStart = 60;

    public int WellFriedStart = 40;
    public int HardFriedStart = 60;

    public int TrashLimit = 100;

    public void SetData(IngredientData ingredientData)
    {
        IngredientData = ingredientData;

        if (!bInitialize)
        {
            bInitialize = true;

            CookingFlowString = IngredientData.IngredientName;
        }
    }

    public void Slice()
    {
        CookingFlowString += ",Sliced";
        // 만약 SetData에서 FlowString에 이니셜이 된 직후 호출이 되었으면 "Potato,Sliced,"가 들어가 있을 것

        var ItemData = ItemManager.Instance.GetItemDataByName(IngredientData.SlicedIngredientName);
        var CreatedItemBase = ItemBase.ItemBaseCreator.CreateItemBase(ItemData);

        CurrentWorldItem.SetItem(CreatedItemBase);
        CurrentWorldItem.ClientToAllItemDataSync();

        // WorldItem은 IngredientData.SlicedIngredientName로 변경이 된 상태 (Potato -> Sliced Potato)

        // 여기서 아이템매니저에 파생 계층 데이터 갖고와서 엎어줘야 함
        // IngredientData = ItemManager.Instance.DoSomeThing();
    }

    private bool LastCookFlowCheck(string CheckString)
    {
        int Find = CookingFlowString.LastIndexOf(',');

        if (Find != -1)
        {
            string lastPart = CookingFlowString.Substring(Find + 1);

            return (lastPart == CheckString);
        }

        return false;
    }

    private bool TrashCheck()
    {
        float Sum = GrilledValue + BoiledValue + FriedValue;

        return (Sum >= TrashLimit);
    }

    public void TransfromToTrash()
    {

    }

    public void AccumulateGrilledValue(float fValue)
    {
        if (TrashCheck())
        {
            if (false == LastCookFlowCheck("Trash"))
            {
                // 여기서 음식물 쓰레기로 변환
                TransfromToTrash();
                CurrentWorldItem.ClientToAllItemDataSync();
            }

            // 음식물 쓰레기가 된 상태

            return;
        }

        // 일단은 처음 내용대로 조리도구에 들어간 직후에 얘의 미래는 결정됨
        // 처음에 구웠다가 도중에 꺼내서 끓는솥 같은데에 들어왔다가 이런식으로 조리자체의 방법이 섞이는건 일단 무시....
        if (false == LastCookFlowCheck(ECookType.Grill.ToString()))
        {
            CookingFlowString += $",{ECookType.Grill.ToString()}";
        }

        // 만약 SetData에서 FlowString에 이니셜이 된 직후 호출이 되었으면 "Potato,Grilled"가 들어가 있을 것


        // 아니면 조리 시작부터 얘의 상태를 변경시키는게 아니라 값에 Limit을 줘서 30정도 이상 구웠을 경우 구운상태의 데이터가 된다던가 한다고 치면.......
        // 그리고 이미 상태가 변경된 식재료를 다른 조리방법에 때려넣고 그 값의 Limit까지 냅두면 음식물쓰레기가 된다거나.....

        GrilledValue += fValue;

        if (GrilledValue > HardGrilledStart)
        {
            // 여기서 음식물 쓰레기로 변환   
            TransfromToTrash();
            CurrentWorldItem.ClientToAllItemDataSync();
        }
        else if (bTransformed == false && GrilledValue > WellGrilledStart && !IngredientData.GrilledIngredientName.IsNullOrEmpty())
        {
            // 여기서 잘 구워진 에셋으로 변환

            var ItemData = ItemManager.Instance.GetItemDataByName(IngredientData.GrilledIngredientName);
            var CreatedItemBase = ItemBase.ItemBaseCreator.CreateItemBase(ItemData);

            CurrentWorldItem.SetItem(CreatedItemBase);

            SetData(ItemManager.Instance.GetIngredientData(IngredientData.GrilledIngredientName));

            CurrentWorldItem.ClientToAllItemDataSync();

            // WorldItem은 IngredientData.GrilledIngredientName로 변경이 된 상태 (Potato -> Grilled Potato)

            // 여기서 아이템매니저에 파생 계층 데이터 갖고와서 엎어줘야 함 << 파생 계층 데이터 작업 필요
            // 완성된 요리의 조리방법 중 하나의 식재료가 삶기->썰기->굽기등의 순으로 이루어진다고 하면 그 레시피는 일단 나중에 생각해보는걸로
            // Potato에서부터 Fried Julienne cut Potato까지 그러니까 시작부터 끝까지는 조리 레벨이 유지가 되는 구조이기때문에
            // 플레이어의 자유도때문에 조리 순서가 통감자를 튀기고 썰고 다시 썰어서 줄리엔느 컷 상태로 만든다 했을 때 튀긴 레벨은 적당하더라도 FlowString에서 걸러낼 수 있을듯?
            // 만약 위의 순서도 정상조리로 봐야한다고 하면 FlowString을 체크하는 방식을 그냥 PerfectFlow의 모든 요소가 있는지만 확인하면 될듯
            // IngredientData = ItemManager.Instance.DoSomeThing();
        }
    }

    public void AccumulateBoiledValue(float fValue)
    {
        if (TrashCheck())
        {
            if (false == LastCookFlowCheck("Trash"))
            {
                // 여기서 음식물 쓰레기로 변환
                TransfromToTrash();
                CurrentWorldItem.ClientToAllItemDataSync();
            }

            // 음식물 쓰레기가 된 상태
            return;
        }

        if (false == LastCookFlowCheck(ECookType.Boiling.ToString()))
        {
            CookingFlowString += $",{ECookType.Boiling.ToString()}";
        }

        BoiledValue += fValue;

        if (BoiledValue > HardBoiledStart)
        {
            // 여기서 음식물 쓰레기로 변환
            TransfromToTrash();
            CurrentWorldItem.ClientToAllItemDataSync();
        }
        else if (bTransformed == false && BoiledValue > WellBoiledStart && !IngredientData.BoiledIngredientName.IsNullOrEmpty())
        {
            var ItemData = ItemManager.Instance.GetItemDataByName(IngredientData.BoiledIngredientName);
            var CreatedItemBase = ItemBase.ItemBaseCreator.CreateItemBase(ItemData);

            CurrentWorldItem.SetItem(CreatedItemBase);

            SetData(ItemManager.Instance.GetIngredientData(IngredientData.BoiledIngredientName));

            CurrentWorldItem.ClientToAllItemDataSync();
        }
    }

    public void AccumulateFriedValue(float fValue)
    {
        if (TrashCheck())
        {
            if (false == LastCookFlowCheck("Trash"))
            {
                // 여기서 음식물 쓰레기로 변환
                TransfromToTrash();
                CurrentWorldItem.ClientToAllItemDataSync();
            }

            // 음식물 쓰레기가 된 상태
            return;
        }

        if (false == LastCookFlowCheck(ECookType.Fry.ToString()))
        {
            CookingFlowString += $",{ECookType.Fry.ToString()}";
        }

        FriedValue += fValue;

        if (FriedValue > HardFriedStart)
        {
            // 여기서 음식물 쓰레기로 변환
            TransfromToTrash();
            CurrentWorldItem.ClientToAllItemDataSync();
        }
        else if (bTransformed == false && FriedValue > WellFriedStart && !IngredientData.FriedIngredientName.IsNullOrEmpty())
        {
            var ItemData = ItemManager.Instance.GetItemDataByName(IngredientData.FriedIngredientName);
            var CreatedItemBase = ItemBase.ItemBaseCreator.CreateItemBase(ItemData);

            CurrentWorldItem.SetItem(CreatedItemBase);

            SetData(ItemManager.Instance.GetIngredientData(IngredientData.FriedIngredientName));

            CurrentWorldItem.ClientToAllItemDataSync();
        }
    }

    private void Awake()
    {
        GetComponent<PhotonView>().ObservedComponents.Add(this);
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(GrilledValue);
            stream.SendNext(BoiledValue);
            stream.SendNext(FriedValue);
        }
        else
        {
            GrilledValue = (float)stream.ReceiveNext();
            BoiledValue = (float)stream.ReceiveNext();
            FriedValue = (float)stream.ReceiveNext();
        }
    }
}

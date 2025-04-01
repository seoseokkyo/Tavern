using UnityEngine;

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


public class IngredientComp : MonoBehaviour
{
    // 원본객체 접근용
    public WorldItem CurrentWorldItem = null;

    // 파생 아이템 이름들
    public IngredientData IngredientData = null;

    // 각 파생 전 후 값을 유지를 해야하나...
    // 굽고 난 다음 썰거나 튀긴다음 썰거나....
    public float GrilledValue = 0.0f;
    public float BoiledValue = 0.0f;
    public float FriedValue = 0.0f;

    // 여기에 각 조리 단계를 적어놓는다??? 감자에서 시작해서 썰고 구우면 Potato,Slice,Grill 이런식으로?
    // 조리순서(조리법)을 지키지 않으면 맞는 음식이긴 한데 감점을 준다???
    private bool bInitialize = false;
    public string CookingFlowString = "";

    // 그럼 원래 의도된 순서도 적어놓아야겠네....?? 일단 인그리디언트 데이터에다가...

    // 일단 이렇게 각 식재료에 컴포넌트 붙여놓고 Plate에서 합쳐질때마다 Plate의 리스트에 Comp의 현재 식재료 이름과 각 조리값, 현재까지의 조리 Flow를 얹는다.....?

    public void SetData(IngredientData ingredientData)
    {
        IngredientData = ingredientData;

        if (!bInitialize)
        {
            bInitialize = true;

            CookingFlowString = IngredientData.IngredientName + ",";
        }
    }

    public void Slice()
    {
        CookingFlowString += "Slice,";
        // 만약 SetData에서 FlowString에 이니셜이 된 직후 호출이 되었으면 "Potato,Slice,"가 들어가 있을 것

        var ItemData = ItemManager.Instance.GetItemDataByName(IngredientData.SlicedIngredientName);
        var CreatedItemBase = ItemBase.ItemBaseCreator.CreateItemBase(ItemData);

        CurrentWorldItem.SetItem(CreatedItemBase);
        // WorldItem은 IngredientData.SlicedIngredientName로 변경이 된 상태 (Potato -> Slice Potato)

        // 여기서 아이템매니저에 파생 계층 데이터 갖고와서 엎어줘야 함
        // IngredientData = ItemManager.Instance.DoSomeThing();
    }

    private bool CookingStartedCheck()
    {
        // 모든 조리값이 0일경우 아직 조리시작을 하지 않은것
        return (BoiledValue == 0.0f && BoiledValue == 0.0f && FriedValue == 0.0f);
    }

    public void AccumulateGrilledValue(float fValue)
    {
        // 일단은 처음 내용대로 조리도구에 들어간 직후에 얘의 미래는 결정됨
        // 처음에 구웠다가 도중에 꺼내서 끓는솥 같은데에 들어왔다가 이런식으로 조리자체의 방법이 섞이는건 일단 무시....
        if (CookingStartedCheck())
        {
            CookingFlowString += "Grill,";
            // 만약 SetData에서 FlowString에 이니셜이 된 직후 호출이 되었으면 "Potato,Slice,"가 들어가 있을 것

            var ItemData = ItemManager.Instance.GetItemDataByName(IngredientData.GrilledIngredientName);
            var CreatedItemBase = ItemBase.ItemBaseCreator.CreateItemBase(ItemData);

            CurrentWorldItem.SetItem(CreatedItemBase);
            // WorldItem은 IngredientData.GrilledIngredientName로 변경이 된 상태 (Potato -> Grilled Potato)

            // 여기서 아이템매니저에 파생 계층 데이터 갖고와서 엎어줘야 함 << 파생 계층 데이터 작업 필요
            // 완성된 요리의 조리방법 중 하나의 식재료가 삶기->썰기->굽기등의 순으로 이루어진다고 하면 그 레시피는 일단 나중에 생각해보는걸로
            // Potato에서부터 Fried Julienne cut Potato까지 그러니까 시작부터 끝까지는 조리 레벨이 유지가 되는 구조이기때문에
            // 플레이어의 자유도때문에 조리 순서가 통감자를 튀기고 썰고 다시 썰어서 줄리엔느 컷 상태로 만든다 했을 때 튀긴 레벨은 적당하더라도 FlowString에서 걸러낼 수 있을듯?
            // 만약 위의 순서도 정상조리로 봐야한다고 하면 FlowString을 체크하는 방식을 그냥 PerfectFlow의 모든 요소가 있는지만 확인하면 될듯
            // IngredientData = ItemManager.Instance.DoSomeThing();

            // 아니면 조리 시작부터 얘의 상태를 변경시키는게 아니라 값에 Limit을 줘서 30정도 이상 구웠을 경우 구운상태의 데이터가 된다던가 한다고 치면.......
            // 그리고 이미 상태가 변경된 식재료를 다른 조리방법에 때려넣고 그 값의 Limit까지 냅두면 음식물쓰레기가 된다거나.....
        }

        GrilledValue += fValue;
    }

    public void AccumulateBoiledValue(float fValue)
    {
        if (CookingStartedCheck())
        {
            CookingFlowString += "Boiled,";
            // 만약 SetData에서 FlowString에 이니셜이 된 직후 호출이 되었으면 "Potato,Boiled,"가 들어가 있을 것
        }

        BoiledValue += fValue;
    }

    public void AccumulateFriedValue(float fValue)
    {
        if (CookingStartedCheck())
        {
            CookingFlowString += "Fried,";
            // 만약 SetData에서 FlowString에 이니셜이 된 직후 호출이 되었으면 "Potato,Fried,"가 들어가 있을 것
        }

        FriedValue += fValue;
    }
}

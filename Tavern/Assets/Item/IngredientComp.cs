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
     *BoiledIngredientName = Boiled Whole Potato; -> �Ž��� ���������� ��ᰡ �� �� ����
     *FriedIngredientName = Fried Whole Potato;
     *PerfectFlow = Potato;
     *
     *IngredientName = Slice Potato;
     *GrilledIngredientName = Grilled Slice Potato; 
     *SlicedIngredientName = Julienne cut Potato;
     *BoiledIngredientName = Boiled Slice Potato; -> �Ž��� ���������� ��ᰡ �� �� ����
     *FriedIngredientName = Fried Slice Potato;
     *PerfectFlow = Potato,Slice;
     *
     *.... etc
    */

}


public class IngredientComp : MonoBehaviour
{
    // ������ü ���ٿ�
    public WorldItem CurrentWorldItem = null;

    // �Ļ� ������ �̸���
    public IngredientData IngredientData = null;

    // �� �Ļ� �� �� ���� ������ �ؾ��ϳ�...
    // ���� �� ���� ��ų� Ƣ����� ��ų�....
    public float GrilledValue = 0.0f;
    public float BoiledValue = 0.0f;
    public float FriedValue = 0.0f;

    // ���⿡ �� ���� �ܰ踦 ������´�??? ���ڿ��� �����ؼ� ��� ����� Potato,Slice,Grill �̷�������?
    // ��������(������)�� ��Ű�� ������ �´� �����̱� �ѵ� ������ �ش�???
    private bool bInitialize = false;
    public string CookingFlowString = "";

    // �׷� ���� �ǵ��� ������ ������ƾ߰ڳ�....?? �ϴ� �α׸����Ʈ �����Ϳ��ٰ�...

    // �ϴ� �̷��� �� ����ῡ ������Ʈ �ٿ����� Plate���� ������������ Plate�� ����Ʈ�� Comp�� ���� ����� �̸��� �� ������, ��������� ���� Flow�� ��´�.....?

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
        // ���� SetData���� FlowString�� �̴ϼ��� �� ���� ȣ���� �Ǿ����� "Potato,Slice,"�� �� ���� ��

        var ItemData = ItemManager.Instance.GetItemDataByName(IngredientData.SlicedIngredientName);
        var CreatedItemBase = ItemBase.ItemBaseCreator.CreateItemBase(ItemData);

        CurrentWorldItem.SetItem(CreatedItemBase);
        // WorldItem�� IngredientData.SlicedIngredientName�� ������ �� ���� (Potato -> Slice Potato)

        // ���⼭ �����۸Ŵ����� �Ļ� ���� ������ ����ͼ� ������� ��
        // IngredientData = ItemManager.Instance.DoSomeThing();
    }

    private bool CookingStartedCheck()
    {
        // ��� �������� 0�ϰ�� ���� ���������� ���� ������
        return (BoiledValue == 0.0f && BoiledValue == 0.0f && FriedValue == 0.0f);
    }

    public void AccumulateGrilledValue(float fValue)
    {
        // �ϴ��� ó�� ������ ���������� �� ���Ŀ� ���� �̷��� ������
        // ó���� �����ٰ� ���߿� ������ ���¼� �������� ���Դٰ� �̷������� ������ü�� ����� ���̴°� �ϴ� ����....
        if (CookingStartedCheck())
        {
            CookingFlowString += "Grill,";
            // ���� SetData���� FlowString�� �̴ϼ��� �� ���� ȣ���� �Ǿ����� "Potato,Slice,"�� �� ���� ��

            var ItemData = ItemManager.Instance.GetItemDataByName(IngredientData.GrilledIngredientName);
            var CreatedItemBase = ItemBase.ItemBaseCreator.CreateItemBase(ItemData);

            CurrentWorldItem.SetItem(CreatedItemBase);
            // WorldItem�� IngredientData.GrilledIngredientName�� ������ �� ���� (Potato -> Grilled Potato)

            // ���⼭ �����۸Ŵ����� �Ļ� ���� ������ ����ͼ� ������� �� << �Ļ� ���� ������ �۾� �ʿ�
            // �ϼ��� �丮�� ������� �� �ϳ��� ����ᰡ ���->���->������� ������ �̷�����ٰ� �ϸ� �� �����Ǵ� �ϴ� ���߿� �����غ��°ɷ�
            // Potato�������� Fried Julienne cut Potato���� �׷��ϱ� ���ۺ��� �������� ���� ������ ������ �Ǵ� �����̱⶧����
            // �÷��̾��� ������������ ���� ������ �밨�ڸ� Ƣ��� ��� �ٽ� �� �ٸ����� �� ���·� ����� ���� �� Ƣ�� ������ �����ϴ��� FlowString���� �ɷ��� �� ������?
            // ���� ���� ������ ���������� �����Ѵٰ� �ϸ� FlowString�� üũ�ϴ� ����� �׳� PerfectFlow�� ��� ��Ұ� �ִ����� Ȯ���ϸ� �ɵ�
            // IngredientData = ItemManager.Instance.DoSomeThing();

            // �ƴϸ� ���� ���ۺ��� ���� ���¸� �����Ű�°� �ƴ϶� ���� Limit�� �༭ 30���� �̻� ������ ��� ��������� �����Ͱ� �ȴٴ��� �Ѵٰ� ġ��.......
            // �׸��� �̹� ���°� ����� ����Ḧ �ٸ� ��������� �����ְ� �� ���� Limit���� ���θ� ���Ĺ������Ⱑ �ȴٰų�.....
        }

        GrilledValue += fValue;
    }

    public void AccumulateBoiledValue(float fValue)
    {
        if (CookingStartedCheck())
        {
            CookingFlowString += "Boiled,";
            // ���� SetData���� FlowString�� �̴ϼ��� �� ���� ȣ���� �Ǿ����� "Potato,Boiled,"�� �� ���� ��
        }

        BoiledValue += fValue;
    }

    public void AccumulateFriedValue(float fValue)
    {
        if (CookingStartedCheck())
        {
            CookingFlowString += "Fried,";
            // ���� SetData���� FlowString�� �̴ϼ��� �� ���� ȣ���� �Ǿ����� "Potato,Fried,"�� �� ���� ��
        }

        FriedValue += fValue;
    }
}

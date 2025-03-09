using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CreateToolUI : MonoBehaviour
{
    public TMP_Dropdown RecipeDropDown;

    public Button ApplyButton;

    void Start()
    {
        RecipeDropDown = GetComponentInChildren<TMP_Dropdown>();
        RecipeDropDown.ClearOptions();
        
        var options = new System.Collections.Generic.List<string> { "Option 1", "Option 2", "Option 3" };
        
        RecipeDropDown.AddOptions(options);
        
        RecipeDropDown.onValueChanged.AddListener(OnDropdownValueChanged);
    }
    void OnDropdownValueChanged(int index)
    {
        Debug.Log($"선택된 옵션: {RecipeDropDown.options[index].text}");
    }


    void Update()
    {
        
    }
}

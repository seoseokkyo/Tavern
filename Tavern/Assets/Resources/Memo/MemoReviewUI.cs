using UnityEngine;
using UnityEngine.UI;


public class MemoReviewUI : MonoBehaviour
{
    public GameObject reviewUIPanel;
    public Transform foodsContentTransform;
    public UnityEngine.UI.Text extraNoteText;

    void Start()
    {
        reviewUIPanel.SetActive(false);
    }
    
    public void Initialize(MemoData data)
    {
        extraNoteText.text = data.extraNote;
        extraNoteText.enabled = true;
    }

    public void OpenUI()
    {
        reviewUIPanel.SetActive(true);
    }

    public void CloseUI()
    {
        reviewUIPanel.SetActive(false);
    }
}

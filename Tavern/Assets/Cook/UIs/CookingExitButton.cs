using UnityEngine;
using UnityEngine.UI;

public class CookingExitButton : MonoBehaviour
{
    [SerializeField] private Button exitButton;
    private ModeController modeController;
    public GameObject cookingUI;
    public GameObject player;

    private bool isActivated = false;

    // Start�� UI Active�� ���� ��� �÷���
    public bool bTestFlag = false;

    public void SetUIActivated(bool state)
    {
        isActivated = state;
    }

    void Start()
    {
        exitButton.onClick.AddListener(OnClickButton);

        if (null != player)
        {
            modeController = player.GetComponent<ModeController>();
        }

        if (!bTestFlag)
        {
            cookingUI.SetActive(false);
        }
    }

    void Update()
    {
        OnEscapeKeyDown();
    }

    void OnClickButton()
    {
        isActivated = false;
        // test UI ����
        cookingUI.SetActive(false);
        // Game Mode
        modeController.SetMode(false);
    }

    private void OnEscapeKeyDown()
    {
        KeyCode key = KeyCode.Escape;
        if (Input.GetKey(key) && isActivated)
        {
            OnClickButton();
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class MenuExitButton : MonoBehaviour
{
    [SerializeField] private Button exitButton;
    private ModeController modeController;
    public GameObject menuUI;
    public GameObject player;

    private bool isActivated = false;
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
            menuUI.SetActive(false);
        }
    }

    void Update()
    {
        OnEscapeKeyDown();
    }

    void OnClickButton()
    {
        isActivated = false;
        // test UI Áö¿ò
        menuUI.SetActive(false);
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

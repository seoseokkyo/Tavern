using UnityEngine;
using UnityEngine.UI;

public class ClickEventTest : MonoBehaviour
{
    [SerializeField] private Button exitButton;
    public GameObject popUI;
    public GameObject player;
    private ModeController modeController;

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
            popUI.SetActive(false);
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
        popUI.SetActive(false);
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

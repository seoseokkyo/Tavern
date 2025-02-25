using UnityEngine;
using UnityEngine.UI;

public class ClickEventTest : MonoBehaviour
{
    [SerializeField] private Button exitButton;
    public GameObject popUI;
    public GameObject player;
    private ModeController modeController;

    private bool isActivated = false;

    public void SetUIActivated(bool state)
    {
        isActivated = state;
    }

    void Start()
    {
        exitButton.onClick.AddListener(OnClickButton);
        modeController = player.GetComponent<ModeController>();
        popUI.SetActive(false);
    }

    void Update()
    {
        OnEscapeKeyDown();
    }

    void OnClickButton()
    {
        isActivated = false;
        // test UI Áö¿ò
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

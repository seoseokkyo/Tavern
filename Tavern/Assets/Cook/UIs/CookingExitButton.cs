using UnityEngine;
using UnityEngine.UI;

public class CookingExitButton : MonoBehaviour
{
    [SerializeField] private Button exitButton;
    private ModeController modeController;
    public GameObject cookingUI;
    public GameObject player;

    private bool isActivated = false;

    public void SetUIActivated(bool state)
    {
        isActivated = state;
    }

    void Start()
    {
        exitButton.onClick.AddListener(OnClickButton);
        modeController = player.GetComponent<ModeController>();
        cookingUI.SetActive(false);
    }

    void Update()
    {
        OnEscapeKeyDown();
    }

    void OnClickButton()
    {
        isActivated = false;
        // test UI Áö¿ò
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

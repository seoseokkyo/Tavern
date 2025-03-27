using Photon.Pun;
using Steamworks;
using System;
using System.Threading;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviourPunCallbacks
{
    [HideInInspector]
    public Canvas PlayerCanvas;

    public GameObject recipe;
    private SelectedRecipeUI selectedRecipeUI;
    private RecipeUI recipeUI;

    [HideInInspector]
    public ItemBase CurrentEquipmentItem
    {
        get
        {
            if (null != CurrentPlayer)
            {
                return CurrentPlayer.RightHandItem != null ? CurrentPlayer.RightHandItem.item : null;
            }
            else
            {
                return null;
            }
        }


    }

    [HideInInspector]
    public TavernPlayer CurrentPlayer = null;

    public Camera PlayerCamera;
    public Transform CameraTransform;

    public PhotonView PlayerPhotonView;

    private void Awake()
    {
        // 플레이어의 캔버스 생성
        GameObject PlayerCanvasObj = new GameObject("PlayerCanvas");
        PlayerCanvas = PlayerCanvasObj.AddComponent<Canvas>();

        PlayerCanvasObj.transform.SetParent(transform);
        PlayerCanvas.renderMode = RenderMode.ScreenSpaceOverlay;

        PlayerCamera.gameObject.transform.SetParent(this.gameObject.transform);
        PlayerCamera.transform.localPosition = CameraTransform.transform.localPosition;
        PlayerCamera.transform.localRotation = CameraTransform.transform.localRotation;
    }

    void Start()
    {
        CurrentPlayer = GetComponentInParent<TavernPlayer>();
        if (null == CurrentPlayer)
        {
            Debug.Log("CurrentPlayer Is Null");
        }

        if (photonView.IsMine)
        {
            PlayerCamera.gameObject.SetActive(true);

            var UIManager = GetComponentInParent<PlayerUIManager>();
            if (null != UIManager && null != UIManager.selectedRecipe)
            {
                selectedRecipeUI = UIManager.selectedRecipe.GetComponent<SelectedRecipeUI>();
                selectedRecipeUI.GetInventoryFromController(this);
            }
            else
            {
                Debug.Log("UIManager Is Null");
            }

            recipeUI = recipe.GetComponent<RecipeUI>();
            recipeUI.playerController = this;
        }
        else
        {
            PlayerCamera.gameObject.SetActive(false);
            Destroy(PlayerCamera);
        }
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1) || UnityEngine.Input.GetKeyDown(KeyCode.Keypad1))
        {
            TavernGameManager.Instance.ClientToServerUseItem("Chickens", 3);
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha2) || UnityEngine.Input.GetKeyDown(KeyCode.Keypad2))
        {
            TavernGameManager.Instance.ClientToServerUseItem("Breads", 2);
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha3) || UnityEngine.Input.GetKeyDown(KeyCode.Keypad3))
        {
            TavernGameManager.Instance.ClientToServerUseItem("Vegetable Soup", 2);
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha4) || UnityEngine.Input.GetKeyDown(KeyCode.Keypad4))
        {
            TavernGameManager.Instance.TestFunction_PassedTimeToLimit();
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha5) || UnityEngine.Input.GetKeyDown(KeyCode.Keypad5))
        {
            TavernGameManager.Instance.UserCheckedDailyResult();
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha6) || UnityEngine.Input.GetKeyDown(KeyCode.Keypad6))
        {

        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha7) || UnityEngine.Input.GetKeyDown(KeyCode.Keypad7))
        {

        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha8) || UnityEngine.Input.GetKeyDown(KeyCode.Keypad8))
        {
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha9) || UnityEngine.Input.GetKeyDown(KeyCode.Keypad9))
        {

        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha0) || UnityEngine.Input.GetKeyDown(KeyCode.Keypad0))
        {
            CurrentPlayer.ItemDetachFromRightHand();
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenuScene", LoadSceneMode.Single);
        }

        // Item Use
    }

    private void OnDestroy()
    {
        Destroy(PlayerCanvas.transform.root);
    }
}

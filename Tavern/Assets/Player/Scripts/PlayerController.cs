using Photon.Pun;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviourPunCallbacks
{
    [HideInInspector]
    public Canvas PlayerCanvas;

    public GameObject recipe;
    private SelectedRecipeUI selectedRecipeUI;
    private RecipeUI recipeUI;

    public GameObject memoReviewUIPrefab;  
    private MemoReviewUI memoReviewUIInstance;

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
        PlayerCanvas.AddComponent<GraphicRaycaster>();
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
            //if(photonView.IsMine)
            //{
            //    PhotonNetwork.LeaveRoom();                
            //}
        }

        // Memo 관련 

        if (UnityEngine.Input.GetMouseButtonDown(0)) 
        {
            if (CurrentPlayer.RightHandItem == null)
                return;

            if (CurrentPlayer.RightHandItem.item.CurrentItemData.itemName == "Memo")
                TryAttachMemoItem();
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (CurrentPlayer.RightHandItem == null)
                return;

            if (CurrentPlayer.RightHandItem != null &&
                CurrentPlayer.RightHandItem.item != null)
            {
                string name = CurrentPlayer.RightHandItem.item.CurrentItemData.itemName;
                if (name == "Memo")
                {
                    TryOpenMemoUI();
                }
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            if (CurrentPlayer.RightHandItem == null)
                return;

            if (CurrentPlayer.RightHandItem.item.CurrentItemData.itemName == "Memo")
            {
                if (memoReviewUIInstance != null)
                {
                    memoReviewUIInstance.CloseUI();
                }
            }
        }
    }

    private void TryAttachMemoItem()
    {
        var memoObject = CurrentPlayer.RightHandItem;
        var menoScript = memoObject.GetComponent<MenoScript>();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 30f, ~LayerMask.GetMask("UI")))
        {
            Vector3 attachPos = hit.point;
            Quaternion targetRotation = Quaternion.LookRotation(hit.normal);  

            // 메모 아이템 붙이기 처리
            menoScript.TryAttachMemo(attachPos, targetRotation);
        }
    }
    void TryOpenMemoUI()
    {
        if (CurrentPlayer.RightHandItem.item is MemoItemBase memoItem)
        {
            if (memoReviewUIInstance == null)
            {
                GameObject uiObj = Instantiate(memoReviewUIPrefab);
                memoReviewUIInstance = uiObj.GetComponent<MemoReviewUI>();
            }

            memoReviewUIInstance.Initialize(memoItem.orderedFoods, memoItem.extraNote);
            memoReviewUIInstance.OpenUI();
        }
    }

    private void OnDestroy()
    {
        Destroy(PlayerCanvas.transform.root);
    }
}

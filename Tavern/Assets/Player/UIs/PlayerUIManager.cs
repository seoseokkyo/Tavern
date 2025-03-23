using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    private PlayerController OwnerPlayerCon;

    public GameObject InteractCanvas_Prefab;
    public GameObject popUICanvas_Prefab;
    public GameObject selectedRecipe_Prefab;

    [HideInInspector]
    public GameObject InteractCanvas;

    [HideInInspector]
    public GameObject popUICanvas;

    [HideInInspector]
    public GameObject selectedRecipe;

    public delegate void OnPlayerUIInit();
    public OnPlayerUIInit OnPlayerUIInitEnd;

    private void Awake()
    {
        OwnerPlayerCon = GetComponentInParent<PlayerController>();

        SetUIFromPrefab(ref InteractCanvas, InteractCanvas_Prefab);
        SetUIFromPrefab(ref popUICanvas, popUICanvas_Prefab);
        SetUIFromPrefab(ref selectedRecipe, selectedRecipe_Prefab);

        OnPlayerUIInitEnd();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetUIFromPrefab(ref GameObject UIGameobj, GameObject UIPrefab)
    {
        if (UIPrefab != null)
        {
            UIGameobj = Instantiate(UIPrefab);
            UIGameobj.transform.SetParent(OwnerPlayerCon.PlayerCanvas.transform, false);

            UIGameobj.SetActive(false);
        }
        else
        {
            Debug.LogWarning($"{UIPrefab} prefab is not assigned!");
        }
    }
}

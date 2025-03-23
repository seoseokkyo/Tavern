using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    public GameObject InteractCanvas;
    public GameObject popUICanvas;

    private void Awake()
    {
        if (InteractCanvas != null)
            InteractCanvas = Instantiate(InteractCanvas);
        else
            Debug.LogWarning("Interact Canvas prefab is not assigned!");

        if (popUICanvas != null)
            popUICanvas = Instantiate(popUICanvas);
        else
            Debug.LogWarning("Pop-up Canvas prefab is not assigned!");
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

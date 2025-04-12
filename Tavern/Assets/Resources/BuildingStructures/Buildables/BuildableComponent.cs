using Photon.Pun;
using UnityEngine;

public class BuildableComponent : MonoBehaviour
{
    public Material validMaterial;
    public Material invalidMaterial;
    public GameObject previewMesh; 
    public GameObject furniturePrefab; 

    public float placementRadius = 2f; 
    public Collider placementAreaCollider; 

    private Camera playerCamera;
    private bool isPreviewActive = false;
    private bool isBuilding = false;

    public PhotonView photonView;

    void Start()
    {
        playerCamera = Camera.main;
    }

    public void StartBuilding()
    {
        isBuilding = true;

        previewMesh = Instantiate(furniturePrefab, transform.position, transform.rotation);
        previewMesh.GetComponent<Renderer>().material = IsPlacementValid() ? validMaterial : invalidMaterial;
        previewMesh.SetActive(true); 
    }

    public void StopBuilding()
    {
        isBuilding = false;
        if (previewMesh != null)
        {
            Destroy(previewMesh);
        }
    }

    public bool IsPlacementValid()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, placementRadius);
        foreach (Collider col in colliders)
        {
            if(col.CompareTag("Floor"))
            {
                continue;
            }
            else
            {
                return false;
            }
        }

        return true; 
    }

    public void UpdatePreviewMeshPosition()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition); 
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f)) 
        {
            if (hit.collider.CompareTag("Floor"))
            {
                previewMesh.transform.position = hit.point; 
                previewMesh.transform.up = hit.normal;

                if (IsPlacementValid())
                {
                    previewMesh.GetComponent<Renderer>().material = validMaterial; 
                }
                else
                {
                    previewMesh.GetComponent<Renderer>().material = invalidMaterial;
                }
            }
        }
    }

    public void PlaceFurniture(Vector3 position, Quaternion rotation)
    {
        if (IsPlacementValid())
        {
            furniturePrefab.transform.position = position;
            furniturePrefab.transform.rotation = rotation;
//            placementAreaCollider.enabled = false;

            StopBuilding();
            photonView.RPC("PlaceFurnitureRPC", RpcTarget.All, position, rotation);
        }
    }

    [PunRPC]
    void PlaceFurnitureRPC(Vector3 position, Quaternion rotation)
    {
        furniturePrefab.transform.position = position;
        furniturePrefab.transform.rotation = rotation;
    }

    public bool IsBuilding()
    {
        return isBuilding;
    }
}

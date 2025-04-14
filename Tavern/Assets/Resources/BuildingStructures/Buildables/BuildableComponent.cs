using Photon.Pun;
using UnityEngine;

public class BuildableComponent : MonoBehaviour
{
    public Material validMaterial;
    public Material invalidMaterial;
    public GameObject previewMesh; 
    public GameObject furniturePrefab;

    public GameObject originFurniture;

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

        Renderer previewMeshRenderer = previewMesh.GetComponent<Renderer>();
        if (previewMeshRenderer != null)
        {
            previewMeshRenderer.material = IsPlacementValid() ? validMaterial : invalidMaterial;
        }
        else
        {
            Debug.LogError("Preview Mesh Renderer is null!");
        }

        BoxCollider collider = previewMesh.GetComponent<BoxCollider>();
        if (collider != null)
        {
            collider.enabled = true;
            placementAreaCollider = collider;
        }
        else
        {
            Debug.LogError("Preview Mesh does not have a BoxCollider!");
        }

        previewMesh.SetActive(true);
    }

    public void StopBuilding()
    {
        isBuilding = false;
        if (previewMesh != null)
        {
            Destroy(previewMesh);
            previewMesh = null;
        }
    }

    public bool IsPlacementValid()
    {
        Collider[] colliders = Physics.OverlapBox(placementAreaCollider.bounds.center,
        placementAreaCollider.bounds.extents, Quaternion.identity);

        foreach (Collider col in colliders)
        {
            if (col.gameObject == previewMesh)
                continue;
            if (col.CompareTag("Untagged"))
                continue;
            if (col.CompareTag("Floor"))
                continue;

            if (!col.CompareTag("Floor"))
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

                UpdatePreviewMeshMaterials(IsPlacementValid() ? validMaterial : invalidMaterial);
            }
        }
    }

    public void UpdatePreviewMeshMaterials(Material m)
    {
        Renderer[] renderers = previewMesh.GetComponentsInChildren<Renderer>();
        foreach(var renderer in renderers)
        {
            renderer.material = m;
        }
    }

    public void PlaceFurniture(Vector3 position, Quaternion rotation)
    {       
        originFurniture.transform.position = position;
        originFurniture.transform.rotation = rotation;
        placementAreaCollider.enabled = false;
        photonView.RPC("PlaceFurnitureRPC", RpcTarget.AllBuffered, position, rotation);
        

        StopBuilding();
    }

    [PunRPC]
    void PlaceFurnitureRPC(Vector3 position, Quaternion rotation)
    {
        originFurniture.transform.position = position;
        originFurniture.transform.rotation = rotation;
    }

    public bool IsBuilding()
    {
        return isBuilding;
    }
}

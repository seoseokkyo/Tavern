using UnityEngine;

public class MemoMesh : MonoBehaviour
{
    public Texture2D texture;
    public string shaderName = "Standard";

    public float memoThickness = 0.05f;

    private void Awake()
    {
        MeshFilter filter = GetComponent<MeshFilter>();
        MeshRenderer renderer = GetComponent<MeshRenderer>();

        if (filter == null)
        {
            filter = gameObject.AddComponent<MeshFilter>();
        }
        if (renderer == null)
        {
            renderer = gameObject.AddComponent<MeshRenderer>();
        }

        if (renderer.material == null)
        {
            renderer.material = new Material(Shader.Find(shaderName));
        }

        if (renderer.material.mainTexture == null)
        {
            renderer.material.mainTexture = texture;
        }

        Mesh mesh = new();
        float width = 1.5f;  
        float height = 2f;   

        Vector3[] vertices = new Vector3[]
        {
            new Vector3(-width / 2, -height / 2, -memoThickness),  
            new Vector3(-width / 2, height / 2, -memoThickness),
            new Vector3(width / 2, height / 2, -memoThickness),
            new Vector3(width / 2, -height / 2, -memoThickness),

            new Vector3(width / 2, -height / 2, memoThickness),
            new Vector3(width / 2, height / 2, memoThickness),
            new Vector3(-width / 2, height / 2, memoThickness),
            new Vector3(-width / 2, -height / 2, memoThickness),

            new Vector3(-width / 2, -height / 2, memoThickness),
            new Vector3(-width / 2, -height / 2, -memoThickness),
            new Vector3(width / 2, -height / 2, -memoThickness),
            new Vector3(width / 2, -height / 2, memoThickness),

            new Vector3(-width / 2, height / 2, -memoThickness),
            new Vector3(-width / 2, height / 2, memoThickness),
            new Vector3(width / 2, height / 2, memoThickness),
            new Vector3(width / 2, height / 2, -memoThickness),

            new Vector3(-width / 2, -height / 2, memoThickness),
            new Vector3(-width / 2, height / 2, memoThickness),
            new Vector3(-width / 2, height / 2, -memoThickness),
            new Vector3(-width / 2, -height / 2, -memoThickness),

            new Vector3(width / 2, -height / 2, -memoThickness),
            new Vector3(width / 2, height / 2, -memoThickness),
            new Vector3(width / 2, height / 2, memoThickness),
            new Vector3(width / 2, -height / 2, memoThickness),
        };

        Vector2[] uv = new Vector2[]
        {
            new Vector2(0f, 0f),
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),

            new Vector2(0f, 0f),
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),

            new Vector2(0f, 0f),
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),

            new Vector2(0f, 0f),
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),

            new Vector2(0f, 0f),
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),

            new Vector2(0f, 0f),
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
        };

        int[] triangles = new int[]
        {
            0, 1, 2,
            3, 0, 2,

            4, 5, 6,
            7, 4, 6,

            8, 9, 10,
            11, 8, 10,

            12, 13, 14,
            15, 12, 14,

            16, 17, 18,
            19, 16, 18,

            20, 21, 22,
            23, 20, 22,
        };

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        filter.mesh = mesh;
    }
}

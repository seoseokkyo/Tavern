using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image ItemViewImage;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitData(Texture2D itemIcon, Transform parentTransform)
    {
        ItemViewImage = GetComponent<Image>();

        if (ItemViewImage != null)
        {
            if (itemIcon != null && ItemViewImage != null)
            {
                Rect rect = new Rect(0, 0, Mathf.Min(itemIcon.width, 100), Mathf.Min(itemIcon.height, 100));

                var temp = Sprite.Create(itemIcon, rect, new Vector2(0.5f, 0.5f));

                if (temp != null)
                {
                    ItemViewImage.sprite = temp;

                    transform.SetParent(parentTransform);
                }
            }
        }
    }
}

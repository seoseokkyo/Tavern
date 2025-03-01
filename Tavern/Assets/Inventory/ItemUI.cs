using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image ItemViewImage;
    public Image ItemCountbackground;
    public TextMeshProUGUI ItemCount;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitData(Texture2D itemIcon, Transform parentTransform, int itemCount)
    {
        if (itemCount > 1)
        {
            ItemCountbackground.enabled = true;

            ItemCount.text = itemCount.ToString();
            ItemCount.enabled = true;
        }
        else
        {
            ItemCountbackground.enabled = false;
            ItemCount.enabled = false;
        }

        if (ItemViewImage != null)
        {
            if (itemIcon != null && ItemViewImage != null)
            {
                Rect rect = new Rect(0, 0, Mathf.Min(itemIcon.width, 500), Mathf.Min(itemIcon.height, 500));

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

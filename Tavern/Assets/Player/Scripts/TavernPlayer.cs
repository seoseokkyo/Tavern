using Photon.Pun;
using UnityEngine;


public class TavernPlayer : MonoBehaviour
{
    public float MaxHP { get { return maxHP; } }
    public float CurrentHP { get { return currentHP; } }
    public float MoveSpeed { get { return moveSpeed; } }

    [SerializeField] protected float maxHP;
    [SerializeField] protected float currentHP;
    [SerializeField] protected float moveSpeed;

    public delegate void OnRightHandItemChanged(WorldItem itemBase);
    public OnRightHandItemChanged OnChanged;
    Transform RightHandSlot;

    [HideInInspector]
    public WorldItem RightHandItem;

    Animator PlayerAnimator;

    private PhotonView PV;

    public void OnUpdatePlayerStat(float _maxHP, float _currentHP, float _moveSpeed)
    {
        this.maxHP = _maxHP;
        this.currentHP = _currentHP;
        this.moveSpeed = _moveSpeed;

    }

    void Start()
    {
        PV = GetComponentInParent<PhotonView>();
    }

    void Update()
    {

    }

    private void Awake()
    {
        PlayerAnimator = gameObject.GetComponentInChildren<Animator>();
        if (null != PlayerAnimator)
        {
            Transform TempTransform = PlayerAnimator.GetBoneTransform(HumanBodyBones.RightHand).transform;

            int ChildCount = TempTransform.childCount;

            for (int i = 0; i < ChildCount; i++)
            {
                string ChildName = TempTransform.GetChild(i).name;

                if (ChildName == "PT_Right_Hand_Weapon_slot")
                {
                    RightHandSlot = TempTransform.GetChild(i);
                    break;
                }
            }
        }

        OnChanged += ItemAttachToRightHand;
    }

    public void ChangeHP(float fValue)
    {
        Debug.Log($"{currentHP} -> {currentHP + fValue}");
        currentHP += fValue;

        if (currentHP <= 0)
        {
            Debug.Log($"{this.ToString()} Is Die");
        }
    }

    public void ItemAttachToRightHand(WorldItem Item)
    {
        if (RightHandSlot == null)
        {
            Debug.Log("PT_Right_Hand_Weapon_slot Not Found");
            return;
        }

        // 기존 아이템 제거
        if (RightHandItem != null)
        {
            PV.RPC("SendToAllItemParentChange", RpcTarget.All, Item.photonView.ViewID, false);
        }

        if (Item != null)
        {
            //RightHandItem = ItemManager.Instance.ItemSpawn(itemBase, Vector3.zero, Quaternion.identity);

            PV.RPC("SendToAllItemParentChange", RpcTarget.All, Item.photonView.ViewID, true);
        }
    }

    [PunRPC]
    public void SendToAllItemParentChange(int ItemViewID, bool bGrab)
    {
        var TargetView = PhotonView.Find(ItemViewID);
        var NetItem = TargetView.gameObject.GetComponent<WorldItem>();

        if (bGrab)
        {
            RightHandItem = NetItem;
            RightHandItem.transform.SetParent(RightHandSlot, false);

            RightHandItem.transform.localPosition = RightHandItem.item.CurrentItemData.AttachLocation;
            RightHandItem.transform.localRotation = RightHandItem.item.CurrentItemData.AttachRotation;
            RightHandItem.transform.localScale = new Vector3(1, 1, 1);

            RightHandItem.GetComponent<Collider>().enabled = false;
        }
        else
        {
            RightHandItem.transform.parent = null;

            var ItemCollider = RightHandItem.GetComponent<Collider>().enabled = true;

            RightHandItem.ItemRigidbody.isKinematic = false;
            RightHandItem.ItemRigidbody.useGravity = false;

            Ray ray = new Ray(RightHandItem.transform.position, new Vector3(0, -1, 0));
            Physics.Raycast(ray, out RaycastHit hit, 10);

            Vector3 NewPosition = RightHandItem.transform.position;
            NewPosition.y = hit.point.y;

            RightHandItem.transform.SetPositionAndRotation(NewPosition, Quaternion.identity);
            RightHandItem = null;
        }
    }

    public void ItemDetachFromRightHand()
    {
        if (null != RightHandItem)
        {
            PV.RPC("SendToAllItemParentChange", RpcTarget.All, RightHandItem.photonView.ViewID, false);
        }
    }
}

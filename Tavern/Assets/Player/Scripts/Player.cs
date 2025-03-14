using UnityEngine;

public class Player : MonoBehaviour
{
    public float MaxHP { get { return maxHP; } }
    public float CurrentHP { get { return currentHP; } }
    public float MoveSpeed { get { return moveSpeed; } }

    [SerializeField] protected float maxHP;
    [SerializeField] protected float currentHP;
    [SerializeField] protected float moveSpeed;

    public void OnUpdatePlayerStat(float _maxHP, float _currentHP, float _moveSpeed)
    {
        this.maxHP = _maxHP;
        this.currentHP = _currentHP;
        this.moveSpeed = _moveSpeed;
    }

    void Start()
    {

    }

    void Update()
    {

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
}

using System;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Action whileMoving;
    public Action OnStopped;

    [SerializeField]
    private int testStrength;

    [SerializeField] 
    private float maxFuel;
    private float currentFuel;

    private Vector2 direction;
    private Rigidbody2D rb;

    public float GetRatio()
    {
        return currentFuel / maxFuel;   
    }
    private void Awake()
    {
        currentFuel = maxFuel;
        enabled= false;
        rb = GetComponent<Rigidbody2D>();
    }

    private bool _isMaxHeight = false;
    private void FixedUpdate()
    {
        if (transform.position.y > 10)
        {
            direction.y = Mathf.Min(direction.y, 0);
            if (!_isMaxHeight)
            {
                _isMaxHeight = true;
                PopUpManager.Instance.SpawnText("A blimp cannot fly too high", transform.position, Color.white);
            }
        }
        else
        {
            _isMaxHeight = false;
        }
        if (currentFuel <= 0) return;
        rb.AddForce(testStrength * Time.fixedDeltaTime * direction);
        currentFuel -= testStrength * Time.fixedDeltaTime * direction.magnitude;
        whileMoving();
    }

    public void Move(Vector2 dragVec)
    {
        this.enabled = true;
        direction = dragVec.normalized;
    }

    public void StopMove()
    {
        OnStopped();
        this.enabled = false;
    }

    public void Restore(int amount)
    {
        currentFuel += amount;
        if (currentFuel > maxFuel) currentFuel = maxFuel;
    }
}

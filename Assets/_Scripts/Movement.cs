using System;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Action OnStarted;
    public Action onFuelChanged;
    public Action whileMoving;
    public Action OnStopped;

    [SerializeField]
    private int testStrength;

    [SerializeField] 
    private float maxFuel;
    private float currentFuel;

    [SerializeField]
    private int maxHeight;

    private Vector2 direction;
    private Rigidbody2D rb;
    private bool m_started = false;

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
        CheckBorderMax();
        if (currentFuel <= 0) return;
        rb.AddForce(testStrength * rb.mass * Time.fixedDeltaTime * direction);
        currentFuel -= testStrength * Time.fixedDeltaTime * direction.magnitude;
        whileMoving();
        onFuelChanged();
    }

    private void CheckBorderMax()
    {
        if (transform.position.y > maxHeight)
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
    }

    public void Move(Vector2 dragVec)
    {
        this.enabled = true;
        direction = dragVec.normalized;
        if (!m_started)
        {
            m_started = true;
            OnStarted?.Invoke();
        }
    }

    public void StopMove()
    {
        OnStopped();
        this.enabled = false;
        m_started = false;
    }

    public void RestorePercent(int percent)
    {
        currentFuel += (float)percent/100 * maxFuel;
        if (currentFuel > maxFuel) currentFuel = maxFuel;
        onFuelChanged?.Invoke();
    }
}

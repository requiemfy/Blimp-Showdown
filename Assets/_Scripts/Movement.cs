using System;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Action whileMoving;
    [SerializeField] private float maxFuel;
    private float currentFuel;
    [SerializeField] private Vector2 maxSpd;
    private Vector2 direction;

    public float GetRatio()
    {
        return currentFuel / maxFuel;   
    }
    private void Awake()
    {
        currentFuel = maxFuel;
        enabled= false;
    }
    private void Update()
    {
        Move();
    }
    private void Move()
    {
        if (direction == Vector2.zero) 
        {
            enabled = false;
            return;
        }
        if (currentFuel <= 0)
        {
            Debug.Log("Out of fuel");
            StopMoveX();
            StopMoveY();
            return;
        }
        Vector2 moveVec = new(direction.normalized.x * maxSpd.x, direction.normalized.y * maxSpd.y);
        transform.position += Time.deltaTime * (Vector3) moveVec;
        currentFuel -= moveVec.magnitude * Time.deltaTime;
        whileMoving();
    }

    public void StartMoveRight()
    {
        direction = new(1, direction.y);
        enabled = true;
    }
    public void StartMoveLeft()
    {
        direction = new(-1, direction.y);
        enabled = true;
    }
    public void StopMoveX()
    {
        direction = new(0, direction.y);
    }

    public void StartMoveUp()
    {
        direction = new(direction.x, 1);
        enabled = true;
    }
    public void StartMoveDown()
    {
        direction = new(direction.x, -1);
        enabled = true;
    }
    public void StopMoveY()
    {
        direction = new(direction.x, 0);
    }

    public void Restore(int amount)
    {
        currentFuel += amount;
        if (currentFuel > maxFuel) currentFuel = maxFuel;
    }
}

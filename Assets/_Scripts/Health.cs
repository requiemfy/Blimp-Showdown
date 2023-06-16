using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class Health : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }
    public Action OnDamageTaken;
    public Action OnDeath;

    private void Awake()
    {
        if (healthBar) healthBar.gameObject.SetActive(false);
    }
    public void SetMaxHealth(int value)
    {
        MaxHealth = value;
        CurrentHealth = value;
    }
    public float GetRatio()
    {
        return (float)CurrentHealth / MaxHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(tag)) return;
        var bullet = collision.GetComponent<Bullet>();
        DecreaseHealth(bullet.Damage);
    }


    private bool isAlive = true;
    public void DecreaseHealth(int val)
    {
        CurrentHealth -= val;
        SwitchMaterial();
        OnDamageTaken?.Invoke();
        if (healthBar) 
        {
            DOTween.Kill(healthBar);
            healthBar.gameObject.SetActive(true);
            healthBar.DOFillAmount((float)GetRatio(), 0.3f);
        }
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            if (isAlive)
            {
                OnDeath?.Invoke();
                isAlive = false;
            }
        }
    }

    #region FLASH
    [SerializeField] private SpriteRenderer[] stunRens;
    [SerializeField] private Material flashMat;
    private Color[] originalCol;
    private Material[] originalMat;

    private void Start()
    {
        originalCol = new Color[stunRens.Length];
        originalMat = new Material[stunRens.Length];
        for (int i = 0; i < stunRens.Length; i++)
        {
            originalCol[i] = stunRens[i].color;
            originalMat[i] = stunRens[i].material;
        }
    }
    private void SwitchMaterial()
    {
        StartCoroutine(SwitchMatrl());
        IEnumerator SwitchMatrl()
        {
            for (int i = 0; i < stunRens.Length; i++)
            {
                stunRens[i].color = Color.white;
                if (flashMat) stunRens[i].material = flashMat;
            }
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < stunRens.Length; i++)
            {
                stunRens[i].color = originalCol[i];
                stunRens[i].material = originalMat[i];
            }
        }
    }
    #endregion
}

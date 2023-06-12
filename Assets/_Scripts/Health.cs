using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class Health : MonoBehaviour
{
    [SerializeField] Image healthBar;
    [HideInInspector] public int maxHealth;
    [HideInInspector] public int currentHealth;
    public Action onDamageTaken;
    public Action onDeath;

    private void Awake()
    {
        if (healthBar) healthBar.gameObject.SetActive(false);
    }
    public void SetMaxHealth(int value)
    {
        maxHealth = value;
        currentHealth = value;
    }
    public float GetRatio()
    {
        return (float)currentHealth / maxHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(tag)) return;
        var bullet = collision.GetComponent<Bullet>();
        DecreaseHealth(bullet.damage);
    }


    private bool isAlive = true;
    public void DecreaseHealth(int val)
    {
        currentHealth -= val;
        SwitchMaterial();
        onDamageTaken?.Invoke();
        if (healthBar) 
        {
            DOTween.Kill(healthBar);
            healthBar.gameObject.SetActive(true);
            healthBar.DOFillAmount((float)GetRatio(), 0.3f);
        }
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            if (isAlive)
            {
                onDeath?.Invoke();
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

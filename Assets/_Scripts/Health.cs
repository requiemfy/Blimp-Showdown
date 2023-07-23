using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Health : MonoBehaviour
{
    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }
    public Action OnDamageTaken;
    public Action OnDeath;

    [SerializeField] private Bar healthBar;
    private void Awake()
    {
        if (healthBar) ShowHealthBar(false);
    }
    private void Start()
    {
        healthBar.SetFill(CurrentHealth, MaxHealth);
        Start_Visual();
    }
    public void ShowHealthBar(bool state)
    {
        healthBar.gameObject.SetActive(state);
    }
    public void Construct(int maxHealth, string tag = null)
    {
        if (tag != null) this.tag = tag;
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
    }
    public float GetRatio()
    {
        return (float)CurrentHealth / MaxHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet")) return;
        var bullet = collision.GetComponentInParent<Bullet>();
        DecreaseHealth(bullet.Damage);
        PopUpManager.Instance.SpawnText(bullet.Damage.ToString(), collision.transform.position, CustomColors.Red);
    }

    //take damage
    private bool isAlive = true;
    public void DecreaseHealth(int val)
    {
        CurrentHealth -= val;
        SwitchMaterial();
        CountDownHealthBar();
        OnDamageTaken?.Invoke();
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            if (isAlive)
            {
                OnDeath?.Invoke();
                isAlive = false;
                GetComponent<Collider2D>().enabled = false;
            }
        }
        if (healthBar) 
        {
            DOTween.Kill(healthBar);
            healthBar.gameObject.SetActive(true);
            healthBar.SetFill(CurrentHealth, MaxHealth);
        }
    }

    //healthbar timer
    private Coroutine _currentRoutine;
    private void CountDownHealthBar()
    {
        if (_currentRoutine != null) StopCoroutine(_currentRoutine);
        _currentRoutine = StartCoroutine(ShowHealthBarCO());
        IEnumerator ShowHealthBarCO()
        {
            ShowHealthBar(true);
            yield return new WaitForSeconds(2f);
            ShowHealthBar(false);
        }
    }

    #region FLASH
    [SerializeField] private SpriteRenderer[] stunRens;
    [SerializeField] private Material flashMat;
    private Color[] originalCol;
    private Material[] originalMat;

    private void Start_Visual()
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

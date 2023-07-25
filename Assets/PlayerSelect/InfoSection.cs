using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoSection : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI nameTMP;
    [SerializeField] private TextMeshProUGUI energyTMP;
    [SerializeField] private TextMeshProUGUI healthTMP;
    [SerializeField] private TextMeshProUGUI damageTMP;
    [SerializeField] private TextMeshProUGUI bulletCountTMP;

    [Header("Button")]
    [SerializeField] private Button closeBtn;

    private void Start()
    {
        closeBtn.onClick.AddListener(() => { gameObject.SetActive(false); });
    }

    public void ShowInfoOf(WeaponType weapon)
    {
        gameObject.SetActive(true);
        //flip card animation
        nameTMP.text = weapon.name;
        energyTMP.text = $"{weapon.energyCost}";
        healthTMP.text = $"{weapon.health}";
        damageTMP.text = $"{weapon.damage}";
        bulletCountTMP.text = $"x{weapon.bulletCount}";
    }
}

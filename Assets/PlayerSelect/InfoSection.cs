using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoSection : MonoBehaviour
{
    [Header("Header")]
    [SerializeField] TextMeshProUGUI nameTMP;
    [SerializeField] TextMeshProUGUI energyTMP;
    [SerializeField] Image thumbnail;

    [Header("Body")]
    [SerializeField] TextMeshProUGUI healthTMP;
    [SerializeField] TextMeshProUGUI damageTMP;
    [SerializeField] TextMeshProUGUI rangeTMP;
    [SerializeField] TextMeshProUGUI explodeRadiusTMP;
    [SerializeField] TextMeshProUGUI bulletCountTMP;
    [SerializeField] TextMeshProUGUI trajectoryTMP;

    [Header("Button")]
    [SerializeField] private Button closeBtn;

    private void Start()
    {
        gameObject.SetActive(false);
        closeBtn.onClick.AddListener(() => { gameObject.SetActive(false); });
    }

    public void ShowInfoOf(WeaponType weapon)
    {
        gameObject.SetActive(true);

        //flip card animation

        //header
        nameTMP.text = weapon.name;
        energyTMP.text = $"{weapon.energyCost}";
        thumbnail.sprite = weapon.barrel;

        //body
        healthTMP.text = $"{weapon.health}";
        damageTMP.text = $"{weapon.damage}";
        rangeTMP.text = $"{weapon.range}";
        explodeRadiusTMP.text = $"{weapon.explodeRadius}";
        bulletCountTMP.text = $"x{weapon.bulletCount}";
        trajectoryTMP.text = weapon.isGravityAffected ? "parabola" : "straight line";
    }
}

using System.Collections;
using TMPro;
using UnityEngine;

public class QuickInfo : MonoBehaviour
{
    public static QuickInfo Instance { get; private set; }

    [SerializeField] private GameObject quickInfoMenu;
    [SerializeField] private TextMeshProUGUI title;
    private Camera cam;
    private void Awake()
    {
        Instance = this;
        cam = Camera.main;
        quickInfoMenu.SetActive(false);
    }

    public void StartCheckHold(object targetIfHold)
    {
        StopAllCoroutines();
        StartCoroutine(HoldCounterCO());
        IEnumerator HoldCounterCO()
        {
            yield return new WaitForSeconds(0.65f);
            OnHold();
        }
        void OnHold()
        {
            quickInfoMenu.SetActive(true);
            if (targetIfHold is WeaponController weapon)
            {
                //show weapon quickInfo
                quickInfoMenu.transform.position = cam.WorldToScreenPoint(weapon.transform.position) + new Vector3(0,125,0);
                title.text = $"{weapon.health.CurrentHealth}/{weapon.health.MaxHealth}";
            }
            else if (targetIfHold is PlayerController player)
            {
                //show player quickInfo
                quickInfoMenu.transform.position = cam.WorldToScreenPoint(player.transform.position) + new Vector3(0, 125, 0);
                title.text = $"{player.health.CurrentHealth}/{player.health.MaxHealth}";
            }
        }
    }

    public void StopCheckHold()
    {
        quickInfoMenu.SetActive(false);
        StopAllCoroutines();
    }

}

using DG.Tweening;
using TMPro;
using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    public static PopUpManager Instance { get; private set; }
    [SerializeField]
    private TextMeshPro textPrefab;
    private void Awake()
    {
        Instance = this;
    }
    public void SpawnText(string strg, Vector2 position, Color32 color)
    {
        TextMeshPro TMP = Instantiate(textPrefab);
        TMP.text = strg;
        TMP.color = color;
        TMP.transform.position = position + new Vector2(Random.Range(-2f,2f), Random.Range(0f,2f));
        TMP.transform.DOMoveY(position.y + 3, duration: 1.5f)
            .onComplete = () => TMP.DOColor(color.ChangeAlpha(0), duration: 0.5f)
            .onComplete = () => Destroy(TMP.gameObject);
    }
}

using TMPro;
using UnityEngine;

public class VersionText : MonoBehaviour
{
    [SerializeField]
    private string m_prefix;
    [SerializeField]
    private TextMeshProUGUI m_tmp;

    private void Awake()
    {
        m_tmp.text =  $"{m_prefix} {Application.version}";
    }
}

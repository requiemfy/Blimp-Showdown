using TMPro;
using UnityEngine;

public class VersionText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_tmp;

    private void Awake()
    {
        m_tmp.text = Application.version;
    }
}

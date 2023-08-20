using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class ParallaxRawImg : MonoBehaviour
{
    [SerializeField] Vector2 parallaxAmount;
    [SerializeField] RawImage rawImage;
    private void Update()
    {
        rawImage.uvRect = new Rect(rawImage.uvRect.position + parallaxAmount*Time.deltaTime, rawImage.uvRect.size);
    }
}

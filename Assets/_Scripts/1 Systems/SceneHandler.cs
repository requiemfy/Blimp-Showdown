using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler Instance { get; private set; }
    [SerializeField]
    private CanvasGroup m_loadingScreen;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        m_loadingScreen.gameObject.SetActive(false);
        m_loadingScreen.alpha = 0;
    }
    public void LoadScene(int index)
    {
        StartCoroutine(LoadSceneSequence());
        IEnumerator LoadSceneSequence()
        {
            m_loadingScreen.gameObject.SetActive(true);
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);
            asyncLoad.allowSceneActivation = false;
            bool adClosed = false;
            m_loadingScreen.DOFade(1, duration: 1)
                .onComplete = () => AdsManager.Instance.ShowAd(()=>
                {
                    adClosed = true;
                    asyncLoad.allowSceneActivation = true;
                });

            yield return new WaitUntil(() => asyncLoad.isDone && adClosed);
            
            m_loadingScreen.DOFade(0, duration: 1)
                .onComplete = () => m_loadingScreen.gameObject.SetActive(false);
        }
    }
}

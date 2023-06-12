using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    [SerializeField] private float speed;
    private Transform cam;
    private Transform follow;
    private bool onlyX = false;
    private void Awake()
    {
        Instance= this;
        cam = Camera.main.transform;
    }
    private Vector3 targetPos;
    public Vector2 offset;
    private void Update()
    {
        Vector3 camPos = cam.position;
        if (follow)
        {
            float X = follow.position.x + offset.x;
            float Y = onlyX? camPos.y : follow.position.y + offset.y;
            targetPos = new Vector3(X, Y, camPos.z);
            StopAllCoroutines();
        }
        else
        {
            if (!triggered) StartCoroutine(BackToCurrentPlayerCO());
        }
        cam.position = Vector3.Lerp(camPos, targetPos, Time.deltaTime * speed);
    }
    public void SetFollow(Transform transform, bool onlyX = false, float camSpeed = 1)
    {
        follow = transform;
        this.onlyX = onlyX;
        speed = camSpeed;
        offset = Vector2.zero;
        triggered = false;
    }

    private Transform currentPlayer;
    public void SetCurrentPlayer(Transform transform)
    {
        currentPlayer = transform;
        SetFollow(currentPlayer);
    }

    private bool triggered = false;
    private IEnumerator BackToCurrentPlayerCO()
    {
        triggered = true;
        yield return new WaitForSeconds(1.5f);
        SetFollow(currentPlayer);
    }
}


using DG.Tweening;
using System;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Action onTurnEnded;
    public Team[] OpennedTeams { get; private set; }
    public Vector2 Wind;

    [SerializeField] private PlayerController playerPrefab;
    [SerializeField] private GameOverScreen gameOverScreen;
    [SerializeField] private Image cameraRaycast;

    private int _playerRemaining;
    private GameObject[] _respawnPoints;

    private void Awake()
    {
        Instance = this;
        OpennedTeams = DataPersistence.GetOpenedTeams();
        _playerRemaining = OpennedTeams.Length;
        RandomizeTurnOrder();
    }
    private void RandomizeTurnOrder()
    {
        Random random = new();
        OpennedTeams = OpennedTeams.OrderBy(x => random.Next()).ToArray();
    }
    private IEnumerator Start()
    {
        GetRespawnPoints();
        SpawnPlayers();
        EnableCamControll(false);

        //cam zoom animation
        var cineMana = CinemachineManager.Instance;
        cineMana.OrthographicSize = 30;
        DOTween.To(() => cineMana.OrthographicSize, x => cineMana.OrthographicSize = x, 15, duration: 3).SetEase(Ease.InOutCubic);

        //go through players
        foreach (var team in OpennedTeams)
        {
            CinemachineManager.Instance.SetFollow(DataPersistence.Get(team).Controller.transform);
            yield return new WaitForSeconds(2);
        }

        //cam controll on
        EnableCamControll(true);
        var firstTeam = OpennedTeams[0];
        SetTurn(firstTeam);
        HUD.Instance.FindTurnIndicator(firstTeam).DOColor(firstTeam.GetTeamColor(), duration: 0.5f);
    }
    private void GetRespawnPoints()
    {
        _respawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
        Random random = new();
        _respawnPoints = _respawnPoints.OrderBy(x => random.Next()).ToArray();
    }
    private void SpawnPlayers()
    {
        int i = 0;
        foreach (Team team in OpennedTeams)
        {
            var playerCtrl = Instantiate(playerPrefab);
            playerCtrl.Construct(team);
            playerCtrl.transform.position = _respawnPoints[i].transform.position;
            i++;
        }
    }
    private void EnableCamControll(bool value)
    {
        cameraRaycast.raycastTarget = value;
        CinemachineManager.Instance.enabled = value;
    }



    private int current = 0;
    public void NextTurn()
    {
        //before
        var before = OpennedTeams[current];
        HUD.Instance.FindTurnIndicator(before).DOColor(before.GetTeamColor().ChangeAlpha(50), duration: 0.5f);
        onTurnEnded();

        //after
        current++;
        current %= OpennedTeams.Length;
        if (DataPersistence.Get(OpennedTeams[current]).Controller.WeaponLeft == 0)
        {
            Debug.Log("Ship shinked" + current);
            NextTurn();
            return;
        }
        var after = OpennedTeams[current];
        SetTurn(after);
        HUD.Instance.FindTurnIndicator(after).DOColor(after.GetTeamColor(), duration: 0.5f);
    }
    private void SetTurn(Team team)
    {
        StopAllCoroutines();
        StartCoroutine(SetTurnRoutine());
        IEnumerator SetTurnRoutine()
        {
            foreach (Team otherTeam in OpennedTeams)
            {
                if (otherTeam == team)
                {
                    continue;
                }
                DataPersistence.Get(otherTeam).Controller.IsInTurn = false;
            }
            var target = DataPersistence.Get(team).Controller;
            target.IsInTurn = true;
            CinemachineManager.Instance.SetFollow(target.transform, isPlayer: true);
            HUD.Instance.StartObserveWeapon(null);
            HUD.Instance.StartObservePlayer(null);
            yield return new WaitForSeconds(3);
            HUD.Instance.StartObservePlayer(target);
        }
    }

    public void DecreasePlayerRemaining(Team team)
    {
        _playerRemaining--;
        DataPersistence.Get(team).isDestroyed = true;
        if (_playerRemaining > 1) return;
        gameOverScreen.Show();
    }
}

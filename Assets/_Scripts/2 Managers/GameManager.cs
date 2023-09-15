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
    public Action onCycleEnded;
    public Team[] OpennedTeams { get; private set; }
    public Vector2 Wind;
    public bool IsInputEnabled = false;

    [SerializeField] private PlayerController playerPrefab;
    [SerializeField] private GameOverScreen gameOverScreen;

    private int m_turnCount;
    private int m_playerRemaining;
    private GameObject[] m_respawnPoints;

    private void Awake()
    {
        Instance = this;
        OpennedTeams = DataPersistence.GetOpenedTeams();
        m_playerRemaining = OpennedTeams.Length;
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
        IsInputEnabled = false;

        //cam zoom animation
        var cineMana = CinemachineManager.Instance;
        cineMana.OrthographicSize = 30;
        DOTween.To(() => cineMana.OrthographicSize, x => cineMana.OrthographicSize = x, 15, duration: 3).SetEase(Ease.InOutCubic);

        //go through players
        float timePerShip = 1.75f;
        for (int i=OpennedTeams.Length-1; i>=1; i--)
        {
            CinemachineManager.Instance.SetFollow(DataPersistence.Get(OpennedTeams[i]).Controller.transform);
            yield return new WaitForSeconds(timePerShip);
        }

        //cam controll on
        var firstTeam = OpennedTeams[0];
        SetTurn(firstTeam);
        HUD.Instance.FindTurnIndicator(firstTeam).DOColor(firstTeam.GetTeamColor(), duration: 0.5f);
    }
    private void GetRespawnPoints()
    {
        m_respawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
        Random random = new();
        m_respawnPoints = m_respawnPoints.OrderBy(x => random.Next()).ToArray();
    }
    private void SpawnPlayers()
    {
        int i = 0;
        foreach (Team team in OpennedTeams)
        {
            var playerCtrl = Instantiate(playerPrefab);
            playerCtrl.Construct(team);
            playerCtrl.transform.position = m_respawnPoints[i].transform.position;
            i++;
        }
    }



    private int current = 0;
    public void NextTurn()
    {
        //before
        var before = OpennedTeams[current];
        HUD.Instance.FindTurnIndicator(before).DOColor(before.GetTeamColor().ChangeAlpha(50), duration: 0.5f);

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
        onTurnEnded();
        SetTurn(after);
        HUD.Instance.FindTurnIndicator(after).DOColor(after.GetTeamColor(), duration: 0.5f);

        //cycle 
        m_turnCount++;
        if (m_turnCount % m_playerRemaining == 0) onCycleEnded();
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
            HUD.Instance.StartObserveWeapon(null);
            HUD.Instance.StartObservePlayer(null);
            yield return new WaitForSeconds(2);
            CinemachineManager.Instance.SetFollow(target.transform, isPlayer: true);
            yield return new WaitForSeconds(2);
            HUD.Instance.StartObservePlayer(target);
            IsInputEnabled = true;
        }
    }

    public void DecreasePlayerRemaining(Team team)
    {
        m_playerRemaining--;
        DataPersistence.Get(team).isDestroyed = true;
        if (m_playerRemaining > 1) return;
        gameOverScreen.Show();
    }
}

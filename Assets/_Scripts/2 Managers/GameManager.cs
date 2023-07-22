using System;
using System.Collections;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Action onTurnEnded;
    public Team[] OpennedTeams { get; private set; }
    public Vector2 wind;

    [SerializeField] private PlayerController playerPrefab;

    private GameObject[] _respawnPoints;

    private void Awake()
    {
        Instance = this;
        OpennedTeams = DataPersistence.GetOpenedTeams();
        RandomizeTurnOrder();
    }
    private void RandomizeTurnOrder()
    {
        Random random = new();
        OpennedTeams = OpennedTeams.OrderBy(x => random.Next()).ToArray();
    }
    private void Start()
    {
        GetRespawnPoints();
        SpawnPlayers();
        SetTurn(OpennedTeams[0]);
        HUD.Instance.FindTurnIndicator(OpennedTeams[0]).color = OpennedTeams[0].GetTeamColor();
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



    private int current = 0;
    public void NextTurn()
    {
        //before
        var before = OpennedTeams[current];
        HUD.Instance.FindTurnIndicator(before).color = before.GetTeamColor().ChangeAlpha(50);
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
        HUD.Instance.FindTurnIndicator(after).color = after.GetTeamColor();
    }
    private void SetTurn(Team team)
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
        HUD.Instance.StartObservePlayer(target);
    }
}

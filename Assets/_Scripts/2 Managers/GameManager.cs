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
    public Action<Team, Team> afterTurnChanged;
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
        Team before = OpennedTeams[current];
        onTurnEnded();
        current++;
        current %= OpennedTeams.Length;
        if (DataPersistence.Get(OpennedTeams[current]).Controller.WeaponLeft == 0)
        {
            Debug.Log("Ship shinked" + current);
            NextTurn();
            return;
        }
        SetTurn(OpennedTeams[current]);
        afterTurnChanged?.Invoke(before, OpennedTeams[current]);
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
        CameraManager.Instance.SetCurrentPlayer(target.transform);
        HUD.Instance.StartObserveWeapon(null);
        HUD.Instance.StartObservePlayer(target);
    }
}

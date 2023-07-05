using System;
using System.Collections;
using System.Linq;
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
        SpawnPlayers();
        SetTurn(OpennedTeams[0]);
    }
    private void SpawnPlayers()
    {
        foreach (Team team in OpennedTeams)
        {
            var playerCtrl = Instantiate(playerPrefab);
            playerCtrl.Construct(team);
            playerCtrl.transform.position = new Vector2(UnityEngine.Random.Range(1, 15), UnityEngine.Random.Range(1, 15));
        }
    }



    private int current = 0;
    public void NextTurn()
    {
        Team before = OpennedTeams[current];
        onTurnEnded();
        current++;
        current %= OpennedTeams.Length;
        if (OpennedTeams[current].GetContrl().WeaponLeft == 0)
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
            otherTeam.GetContrl().IsInTurn = false;
        }
        var target = team.GetContrl();
        target.IsInTurn = true;
        CameraManager.Instance.SetCurrentPlayer(target.transform);
        HUD.Instance.StartObserveWeapon(null);
        HUD.Instance.StartObservePlayer(target);
    }
}

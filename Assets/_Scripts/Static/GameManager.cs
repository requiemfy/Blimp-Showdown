using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Team[] OpennedTeams { get; private set; }
    public Action onTurnEnded;
    public Vector2 wind;

    [SerializeField] private PlayerController playerPrefab;
    private void Awake()
    {
        Instance = this;
        SpawnPlayers();
    }
    private void SpawnPlayers()
    {
        OpennedTeams = DataPersistence.GetOpenedTeams();
        foreach (Team team in OpennedTeams)
        {
            var playerCtrl = Instantiate(playerPrefab);
            team.Set(playerCtrl);
            var WeaponTypes = team.GetWeapons();
            playerCtrl.Construct(team, WeaponTypes);
            playerCtrl.transform.position = new Vector2(UnityEngine.Random.Range(1, 15), UnityEngine.Random.Range(1, 15));
        }
    }


    private void Start()
    {
        SetTurn(OpennedTeams[0]);
    }

    private int i = 0;
    public void NextTurn()
    {
        onTurnEnded();
        i++;
        i %= OpennedTeams.Length;
        if (OpennedTeams[i].GetContrl().WeaponLeft == 0)
        {
            Debug.Log("Ship shinked" + i);
            NextTurn();
            return;
        }
        SetTurn(OpennedTeams[i]);
    }

    private IEnumerator TimedManageTurn()
    {
        while(gameObject.activeInHierarchy)
        {
            NextTurn();
            yield return new WaitForSeconds(8);
        }
    }

    private void SetTurn(Team team)
    {
        var target = team.GetContrl();
        CameraManager.Instance.SetCurrentPlayer(target.transform);
        HUD.Instance.StartObserveWeapon(null);
        HUD.Instance.StartObservePlayer(target);
        target.IsInTurn = true;

        foreach (Team otherTeam in OpennedTeams)
        {
            if (otherTeam == team)
            {
                continue;
            }
            otherTeam.GetContrl().IsInTurn = false;
        }
    }
}

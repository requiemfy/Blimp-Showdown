using System;
using System.Collections.Generic;
using UnityEngine;

public static class DataPersistence
{
    private static TeamData[] _teamDatabase = new TeamData[4];//all null

    //METHODS
    public static void ClearTeamDB()
    {
        _teamDatabase = new TeamData[_teamDatabase.Length]; //all null
    }
    public static void Rematch()
    {
        foreach(Team team in GetOpenedTeams())
        {
            Get(team).isDestroyed = false;
        }
    }
    public static Team[] GetOpenedTeams()
    {
        var result = new List<Team>();
        for(int i=0; i<_teamDatabase.Length; i++)
        {
            if (_teamDatabase[i] == null) continue;
            result.Add((Team)i);
        }
        return result.ToArray();
    }

    //PUSH
    public static void Push(Team team, TeamData data)
    {
        _teamDatabase[(int)team] = data;
    }
    public static void Push(Team team, PlayerController playerCtrl)
    {
        _teamDatabase[(int)team].Controller = playerCtrl;
    }

    //GET
    public static TeamData Get(Team team)
    {
        return _teamDatabase[(int)team];
    }
    
} 
public class TeamData {
    public WeaponType[] Weapons { get; private set; }
    public PlayerController Controller;
    public bool isDestroyed = false;

    public TeamData(WeaponType[] weapons)
    {
        Weapons = weapons;
    }
}


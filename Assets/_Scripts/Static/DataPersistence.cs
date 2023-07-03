using System;
using System.Collections.Generic;
using UnityEngine;

public static class DataPersistence
{
    private static TeamData[] TeamDatas = new TeamData[4];//all null

    //METHODS
    public static void ClearPlayers()
    {
        TeamDatas = new TeamData[TeamDatas.Length]; //all null
    }
    public static Team[] GetOpenedTeams()
    {
        var result = new List<Team>();
        for(int i=0; i<TeamDatas.Length; i++)
        {
            if (TeamDatas[i] == null) continue;
            result.Add((Team)i);
        }
        return result.ToArray();
    }
    public static void Open(this Team team)
    {
        if (TeamDatas[(int)team] != null)
        {
            Debug.LogWarning("Try not to open team that already openned");
            return;
        }
        TeamDatas[(int)team] = new TeamData();
    }

    //SET
    public static void Set(this Team team, WeaponType weapon, int index)
    {
        var destination = TeamDatas[(int)team].weapons;
        destination[index] = ScriptableObject.CreateInstance<WeaponType>();
        destination[index] = weapon;
    }
    public static void Set(this Team team, PlayerController ctrl)
    {
        TeamDatas[(int)team].controller = ctrl;
    }

    //GET
    public static WeaponType[] GetWeapons(this Team team)
    {
        return TeamDatas[(int)team].weapons;
    }
    public static PlayerController GetContrl(this Team team)
    {
        return TeamDatas[(int)team].controller;
    }
} 
public class TeamData {
    public WeaponType[] weapons = new WeaponType[3];
    public PlayerController controller;
}


using System;
using System.Collections.Generic;
using UnityEngine;

public static class DataPersistence
{
    private static TeamData[] _teamDatas = new TeamData[4];//all null

    //METHODS
    public static void ClearPlayers()
    {
        _teamDatas = new TeamData[_teamDatas.Length]; //all null
    }
    public static Team[] GetOpenedTeams()
    {
        var result = new List<Team>();
        for(int i=0; i<_teamDatas.Length; i++)
        {
            if (_teamDatas[i] == null) continue;
            result.Add((Team)i);
        }
        return result.ToArray();
    }
    public static void Open(this Team team)
    {
        if (_teamDatas[(int)team] != null)
        {
            Debug.LogWarning("Try not to open team that already openned");
            return;
        }
        _teamDatas[(int)team] = new TeamData();
    }

    //SET
    public static void Set(this Team team, WeaponType weapon, int index)
    {
        var destination = _teamDatas[(int)team].weapons;
        destination[index] = ScriptableObject.CreateInstance<WeaponType>();
        destination[index] = weapon;
    }
    public static void Set(this Team team, PlayerController ctrl)
    {
        _teamDatas[(int)team].controller = ctrl;
    }

    //GET
    public static WeaponType[] GetWeapons(this Team team)
    {
        return _teamDatas[(int)team].weapons;
    }
    public static PlayerController GetContrl(this Team team)
    {
        return _teamDatas[(int)team].controller;
    }
    public static Color32 GetTeamColor(this Team team)
    {
        switch (team)
        {
            case Team.Red:
                return CustomColors.Red;
            case Team.Blue:
                return CustomColors.Blue;
            case Team.Yellow:
                return CustomColors.Yellow;
            case Team.Green:
                return CustomColors.Green;
            default:
                break;
        }
        throw new Exception("Team has no color in palette");
    }
} 
public class TeamData {
    public WeaponType[] weapons = new WeaponType[3];
    public PlayerController controller;
}


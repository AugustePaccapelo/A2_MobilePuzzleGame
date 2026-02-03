using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum PlacableObstacle
{
    Flute,
    Tamboure,
    Portail
}

[Serializable]
public class ObstacleInfo
{
    public PlacableObstacle obstacle;
    public Texture icon;
    public GameObject prefabToPlace;
}

[CreateAssetMenu(fileName = "ObstacleData", menuName = "Scriptable Objects/ObstacleData")]
public class ObstacleData : ScriptableSingleton<ObstacleData>
{
    [SerializeField] private List<ObstacleInfo> _obstacleInfos;

    public ObstacleInfo FindObstacleInfo(PlacableObstacle obstacleToFind)
    {
        if (_obstacleInfos == null)
        {
            return null;
        }

        foreach (ObstacleInfo obsInfo in _obstacleInfos)
        {
            if (obsInfo.obstacle == obstacleToFind)
            {
                return obsInfo;
            }
        }

        return null;
    }
}

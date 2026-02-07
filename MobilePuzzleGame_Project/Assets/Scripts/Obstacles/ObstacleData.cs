using System;
using System.Collections.Generic;
using UnityEngine;

public enum PlacableObstacle
{
    Wall,
    Drum,
    Portal,
    Empty,
    Cor,
    Trumpet
}

[Serializable]
public class ObstacleInfo
{
    public PlacableObstacle obstacle;
    public Texture icon;
    public GameObject prefabToPlace;
}

[CreateAssetMenu(fileName = "ObstacleData", menuName = "Scriptable Objects/ObstacleData")]
public class ObstacleData : ScriptableObject
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

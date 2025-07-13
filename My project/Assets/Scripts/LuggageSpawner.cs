using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class SpawnData
{
    public GameObject prefab;
    public float spawnChance;
}

public class LuggageSpawner : MonoBehaviour
{
    [SerializeField] private SpawnData[] _spawnDatas;
    [SerializeField] private float _spawnInterval;
    [SerializeField] private int _maxLuggages = 200;
    [SerializeField] private int _minBoundary = -27;
    [SerializeField] private int _maxBoundary = 27;
    [SerializeField] private int _minInnerBoundary = -8;
    [SerializeField] private int _maxInnerBoundary = 8;

    private List<Vector2Int> _availableGrid = new List<Vector2Int>();
    private Dictionary<GameObject, Vector2Int> _occupiedGrid = new Dictionary<GameObject, Vector2Int>();

    private void Start()
    {
        for (int i = _minBoundary; i <= _maxBoundary; i++)
        {
            for (int j = _minBoundary; j <= _maxBoundary; j++)
            {
                if (i > _minInnerBoundary && i < _maxInnerBoundary && j > _minInnerBoundary && j < _maxInnerBoundary)
                    continue;
                else
                    _availableGrid.Add(new Vector2Int(i, j));
            }
        }

        for (int i = 0; i <= _maxLuggages; i++)
        {
            int random = Random.Range(0, _availableGrid.Count);
            SpawnObject(_availableGrid[random]);
        }
    }

    private void SpawnObject(Vector2Int gridPos)
    {
        GameObject spawnedObject = Instantiate(GetRandomSpawnObject(), new Vector3(gridPos.x, gridPos.y, 0f), Quaternion.identity);
        _availableGrid.Remove(gridPos);
        _occupiedGrid.Add(spawnedObject, gridPos);
        //spawnedObject.OnCollected += FreeLuggageSpace;
    }

    private GameObject GetRandomSpawnObject()
    {
        float totalChance = 0f;
        foreach (var data in _spawnDatas)
        {
            totalChance += data.spawnChance;
        }

        float chance = Random.Range(0f, totalChance);
        float cumulativeChance = 0f;

        for (int i = 0; i < _spawnDatas.Length; i++)
        {
            cumulativeChance += _spawnDatas[i].spawnChance;
            if (chance <= cumulativeChance)
            {
                return _spawnDatas[i].prefab;
            }
        }

        return _spawnDatas[_spawnDatas.Length].prefab;
    }

    private void FreeGridSpace(GameObject occupant)
    {
        _availableGrid.Add(_occupiedGrid[occupant]);
        _occupiedGrid.Remove(occupant);
    }
}

using System.Collections.Generic;
using UnityEngine;

public class LuggageSpawner : MonoBehaviour
{
    [SerializeField] private Luggage[] _luggagePrefabs;
    [SerializeField] private float _spawnInterval;
    [SerializeField] private int _maxLuggages = 200;
    [SerializeField] private int _minBoundary = -27;
    [SerializeField] private int _maxBoundary = 27;
    [SerializeField] private int _minInnerBoundary = -8;
    [SerializeField] private int _maxInnerBoundary = 8;

    private List<Vector2Int> _availableGrid = new List<Vector2Int>();
    private Dictionary<Luggage, Vector2Int> _luggagesGrid = new Dictionary<Luggage, Vector2Int>();

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
            SpawnLuggage(_availableGrid[random]);
        }
    }

    private void SpawnLuggage(Vector2Int gridPos)
    {
        Luggage luggagePrefab = _luggagePrefabs[Random.Range(0, _luggagePrefabs.Length)];
        Luggage luggage = Instantiate(luggagePrefab, new Vector3(gridPos.x, gridPos.y, 0f), Quaternion.identity);
        _availableGrid.Remove(gridPos);
        _luggagesGrid.Add(luggage, gridPos);
        luggage.OnCollected += FreeLuggageSpace;
    }

    private void FreeLuggageSpace(Luggage luggage)
    {
        _availableGrid.Add(_luggagesGrid[luggage]);
        _luggagesGrid.Remove(luggage);
    }
}

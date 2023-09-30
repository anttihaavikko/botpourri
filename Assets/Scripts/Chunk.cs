using System;
using System.Collections.Generic;
using System.Linq;
using AnttiStarterKit.Extensions;
using UnityEngine;
using Random = UnityEngine.Random;

public class Chunk : MonoBehaviour
{
    [SerializeField] private Node nodePrefab;
    [SerializeField] private List<Transform> spawns;
    [SerializeField] private Cluster clusterPrefab;

    public Vector2Int Position { get; private set; }

    public void Setup(Vector2Int pos, bool starter = false)
    {
        Position = pos;
        transform.position = new Vector3(pos.x * 20, pos.y * 15, 0);

        var level = Mathf.Max(Mathf.Abs(pos.x), Mathf.Abs(pos.y));
        
        var spots = spawns.Skip(starter ? 1 : 0).RandomOrder().ToList();
        
        var node = Instantiate(nodePrefab, transform);
        node.transform.position = spots.First().position.RandomOffset(3f);
        node.Setup(level);
        node.Hide();
        
        foreach (var p in spots.TakeLast(1 + level))
        {
            Instantiate(clusterPrefab, p.position.RandomOffset(3f), Quaternion.identity).SetLevel(level);
        }
    }
    
    public bool IsIn(int x, int y)
    {
        return Position.x == x && Position.y == y;
    }
}
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
    
    public Vector2Int Position { get; private set; }

    public void Setup(Vector2Int pos, bool starter = false)
    {
        Position = pos;
        transform.position = new Vector3(pos.x * 20, pos.y * 15, 0);
        
        var node = Instantiate(nodePrefab, transform);
        node.transform.position = spawns.Skip(starter ? 1 : 0).ToList().Random().position.RandomOffset(3f);
        node.Hide();
    }
    
    public bool IsIn(int x, int y)
    {
        return Position.x == x && Position.y == y;
    }
}
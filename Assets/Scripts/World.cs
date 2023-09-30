using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class World : MonoBehaviour
{
    [SerializeField] private Chunk chunkPrefab;
    [SerializeField] private Transform player;

    private readonly List<Chunk> chunks = new();
    private Chunk chunk;

    private void Start()
    {
        Random.InitState(Environment.TickCount);

        chunk = Instantiate(chunkPrefab, transform);
        chunk.Setup(GetChunkPosition(), true);
        chunks.Add(chunk);
        
        AddNeighbours();
    }

    private Vector2Int GetChunkPosition()
    {
        var pp = player.position;
        return new Vector2Int(Mathf.RoundToInt(pp.x / 20f), Mathf.RoundToInt(pp.y / 15f));
    }

    private void Update()
    {
        if (!player) return;
        
        var cp = GetChunkPosition();
        
        if (chunk && !chunk.IsIn(cp.x, cp.y))
        {
            // Debug.Log($"Loading chunk {pos}");
            chunk = chunks.Find(c => c.IsIn(cp.x, cp.y));
            AddNeighbours();
        }
    }

    private void AddNeighbours()
    {
        for (var x = -1; x <= 1; x++)
        {
            for (var y = -1; y <= 1; y++)
            {
                var p = new Vector2Int(chunk.Position.x + x, chunk.Position.y + y);
                if (chunks.Any(c => c.IsIn(p.x, p.y))) continue;
                
                var neighbour = Instantiate(chunkPrefab, transform);
                neighbour.Setup(p);
                chunks.Add(neighbour);
            }   
        }
        
        chunks.ForEach(c => c.gameObject.SetActive(Vector2Int.Distance(c.Position, chunk.Position) < 1.9f));
    }
}
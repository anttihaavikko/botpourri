using System.Collections.Generic;
using System.Linq;
using AnttiStarterKit.Extensions;
using UnityEngine;

public class Enemies
{
    private List<Enemy> enemies = new();

    public void Add(Enemy e)
    {
        enemies.Add(e);
    }

    public void Remove(Enemy e)
    {
        enemies.Remove(e);
    }

    public Enemy Find(Vector3 pos, float range)
    {
        return enemies
            .OrderBy(e => Vector3.Distance(pos, e.transform.position))
            .FirstOrDefault(e => Vector3.Distance(pos, e.transform.position) < range);
    }

    public Enemy GetRandom(Enemy enemy)
    {
        return enemies.Where(e => e != enemy).ToList().Random();
    }
}
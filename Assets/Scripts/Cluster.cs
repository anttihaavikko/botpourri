using UnityEngine;

public class Cluster : MonoBehaviour
{
    [SerializeField] private Enemy prefab;

    private bool triggered;

    public void Activate(Bug player, Enemies enemies)
    {
        if (triggered) return;
        var enemy = Instantiate(prefab, transform.position, Quaternion.identity);
        enemy.SetupContainer(enemies);
        enemy.SetTarget(player);
        triggered = true;
    }
}
using Codice.Client.BaseCommands;
using UnityEngine;

namespace AnttiStarterKit.Animations
{
    public class Follower : MonoBehaviour
    {
        [SerializeField] private Transform target;

        private void Update()
        {
            if (target)
            {
                transform.position = target.position;   
            }
        }
    }
}
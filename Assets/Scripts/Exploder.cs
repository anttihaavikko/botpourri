using System.Collections.Generic;
using System.Linq;
using AnttiStarterKit.Extensions;
using AnttiStarterKit.Managers;
using UnityEngine;

namespace AnttiStarterKit.Visuals
{
    public class Exploder : MonoBehaviour
    {
        [SerializeField] private GameObject rootObject;
        [SerializeField] private List<int> effects;
        [SerializeField] private float shakeAmount = 0.5f;

        private EffectCamera cam;

        private void Start()
        {
            cam = Camera.main.GetComponent<EffectCamera>();
        }

        public void Explode()
        {
            if (effects.Any())
            {
                EffectManager.AddEffects(effects, transform.position);    
            }
            
            cam.BaseEffect(shakeAmount);

            rootObject.SetActive(false);
            // Invoke(nameof(DoDestroy), 0.25f);
        }

        private void DoDestroy()
        {
            Destroy(rootObject);
        }
    }
}
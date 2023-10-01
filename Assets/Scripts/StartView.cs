using System;
using System.Collections.Generic;
using System.Linq;
using AnttiStarterKit.Extensions;
using AnttiStarterKit.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

public class StartView : MonoBehaviour
{
    [SerializeField] private Bug bug;
    [SerializeField] private List<Transform> spots;

    private void Start()
    {
        AudioManager.Instance.TargetPitch = 1f;
        ChangeSpot();
    }

    private void ChangeSpot()
    {
        bug.MoveTo(spots.Where(p => Vector3.Distance(p.position, bug.transform.position) > 1f).ToList().Random().position);
        Invoke(nameof(ChangeSpot), Random.Range(2f, 5f));
    }

    public void NameOrPlay()
    {
        var scene = PlayerPrefs.HasKey("PlayerName") && PlayerPrefs.HasKey("PlayerId") ? "Main" : "Name";
        SceneChanger.Instance.ChangeScene(scene);
    }
}
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private GameObject wrapper;
    [SerializeField] private TMP_Text title, description, size;
    [SerializeField] private Camera cam;
    [SerializeField] private RectTransform descBox;

    public void Show(Bonus bonus, Vector3 pos)
    {
        transform.position = cam.WorldToScreenPoint(pos);
        wrapper.SetActive(true);
        title.text = bonus.title;
        description.text = bonus.description;
        size.text = $"{bonus.size} bytes";
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(descBox);
    }

    public void Hide()
    {
        wrapper.SetActive(false);
    }
}
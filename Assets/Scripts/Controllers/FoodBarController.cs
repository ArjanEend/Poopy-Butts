using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodBarController : MonoBehaviour {

    [SerializeField]
    private Image bar;

    private float fill = 0f;

    private void Start()
    {
        bar.fillAmount = 0f;
    }

    private void Update()
    {
        bar.fillAmount = Mathf.Lerp(bar.fillAmount, fill, Time.deltaTime * 5f);
    }

    public void UpdateDisplay(Stomach stomach)
    {
        fill = (float)stomach.pickups.Count / 8f;
    }
}

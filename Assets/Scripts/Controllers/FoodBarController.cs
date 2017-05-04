using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodBarController : MonoBehaviour {

    [SerializeField]
    private Image bar;
    



    public void UpdateDisplay(Stomach stomach)
    {
        bar.fillAmount = stomach.pickups.Count * .15f;
    }
}

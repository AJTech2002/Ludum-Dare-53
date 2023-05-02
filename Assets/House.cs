using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class House : MonoBehaviour
{
    public ColorSide houseColor;
    public Animator discAnimator;
    public Disc outterDisc;
    public Disc innerDisc;

    private void Awake() {
        GridManager manager = GameObject.FindObjectOfType<GridManager>();
        switch (houseColor) {
            case ColorSide.Blue:
                outterDisc.Color = manager.blueLine;
                innerDisc.Color = manager.blue;
                break;
            case ColorSide.Red:
                outterDisc.Color = manager.redLine;
                innerDisc.Color = manager.red;
                break;
            case ColorSide.Green:
                outterDisc.Color = manager.greenLine;
                innerDisc.Color = manager.green;
                break;
            case ColorSide.Yellow:
                outterDisc.Color = manager.yellowLine;
                innerDisc.Color = manager.yellow;
                break;  
        }
    }

    bool completed = false;
    public void OnTriggerEnter (Collider collider) {
        if (collider.CompareTag("Truck") && collider.GetComponent<Truck>().side == houseColor && !completed) {
            Debug.Log("Driver completed task!");
            discAnimator.SetBool("completed",true);
            completed = true;
            GameManager.HouseReached();
        }
    }
}

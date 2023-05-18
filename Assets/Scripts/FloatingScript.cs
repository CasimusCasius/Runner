using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingScript : MonoBehaviour
{
    [SerializeField] private float floatHeight = 0.5f;

    private const int STEPS = 25;
    private int count = 0;
    private bool up = true;

    private void FixedUpdate()
    {

        // coútam = (sprawdü warunek) ? jeøeli prawda : jeøeli fa≥sz
        Vector3 move = (up)? new Vector3(0, floatHeight/STEPS,0) 
            : new Vector3(0, -floatHeight / STEPS, 0);

        transform.position += move;
        count++;
        if (count == STEPS) 
        {
            up = !up;
            count = 0;
        }

    }
}

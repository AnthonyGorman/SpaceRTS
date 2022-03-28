using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Randomize : MonoBehaviour
{
    // Start is called before the first frame update
    
    void Update()
    {
        System.Random random = new System.Random();
        float h = 9, v = 9;
        int x = random.Next(0, 4);
        switch (x)
        {
            case 0:
                h = 9; v = random.Next(-9, 9); break;
            case 1:
                h = random.Next(-9, 9); v = 9; break;
            case 2:
                h = -9; v = random.Next(-9, 9); break;
            case 3:
                h = random.Next(-9, 9); v = -9; break;
        }

        //update the position
        transform.position = new Vector3(h, v, 0);

        //output to log the position change
        //Debug.Log(transform.position);
    }
}

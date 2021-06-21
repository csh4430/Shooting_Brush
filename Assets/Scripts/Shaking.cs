using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaking : MonoBehaviour
{

    public IEnumerator Shake()
    {
        float xPos = transform.position.x;
        float randomX = 0.02f ;
        for(int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(0.1f);
            transform.position = new Vector3(xPos + randomX, transform.position.y, transform.position.z);
            randomX *= -1;
        }
        transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
    }
}

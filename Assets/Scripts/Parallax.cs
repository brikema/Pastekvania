using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, height, startPos, posY, topYInit, bottomYInit;
    public GameObject cam;
    public float parallaxEffect;

    public bool topRepeat;
    public bool bottomRepeat;

    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        height = GetComponent<SpriteRenderer>().bounds.size.y;
        posY = transform.position.y;
        topYInit = 0;
        bottomYInit = 0;
    }

    void Update()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float tempY = (cam.transform.position.y);
        float dist = (cam.transform.position.x * parallaxEffect);
        float distY = (cam.transform.position.y);

        transform.position = new Vector3(startPos + dist, posY, transform.position.z);

        if(topRepeat && tempY > posY + height){
            posY += height;
            topYInit += 1;
        }
        if(topRepeat &&  tempY < posY - height){
            if(topYInit > 0){
                posY -= height;
                topYInit -= 1;
            }
        }

        if(bottomRepeat && tempY < posY - height){
            posY -= height;
            bottomYInit += 1;
        }
        if(bottomRepeat && tempY > posY + height){
            if(bottomYInit > 0){
                posY += height;
                bottomYInit -= 1;
            }
        }

        if(temp > startPos + length) startPos += length;
        else if (temp < startPos - length) startPos -= length;
    }
}

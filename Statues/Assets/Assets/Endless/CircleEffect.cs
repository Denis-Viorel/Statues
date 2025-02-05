using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleEffect : MonoBehaviour
{
    [SerializeField] public float rotationSpeed;
    [SerializeField] public Vector3 scale;
    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed);
        
        if(transform.localScale.x >= 1f || transform.localScale.x <= 0.89f)
        {
            scale = scale * -1;
        }
        transform.localScale = transform.localScale + scale;
    }
}

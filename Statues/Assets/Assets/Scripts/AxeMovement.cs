using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeMovement : MonoBehaviour
{
    [SerializeField] public float speed = 1.5f;
    [SerializeField] private AudioSource loop;
    public float limit = 75f;
    public bool randomStart = false;
    private float random = 0;

    private void Awake()
    {
        if (randomStart) random = Random.Range(0f, 1f);

        StartCoroutine(Delay(3f));
    }

    // Update is called once per frame
    void Update()
    {
        float angle = limit * Mathf.Sin(Time.time + random * speed);
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    IEnumerator Delay(float delayTime)
    {
        loop.Play();
        while (true)
        {
            yield return new WaitForSeconds(delayTime);
            loop.Play();
        }
    }
}

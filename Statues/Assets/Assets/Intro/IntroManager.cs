using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroManager : MonoBehaviour
{
    [SerializeField]private int timer;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TimerCoroutine());
    }

    IEnumerator TimerCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            timer++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(timer == 51)
        {
            SceneManager.LoadScene(1);
        }
        if(Input.anyKey)
        {
            SceneManager.LoadScene(1);
        }
    }
}

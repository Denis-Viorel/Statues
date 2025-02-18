using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private bool isInGameScene;
    [SerializeField] private GlobalManager_SCPT globalManager;
    [SerializeField] private AudioSource bgMusic;

    // Start is called before the first frame update
    void Start()
    {
        isInGameScene = false;
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().name.Equals("Endless") && (isInGameScene == false))
        {
            isInGameScene = true;
            globalManager = GameObject.Find("GlobalManager").GetComponent<GlobalManager_SCPT>();
        }
        if(isInGameScene == true)
        {
            float calmValue = globalManager.calmGlobal;

            float effectiveCalm = Mathf.Max(calmValue, 25f);

            bgMusic.pitch = Mathf.Lerp(-3f, 1f, (effectiveCalm - 25f) / (100f - 25f));

            bgMusic.volume = Mathf.Lerp(1.0f, 0.4f, (effectiveCalm - 25f) / (100f - 25f));
        }
    }
}

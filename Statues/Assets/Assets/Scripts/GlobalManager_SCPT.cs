using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;
using static UnityEngine.EventSystems.EventTrigger;
using System.Runtime.CompilerServices;
using UnityEngine.SocialPlatforms.Impl;

public class GlobalManager_SCPT : MonoBehaviour
{
    [Tooltip("Sansa ca un agent sa fie protector")]
    [SerializeField] public float chanceProtector = 1.20f;
    [Tooltip("Sansa ca un agent sa fie sabotor")]
    [SerializeField] public float chanceSaboteur = 0.20f;
    [Tooltip("Limita de panica/nDaca scate calmul sub, agentul se poate panica")]
    [SerializeField] public int panic = 50;
    [Tooltip("Calmul minim initial al agentului")]
    [SerializeField] public int calmMin = 80;
    [Tooltip("Cat calm se pierde pe secunda in red light")]
    [SerializeField] public int calmLossPerTick = 1;
    [Tooltip("Cat calm se pierde in jur la moarte")] 
    [SerializeField] public int calmLossDeath = 10;
    [Tooltip("Variatie viteza")] 
    [SerializeField] public float speedVariation = 1;
    [SerializeField] private AudioSource redLightSound;
    [SerializeField] private AudioSource runningSound;
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private GameObject winscreen;

    /* Endless run variables */
    [SerializeField] private TextMeshProUGUI timerUI;
    [SerializeField] private TextMeshProUGUI roundNoUI;
    [SerializeField] private TextMeshProUGUI survivingAgentsUI;
    [SerializeField] private GameObject circle;
    [SerializeField] private int timeBetweenRounds;
    [SerializeField] private Vector3 restartPlayerPosition;
    [SerializeField] public float newRoundBonusValue;
    private int round;
    private int score;

    private bool lightSwitch = true; //false = red light, true = green light;
    // Start is called before the first frame update

    public UnityEvent greenLight = new UnityEvent();
    public UnityEvent redLight = new UnityEvent();
    public List<int> ids_used = new List<int>();

    public float calmGlobal = 0;
    public float initialCalmGlobal = 0;
    public int activeAgentsNumber = 0;
    public int initialAgentsNumber = 0;

    [SerializeField] public UI_Manager managerUI;
    [SerializeField] private DesaturateImageEffect desaturate;
    [SerializeField] private RandomManager randomManager;
    [SerializeField] private WallManager wallManager;

    void Start()
    {
        //calmGlobal = 0;
        //activeAgentsNumber = 0;

        timeBetweenRounds = 10;

        if (winscreen == null)
        {
            winscreen = GameObject.Find("WinscreenOverlay");
        }

        if(circle == null)
        {
            circle = GameObject.Find("Circle");
        }

        if(randomManager == null)
        {
            randomManager = GameObject.Find("RandomManager").GetComponent<RandomManager>();
        }

        if(wallManager == null)
        {
            wallManager = GameObject.Find("Main Camera").GetComponent<WallManager>();
        }

        circle.SetActive(false);

        restartPlayerPosition = new Vector3(-11.0f, -6.31f, 49.0f);
        round = 0;
        score = 0;

        runningSound.Play();
        
        initialAgentsNumber = activeAgentsNumber;
        calmGlobal = initialCalmGlobal / initialAgentsNumber;
    }

     private void OnEnable()
    {
        WallManager.OnMouseClick += LightSwitch;
    }

    private void OnDisable()
    {
        WallManager.OnMouseClick -= LightSwitch;
    }
    // Update is called once per frame
    void Update()
    {
        managerUI.UpdateCalmBar(calmGlobal);

        /* Apply the screen desaturation */
        desaturate.desaturateAmount = 1 - calmGlobal/100;
    }

    void LightSwitch()
    {
        lightSwitch = !lightSwitch;
        if (lightSwitch)
        {
            greenLight.Invoke();

            runningSound.Play();

            /* Play the doll red light song */
            redLightSound.Stop();
        }
        else
        {
            /* Stop the doll red light song */
            //calmGlobal = 0;
            redLight.Invoke();

            redLightSound.Play();

            runningSound.Stop();
        }
    }

    void GameOver(int savedAgents)
    {
        circle.SetActive(true);
        roundNoUI.text = "You lost!";
        timerUI.text = score.ToString();
        survivingAgentsUI.text = "You saved " + savedAgents.ToString() + "/" + initialAgentsNumber.ToString() + " players";

        HighscoreManager.SaveHighScore(score);

        StartCoroutine(Delay(5f));
    }

    IEnumerator Delay(float delayTime)
    {
        //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(delayTime);
        SceneManager.LoadScene("MainMenu");
        //Do the action after the delay time has finished.
    }

    public void WinRound(int savedAgents)
    {
        Time.timeScale = 0;
        round++;
        score = score + round * savedAgents;
        newRoundBonusValue += 1.0f/round;

        /* Display round info and timer */
        StartCoroutine(Countdown(timeBetweenRounds, savedAgents));
    }

    private IEnumerator Countdown(int timeRemaining, int savedAgents)
    {
        circle.SetActive(true);
        roundNoUI.text = "Round No. " + round.ToString();
        survivingAgentsUI.text = "You saved " + savedAgents.ToString() + "/" + initialAgentsNumber.ToString() + " players";

        while (timeRemaining >= 0)
        {
            yield return new WaitForSecondsRealtime(1f); // Wait 1 second
            timerUI.text = timeRemaining.ToString();
            timeRemaining--;
        }

        circle.SetActive(false);

        ResetAgents();
        
        wallManager.setNrWalls( round / 4 );

        randomManager.setNrTraps(round + UnityEngine.Random.Range(0, 3));
        randomManager.SettingTraps();
        wallManager.ClearWalls();

        yield return new WaitForSecondsRealtime(3f);

        Time.timeScale = 1;
    }

    private void ResetAgents()
    {
        int aliveAgentCol = 0;
        int aliveAgentRow = 0;
        Vector3 localRestartPlayerPosition;

        /* Restart for the next round */
        GameObject[] allPlayer = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in allPlayer)
        {
            BehaviourManager_SCPT playerBehaviour = player.GetComponent<BehaviourManager_SCPT>();

            playerBehaviour.newRoundBonus(newRoundBonusValue);

            if (playerBehaviour.agentStatus != AgentStatus.Dead)
            {
                playerBehaviour.agentStatus = AgentStatus.Idle;
                localRestartPlayerPosition = restartPlayerPosition + new Vector3(1 * aliveAgentCol, 0, -0.5f * aliveAgentRow);
                player.transform.position = localRestartPlayerPosition;
                aliveAgentCol++;

                playerBehaviour.ResetDestination();

                if (aliveAgentCol == 24)
                {
                    aliveAgentCol = 0;
                    aliveAgentRow++;
                }
            }
        }
        
        aliveAgentRow = 0;
    }

    public void checkAllAgentsDead()
    {
        if(activeAgentsNumber == 0)
        {
            GameOver(activeAgentsNumber);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using Random = UnityEngine.Random;

public enum AgentType{
        Protector,
        Saboteur,
        Normie
}

public enum AgentStatus{
    Idle,
    Walking,
    Running,
    Victory,
    Dead
}

public class BehaviourManager_SCPT : MonoBehaviour
{
    [SerializeField]AgentType type = new AgentType();
    public AgentStatus agentStatus = AgentStatus.Idle;
    [SerializeField]CrowdManager_SCPT crowdManager;
    [SerializeField]GlobalManager_SCPT globalManager;
    [SerializeField]AgentDisplayManager agentDisplayManager;
    [SerializeField]UI_Manager agentUIManager;
    [SerializeField]AnimationManager agentAnimationManager;
    [SerializeField] private CapsuleCollider cap;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private FollowerEntity _follower;
    [SerializeField] private GameObject lowCalmImage;
    public Coroutine coroutine;
    private float calm;
    private int calmMin;
    private int panic;
    private int panicCheck;
    private float _calmLossPerSecond;
    private float _calmLossDeath;
    private int agentId;
    private float chanceProtector = 0.10f;
    private float chanceSaboteur = 0.10f;
    private float typeRoll;
    private float time;
    private float _speedVariation;
    private float rollSpeed;
    private bool isPanicked = false;
    private Vector3 _destination;
    private string agentName="x";
    private string agentJob="Nerd";
    private string agentSpeed="Fast";
    private string[] names;
    private string[] jobs;

    private Finish _finish;
    //private SphereCollider crowdCheck;

    private bool redLightActive = false;

    // Start is called before the first frame update
    void Awake()
    {
        if (globalManager == null)
        {
            globalManager = GameObject.Find("GlobalManager").GetComponent<GlobalManager_SCPT>();
        }

        if (agentUIManager == null)
        {
            agentUIManager = GameObject.Find("DetailPanelCanvas").GetComponent<UI_Manager>();
        }

        if (cap == null)
        {
            cap = GetComponent<CapsuleCollider>();
        }

        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
        
        if (_finish == null)
        {
            _finish = GameObject.Find("Finish").GetComponent<Finish>();
        }

        SetTypeAgent();
       
        calm = Random.Range(calmMin, 100);
        if (crowdManager == null)
        {
            crowdManager = GetComponentInChildren<CrowdManager_SCPT>();
        }
        
        time = Time.time; 
        globalManager.greenLight.AddListener(GreenLight);
        globalManager.redLight.AddListener(RedLight);
        _follower = GetComponent<FollowerEntity>();
        _follower.maxSpeed += rollSpeed;
        Debug.Log($"Setup: ID - {gameObject.GetInstanceID()}, tip - {type}, calm - {calm}, follower gasit: {_follower.isActiveAndEnabled}");

        /* Set the calm bar */
        agentDisplayManager.UpdateCalmBar(calm);
        agentDisplayManager.SetAgentTypePhoto(type);

        /* Add the agent to the alive agents pool */
        globalManager.activeAgentsNumber++;
        if (globalManager.activeAgentsNumber != 0)
        {
            globalManager.initialCalmGlobal += calm;
        }

        /* Set the agent to moving status -> Stops the move animation */
        switch(agentSpeed)
        {
            case "Fast":
                agentStatus = AgentStatus.Running;
                break;
            case "Medium":
            case "Slow":
            default:
                agentStatus = AgentStatus.Walking;
                break;
        }

        _destination = new Vector3(0f, -6.31f, 3.25f);
        coroutine = StartCoroutine(SetDestination());
        agentAnimationManager.UpdateAnimationState(agentStatus);
        gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    void GreenLight()
    {
        //Debug.Log("GreenLight");
        redLightActive = false;
        _follower.isStopped = false;

        /* Set the agent to moving status -> Stops the move animation */
        if (agentStatus == AgentStatus.Victory) return;
        switch (agentSpeed)
        {
            case "Fast":
                agentStatus = AgentStatus.Running;
                break;
            case "Medium":
            case "Slow":
            default:
                agentStatus = AgentStatus.Walking;
                break;
        }

        agentAnimationManager.UpdateAnimationState(agentStatus);
    }

    void RedLight()
    {
        //Debug.Log("RedLight");
        redLightActive = true;
        _follower.isStopped = true;

        /*if (globalManager.activeAgentsNumber != 0)
        {
            globalManager.calmGlobal += calm / globalManager.activeAgentsNumber;
        }*/

        /* Set the agent to idle status -> Stops the move animation */
        if (agentStatus == AgentStatus.Victory) return;

        agentStatus = AgentStatus.Idle;
        agentAnimationManager.UpdateAnimationState(agentStatus);
    }

    public float GetCalm(){
        return calm;
    }

    public void AddCalm(float calmReceived)
    {
        /* Apply extra calm only if the agent is not a Saboteur */
        if (this.type == AgentType.Protector || this.type == AgentType.Normie)
        {
            calm = calm + calmReceived;
            globalManager.calmGlobal += 1 / globalManager.initialAgentsNumber;
        }
    }

    public void ModifyCalm( float value, AgentType typeReceiving, bool isDeathEffect ){
        // Debug.Log($"Valori initiale: ID - {gameObject.GetInstanceID()}, tip - {type}, calm - {calm}, calm primit - {value}, tip agent primit - {typeReceiving}, moarte - {isDeathEffect}");
        // Debug.Log("Valori initiale: " + "calm primit: " + value);
        // Debug.Log("Valori initiale: " + "tip agent primit: " + typeReceiving);
        // Debug.Log("Valori initiale: " + "calm: " + calm);

        if (agentStatus == AgentStatus.Victory) return;

        switch ( type ){
            case AgentType.Protector:{
                if (isDeathEffect)
                {
                    calm -= _calmLossDeath;
                    globalManager.calmGlobal -= _calmLossDeath/globalManager.activeAgentsNumber;
                    break;
                }

                if (calm < value && typeReceiving != AgentType.Saboteur)
                {
                    calm++;
                    globalManager.calmGlobal += 1 / globalManager.initialAgentsNumber;
                }

                //Debug.Log("Protector:" + calm );
                 break;
            }
            case AgentType.Saboteur:{
                if (isDeathEffect)
                {
                    calm += _calmLossDeath/2;
                    globalManager.calmGlobal += (_calmLossDeath/2)/globalManager.initialAgentsNumber;
                    break;
                }

                if (calm > value && typeReceiving != AgentType.Protector)
                {
                    calm--;
                    globalManager.calmGlobal -= 1 / globalManager.initialAgentsNumber;
                }

                //Debug.Log("Saboteur:" + calm );
                break;
            }
            case AgentType.Normie:{
                if (isDeathEffect)
                {
                    calm -= _calmLossDeath;
                    globalManager.calmGlobal -= _calmLossDeath/globalManager.initialAgentsNumber;
                    break;
                }

                if (calm < value)
                {
                    calm++;
                    globalManager.calmGlobal += 1 / globalManager.initialAgentsNumber;
                }
                else if (calm > value && typeReceiving != AgentType.Protector)
                {
                    calm--;
                    globalManager.calmGlobal -= 1 / globalManager.initialAgentsNumber;
                }

                //Debug.Log("Normie:" + calm );
                 break;
            }
        }

        /* Global calm calcultaion */
        //globalManager.calmGlobal += calm/globalManager.activeAgentsNumber;

        // Debug.Log($"Dupa switch: ID - {gameObject.GetInstanceID()}, tip - {type}, calm - {calm}, calm primit - {value}, tip agent primit - {typeReceiving}, moarte - {isDeathEffect}");
    }

    public void newRoundBonus(float value)
    {
        value = Mathf.Clamp(value, 0.1f, 0.25f);

        float calmBonusValue = Mathf.Clamp(value * calm, 0, 100 - calm);

        calm += calmBonusValue;
        globalManager.calmGlobal += calmBonusValue / globalManager.initialAgentsNumber;
    }

    void SetTypeAgent(){

        if( globalManager != null ){
            chanceProtector = globalManager.chanceProtector;
            chanceSaboteur = globalManager.chanceSaboteur;
            panic = globalManager.panic;
            calmMin = globalManager.calmMin;
            _calmLossPerSecond = globalManager.calmLossPerTick;
            _calmLossDeath = globalManager.calmLossDeath;
            _speedVariation = globalManager.speedVariation;
            rollSpeed = Random.Range(-_speedVariation, _speedVariation);
            agentId = Random.Range(1, 456);

            while( globalManager.ids_used.Contains(agentId) ){
                agentId = Random.Range(1, 456);
            }

            globalManager.ids_used.Add(agentId);
        }


        TextAsset textFile = Resources.Load<TextAsset>("first-names");
        names = textFile.text.Split('\n');
        if (names.Length > 0)
        {
            agentName = names[Random.Range(0, names.Length)];
        }
        
        TextAsset textFileJobs = Resources.Load<TextAsset>("jobs");
        jobs = textFileJobs.text.Split('\n');
        if (jobs.Length > 0)
        {
            agentJob = jobs[Random.Range(0, jobs.Length)];
        }

        if( rollSpeed < -_speedVariation / 3  ){
            agentSpeed = "Slow";
        }
        else if( rollSpeed >= -_speedVariation / 3 && rollSpeed < _speedVariation / 3){
            agentSpeed = "Medium";
        }
        else{
            agentSpeed = "Fast";
        }

        typeRoll = Random.value; // returns numbers between {0 - 1}

        if( typeRoll < chanceProtector ){
            type = AgentType.Protector;
        }
        else if( typeRoll < chanceProtector + chanceSaboteur ){
            type = AgentType.Saboteur;
        }
        else{
            type = AgentType.Normie;
        }
    }

    void PanicCheck(){
        if (panic > calm)
        {
            //daca calm e sub panic, sansa de panicare direct proportionala cu cat de mult e calm sub panic
            lowCalmImage.SetActive(true);
            panicCheck = Random.Range(0, panic);
            Debug.Log($"Panic check: ID - {gameObject.GetInstanceID()}, valoare - {panicCheck}, calm - {calm}");
            if (panicCheck > calm)
            {
                isPanicked = true;
                Death();
                Debug.Log($"Panicat: {gameObject.GetInstanceID()}");
            }
        }
        else
        {
            lowCalmImage.SetActive(false);
        }
    }

    public void Death(){
        StopCoroutine(coroutine);

        /* Set tag to DeadAgent */
        this.transform.Find("AgentsDetectionSphere").gameObject.tag = "DeadAgent";
        lowCalmImage.SetActive(false);

        crowdManager.AgentCrowdEffect( calm, type, true);
        globalManager.calmGlobal -= calm/globalManager.initialAgentsNumber;
        calm = 0;
        agentDisplayManager.gameObject.SetActive(false);
        crowdManager.enabled = false;
        globalManager.greenLight.RemoveListener(GreenLight);
        globalManager.redLight.RemoveListener(RedLight);
        _follower.enabled = false;
        cap.enabled = false;
        rb.isKinematic = true;

        /* Remove the agent from the alive agents pool */
        globalManager.activeAgentsNumber--;
        globalManager.checkAllAgentsDead();

        /* Play death animation */
        agentStatus = AgentStatus.Dead;
        agentAnimationManager.UpdateAnimationState(agentStatus);

        this.enabled = false;
    }

    public void Victory()
    {
        /* Play victory animation */
        agentStatus = AgentStatus.Victory;
        agentAnimationManager.UpdateAnimationState(agentStatus);

        /* Mark agent as finished */
        _finish.AgentFinished();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finish")
        {
            Victory();

            /* Add calm when an agent finishes the round */
            crowdManager.AgentFinishEffect(10f);
        }
        if (other.tag == "DeadAgent")
        {
            calm -= 2.5f;
            globalManager.calmGlobal -= 2.5f / globalManager.initialAgentsNumber;
            Debug.Log("--- Dead effect: -2.5 calm ---");
        }
    }

    void FixedUpdate(){
        if(redLightActive && (time <= Time.time) && (agentStatus != AgentStatus.Victory)){
            time = Time.time + 1f;
            crowdManager.AgentCrowdEffect( calm, type, false );
            PanicCheck();

            calm -= _calmLossPerSecond;
            globalManager.calmGlobal -= _calmLossPerSecond/globalManager.initialAgentsNumber;
            Debug.Log("Calm loss per second: " +_calmLossPerSecond + " Active Agents: " 
                      + globalManager.initialAgentsNumber + " calm loss calculated: " 
                      + _calmLossPerSecond/globalManager.initialAgentsNumber);
            Debug.Log($"Red Light Loss: ID - {gameObject.GetInstanceID()}, calm - {calm}");
        }

        /* Update calm bar */
        agentDisplayManager.UpdateCalmBar(calm);
    }

    /* When hovering, display the information panel */
    void OnMouseOver()
    {
        agentUIManager.SetPanelActive(agentId, agentName, agentJob, agentSpeed, type);
    }

    private void OnMouseExit()
    {
        agentUIManager.SetPanelInactive();
    }

    IEnumerator SetDestination()
    {
        while (true)
        {
            _destination.x = transform.position.x;
            _follower.destination = _destination;
            
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void ResetDestination()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = StartCoroutine(SetDestination());
        }
    }
}

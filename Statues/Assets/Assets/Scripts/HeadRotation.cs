using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadRotation : MonoBehaviour
{
    public Transform head; // Assign the head transform in the Inspector
    public float rotationSpeed = 2f; // Speed of rotation
    private bool isFacingBack = false; // Initial state
    private int enter = 0;
    private Coroutine currentRotationCoroutine;

    [SerializeField]GlobalManager_SCPT globalManager;

    void Start()
    {
        if (globalManager == null)
        {
            globalManager = GameObject.Find("GlobalManager").GetComponent<GlobalManager_SCPT>();
        }

        globalManager.greenLight.AddListener(OnLightChange);
        globalManager.redLight.AddListener(OnLightChange);

    }

    void OnLightChange()
    {
        enter++;

        if (currentRotationCoroutine != null)
        {
            StopCoroutine(currentRotationCoroutine);
        }

        currentRotationCoroutine = StartCoroutine(RotateHead());
         // Toggle the state
            isFacingBack = !isFacingBack;
    }

    IEnumerator RotateHead()
    {

            // Calculate the target rotation
            Quaternion targetRotation = isFacingBack 
                ? Quaternion.Euler(0, 0, 0) // Head facing forward
                : Quaternion.Euler(25, 180, 0); // Head facing backward
            //  Debug.Log ("inainte de while: " + enter + " " + isFacingBack);

             Quaternion startRotation = head.localRotation;
             float elapseTime = 0f;
            // Smoothly rotate to the target rotation
            while (Quaternion.Angle(head.localRotation, targetRotation) > 0.1f)
            {
                elapseTime += rotationSpeed * Time.deltaTime;
                head.localRotation = Quaternion.Slerp(startRotation, targetRotation, elapseTime);
                yield return null;
            }
            // Debug.Log ("dupa while: " + enter);

            // Snap to the target rotation to avoid tiny discrepancies
            head.localRotation = targetRotation;
            currentRotationCoroutine = null;
    }
}

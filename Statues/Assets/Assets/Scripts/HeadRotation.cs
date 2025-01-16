using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadRotation : MonoBehaviour
{
    public Transform head; // Assign the head transform in the Inspector
    public float rotationSpeed = 2f; // Speed of rotation
    private bool isFacingBack = false; // Initial state

    [SerializeField]GlobalManager_SCPT globalManager;

    void Start()
    {
        if (globalManager == null)
        {
            globalManager = GameObject.Find("GlobalManager").GetComponent<GlobalManager_SCPT>();
        }

        globalManager.greenLight.AddListener(RotateHead);
        globalManager.redLight.AddListener(RotateHead);

        // StartCoroutine(RotateHead());
    }

    void RotateHead()
    {
        //  while (true)
        //  {
            // Wait for 5 seconds
            // yield return new WaitForSeconds(5f);

            // Calculate the target rotation
            Quaternion targetRotation = isFacingBack 
                ? Quaternion.Euler(0, 0, 0) // Head facing forward
                : Quaternion.Euler(25, 180, 0); // Head facing backward

            // Smoothly rotate to the target rotation
            while (Quaternion.Angle(head.localRotation, targetRotation) > 0.1f)
            {
                head.localRotation = Quaternion.Slerp(head.localRotation, targetRotation, rotationSpeed * Time.deltaTime);
                // yield return null;
            }

            // Snap to the target rotation to avoid tiny discrepancies
            // head.localRotation = targetRotation;

            // Toggle the state
            isFacingBack = !isFacingBack;
        //  }
    }
}

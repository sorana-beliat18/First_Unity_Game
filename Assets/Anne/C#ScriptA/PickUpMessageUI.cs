using UnityEngine;
using TMPro;
using System.Collections;

public class PickupMessageUI : MonoBehaviour
{
    public static PickupMessageUI Instance;

    public TMP_Text messageText;
    public float showTime = 2f;

    private Coroutine currentRoutine;

    void Awake()
    {
        Instance = this;

        if (messageText == null)
            messageText = GetComponent<TMP_Text>() ?? GetComponentInChildren<TMP_Text>(true);

        messageText.enabled = false; 
    }

    public void Show(string message)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowRoutine(message));
    }

    IEnumerator ShowRoutine(string message)
    {
        messageText.text = message;
        messageText.enabled = true;

        yield return new WaitForSeconds(showTime);

        messageText.enabled = false;
    }
}

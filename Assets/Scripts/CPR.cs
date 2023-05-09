using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;

public class CPR : MonoBehaviour
{
    [SerializeField] private Sprite standUp;
    [SerializeField] private GameObject rightClick;
    [SerializeField] private GameObject wrongClick;
    [SerializeField] private int requiredClicks = 100;
    [SerializeField] private int maxClicks = 120;
    [SerializeField] private TextMeshPro resultText;
    [SerializeField] private GameObject patient;
    [SerializeField] InputAction startCPR = new InputAction(type: InputActionType.Button);
    [SerializeField] InputAction call101 = new InputAction(type: InputActionType.Button);
    [SerializeField] private InputAction push = new InputAction(type: InputActionType.Button);
    [SerializeField] private string triggeringTag;
    [SerializeField] private float timer = 0f;
    [SerializeField] private float maxTime = 60f;
    [SerializeField] private int clickCount = 0;
    [SerializeField] private TextMeshPro text;
    [SerializeField] private TextMeshPro text2;
    private AudioSource stayingAliveAudio; 
    private bool isResuscitating = false;
    private bool isSuccess = false;
    private float startTime;
    private float clicksPerMin;
    private SpriteRenderer patientSpriteRenderer;
    [SerializeField] private SpriteRenderer paramedicSpriteRenderer;
    [SerializeField] private GameObject whiteBoard;
    private IEnumerator coroutine;
    private AudioSource callingAudioSource;
    public bool interact = false;
    public bool canStartOverAudio = false;
    
     void Awake()
    {
        // Disable the whiteboard and texts at the start of the game
        text.gameObject.SetActive(false);
        text2.gameObject.SetActive(false);
        whiteBoard.gameObject.SetActive(false);
    } 
    
    void OnEnable()
    {
        // Enable all InputActions when the script is enabled
        push.Enable();
        call101.Enable();
        startCPR.Enable();
    }

    void OnDisable()
    {
        // Disable all InputActions when the script is disabled
        push.Disable();
        call101.Disable();
        startCPR.Disable();
    }

    void Start()
    {
        // Initialize variables and components
        callingAudioSource = GetComponent<AudioSource>();
        stayingAliveAudio = patient.GetComponent<AudioSource>();
        patientSpriteRenderer = patient.GetComponent<SpriteRenderer>();
        resultText.text = "";
        rightClick.SetActive(false);
        wrongClick.SetActive(false);
    }

    void Update()
    {
        if(interact){
            if (call101.IsPressed()){ // When calling 101 the "phone dial and ring sound" audio is playing and the text changes accordingly when the paramedic answers the call (make this functionality using Coroutine)
                callingAudioSource.Play(); 
                interact = false;
                coroutine = paramedicAnswersTheCall();
                StartCoroutine(coroutine); 
            }
        }

        if(canStartOverAudio){
            if (startCPR.IsPressed()){ // When the player press a button the Resuscitation starts and the "Staying Alive" audio starts over
                stayingAliveAudio.Stop();
                StartResuscitation();
                canStartOverAudio = false;
            }
        }
        if (push.triggered && isResuscitating) // Check if the player is pushing and is currently in the middle of resuscitating
        {   
            // Calculate the number of clicks per minute the player is performing
            float timeElapsed = Time.time - startTime;
            clickCount++;
            clicksPerMin = Mathf.RoundToInt((clickCount / timeElapsed) * 60);
            // Update the result text to display the clicks per minute and whether it's within the required range
            resultText.text = "You need to do between 100 to 120 clicks per minute\n Clicks per minute: " + clicksPerMin.ToString();
            // Show the appropriate feedback image based on whether the clicks per minute is within the required range or not
            if (clicksPerMin >= requiredClicks && clicksPerMin <= maxClicks)
            {
                rightClick.SetActive(true);
                wrongClick.SetActive(false);
            }
            else
            {
                rightClick.SetActive(false);
                wrongClick.SetActive(true);
            }
        }

        if (isResuscitating) // Check if the player is currently in the middle of resuscitating
        {
            // Update the timer and end the resuscitation if the maximum time limit has been reached
            timer += Time.deltaTime;
            if (timer >= maxTime) 
            {
                EndResuscitation();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(triggeringTag))
        {
            gameObject.GetComponent<Dehydration>().enabled = false;
            interact = true;     
        }
    }

    // This coroutine is called when the player calls 101 and waits for a paramedic to answer
    private IEnumerator paramedicAnswersTheCall()
    {
        yield return new WaitForSeconds(8);
        // Display the appropriate text and image to indicate that the paramedic has answered the call
        text.gameObject.SetActive(true);
        whiteBoard.gameObject.SetActive(true);
        paramedicSpriteRenderer.enabled = true;  
        stayingAliveAudio.Play();
        canStartOverAudio = true;
    }

    // This function is called when the player starts resuscitation
    private void StartResuscitation()
    {
        isResuscitating = true;
        stayingAliveAudio.Play();
        startTime = Time.time;
    }
    
    // This function is called when the player ends resuscitation
    private void EndResuscitation()
    {
        isResuscitating = false;
        stayingAliveAudio.Stop();
        if (clicksPerMin >= requiredClicks && clicksPerMin <= maxClicks)
        {
            isSuccess = true;
        }
        if (isSuccess)
        {
            patientSpriteRenderer.sprite = standUp;
            resultText.text = "CPR successful! You did " + clicksPerMin.ToString() + " clicks per minute.";
            text.gameObject.SetActive(false);
            text2.gameObject.SetActive(true);
            coroutine = RemoveWhiteBoard();
            StartCoroutine(coroutine);   
        }
        else
        {
            resultText.text = "CPR failed. You did " + clicksPerMin.ToString() + " clicks per minute.";
            text.gameObject.SetActive(false);
            whiteBoard.gameObject.SetActive(false);
            paramedicSpriteRenderer.enabled = false; 
        }
        rightClick.SetActive(false);
        wrongClick.SetActive(false);
        gameObject.GetComponent<Dehydration>().enabled = true;
    }

    private IEnumerator RemoveWhiteBoard()
    {
        yield return new WaitForSeconds(2);
        text2.gameObject.SetActive(false);
        whiteBoard.gameObject.SetActive(false);
        paramedicSpriteRenderer.enabled = false;
    } 
}
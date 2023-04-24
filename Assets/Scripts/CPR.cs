using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class CPR : MonoBehaviour
{
    [SerializeField] private Sprite standUp;
    [SerializeField] private GameObject rightClick;
    [SerializeField] private GameObject wrongClick;
    [SerializeField] private int requiredClicks = 100;
    [SerializeField] private int maxClicks = 120;
    [SerializeField] private TextMeshPro resultText;
    [SerializeField] private GameObject patient;
    [SerializeField] private InputAction push = new InputAction(type: InputActionType.Button);
    [SerializeField] private string triggeringTag;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float timer = 0f;
    [SerializeField] private float maxTime = 60f;
    [SerializeField] private int clickCount = 0;

    private bool isResuscitating = false;
    private bool isSuccess = false;
    private float startTime;
    private float clicksPerMin;
    private SpriteRenderer patientSpriteRenderer;
    
    void OnEnable()
    {
        push.Enable();
    }

    void OnDisable()
    {
        push.Disable();
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        patientSpriteRenderer = patient.GetComponent<SpriteRenderer>();
        resultText.text = "";
        rightClick.SetActive(false);
        wrongClick.SetActive(false);
    }

    void Update()
    {
        if (push.triggered && isResuscitating)
        {   
            float timeElapsed = Time.time - startTime;
            clickCount++;
            clicksPerMin = Mathf.RoundToInt((clickCount / timeElapsed) * 60);
            resultText.text = "You need to do between 100 to 120 clicks per minute\n Clicks per minute: " + clicksPerMin.ToString();
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

        if (isResuscitating)
        {
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
            StartResuscitation();
        }
    }

    private void StartResuscitation()
    {
        isResuscitating = true;
        audioSource.Play();
        startTime = Time.time;
    }
    
    private void EndResuscitation()
    {
        isResuscitating = false;
        audioSource.Stop();
        if (clicksPerMin >= requiredClicks && clicksPerMin <= maxClicks)
        {
            isSuccess = true;
        }
        if (isSuccess)
        {
            patientSpriteRenderer.sprite = standUp;
            resultText.text = "CPR successful! You did " + clicksPerMin.ToString() + " clicks per minute.";
        }
        else
        {
            resultText.text = "CPR failed. You did " + clicksPerMin.ToString() + " clicks per minute.";
        }
        rightClick.SetActive(false);
        wrongClick.SetActive(false);
    }
}
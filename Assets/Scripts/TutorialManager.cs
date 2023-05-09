using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// Define a class that handles the tutorial scene 
public class TutorialManager : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] InputAction moveUp = new InputAction(type: InputActionType.Button);
    [SerializeField] InputAction moveDown = new InputAction(type: InputActionType.Button);
    [SerializeField] InputAction moveLeft = new InputAction(type: InputActionType.Button);
    [SerializeField] InputAction moveRight = new InputAction(type: InputActionType.Button);
    [SerializeField] InputAction call101 = new InputAction(type: InputActionType.Button);
    [SerializeField] InputAction moveToAShadyPlace = new InputAction(type: InputActionType.Button);
    [SerializeField] InputAction water = new InputAction(type: InputActionType.Button);
    [SerializeField] private TextMeshPro text1;
    [SerializeField] private TextMeshPro text2;
    [SerializeField] private TextMeshPro text3;
    [SerializeField] private TextMeshPro text4;
    [SerializeField] private TextMeshPro text5;
    [SerializeField] private TextMeshPro text6;
    [SerializeField] private TextMeshPro text7;
    [SerializeField] string triggeringTag;
    [SerializeField] private Sprite lyeDown;
    [SerializeField] private Sprite standUp;
    [SerializeField] private GameObject patient;
    [SerializeField] private GameObject paramedic;
    private SpriteRenderer patientSpriteRenderer;
    private SpriteRenderer paramedicSpriteRenderer;
    private bool flag = true;
    Animator anim;
    [SerializeField] private InputAction yesInput;
    [SerializeField] private InputAction noInput;
    [SerializeField] private string sceneName;
    [SerializeField] private AudioSource audioSource;
    private IEnumerator coroutine;

   void Awake()
    {
        // Disable texts except for the first text
        text1.gameObject.SetActive(true);
        text2.gameObject.SetActive(false);
        text3.gameObject.SetActive(false);
        text4.gameObject.SetActive(false);
        text5.gameObject.SetActive(false);
        text6.gameObject.SetActive(false);
        text7.gameObject.SetActive(false);
    }

    void Start(){
        // Initialize variables and components
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        patientSpriteRenderer = patient.GetComponent<SpriteRenderer>();
        paramedicSpriteRenderer = paramedic.GetComponent<SpriteRenderer>();
        paramedicSpriteRenderer.enabled = false;
    }

    void OnEnable()
    {
        // Enable all InputActions when the script is enabled
        moveUp.Enable();
        moveDown.Enable();
        moveLeft.Enable();
        moveRight.Enable();
        call101.Enable();
        water.Enable();
        moveToAShadyPlace.Enable();
        yesInput.Enable();
        noInput.Enable();
    }

    void OnDisable()
    {
        // Disable all InputActions when the script is disabled
        moveUp.Disable();
        moveDown.Disable();
        moveLeft.Disable();
        moveRight.Disable();
        call101.Disable();
        water.Disable();
        moveToAShadyPlace.Disable();
        yesInput.Disable();
        noInput.Disable();
    }

    // Update is called once per frame
    void Update(){
        anim.enabled = false; // Disable the Animator by default
        if (moveUp.IsPressed()){
            anim.enabled = true;
            transform.position += new Vector3(0, speed * Time.deltaTime, 0);
            if(flag){ // Uses bool variable for changing to text2 only once when player moves on the first time in the scene 
            ChangeToText2();
            flag = false;
            }
        }
        else if (moveDown.IsPressed()){
            anim.enabled = true;
            transform.position += new Vector3(0, -speed * Time.deltaTime, 0);
            if(flag){ 
            ChangeToText2();
            flag = false;
            }
        }
        else if (moveRight.IsPressed()){
            anim.enabled = true;
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
            if(flag){ 
            ChangeToText2();
            flag = false;
            }
        } 
        else if (moveLeft.IsPressed()){
            anim.enabled = true;
            transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
            if(flag){ 
            ChangeToText2();
            flag = false;
            }
        }
        else if (call101.IsPressed()){ // When calling 101 the "phone dial and ring sound" audio is playing and the text changes accordingly when the paramedic answers the call (make this functionality using Coroutine)
            audioSource.Play();
            coroutine = paramedicAnswersTheCall();
            StartCoroutine(coroutine);      
        }
        else if(moveToAShadyPlace.IsPressed()){ // The player moves the patient to a specific position 
            Vector3 targetPosition = new Vector3(947f, 340f, 0f);
            transform.position = targetPosition;
            patient.transform.position = targetPosition + new Vector3(1.2f, 0f, 0f);
            text4.gameObject.SetActive(false);
            text5.gameObject.SetActive(true);
        }
        else if(yesInput.IsPressed()){ // The player press "yes" if the patient is lying down
            text5.gameObject.SetActive(false);
            text6.gameObject.SetActive(true);
        }
        else if(noInput.IsPressed()){ // The player press "No" if the patient is not lying down and it makes him to lye down using the patient's sprite
            patientSpriteRenderer.sprite = lyeDown;
            text5.gameObject.SetActive(false);
            text6.gameObject.SetActive(true);
        }
        else if(water.IsPressed()){ // The player gives water to the patient and saves his life 
            text6.gameObject.SetActive(false);
            text7.gameObject.SetActive(true);
            patientSpriteRenderer.sprite = standUp;
            SceneManager.LoadScene(sceneName);
        }
    }

    // This coroutine is called when the player calls 101 and waits for a paramedic to answer
    private IEnumerator paramedicAnswersTheCall()
    {
        yield return new WaitForSeconds(8);
        // Display the appropriate text and image to indicate that the paramedic has answered the call
        text3.gameObject.SetActive(false);
        text4.gameObject.SetActive(true);
        paramedicSpriteRenderer.enabled = true;  
    }

    private void ChangeToText2(){
        text1.gameObject.SetActive(false);
        text2.gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(triggeringTag))
        {
            text2.gameObject.SetActive(false);
            text3.gameObject.SetActive(true);
        }
    }
}

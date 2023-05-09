using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Define a class that handles the player movement in the game world
public class KeyboardSmoothMover : MonoBehaviour
{
    // Define a float variable to hold the speed at which the player moves
    [SerializeField] float speed = 1f;
    // Define input actions to handle movement in four directions
    [SerializeField] InputAction moveUp = new InputAction(type: InputActionType.Button);
    [SerializeField] InputAction moveDown = new InputAction(type: InputActionType.Button);
    [SerializeField] InputAction moveLeft = new InputAction(type: InputActionType.Button);
    [SerializeField] InputAction moveRight = new InputAction(type: InputActionType.Button);
    Animator anim;

    void Start(){
        anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        // Enable all InputActions when the script is enabled
        moveUp.Enable();
        moveDown.Enable();
        moveLeft.Enable();
        moveRight.Enable();
    }

    void OnDisable()
    {
        // Disable all InputActions when the script is disabled
        moveUp.Disable();
        moveDown.Disable();
        moveLeft.Disable();
        moveRight.Disable();
    }

    // Update is called once per frame
    void Update(){
        anim.enabled = false;
        if (moveUp.IsPressed()){
            anim.enabled = true;
            transform.position += new Vector3(0, speed * Time.deltaTime, 0);
        }
        else if (moveDown.IsPressed()){
            anim.enabled = true;
            transform.position += new Vector3(0, -speed * Time.deltaTime, 0);
        }
        else if (moveRight.IsPressed()){
            anim.enabled = true;
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        } 
        else if (moveLeft.IsPressed()){
            anim.enabled = true;
            transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
        }
    }
}
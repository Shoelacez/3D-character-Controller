using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform viewPoint;
    public float mouseSensitivity = 1f;
    private float verticalRotationStore;
    private Vector2 mouseInput;

    public float moveSpeed = 5f;
    private Vector3 moveDirection, movement;
    private float activeMoveSpeed;
    private float runSpeed = 8f;
    public float jumpForce = 12f, gravitymod = 2.5f;

    private Camera cam;

    public Transform groundCheck;
    private bool isGrounded;
    public LayerMask groundLayers;

    public CharacterController characterController;




    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        View();
        Movement();
    }
    void LateUpdate()
    {
        cam.transform.position = viewPoint.position;
        cam.transform.rotation = viewPoint.rotation;
    }

    void View()
    {
        mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);
        verticalRotationStore += mouseInput.y;
        verticalRotationStore = Mathf.Clamp(verticalRotationStore, -60f, 60f);
        viewPoint.rotation = Quaternion.Euler(verticalRotationStore + mouseInput.y, viewPoint.rotation.eulerAngles.y, viewPoint.rotation.eulerAngles.z);
    }
    void Movement()
    {
        //movement and Jumping
        moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        if (Input.GetKey(KeyCode.LeftShift))
        {
            activeMoveSpeed = runSpeed;
        }
        else
        {
            activeMoveSpeed = moveSpeed;
        }

        float yVel = movement.y;
        movement = ((transform.forward * moveDirection.z) + (transform.right * moveDirection.x)).normalized * activeMoveSpeed;
        movement.y = yVel;

        isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, 0.25f, groundLayers);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            movement.y = jumpForce;
        }

        movement.y += Physics.gravity.y * Time.deltaTime * gravitymod;

        if (characterController.isGrounded)
        {
            movement.y = 0f;
        }

        characterController.Move(movement * Time.deltaTime);

        //to avoid players failing to use the cursor after they are done playimg or want to use the menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else if (Cursor.lockState == CursorLockMode.None)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}

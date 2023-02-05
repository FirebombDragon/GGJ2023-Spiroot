using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerActions : MonoBehaviour
{
    private InputController controls;
    private Collider2D playerCollider;
    private Rigidbody2D rb;
    private Vector2 moveDir = Vector2.zero;
    private Vector2 velocity = Vector2.zero;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5f;
    private bool isGrounded = false;

    private int spirits = 0;

    // Start is called before the first frame update
    void Start()
    {
        controls = new InputController();
        rb = this.GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        controls.CharacterControls.Movement.performed += MovementInput;
        controls.CharacterControls.Movement.canceled += StopMovement;
        controls.CharacterControls.Movement.Enable();
        controls.CharacterControls.Jump.performed += Jump;
        controls.CharacterControls.Jump.Enable();
        controls.CharacterControls.Attack.performed += Root;
        controls.CharacterControls.Attack.Enable();
    }
    void Jump(CallbackContext ctx)
    {
        //Debug.Log(isGrounded);
        if (isGrounded)
            rb.AddForce(new Vector2(0, jumpForce));
    }
    void MovementInput(CallbackContext ctx)
    {
        moveDir.x = ctx.ReadValue<Vector2>().x * speed;
    }

    void StopMovement(CallbackContext ctx)
    {
        moveDir.x = 0;
    }

    void Root(CallbackContext ctx)
    {
        object[] obj = GameObject.FindObjectsOfType(typeof(GameObject));
        foreach (object o in obj)
        {
            GameObject g = (GameObject)o;
            if (g.tag == "Root")
            {
                if (g.transform.localScale.y == 1)
                {
                    g.transform.position = new Vector3(g.transform.position.x, g.transform.position.y + 5, g.transform.position.z);
                    g.transform.localScale = new Vector3(g.transform.localScale.x, g.transform.localScale.y * 10, g.transform.localScale.z);
                }
                else
                {
                    g.transform.position = new Vector3(g.transform.position.x, g.transform.position.y - 5, g.transform.position.z);
                    g.transform.localScale = new Vector3(g.transform.localScale.x, g.transform.localScale.y / 10, g.transform.localScale.z);
                }
            }
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = moveDir;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //Debug.Log("Triggered");
        if (other.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        //Debug.Log("Triggered");
        if (other.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerActions : MonoBehaviour
{
    //Controller that defines input mappings
    private InputController controls;
    //Collider of the player
    private Collider2D playerCollider;
    //Rigidbody of the player
    private Rigidbody2D rb;
    //Current direction the character is moving
    private Vector2 moveDir = Vector2.zero;
    //Current velocity of the player
    private Vector2 velocity = Vector2.zero;
    //How fast the character moves
    [SerializeField] private float speed = 5f;
    //How high the character is pushed when jumping
    [SerializeField] private float jumpForce = 5f;
    //Boolean to define whether the player is grounded
    private bool isGrounded = false;
    //Boolean to determine whether roots need to be extended or retracted
    private bool extended = false;
    private bool waiting = false;
    //How much the root changes in positioning when extending or retracting
    [SerializeField] private float rootpos = 0.3f;
    //How much the root changes in scale when extending
    [SerializeField] private float rootscale = 1.2f;

    // Start is called before the first frame update
    void Start()
    {
        //Initializes input controller
        controls = new InputController();
        //Initializes rigidbody
        rb = this.GetComponent<Rigidbody2D>();
        //Initializes collider
        playerCollider = GetComponent<Collider2D>();
        //Maps movement key to methods MovementInput and StopMovement
        controls.CharacterControls.Movement.performed += MovementInput;
        controls.CharacterControls.Movement.canceled += StopMovement;
        controls.CharacterControls.Movement.Enable();
        //Maps jump key to method Jump
        controls.CharacterControls.Jump.performed += Jump;
        controls.CharacterControls.Jump.Enable();
        //Maps root key to method Root
        controls.CharacterControls.Attack.performed += Root;
        controls.CharacterControls.Attack.Enable();
    }
    void Jump(CallbackContext ctx)
    {
        Debug.Log(isGrounded);
        //Jumps only if the player is grounded
        if (isGrounded)
            // rb.AddForce(new Vector2(0, jumpForce));
            StartCoroutine(Jumping());
    }
    void MovementInput(CallbackContext ctx)
    {
        //Sets the direction and speed in the direction of input if horizontal
        moveDir.x = ctx.ReadValue<Vector2>().x * speed;
    }

    void StopMovement(CallbackContext ctx)
    {
        //Stops any movement
        moveDir.x = 0;
    }

    void Root(CallbackContext ctx)
    {
        //Stops player from moving
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Root");
        GameObject[] horzroot = GameObject.FindGameObjectsWithTag("Horz Root");
        Debug.Log(extended);
        waiting = true;
        StartCoroutine(ScaleRoot(obj, horzroot));
        Debug.Log(extended);
        rb.constraints = RigidbodyConstraints2D.None;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (moveDir.y > 0)
            moveDir.y -= 0.1f;
        rb.velocity = moveDir;
        //Debug.Log(rb.velocity);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //Debug.Log("Triggered");
        isGrounded = true;
    }

    void OnCollisionExit2D(Collision2D other)
    {
        //Debug.Log("Triggered");
        if (other.gameObject.tag != "Spirit")
            isGrounded = false;
    }

    IEnumerator ScaleRoot(GameObject[] roots, GameObject[] horzroot)
    {
        foreach (GameObject root in roots)
        {
            for (int i = 0; i < 10; i++)
            {
                if (!extended)
                {
                    root.transform.position = new Vector3(root.transform.position.x, root.transform.position.y + rootpos, root.transform.position.z);
                    root.transform.localScale = new Vector3(root.transform.localScale.x, root.transform.localScale.y * rootscale, root.transform.localScale.z);
                }
                else
                {
                    root.transform.position = new Vector3(root.transform.position.x, root.transform.position.y - rootpos, root.transform.position.z);
                    root.transform.localScale = new Vector3(root.transform.localScale.x, root.transform.localScale.y / rootscale, root.transform.localScale.z);
                }
                yield return new WaitForSeconds(0.1f);
            }
        }
        foreach (GameObject root in horzroot)
        {
            for (int i = 0; i < 10; i++)
            {
                if (!extended)
                {
                    root.transform.position = new Vector3(root.transform.position.x + rootpos, root.transform.position.y, root.transform.position.z);
                    root.transform.localScale = new Vector3(root.transform.localScale.x * rootscale, root.transform.localScale.y, root.transform.localScale.z);
                }
                else
                {
                    root.transform.position = new Vector3(root.transform.position.x - rootpos, root.transform.position.y, root.transform.position.z);
                    root.transform.localScale = new Vector3(root.transform.localScale.x / rootscale, root.transform.localScale.y, root.transform.localScale.z);
                }
                yield return new WaitForSeconds(0.1f);
            }
        }
        extended = !extended;
    }
    IEnumerator Jumping()
    {
        moveDir.y = jumpForce;
        yield return new WaitForSeconds(0.1f);
    }
}

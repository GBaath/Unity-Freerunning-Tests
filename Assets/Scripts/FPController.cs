using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPController : MonoBehaviour
{

    [SerializeField] private HandsAnimator handsAnim;
    private Rigidbody rb;

    private Vector3 _move;
    [HideInInspector] public Vector3 move { get { return _move; } private set { _move = value; } }
    public Vector3 moveRaw;
    private Vector3 autoMove;

    [HideInInspector] public LayerMask playerLayers;

    public float speed;
    public float speedMultiplier = 1;
    public float jumpForce = 10;

    public float gravityScale;
    private float _gravityScale;
    [SerializeField] private float jumpDurationLeft;
    private float _jumpDurationLeft;
    public bool isJumping;

    private float moveX;
    private float moveZF;
    private float moveZB;
    private float moveZ;
    private float moveXRaw;
    private float moveZRaw;


    //for poslerp offset
    [HideInInspector]public float faceYOffset = 1.5f;

    private IEnumerator speedLerp;
    [SerializeField]private bool cr_running = false;
    [SerializeField]private bool canMove = true;
    [HideInInspector] public bool grounded;
    [HideInInspector] public bool lockCamera;


    //look
    private float mouseX;
    private float mouseY;

    public float mouseSens;
    public float sensMultiplier = 1f;
    private float xRotation = 0f;

    private float startFov;
    private float sprintFov;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        startFov = Camera.main.fieldOfView;
        sprintFov = startFov + 15;

        //hoes mad if dont do in start
        speedLerp = LerpSpeed(1.5f,sprintFov, 0.5f);

        //set default from inspector value
        _gravityScale = gravityScale;

        //hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //get player layers
        playerLayers += gameObject.layer;
        for (int i = 0; i < transform.childCount; i++)
        {
            playerLayers += transform.GetChild(i).gameObject.layer;
        }
    }

    private void Update()
    {
        //for input reference
        moveXRaw = Input.GetAxisRaw("Horizontal");
        moveX = Input.GetAxis("Horizontal");
        moveZF = Input.GetAxis("VerticalForward")*2;
        moveZB = Input.GetAxis("VerticalBackward");
        moveZRaw = Input.GetAxisRaw("VerticalForward") + Input.GetAxisRaw("VerticalBackward");
        //total zmove
        moveZ = moveZF + moveZB;
        //total move input
        move = transform.right * moveX + transform.forward * moveZ;
        moveRaw = transform.right * moveXRaw + transform.forward * moveZRaw;

        mouseX = Input.GetAxis("Mouse X") * mouseSens * sensMultiplier * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSens * sensMultiplier * Time.deltaTime;

        SprintChecks();
        Look();
        Jump();
    }
    private void FixedUpdate()
    {
        Move();
    }
    private void Look()
    {

        //rotate player and tilt camera
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        if(!lockCamera)
            Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
    public void Move()
    {
        if (canMove)
        {
            //if wall in move dir
            /*if(Physics.Raycast(transform.position, move.normalized, 0.75f, ~playerLayers))
            {
                move = Vector3.zero;
            }*/
            //move does not modify y vel
            if (grounded)
            {
                Vector3 vel = move * speed * speedMultiplier;
                vel.y += rb.velocity.y;
                vel += autoMove;

                rb.velocity = vel;        
            }
            else
            {
                Vector3 vel = autoMove + rb.velocity;
                rb.velocity = vel;
            }
        }
        //gravity
        Vector3 velocity = rb.velocity;
        velocity.y -= gravityScale;
        rb.AddForce(Vector3.down * gravityScale);
    }
    private void Jump()
    {
        //back on ground no input
        if (grounded & !Input.GetButton("Jump"))
        {
            _jumpDurationLeft = jumpDurationLeft;
            isJumping = false;
        }

        //if hold and duration is left, increase height
        if (Input.GetButton("Jump")&& canMove && _jumpDurationLeft > 0)
        {
            Vector3 velocity = rb.velocity;
            velocity.y += jumpForce;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            isJumping = true;
        }
        //lower hold duration
        if (!grounded && Input.GetButton("Jump")&&canMove)
        {
            _jumpDurationLeft -= Time.deltaTime;
            isJumping = true;
        }
        //when release
        if (Input.GetButtonUp("Jump"))
        {
            _jumpDurationLeft = 0;
            isJumping = false;
        }
    }
    public void StopJump()
    {
        _jumpDurationLeft = 0;
    }
    private void SprintChecks()
    {
        //sprint when keydown and forwardmove, stop when release forward move
        if (Input.GetButtonDown("Sprint") && moveZF>0 && speedMultiplier==1)
        {
            //set anim bool
            handsAnim.sprinting = true;

            //increase fov and speed
            StopCoroutine(speedLerp);
            cr_running = false;
            speedLerp = LerpSpeed(1.5f,sprintFov ,0.5f);
            StartCoroutine(speedLerp);

            //sens mult
            sensMultiplier = 0.5f;
        }
        if((moveZRaw<=0 && cr_running==false && speedMultiplier==1.5f)|| cr_running == false && speedMultiplier == 1.5f && Input.GetButtonDown("Sprint"))
        {
            //set anim bool
            handsAnim.sprinting = false;

            //decrease fov and speed
            StopCoroutine(speedLerp);
            cr_running = false;
            speedLerp = LerpSpeed(1,startFov, 0.25f);
            StartCoroutine(speedLerp);

            //sens mult
            sensMultiplier = 1;
        }
    }
    //set from unityevent in other collder
    public void Grounded(bool grounded)
    {
        this.grounded = grounded;
    }
    public void ResetGravityScale()
    {
        gravityScale = _gravityScale;
    }
    public void AddDirectionForce(Vector3 force)
    {
        rb.AddForce(force, ForceMode.VelocityChange);
    }
    //set camera fov depending on velocity
    private void SetSpeedFOV()
    {
        float percentage = speed / (speed * speedMultiplier);
        percentage = Mathf.Pow(percentage, -1);
        Camera.main.fieldOfView = Mathf.Clamp(startFov * percentage,startFov,sprintFov);
    }
    private IEnumerator LerpSpeed(float endVal, float duration)
    {
        float time = 0;
        float startValue = speedMultiplier;
        cr_running = true;
        while (time < duration)
        {
            speedMultiplier = Mathf.Lerp(startValue, endVal, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        speedMultiplier = endVal;
        cr_running = false;
    }
    //lerp speed and fov
    private IEnumerator LerpSpeed(float endVal, float endFov, float duration)
    {
        float time = 0;
        float startValue = speedMultiplier;
        //float startFov = Camera.main.fieldOfView;
        cr_running = true;
        while (time < duration)
        {
            speedMultiplier = Mathf.Lerp(startValue, endVal, time / duration);
            SetSpeedFOV();
            time += Time.deltaTime;
            yield return null;
        }
        speedMultiplier = endVal;
        cr_running = false;
        SetSpeedFOV();
    }

    public IEnumerator LerpPlayerPos(Vector3 endValue, float duration)
    {
        //temp disable gravity & lock movement
        gravityScale = 0;
        canMove = false;
        float time = 0;
        Vector3 startValue = transform.position;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startValue, endValue, time/duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = endValue;
        canMove = true;
        gravityScale = _gravityScale;
    }
    public IEnumerator LerpPlayerY(float endVal, float duration)
    {
        gravityScale = 0;
        float time = 0;
        float startVal = transform.position.y;
        while (time<duration)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(startVal, endVal, time / duration), transform.position.z);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = new Vector3(transform.position.x, endVal, transform.position.z);
        gravityScale = _gravityScale;
    }
    //lerps automove value to zero
    public IEnumerator LerpAutoMove(Vector3 startValue, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            autoMove = Vector3.Lerp(startValue, Vector3.zero, time/duration);
            time += Time.deltaTime;
            yield return null;
        }
        autoMove = Vector3.zero;
    }
    public IEnumerator SlerpPlayerPos(Vector3 endValue, float duration)
    {
        //temp disable gravity & lock movement
        gravityScale = 0;
        canMove = false;
        float time = 0;
        Vector3 startValue = transform.position;
        while (time < duration)
        {
            transform.position = Vector3.Slerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = endValue;
        canMove = true;
        gravityScale = _gravityScale;
    }
}

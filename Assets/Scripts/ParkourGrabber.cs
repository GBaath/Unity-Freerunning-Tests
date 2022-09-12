using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkourGrabber : MonoBehaviour
{
    public ObstacleChecker obstCheck;
    public SphereCollider topColl;
    public CapsuleCollider lowColl;

    [SerializeField] private HandsAnimator handAnim;
    public LayerMask climbLayer;

    private FPController fpController;
    private Rigidbody rb;

    public float climbSpeed =0.5f;
    private bool canWallJump;

    [SerializeField]private bool doingMove = false;
    private void Start()
    {
        fpController = GetComponent<FPController>();
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        
        //if unput vaultover & nothing in the way & not doing another move //&& has speed forward
        if (Input.GetButton("Hand") && obstCheck.blockedLow &! obstCheck.blockedForward &! obstCheck.blockedMid &! doingMove) //&& (rb.velocity.magnitude*transform.forward).magnitude > 2f)
        {
            if (Input.GetAxisRaw("VerticalForward")>0)
            {
                doingMove = true;
                //pause grav, disable lower collider, timer
                VaultForward();
            }
        }
        //jump & obstacleinfront+low & space above it & not doing another
        else if (Input.GetButton("Jump") && obstCheck.blockedLow & !obstCheck.blockedMid & !obstCheck.blockedAbove & !doingMove)
        {
            if (Input.GetAxisRaw("VerticalForward") > 0)
            {
                fpController.StopJump();
                doingMove = true;
                VaultUp();
            }
        }
        //jump & obstacleinfront & space above it & not doing another
        else if (Input.GetButton("Jump") && obstCheck.blockedMid &!obstCheck.blockedAbove & !doingMove)
        {
            if (Input.GetAxisRaw("VerticalForward") > 0)
            {
                fpController.StopJump();
                doingMove = true;
                //playing animation for moving climbcollider
                //  colliderAnimator.enabled = true;
                ClimbUp();
            }
            
        }
        else if(Input.GetButton("Jump") & !doingMove && canWallJump)
        {
            Debug.Log("wjump");
        }
    }
    public void VaultForward()
    {
        //play vault1 animation //TODO make several and randomize
        handAnim.SetTrigger("Vault1");

        //lerping legs collider for climbeffect
        lowColl.center = new Vector3(0, 0.4f, 0);
        IEnumerator delayHolder = LerpColliderCenter(new Vector3(0, -0.7f, 0), 0.25f, lowColl);
        StartCoroutine(CoroutineDelay(delayHolder,0.25f));
        fpController.gravityScale = fpController.gravityScale/50;
        fpController.Invoke("ResetGravityScale",0.75f);

        //automove
        fpController.StartCoroutine(fpController.LerpAutoMove(fpController.moveRaw*3, 1f));

        //reset doingMove in 500ms
        StartCoroutine(VarChange(result => doingMove = result, 0.5f, false));
        //sensmultiplier
        fpController.sensMultiplier = 0.5f;
        StartCoroutine(VarChange(result => fpController.sensMultiplier = result, 0.5f, 1f));
    }
    public void VaultUp()
    {
        //play vault1 animation //TODO make several and randomize
        handAnim.SetTrigger("VaultUp1");

        //lerping legs collider for climbeffect
        lowColl.center = new Vector3(0, 0.4f, 0);
        IEnumerator delayHolder = LerpColliderCenter(new Vector3(0, -0.7f, 0), 0.25f, lowColl);
        StartCoroutine(CoroutineDelay(delayHolder, 0.1f));
        fpController.gravityScale = fpController.gravityScale / 50;
        fpController.Invoke("ResetGravityScale", 0.75f);

        //automove
        fpController.StartCoroutine(fpController.LerpAutoMove(fpController.moveRaw * 2, 1f));

        //lerpY

        //ray for destination point
        RaycastHit ray;
            Vector3 raystartpos = new Vector3(obstCheck.transform.position.x, obstCheck.transform.position.y + 2, obstCheck.transform.position.z);
            Physics.Raycast(raystartpos, Vector3.down, out ray, 2f);
            Vector3 pos = ray.point + Vector3.up*0.5f; //y+1 cus player center is higher


        fpController.StartCoroutine(fpController.LerpPlayerY(pos.y, 0.25f));

        //reset doingMove in 500ms
        StartCoroutine(VarChange(result => doingMove = result, 0.5f, false));
        //sensmultiplier
        fpController.sensMultiplier = 0.5f;
        StartCoroutine(VarChange(result => fpController.sensMultiplier = result, 0.5f, 1f));
    }
    public void ClimbUp()
    {

        //anim
        handAnim.SetTrigger("Climb1");
        //match speed of climb with anim
        handAnim.SetAnimSpeed(Mathf.Pow(climbSpeed,-1));

        //lerppos
        //ray for destination point
        //TODO NÅT HÄÖR ÖR JÖVLIGT FEL LÖS PROBLEMET
        RaycastHit ray;
            Vector3 raystartpos = new Vector3(obstCheck.transform.position.x, obstCheck.transform.position.y + 5, obstCheck.transform.position.z);
            Physics.Raycast(raystartpos, Vector3.down, out ray, 5f);
            Vector3 pos = ray.point + Vector3.up; //y+1 cus player center is higher
            
        //extra forwardmove so you wont fall bdown backwards
        float xzDif = (pos.x + pos.z) - (transform.position.x + transform.position.z);
        if (xzDif < 1.2f)
            pos += transform.forward * 0.25f;

        //climbingspeed
        float cSpeed = Vector3.Distance(ray.point, Camera.main.transform.position);
        fpController.StartCoroutine(fpController.SlerpPlayerPos(pos, climbSpeed));
        //fpController.StartCoroutine(fpController.LerpPlayerY(pos.y,climbSpeed));

        ////automove
        //fpController.StartCoroutine(fpController.LerpAutoMove(fpController.moveRaw * climbSpeed ,climbSpeed));

        //lowcolLerpfor fallthru-proofing //should prollly convert to method
        lowColl.center = new Vector3(0, 0.4f, 0);
        IEnumerator delayHolder = LerpColliderCenter(new Vector3(0, -0.7f, 0), climbSpeed/4, lowColl);
        StartCoroutine(CoroutineDelay(delayHolder, climbSpeed/2));

        //tempdisable colliders for posLerp
        topColl.enabled = false;
        lowColl.enabled = false;
        StartCoroutine(VarChange(result => topColl.enabled = result, climbSpeed-0.25f, true));
        StartCoroutine(VarChange(result => lowColl.enabled = result, climbSpeed-0.25f, true));

        //reset doingMove in 500ms
        StartCoroutine(VarChange(result => doingMove = result, 1f, false));
        //sensmultiplier
        fpController.sensMultiplier = 0.5f;
        StartCoroutine(VarChange(result => fpController.sensMultiplier = result, 0.5f, 1f));
    }
    public void MoveDone()
    {
        doingMove = false;
    }
    public void CanWallJump(bool canWallJump)
    {
        this.canWallJump = canWallJump;
    }
    IEnumerator VarChange(System.Action<bool> boolVar, float cooldown, bool endValue)
    {
        yield return new WaitForSeconds(cooldown);
        boolVar(endValue);
    }
    IEnumerator VarChange(System.Action<float> floatVar, float cooldown, float endValue)
    {
        yield return new WaitForSeconds(cooldown);
        floatVar(endValue);
    }
    private IEnumerator LerpColliderCenter(Vector3 endVal, float duration, CapsuleCollider capsuleCollider)
    {
        float time = 0;
        Vector3 startVal = capsuleCollider.center;
        while (time < duration)
        {
            capsuleCollider.center = Vector3.Lerp(startVal, endVal, time/duration);
            time += Time.deltaTime;
            yield return null;
        }
        capsuleCollider.center = endVal;
    }
    private IEnumerator CoroutineDelay(IEnumerator ien, float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(ien);
    }
}

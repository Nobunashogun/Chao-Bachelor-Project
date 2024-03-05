using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;


public class CraneScript : MonoBehaviour
{

    [Header("Arms")]
    public GameObject arm1;
    public GameObject arm2;
    public GameObject arm3;

    [Header("Movement")]
    public float downSpeed;
    public float rotationSpeed;
    public float armSpeed;
    public float MaxArmSpeed;
    public float smoothDownSpeed;
    public float returnSpeed;
    public float raycastDistance;
    private bool rotateArms;
    private bool extractDown;
    private bool extractUp;
    private bool canSteer = true;
    private Rigidbody rb;
    private Player player;
    private float startRotZ;
    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(0);
        rb = GetComponent<Rigidbody>();
        startRotZ = arm1.transform.rotation.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        MoveUfo();
        if(extractDown)
        {
            DownAndRaycast();
        }
        RotateArms();
    }
    void DownAndRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 100f))
        {
            Debug.DrawRay(transform.position, Vector3.down * hit.distance, Color.yellow);
            if(hit.distance >= raycastDistance)
            {
                rb.velocity = Vector3.down*downSpeed*Time.fixedDeltaTime;
            }
            else
            {
                rb.velocity = Vector3.zero;
                rotateArms = true;
                extractDown = false;
            }
        }
    }
    void MoveUfo()
    {
        if (player.GetButtonDown("Confirm")&&canSteer)
        {
            canSteer = false;
            extractDown = true;
        }
        if (canSteer)
        {
            var camera = Camera.main;

            //camera forward and right vectors:
            var forward = camera.transform.forward;
            var right = camera.transform.right;
            
            //steering.Normalize();
            //project forward and right vectors on the horizontal plane (y = 0)
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            //this is the direction in the world space we want to move:
            var desiredMoveDirection = forward * player.GetAxisRaw("Vertical") + right * player.GetAxisRaw("Horizontal");
            desiredMoveDirection.Normalize();
            //transform.position += desiredMoveDirection*armSpeed*Time.deltaTime;
            if(rb.velocity.magnitude <= MaxArmSpeed)
            {
                rb.velocity += desiredMoveDirection * armSpeed * Time.fixedDeltaTime;
            }
            
            
            if(rb.velocity.magnitude >= 0)
            {
                    rb.velocity -= rb.velocity * smoothDownSpeed;
            }
                
            
            
        }
    }

    void RotateArms()
    {
        
        float Angle = Mathf.SmoothDampAngle(arm1.transform.rotation.eulerAngles.z, targetangle, ref r, moveDuration);
        transform.rotation = Quaternion.Euler(0, Angle, 0);
    }
}

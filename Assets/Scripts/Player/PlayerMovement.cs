using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private static Vector3 startPosition;
    private static Quaternion startRotation;

    [SerializeField] private PlayerData playerData;
    [SerializeField] private LayerMask drivableLayer;
    [SerializeField] private Transform[] hitPoints;
    [SerializeField] private Transform[] wheels;
    [SerializeField] private Transform[] wheelsVisuals;

    private Vector3 localVelocity;
    private float velocityRatio;
    private bool isGrounded;
    private bool wasGrounded;
    private Rigidbody body;

    // Input data
    private float accel;
    private float steer;
    private float breaking;
    private float flip;

    [SerializeField] private AudioSource engineSound;
    [SerializeField] private AudioSource hitSound;
    private float currentPitch = 0.0f;

    private bool isEnable = true;

    private void Awake()
    {
        InputManager.onAccelerate += OnAccelerate;
        InputManager.onBreak += OnBreak;
        InputManager.onSteer += OnSteer;
        InputManager.onFlip += OnFlip;

        startPosition = transform.position;
        startRotation = transform.rotation;
        body = GetComponent<Rigidbody>();
        velocityRatio = 0.0f;
        isGrounded = false;
        wasGrounded = isGrounded;
        isEnable = true;
    }

    private void OnDestroy()
    {
        InputManager.onAccelerate -= OnAccelerate;
        InputManager.onBreak -= OnBreak;
        InputManager.onSteer -= OnSteer;
        InputManager.onFlip -= OnFlip;
    }

    private void Update()
    {
        if (isEnable)
        {
            currentPitch = Mathf.Lerp(0.5f, 1.0f, Mathf.Abs(accel));
            engineSound.pitch = currentPitch;

            UpdateWheelRotation();
        }
    }

    private void FixedUpdate()
    {
        ProcessLocalVelocity();
        Suspension();
        if (isEnable)
        {
            ProcessMovement();
        }
        else
        {
            transform.rotation = startRotation;
            transform.position = startPosition;
            body.position = startPosition;
            body.velocity = Vector3.zero;
        }
    }

    public void SetEnable(bool value)
    {
        isEnable = value;
        if (isEnable)
        {
            currentPitch = 0.5f;
            engineSound.Play();
        }
        else
        {
            engineSound.Stop();
        }
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    public float GetVelocityRatio()
    {
        return velocityRatio;
    }

    public void OnAccelerate(float accel)
    {
        this.accel = accel;
    }

    public void OnBreak(float breaking)
    {
        this.breaking = breaking;
    }

    public void OnSteer(float steer)
    {
        this.steer = steer;
    }

    public void OnFlip(float flip)
    {
        this.flip = flip;
    }

    private void ProcessLocalVelocity()
    {
        if(isGrounded)
        {
            localVelocity = transform.InverseTransformDirection(body.velocity);
            velocityRatio = localVelocity.z / playerData.maxVelocity;
        }
    }

    private void ProcessMovement()
    {
        if(isGrounded)
        {
            body.AddTorque(steer * body.transform.up * playerData.rotVelcoity *
                playerData.turnCurve.Evaluate(Mathf.Abs(velocityRatio)) * Mathf.Sign(velocityRatio),
             ForceMode.Acceleration);

            body.AddForce(body.transform.forward * accel * playerData.speedForce, ForceMode.Acceleration);
            if(localVelocity.z > 0)
            {
                body.AddForce(-body.transform.forward * breaking * playerData.breakFroce, ForceMode.Acceleration);
            }
           
            float sidewaySpeed = localVelocity.x;
            float dragMagnitude = -sidewaySpeed * playerData.dragCoefficient;
            Vector3 dragForce = transform.right * dragMagnitude;
            body.AddForceAtPosition(dragForce, body.worldCenterOfMass, ForceMode.Acceleration);   

            if(body.velocity.magnitude > playerData.maxVelocity)
            {
                body.velocity = body.velocity.normalized * playerData.maxVelocity; 
            }
        }
        else
        {
            body.AddTorque(steer * body.transform.up * playerData.rotVelcoity, ForceMode.Acceleration);
            body.AddTorque(flip * body.transform.right * playerData.rotVelcoity, ForceMode.Acceleration);
        }
    }

    private void Suspension()
    {
        isGrounded = false;
        int i = 0;
        foreach(Transform hitRay in hitPoints)
        {
            Vector3 newPosition = wheels[i].position;

            Ray ray = new Ray(hitRay.position, -hitRay.up);
            RaycastHit hit;
            float maxLength = playerData.restLength + playerData.springTravel;
            if(Physics.Raycast(ray, out hit, maxLength + playerData.wheelsRadius, drivableLayer))
            {
                isGrounded = true;
                float springLegth = hit.distance - playerData.wheelsRadius;
                float springCompression = (playerData.restLength - springLegth) / playerData.springTravel;

                float springVelocity = Vector3.Dot(body.GetPointVelocity(hitRay.position), hitRay.up);
                float dampForce = playerData.damperCoefficient * springVelocity;

                float force = (playerData.springConstant * springCompression) - dampForce;
                body.AddForceAtPosition(force * hitRay.up, hitRay.position);

                newPosition = hit.point + hitRay.up * playerData.wheelsRadius;
            }
            else
            {
                Vector3 targetPosition = hitRay.position -  hitRay.up * maxLength;
                newPosition = Vector3.Lerp(wheels[i].position, targetPosition, 10.0f*Time.deltaTime);
            }

            wheels[i].position = newPosition;
            i++;
        }

        if (isGrounded && !wasGrounded)
        {
            hitSound.Play();
        }
        wasGrounded = isGrounded;

    }

    private void UpdateWheelRotation()
    {
        for(int i = 0; i < 2; i++)
        {
            float steeringAngle = 30.0f * steer;
            wheels[i].localEulerAngles = new Vector3(wheels[i].localEulerAngles.x, steeringAngle, wheels[i].localEulerAngles.z);                
        }

        for(int i = 0; i < 4; ++i)
        {
            wheelsVisuals[i].Rotate(Vector3.right, velocityRatio * 1200.0f * Time.deltaTime, Space.Self);
        }
    }

    public Vector3 GetLocalVelocity()
    {
        return localVelocity;
    }
    
}

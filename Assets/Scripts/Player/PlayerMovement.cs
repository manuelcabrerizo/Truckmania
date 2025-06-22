using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static event Action<PlayerMovement> onPlayerCreated;

    private static Vector3 startPosition;
    private static Quaternion startRotation;

    [SerializeField] private PlayerData playerData;

    [SerializeField] private LayerMask drivableLayer;
    [SerializeField] private Transform[] hitPoints;
    [SerializeField] private Transform[] wheels;
    [SerializeField] private Transform[] wheelsVisuals;

    [SerializeField] private ParticleSystem dirtLeft;
    [SerializeField] private ParticleSystem dirtRight;

    private Vector3 localVelocity;
    private float velocityRatio;
    private bool isGrounded;
    private bool wasGrounded;
    private Rigidbody body;

    private bool isDrifting;

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
        InputManager.onJump += OnJump;

        startPosition = transform.position;
        startRotation = transform.rotation;
        body = GetComponent<Rigidbody>();
        velocityRatio = 0.0f;
        isGrounded = false;
        wasGrounded = isGrounded;
        isEnable = true;
        isDrifting = false;
    }

    private void Start()
    {
        onPlayerCreated?.Invoke(this);
    }
    private void OnDestroy()
    {
        InputManager.onAccelerate -= OnAccelerate;
        InputManager.onBreak -= OnBreak;
        InputManager.onSteer -= OnSteer;
        InputManager.onFlip -= OnFlip;
        InputManager.onJump -= OnJump;
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

    public void OnJump()
    {
        if (isGrounded)
        {
            // zeron velocity in the up direction
            Vector3 downVelocity = Vector3.Dot(-transform.up, body.velocity) * transform.up;
            body.velocity -= downVelocity;
            body.AddForce(transform.up * 100.0f, ForceMode.Impulse);
        }
    }

    private void ProcessLocalVelocity()
    {
        if (isGrounded)
        {
            localVelocity = transform.InverseTransformDirection(body.velocity);
            velocityRatio = localVelocity.z / playerData.maxVelocity;
        }
    }

    private void ProcessMovement()
    {
        if (isGrounded && isDrifting)
        {
            dirtLeft.Play();
            dirtRight.Play();
        }
        else
        {
            dirtLeft.Stop();
            dirtRight.Stop();
        }

        if (isGrounded)
        {
            ProcessGroundMovement();
        }
        else
        {
            ProcessAirMovement();
        }
    }

    private void ProcessGroundMovement()
    {
        Plane xzPlane = new Plane(transform.up, 0.0f);
        Vector3 forwardVel = xzPlane.ClosestPointOnPlane(body.velocity);
        forwardVel.Normalize();

        float sliptAngle = 0.0f;
        if (localVelocity.z >= 0.0f)
        {
            sliptAngle = Vector3.Angle(transform.forward, forwardVel);
        }
        else
        {
            sliptAngle = Vector3.Angle(-transform.forward, forwardVel);
        }

        // TODO: test this Animation curve
        if (!isDrifting && breaking >= 0.001f)
        {
            isDrifting = true;
        }
        if (isDrifting && sliptAngle * playerData.turnCurve.Evaluate(Mathf.Abs(velocityRatio)) <= playerData.driftAngle)
        {
            isDrifting = false;
        }

        float dragCoefficient = playerData.dragCoefficient;
        float rotVelocity = Mathf.Lerp(playerData.rotVelcoity, playerData.breakingRotVelocity, breaking);
        if (isDrifting)
        {
            dragCoefficient = playerData.driftDragCoefficient;
            rotVelocity *= playerData.driftRotVelocityMul;
        }

        body.AddTorque(steer * body.transform.up * rotVelocity * playerData.turnCurve.Evaluate(Mathf.Abs(velocityRatio)) * Mathf.Sign(velocityRatio), ForceMode.Acceleration);
        body.AddForce(body.transform.forward * accel * playerData.speedForce, ForceMode.Acceleration);
        if (localVelocity.z > 0)
        {
            body.AddForce(-body.transform.forward * breaking * playerData.breakFroce, ForceMode.Acceleration);
        }

        float sidewaySpeed = localVelocity.x;
        float dragMagnitude = -sidewaySpeed * dragCoefficient;
        Vector3 dragForce = transform.right * dragMagnitude;
        body.AddForceAtPosition(dragForce, body.worldCenterOfMass, ForceMode.Acceleration);

        Vector3 velocity = body.velocity;
        if (velocity.magnitude > playerData.maxVelocity)
        {
            velocity = velocity.normalized * playerData.maxVelocity;
        }
        body.velocity = velocity;
    }

    private void ProcessAirMovement()
    {
        if (breaking > 0.0f)
        {
            body.AddTorque(-steer * body.transform.forward * playerData.airRotVelocity*1.5f, ForceMode.Acceleration);
        }
        else
        {
            body.AddTorque(steer * body.transform.up * playerData.airRotVelocity, ForceMode.Acceleration);
        }
        body.AddTorque(flip * body.transform.right * playerData.airRotVelocity, ForceMode.Acceleration);
    }

    private void Suspension()
    {
        isGrounded = false;
        int i = 0;
        foreach (Transform hitRay in hitPoints)
        {
            Vector3 newPosition = wheels[i].position;

            Ray ray = new Ray(hitRay.position, -hitRay.up);
            RaycastHit hit;
            float maxLength = playerData.restLength + playerData.springTravel;
            if (Physics.Raycast(ray, out hit, maxLength + playerData.wheelsRadius, drivableLayer))
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
                Vector3 targetPosition = hitRay.position - hitRay.up * maxLength;
                newPosition = Vector3.Lerp(wheels[i].position, targetPosition, 10.0f * Time.deltaTime);
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
        for (int i = 0; i < 2; i++)
        {
            float steeringAngle = 30.0f * steer;
            wheels[i].localEulerAngles = new Vector3(wheels[i].localEulerAngles.x, steeringAngle, wheels[i].localEulerAngles.z);
        }

        for (int i = 0; i < 4; ++i)
        {
            if (i >= 2)
            {
                if (Mathf.Abs(accel) > 0.001f)
                {
                    wheelsVisuals[i].Rotate(Vector3.right, (1.0f - breaking) * accel * 1200.0f * Time.deltaTime, Space.Self);
                }
                else
                {
                    wheelsVisuals[i].Rotate(Vector3.right, (1.0f - breaking) * velocityRatio * 1200.0f * Time.deltaTime, Space.Self);
                }
            }
            else
            {
                wheelsVisuals[i].Rotate(Vector3.right, velocityRatio * 1200.0f * Time.deltaTime, Space.Self);
            }
        }
    }

    public Vector3 GetLocalVelocity()
    {
        return localVelocity;
    }

}
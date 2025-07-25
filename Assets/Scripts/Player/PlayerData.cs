using System;
using UnityEngine;

[Serializable]
public class PlayerData
{
    [Header("Layers")]
    public LayerMask drivableLayer;
    public LayerMask enemyMask;

    [Header("References")]
    public PlayerDataSo playerData;
    public SoundClipsSO clips;
    public Transform[] hitPoints;
    public Transform[] wheels;
    public Transform[] wheelsVisuals;
    public Material[] materials;
    public ParticleSystem dirtLeft;
    public ParticleSystem dirtRight;
    public Transform barrilTransform;

    [Header("Gameplay State")]
    public bool isCoundown;
    public bool isGrounded;
    public bool wasGrounded;
    public Vector3 localVelocity;
    public float velocityRatio;
    public float sliptAngle;
    public float upsideDownRatio;
    public ToxicBarrilProjectile barril = null;
    public bool wasDrifting = false;
    public bool keepDrifting = false;
    public bool isNoclipCheatActive = false;
    public bool isGodModeCheatActive = false;


    [Header("Input State")]
    public float accel;
    public float steer;
    public float breaking;
    public float flip;
    public float sideFlip;

    [Header("Components")]
    public Transform transform;
    public Rigidbody body;
    public Collider collision;
    public PlayerAimBar aimBar;
    public CameraMovement cameraMovement;
    public AudioSource engineSound;

    public void Initialize()
    {
        GameEventManager.Instance.AddListener<CountDownStateEnterEvent>(OnCountDownEnter);
        GameEventManager.Instance.AddListener<CountDownStateExitEvent>(OnCountDownExit);
        GameEventManager.Instance.AddListener<ToxicBarrilPickEvent>(OnBarrilPickUp);
        GameEventManager.Instance.AddListener<CameraCreatedEvent>(OnCameraCreated);
    }

    public void Destroy()
    {
        GameEventManager.Instance.RemoveListener<CountDownStateEnterEvent>(OnCountDownEnter);
        GameEventManager.Instance.RemoveListener<CountDownStateExitEvent>(OnCountDownExit);
        GameEventManager.Instance.RemoveListener<ToxicBarrilPickEvent>(OnBarrilPickUp);
        GameEventManager.Instance.RemoveListener<CameraCreatedEvent>(OnCameraCreated);

        if (barril)
        {
            barril.SendReleaseEvent();
        }

    }

    public void Restart()
    {
        if (barril)
        {
            barril.SendReleaseEvent();
        }
        body.position = Player.startPosition;
        transform.position = Player.startPosition;
        transform.rotation = Player.startRotation;
        barril = null;
        isGrounded = false;
        wasGrounded = false;
        wasDrifting = false;
        keepDrifting = false;
        localVelocity = Vector3.zero;
        velocityRatio = 0.0f;
        sliptAngle = 0;
        isNoclipCheatActive = false;
        cameraMovement.Restart();
    }

    public void Update(Player player)
    {
        if (!isCoundown)
        {
            UpdateWheelRotation();
        }
    }

    public void FixedUpdate(Player player)
    {
        if (!isNoclipCheatActive)
        {
            upsideDownRatio = Vector3.Dot(player.transform.up, Vector3.up);
            ProcessLocalVelocity(player.transform);
            Suspension();
        }
    }

    private void ProcessLocalVelocity(Transform transform)
    {
        if (isGrounded)
        {
            localVelocity = transform.InverseTransformDirection(body.velocity);
            velocityRatio = localVelocity.z / playerData.maxVelocity;
        }
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
    private void OnBarrilPickUp(GameEvent gameEvent)
    {
        ToxicBarrilPickEvent pickEvent = (ToxicBarrilPickEvent)gameEvent;
        ToxicBarrilProjectile barril = pickEvent.barril;

        if (this.barril != null)
        {
            this.barril.SendReleaseEvent();
        }
        this.barril = barril;
        this.barril.gameObject.layer = LayerMask.NameToLayer("ToxicBarril");
    }
    private void OnCameraCreated(GameEvent gameEvent)
    {
        CameraCreatedEvent cameraCreatedEvent = (CameraCreatedEvent)gameEvent;
        this.cameraMovement = cameraCreatedEvent.cameraMovement;
    }

    public void OnCountDownEnter(GameEvent gameEvent)
    {
        isCoundown = true;
    }

    public void OnCountDownExit(GameEvent gameEvent)
    {
        isCoundown = false;
    }

}

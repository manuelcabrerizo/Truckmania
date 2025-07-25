using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CameraFollow cameraFollow;
    private CinemachineVirtualCamera vCameraFolow;

    [SerializeField] private CameraLockTarget cameraLockTarget;
    private CinemachineVirtualCamera vCameraLockTarget;

    private bool isLock = false;

    private void Awake()
    {
        GameEventManager.Instance.AddListener<LockCameraEvent>(OnLockCamera);
        GameEventManager.Instance.AddListener<BigfootKillEvent>(OnBigfootKill);
        GameEventManager.Instance.AddListener<EndStateEnterEvent>(OnEnterEndState);
        isLock = false;
        vCameraFolow = cameraFollow.GetComponent<CinemachineVirtualCamera>();
        vCameraLockTarget = cameraLockTarget.GetComponent<CinemachineVirtualCamera>();
        vCameraFolow.Priority = 20;
        vCameraLockTarget.Priority = 10;
    }

    private void OnDestroy()
    {
        GameEventManager.Instance.RemoveListener<LockCameraEvent>(OnLockCamera);
        GameEventManager.Instance.RemoveListener<BigfootKillEvent>(OnBigfootKill);
        GameEventManager.Instance.RemoveListener<EndStateEnterEvent>(OnEnterEndState);
    }

    private void Start()
    {
        GameEventManager.Instance.TriggerEvent(new CameraCreatedEvent(this));
    }

    private void OnLockCamera(GameEvent gameEvent)
    {
        if (isLock)
        {
            UnlockCamera();
        }
        else
        {
            LockCamera();
        }

    }

    private void LockTargetLost(GameObject lockTarget)
    {
        if(cameraLockTarget.TargetLost(lockTarget))
        {
            UnlockCamera();
        }
    }

    public void Restart()
    {
        cameraFollow.Restart();
        cameraLockTarget.Restart();
        UnlockCamera();
    }

    private void LockCamera()
    {
        if (isLock = cameraLockTarget.TryToLock())
        {
            vCameraFolow.Priority = 10;
            vCameraLockTarget.Priority = 20;
            GameEventManager.Instance.TriggerEvent(new TargetLockEvent());
        }
    }

    private void UnlockCamera()
    {
        isLock = false;
        vCameraFolow.Priority = 20;
        vCameraLockTarget.Priority = 10;
        cameraLockTarget.Unlock();
        GameEventManager.Instance.TriggerEvent(new TargetUnlockEvent());
    }

    private void OnBigfootKill(GameEvent gameEvent)
    {
        BigfootKillEvent bigfootKillEvent = (BigfootKillEvent)gameEvent;
        LockTargetLost(bigfootKillEvent.enemy.gameObject);
    }

    private void OnEnterEndState(GameEvent gameEvent)
    {  
        cameraFollow.SetEnable(false);
        cameraLockTarget.SetEnable(false);
        UnlockCamera();
    }
}

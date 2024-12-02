using UnityEngine;
using UnityEngine.Events;

public class ColliderEvents : MonoBehaviour
{
    [Header("Layer Settings")]
    public LayerMask requiredLayer; // Layer to check for

    [Header("Events")]
    public UnityEvent onTriggerEnterEvent; // Unity Event for trigger enter
    public UnityEvent onTriggerExitEvent; // Unity Event for trigger exit
    public UnityEvent onCollisionEnterEvent; // Unity Event for collision enter
    public UnityEvent onCollisionExitEvent; // Unity Event for collision exit

    // Called when another collider enters a trigger collider attached to this GameObject
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (IsInRequiredLayer(other.gameObject.layer))
        {
            onTriggerEnterEvent.Invoke();
            HandleOnTriggerEnter(other);
        }
    }

    // Called when another collider exits a trigger collider attached to this GameObject
    protected virtual void OnTriggerExit(Collider other)
    {
        if (IsInRequiredLayer(other.gameObject.layer))
        {
            onTriggerExitEvent.Invoke();
            HandleOnTriggerExit(other);
        }
    }

    // Called when another collider enters a non-trigger collider attached to this GameObject
    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (IsInRequiredLayer(collision.gameObject.layer))
        {
            onCollisionEnterEvent.Invoke();
            HandleOnCollisionEnter(collision);
        }
    }

    // Called when another collider exits a non-trigger collider attached to this GameObject
    protected virtual void OnCollisionExit(Collision collision)
    {
        if (IsInRequiredLayer(collision.gameObject.layer))
        {
            onCollisionExitEvent.Invoke();
            HandleOnCollisionExit(collision);
        }
    }

    // Helper function to check if the object's layer is within the required LayerMask
    private bool IsInRequiredLayer(int layer)
    {
        return (requiredLayer.value & (1 << layer)) != 0;
    }

    // Virtual methods for customization in child classes
    protected virtual void HandleOnTriggerEnter(Collider other) { }
    protected virtual void HandleOnTriggerExit(Collider other) { }
    protected virtual void HandleOnCollisionEnter(Collision collision) { }
    protected virtual void HandleOnCollisionExit(Collision collision) { }
}

using System.Collections.Generic;
using UnityEngine;

public enum ColliderState
{
    Closed,
    Open,
    Colliding,
    Parryable
}

public class HitboxController : MonoBehaviour
{
    public Vector3 hitBoxSize = Vector3.one * 0.25f;
    public LayerMask mask;
    public Color inactiveColor = new Color(255, 255, 255, 0.75f);
    public Color collisionOpenColor = new Color(44, 144, 53, 0.75f);
    public Color collidingColor = new Color(180, 28, 34, 0.75f);
    public Color parryableColor = Color.yellow;

    public bool alwaysVisible = false;
    public bool showWireFrame = false;

    private ColliderState _state = ColliderState.Closed;
    private IHitboxResponder _responder = null;

    private List<Collider> _collidersHit = new List<Collider>();

    #region Draw Gizmo

    private void OnDrawGizmosSelected()
    {
        if (alwaysVisible)
        {
            return;
        }

        DrawHitbox();
    }
    private void OnDrawGizmos()
    {
        if (!alwaysVisible)
        {
            return; 
        }

        DrawHitbox();
    }

    private void DrawHitbox()
    {
        Vector3 hboxSize = new Vector3(hitBoxSize.x * 2, hitBoxSize.y * 2, hitBoxSize.z * 2);
        CheckGizmoColor();
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
        Gizmos.DrawCube(Vector3.zero, hboxSize);

        if (showWireFrame)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(Vector3.zero, hboxSize);
        }
    }

    private void CheckGizmoColor()
    {
        switch (_state)
        {
            case ColliderState.Closed:
                Gizmos.color = inactiveColor;
                break;
            case ColliderState.Open:
                Gizmos.color = collisionOpenColor;
                break;
            case ColliderState.Parryable:
                Gizmos.color = parryableColor;
                break;
            case ColliderState.Colliding:
                Gizmos.color = collidingColor;
                break;
        }
    }
    #endregion

    //Called in an update method.
    public void Tick()
    {
        if (_state == ColliderState.Closed) { return; }
        Collider[] colliders = Physics.OverlapBox(transform.position, hitBoxSize, transform.rotation, mask);

        for (int i = 0; i < colliders.Length; i++)
        {
            Collider aCollider = colliders[i];
            if (_collidersHit.Contains(aCollider)) //Prevents the same collider to get hit more than once.
            {
                continue;
            }
            _responder?.CollisionedWith(aCollider);
            _collidersHit?.Add(aCollider);
        }

        _state = colliders.Length > 0 ? ColliderState.Colliding : ColliderState.Open;

    }

    public void StartCheckingCollision()
    {
        _state = ColliderState.Open;
    }

    public void StopCheckingCollision()
    {
        _state = ColliderState.Closed;
        _collidersHit.Clear();
    }

    public void StartParryCheck()
    {
        _state = ColliderState.Parryable;
    }

    public void SetResponder(IHitboxResponder responder)
    {
        _responder = responder;
    }
}

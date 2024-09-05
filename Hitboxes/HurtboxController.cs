using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtboxController : MonoBehaviour
{

    public Collider collider;
    public Color hurtBoxColor;
    private ColliderState _state = ColliderState.Open;

    private void OnDrawGizmos()
    {
        Gizmos.color = hurtBoxColor;
        //Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
        Gizmos.DrawCube(collider.bounds.center, new Vector3(collider.bounds.extents.x *2, collider.bounds.extents.y*2, collider.bounds.extents.z*2));
    }

    public bool getHit(int damage)
    {
        Debug.Log("Took Damage of " + damage);
        return true;
    }

}

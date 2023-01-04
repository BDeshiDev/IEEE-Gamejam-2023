using UnityEngine;

public class Bumper: MonoBehaviour
{
    public Vector3 size = Vector3.one;
    public LayerMask collisionLayerMask;
    private Collider[] unusedResults = new Collider[1];
    public bool checkCollision()
    {
        return Physics.OverlapBoxNonAlloc(transform.position, size * .5f, unusedResults, transform.rotation, collisionLayerMask) > 0;
    }
    private void OnDrawGizmosSelected()
    {
        
        Gizmos.matrix =  transform.localToWorldMatrix;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Vector3.zero, size);
    }
}
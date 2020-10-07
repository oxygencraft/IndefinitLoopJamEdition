using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class Pusher : MonoBehaviour
{
    // dirty hack because I don't feel like making this work properly at 2am
    public Transform objectTransform;

    public void Push()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 4f);
        Transform obj = hit.collider.transform;
        obj = objectTransform;
        Vector2 pos = obj.position;
        Vector2 direction = pos - (Vector2) transform.position.normalized;
        pos += direction * 2;
        // also another dirty hack because it's 2am
        pos = obj.position;
        pos.x += 2f;
        obj.position = pos;
    }
}

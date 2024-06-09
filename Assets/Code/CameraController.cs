using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float smoothing = 0.2f;
    [SerializeField] GameObject player;
    
    private void FixedUpdate()
    {
        Vector3 targetPos = new Vector3(player.transform.position.x, player.transform.position.y, this.transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
    }
}

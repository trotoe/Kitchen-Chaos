using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private void FixedUpdate()
    {
        if (Player.Instance != null)
        { 
            transform.position = Player.Instance.transform.position;
        }
    }
}

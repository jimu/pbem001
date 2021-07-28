using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommandAnimation : MonoBehaviour
{
    [SerializeField] Vector3 startPos;
    [SerializeField] Vector3 endPos;
    Animator animator;

    public float dist;

    private void Awake()
    {
        animator = GetComponent<Animator>();

    }

    public void Update()
    {
        if (!animator.GetBool("Moving"))
            Disable();
        else
            transform.position = Vector3.LerpUnclamped(startPos, endPos, dist);
    }

    private void Disable()
    {
        Debug.Log($"Moving Animation Disabled");
        transform.position = endPos;
        enabled = false;
    }

    public void Init(Vector3 start, Vector3 end)
    {
        Debug.Log($"Starting Moving Animation");
        animator = GetComponent<Animator>();
        dist = 0f;
        startPos = start;
        endPos = end;
        transform.position = startPos;
        enabled = true;
    }
}

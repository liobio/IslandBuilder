using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Belt : MonoBehaviour
{
    private static Vector3 right = Vector3.right;
    private static Vector3 left = Vector3.left;
    private static Vector3 up = Vector3.forward;
    private static Vector3 down = Vector3.back;

    private static int _beltID = 0;

    public float speed = 5f;
    public Belt beltInSequence;
    public BeltItem beltItem;
    public bool isSpaceTaken;



    private void Start()
    {

        beltInSequence = null;
        beltInSequence = FindNextBelt();
        gameObject.name = $"Belt: {_beltID++}";
    }

    private void Update()
    {
        if (beltInSequence == null)
            beltInSequence = FindNextBelt();

        if (beltItem != null && beltItem.item != null)
            StartCoroutine(StartBeltMove());
    }

    public Vector3 GetItemPosition()
    {
        var padding = 1.1f;
        var position = transform.position;
        var forward = transform.forward;
        if (forward.normalized == right)
        {
            return new Vector3(position.x + 1, position.y + padding, position.z - 1);
        }
        if (forward.normalized == up)
        {
            return new Vector3(position.x + 1, position.y + padding, position.z + 1);
        }
        if (forward.normalized == left)
        {
            return new Vector3(position.x - 1, position.y + padding, position.z + 1);
        }
        if (forward.normalized == down)
        {
            return new Vector3(position.x - 1, position.y + padding, position.z - 1);
        }
        return new Vector3(position.x, position.y + padding, position.z);
    }

    private IEnumerator StartBeltMove()
    {
        isSpaceTaken = true;

        if (beltItem.item != null && beltInSequence != null && beltInSequence.isSpaceTaken == false)
        {
            Vector3 toPosition = beltInSequence.GetItemPosition();

            beltInSequence.isSpaceTaken = true;

            var step = speed * Time.deltaTime;

            while (beltItem.item.transform.position != toPosition)
            {
                beltItem.item.transform.position =
                    Vector3.MoveTowards(beltItem.transform.position, toPosition, step);

                yield return null;
            }

            isSpaceTaken = false;
            beltInSequence.beltItem = beltItem;
            beltItem = null;
        }
    }

    private Belt FindNextBelt()
    {
        Vector3 origin = Vector3.zero;
        RaycastHit hit;

        var forward = transform.forward;
        if (forward.normalized == right)
        {
            origin = new Vector3(transform.position.x + 1, transform.position.y + 0.25f, transform.position.z - 1);
        }
        if (forward.normalized == up)
        {
            origin = new Vector3(transform.position.x + 1, transform.position.y + 0.25f, transform.position.z + 1);
        }
        if (forward.normalized == left)
        {
            origin = new Vector3(transform.position.x - 1, transform.position.y + 0.25f, transform.position.z + 1);
        }
        if (forward.normalized == down)
        {
            origin = new Vector3(transform.position.x - 1, transform.position.y + 0.25f, transform.position.z - 1);
        }

        Ray ray = new Ray(origin, forward);
        if (Physics.Raycast(ray, out hit, 2f))
        {
            Belt belt = hit.collider.GetComponent<Belt>();

            if (belt != null)
                return belt;
        }

        return null;
    }
}

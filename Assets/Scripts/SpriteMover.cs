using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteMover : MonoBehaviour {

    public bool isMovementActive = false;

    [Range(0.1f,10f)]
    public float speed;

    private Vector3 startingPosition;
    private Vector3 targetPosition;
    private Vector3 nextPosition;

    private void Start()
    {
        startingPosition = gameObject.transform.position;
        targetPosition = new Vector3(startingPosition.x + Random.Range(-2f, 2f), startingPosition.y + Random.Range(-2f, 2f),
            startingPosition.z + Random.Range(-2f, 2f));
        nextPosition = targetPosition;
        gameObject.GetComponent<Teleport>().canTeleport = true;
    }

    public void Recalibrate()
    {
        startingPosition = gameObject.transform.position;
        targetPosition = new Vector3(startingPosition.x + Random.Range(-2f, 2f), startingPosition.y + Random.Range(-2f, 2f),
            startingPosition.z + Random.Range(-2f, 2f));
        nextPosition = targetPosition;
        gameObject.GetComponent<Teleport>().canTeleport = true;
    }

    // Update is called once per frame
    void Update () {
        if (isMovementActive)
        {
            Move();
        }
        if (gameObject.GetComponent<Teleport>().gazedAt)
        {
            isMovementActive = false;
        }
        else
        {
            isMovementActive = true;
        }
	}

    private void Move()
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, nextPosition, speed * Time.deltaTime);

        if (gameObject.transform.position == startingPosition || gameObject.transform.position == targetPosition)
        {
            ChangeDestination();
        }
    }

    public void ChangeDestination()
    {
        nextPosition = (nextPosition != startingPosition ? startingPosition : targetPosition);
        gameObject.GetComponent<Teleport>().canTeleport = false;
    }
}

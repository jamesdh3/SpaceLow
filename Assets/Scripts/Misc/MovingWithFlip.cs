using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWithFlip : MonoBehaviour
{
	public Transform pos1, pos2;
	public float speed;
	public Transform startPos;

	Vector3 nextPos;

	// Start is called before the first frame update
	void Start()
	{
		nextPos = startPos.position;
	}

	// Update is called once per frame
	void Update()
	{
		if (transform.position == pos1.position)
		{
			flipObjectfromPos1();
			nextPos = pos2.position;
		}
		if (transform.position == pos2.position)
		{
			flipObjectfromPos2();
			nextPos = pos1.position;
		}

		transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
		
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawLine(pos1.position, pos2.position);
	}

	private void flipObjectfromPos1()
	{
		transform.Rotate(0, -180, 0);
	}

	private void flipObjectfromPos2()
    {
		transform.Rotate(0, 180, 0);
	}


}

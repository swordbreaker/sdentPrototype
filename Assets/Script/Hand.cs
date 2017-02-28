using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{

	public Vector3[] Rays = new Vector3[0];
	public float distance = 1f;
	public float GrabForce;
	public float BreackForce = 400f;

	private FixedJoint _join;
	private Rigidbody _grabbedRigidbody;
	private GameObject _grabbedObject;
	private float _breackForceSqr;
  

	private void Start ()
	{
		_join = GetComponent<FixedJoint>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.E))
		{
			if (_join.connectedBody != null)
			{
				_join.connectedBody = null;
			}
			else
			{
				foreach (var pos in Rays)
				{
					RaycastHit hit;
					if (Physics.Linecast(transform.position, transform.position + transform.TransformDirection(pos), out hit, LayerMask.GetMask("Grab")))
					{
						_join.connectedBody = hit.collider.attachedRigidbody;
						//_grabbedObject = hit.collider.gameObject;

					}
				}
			}    
		}

		if (_grabbedObject != null)
		{
			_grabbedObject.transform.position = transform.position;
		}

	}

	private void FixedUpdate()
	{
		if (_join.currentForce.sqrMagnitude < _breackForceSqr)
		{
			_join.connectedBody = null;
		}
		//if (_join.connectedBody != null)
		//{
		//	_grabbedRigidbody.velocity = (transform.position - _grabbedRigidbody.transform.position) * GrabForce;
		//}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;

		foreach (var pos in Rays)
		{
			
			Gizmos.DrawLine(transform.position, transform.position + transform.TransformDirection(pos.normalized * distance));
		}
	}


}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Assets.Script;
using Assets.Script.Helpers;
using UnityEngine;

public class GravityController : MonoBehaviour
{
	public Vector3[] _rays;
	public float MaxDistance;
	private Ray _groundRay = new Ray();
	private Ray _forwardRay;
	private Rigidbody _rigidbody;
	private Vector3 _playerGravity;
	private Vector3 _defaultGravity = new Vector3(0f,-9.81f,0f);
	private LerpHelper<Quaternion> _rotationHerper;
	private CharacterController _characterController;

	public bool UsesGravityManipultation { get; private set; }
	public Vector3 Normal { get; private set; }

	void Start () {
		_groundRay = new Ray(transform.position, Vector3.down);
		_playerGravity = _defaultGravity;
		_rigidbody = GetComponent<Rigidbody>();
	}

	private void FixedUpdate ()
	{
		//_rigidbody.AddForce(_playerGravity);
		//_groundRay.origin = transform.position;
		//_groundRay.direction = transform.TransformDirection(Vector3.down);
	 //   _forwardRay.origin = transform.position;
  //      _forwardRay.direction = transform.TransformDirection(new Vector3(0,-1,1));

	 //   RaycastHit hitDown;
	 //   RaycastHit hitForward;
  //      Vector3 forwardNormal = new Vector3();

		var normal = Vector3.zero;

		foreach (var r in GetRays())
		{
			RaycastHit hit;
			
			if (Physics.Raycast(r, out hit, MaxDistance, LayerMask.GetMask("GravityChanger")))
			{
				if (normal == Vector3.zero)
				{
					normal = hit.normal;
				}
				else
				{
					normal = Vector3.Lerp(normal, hit.normal, 0.5f);
				}
			}
		}

		if (normal != Vector3.zero)
		{
			Physics.gravity = -normal * 9.81f;
			UsesGravityManipultation = true;
			Normal = normal;
		}
		else
		{
			UsesGravityManipultation = false;
		}
	}

	private void OnDrawGizmos()
	{
		foreach (var r in GetRays())
		{
			Gizmos.color = Color.cyan;
			Gizmos.DrawRay(r);
		}
	}

	private IEnumerable<Ray> GetRays()
	{
		return _rays.Select(r => new Ray(transform.position, transform.TransformDirection(r)));
	}
}

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Assets.Script;
using Assets.Script.Helpers;
using UnityEngine;

public class GravityController : MonoBehaviour
{
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
		_groundRay.origin = transform.position;
		_groundRay.direction = transform.TransformDirection(Vector3.down);
	    _forwardRay.origin = transform.position;
        _forwardRay.direction = transform.TransformDirection(new Vector3(0,-1,1));


        Debug.DrawRay(_groundRay.origin, _groundRay.direction * 3, Color.blue);
        Debug.DrawRay(_forwardRay.origin, _forwardRay.direction * 3, Color.blue);
		Debug.DrawRay(transform.position, _playerGravity, Color.red);

	    RaycastHit hitDown;
	    RaycastHit hitForward;
        Vector3 forwardNormal = new Vector3();

        if (Physics.Raycast(_groundRay, out hitForward, MaxDistance, LayerMask.GetMask("GravityChanger")))
        {
            forwardNormal = hitForward.normal;
        }

        if (Physics.Raycast(_groundRay, out hitDown, MaxDistance, LayerMask.GetMask("GravityChanger")))
		{
			var normal = hitDown.normal;
            Physics.gravity = -normal * 9.81f;
			UsesGravityManipultation = true;

			Normal = Vector3.Lerp(normal, forwardNormal, 0.5f);
		}
        //else
        //{
        //    Normal = Vector3.up;
        //}
	}
}

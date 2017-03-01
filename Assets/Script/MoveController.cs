using DG.Tweening;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;


public class MoveController : MonoBehaviour
{
    public Transform FpsCamera;
    public float Speed;
    public float JumpForce;
    public float GroundCheckDistance = 0.1f;
    public MouseLook MouseLook = new MouseLook();
    private Vector3 _movementDirection;
    private Rigidbody _rigidbody;
    private bool _isGrounded;
    private GravityController _gravityController;
    private CapsuleCollider _capsuleCollider;
    private Vector3 _jumpForce;
    private bool _jump;
    private bool _isJumping;
    private bool _previouslyGrounde;

    

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _gravityController = GetComponent<GravityController>();
        MouseLook.Init(transform, FpsCamera);
    }

    private void Update()
    {
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxisRaw("Vertical");

        if (_isGrounded && Input.GetButtonDown("Jump"))
        {
            _jump = true;
        }

        var movementVector = FpsCamera.TransformDirection(new Vector3(x, 0f, y));
        movementVector = Vector3.ProjectOnPlane(movementVector, transform.up);
        _movementDirection = movementVector.normalized*Speed;
        RotateView();
    }

    private void FixedUpdate()
    {
        if (_jump && _isGrounded)
        {
            _rigidbody.drag = 0f;
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);
            var jumpVector = transform.TransformDirection(new Vector3(0f, JumpForce, 0f));
            _rigidbody.AddForce(jumpVector, ForceMode.Impulse);
            _jumpForce = Vector3.zero;
            _jump = false;
            _isJumping = true;
        }
        else
        {
            CheckIsGrounded();
            if (!_isJumping && _isGrounded)
            {
                _rigidbody.velocity = _movementDirection;
            }
        }
    }

    private void CheckIsGrounded()
    {
        _previouslyGrounde = _isGrounded;
        var ray = new Ray(transform.position, transform.TransformDirection(Vector3.down));
        if (Physics.SphereCast(ray, _capsuleCollider.radius,
            ((_capsuleCollider.height/2f) - _capsuleCollider.radius) + GroundCheckDistance,
            Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }

        if (!_previouslyGrounde && _isJumping && _isGrounded)
        {
            _isJumping = false;
        }
    }

    private void RotateView()
    {
        //avoids the mouse looking if the game is effectively paused
        if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

        // get the rotation before it's changed

        var mouseRotation = MouseLook.LookRotation(transform, FpsCamera);
        transform.localRotation = mouseRotation;

        if (_gravityController.UsesGravityManipultation)
        {
            var newRotation = Quaternion.FromToRotation(transform.up, _gravityController.Normal)*transform.rotation;
            transform.DORotate(newRotation.eulerAngles, Time.deltaTime*30f);
        }

        float oldYRotation = transform.eulerAngles.y;

        if (_isGrounded)
        {
            // Rotate the rigidbody velocity to match the new direction that the character is looking
            Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y - oldYRotation, Vector3.up);
            _rigidbody.velocity = velRotation*_rigidbody.velocity;
        }
    }
}
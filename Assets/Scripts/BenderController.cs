using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlanetaryCharacterController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 180f; // 度/秒
    public float jumpForce = 5f;
    public float gravity = 9.81f;

    public Transform earthTransform;
    public Transform groundCheck;
    public float groundCheckDistance = 0.5f;
    public LayerMask groundMask;

    public Transform modelTransform;   // 模型fbx

    private Rigidbody rb;
    private bool isGrounded;

    private Vector3 gravityDir;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.freezeRotation = true;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Debug.Log("Jump");
            rb.AddForce(-gravityDir * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        gravityDir = (earthTransform.position - transform.position).normalized;
        Vector3 upDir = -gravityDir;

        rb.AddForce(gravityDir * gravity, ForceMode.Acceleration);

        isGrounded = Physics.Raycast(groundCheck.position, gravityDir, groundCheckDistance, groundMask);

        

        if (modelTransform != null)
        {
            Quaternion targetRotation =
                Quaternion.FromToRotation(modelTransform.up, upDir) * modelTransform.rotation;
            modelTransform.rotation =
                Quaternion.Slerp(modelTransform.rotation, targetRotation, Time.fixedDeltaTime * 5f);
        }

            // 输入
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(h) > 0.01f)
        {
            modelTransform.Rotate(Vector3.up * h * rotationSpeed * Time.fixedDeltaTime);
        }

        if (Mathf.Abs(v) > 0.01f)
        {
            Vector3 moveDir = modelTransform.forward * v;
            moveDir = Vector3.ProjectOnPlane(moveDir, gravityDir).normalized;

            rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);
        }
    }
}

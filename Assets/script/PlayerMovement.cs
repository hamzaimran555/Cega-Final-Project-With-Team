using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Collider HitBox;
    public int Speed = 10;
    public int RunSpeed = 5;
    public int TargetFrameRate = 60;
    public float JumpForce = 100;
    public bool CanJump = false;

    private Animator playerAnimator;
    private bool Run = false;
    private Rigidbody playerRigidBody;

    private void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        playerRigidBody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            CanJump = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            CanJump = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            CanJump = true;
        }

        if (other.CompareTag("Enemy"))
        {
            Animator enemyAnimator = other.GetComponent<Animator>();

            if (enemyAnimator != null)
            {
                enemyAnimator.Play("TakeDamage");
            }
        }
    }

    void Update()
    {
        Application.targetFrameRate = TargetFrameRate;

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Run = Input.GetKey(KeyCode.LeftShift);

        playerAnimator.SetBool("Ground", CanJump);

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && CanJump)
        {
            playerRigidBody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            playerAnimator.SetTrigger("jump");
        }

        Vector3 direction = horizontal * cameraRight + vertical * cameraForward;

        playerAnimator.SetFloat("Speed", direction.magnitude);
        playerAnimator.SetBool("Run", Run);

        if (direction.magnitude > 0)
        {
            if (Run)
            {
                transform.position += direction * RunSpeed * Time.deltaTime;
            }
            else
            {
                transform.position += direction * Speed * Time.deltaTime;
            }

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(direction, Vector3.up),
                0.25f
            );
        }

        // Attack
        if (Input.GetMouseButtonDown(0))
        {
            playerAnimator.SetTrigger("Attack");
        }
    }
}
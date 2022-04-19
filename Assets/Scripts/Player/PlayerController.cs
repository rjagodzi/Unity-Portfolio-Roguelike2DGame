using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody2D playerRigidbody;

    [SerializeField] Transform weaponsArm;

    private Camera mainCamera;

    [SerializeField] int movementSpeed;

    private Vector2 movementInput;

    private Animator playerAnimator;

    private float currentMovementSpeed;

    private bool canDash;

    [SerializeField] float dashSpeed = 16f, dashLength = 0.5f, dashCooldown = 1f;

    [SerializeField] List<WeaponsSystem> availableGuns = new List<WeaponsSystem>();

    private int currentGun;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;

        playerAnimator = GetComponent<Animator>();
        currentMovementSpeed = movementSpeed;
        canDash = true;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMoving();
        PointingGunAtMouse();
        AnimatingPlayer();
        PlayerTorpedo();
        SwitchGun();
    }

    public void SwitchGun()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(availableGuns.Count > 0)
            {
                currentGun++;

                if(currentGun >= availableGuns.Count)
                {
                    currentGun = 0;
                }

                foreach(WeaponsSystem gun in availableGuns)
                {
                    gun.gameObject.SetActive(false);
                }

                availableGuns[currentGun].gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning("No guns available. Pick some up!");
            }
        }
    }

    public bool PlayerIsDashing()
    {
        if (currentMovementSpeed == dashSpeed)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void PlayerTorpedo()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            currentMovementSpeed = dashSpeed;
            canDash = false;

            playerAnimator.SetTrigger("Torpedo");

            StartCoroutine(DashCooldownCounter());
            StartCoroutine(DashLengthCounter());

        }
    }



    IEnumerator DashCooldownCounter()
    {
        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }

    IEnumerator DashLengthCounter()
    {
        yield return new WaitForSeconds(dashLength);

        currentMovementSpeed = movementSpeed;
    }

    private void AnimatingPlayer()
    {
        if (movementInput != Vector2.zero)
        {
            playerAnimator.SetBool("isMoving", true);
        }
        else
        {
            playerAnimator.SetBool("isMoving", false);

        }
    }

    private void PointingGunAtMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 screenPoint = mainCamera.WorldToScreenPoint(transform.localPosition);

        Vector2 offset = new Vector2(mousePosition.x - screenPoint.x, mousePosition.y - screenPoint.y);

        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

        weaponsArm.rotation = Quaternion.Euler(0, 0, angle);

        //if statement for turning the player and weaponArm sprites left or right depending on the mouse position

        if (mousePosition.x < screenPoint.x)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            weaponsArm.localScale = new Vector3(-1f, -1f, 1f);
        }
        else
        {
            transform.localScale = Vector3.one;
            weaponsArm.localScale = Vector3.one;
        }
    }

    private void PlayerMoving()
    {
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");

        //normalizing movement input ensures that the magnitude of diagonal movement is the same as vertical and horizontal movements

        movementInput.Normalize();

        //the method below is not sufficient - it causes jittering when coliding with walls
        //transform.position += new Vector3(movementInput.x, movementInput.y, 0f) * movementSpeed * Time.deltaTime;

        playerRigidbody.velocity = movementInput * currentMovementSpeed;
    }
}

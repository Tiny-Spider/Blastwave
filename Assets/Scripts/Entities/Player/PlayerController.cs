using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {
    private static Plane groundPlane = new Plane(Vector3.up, Vector3.up);

    private Player player;
    private InputDevice input;
    private Camera viewCamera;
    private Rigidbody myRigidbody;
    private Vector3 lookDirection;

	void Awake() {
		player = GetComponent<Player>();
        myRigidbody = GetComponent<Rigidbody>();
	}

    void Start() {
        viewCamera = Camera.main;
        input = player.GetUser().input;
    }

	void Update() {
		// Movement
        Vector3 moveInput = new Vector3(InputManager.GetAxis(input, InputAxis.Horizontal), 0, InputManager.GetAxis(input, InputAxis.Vertical));
        Vector3 moveVelocity = moveInput.normalized * player.moveSpeed;

		Move(moveVelocity);

		// Looking
        Ray ray;

        switch (input) {
            case InputDevice.Keyboard:
                ray = viewCamera.ScreenPointToRay(Input.mousePosition);
                float rayDistance;

                if (groundPlane.Raycast(ray, out rayDistance)) {
                    Vector3 point = ray.GetPoint(rayDistance);
                    point = -(transform.position - point);
                    //point.y = 0;
                    lookDirection = point.normalized;
                }
                break;
            default:
                Vector3 lookInput = new Vector3(InputManager.GetAxis(input, InputAxis.HorizontalLook), 0, InputManager.GetAxis(input, InputAxis.VerticalLook));
                lookDirection = lookInput.normalized;
                break;
        }

        RaycastHit hit;
        ray = new Ray(transform.position, lookDirection);

        if (Physics.Raycast(ray, out hit, player.weapons[0].range, player.targetMask, QueryTriggerInteraction.Collide)) {
            LookAt(hit.point);
            Aim(hit.point);
        }
        else {
            LookAt(transform.position + lookDirection);
            Aim();
        }

		// Shooting
        if (InputManager.GetButton(input, InputButton.TriggerRight, InputButton.TriggerLeft)) {
            foreach (Weapon weapon in player.weapons) {
                weapon.Attack(player);
            }
        }
	}

	public void Move(Vector3 velocity) {
		player.velocity = velocity;
	}

	public void LookAt(Vector3 target) {
		Vector3 lookAtPoint = new Vector3(target.x, transform.position.y, target.z);
		transform.LookAt(lookAtPoint);
	}

    public void Aim(Vector3 target) {
        foreach (Transform hand in player.hands) {
            if ((hand.transform.position - target).sqrMagnitude > 1) {
                hand.LookAt(target);
            }
        }
    }

    public void Aim() {
        foreach (Transform hand in player.hands) {
            hand.localRotation = Quaternion.identity;
        }
    }

	void FixedUpdate() {
		myRigidbody.MovePosition(myRigidbody.position + player.velocity * Time.fixedDeltaTime);
	}
}

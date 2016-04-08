using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : LivingEntity {
    public float stateUpdateSpeed = 0.2f;
    public float lookSmoothing = 5;

    public Animator animator;

    [Header("AI")]
    public Player target;
    public LayerMask targetMask;
    public float movementUpdateSpeed = 0.5f;
    public float searchInterval = 2f;

    [Header("Weapons")]
    public Transform[] hands;
    private Weapon[] weapons;
    private int weaponIndex = 0;

	public NavMeshAgent pathfinder { get; private set; }
    public Level level { get; private set; }

    private StateMachine<Enemy> stateMachine;

    public delegate void EnemyDeathEvent(Enemy enemy, IDamageSource source);
    public static event EnemyDeathEvent OnEnemyDeath = delegate { };

    void Awake() {
        weapons = new Weapon[hands.Length];
        stateMachine = new StateMachine<Enemy>(this, StateSearch.instance);

        level = FindObjectOfType<Level>();
        pathfinder = GetComponent<NavMeshAgent>();
        pathfinder.updateRotation = false;
	}

    public void Initalize(EnemyData data) {
        base.Initalize(data.GetBase());
    }

    void Start() {
        if (!animator) {
            Debug.LogWarning("[Enemy] No animator found!");
        }
    }

    void OnEnable() {
        if (animator) {
            animator.SetTrigger("reset");
        }

        dead = false;
        SwitchToState(StateSearch.instance);
    }

    void FixedUpdate() {
        stateMachine.Update();

        if (animator) {
            animator.SetFloat("speed", pathfinder.desiredVelocity.magnitude);
            animator.SetBool("move", pathfinder.desiredVelocity.magnitude > 0);
        }
    }

    public void Attack() {
        foreach (Weapon weapon in weapons) {
            weapon.Attack(this);
        }

        if (animator) {
            animator.SetTrigger("attack");
        }
    }

    public void GiveWeapon(Weapon weapon) {
        do {
            if (weaponIndex >= weapons.Length) {
                weaponIndex = 0;
                SpawnWeapon(weapon);
                break;
            }

            if (weapons[weaponIndex] == null) {
                SpawnWeapon(weapon);
                break;
            }
        }
        while (weapons[weaponIndex++] != null);
    }

    private void SpawnWeapon(Weapon weapon) {
        Weapon spawnedWeapon = Instantiate<Weapon>(weapon);
        Transform spawnedTransform = spawnedWeapon.transform;

        spawnedTransform.parent = hands[weaponIndex];
        spawnedTransform.localPosition = Vector3.zero;
        spawnedTransform.localRotation = Quaternion.identity;

        weapons[weaponIndex] = spawnedWeapon;
        spawnedWeapon.Initalize(targetMask);
    }

    public Weapon[] GetWeapons() {
        return weapons;
    }

    public void SwitchToState(State<Enemy> state) {
        stateMachine.ChangeState(state);
    }

    public void MoveTo(Vector3 position) {
        pathfinder.SetDestination(position);
    }

	public override void Die(IDamageSource source) {
        OnEnemyDeath(this, source);

        dead = true;
        gameObject.Recycle();
	}

    public void Dance() {
        animator.SetTrigger("victory");
    }

    void OnDrawGizmos() {
        if (debug && pathfinder) {
            NavMeshPath path = pathfinder.path;

            for (int i = 0; i < path.corners.Length - 1; i++) {
                Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
            }
        }
    }
}

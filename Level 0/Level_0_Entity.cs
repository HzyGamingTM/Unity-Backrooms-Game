using UnityEngine;
using UnityEngine.AI;

public class Level_0_Entity : MonoBehaviour {
    public Animator animator;
    public Transform player;
	public AudioSource audioSource;

	float growlCoolDown = 0f;
    NavMeshAgent agent;
    Vector3 direction;

	void Start() {
		agent = GetComponent<NavMeshAgent>();
		agent.destination = player.position;
	}

	void FixedUpdate() {
		growlCoolDown += Time.deltaTime;

        agent.destination = player.position;

        if (agent.velocity.magnitude > 0) {
			animator.SetBool("Walking", true);
		} else {
			animator.SetBool("Walking", false);
        }

        if (agent.remainingDistance < 100) {
			agent.speed = 2f;
			animator.speed = 1f;
		} else {
			animator.speed = 0.5f;
		}

		RaycastHit hit;
		direction = (player.position - transform.position).normalized;

		// If entity is within 20m distance and can see you, it will speed.
		if (agent.remainingDistance < 20 &&
			Physics.Raycast(transform.position, direction, out hit, 15) && 
			hit.collider.gameObject == player.gameObject &&
			growlCoolDown > 5
		) {
			growlCoolDown = 0f;
            agent.speed = 4.1f;
			audioSource.Play();
			Player.shared.glitchManager.glitch = true;
		}
    }
}

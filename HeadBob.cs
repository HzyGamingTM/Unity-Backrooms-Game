using UnityEngine;

public class HeadBob : MonoBehaviour {
	[SerializeField] bool _enabled = true;
	public static float amplitude;
	public static float frequency = 1;
	public Transform hands;

	float bobSpeed = 14f;
	float defaultPosY = 0.7f;
	float timer = 0f;

	float tiltAngle = 15f;
	public Transform HandPivot;
	public Transform peakOrigin;
	public Transform peakLeft;
	public Transform peakRight;

	private void Update() {
		// Bruh this took me 2hrs to figure out :skull:
		if (Input.GetKey(KeyCode.Q)) {
			transform.position = Vector3.Lerp(transform.position, peakLeft.position, 6.5f * Time.deltaTime);
            Player.shared.cameraRotZ = Mathf.Lerp(Player.shared.cameraRotZ, tiltAngle, 6.5f * Time.deltaTime);
		} else {
			transform.position = Vector3.Lerp(transform.position, peakOrigin.position, 6.5f * Time.deltaTime);
            Player.shared.cameraRotZ = Mathf.Lerp(Player.shared.cameraRotZ, 0, 6.5f * Time.deltaTime);
		}
		
		if (Input.GetKey(KeyCode.E)) {
			transform.position = Vector3.Lerp(transform.position, peakRight.position, 6.5f * Time.deltaTime);
            Player.shared.cameraRotZ = Mathf.Lerp(Player.shared.cameraRotZ, -tiltAngle, 6.5f * Time.deltaTime);
		} else {
			transform.position = Vector3.Lerp(transform.position, peakOrigin.position, 6.5f * Time.deltaTime);
            Player.shared.cameraRotZ = Mathf.Lerp(Player.shared.cameraRotZ, 0, 6.5f * Time.deltaTime);
		}

		if (!_enabled) return;
		hands.rotation = transform.rotation;

		Vector3 velocity = Player.velocity;
		Vector3 localPos = transform.localPosition;
		if (velocity.x != 0 && velocity.z != 0) {
			timer += Time.deltaTime * bobSpeed;
            transform.localPosition = new(
				localPos.x, amplitude * Mathf.Sin(timer * frequency) + defaultPosY, localPos.z
			);
		} else {	
			timer = 0;
			transform.localPosition = new(
				localPos.x, Mathf.Lerp(localPos.y, defaultPosY, bobSpeed * Time.deltaTime
				), localPos.z
			);
		}
	}
}
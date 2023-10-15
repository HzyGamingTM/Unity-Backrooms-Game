using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit_Door : MonoBehaviour {
    bool loading = false;

    void OnTriggerEnter(Collider other) {
        Debug.Log(other.gameObject.name);
        if (other.transform.gameObject == Player.shared.capsule) {
            Debug.Log("works");
            Player.shared.gameOverScreen.SetActive(true);
        }
    }

    private void FixedUpdate() {
        if (Player.shared.gameOverScreen.active == true && Input.GetKeyDown(KeyCode.E) && loading == false) {
            loading = true;
            Player.shared.loadingScreen.SetActive(true);
            Player.shared.gameOverScreen.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadSceneAsync("Map");
        }
    }
}
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
	public static MenuManager instance;
	public GameObject menu;
	public GameObject loadingScreen;
	public GameObject GUI;

	public TMP_Text playText;
	public TMP_Text settingsText;
	public TMP_Text quitText;

	public GameObject playerPrefab;
	public static Vector3 spawnLocation;

    private void Awake() {
        if (instance == null) {
			instance = this;
		}
    }

    public void StartGame() {
		spawnLocation = new(1.87f, 0, -1);
		Instantiate(playerPrefab, spawnLocation, Quaternion.identity);
		Destroy(Camera.main.gameObject);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ButtonPlay() {
        menu.SetActive(false);
        Camera.main.GetComponent<Animator>().Play("Camera");
        StartCoroutine(Transition());
    }

	IEnumerator Transition() {
		yield return new WaitForSeconds(3f);
		StartGame();
    }

	public void ButtonSettings() {

	}

	public void ButtonQuit() {
		Application.Quit();
	}

	public void UnHover() {
        playText.text = "Play";
        settingsText.text = "Settings";
        quitText.text = "Quit";

        playText.color = Color.white;
        settingsText.color = Color.white;
        quitText.color = Color.white;
    }
	public void Hover(int hoveredButton) {
		playText.text = "Play";
		settingsText.text = "Settings";
		quitText.text = "Quit";

		playText.color = Color.white;
		settingsText.color = Color.white;
		quitText.color = Color.white;

		switch (hoveredButton) {
			case 0:
				playText.text = "> Play <";
				playText.color = Color.yellow;
				break;
			case 1:
				settingsText.text = "> Settings <";
				settingsText.color = Color.yellow;
				break;
			case 2:
				quitText.text = "> Quit <";
				quitText.color = Color.yellow;
				break;
		}
	}

	public static bool allowTeleport = false;

    void FixedUpdate() {
		if (!allowTeleport) return;
		if (Player.shared.playerCamera.transform.position.y < -10) {
			ItemManager.Clear();
			loadingScreen.SetActive(true);
            SceneManager.LoadSceneAsync(1);
            allowTeleport = false;
        }
    }
}

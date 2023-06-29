using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class LevelManager : MonoBehaviour {
    public static LevelManager Instance;
    public PostProcessVolume volume;

    void Awake() {
        Instance = this;
    }

    public static void Blur() {
        DepthOfField d;
        Instance.volume.profile.TryGetSettings(out d);
        d.focusDistance.value = Mathf.Lerp(d.focusDistance.value, 0.1f, 6.5f * Time.deltaTime);
        d.focalLength.value = Mathf.Lerp(d.focalLength.value, 112f, 6f * Time.deltaTime);
    }

    public static void Unblur() {
        DepthOfField d;
        Instance.volume.profile.TryGetSettings(out d);
        d.focusDistance.value = Mathf.Lerp(d.focusDistance.value, 10, 3f * Time.deltaTime);
        d.focalLength.value = Mathf.Lerp(d.focalLength.value, 32, 6f * Time.deltaTime);
    }
}

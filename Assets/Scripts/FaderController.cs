using UnityEngine;

public class FaderController : MonoBehaviour
{
    public static FaderController Instance;
    [SerializeField] private OVRScreenFade fader;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void FadeIn() { Instance.fader.FadeIn(); }

    public static void FadeOut() { Instance.fader.FadeOut(); }

}

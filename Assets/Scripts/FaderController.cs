using UnityEngine;

public class FaderController : MonoBehaviour
{
    public static FaderController instance;
    [SerializeField] private OVRScreenFade fader;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void FadeIn() { instance.fader.FadeIn(); }

    public static void FadeOut() { instance.fader.FadeOut(); }

}

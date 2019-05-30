using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image foreground = null;

    public void SetProgress(float progress)
    {
        foreground.fillAmount = progress;
    }
}

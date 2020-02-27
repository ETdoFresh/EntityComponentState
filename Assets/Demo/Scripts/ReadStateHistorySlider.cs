using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ReadStateHistorySlider : MonoBehaviour
{
    public Slider slider;
    public ReadStateFromStateHistoryFile file1;
    public ReadDeltaStateFromStateHistoryFile file2;

    public int Count => file1 ? file1.count : file2 ? file2.count : 0;
    public int CountPosition => file1 ? file1.countPosition : file2 ? file2.tick : 0;
    public bool IsPlaying => file1 && file1.isPlaying || file2 && file2.isPlaying;

    private void OnValidate()
    {
        if (!slider) slider = GetComponent<Slider>();
        if (!file1) file1 = FindObjectOfType<ReadStateFromStateHistoryFile>();
        if (!file2) file2 = FindObjectOfType<ReadDeltaStateFromStateHistoryFile>();
    }

    private void Update()
    {
        slider.maxValue = Count;
        if (IsPlaying)
        {
            if (Count > 0)
            {
                slider.value = CountPosition;
            }
        }
        else
        {
            if (file1) file1.countPosition = Mathf.RoundToInt(slider.value);
            if (file2) file2.tick = Mathf.RoundToInt(slider.value);
        }
    }

    public void TogglePlaying()
    {
        if (CountPosition >= Count)
        {
            if (file1) file1.countPosition = 0;
            if (file2) file2.tick = 0;
        }
        else
        {
            if (file1) file1.isPlaying = !IsPlaying;
            if (file2) file2.isPlaying = !IsPlaying;
        }
    }
}

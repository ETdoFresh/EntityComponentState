using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ReadStateHistorySlider : MonoBehaviour
{
    public Slider slider;
    public ReadStateFromStateHistoryFile readStateFromStateHistoryFile;

    public bool IsPlaying => readStateFromStateHistoryFile.isPlaying;

    private void OnValidate()
    {
        if (!slider) slider = GetComponent<Slider>();
        if (!readStateFromStateHistoryFile) readStateFromStateHistoryFile = FindObjectOfType<ReadStateFromStateHistoryFile>();
    }

    private void Update()
    {
        if (IsPlaying)
        {
            if (readStateFromStateHistoryFile.count > 0)
            {
                slider.value = (float)readStateFromStateHistoryFile.countPosition / readStateFromStateHistoryFile.count;
            }
        }
        else
        {
            readStateFromStateHistoryFile.countPosition = Mathf.RoundToInt(slider.value * readStateFromStateHistoryFile.count);
        }
    }

    public void TogglePlaying()
    {
        if (readStateFromStateHistoryFile.countPosition >= readStateFromStateHistoryFile.count)
        {
            readStateFromStateHistoryFile.countPosition = 0;
        }
        else
        {
            readStateFromStateHistoryFile.isPlaying = !IsPlaying;
        }
    }
}

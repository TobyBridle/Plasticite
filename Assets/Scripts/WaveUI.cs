using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveUI : MonoBehaviour
{
    public TMPro.TextMeshProUGUI waveText;
    // Start is called before the first frame update
    void Start()
    {
         waveText.text = "Wave " + Mathf.Max(PlayerPrefs.GetInt("wave"), 1); 
    }
}

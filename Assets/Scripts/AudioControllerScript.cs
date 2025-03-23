using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioControllerScript : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audioMixer;
    private float masterVolume;
    private float ambientVolume;
    private float effectsVolume;
    private float musicVolume;

    private Slider masterSlider;
    private Slider ambientSlider;
    private Slider effectsSlider;
    private Slider musicSlider;

    void Start()
    {
        GetVolumes();

        Transform grp = transform.Find("Content/SoundVolumes/Layout");
        masterSlider = grp.Find("Master/Slider").GetComponent<Slider>();
        masterSlider.value = DbToVolume(masterVolume);
        ambientSlider = grp.Find("Ambient/Slider").GetComponent<Slider>();
        ambientSlider.value = DbToVolume(ambientVolume);
        effectsSlider = grp.Find("Effects/Slider").GetComponent<Slider>();
        effectsSlider.value = DbToVolume(effectsVolume);
        musicSlider = grp.Find("Music/Slider").GetComponent<Slider>();
        musicSlider.value = DbToVolume(musicVolume);
    }

    private void GetVolumes()
    {
        if (PlayerPrefs.HasKey(nameof(masterVolume)))
        {
            masterVolume = PlayerPrefs.GetFloat(nameof(masterVolume));
            audioMixer.SetFloat(nameof(masterVolume), masterVolume);
        }
        else if (!audioMixer.GetFloat(nameof(masterVolume), out masterVolume))
        {
            Debug.LogError("Error getting float: " + nameof(masterVolume));
        }

        if (PlayerPrefs.HasKey(nameof(ambientVolume)))
        {
            ambientVolume = PlayerPrefs.GetFloat(nameof(ambientVolume));
            audioMixer.SetFloat(nameof(ambientVolume), ambientVolume);
        }
        else if (!audioMixer.GetFloat(nameof(ambientVolume), out ambientVolume))
        {
            Debug.LogError("Error getting float: " + nameof(ambientVolume));
        }

        if (PlayerPrefs.HasKey(nameof(effectsVolume)))
        {
            effectsVolume = PlayerPrefs.GetFloat(nameof(effectsVolume));
            audioMixer.SetFloat(nameof(effectsVolume), effectsVolume);
        }
        else if (!audioMixer.GetFloat(nameof(effectsVolume), out effectsVolume))
        {
            Debug.LogError("Error getting float: " + nameof(effectsVolume));
        }

        if (PlayerPrefs.HasKey(nameof(musicVolume)))
        {
            musicVolume = PlayerPrefs.GetFloat(nameof(musicVolume));
            audioMixer.SetFloat(nameof(musicVolume), musicVolume);
        }
        else if (!audioMixer.GetFloat(nameof(musicVolume), out musicVolume))
        {
            Debug.LogError("Error getting float: " + nameof(musicVolume));
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Minus) 
            || Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            ChangeMasterVolume();
        }
        if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Plus) 
            || Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            ChangeMasterVolume(step: 1);
        }
    }

    private void ChangeMasterVolume(float step = -1)
    {
        if (audioMixer.GetFloat(nameof(masterVolume), out masterVolume))
        {
            masterVolume = Mathf.Clamp(
                masterVolume + step * (Mathf.Abs(masterVolume + 3.0f) * 0.25f + 3.0f),
                -80f, 20f);

            audioMixer.SetFloat(nameof(masterVolume), masterVolume);
        }
        else
        {
            Debug.Log("GetFloat Error");
        }
    }

    public void OnMasterSliderChange(float value)
    {
        masterVolume = VolumeToDb(value);
        audioMixer.SetFloat(nameof(masterVolume), masterVolume);
        PlayerPrefs.SetFloat(nameof(masterVolume), masterVolume);
    }

    public void OnAmbientSliderChange(float value)
    {
        ambientVolume = VolumeToDb(value);
        audioMixer.SetFloat(nameof(ambientVolume), ambientVolume);
        PlayerPrefs.SetFloat(nameof(ambientVolume), ambientVolume);
    }
    
    public void OnEffectsSliderChange(float value)
    {
        effectsVolume = VolumeToDb(value);
        audioMixer.SetFloat(nameof(effectsVolume), effectsVolume);
        PlayerPrefs.SetFloat(nameof(effectsVolume), effectsVolume);
    }
    public void OnMusicSliderChange(float value)
    {
        musicVolume = VolumeToDb(value);
        audioMixer.SetFloat(nameof(musicVolume), musicVolume);
        PlayerPrefs.SetFloat(nameof(musicVolume), musicVolume);
    }

    private void OnDestroy()
    {
        PlayerPrefs.Save();
    }

    private float DbToVolume(float db)
    {
        return Mathf.Pow((db + 80.0f) / 1.0e2f, 2.0f);
    }
    private float VolumeToDb(float volume)
    {
        // [0..0.5..1] -> [-80..-10..20]
        return -80.0f + 100.0f * Mathf.Sqrt(volume);
    }
}
/* dB дБ - децибел 
 * dB = 10 log( E2 / E1 ) -- логарифм відношення (частки) - альтернативна форма
 *  для коефіцієнту між двома величинами.
 * 
 * E1 ---> |Attenuator| ---> E2
 * 
 * E2 / E1          dB
 *  1000            30
 *  100             20
 *  10              10
 *  1 (E2==E1)      0
 *  0.1            -10
 *  0.01           -20
 *  0.001          -30
 */

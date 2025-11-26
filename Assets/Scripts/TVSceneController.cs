using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class TVSceneController : MonoBehaviour
{
    public CameraZoomToTV cameraZoom;
    public TextMeshProUGUI channelText;
    public Image staticOverlay;
    public float channelDisplayTime = 0.5f;

    void Start()
    {
        var gm = SportsBlitzGameManager.Instance;

     

        cameraZoom.ZoomOut();

        if (CheckRunFinished())
            return;

        StartCoroutine(TVSequence());
    }


    bool CheckRunFinished()
    {
        var gm = SportsBlitzGameManager.Instance;

        if (gm.HasClearedRun() || gm.HasFailedRun())
        {
            HighScoreUI ui = FindObjectOfType<HighScoreUI>(true);

            if (ui != null)
                ui.Show(gm.gamesWon, gm.HasClearedRun());

            return true;
        }

        return false;
    }


    IEnumerator TVSequence()
    {
        yield return new WaitForSeconds(1f);

        ChannelFlicker();
        yield return new WaitForSeconds(channelDisplayTime);

        cameraZoom.StartZoom();
        yield return new WaitForSeconds(cameraZoom.delayBeforeSwitch);

        SportsBlitzGameManager.Instance.StartNextGame();
    }

    void ChannelFlicker()
    {
        int ch = Random.Range(1, 30);
        channelText.text = "CH " + ch.ToString("00");

        staticOverlay.gameObject.SetActive(true);
        channelText.gameObject.SetActive(true);

        Invoke(nameof(HideFlicker), channelDisplayTime);
    }

    void HideFlicker()
    {
        staticOverlay.gameObject.SetActive(false);
        channelText.gameObject.SetActive(false);
    }
}



using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


public class Fading : MonoBehaviour
{

    public Texture2D ProgressBar;
    public Texture2D ProgressBarBackground;
    public Texture2D fadeOutTexture;        // Textur über den Screen legen
    public float fadeSpeed = 0.8f;          // Fade In Geschwindigkeit

    private int drawDepth = -1000;      // Hirarchie in der Anzeige, je tiefer desto früher wird es angezeigt
    private float alpha = 1.0f;         // Alpha Wert
    private int fadeDir = -1;           // Minuswert = Fade In, Pluswert = Fade Out;
    private AsyncOperation Async;

    void OnGUI()
    {

        // Konvertiere die Operation in einen Sekundenwert
        alpha += fadeDir * fadeSpeed * Time.fixedDeltaTime;

        // erzwinge einen Wert zwischen 0 und 1, da der ALphawert zwischen 0 und 1 liegen muss
        alpha = Mathf.Clamp01(alpha);

        // Farbe festlegen
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);                // setze Alphawert
        GUI.depth = drawDepth;                                                              // Texture über dem Screen laden
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);       // Textur über die ganze Grösse des Screens legen

        if (Async != null)
        {

            // Grösse der Progress Bar
            int Width = Screen.width / 3;
            int Heigth = 60;

            // Auf dem Screen zentrieren
            int X = (Screen.width / 2) - (Width / 2);
            int Y = (Screen.height / 2) - (Heigth / 2);

            // Zeichne auf dem Screen
            GUI.depth = drawDepth;                                                              // Textur im Vodergrund rendern
            GUI.DrawTexture(new Rect(X, Y, Width, Heigth), ProgressBarBackground);          // Zeichne den Background der Progr.Bar
            GUI.DrawTexture(new Rect(X, Y, Width * Async.progress, Heigth), ProgressBar);       // Zeichne die Progr.Bar	

            GUIStyle gs = new GUIStyle();
            gs.fontSize = 40;
            gs.alignment = TextAnchor.MiddleCenter;

            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUI.Label(new Rect(X, Y, Width, Heigth), string.Format("{0:N0}%", Async.progress * 100), gs);
        }
    }


    public float BeginFade(int direction)
    {

        fadeDir = direction;
        return fadeSpeed;       // fadeSpeed wiedergeben, um das Timing zu erleichtern
    }


    // Wird aufgerufen, wenn die Szene geladen wird
    void OnLevelWasLoaded()
    {


        BeginFade(-1);          
    }


    /**
	 * Lade Szene mit Fade Out Effekt
	 * 
	 * @param string SceneName
	 * @param float WaitFor = 0.6f
	 * @return void
	 */
    public void LoadScene(string SceneName, float WaitFor = 0.6f)
    {

        StartCoroutine(ChangeScene(SceneName, WaitFor));
    }


    /**
	 * Lade die Szene asynchron
	 * 
	 * @param string SceneName
	 * @param float WaitFor = 0.6f
	 * @return void
	 */
    public void LoadSceneAsync(string SceneName, float WaitFor = 0.6f)
    {

        StartCoroutine(ChangeSceneAsync(SceneName, WaitFor));
    }


    /**
	 * Ändere die Szene
	 * 
	 * @param string SceneName
	 * @param float WaitFor = 0.6f
	 * @return void
	 */
    IEnumerator ChangeScene(string SceneName, float WaitFor = 0.6f)
    {

        // Warte auf die Animation
        yield return StartCoroutine(WaitForRealSeconds(WaitFor));

        // Fade Out und neue Szene laden
        float fadeTime = BeginFade(1);
        yield return StartCoroutine(WaitForRealSeconds(fadeTime));

        SceneManager.LoadScene(SceneName);
    }


    /**
	 * Verändere die Szene asynchron
	 * 
	 * @param string SceneName
	 * @param float WaitFor = 0.6f
	 * @return void
	 */
    IEnumerator ChangeSceneAsync(string SceneName, float WaitFor = 0.6f)
    {

        // auf Animation warten
        yield return StartCoroutine(WaitForRealSeconds(WaitFor));

        // Fade Out und lade neue Szene
        BeginFade(1);

        Async = SceneManager.LoadSceneAsync(SceneName);
        yield return Async;
    }


    /**
	 * Diese Funktion bricht nicht, wenn die Zeit auf 0 steht
	 */
    public static IEnumerator WaitForRealSeconds(float time)
    {

        float start = Time.realtimeSinceStartup;

        while (Time.realtimeSinceStartup < start + time)
        {

            yield return null;
        }
    }
}
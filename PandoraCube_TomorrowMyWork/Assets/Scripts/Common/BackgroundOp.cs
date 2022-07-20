using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundOp : MonoBehaviour
{
    public int IllustNum;
    Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();

        StartCoroutine(FadeIn(2f));
    }

    public void callFadeOut()
    {
        Camera.main.backgroundColor = Color.black;
        StartCoroutine(FadeOut(2f));
    }

    public IEnumerator FadeOut(float time)
    {
        Color color = image.color;
        while (color.a > 0f)
        {
            color.a -= Time.deltaTime / time;
            image.color = color;
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);


        SceneChanger.Instance.LoadScene(IllustNum + 2);
    }

    public IEnumerator FadeIn(float time)
    {
        Color color = image.color;
        while (color.a < 1f)
        {
            color.a += Time.deltaTime / time;
            image.color = color;
            yield return null;
        }

    }
}

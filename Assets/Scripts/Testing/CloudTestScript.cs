using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CloudTestScript - A basic pixelated 2d weather/sky simulation system
/// Uses a bunch of variables to draw a faux sky
/// </summary>

public class CloudTestScript : MonoBehaviour
{
    public float time;
    float windAngle;
    Weather weather;
    public SpriteRenderer spriteR;
    public Camera camera;

    Texture2D texture;
    Sprite sprite;

    public Color night;
    public Color day;
    public Color setrise;

    public Color skyColor;
    Color cloudColor;

    Color[,] colorArray;
    Color[] colorText;
    Color[] colors = new Color[3];


    // Start is called before the first frame update
    void Start()
    {
        if (spriteR == null)
        {
            spriteR = gameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
        }

        //texture = new Texture2D(camera.pixelWidth, camera.pixelHeight, TextureFormat.RGBA32, true);

        texture = Instantiate(spriteR.sprite.texture) as Texture2D;
        //colorText = new Color[(camera.pixelWidth * camera.pixelHeight)];
        //sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f,texture.width, texture.height), new Vector2(0.5f, 0.05f), 100.0f);
        colorText = new Color[texture.width * texture.height];
        colorArray = new Color [texture.width, texture.height];
        spriteR.sprite = sprite;

        
        colors[0] = Color.red;
        colors[1] = Color.green;
        colors[2] = Color.blue;
        int mipCount = Mathf.Min(3, texture.mipmapCount);


    }

    // Update is called once per frame
    void Update()
    {

        if (time < 8)
        {
            skyColor = Color.Lerp(skyColor, night, 0.01f);
        }
        else if (time >= 8 && time < 18)
        {
            skyColor = Color.Lerp(skyColor, day, 0.01f);
        }
        else if (time >= 18)
        {
            skyColor = Color.Lerp(skyColor, setrise, 0.01f);

        }
        if (time >= 24) 
        {
            time = 0;
        }

        time += Time.deltaTime;

        

        drawSky();
        
    }

    void drawSky() 
    {
        int mipCount = Mathf.Min(3, texture.mipmapCount);

        for (int mip = 0; mip < mipCount; ++mip)
        {
            Color[] cols = texture.GetPixels(mip);
            for (int i = 0; i < cols.Length; ++i)
            {
                cols[i] = skyColor;
            }
            texture.SetPixels(cols, mip);
        }

        texture.SetPixels(colorText,1);
        texture.Apply(false);
    }

}

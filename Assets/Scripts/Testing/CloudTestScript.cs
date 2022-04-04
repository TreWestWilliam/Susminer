using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CloudTestScript - A basic pixelated 2d weather/sky simulation system
/// Uses a bunch of variables to draw a faux sky
/// </summary>

public class CloudTestScript : MonoBehaviour
{
    public float time;

    public float windAngle;
    public float maximumWindSpeed = 3;
    public float windSpeedX = 0;
    public float windSpeedY = 0;

    Weather weather;
    public SpriteRenderer spriteR;
    public Camera cam;

    Texture2D texture;
    Sprite sprite;

    public Color night;
    public Color day;
    public Color setrise;

    public Color azure;
    public Color skyBlue;
    public Color cobolt;


    public Color skyColor;
    public Color bottomSky;

    public int bottomInt;
    public int topInt;

    public Color starColor = Color.white;

    public int cloudMin;
    public int cloudMax;
    public int cloudCount;
    ArrayList clouds;
    public Color cloudColor;
    public Texture2D cloudTexture;
    Sprite cloudSprite;

    //These variables exist to fuck with things
    public int g = 0;
    public int w = 0;
    public int a = 0;
    public bool gfreeze;

    star[] stars;
    public int starCount = 25;

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

        texture = new Texture2D(cam.pixelWidth, cam.pixelHeight, TextureFormat.RGBA32, false);
        //texture = new Texture2D(320, 180, TextureFormat.RGBA32, false);
        //texture = Instantiate(spriteR.sprite.texture) as Texture2D;
        //colorText = new Color[(cam.pixelWidth * cam.pixelHeight)];
        sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.05f), 100.0f);
        colorText = new Color[texture.width * texture.height];
        colorArray = new Color[texture.width, texture.height];
        spriteR.sprite = sprite;

        randWindAngle();
        windMovement();


        stars = new star[starCount];
        clouds = new ArrayList();

        cloudSprite = Sprite.Create(cloudTexture, new Rect(0.0f, 0.0f, cloudTexture.width, cloudTexture.height), new Vector2(0.5f, 0.05f), 1000.0f);

        cloudCount = UnityEngine.Random.Range(cloudMin, cloudMax);

        colors[0] = Color.red;
        colors[1] = Color.green;
        colors[2] = Color.blue;
        int mipCount = Mathf.Min(3, texture.mipmapCount);

        generateStars();
        generateClouds();


        /*
        drawSky();

        Color[] rendered = texture.GetPixels(0);

        for (int i = 0; i < rendered.Length; i++) 
        {
            Debug.Log($"i:{i}: {rendered[i]}");
        }*/


    }

    // Update is called once per frame
    void Update()
    {


        timeSkyLerp();
        moveClouds();

        if (time >= 24)
        {
            time = 0;
        }

        time += Time.deltaTime;

        drawSky();
    }

    void timeSkyLerp() //Game Time Sky Color Linear Interpolation
    {
        if (time < 8)
        {
            skyColor = Color.Lerp(skyColor, night, 0.01f);
            bottomSky = Color.Lerp(bottomSky, skyBlue, 0.01f);
        }
        else if (time >= 8 && time < 18)
        {
            skyColor = Color.Lerp(skyColor, day, 0.01f);
            bottomSky = Color.Lerp(bottomSky, azure, 0.01f);
        }
        else if (time >= 18)
        {
            skyColor = Color.Lerp(skyColor, night, 0.01f);
            bottomSky = Color.Lerp(bottomSky, setrise, 0.01f);
        }
    }


    void OnPreRender()
    {

    }

    void drawSky()
    {
        //int mipCount = Mathf.Min(3, texture.mipmapCount);
        /*
        for (int i = 0; i < colorText.Length; i++) 
        {
            colorText[i] = skyColor;
        }*/

        if (!gfreeze) { g++; }
        /*
        int iterator = 0;
        for (int x = 0; x < colorArray.GetUpperBound(1); x++)
        {
            for (int y = 0; y < colorArray.GetUpperBound(0); y++)
            {
                colorText[iterator] = colorArray[y,x];
                iterator++;
            }
        }*/

        colorArray = skyGradient(texture.width, texture.height, bottomInt, topInt, skyColor, bottomSky);
        colorArray = drawStars(colorArray);

        if (weather == Weather.lightRain) 
        { 
            //TODO:Rain
        }

        //This code converts a two dimensional array into a single dimensional array
        //You may think a nested for loop would be better for this, but you are wrong.
        //And by you, I mean me.

        for (int i = 0; i < (colorArray.GetUpperBound(0) * colorArray.GetUpperBound(1)); i++)
        {
            int y = (int)(i / texture.width);
            int x = i - (y * texture.width);
            colorText[i] = colorArray[x, y];
        }

        texture.SetPixels(colorText, 0);

        texture.Apply();


    }

    void generateStars()
    {
        for (int i = 0; i < starCount; i++)
        {
            stars[i] = new star();
            stars[i].x = (int)UnityEngine.Random.Range(5, colorArray.GetUpperBound(0));
            stars[i].y = (int)UnityEngine.Random.Range(colorArray.GetUpperBound(1) / 2, colorArray.GetUpperBound(1));
            stars[i].size = (int)UnityEngine.Random.Range(1, 8);
        }
    }

    void moveClouds() 
    {
        foreach (Cloud c in clouds) 
        {
            c.x += windSpeedX;
            c.y += windSpeedY;

            //c.thisObject.transform.Translate((c.thisObject.transform.position - new Vector3(c.x,c.y,1)) * Time.deltaTime);
            c.thisObject.transform.position = new Vector3(c.x, c.y, 1);
        }
    }

    void generateClouds() 
    {
        for (int i = 0; i < cloudCount; i++)
        {
            Cloud cloud = new Cloud();

            cloud.x = UnityEngine.Random.Range(5, colorArray.GetUpperBound(0)/100);
            cloud.y = UnityEngine.Random.Range((colorArray.GetUpperBound(1)/100) / 2, colorArray.GetUpperBound(1)/100);
            cloud.size = UnityEngine.Random.Range(1, 8) ;

            GameObject g = new GameObject($"Cloud {i}");
            g.transform.position = new Vector3(cloud.x, cloud.y,1);
            g.transform.localScale = new Vector3(cloud.size, cloud.size, 1);
            g.transform.SetParent(this.transform);

            cloud.thisObject = g;

            SpriteRenderer sr = g.AddComponent<SpriteRenderer>();
            sr.sprite = cloudSprite;

            clouds.Add(cloud);
        }
    }

    Color[,] drawStars(Color[,] image)
    {

        for (int i = 0; i < stars.Length; i++)
        {
            for (int x = 0; x < stars[i].size; x++)
            {
                for (int y = 0; y < stars[i].size; y++)
                {
                    //Debug.Log($"Star x:{stars[i].x} Star Y:{stars[i].y} Star Size{stars[i].size}");
                    image[(stars[i].x - stars[i].size) + x, (stars[i].y - stars[i].size) + y] = starColor;
                }
            }

        }


        return image;
    }

    Color[,] drawStars(Color[,] image, float interpolate)
    {

        for (int i = 0; i < stars.Length; i++)
        {
            for (int x = 0; x < stars[i].size; x++)
            {
                for (int y = 0; y < stars[i].size; y++)
                {
                    //Debug.Log($"Star x:{stars[i].x} Star Y:{stars[i].y} Star Size{stars[i].size}");
                    image[(stars[i].x - stars[i].size) + x, (stars[i].y - stars[i].size) + y] = Color.Lerp(starColor, image[(stars[i].x - stars[i].size) + x, (stars[i].y - stars[i].size) + y], interpolate);
                }
            }

        }


        return image;
    }

    Color[,] skyGradient(int width, int height, Color topColor, Color bottomColor)
    {
        Color[,] myGradient = new Color[width, height];
        float changePercent = 1 / height;
        //Debug.Log($"{width}, {height}");

        for (int y = 0; y < height; y++)
        {
            Color thisLayer = Color.Lerp(bottomColor, topColor, ((float)(y) / height));

            for (int x = 0; x < width; x++)
            {
                myGradient[x, y] = thisLayer;
            }
            //Debug.Log($"Color Y{y}={thisLayer} cause { ((float) y/height)}");
        }
        return myGradient;

    }


    Color[,] skyGradient(int width, int height, int start, int end, Color topColor, Color bottomColor)
    {
        Color[,] myGradient = new Color[width, height];
        float changePercent = 1 / (height - (start + end));
        //Debug.Log($"{width}, {height}");
        Color thisLayer;
        if (start > end)
        {
            int swap = start;
            start = end;
            end = swap;
        }

        for (int y = 0; y < height; y++)
        {
            if (y < start)
            {
                thisLayer = bottomColor;
            }
            else if (y > end)
            {
                thisLayer = topColor;
            }
            else
            {
                thisLayer = Color.Lerp(bottomColor, topColor, ((float)(y - start) / (end - start)));
            }


            for (int x = 0; x < width; x++)
            {
                myGradient[x, y] = thisLayer;
            }
            //Debug.Log($"Color Y{y}={thisLayer} cause { ((float) y/height)}");
        }
        return myGradient;

    }

    float randWindAngle() 
    {
        return UnityEngine.Random.Range(0, 360);
    }

    float[] windMovement() 
    {
        float[] movement = new float[2];

        if (windAngle > 0 && windAngle < 180)
        {
            windSpeedY = maximumWindSpeed;
        }
        else 
        {
            windSpeedY = -maximumWindSpeed;
        }

        if (windAngle > 90 && windAngle < 270)
        {
            windSpeedX = -maximumWindSpeed;
        }
        else 
        {
            windSpeedX = maximumWindSpeed;
            windSpeedX = maximumWindSpeed;
        }

        return movement;
    }
}

struct star
{
    public int x;
    public int y;
    public int size;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TemporaryInfoPanel : MonoBehaviour
{
    public Player player;
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = $"HP:{player.getHP()}/{player.getMaxHP()}";
    }
}

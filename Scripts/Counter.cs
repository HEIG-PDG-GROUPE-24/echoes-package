using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public TextMeshProUGUI countDisplay;
    
    private int _count = 0;

    public void on_increment_press()
    {
        ++_count;
        UpdateValue();
    }

    public void on_decrement_press()
    {
        --_count;
        UpdateValue();
    }

    public void UpdateValue()
    {
        countDisplay.text = _count.ToString();
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

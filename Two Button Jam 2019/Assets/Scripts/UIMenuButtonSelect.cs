using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuButtonSelect : MonoBehaviour
{
    [SerializeField]
    private Button[] Buttons = null;

    private int _selectedButtonIndex = 0;

    private void Start()
    {
        Buttons[_selectedButtonIndex].Select();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Button1"))
        {
            _selectedButtonIndex = (_selectedButtonIndex + 1) % Buttons.Length;

            Buttons[_selectedButtonIndex].Select();
        }

        if (Input.GetButtonDown("Button2"))
        {
            Buttons[_selectedButtonIndex].onClick.Invoke();
        }
    }
}

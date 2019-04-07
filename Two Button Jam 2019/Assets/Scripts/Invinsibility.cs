using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invinsibility : MonoBehaviour
{
    [SerializeField]
    private int InvinsibilityLayer = 0;

    [SerializeField]
    private GameObject Graphics = null;

    [SerializeField]
    private float Duration = 1f;

    private float _timeLeft = 0f;

    private int _originalLayer;

    private float _flashDuration = 0.1f;
    private float _flashTimer = 0f;

    private void Update()
    {
        if (_timeLeft > 0)
        {
            _timeLeft -= Time.deltaTime;

            if (_timeLeft <= 0)
            {
                _timeLeft = 0;
                Graphics.SetActive(true);

                gameObject.layer = _originalLayer;
            }
        }
    }

    private void FixedUpdate()
    {
        if (_timeLeft > 0)
        {
            _flashTimer += Time.deltaTime;

            if (_flashTimer > _flashDuration)
            {
                _flashTimer = 0;
                Graphics.SetActive(!Graphics.activeSelf);
            }
        }
    }

    public void MakeInvinsible()
    {
        _timeLeft = Duration;

        _originalLayer = gameObject.layer;

        gameObject.layer = InvinsibilityLayer;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class BackgroundMovement : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    private float Speed = 1.2f;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        Vector2 offset = meshRenderer.material.mainTextureOffset;
        offset.y += Time.deltaTime * Speed;
        meshRenderer.material.mainTextureOffset = offset;
    }
}

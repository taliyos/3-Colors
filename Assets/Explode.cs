﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    [Header("Burst Size")]
    public GameObject splatter;
    public int size;
    public int points;
    public int smoothing;


    private Rigidbody2D rb;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D other) {
        Burst(other);
        RemoveFromScene();
    }

    private void Burst(Collision2D col) {
        GameObject splat = Instantiate(splatter);
        splat.transform.position = new Vector2(transform.position.x, transform.position.y);
        splat.transform.eulerAngles = new Vector3(0,0,Random.Range(0,360));
        Color color;
        ColorUtility.TryParseHtmlString(ColorScheme.primaryColors[(int) ColorSelector.GetColor()], out color);
        splat.GetComponent<SpriteRenderer>().color = color;
        RemoveFromScene();
    }

    private RaycastHit2D FindRelevantObject(RaycastHit2D[] hits) {
        foreach (RaycastHit2D hit in hits) {
            if (Physics2D.GetIgnoreLayerCollision(LayerMask.NameToLayer("Player Attack"), hit.transform.gameObject.layer)) continue;
            return hit; // Assuming first valid object is closest
        }

        return default(RaycastHit2D);

    }

    private void RemoveFromScene() {

    }

}
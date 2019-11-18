﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    public float minDistance;
    public float maxDistance;
    public float maxForce;
    public float gapForceMultiplier;

    public float gapHoverTime;
    private float time;
    private GapHoverState gapHover;
    private bool gapDetected;

    private Rigidbody2D rb;

    private float relativeGroundY;
    private float lastRelativeGroundY;


    private Movement movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<Movement>();
    }

    void FixedUpdate()
    {
        GetGroundY();
        AddHoverForce();
        if (!gapDetected && gapHover == GapHoverState.ENDED) gapHover = GapHoverState.NONE;
    }

    private void GetGroundY()
    {
        RaycastHit2D rayHit = Physics2D.Raycast(transform.position, Vector2.down);
        if (rayHit == new RaycastHit2D())
        {
            gapDetected = true;
            if (gapHover != GapHoverState.ENDED) gapHover = GapHoverState.HOVER;
            return;
        }
        float groundY = rayHit.transform.position.y + rayHit.transform.localScale.y / 2;
        relativeGroundY = transform.position.y - transform.localScale.y / 2 - groundY;
        lastRelativeGroundY = relativeGroundY;
    }


    private void AddHoverForce()
    {
        float yPosition = relativeGroundY;
        float hoverForce = maxForce;
        if (gapHover == GapHoverState.HOVER)
        {
            if (gapHoverTime <= time) {
                time = 0;
                gapHover = GapHoverState.ENDED;
                return;
            }
            yPosition = lastRelativeGroundY;
            time++;
            hoverForce *= gapForceMultiplier;
        }
        if (gapHover == GapHoverState.ENDED) return;
        if (movement.GetInput().y != 0 && movement.GetJump()) return;
        float force = (maxDistance - yPosition) / (maxDistance - minDistance);
        if (force < 0) return;
        force *= hoverForce;
        rb.AddForce(Vector2.up * force);
    }

    public GapHoverState GetGapStatus() {
        return gapHover;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : AttackEntity
{
    public bool IsSelected { get; set; }
    private bool mouseUpSelect = false;
    private bool uiDelay = false;
    private int uiDelayCounter = 0;

    protected LineRenderer rangeLine;

    [Header("Setup")]
    public GameObject rangeIndicator;
    public Material rangeMaterial;
    public GameObject optionCanvasPrefab;
    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();
        unit.InitStructure();
        rangeLine = rangeIndicator.DrawCircle(unit.range / BuildManager.instance.GetTurretToBuild().scale, 0.1f, rangeMaterial);
        rangeLine.enabled = false;
        optionCanvasPrefab.SetActive(false);
    }

    // Update is called once per frame
    protected void Update()
    {
        if (uiDelay)
        {
            uiDelayCounter--;
        }
        if (uiDelayCounter <= 0 && uiDelay)
        {
            optionCanvasPrefab.SetActive(false);
            uiDelay = false;
        }
        if (mouseUpSelect && IsSelected && Input.GetMouseButtonDown(0))
        {
            IsSelected = false;
            mouseUpSelect = false;
            rangeLine.enabled = false;
            uiDelay = true;
            uiDelayCounter = 10;
        }


        if (IsSelected)
        {
            if (BuildManager.instance.IsPlacing)
            {
                IsSelected = false;
                mouseUpSelect = false;
                rangeLine.enabled = false;
                optionCanvasPrefab.SetActive(false);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                mouseUpSelect = true;
            }
            DisplayStats();
        }
    }

    void DisplayStats()
    {
        if (!rangeLine.enabled)
        {
            rangeLine.enabled = true;
            optionCanvasPrefab.SetActive(true);
        }
    }

    public override void Kill()
    {
        StatManager.Gold += unit.baseValue;
        StatManager.DefenseUnit -= unit.defenseUnit;
        Destroy(gameObject);
    }
}

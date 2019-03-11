using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Build : MonoBehaviour
{
    public GameObject rangeIndicator;
    public Material rangeMaterial;
    private bool hasIndicator;
    private LineRenderer rangeLine;
    // Update is called once per frame
    private void Update()
    {
        if (!BuildManager.instance.IsPlacing && Input.GetMouseButtonDown(0))
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = Vector2.Lerp(transform.position, point, 1f);

            if (EventSystem.current.IsPointerOverGameObject())
            {
                PointerEventData pointer = new PointerEventData(EventSystem.current)
                {
                    position = Input.mousePosition
                };

                List<RaycastResult> raycastResults = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointer, raycastResults);

                if (raycastResults.Count > 0)
                {
                    foreach (var go in raycastResults)
                    {
                        if (go.gameObject.tag == "Shop")
                        {
                            return;
                        }
                    }
                }
            }

            point.z = Camera.main.transform.position.z;
            RaycastHit2D hit = Physics2D.Raycast(point, Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.tag == "Structure")
                {
                    Structure structure = hit.collider.gameObject.GetComponent<Structure>();
                    if (structure.PlayerOwned)
                    {
                        structure.IsSelected = true;
                    }
                }
            }
            
        }
        if (BuildManager.instance.IsPlacing)
        {
            if (Input.GetMouseButtonDown(1))
            {
                BuildManager.instance.DisablePreview();
                RemoveIndicator();
                return;
            }
            AddIndicator();
            Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = Vector2.Lerp(transform.position, point, 1f);

            if (EventSystem.current.IsPointerOverGameObject())
            {
                PointerEventData pointer = new PointerEventData(EventSystem.current)
                {
                    position = Input.mousePosition
                };

                List<RaycastResult> raycastResults = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointer, raycastResults);

                if (raycastResults.Count > 0)
                {
                    foreach (var go in raycastResults)
                    {
                        if (go.gameObject.tag == "Shop")
                        {
                            return;
                        }
                    }
                }
            }
            //GameObject turretToBuild = BuildManager.instance.GetTurretToBuild();

            point.z = Camera.main.transform.position.z;
            RaycastHit2D hit = Physics2D.Raycast(point, Vector2.zero);
            bool canBuild = true;
            if (point.x >= BuildManager.instance.barrier.position.x)
            {
                canBuild = false;
            }
            if (hit.collider != null)
            {
                
                if (hit.collider.tag == "TileBase")
                {
                    point.Set(point.x, point.y, 0);
                    RaycastHit2D[] hitBox = Physics2D.BoxCastAll(point, new Vector2(1, 1), 0, Vector2.zero);

                    if (!(hitBox.Length < 2))
                        canBuild = false;
                }
                else
                {
                    canBuild = false;
                }
            }
            else
            {
                canBuild = false;
            }
            BuildManager.instance.ColorPreview(canBuild);
        }

        if (Input.GetMouseButtonDown(0) && BuildManager.instance.IsPlacing)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                PointerEventData pointer = new PointerEventData(EventSystem.current)
                {
                    position = Input.mousePosition
                };

                List<RaycastResult> raycastResults = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointer, raycastResults);

                if (raycastResults.Count > 0)
                {
                    foreach (var go in raycastResults)
                    {
                        if (go.gameObject.tag == "Shop")
                        {
                            return;
                        }
                    }
                }
            }
            Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //GameObject turretToBuild = BuildManager.instance.GetTurretToBuild();

            point.z = Camera.main.transform.position.z;
            RaycastHit2D hit = Physics2D.Raycast(point, Vector2.zero);

            bool canBuild = true;
            if (point.x >= BuildManager.instance.barrier.position.x)
            {
                canBuild = false;
            }
            else if (hit.collider != null)
            {
                
                if (hit.collider.tag == "TileBase")
                {
                    point.Set(point.x, point.y, 0);
                    RaycastHit2D[] hitBox = Physics2D.BoxCastAll(point, new Vector2(1, 1), 0, Vector2.zero);

                    if (hitBox.Length < 2)
                    {
                        BuildManager.instance.BuildStructure(point);
                        RemoveIndicator();
                    }
                    else
                        canBuild = false;
                } else
                {
                    canBuild = false;
                }
            } else
            {
                canBuild = false;
            }
            if (!canBuild)
            {
                Debug.Log("Can't build there");
                BuildManager.instance.shopText.text = "Can't build there";
            }
        }
    }

    void AddIndicator()
    {
        if (!hasIndicator)
        {
            if (rangeLine == null)
            {
                rangeLine = rangeIndicator.DrawCircle(BuildManager.instance.GetTurretToBuild().range / BuildManager.instance.GetTurretToBuild().scale, 0.1f, rangeMaterial);
            } else
            {
                rangeLine = rangeIndicator.DrawCircle(BuildManager.instance.GetTurretToBuild().range / BuildManager.instance.GetTurretToBuild().scale, 0.1f, rangeMaterial, rangeLine);
                rangeLine.enabled = true;
            }
        }
    }

    void RemoveIndicator()
    {
        hasIndicator = false;
        rangeLine.enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Thanks to https://samirgeorgy.medium.com/reflecting-laser-light-in-unity-6d8aa1a4daa2
 */

public class LaserBeam
{
    #region Private Variables
    public GameObject laserObject;
    private LineRenderer _laser;
    private List<Vector3> _laserIndices = new List<Vector3>();
    #endregion

    public LaserBeam(Vector3 position, Vector3 direction, float startWidth, float endWidth, Color startColor, Color endColor, Material material)
    {
        _laser = new LineRenderer();
        laserObject = new GameObject();
        laserObject.name = "Laser Beam";

        _laser = this.laserObject.AddComponent(typeof(LineRenderer)) as LineRenderer;
        _laser.startWidth = startWidth;
        _laser.endWidth = endWidth;
        _laser.startColor = startColor;
        _laser.endColor = endColor;
        _laser.material = material;

        CastLaser(position, direction);
    }

    private void CastLaser(position, direction)
    {
        _laserIndices.Add(position);

        Ray ray = new Ray(position, direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, 1))
        {
            CheckHit(hit, direction);
        }
        else
        {
            _laserIndices.Add(ray.GetPoint(199));
            UpdateLaser();
        }
    }

    private void CheckHit(RaycastHit hit, Vector3 direction)
    {
        if (hit.collider.tag.Equals("Mirror"))
        {
            Vector3 pos = hit.point;
            Vector3 dir = Vector3.Reflect(direction, hit.normal);

            CastLaser(pos, dir);
        } else
        {
            _laserIndices.Add(hit.point);
            UpdateLaser();
        }
    }

    private void UpdateLaser()
    {
        int count = 0;
        _laser.positionCount = _laserIndices.Count;
        
        foreach (Vector 3 index in _laserIndices) {
            _laser.SetPosition(count, index);
            count++;
        }
    }
}

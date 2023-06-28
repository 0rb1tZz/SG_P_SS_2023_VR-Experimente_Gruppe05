using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LaserBeam
{

    GameObject laserObject;
    LineRenderer laser;
    List<Vector3> laserIndices = new List<Vector3>();
    GameObject parentObject;
    float laserWavenlength;

    public LaserBeam(GameObject parentObject, Material material, float wavelength)
    {
        this.parentObject = parentObject;
        this.laserWavenlength = wavelength;

        this.laser = new LineRenderer();
        this.laserObject = new GameObject();
        this.laserObject.name = "Laser Beam";

        this.laser = this.laserObject.AddComponent(typeof(LineRenderer)) as LineRenderer;
        this.laser.startWidth = 0.1f;
        this.laser.endWidth = 0.1f;
        this.laser.material = material;
        this.laser.startColor = this.RgbFromWavelength(wavelength);
        this.laser.endColor = this.RgbFromWavelength(wavelength);

        CastRay(this.parentObject.transform.position, this.parentObject.transform.forward);
    }

    void CastRay(Vector3 pos, Vector3 dir)
    {
        laserIndices.Add(pos);

        Ray ray = new Ray(pos, dir);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit, 100))
        {
            CheckHit(hit, dir);
        }
        else
        {
            laserIndices.Add(ray.GetPoint(200));
            UpdateLineRenderer();
        }
    }

    void CheckHit(RaycastHit hitInfo, Vector3 dir)
    {
        string tag = hitInfo.collider.tag;
        if (tag == "Mirror")
        {
            Vector3 position = hitInfo.point;
            Vector3 direction = Vector3.Reflect(dir, hitInfo.normal);

            CastRay(position, direction);
        }
        else if (tag == "Prism")
        {
            HandlePrism(hitInfo, dir);
        }
        else if (tag == "LaserDetector")
        {
            LaserDetector detector = hitInfo.collider.gameObject.GetComponent<LaserDetector>();
            if (detector != null && detector.acceptedWavelength == this.laserWavenlength)
            {
                hitInfo.transform.SendMessage("HitByLaser");
            }

            laserIndices.Add(hitInfo.point);
            UpdateLineRenderer();
        }
        else
        {
            laserIndices.Add(hitInfo.point);
            UpdateLineRenderer();
        }
    }

    void UpdateLineRenderer()
    {
        int count = 0;
        laser.positionCount = laserIndices.Count;

        foreach (Vector3 vector in laserIndices)
        {
            laser.SetPosition(count, vector);
            count++;
        }
    }

    public void UpdateLaser()
    {
        this.laserIndices.Clear();
        CastRay(this.parentObject.transform.position, this.parentObject.transform.forward);
    }

    /*
     * https://stackoverflow.com/questions/1472514/convert-light-frequency-to-rgb
     */
    Color RgbFromWavelength(float wavelength)
    {

        float red, green, blue;
        float factor;

        if (wavelength >= 380 && wavelength < 440)
        {
            red = -(wavelength - 440) / (440 - 380);
            green = 0f;
            blue = 0f;
        }
        else if (wavelength >= 440 && wavelength < 490)
        {
            red = 0f;
            green = (wavelength - 440) / (490 - 440);
            blue = 1f;
        }
        else if (wavelength >= 490 && wavelength < 510)
        {
            red = 0f;
            green = 1f;
            blue = -(wavelength - 510) / (510 - 490);
        }
        else if (wavelength >= 510 && wavelength < 580)
        {
            red = (wavelength - 510) / (580 - 510);
            green = 1f;
            blue = 0f;
        }
        else if (wavelength >= 580 && wavelength < 645)
        {
            red = 1f;
            green = -(wavelength - 645) / (645 - 580);
            blue = 0f;
        }
        else if (wavelength >= 645 && wavelength < 781)
        {
            red = 1f;
            green = 0f;
            blue = 0f;
        }
        else
        {
            red = 0f;
            green = 0f;
            blue = 0f;
        }

        if (wavelength >= 380 && wavelength < 420)
        {
            factor = 0.3f + 0.7f * (wavelength - 380f) / (420f - 380f);
        }
        else if (wavelength >= 420 && wavelength < 701)
        {
            factor = 1f;
        }
        else if (wavelength >= 701 && wavelength < 781)
        {
            factor = 0.3f + 0.7f * (780 - wavelength) / (780 - 700);
        }
        else
        {
            factor = 0f;
        }

        float realRed = red == 0f ? 0f : Mathf.Pow(red * factor, 0.8f);
        float realGreen = green == 0f ? 0f : Mathf.Pow(green * factor, 0.8f);
        float realBlue = blue == 0f ? 0f : Mathf.Pow(blue * factor, 0.8f);

        return new Color(realRed, realGreen, realBlue, 1f);
    }

    void HandlePrism(RaycastHit hitInfo, Vector3 dir)
    {
        float n1 = 1.000293f;
        float n2 = 1.52f;

        laserIndices.Add(hitInfo.point);

        Vector3 refractedRay = RefractRay(n1, n2, hitInfo.normal, dir);
        Ray ray = new Ray(hitInfo.point, refractedRay);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit, 100) && hit.collider.gameObject.tag == "Prism")
        {
            laserIndices.Add(hit.point);
            Vector3 refractedRay2 = RefractRay(n2, n1, hit.normal, refractedRay);
            CastRay(hit.point + refractedRay2 * 0.1f, refractedRay2);
        } else
        {
            Debug.DrawRay(hitInfo.point, refractedRay);
        }
    }

    Vector3 RefractRay(float n1, float n2, Vector3 norm, Vector3 dir)
    {
        dir.Normalize();

        Vector3 refractedVector = (n1 / n2 * Vector3.Cross(norm, Vector3.Cross(-norm, dir)) - norm * Mathf.Sqrt(1 - Vector3.Dot(Vector3.Cross(norm, dir) * (n1 / n2 * n1 / n2), Vector3.Cross(norm, dir)))).normalized;

        return refractedVector;
    }

}

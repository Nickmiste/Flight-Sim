using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour
{
    private static TurretManager instance;

    public enum TurretLayout {TOP, LEFT_RIGHT, LEFT_RIGHT_TOP}
    [SerializeField] private TurretLayout layout = TurretLayout.LEFT_RIGHT;

    [SerializeField] private GameObject turretPrefab = null;

    public static TurretData[] turretDataArray = new TurretData[10];

    void Start()
    {
        instance = this;

        for (int i = 0; i < turretDataArray.Length; i++)
            turretDataArray[i] = new TurretData();

        //turretDataArray[0].firePaths.Add(TurretData.FirePath.SIN_VERT);
        //turretDataArray[0].firePaths.Add(TurretData.FirePath.COS_HOR);
        //turretDataArray[0].firePaths.Add(TurretData.FirePath.GUIDED);

        if (layout == TurretLayout.TOP)
        {
            AddTurret(0, 0.0f, 3.50f, 0.0f);
        }
        else if (layout == TurretLayout.LEFT_RIGHT)
        {
            AddTurret(0,  2.5f, 1.62f, 0.0f);
            AddTurret(0, -2.5f, 1.62f, 0.0f);
        }
        else if (layout == TurretLayout.LEFT_RIGHT_TOP)
        {
            AddTurret(0,  2.5f, 1.62f, 0.0f);
            AddTurret(0, -2.5f, 1.62f, 0.0f);
            AddTurret(1,  0.0f, 3.50f, 0.0f);
        }
    }

    private void AddTurret(int dataId, float x, float y, float z)
    {
        GameObject turret = Instantiate(turretPrefab) as GameObject;
        turret.transform.parent = this.transform;
        turret.transform.localPosition = new Vector3(x, y, z);
        turret.GetComponent<Turret>().dataId = dataId;
    }
}

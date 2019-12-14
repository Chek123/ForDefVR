using UnityEngine;

#if USE_STEAMVR
using Valve.VR;
#endif

public static class PlayAreaRealSize 
{

    //Optimalna velkost plochy vo VR v metroch (3x3 metre)
    private static Vector3 optimalPlayAreaSize = new Vector3(3f, 0f, 3f);

    public static Vector3 GetScaleFactor()
    {
#if USE_STEAMVR
        var rect = new HmdQuad_t();
        SteamVR_PlayArea.GetBounds(SteamVR_PlayArea.Size.Calibrated, ref rect);

        float countx = Mathf.Abs(convertValveVector(rect.vCorners0).x)
            + Mathf.Abs(convertValveVector(rect.vCorners1).x)
            + Mathf.Abs(convertValveVector(rect.vCorners2).x)
            + Mathf.Abs(convertValveVector(rect.vCorners3).x);

        float countz = Mathf.Abs(convertValveVector(rect.vCorners0).z)
            + Mathf.Abs(convertValveVector(rect.vCorners1).z)
            + Mathf.Abs(convertValveVector(rect.vCorners2).z)
            + Mathf.Abs(convertValveVector(rect.vCorners3).z);
        Debug.Log("sum: x=" + (countx / 2) / optimalPlayAreaSize.x + ", y=" + (countz / 2) / optimalPlayAreaSize.z);

        return new Vector3((countx / 2) / optimalPlayAreaSize.x, 1f, (countz / 2) / optimalPlayAreaSize.z);
#else
        return Vector3.one;
#endif
    }

    //helpers
#if USE_STEAMVR

    private static Vector3 convertValveVector(HmdVector3_t vector)
    {
        return new Vector3(vector.v0, vector.v1, vector.v2);
    }

#endif

}

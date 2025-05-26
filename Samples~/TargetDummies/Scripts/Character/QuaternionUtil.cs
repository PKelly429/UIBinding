using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public static class QuaternionUtil
{
    public static Quaternion SmoothDamp(Quaternion rot, Quaternion target, ref Quaternion deriv, float time)
    {
        return SmoothDamp(rot, target, ref deriv, time, Time.deltaTime);
    }

    public static Quaternion SmoothDamp(Quaternion rot, Quaternion target, ref Quaternion deriv, float time, float deltaTime)
    {
        if (deltaTime < 0.0001f) return rot;
        // account for double-cover
        var Dot = Quaternion.Dot(rot, target);
        var Multi = Dot > 0f ? 1f : -1f;
        target.x *= Multi;
        target.y *= Multi;
        target.z *= Multi;
        target.w *= Multi;
        // smooth damp (nlerp approx)
        var Result = new Vector4(
            Mathf.SmoothDamp(rot.x, target.x, ref deriv.x, time),
            Mathf.SmoothDamp(rot.y, target.y, ref deriv.y, time),
            Mathf.SmoothDamp(rot.z, target.z, ref deriv.z, time),
            Mathf.SmoothDamp(rot.w, target.w, ref deriv.w, time)
        ).normalized;
        // compute deriv
        var dtInv = 1f / deltaTime;
        deriv.x = (Result.x - rot.x) * dtInv;
        deriv.y = (Result.y - rot.y) * dtInv;
        deriv.z = (Result.z - rot.z) * dtInv;
        deriv.w = (Result.w - rot.w) * dtInv;
        return new Quaternion(Result.x, Result.y, Result.z, Result.w);
    }


    //roll  = Mathf.Atan2(2*y* w - 2*x* z, 1 - 2*y* y - 2*z* z);
    //pitch = Mathf.Atan2(2*x* w - 2*y* z, 1 - 2*x* x - 2*z* z);
    //yaw   =  Mathf.Asin(2*x* y + 2*z* w);
    /// <summary>
    /// Get pitch from a quaternion in radians
    /// </summary>
    /// <param name="quaternion"></param>
    /// <returns></returns>
    public static float GetPitch(Quaternion quaternion)
    {
        return Mathf.Atan2(2 * quaternion.x * quaternion.w - 2 * quaternion.y * quaternion.z, 1 - 2 * quaternion.x * quaternion.x - 2 * quaternion.z * quaternion.z);
    }
    /// <summary>
    /// Get yaw from a quaternion in radians
    /// </summary>
    /// <param name="quaternion"></param>
    /// <returns></returns>
    public static float GetYaw(Quaternion quaternion)
    {
        return Mathf.Atan2(2 * quaternion.y * quaternion.w - 2 * quaternion.x * quaternion.z, 1 - 2 * quaternion.y * quaternion.y - 2 * quaternion.z * quaternion.z);
    }

    /// <summary>
    /// Get roll from a quaternion in radians
    /// </summary>
    /// <param name="quaternion"></param>
    /// <returns></returns>
    public static float GetRoll(Quaternion quaternion)
    {
        float roll = Mathf.Asin(Mathf.Clamp(2 * quaternion.x * quaternion.y + 2 * quaternion.z * quaternion.w, -1, 1));
        if (float.IsNaN(roll)) return Mathf.PI/2;
        return roll;
    }

}

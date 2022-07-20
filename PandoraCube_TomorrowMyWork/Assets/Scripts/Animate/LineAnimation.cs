using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class LineAnimation
{
    public static float Lerp(float from, float to, float playTime, float duration, float smoothStart = 0, float smoothEnd = 0)
    {
        return Mathf.Lerp(from, to, LerpEff(playTime, duration, smoothStart, smoothEnd));
    }

    public static Vector3 Lerp(Vector3 from, Vector3 to, float playTime, float duration, float smoothStart = 0, float smoothEnd = 0)
    {
        return Vector3.Lerp(from, to, LerpEff(playTime, duration, smoothStart, smoothEnd));
    }

    private static float LerpEff(float playTime, float duration, float smoothStart = 0, float smoothEnd = 0)
    {
        float cl = duration * 2 - smoothStart - smoothEnd;
        float Result;

        if (playTime < smoothStart)
        {
            Result = (playTime / cl) -
                (smoothStart * Mathf.Sin(Mathf.PI * playTime / smoothStart) / (cl * Mathf.PI));
        } // 부드러운 시작
        else if (playTime <= duration - smoothEnd)
        {
            Result = (playTime * 2 - smoothStart) / cl;
        } // 일반 이동
        else if (playTime < duration)
        {
            Result =
                (playTime - duration + smoothEnd) / cl +
                smoothEnd * Mathf.Sin((playTime - duration + smoothEnd) * Mathf.PI / smoothEnd) / (cl * Mathf.PI) +
                (cl - smoothEnd) / cl;
        } // 부드러운 종료
        else
        {
            Result = 1;
        } // 종료

        return Result;
    }
}
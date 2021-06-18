using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PRD 
{
    /// <summary>
    /// 精度
    /// </summary>
    static double epsipon = 0.00000000000001d;


    /// <summary>
    /// 是否能中奖
    /// </summary>
    /// <param name="times">次数</param>
    /// <param name="p">预期概率</param>
    /// <returns></returns>
    public static bool Winning(int times,float p)
    {
        bool value = false;

        double c = GetC(p);

        double value1 = times * c;

        double value2 = UnityEngine.Random.Range(1, 10000) / 10000d;

        if (value2<=value1)
        {
            value = true;
        }

        return value;
    }


    /// <summary>
    /// 最大多少次能必中奖
    /// </summary>
    /// <param name="p">预期概率</param>
    /// <returns></returns>
    public static int GetMaxWin(float p)
    {
        double c = GetC(p);
        int n = 1;
        while (true)
        {
            if (n*c>=1)
            {
                return n;
            }
            n++;
        }
    }

    /// <summary>
    /// p 为预期中奖概率  如 0.1 就是10%
    /// </summary>
    /// <param name="p">预期概率</param>
    /// <returns></returns>
    public static double GetC(double p)
    {
        double dUp = p;
        double dLow = 0d;
 
  
        double dMid = 0d;
        double dPLast = 1d;
        while (true)
        {
             dMid = (dUp + dLow) / 2d;
            double dPtested = C2P(dMid);
            if (Math.Abs(dPtested-dPLast)<=epsipon)
            {
                break;
            }
            if (dPtested > p)
            {
                dUp = dMid;

            }
            else
            {
                dLow = dMid;
            }
            dPLast = dPtested;

        }
        return dMid;
    }



    private static double C2P(double c)
    {
        double dCurP = 0d;
        double dPreSuccessP = 0d;
        double dPE = 0;  
        int nMaxFail = (int)Math.Ceiling(1d / c);

        for (int i = 1  ; i <= nMaxFail; ++i)
        {
            dCurP = Math.Min(1d, c * i)*(1 - dPreSuccessP);
            dPreSuccessP += dCurP;
            dPE += i*dCurP;
        }

        return 1/dPE;
    }






}

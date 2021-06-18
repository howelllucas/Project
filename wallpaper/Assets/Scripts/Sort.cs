using System;

/// <summary>
/// 排序算法类。
/// </summary>
public class Sort<T> where T : IComparable
{
    #region 基础公有方法
    /// <summary>
    /// 数组快速排序。
    /// </summary>
    /// <param name="array">待排序数组。</param>
    /// <param name="low">排序起点。</param>
    /// <param name="high">排序终点。</param>
    public void QuickSort(T[] array, int low, int high)
    {
        if (low >= high)
            return;
        int first = low;
        int last = high;
        T key = array[low];
        while (first < last)
        {
            while (first < last && CompareGeneric(array[last], key) >= 0)
                last--;
            array[first] = array[last];
            while (first < last && CompareGeneric(array[first], key) <= 0)
                first++;
            array[last] = array[first];
        }
        array[first] = key;
        QuickSort(array, low, first - 1);
        QuickSort(array, first + 1, high);
    }
    #endregion

    #region 静态私有方法
    /// <summary>
    /// 泛型对象比较大小。
    /// </summary>
    /// <param name="t1">待比较对象。</param>
    /// <param name="t2">待比较对象。</param>
    /// <returns>大于0则前者的值更大，小于0则反之，等于0则二者的值相等。</returns>
    private static int CompareGeneric(T t1, T t2)
    {
        if (t1.CompareTo(t2) > 0)
            return 1;
        else if (t1.CompareTo(t2) == 0)
            return 0;
        else
            return -1;
    }
    #endregion
}


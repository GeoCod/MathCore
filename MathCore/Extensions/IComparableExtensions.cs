namespace System
{
    /// <summary>����� �������-���������� ��� ��������� ��������</summary>
    public static class IComparableExtensions
    {
        /// <summary>����� ������ �������� � ������������� ��������� ���������� ��������</summary>
        /// <typeparam name="T">��� ��������� ���������</typeparam>
        /// <param name="Collection">������ ���������, ������������� �� �����������</param>
        /// <param name="From">��������� ������ ������</param>
        /// <param name="To">�������� ������ ������</param>
        /// <param name="Item">������� �������</param>
        /// <returns>������ �������� � �������</returns>
        public static int? Search<T>(this T[] Collection, int From, int To, T Item) where T : IComparable<T>
        {
            while(To >= From)
            {
                var middle = (From + To) / 2;

                var comp = Collection[middle].CompareTo(Item);
                if(comp > 0)
                    To = middle - 1;
                else if(comp < 0)
                    From = middle + 1;
                else
                    return middle;
            }
            return null;
        }

        public static T Max<T>(this T x, T y) where T : IComparable<T> => x.CompareTo(y) >= 0 ? x : y;
        public static T Min<T>(this T x, T y) where T : IComparable<T> => x.CompareTo(y) <= 0 ? x : y;

        public static bool IsGreather<T>(this T a, T b) where T : IComparable<T> => a.CompareTo(b) > 0;
        public static bool IsLess<T>(this T a, T b) where T : IComparable<T> => a.CompareTo(b) < 0;
        public static bool IsGreatherEqual<T>(this T a, T b) where T : IComparable<T> => a.CompareTo(b) >= 0;
        public static bool IsLessEqual<T>(this T a, T b) where T : IComparable<T> => a.CompareTo(b) <= 0;

        public static T Between<T>(this T x, T a, T b) where T : IComparable<T>
        {
            var min = a.Min(b);
            var max = b.Max(a);
            return x.Max(min).Min(max);
        }
    }
}
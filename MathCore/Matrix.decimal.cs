﻿using System;
using System.Collections.Generic;
using DST = System.Diagnostics.DebuggerStepThroughAttribute;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
// ReSharper disable UnusedMember.Global

namespace MathCore
{
    /// <summary>Матрица NxM</summary>
    /// <remarks>
    /// i (первый индекс) - номер строки, 
    /// j (второй индекс) - номер столбца
    /// ------------ j ---------->
    /// | a11 a12 a13 a14 a15 a16 a1M
    /// | a21........................
    /// | a31........................
    /// | a41.......aij..............
    /// i a51........................
    /// | a61........................
    /// | aN1.....................aNM
    /// \/
    /// </remarks>
    [Serializable]
    public class MatrixDecimal : ICloneable, IEquatable<MatrixDecimal>, IIndexable<int, int, decimal>
    {
        /* -------------------------------------------------------------------------------------------- */

        /// <summary>Получить единичную матрицу размерности NxN</summary>
        /// <param name="N">Размерность матрицы</param>
        /// <returns>Единичная матрица размерности NxN</returns>
        [DST]
        public static MatrixDecimal GetUnitaryMatryx(int N)
        {
            var Result = new MatrixDecimal(N);
            for (var i = 0; i < N; i++) Result[i, i] = 1;
            return Result;
        }

        /// <summary>Трансвекция матрицы</summary>
        /// <param name="A">Трансвецируемая матрица</param>
        /// <param name="j">Оборный столбец</param>
        /// <returns>Трансвекция матрицы А</returns>                    
        public static MatrixDecimal GetTransvection(MatrixDecimal A, int j)
        {
            if (!A.IsSquare)
                throw new InvalidOperationException("Трансквенция неквадратной матрицы невозможна");

            var result = GetUnitaryMatryx(A.N);
            for (var i = 0; i < A.N; i++)
                result[i, j] = i == j ? 1 / A[j, j] : -A[i, j] / A[j, j];
            return result;
        }

        /* -------------------------------------------------------------------------------------------- */

        /// <summary>Число строк матрицы</summary>
        private readonly int _N;

        /// <summary>Число столбцов матрицы</summary>
        private readonly int _M;

        /// <summary>Элементы матрицы</summary>
        private readonly decimal[,] _Data;

        /* -------------------------------------------------------------------------------------------- */

        /// <summary>Число строк матрицы</summary>
        public int N => _N;

        /// <summary>Число столбцов матрицы</summary>
        public int M => _M;

        /// <summary>Элемент матрицы</summary>
        /// <param name="i">Номер строки (элемента в столбце)</param>
        /// <param name="j">Номер столбца (элемента в строке)</param>
        /// <returns>Элемент матрицы</returns>
        public decimal this[int i, int j] { [DST] get => _Data[i, j];
            [DST] set => _Data[i, j] = value;
        }

        /// <summary>Вектор-стольбец</summary>
        /// <param name="j">Номер столбца</param>
        /// <returns>Столбец матрицы</returns>
        public MatrixDecimal this[int j] => GetCol(j);

        /// <summary>Матрица является квадратной матрицей</summary>
        public bool IsSquare => M == N;

        /// <summary>Матрица является столбцом</summary>
        public bool IsCol => !IsSquare && M == 1;

        /// <summary>Матрица является строкой</summary>
        public bool IsRow => !IsSquare && N == 1;

        /// <summary>Матрица является числом</summary>
        public bool IsDigit => N == 1 && M == 1;

        public MatrixDecimal T => GetTransponse();

        public decimal Norm_m
        {
            get
            {
                var v = new decimal[N];
                for (var i = 0; i < N; i++)
                    for (var j = 0; j < M; j++)
                        v[i] += Math.Abs(_Data[i, j]);
                return v.Max();
            }
        }

        public decimal Norm_l
        {
            get
            {
                var v = new decimal[M];
                for (var j = 0; j < M; j++)
                    for (var i = 0; i < N; i++)
                        v[j] += Math.Abs(_Data[i, j]);
                return v.Max();
            }
        }

        public decimal Norm_k
        {
            get
            {
                var v = default(decimal);
                for (var i = 0; i < N; i++)
                    for (var j = 0; j < M; j++)
                        v += _Data[i, j] * _Data[i, j];
                return v.Sqrt();
            }
        }

        /* -------------------------------------------------------------------------------------------- */

        /// <summary>Матрица</summary>
        /// <param name="N">Число строк</param>
        /// <param name="M">Число столбцов</param>
        [DST]
        public MatrixDecimal(int N, int M) => _Data = new decimal[_N = N, _M = M];

        /// <summary>Квадратная матрица</summary>
        /// <param name="N">Размерность</param>
        [DST]
        public MatrixDecimal(int N) : this(N, N) { }

        /// <summary>Метод определения значения элемента матрицы</summary>
        /// <param name="i">Номер строки</param>
        /// <param name="j">Номер столбца</param>
        /// <returns>Значение элемента матрицы M[<paramref name="i"/>, <paramref name="j"/>]</returns>
        public delegate decimal MatrixDecimalItemCreator(int i, int j);

        /// <summary>Квадратная матрица</summary>
        /// <param name="N">Размерность</param>
        /// <param name="CreateFunction">Порождающая функция</param>
        [DST]
        public MatrixDecimal(int N, MatrixDecimalItemCreator CreateFunction) : this(N, N, CreateFunction) { }

        /// <summary>Матрица</summary>
        /// <param name="N">Число строк</param>
        /// <param name="M">Число столбцов</param>
        /// <param name="CreateFunction">Порождающая функция</param>
        [DST]
        public MatrixDecimal(int N, int M, MatrixDecimalItemCreator CreateFunction)
            : this(N, M)
        {
            Contract.Requires(N > 0);
            Contract.Requires(M > 0);
            Contract.Requires(CreateFunction != null);
            for (var i = 0; i < N; i++)
                for (var j = 0; j < M; j++)
                    _Data[i, j] = CreateFunction(i, j);
        }

        [DST]
        public MatrixDecimal(decimal[,] Data)
            : this(Data.GetLength(0), Data.GetLength(1))
        {
            Contract.Requires(Data != null);
            for (var i = 0; i < _N; i++)
                for (var j = 0; j < _M; j++)
                    _Data[i, j] = Data[i, j];
        }

        [DST]
        public MatrixDecimal(IList<decimal> DataRow)
            : this(DataRow.Count, 1)
        {
            Contract.Requires(DataRow != null);
            for (var i = 0; i < _N; i++)
                _Data[i, 0] = DataRow[i];
        }

        public MatrixDecimal(IEnumerable<IEnumerable<decimal>> Items) : this(GetElements(Items)) { }

        private static decimal[,] GetElements(IEnumerable<IEnumerable<decimal>> Items)
        {
            Contract.Requires(Items != null);
            var cols = Items.Select(col => col.ToListFast()).ToList();
            var cols_count = cols.Count;
            var rows_count = cols.Max(col => col.Count);
            var data = new decimal[rows_count, cols_count];
            for (var j = 0; j < cols_count; j++)
            {
                var col = cols[j];
                for (var i = 0; i < col.Count && i < rows_count; i++)
                    data[i, j] = col[i];
            }
            return data;
        }

        /* -------------------------------------------------------------------------------------------- */

        /// <summary>Получить столбец матрицы</summary>
        /// <param name="j">Номер столбца</param>
        /// <returns>Столбец матрицы номер j</returns>
        [DST]
        public MatrixDecimal GetCol(int j)
        {
            var lv_A = new MatrixDecimal(N, 1);
            for (var i = 0; i < N; i++) lv_A[i, j] = this[i, j];
            return lv_A;
        }

        /// <summary>Получить строку матрицы</summary>
        /// <param name="i">Номер строки</param>
        /// <returns>Строка матрицы номер i</returns>
        [DST]
        public MatrixDecimal GetRow(int i)
        {
            var lv_A = new MatrixDecimal(1, M);
            for (var j = 0; j < M; j++) lv_A[i, j] = this[i, j];
            return lv_A;
        }

        /// <summary>Приведение матрицы к ступенчатому виду методом гауса</summary>
        /// <returns></returns>
        public MatrixDecimal GetTriangle()
        {
            var result = (MatrixDecimal)Clone();
            var lv_RowCount = N;
            var lv_ColCount = M;
            var row = new decimal[lv_ColCount];
            for (var lv_FirstRowIndex = 0; lv_FirstRowIndex < lv_RowCount - 1; lv_FirstRowIndex++)
            {
                var lv_A = result[lv_FirstRowIndex, lv_FirstRowIndex]; //Захватываем первый элемент строки
                for (var lv_RowElementI = lv_FirstRowIndex; lv_RowElementI < result.M; lv_RowElementI++) //Нормируем строку по первому элементу
                    row[lv_RowElementI] = result[lv_FirstRowIndex, lv_RowElementI] / lv_A;

                for (var i = lv_FirstRowIndex + 1; i < lv_RowCount; i++) //Для всех оставшихся строк:
                {
                    lv_A = result[i, lv_FirstRowIndex]; //Захватываем первый элемент строки
                    for (var j = lv_FirstRowIndex; j < lv_ColCount; j++)
                        result[i, j] -= lv_A * row[j]; //Вычитаем рабочую строку, домноженную на первый элемент
                }
            }
            return result;
        }

        /// <summary>Получить обратную матрицу</summary>
        /// <returns>Обратная матрица</returns>
        public MatrixDecimal GetInverse()
        {
            if (!IsSquare)
                throw new InvalidOperationException("Обратная матрица существует только для квадратной матрицы");

            var result = GetTransvection(this, 0);
            for (var i = 1; i < N; i++)
                result *= GetTransvection(this, i);
            return result;
        }

        /// <summary>Транспонирование матрицы</summary>
        /// <returns>Транспонированная матрица</returns>
        [DST]
        public MatrixDecimal GetTransponse()
        {
            var Result = new MatrixDecimal(M, N);

            for (var i = 0; i < N; i++)
                for (var j = 0; j < M; j++)
                    Result[j, i] = this[i, j];

            return Result;
        }

        /// <summary>Алгебраическое дополнение к элементу [n,m]</summary>
        /// <param name="n">Номер столбца</param>
        /// <param name="m">Номер строки</param>
        /// <returns>Алгебраическое дополнение к элементу [n,m]</returns>
        public MatrixDecimal GetAdjunct(int n, int m) => ((n + m) % 2 == 0 ? 1 : -1) * GetMinor(n, m).GetDeterminant();

        /// <summary>Минор матрицы по определённому элементу</summary>
        /// <param name="n">Номер столбца</param>
        /// <param name="m">Номер строки</param>
        /// <returns>Минор элемента матрицы [n,m]</returns>
        public MatrixDecimal GetMinor(int n, int m)
        {
            var result = new MatrixDecimal(N - 1, M - 1);

            var i0 = 0;
            for (var i = 0; i < N; i++)
                if (i != n)
                {
                    var j0 = 0;
                    for (var j = 0; j < _M; j++)
                        if (j != m) result[i0, j0++] = this[i, j];
                    i0++;
                }
            return result;
        }

        /// <summary>Определитель матрицы</summary>
        public decimal GetDeterminant()
        {
            if (_N != _M)
                throw new InvalidOperationException("Нельзя найти определитель неквадратной матрицы!");
            var n = _N;
            if (n == 1) return this[0, 0];

            if (n == 2) return this[0, 0] * this[1, 1] - this[0, 1] * this[1, 0];

            var lv_DataArray = (decimal[,])_Data.Clone();

            //var det = 1.0;
            //for(var k = 0; k <= n; k++)
            //{
            //    int i;
            //    int j;
            //    if(lv_DataArray[k, k].Equals(0))
            //    {
            //        j = k;
            //        while(j < n && lv_DataArray[k, j].Equals(0)) j++;

            //        if(lv_DataArray[k, j].Equals(0)) return 0;

            //        for(i = k; i <= n; i++)
            //        {
            //            var save = lv_DataArray[i, j];
            //            lv_DataArray[i, j] = lv_DataArray[i, k];
            //            lv_DataArray[i, k] = save;
            //        }
            //        det = -det;
            //    }
            //    var doagonal_item = lv_DataArray[k, k];

            //    det *= doagonal_item;

            //    if(k >= n) continue;

            //    var k1 = k + 1;
            //    for(i = k1; i <= n; i++)
            //        for(j = k1; j <= n; j++)
            //            lv_DataArray[i, j] -= lv_DataArray[i, k] * lv_DataArray[k, j] / doagonal_item;
            //}

            #region

            GetLUDecomposition(out var L, out var U, out var P);
            decimal det = 1;
            for (var i = 0; i < N; i++)
                det *= U[i, i];

            //decimal det = 0;
            //for(int j = 0, k = 1; j < M; j++, k *= -1)
            //    det += this[0, j] * k * GetMinor(0, j).GetDeterminant();

            #endregion


            return det;
        }

        public void GetLUDecomposition(out MatrixDecimal L, out MatrixDecimal U, out MatrixDecimal P)
        {
            LUDecomposition(_Data, out var l, out var u, out var p);
            L = new MatrixDecimal(l);
            U = new MatrixDecimal(u);
            P = new MatrixDecimal(p);
        }

        /// <summary>
        /// Returns the LU Decomposition of a matrix. 
        /// the output is: lower triangular matrix L, upper
        /// triangular matrix U, and permutation matrix P so that
        ///	P*X = L*U.
        /// In case of an error the error is raised as an exception.
        /// Note - This method is based on the 'LU Decomposition and Its Applications'
        /// section of Numerical Recipes in C by William H. Press,
        /// Saul A. Teukolsky, William T. Vetterling and Brian P. Flannery,
        /// University of Cambridge Press 1992.  
        /// </summary>
        /// <param name="Mat">Array which will be LU Decomposed</param>
        /// <param name="L">An array where the lower traingular matrix is returned</param>
        /// <param name="U">An array where the upper traingular matrix is returned</param>
        /// <param name="P">An array where the permutation matrix is returned</param>
        private static void LUDecomposition(decimal[,] Mat, out decimal[,] L, out decimal[,] U, out decimal[,] P)
        {
            var A = (decimal[,])Mat.Clone();
            var Rows = Mat.GetUpperBound(0);
            var Cols = Mat.GetUpperBound(1);

            if (Rows != Cols) throw new ArgumentException("Матрица не квадратная", nameof(Mat));


            var N = Rows;
            var indexex = new int[N + 1];
            var V = new decimal[N * 10];

            int i, j;
            for (i = 0; i <= N; i++)
            {
                var a_max = 0m;
                for (j = 0; j <= N; j++)
                    if (Math.Abs(A[i, j]) > a_max)
                        a_max = Math.Abs(A[i, j]);

                if (a_max.Equals(0))
                    throw new ArgumentException("Матрица вырождена", nameof(Mat));

                V[i] = 1 / a_max;
            }

            for (j = 0; j <= N; j++)
            {
                int k;
                decimal Sum;
                if (j > 0)
                    for (i = 0; i < j; i++)
                    {
                        Sum = A[i, j];
                        if (i <= 0) continue;
                        for (k = 0; k < i; k++)
                            Sum -= A[i, k] * A[k, j];
                        A[i, j] = Sum;
                    }

                var a_max = 0m;
                decimal Dum;
                var i_max = 0;
                for (i = j; i <= N; i++)
                {
                    Sum = A[i, j];
                    if (j > 0)
                    {
                        for (k = 0; k < j; k++)
                            Sum -= A[i, k] * A[k, j];
                        A[i, j] = Sum;
                    }
                    Dum = V[i] * Math.Abs(Sum);
                    if (Dum < a_max) continue;
                    i_max = i;
                    a_max = Dum;
                }

                if (j != i_max)
                {
                    for (k = 0; k <= N; k++)
                    {
                        Dum = A[i_max, k];
                        A[i_max, k] = A[j, k];
                        A[j, k] = Dum;
                    }
                    V[i_max] = V[j];
                }

                indexex[j] = i_max;

                if (j == N) continue;

                // ReSharper disable RedundantCast
                if (A[j, j].Equals(0))
                    A[j, j] = (decimal)1E-20;
                // ReSharper restore RedundantCast

                Dum = 1 / A[j, j];

                for (i = j + 1; i <= N; i++)
                    A[i, j] = A[i, j] * Dum;
            }

            // ReSharper disable RedundantCast
            if (A[N, N].Equals(0))
                A[N, N] = (decimal)1E-20;
            // ReSharper restore RedundantCast

            var count = 0;
            var l = new decimal[N + 1, N + 1];
            var u = new decimal[N + 1, N + 1];

            for (i = 0; i <= N; i++, count++)
                for (j = 0; j <= count; j++)
                {
                    if (i != 0) l[i, j] = A[i, j];
                    if (i == j) l[i, j] = 1;
                    u[N - i, N - j] = A[N - i, N - j];
                }

            L = l;
            U = u;

            P = Identity(N + 1);
            for (i = 0; i <= N; i++)
                P.SwapRows(i, indexex[i]);
        }

        private static decimal[,] Identity(int n)
        {
            var temp = new decimal[n, n];
            for (var i = 0; i < n; i++)
                temp[i, i] = 1;
            return temp;
        }

        /* -------------------------------------------------------------------------------------------- */

        [DST]
        public override string ToString() => $"MatrixDecimal[{N}x{M}]";

        [DST]
        public string ToStringFormat(string Format) => ToStringFormat('\t', Format);

        //[DST] public string ToStringFormat(char Splitter) { return ToStringFormat(Splitter, "r"); }

        public string ToStringFormat(char Splitter = '\t', string Format = "r")
        {
            var result = new StringBuilder();

            for (var i = 0; i < _N; i++)
            {
                var lv_Str = _Data[i, 0].ToString(Format);
                for (var j = 1; j < _M; j++)
                    lv_Str += Splitter + _Data[i, j].ToString(Format);
                result.AppendLine(lv_Str);
            }
            return result.ToString();
        }

        /* -------------------------------------------------------------------------------------------- */

        #region ICloneable Members

        /// <summary>Клонирование матрицы</summary>
        /// <returns>Копия текущей матрицы</returns>
        [DST]
        public object Clone()
        {
            var result = new MatrixDecimal(N, M);
            for (var i = 0; i < N; i++) for (var j = 0; j < M; j++) result[i, j] = this[i, j];
            return result;
        }

        #endregion

        /* -------------------------------------------------------------------------------------------- */

        public static bool operator ==(MatrixDecimal A, MatrixDecimal B) => A is null && B is null || A is { } && B is { } && A.Equals(B);

        public static bool operator !=(MatrixDecimal A, MatrixDecimal B) => !(A == B);

        [DST]
        public static MatrixDecimal operator +(MatrixDecimal M, decimal x)
        {
            var result = new MatrixDecimal(M.N, M.M);
            for (var i = 0; i < M.N; i++)
                for (var j = 0; j < M.M; j++)
                    result[i, j] = M[i, j] + x;
            return result;
        }

        [DST]
        public static MatrixDecimal operator +(decimal x, MatrixDecimal M)
        {
            var result = new MatrixDecimal(M.N, M.M);
            for (var i = 0; i < M.N; i++)
                for (var j = 0; j < M.M; j++)
                    result[i, j] = M[i, j] + x;
            return result;
        }

        [DST]
        public static MatrixDecimal operator -(MatrixDecimal M, decimal x)
        {
            var result = new MatrixDecimal(M.N, M.M);
            for (var i = 0; i < M.N; i++)
                for (var j = 0; j < M.M; j++)
                    result[i, j] = M[i, j] - x;
            return result;
        }

        [DST]
        public static MatrixDecimal operator -(decimal x, MatrixDecimal M)
        {
            var result = new MatrixDecimal(M.N, M.M);
            for (var i = 0; i < M.N; i++)
                for (var j = 0; j < M.M; j++)
                    result[i, j] = x - M[i, j];
            return result;
        }

        [DST]
        public static MatrixDecimal operator *(MatrixDecimal M, decimal x)
        {
            var result = new MatrixDecimal(M.N, M.M);
            for (var i = 0; i < M.N; i++)
                for (var j = 0; j < M.M; j++)
                    result[i, j] = M[i, j] * x;
            return result;
        }

        [DST]
        public static MatrixDecimal operator *(decimal x, MatrixDecimal M)
        {
            var result = new MatrixDecimal(M.N, M.M);
            for (var i = 0; i < M.N; i++)
                for (var j = 0; j < M.M; j++)
                    result[i, j] = M[i, j] * x;
            return result;
        }

        [DST]
        public static MatrixDecimal operator *(decimal[,] A, MatrixDecimal B) => (MatrixDecimal)A * B;

        [DST]
        public static MatrixDecimal operator *(decimal[] A, MatrixDecimal B) => (MatrixDecimal)A * B;

        [DST]
        public static MatrixDecimal operator *(MatrixDecimal A, decimal[] B) => A * (MatrixDecimal)B;

        [DST]
        public static MatrixDecimal operator *(MatrixDecimal A, decimal[,] B) => A * (MatrixDecimal)B;

        [DST]
        public static MatrixDecimal operator /(MatrixDecimal M, decimal x)
        {
            var result = new MatrixDecimal(M.N, M.M);
            for (var i = 0; i < M.N; i++)
                for (var j = 0; j < M.M; j++)
                    result[i, j] = M[i, j] / x;
            return result;
        }

        public static MatrixDecimal operator /(decimal x, MatrixDecimal M)
        {
            M = M.GetInverse();
            var result = new MatrixDecimal(M.N, M.M);
            for (var i = 0; i < M.N; i++)
                for (var j = 0; j < M.M; j++)
                    result[i, j] = M[i, j] * x;
            return result;
        }

        /// <summary>Оператор сложения двух матриц</summary>
        /// <param name="A">Первое слогаемое</param>
        /// <param name="B">Второе слогаемое</param>
        /// <returns>Сумма двух матриц</returns>
        [DST]
        public static MatrixDecimal operator +(MatrixDecimal A, MatrixDecimal B)
        {
            if (A.N != B.N || A.M != B.M)
                throw new ArgumentOutOfRangeException(nameof(B), "Размеры матриц не равны.");

            var result = new MatrixDecimal(A.N, A.M);

            for (var i = 0; i < result.N; i++)
                for (var j = 0; j < result.M; j++)
                    result[i, j] = A[i, j] + B[i, j];

            return result;
        }

        /// <summary>Оператор разности двух матриц</summary>
        /// <param name="A">Уменьшаемое</param>
        /// <param name="B">Вычитаемое</param>
        /// <returns>Разность двух матриц</returns>
        [DST]
        public static MatrixDecimal operator -(MatrixDecimal A, MatrixDecimal B)
        {
            if (A.N != B.N || A.M != B.M)
                throw new ArgumentOutOfRangeException(nameof(B), "Размеры матриц не равны.");

            var result = new MatrixDecimal(A.N, A.M);

            for (var i = 0; i < result.N; i++)
                for (var j = 0; j < result.M; j++)
                    result[i, j] = A[i, j] - B[i, j];

            return result;
        }

        /// <summary>Оператор произведения двух матриц</summary>
        /// <param name="A">Первый сомножитель</param>
        /// <param name="B">Второй сомножитель</param>
        /// <returns>Произведение двух матриц</returns>
        [DST]
        public static MatrixDecimal operator *(MatrixDecimal A, MatrixDecimal B)
        {
            if (A.M != B.N)
                throw new ArgumentOutOfRangeException(nameof(B), "Матрицы несогласованных порядков.");

            var result = new MatrixDecimal(A.N, B.M);

            for (var i = 0; i < result.N; i++)
                for (var j = 0; j < result.M; j++)
                    for (var k = 0; k < A.M; k++)
                        result[i, j] += A[i, k] * B[k, j];

            return result;
        }

        /// <summary>Оператор деления двух матриц</summary>
        /// <param name="A">Делимое</param>
        /// <param name="B">Делитель</param>
        /// <returns>Частное двух матриц</returns>
        public static MatrixDecimal operator /(MatrixDecimal A, MatrixDecimal B)
        {
            B = B.GetInverse();
            if (A.M != B.N)
                throw new ArgumentOutOfRangeException(nameof(B), "Матрицы несогласованных порядков.");

            var result = new MatrixDecimal(A.N, B.M);

            for (var i = 0; i < result.N; i++)
                for (var j = 0; j < result.M; j++)
                    for (var k = 0; k < A.M; k++)
                        result[i, j] += A[i, k] * B[k, j];

            return result;
        }

        /// <summary>Конкатинация двух матриц (либо по строкам, либо по столбцам)</summary>
        /// <param name="A">Первое слогаемое</param>
        /// <param name="B">Второе слогаемое</param>
        /// <returns>Объединённая матрица</returns>
        public static MatrixDecimal operator |(MatrixDecimal A, MatrixDecimal B)
        {
            MatrixDecimal result;
            if (A.M == B.M) // Конкатинация по строкам
            {
                result = new MatrixDecimal(A.N + B.N, A.M);
                for (var i = 0; i < A.N; i++)
                    for (var j = 0; j < A.M; j++)
                        result[i, j] = A[i, j];
                var i0 = A.N;
                for (var i = 0; i < B.N; i++)
                    for (var j = 0; j < B.M; j++)
                        result[i + i0, j] = B[i, j];

            }
            else if (A.N == B.N) //Конкатинация по строкам
            {
                result = new MatrixDecimal(A.N, A.M + B.M);
                for (var i = 0; i < A.N; i++)
                    for (var j = 0; j < A.M; j++)
                        result[i, j] = A[i, j];
                var j0 = A.M;
                for (var i = 0; i < B.N; i++)
                    for (var j = 0; j < B.M; j++)
                        result[i, j + j0] = B[i, j];
            }
            else
                throw new InvalidOperationException("Конкатинация возможна только по строкам, или по столбцам");

            return result;
        }



        /* -------------------------------------------------------------------------------------------- */

        /// <summary>Оператор неявного преведения типа вещественного числа двойной точнойсти к типу Матрица порядка 1х1</summary>
        /// <param name="X">Приводимое число</param><returns>Матрица порадка 1х1</returns>
        [DST]
        public static implicit operator MatrixDecimal(decimal X) => new MatrixDecimal(1, 1) { [0, 0] = X };

        [DST]
        public static explicit operator decimal[,](MatrixDecimal M) => (decimal[,])M._Data.Clone();

        [DST]
        public static explicit operator MatrixDecimal(decimal[,] Data) => new MatrixDecimal(Data);

        [DST]
        public static explicit operator MatrixDecimal(decimal[] Data) => new MatrixDecimal(Data);

        /* -------------------------------------------------------------------------------------------- */

        #region IEquatable<MatrixDecimal> Members

        public bool Equals(MatrixDecimal other)
        {
            return other is { }
                   && (ReferenceEquals(this, other)
                        || other._N == _N
                            && other._M == _M
                            && Equals(other._Data, _Data));
        }

        [DST]
        bool IEquatable<MatrixDecimal>.Equals(MatrixDecimal other) => Equals(other);

        #endregion

        public override bool Equals(object obj) =>
            obj is { }
            && (ReferenceEquals(this, obj)
                || obj.GetType() == typeof(MatrixDecimal)
                && Equals((MatrixDecimal)obj));

        [DST]
        public override int GetHashCode()
        {
            unchecked
            {
                var result = _N;
                result = (result * 397) ^ _M;
                _Data.Foreach((i, j, v) => result = (result * 397) ^ i ^ j ^ v.GetHashCode());
                //result = (result * 397) ^ (_Data != null ? _Data.GetHashCode() : 0);
                return result;
            }
        }

        /* -------------------------------------------------------------------------------------------- */

        public decimal[,] GetData() => _Data;
    }
}
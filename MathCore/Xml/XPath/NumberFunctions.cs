namespace System.Xml.XPath
{
    internal sealed class NumberFunctions : Query
    {
        #region Fields

        private readonly Function.FunctionType _FuncType;
        private readonly Query _Qy;

        #endregion

        #region Constructors

        public NumberFunctions(Query qy,
            Function.FunctionType ftype)
        {
            _Qy = qy;
            _FuncType = ftype;
        }

        public NumberFunctions() { }

        public NumberFunctions(Query qy)
        {
            _Qy = qy;
            _FuncType = Function.FunctionType.FuncNumber;
        }

        #endregion

        #region Methods

        //
        //
        internal override object GetValue(XPathReader reader)
        {
            var obj = new object();

            switch(_FuncType)
            {
                case Function.FunctionType.FuncNumber:
                    obj = Number(reader);
                    break;

                // case FT.FuncSum:
                //     obj = Sum(reader);
                //     break;

                case Function.FunctionType.FuncFloor:
                    obj = Floor(reader);
                    break;

                case Function.FunctionType.FuncCeiling:
                    obj = Ceiling(reader);
                    break;

                case Function.FunctionType.FuncRound:
                    obj = Round(reader);
                    break;
            }

            return obj;
        }

        //
        //
        internal override XPathResultType ReturnType() => XPathResultType.Number;

        //
        //
        internal static double Number(bool Qy) => Convert.ToInt32(Qy);

        //
        //
        internal static double Number(string Qy)
        {
            try
            {
                return Convert.ToDouble(Qy);
            } catch(Exception)
            {
                return double.NaN;
            }
        }

        //
        //
        //internal static double sum(XPathReader reader) {
        //    return 0;

        //}

        //
        //
        internal static double Number(double num) => num;

        //
        // number number(object?)
        // string: IEEE 754, NaN
        // boolean: true 1, false 0
        // node-set: number(string(node-set))
        //
        // <Root><e a='1'/></Root>
        // /Root/e[@a=number('1')]
        // /Root/e[number(@a)=1]

        private double Number(XPathReader reader)
        {
            if(_Qy == null) return double.NaN;
            var obj = _Qy.GetValue(reader);

            return obj == null ? double.NaN : Convert.ToDouble(obj);
        }


        private double Floor(XPathReader reader) => Math.Floor(Convert.ToDouble(_Qy.GetValue(reader)));

        private double Ceiling(XPathReader reader) => Math.Ceiling(Convert.ToDouble(_Qy.GetValue(reader)));

        private double Round(XPathReader reader)
        {
            var n = Convert.ToDouble(_Qy.GetValue(reader));
            // Math.Round does bankers rounding and Round(1.5) == Round(2.5) == 2
            // This is incorrect in XPath and to fix this we are useing Math.Floor(n + 0.5) istead
            // To deal with -0.0 we have to use Math.Round in [0.5, 0.0]
            return -0.5 <= n && n <= 0.0 ? Math.Round(n) : Math.Floor(n + 0.5);
        }

        #endregion
    }
}
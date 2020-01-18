﻿// ReSharper disable once CheckNamespace
namespace System.Xml.XPath
{
    internal sealed class NumericExpr : Query
    {
        #region Fields

        internal Operator.Op Op;
        internal Query Opnd1;
        internal Query Opnd2;

        #endregion

        #region Constructors

        //
        // The operand needs to use the number function
        // to covert to numbers
        internal NumericExpr(Operator.Op op, Query opnd1, Query opnd2)
        {
            Opnd1 = opnd1.ReturnType() != XPathResultType.Number ? new NumberFunctions(opnd1) : opnd1;

            if(opnd2 != null && (opnd2.ReturnType() != XPathResultType.Number))
                Opnd2 = new NumberFunctions(opnd2);
            else
                Opnd2 = opnd2;

            Op = op;
        }

        #endregion

        #region Methods

        internal override object GetValue(XPathReader reader)
        {
            var n1 = Convert.ToDouble(Opnd1.GetValue(reader));

            var n2 = 0d;
            if(Op != Operator.Op.Negate)
                n2 = Convert.ToDouble(Opnd2.GetValue(reader));

            return Op switch
            {
                Operator.Op.Plus => (n1 + n2),
                Operator.Op.Minus => (n1 - n2),
                Operator.Op.Mod => (n1 % n2),
                Operator.Op.Div => (n1 / n2),
                Operator.Op.Mul => (n1 * n2),
                Operator.Op.Negate => -n1,
                _ => (object)null
            };
        }

        internal override XPathResultType ReturnType() => XPathResultType.Number;

        #endregion
    }
}
using System.Diagnostics.Contracts;

namespace MathCore
{
    /// <summary>������ ��������� ���������� ���������� ��������� ��������</summary>
    [ContractClass(typeof(ContractClassICountable))]
    public interface ICountable
    {
        /// <summary>����� ���������</summary>
        int Count { get; }
    }

    [ContractClassFor(typeof(ICountable))]
    abstract class ContractClassICountable : ICountable
    {
        #region Implementation of ICountable

        /// <summary>����� ���������</summary>
        public int Count
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);
                return 0;
            }
        }

        #endregion
    }
}
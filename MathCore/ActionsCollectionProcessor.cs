using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace System
{
    public class ActionsCollectionProcessor : Processor
    {
        private readonly IEnumerable<Action> _ActionsCollection;

        public ActionsCollectionProcessor(IEnumerable<Action> ActionsCollection)
        {
            Contract.Requires(ActionsCollection != null, "������� ������� ������ �� ����������� ��������");
            _ActionsCollection = ActionsCollection;
        }

        /// <summary>�������� ����� �������� ����������, ���������� � �����. ������ ���� �������������� � �������-�����������</summary>
        protected override void MainAction() => _ActionsCollection.Foreach(a => a());
    }
}
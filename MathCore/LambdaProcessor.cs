using System.Diagnostics.Contracts;

namespace System
{
    public class LambdaProcessor : Processor
    {
        private readonly Action _Action;

        public LambdaProcessor(Action action)
        {
            Contract.Requires(action != null, "������� ������� ������ �� ����������� ��������");

            _Action = action;
        }

        /// <summary>�������� ����� �������� ����������, ���������� � �����. ������ ���� �������������� � �������-�����������</summary>
        protected override void MainAction() => _Action();
    }
}
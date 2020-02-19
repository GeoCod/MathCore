// ReSharper disable once CheckNamespace
namespace System.Drawing
{
    /// <summary>Деконструктор для <see cref="Size"/> и <see cref="SizeF"/></summary>
    public static class SizeExtensions
    {
        /// <summary>Деконструктор на значения ширины и высоты</summary>
        /// <param name="size">Деконструируемое значение размера</param>
        /// <param name="Width">Значение ширины размера</param>
        /// <param name="Height">Значение высоты размера</param>
        public static void Deconstruct(this Size size, out int Width, out int Height)
        {
            Width = size.Width;
            Height = size.Height;
        }

        /// <summary>Деконструктор на значения ширины и высоты</summary>
        /// <param name="size">Деконструируемое значение размера</param>
        /// <param name="Width">Значение ширины размера</param>
        /// <param name="Height">Значение высоты размера</param>
        public static void Deconstruct(this SizeF size, out float Width, out float Height)
        {
            Width = size.Width;
            Height = size.Height;
        }
    }
}
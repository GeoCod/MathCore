using MathCore.Annotations;
using DST = System.Diagnostics.DebuggerStepThroughAttribute;

// ReSharper disable once CheckNamespace
namespace System
{
    /// <summary>Методы-расширения для <see cref="TimeSpan"/></summary>
    public static class TimeSpanExtensions
    {
        /// <summary>
        /// Преобразовать значение в строку.
        /// Если часы равны 0, то поле часов отсутствует,
        /// если часы и минуты равны нулю, то поля часов и минут отсутствуют
        /// </summary>
        /// <param name="time">Преобразуемое время</param>
        /// <returns>Строковое представление времени в укороченном формате</returns>
        [DST, NotNull]
        public static string ToShortString(this TimeSpan time)
        {
            var result = string.Empty;
            var days = time.Days;
            if(days != 0) result = $"{days}";

            var hours = time.Hours;
            if(result.Length > 0)
                result += $":{hours:00}";
            else if(hours != 0) 
                result = hours.ToString();

            var minutes = time.Minutes;
            if(result.Length > 0)
                result += $":{minutes:00}";
            else if(minutes != 0) 
                result = minutes.ToString();

            var seconds = time.Seconds;
            var milliseconds = time.Milliseconds;

            return result.Length > 0
                    ? seconds == 0 && milliseconds == 0
                        ? "0"
                        : $"{seconds + (double)milliseconds / 1000}"
                    : seconds >= 10 
                        ? $"{result}:{seconds + (double)milliseconds/1000}" : $"{result}:0{seconds + (double)milliseconds/1000}";
        }
    }
}
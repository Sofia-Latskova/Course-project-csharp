using System;

namespace Firewatch
{
    // Базовый класс для ошибок времени работы
    public class FirewatchException : Exception
    {
        public FirewatchException(string message) : base(message) { }
    }

    // Датчик неисправен
    public class SensorFailure : FirewatchException
    {
        public SensorFailure(string serial) : base($"датчик [{serial}] неисправен") { }
    }

    // Заряд ниже минимума
    public class LowBatteryException : FirewatchException
    {
        public LowBatteryException(int charge, int min)
            : base($"заряд {charge}% ниже минимума {min}%") { }
    }

    // Разряд ниже 0 
    public class BatteryOverDischargeException : InvalidOperationException
    {
        public BatteryOverDischargeException(int charge, int percent)
            : base($"разряд {percent}% уводит заряд {charge}% ниже 0") { }
    }

    // Неверный статус миссии 
    public class InvalidMissionStateException : InvalidOperationException
    {
        public InvalidMissionStateException(string message) : base(message) { }
    }
}
using System;

namespace Firewatch
{
    public sealed class FlameSensor : BaseSensor
    {
        public int Intensity { get; set; }
        private int _lastValue = 0;

        public FlameSensor(string serial, Quadcopter quadcopter, int intensity) : base(serial, quadcopter)
        {
            Intensity = intensity;
            Console.WriteLine($"[FlameSensor] Создан датчик пламени.");
        }

        public override double ReadValue()
        {
            if (WearDown()) return -999.0;

            if (Intensity > 90) { Break(); return -999.0; }

            if (_lastValue < 10)
            {
                _lastValue = 0;
            }

            // Добавляем шум
            int noise = Rng.Next(-2, 3);
            _lastValue += noise;

            // Проверяем, чтобы значение не вылетело за пределы 0..100
            if (_lastValue < 0) _lastValue = 0;
            if (_lastValue > 100) _lastValue = 100;

            return _lastValue;
        }

        public override string Describe()
        {
            if (IsBroken) return GetHeader() + "\n[ДАТЧИК НЕИСПРАВЕН]";

            string result = GetHeader() + "\n";
            result += $"ID: {Id} | Тип: Датчик пламени\n";
            result += $"Интенсивность: {_lastValue} / 100 баллов\n";

            if (_lastValue > 80) result += "Статус: [КРИТИЧЕСКИЙ ПОЖАР]";
            else if (_lastValue > 40) result += "Статус: [ВНИМАНИЕ: ПЛАМЯ]";
            else result += "Статус: [В норме]";

            return result;
        }

        public override BaseSensor Clone()
        {
            var copy = (FlameSensor)MemberwiseClone();
            copy.Quadcopter = new Quadcopter(Quadcopter);
            return copy;
        }

    }
}
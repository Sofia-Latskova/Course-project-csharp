using System;

namespace Firewatch
{
    public sealed class FlameSensor : BaseSensor, Scannable
    {
        public int Intensity { get; set; }
        private int _lastValue = 0;

        public FlameSensor(string serial, Quadcopter quadcopter, int intensity) : base(serial, quadcopter)
        {
            Intensity = intensity;
            Console.WriteLine($"[FlameSensor] Создан датчик пламени.");
        }

        public double ReadValue()
        {
            if (WearDown()) throw new SensorFailure(_serial);

            if (Intensity > 90) { Break(); throw new SensorFailure(_serial); }

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
        //раннее/позднее
        public new string GetKind() => "датчик пламени";
        public override string GetTypeName() => "пламя";
        public string SensorSerial => _serial;
    }
}
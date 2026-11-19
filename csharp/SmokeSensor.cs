using System;

namespace Firewatch
{
    // sealed запрещает наследование (как final в C++)
    public sealed class SmokeSensor : BaseSensor
    {
        public double SmokeLevel { get; set; }
        private int _warningCount = 0;
        private int _warmupCounter = 0;
        private double _lastValue = 0.0;

        public SmokeSensor(string serial, Quadcopter quadcopter, double smokeLevel) : base(serial, quadcopter)
        {
            SmokeLevel = smokeLevel;
            Console.WriteLine($"[SmokeSensor] Создан датчик дыма.");
        }

        public override double ReadValue()
        {
            if (WearDown()) return -999.0;

            // Прогрев
            if (_warmupCounter < 2)
            {
                _warmupCounter++;
                return 0.0;
            }

            // Перегрузка
            if (SmokeLevel > 0.95)
            {
                Break();
                return -999.0;
            }

            // Накопление тревог
            if (SmokeLevel > 0.5) _warningCount++;
            else _warningCount = 0;

            _lastValue = SmokeLevel;

            // Добавляем шум (Rng из базового класса)
            double noise = (Rng.Next(5) - 2) / 100.0;
            _lastValue += noise;

            // Проверяем, чтобы значение не вылетело за пределы 0.0 .. 1.0
            if (_lastValue < 0.0) _lastValue = 0.0;
            if (_lastValue > 1.0) _lastValue = 1.0;

            return _lastValue;
        }

        // Переопределение без вызова базы
        public override string Describe()
        {
            if (IsBroken) return GetHeader() + "\n[ДАТЧИК НЕИСПРАВЕН]";

            int percent = (int)(_lastValue * 100);

            string result = GetHeader() + "\n";
            result += $"ID: {Id} | Тип: Датчик дыма\n";
            result += $"Задымление: {percent}%\n";

            if (_lastValue > 0.9) result += "Статус: [КРИТИЧЕСКИЙ УРОВЕНЬ]";
            else if (_lastValue > 0.5) result += "Статус: [Внимание: дым]";
            else result += "Статус: [В норме]";

            return result;
        }

        public override BaseSensor Clone()
        {
            var copy = (SmokeSensor)MemberwiseClone();
            copy.Quadcopter = new Quadcopter(Quadcopter);
            return copy;
        }

    }
}
using System;

namespace Firewatch
{
    public class TempSensor : BaseSensor, Scannable
    {
        public double Temperature { get; set; }
        private double _lastValue = 0.0;
        public TempSensor(string serial, Quadcopter quadcopter, double temperature)
            : base(serial, quadcopter)
        {
            Temperature = temperature;
            Console.WriteLine($"[TempSensor] Создан датчик темп. {_serial}");
        }

        public double ReadValue()
        {
            if (WearDown()) throw new SensorFailure(_serial);
            double noise = (Rng.Next(100) - 50) / 100.0;
            _lastValue = Temperature + noise; // Берем текущую температуру
            return _lastValue;
        }
        public string SensorSerial => _serial;
        public override string Describe()
        {
            if (IsBroken) return GetHeader() + "\n[ДАТЧИК НЕИСПРАВЕН]";

            string result = base.Describe() + "Тип: Датчик температуры\n";
            result += $"Показания: {_lastValue:F2} °C\n";

            if (_lastValue > 50.0) result += "Статус: [ОПАСНО: ПЕРЕГРЕВ]";
            else result += "Статус: [В норме]";

            return result;
        }
        //глубокая копия
        public override BaseSensor Clone()
        {
            //Поверхностная копия всех полей
            var copy = (TempSensor)MemberwiseClone();

            //Заменяем ссылку на дрон на новый дрон 
            copy.Quadcopter = new Quadcopter(Quadcopter);

            return copy;
        }

        //раннее/позднее
        public new string GetKind() => "датчик температуры";
        public override string GetTypeName() => "температура";






    }
 

}

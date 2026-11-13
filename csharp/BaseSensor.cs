using System;

namespace Firewatch
{
    public abstract class BaseSensor
    {
        protected static readonly Random Rng = new Random(); //чтобы не было одинаковых чисел
        private static int _lastId = 0; //статический счетчик 

        // доступ у наследников
        protected string _serial;
        public Quadcopter Quadcopter { get; protected set; }

        public int Id { get; }
        public int Durability { get; private set; }
        public bool IsBroken { get; private set; }

        //конструктор
        protected BaseSensor(string serial, Quadcopter quadcopter)
        {
            Id = NextSensorId();
            _serial = serial;
            Quadcopter = quadcopter;
            Durability = Rng.Next(10, 51); // 10..50
            IsBroken = false;

            Console.WriteLine($"[BaseSensor] Создан: {_serial} (ID: {Id}, дрон: {Quadcopter.Number})");
        }
        public static int NextSensorId() => ++_lastId;

        // Абстрактный метод 
        public abstract double ReadValue();

        // Виртуальный метод с реализацией
        public virtual string Describe()
        {
            return GetHeader() + "\n" +
                  $"ID: {Id} | Дрон: {Quadcopter.Number} [{Quadcopter.StateStr}]\n";
        }

        // protected метод 
        protected string GetHeader()
        {
            return $"--- Датчик [{_serial}] на {Quadcopter.Number} ---";
        }

        public void Break() => IsBroken = true;

        public bool WearDown()
        {
            if (IsBroken) return true;
            if (Durability > 0)
            {
                Durability--;
                if (Durability == 0)
                {
                    Break();
                    Console.WriteLine($"[BaseSensor] {_serial} сломался от износа!");
                    return true;
                }
            }
            return false;
        }

        public abstract BaseSensor Clone();

        public virtual BaseSensor ShallowCopy()
        {
            var copy = (BaseSensor)MemberwiseClone();
            return copy;
        }

        public override bool Equals(object? obj)
        {
            // Сравниваем по серийному номеру
            return obj is BaseSensor other && _serial == other._serial;
        }

        public override int GetHashCode()
        {
            return _serial.GetHashCode();
        }

    }
}








using System;

namespace Firewatch
{
    public class Quadcopter
    {
        public enum State
        {
            OnBase,
            Flying,
            Scanning,
            Returning
        }

        public string Number { get; }
        public State CurrentState { get; private set; }
        public Battery Battery { get; }

        public Quadcopter()
        {
            Number = "DRN-000";
            CurrentState = State.OnBase;
            Battery = new Battery();
            Console.WriteLine($"[Quadcopter] Создан {Number}");
        }

        public Quadcopter(string number, int chargePercent, int capacityPercent, string batterySerial)
        {
            Number = number;
            CurrentState = State.OnBase;
            Battery = new Battery(chargePercent, capacityPercent, batterySerial);
            Console.WriteLine($"[Quadcopter] Создан {Number}");
        }

        public Quadcopter(Quadcopter other)
        {
            Number = other.Number;
            CurrentState = other.CurrentState;
            Battery = new Battery(other.Battery); 
            Console.WriteLine($"[Quadcopter] Копия: {Number}");
        }

        public int BatteryCharge => Battery.ChargePercent;
        public string StateStr
        {
            get
            {
                switch (CurrentState)
                {
                    case State.OnBase: return "на базе";
                    case State.Flying: return "в полёте";
                    case State.Scanning: return "сканирует";
                    case State.Returning: return "возвращается";
                    default: return "неизвестно";
                }
            }
        }

        public bool TakeOff()
        {
            if (!CanTakeOff)
            {
                Console.WriteLine($"[Quadcopter] Отказ взлёта: заряд {Battery.ChargePercent}% ниже минимума {Battery.MinCharge}%");
                return false;
            }
            CurrentState = State.Flying;
            Console.WriteLine($"[Quadcopter] {Number} взлетел");
            return true;
        }

        public void Land()
        {
            CurrentState = State.OnBase;
            Console.WriteLine($"[Quadcopter] {Number} сел на базу");
        }

        public void Scan()
        {
            if (CurrentState != State.Flying)
            {
                Console.WriteLine("[Quadcopter] Сканирование невозможно: не в полёте");
                return;
            }
            CurrentState = State.Scanning;
            Battery.Consume(2);
            Console.WriteLine($"[Quadcopter] {Number} снял показания, заряд={Battery.ChargePercent}%");
            CurrentState = State.Flying;
        }

        public bool CanTakeOff => CurrentState == State.OnBase && Battery.ChargePercent >= Battery.MinCharge;

        public override string ToString()
        {
            return $"{Number}: {StateStr}, заряд {BatteryCharge}%";
        }
    }
}












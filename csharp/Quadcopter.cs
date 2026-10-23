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

        private readonly string _number;
        private State _state;
        private readonly Battery _battery;

        public Quadcopter()
        {
            _number = "DRN-000";
            _state = State.OnBase;
            _battery = new Battery();
            Console.WriteLine($"[Quadcopter] Создан {_number}");
        }

        public Quadcopter(string number, int chargePercent, int capacityPercent, string batterySerial)
        {
            _number = number;
            _state = State.OnBase;
            _battery = new Battery(chargePercent, capacityPercent, batterySerial);
            Console.WriteLine($"[Quadcopter] Создан {_number}");
        }

        public string Number
        {
            get { return _number; }
        }

        public State CurrentState
        {
            get { return _state; }
        }

        public int BatteryCharge
        {
            get { return _battery.ChargePercent; }
        }

        public string StateStr
        {
            get
            {
                switch (_state)
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
                Console.WriteLine($"[Quadcopter] Отказ взлёта: заряд {_battery.ChargePercent}% ниже минимума {Battery.MinCharge}%");
                return false;
            }
            _state = State.Flying;
            Console.WriteLine($"[Quadcopter] {_number} взлетел");
            return true;
        }

        public void Land()
        {
            _state = State.OnBase;
            Console.WriteLine($"[Quadcopter] {_number} сел на базу");
        }

        public void Scan()
        {
            if (_state != State.Flying)
            {
                Console.WriteLine("[Quadcopter] Сканирование невозможно: не в полёте");
                return;
            }
            _state = State.Scanning;
            _battery.Consume(2);
            Console.WriteLine($"[Quadcopter] {_number} снял показания, заряд={_battery.ChargePercent}%");
            _state = State.Flying;
        }

        public bool CanTakeOff
        {
            get 
            { 
                return _state == State.OnBase
                    && _battery.ChargePercent >= Battery.MinCharge; 
            } 
        }

        public override string ToString()
        {
            return $"{_number}: {StateStr}, заряд {BatteryCharge}%";
        }
    }
}












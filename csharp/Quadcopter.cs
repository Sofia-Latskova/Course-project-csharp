using System;

namespace Firewatch
{
    public class Quadcopter: IDiagnosable, IReportable
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
                throw new LowBatteryException(Battery.ChargePercent, Battery.MinCharge);
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
                throw new InvalidMissionStateException($"сканирование невозможно: дрон не в полёте (состояние: {StateStr})");
            }
            CurrentState = State.Scanning;
            Battery.Consume(2);
            Console.WriteLine($"[Quadcopter] {Number} снял показания, заряд={Battery.ChargePercent}%");
            if (Battery.ChargePercent < Battery.CriticalCharge)
            {
                CurrentState = State.Returning;
                throw new LowBatteryException(Battery.ChargePercent, Battery.CriticalCharge);
            }
            CurrentState = State.Flying;
        }

        public bool CanTakeOff => CurrentState == State.OnBase && Battery.ChargePercent >= Battery.MinCharge;

        public override string ToString()
        {
            return $"{Number}: {StateStr}, заряд {BatteryCharge}%";
        }

        public bool IsOperational => CurrentState == State.OnBase && Battery.ChargePercent >= Battery.MinCharge;
        public int HealthPercent => Battery.ChargePercent;
        public string ReportSection => "ДРОН";
        public string ToReportLine() => $"{Number};DRONE;{BatteryCharge}%;{StateStr}";

    }
}












using System;

namespace Firewatch
{
    public class Battery: IDisposable
    {
        public const int MinCharge = 20;
        public const int CriticalCharge = 10;
        public const int MaxPercent = 100;

        private int _chargePercent;
        private int _capasityPercent;
        private readonly string _serial;

        public Battery()
        {
            _chargePercent = MaxPercent;
            _capasityPercent = MaxPercent;
            _serial = "BAT-000";
            Console.WriteLine($"[Battery] Создан {_serial} (по умолчанию)");
        }
       
        public Battery(int chargePercent, int capacityPercent, string serial)
        {
            _capasityPercent = capacityPercent;
            _serial = serial;  
            ChargePercent = chargePercent; 
            Console.WriteLine($"[Battery] Создан {_serial} заряд={_chargePercent}%");
        }

        public int ChargePercent
        {
            get { return _chargePercent; }
            private set
            {
                if (value < 0)
                {
                    Console.WriteLine("[Battery] Некорректный заряд (меньше 0), сброшен в 0");
                    _chargePercent = 0;
                }
                else if (value > _capasityPercent)
                {
                    Console.WriteLine($"[Battery] Некорректный заряд (больше ёмкости), установлен {_capasityPercent}%");
                    _chargePercent = _capasityPercent;
                }
                else
                {
                    _chargePercent = value;
                }
            }
        }

        public int CapasityPercent
        {
            get { return _capasityPercent; }
        }

        public string Serial
        {
            get { return _serial; }
        }

        public void Consume(int percent)
        {
            if (percent <= 0) return;
            ChargePercent -= percent;
            Console.WriteLine($"[Battery] {_serial} разряжен до {ChargePercent}%");

        }

        public void Charge(int percent)
        {
            if (percent <= 0) return;
            ChargePercent += percent;
            Console.WriteLine($"[Battery] {_serial} заряжен до {ChargePercent}%");
        }

        public bool IsValid
        {
            get { return _chargePercent >= 0 && _chargePercent <= _capasityPercent; }
        }

        public override string ToString()
        {
            return $"{_serial}: заряд {_chargePercent}%";
        }

        public void Dispose()
        {
            Console.WriteLine($"[Battery] Сеанс с {_serial} завершён (Dispose)");
        }
    }

}






























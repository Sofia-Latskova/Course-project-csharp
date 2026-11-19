using System;

namespace Firewatch
{
    public class Battery: IDisposable
    {
        public const int MinCharge = 20;
        public const int CriticalCharge = 10;
        public const int MaxPercent = 100;

        public int ChargePercent { get; private set; }
        public int CapacityPercent { get; }
        public string Serial { get; }

        public Battery()
        {
            ChargePercent = MaxPercent;
            CapacityPercent = MaxPercent;
            Serial = "BAT-000";
            Console.WriteLine($"[Battery] Создан {Serial} (по умолчанию)");
        }
       
        public Battery(int chargePercent, int capacityPercent, string serial)
        {
            CapacityPercent = capacityPercent;
            Serial = serial;
            SetCharge(chargePercent);
            Console.WriteLine($"[Battery] Создан {Serial} заряд={ChargePercent}%");
        }

        public Battery(Battery other)
        {
            ChargePercent = other.ChargePercent;
            CapacityPercent = other.CapacityPercent;
            Serial = other.Serial;
            Console.WriteLine($"[Battery] Копия: {Serial} заряд={ChargePercent}%");
        }

      
       private void SetCharge(int value) 
       {
          if (value < 0)
          {
             Console.WriteLine("[Battery] Некорректный заряд (меньше 0), сброшен в 0");
             ChargePercent = 0;
          }
          else if (value > CapacityPercent)
          {
             Console.WriteLine($"[Battery] Некорректный заряд (больше ёмкости), установлен {CapacityPercent}%");
             ChargePercent = CapacityPercent;
          }
          else
          {
             ChargePercent = value;
          }     
       }
                 


        public void Consume(int percent)
        {
            if (percent <= 0) return;
            SetCharge(ChargePercent - percent);
            Console.WriteLine($"[Battery] {Serial} разряжен до {ChargePercent}%");

        }

        public void Charge(int percent)
        {
            if (percent <= 0) return;
            SetCharge(ChargePercent + percent);
            Console.WriteLine($"[Battery] {Serial} заряжен до {ChargePercent}%");
        }

        public bool IsValid=> ChargePercent >= 0 && ChargePercent <= CapacityPercent;

        public override string ToString()
        {
            return $"{Serial}: заряд {ChargePercent}%";
        }

        public void Dispose()
        {
            Console.WriteLine($"[Battery] Сеанс с {Serial} завершён (Dispose)");
        }
    }

}






























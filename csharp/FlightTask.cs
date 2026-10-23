using System;

namespace Firewatch
{
    public class FlightTask
    {
        public enum Status
        {
            Created,
            InProgress,
            Completed,
            Aborted
        }

        private readonly string _zoneName;
        private Status _status;
        private Quadcopter _drone;   

        // Конструктор по умолчанию
        public FlightTask()
        {
            _zoneName = "ZONE-0";
            _status = Status.Created;
            _drone = null;
            Console.WriteLine($"[FlightTask] Создано задание для зоны {_zoneName}");
        }

        public FlightTask(string zoneName, Quadcopter drone)
        {
            _zoneName = zoneName;
            _status = Status.Created;
            _drone = drone;
            Console.WriteLine($"[FlightTask] Создано задание для зоны {_zoneName}");
        }

        // Свойства
        public string ZoneName
        {
            get { return _zoneName; }
        }

        public Status CurrentStatus
        {
            get { return _status; }
        }

        public string StatusStr
        {
            get
            {
                switch (_status)
                {
                    case Status.Created: return "создано";
                    case Status.InProgress: return "выполняется";
                    case Status.Completed: return "завершено";
                    case Status.Aborted: return "прервано";
                    default: return "неизвестно";
                }
            }
        }

        public bool Start()
        {
            if (!CanStart)
            {
                Console.WriteLine("[FlightTask] Отказ запуска: дрон не готов или занят");
                return false;
            }
            if (!_drone.TakeOff())
            {
                Console.WriteLine("[FlightTask] Отказ запуска: дрон не смог взлететь");
                return false;
            }
            _status = Status.InProgress;
            Console.WriteLine($"[FlightTask] Задание в зоне {_zoneName} начато");
            return true;
        }

        public void Abort(string reason)
        {
            if (_status != Status.InProgress)
            {
                Console.WriteLine("[FlightTask] Прервать можно только выполняемое задание");
                return;
            }
            _drone.Land();
            _status = Status.Aborted;
            Console.WriteLine($"[FlightTask] Задание прервано. Причина: {reason}");
        }

        public void Complete()
        {
            if (_status != Status.InProgress)
            {
                Console.WriteLine("[FlightTask] Завершить можно только выполняемое задание");
                return;
            }
            _drone.Land();
            _status = Status.Completed;
            Console.WriteLine($"[FlightTask] Задание в зоне {_zoneName} завершено");
        }

        public bool CanStart
        {
            get
            { 
               if(_drone == null) return false;
               if (_drone.CurrentState != Quadcopter.State.OnBase) return false;
               return _status == Status.Created;
            }
        }

        public override string ToString()
        {
            return $"Задание {_zoneName}: {StatusStr}";
        }





    }
}
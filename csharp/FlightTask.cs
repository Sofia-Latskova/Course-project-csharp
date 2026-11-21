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
        private IAlertPolicy? _policy;

        // Конструктор по умолчанию
        public FlightTask()
        {
            _zoneName = "ZONE-0";
            _status = Status.Created;
            _drone = null;
            Console.WriteLine($"[FlightTask] Создано задание для зоны {_zoneName}");
        }

        public FlightTask(string zoneName, Quadcopter drone, IAlertPolicy policy)
        {
            _zoneName = zoneName;
            _status = Status.Created;
            _drone = drone;
            _policy = policy ?? throw new InvalidMissionStateException("политика сигнализации не задана");
            Console.WriteLine($"[FlightTask] Создано задание для зоны {_zoneName} (политика: {_policy.AlertChannel})\");");
        }
        public void SetPolicy(IAlertPolicy policy)
        {
            _policy = policy ?? throw new InvalidMissionStateException("нельзя установить пустую политику");
            Console.WriteLine($"[FlightTask] Политика заменена на: {_policy.AlertChannel}");
        }
        public (string Channel, string Message) TriggerAlert(ScanResult scan)
        {
            if (_policy == null) throw new InvalidMissionStateException("политика сигнализации не задана");
            return (_policy.AlertChannel, _policy.FormatAlert(scan));
        }


        // Свойства
        public string ZoneName => _zoneName; 
        public Status CurrentStatus => _status; 
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
                throw new InvalidMissionStateException("нельзя начать: дрон не готов или задание не в статусе 'создано'");
            }
            _drone.TakeOff();
            _status = Status.InProgress;
            Console.WriteLine($"[FlightTask] Задание в зоне {_zoneName} начато");
            return true;
        }

        public void Abort(string reason)
        {
            if (_status != Status.InProgress)
            {
                throw new InvalidMissionStateException($"прервать можно только выполняемое задание (текущий статус: {StatusStr})");
            }
            _drone.Land();
            _status = Status.Aborted;
            Console.WriteLine($"[FlightTask] Задание прервано. Причина: {reason}");
        }

        public void Complete()
        {
            if (_status != Status.InProgress)
            {
                throw new InvalidMissionStateException($"завершить можно только выполняемое задание (текущий статус: {StatusStr})");
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

        public int CompareTo(FlightTask? other)
        {
            if (other is null) return 1;

            // Сначала сравниваем по статусу ( активные/новые выше завершённых)
            int statusComparison = _status.CompareTo(other._status);
            if (statusComparison != 0) return statusComparison;

            // Если статусы одинаковые — сортируем по названию зоны по алфавиту
            return string.Compare(_zoneName, other._zoneName, StringComparison.Ordinal);
        }



    }
}
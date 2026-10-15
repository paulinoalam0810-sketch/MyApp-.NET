using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Domain
{
    public class VisitEntity
    {
        public Guid Id { get; private set; }
        public Guid PersonId { get; private set; }
        public DateTime EntryTime { get; private set; }
        public DateTime? ExitTime { get; private set; }
        public PersonEntity? Person { get; private set; }

        public bool IsActive => ExitTime == null;
        public TimeSpan? Duration => ExitTime.HasValue ? ExitTime.Value - EntryTime : null;

        private VisitEntity() { } // Private constructor for EF Core

        public VisitEntity(Guid personId, DateTime? entryTime = null)
        {
            if (personId == Guid.Empty)
            {
                throw new ArgumentException("PersonId cannot be empty.", nameof(personId));
            }

            Id = Guid.NewGuid();  
            PersonId = personId;
            EntryTime = entryTime ?? DateTime.UtcNow;
            ExitTime = null;
        }

        public void RegisterExit(DateTime? exitTime = null)
        { 
            var exit = exitTime ?? DateTime.UtcNow;

            if (ExitTime.HasValue)
            { 
                throw new InvalidOperationException("Exit time has already been registered.");
            }

            if (exit <= EntryTime)
            {
                throw new ArgumentException("Exit time cannot be before entry time.", nameof(exitTime));
            }

            ExitTime = exit;
        }

    }
}

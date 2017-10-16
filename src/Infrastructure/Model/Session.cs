using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISTS.Infrastructure.Model
{
    [Table("Session")]
    public class Session
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public Guid Id { get; set; }

        public Guid RoomId { get; set; }

        public virtual Room Room { get; set; }

        public DateTime? ScheduledStartTime { get; set; }

        public DateTime? ScheduledEndTime { get; set; }

        public DateTime? ActualStartTime { get; set; }

        public DateTime? ActualEndTime { get; set; }
    }
}
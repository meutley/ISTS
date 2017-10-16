using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISTS.Infrastructure.Model
{
    [Table("Room")]
    public class Room
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public Guid Id { get; set; }

        public Guid StudioId { get; set; }

        public virtual Studio Studio { get; set; }

        public virtual ICollection<Session> Sessions { get; set; }
    }
}
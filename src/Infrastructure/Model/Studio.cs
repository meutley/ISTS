using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISTS.Infrastructure.Model
{
    [Table("Studio")]
    public class Studio
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string FriendlyUrl { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevPractice.Domain.Core
{
    [Table("verbose")]
    public class Verbose
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
    }
}

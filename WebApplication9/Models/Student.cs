using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication9.Models
{
    public class Student
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Age { get; set; }
        public string ImageUrl { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; } // Modify this property

        public string Class { get; set; }
        [Required]
        public int RollNumber { get; set; }
    }
     public class StudentListViewModel
    {
        public List<Student> Students { get; set; }
        public string ClassName { get; set; }
        public string SearchString { get; set; }
    }

}

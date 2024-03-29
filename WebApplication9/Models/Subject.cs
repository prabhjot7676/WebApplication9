using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication9.Models
{
    public class Language
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class Subject
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Class { get; set; }
       
        public ICollection<Language> Languages { get; set; }
        [NotMapped]
        public ICollection<SubjectTeacher> SubjectTeachers { get; set; }

    }
    public class AddSubjectViewModel
    {
        public Subject Subject { get; set; }
        public List<Teacher> AvailableTeachers { get; set; }
        public List<int> SelectedTeacherIds { get; set; }
        public List<SelectListItem> LanguageOptions { get; set; }
    }

}

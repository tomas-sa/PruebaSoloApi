using PruebaSoloApi.Entities;

namespace PruebaSoloApi.DTOs
{
    public record struct StudentWithCoursesDTO(string Name, List<CourseDTO> Courses);
}

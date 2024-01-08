namespace PruebaSoloApi.Entities
{
    public class Teacher
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}

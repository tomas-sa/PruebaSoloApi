﻿namespace PruebaSoloApi.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Teacher Teacher { get; set; }
        public ICollection<Student> Students { get; set; }
    }
}

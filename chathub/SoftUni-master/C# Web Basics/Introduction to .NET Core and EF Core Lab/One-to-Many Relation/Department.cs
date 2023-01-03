﻿namespace One_to_Many_Relation
{
    using System.Collections.Generic;

    using System.ComponentModel.DataAnnotations;

    public class Department
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public int EmployeeId { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}

﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Project_Manager.Model
{
    public class Group : UserSection
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        //foreign key to organization
        public Guid OrganizationId { get; set; }

        public ICollection<Project>? Projects { get; set; }

        public ICollection<GroupUser>? GroupUsers { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using TestTaskManager.Model.Enums;

namespace TestTaskManager.Model
{
    public class UserTask
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public Status Status { get; set; }

        public Priority Priority { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}

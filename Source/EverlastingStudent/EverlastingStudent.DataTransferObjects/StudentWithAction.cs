namespace EverlastingStudent.DataTransferObjects
{
    using System.Collections.Generic;

    public class StudentWithAction<T>
    {
        public StudentWithAction()
        {
            this.Action = new List<T>();
        }

        public int Energy { get; set; }

        public long Money { get; set; }

        public long Experience { get; set; }

        public long Knowledge { get; set; }

        public string CourseName { get; set; }

        public IEnumerable<T> Action { get; set; }
    }
}

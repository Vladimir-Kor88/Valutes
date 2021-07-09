using System;
using System.Collections.Generic;

#nullable disable

namespace Valuta
{
    public partial class Valute
    {
        public Valute()
        {
            DateCourses = new HashSet<DateCourse>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<DateCourse> DateCourses { get; set; }
    }
}

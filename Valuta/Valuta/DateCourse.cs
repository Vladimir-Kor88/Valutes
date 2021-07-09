using System;
using System.Collections.Generic;

#nullable disable

namespace Valuta
{
    public partial class DateCourse
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string ValuteId { get; set; }
        public decimal Value { get; set; }

        public virtual Valute Valute { get; set; }
    }
}

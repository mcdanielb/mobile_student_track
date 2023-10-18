using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bmcdani_c971_task
{
    class CustomDatePicker
    {
        public static DatePicker CreateDatePicker()
        {
            return new DatePicker
            {
                MinimumDate = new DateTime(2018, 1, 1),
                MaximumDate = new DateTime(2030, 12, 31),
                Date = new DateTime(2023, 10, 18)
            };
        }
    }
}

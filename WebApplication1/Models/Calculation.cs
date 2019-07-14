using System;

namespace WebApplication1.Models
{
    public class Calculation
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Expression { get; set; }
        public double Result { get; set; }
    }
}
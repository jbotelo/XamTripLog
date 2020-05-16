using System;
using System.Collections.Generic;
using System.Text;

namespace TripLog.Models
{
    public class EntryNotProvidedException : Exception
    {
        public EntryNotProvidedException()
            : base("An Entry object was not provided. If using DetailViewModel," +
                  "be sure to use the Init overload that takes an Entry parameter.")
        {
        }
    }
}

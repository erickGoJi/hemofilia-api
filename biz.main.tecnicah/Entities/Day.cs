﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace biz.main.tecnicah.Entities
{
    public partial class Day
    {
        public Day()
        {
            Calendars = new HashSet<Calendar>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Calendar> Calendars { get; set; }
    }
}
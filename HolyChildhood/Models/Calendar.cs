﻿using System;
using System.Collections.Generic;

namespace HolyChildhood.Models
{
    public class Calendar
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Event> Events { get; set; }
    }

    public class Event
    {
        public int Id { get; set; }
        public Calendar Calendar { get; set; }
        public string Title { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Notes { get; set; }
        public bool AllDay { get; set; }
        public bool Repeats { get; set; }
    }
}
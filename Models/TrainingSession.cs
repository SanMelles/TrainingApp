﻿using SQLite;
using System.Collections.ObjectModel;


namespace TrainingApp.Models
{
    public class TrainingSession
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }
}

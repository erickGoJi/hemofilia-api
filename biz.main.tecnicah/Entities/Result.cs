﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace biz.main.tecnicah.Entities
{
    public partial class Result
    {
        public int Id { get; set; }
        public int? QuizId { get; set; }
        public int? UserId { get; set; }
        public int? OptionId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? Status { get; set; }

        public virtual CatOptionDiagnostic Option { get; set; }
        public virtual Quiz Quiz { get; set; }
        public virtual User User { get; set; }
    }
}
﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace biz.main.tecnicah.Entities
{
    public partial class CatRole
    {
        public CatRole()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Role { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? Status { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
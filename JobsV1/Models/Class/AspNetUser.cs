﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobsV1.Models.Class
{
    public class AspUser
    {
        [Key]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }

        public List<AspRoles> aspRoles { get; set; }
    }

    public class AspRoles
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
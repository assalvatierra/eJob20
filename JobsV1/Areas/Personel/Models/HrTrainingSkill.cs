//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JobsV1.Areas.Personel.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class HrTrainingSkill
    {
        public int Id { get; set; }
        public int HrTrainingId { get; set; }
        public int HrSkillId { get; set; }
        public int HrProficiencyId { get; set; }
    
        public virtual HrTraining HrTraining { get; set; }
        public virtual HrSkill HrSkill { get; set; }
        public virtual HrProficiency HrProficiency { get; set; }
    }
}

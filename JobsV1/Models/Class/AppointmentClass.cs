using JobsV1.Areas.AutoCare.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobsV1.Models.Class
{
    public class ApmntSchedule
    {
        [Key]
        public List<ApmntItemSchedule> appointments { get; set; }
        public List<ApmntDateLabel> dateLabels { get; set; }
    }

    public class ApmntItemSchedule
    {
        [Key]
        public AppointmentSlot slot { get; set; }
        public List<ApmntDaysStatus> apDaysStatuses { get; set; }
    }

    public class ApmntDaysStatus
    {
        [Key]
        public List<Appointment> appointments { get; set; }
        public DateTime date { get; set; }
        public bool status { get; set; }
    }

    public class ApmntDateLabel
    {
        public string Day { get; set; }
        public string DateDay { get; set; }
        public string DateMonth { get; set; }
    }
  
    public class AppointmentClass
    {
        public DateClass dateClass = new DateClass();
        public AppointmentDBContainer db = new AppointmentDBContainer();

        public ApmntSchedule GetAppoinmentSchedules()
        {
            ApmntSchedule apSchedules = new ApmntSchedule();
            List<ApmntItemSchedule> rsvdSchedule = new List<ApmntItemSchedule>();

            var today = dateClass.GetCurrentDate();
            var dateInverval = 7;

            var appointmentList = db.Appointments.Where(a => a.AppointmentStatusId < 3).ToList();

            var apppointmenSlots = db.AppointmentSlots.ToList();

            foreach (var slot in apppointmenSlots)
            {

                ApmntItemSchedule tempSchedule = new ApmntItemSchedule();
                tempSchedule.slot = slot;
                tempSchedule.apDaysStatuses = new List<ApmntDaysStatus>();

                var appoinmentsOnSlot = appointmentList.Where(s => s.AppointmentSlotId == slot.Id).ToList();
                for (var i = 0; i <= dateInverval; i++)
                {
                    ApmntDaysStatus daysStatus = new ApmntDaysStatus();
                    daysStatus.date = today.AddDays(i);
                    daysStatus.status = false;

                    if (appoinmentsOnSlot == null)
                    {
                        daysStatus.appointments = new List<Appointment>();
                    }
                    else
                    {
                        if (daysStatus.appointments == null)
                        {
                            daysStatus.appointments = new List<Appointment>();
                        }

                        foreach (var appointment in appoinmentsOnSlot)
                        {
                            var appointmentdate = DateTime.Parse(appointment.AppointmentDate);
                            if (daysStatus.date == appointmentdate)
                            {
                                daysStatus.appointments.Add(appointment);
                                daysStatus.status = true;
                            }
                        }
                    }

                    tempSchedule.apDaysStatuses.Add(daysStatus);
                }

                rsvdSchedule.Add(tempSchedule);
            }


            apSchedules.appointments = rsvdSchedule;
            apSchedules.dateLabels = GetApmntDateLabel();

            return apSchedules;
        }

        public List<ApmntDateLabel> GetApmntDateLabel()
        {
            List<ApmntDateLabel> dateLabels = new List<ApmntDateLabel>();
            var today = dateClass.GetCurrentDate();
            var dateInverval = 7;

            for (var i = 0; i <= dateInverval; i++ )
            {
                var varDate = today.AddDays(i);
                dateLabels.Add(new ApmntDateLabel
                {
                    DateMonth = varDate.Date.ToString("MMM"),
                    DateDay = varDate.Date.ToString("ddd"),
                    Day = varDate.Date.ToString("dd")
                });
            }

            return dateLabels;
        }
    }
}
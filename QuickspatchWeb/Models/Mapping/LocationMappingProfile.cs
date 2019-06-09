using System;
using System.Security.Cryptography;
using AutoMapper;
using Framework.Mapping;
using Framework.Utility;
using QuickspatchWeb.Models.Location;

namespace QuickspatchWeb.Models.Mapping
{
    public class LocationMappingProfile:Profile
    {
        protected override void Configure()
        {
            //
            Mapper.CreateMap<Framework.DomainModel.Entities.Location, DashboardLocationDataViewModel>().AfterMap((s, d) =>
            {
                d.SharedViewModel = s.MapTo<DashboardLocationShareViewModel>();
            });
            //
            Mapper.CreateMap<Framework.DomainModel.Entities.Location, DashboardLocationShareViewModel>()
                .ForMember(d => d.OpenHour, opt => opt.Ignore())
                .ForMember(d => d.CloseHour, opt => opt.Ignore())
                .AfterMap(
                (s, d) =>
                {
                    //
                    if (s.OpenHour != null)
                    {
                        int mins = 0;
                        int h = 0;
                        int m = 0;
                        

                        mins = (int)s.OpenHour;
                        h = mins/60;
                        m = mins%60;
                       
                       //string time = h.ToString("00") + ':' + m.ToString("00");
                       var openHour = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, h, m, 0);

                       d.OpenHour = openHour;
                    }
                    else
                    {
                        d.OpenHour = null;
                    }

                    if (s.CloseHour != null)
                    {
                        int mins = 0;
                        int h = 0;
                        int m = 0;

                        mins = (int)s.CloseHour;
                        h = mins / 60;
                        m = mins % 60;

                        //string time = h.ToString("00") + ':' + m.ToString("00");
                        var closeHour = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, h, m, 0);
                        d.CloseHour = closeHour;
                    }
                    else
                    {
                        d.CloseHour = null;
                    }
                    //
                    if (s.AvailableTime != null && s.AvailableTime.Length == 7)
                    {
                        for (int j = 0; j < s.AvailableTime.Length; j++)
                        {
                            if (s.AvailableTime[j] == 1)
                            {
                                switch (j)
                                {

                                    case 0:
                                        d.Monday=true;
                                        break;
                                    case 1:
                                        d.Tuesday = true;
                                        break;
                                    case 2:
                                        d.Wednesday = true;
                                        break;
                                    case 3:
                                        d.Thursday = true;
                                        break;
                                    case 4:
                                        d.Friday = true;
                                        break;
                                    case 5:
                                        d.Saturday = true;
                                        break;
                                    case 6:
                                        d.Sunday = true;
                                        break;
                                }
                            }
                        }
                    }
                });


            
            Mapper.CreateMap<DashboardLocationDataViewModel, Framework.DomainModel.Entities.Location>().AfterMap((s, d) =>
            {
                d = s.SharedViewModel.MapPropertiesToInstance(d);
            });
            

            Mapper.CreateMap<DashboardLocationShareViewModel, Framework.DomainModel.Entities.Location>()
                .ForMember(d => d.OpenHour, opt => opt.Ignore())
                .ForMember(d => d.CloseHour, opt => opt.Ignore())
                .AfterMap((s, d) =>
                {
                    
                    
                    if (s.OpenHour!=null)
                    {
                        var openhour = ((DateTime) s.OpenHour).ToClientTimeDateTime();
                        var minutesopen = (int) (openhour.Hour*60 + openhour.Minute);

                        d.OpenHour = minutesopen;
                    }
                    else
                    {
                        d.OpenHour = null;
                    }

                    if (s.CloseHour != null)
                    {
                        var closehour = ((DateTime)s.CloseHour).ToClientTimeDateTime();                        
                        var minutesclose = (int)(closehour.Hour * 60 + closehour.Minute);

                        d.CloseHour = minutesclose;
                    }
                    else
                    {
                        d.CloseHour = null;
                    }

                    var availableTime = new byte[7];
                    availableTime[0] = (byte)(s.Monday ? 1 : 0);
                    availableTime[1] = (byte)(s.Tuesday ? 1 : 0);
                    availableTime[2] = (byte)(s.Wednesday ? 1 : 0);
                    availableTime[3] = (byte)(s.Thursday ? 1 : 0);
                    availableTime[4] = (byte)(s.Friday ? 1 : 0);
                    availableTime[5] = (byte)(s.Saturday ? 1 : 0);
                    availableTime[6] = (byte)(s.Sunday ? 1 : 0);
                    d.AvailableTime = availableTime;

                });


        }
    }
}
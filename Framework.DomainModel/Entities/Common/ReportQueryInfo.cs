﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DomainModel.Entities.Common
{
    public class ReportQueryInfo : QueryInfo
    {
        //report for driver
        public int CourierId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}

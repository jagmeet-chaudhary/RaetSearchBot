﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchBot.Model
{
    public class OdataAuditContextDto
    {
        public IList<AuditChangeContextDto> Items { get; set; }
        public int Count { get; set; }
    }
}

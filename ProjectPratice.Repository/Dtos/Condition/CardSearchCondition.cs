﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPratice.Repository.Dtos.Condition
{
    // ProjectN.Repository.Dtos.Condition
    public class CardSearchCondition
    {
        public string Name { get; set; }
        public int? MinAttack { get; set; }
        public int? MaxAttack { get; set; }
        public int? MinHealth { get; set; }
        public int? MaxHealth { get; set; }
        public int? MinCost { get; set; }
        public int? MaxCost { get; set; }
    }
}

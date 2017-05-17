﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta_Project.DAL
{
    public class InputConfiguration: BaseEntityConfiguration<Input>
    {
        public InputConfiguration()
        {
            Property(p => p.PatientAge).IsRequired();
            Property(p => p.Density).IsRequired();
            Property(p => p.ImageStdDev).IsRequired();
            Property(p => p.ImageMedian).IsRequired();
            Property(p => p.ImageMean).IsRequired();
            Property(p => p.ImageSkew).IsRequired();
            Property(p => p.ImageKurt).IsRequired();
            Property(p => p.ImagePath).IsRequired().HasMaxLength(255);
        }
    }
}

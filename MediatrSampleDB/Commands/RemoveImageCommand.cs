﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatrSampleDB.Commands
{
    public class RemoveImageCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
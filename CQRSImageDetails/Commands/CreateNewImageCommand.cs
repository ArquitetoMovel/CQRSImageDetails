﻿using MediatR;
using CQRSImageDetails.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CQRSImageDetails.Commands
{
    public class CreateNewImageCommand : ICommand
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }
}
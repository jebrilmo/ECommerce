using Infrastructure.Enums;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Classes
{
    public class OperationResult<DataType> : IDTO
    {
        public OperationStatus Status { get; set; }
        public DataType Data { get; set; }
        public List<string>Errors { get; set; }
    }
}

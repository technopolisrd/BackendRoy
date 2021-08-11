using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Entities.Tables.DTO
{
    public class ResponseDTO<TEntity>
    {
        public string status { get; set; }
        public string message { get; set; }
        public TEntity data { get; set; }
    }
}

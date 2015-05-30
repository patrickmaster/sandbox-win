using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Sandbox.Contracts;
using Sandbox.Contracts.Types;
using Sandbox.Contracts.Types.Code;

namespace Sandbox.WebApi.Models
{
    public class Input
    {
        [Required]
        public PlatformType Platform { get; set; }

        public string PackageName { get; set; }
        
        public IEnumerable<string> Libraries { get; set; }

        [Required]
        public VariableType ReturnType { get; set; }

        [Required]
        public string Code { get; set; }

    }
}
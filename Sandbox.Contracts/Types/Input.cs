using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Sandbox.Contracts.MySql;
using Sandbox.Contracts.Types.Code;

namespace Sandbox.Contracts.Types
{
    public class Input
    {
        [Required]
        public PlatformType Platform { get; set; }

        public IEnumerable<Library> Libraries { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public VariableType ReturnType { get; set; }

        public bool UseWrapper { get; set; }
    }
}
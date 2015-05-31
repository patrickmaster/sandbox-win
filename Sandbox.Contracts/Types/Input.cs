using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sandbox.Contracts.Types
{
    public class Input
    {
        [Required]
        public PlatformType Platform { get; set; }

        public IEnumerable<string> Libraries { get; set; }

        [Required]
        public string Code { get; set; }

    }
}
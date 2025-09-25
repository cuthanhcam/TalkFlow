using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkFlow.Application.DTOs.User
{
    public class StrangerFilterDto
    {
        public ICollection<string> FindGender { get; set; } = new List<string>();
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        public ICollection<string> FindRegion { get; set; } = new List<string>();
    }
}

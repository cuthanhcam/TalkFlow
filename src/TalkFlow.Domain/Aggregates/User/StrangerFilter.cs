using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkFlow.Domain.Aggregates.User
{
    public class StrangerFilter // entity
    {
        public Guid Id { get; private set; }
        public ICollection<string> FindGender { get; private set; }
        public int MinAge { get; private set; }
        public int MaxAge { get; private set; }
        public ICollection<string> FindRegion { get; private set; }
        public Guid? CurrentRoomId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        protected StrangerFilter()
        {
            FindGender = new List<string>();
            FindRegion = new List<string>();
        }

        public StrangerFilter(
            ICollection<string> findGender,
            int minAge,
            int maxAge,
            ICollection<string> findRegion)
        {
            Id = Guid.NewGuid();
            FindGender = findGender ?? new List<string>();
            MinAge = Math.Max(0, minAge);
            MaxAge = Math.Min(100, maxAge);
            FindRegion = findRegion ?? new List<string>();
            CreatedAt = DateTime.UtcNow;

            ValidateAgeRange();
        }

        public void UpdateFilter(
            ICollection<string> findGender,
            int minAge,
            int maxAge,
            ICollection<string> findRegion)
        {
            FindGender = findGender ?? new List<string>();
            MinAge = Math.Max(0, minAge);
            MaxAge = Math.Min(100, maxAge);
            FindRegion = findRegion ?? new List<string>();
            UpdatedAt = DateTime.UtcNow;

            ValidateAgeRange();
        }

        public void SetCurrentRoom(Guid? roomId)
        {
            CurrentRoomId = roomId;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ClearCurrentRoom()
        {
            CurrentRoomId = null;
            UpdatedAt = DateTime.UtcNow;
        }

        public bool Matches(User user)
        {
            if (user.Gender != null && FindGender.Any() && !FindGender.Contains(user.Gender.Value, StringComparer.OrdinalIgnoreCase))
                return false;

            if (user.Age != null)
            {
                if (user.Age.Value < MinAge || user.Age.Value > MaxAge)
                    return false;
            }

            if (user.Nationality != null && FindRegion.Any() && !FindRegion.Contains(user.Nationality.Value, StringComparer.OrdinalIgnoreCase))
                return false;

            return true;
        }

        private void ValidateAgeRange()
        {
            if (MinAge > MaxAge)
                throw new ArgumentException("MinAge cannot be greater than MaxAge");
        }
    }
}

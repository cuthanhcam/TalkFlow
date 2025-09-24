using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Domain.ValueObjects.User;

namespace TalkFlow.Domain.Aggregates.User
{
    public class UserProfile // entity
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public Gender? Gender { get; private set; }
        public Age? Age { get; private set; }
        public Nationality? Nationality { get; private set; }
        public PhotoUrl PhotoUrl { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        protected UserProfile()
        {
            PhotoUrl = PhotoUrl.Empty;
        }

        public UserProfile(Guid userId, Gender? gender, Age? age, Nationality? nationality, PhotoUrl? photoUrl = null)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Gender = gender;
            Age = age;
            Nationality = nationality;
            PhotoUrl = photoUrl ?? PhotoUrl.Empty;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateProfile(Gender? gender, Age? age, Nationality? nationality, PhotoUrl? photoUrl = null)
        {
            Gender = gender;
            Age = age;
            Nationality = nationality;
            PhotoUrl = photoUrl ?? PhotoUrl;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdatePhoto(PhotoUrl photoUrl)
        {
            PhotoUrl = photoUrl;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}

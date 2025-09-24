using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkFlow.Domain.ValueObjects.User;
using TalkFlow.Domain.Events;
using TalkFlow.Domain.Aggregates.User.Events;
using Microsoft.AspNetCore.Identity;


namespace TalkFlow.Domain.Aggregates.User
{
    public class User : IdentityUser<Guid> // aggregate root
    {
        private readonly List<IDomainEvent> _domainEvents = new();

        public DisplayName DisplayName { get; private set; }
        public DateTime LastActive { get; private set; }
        public bool IsLocked { get; private set; }
        public PhotoUrl PhotoUrl { get; private set; }
        public Gender? Gender { get; private set; }
        public Age? Age { get; private set; }
        public Nationality? Nationality { get; private set; }
        public UserProfile? Profile { get; private set; }
        public StrangerFilter? StrangerFilter { get; private set; }

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected User()
        {
            DisplayName = new DisplayName("Default");
            PhotoUrl = PhotoUrl.Empty;
        }

        public User(DisplayName displayName, string? email = null) : base()
        {
            var userId = UserId.New();
            Id = userId.Value;
            DisplayName = displayName;
            LastActive = DateTime.UtcNow;
            IsLocked = false;
            PhotoUrl = PhotoUrl.Empty;
            UserName = userId.ToString();
            Email = email;
            EmailConfirmed = true; // Auto-confirm for anonymous users
        }

        public static User CreateAnonymous(DisplayName displayName)
        {
            var user = new User(displayName);
            user.AddDomainEvent(new UserCreatedEvent(new UserId(user.Id), user.DisplayName));
            return user;
        }

        public static User CreateWithProfile(DisplayName displayName, UserProfile profile, string? email = null)
        {
            var user = new User(displayName, email);
            user.Profile = profile;
            user.AddDomainEvent(new UserCreatedEvent(new UserId(user.Id), user.DisplayName));
            return user;
        }

        public void UpdateProfile(UserProfile profile)
        {
            Profile = profile;
            AddDomainEvent(new UserProfileUpdatedEvent(new UserId(Id), profile));
        }

        public void UpdateStrangerFilter(StrangerFilter filter)
        {
            StrangerFilter = filter;
        }

        public void UpdatePersonalInfo(Gender? gender, Age? age, Nationality? nationality)
        {
            Gender = gender;
            Age = age;
            Nationality = nationality;
        }

        public void UpdatePhoto(PhotoUrl photoUrl)
        {
            PhotoUrl = photoUrl;
        }

        public void Lock()
        {
            if (IsLocked) return;

            IsLocked = true;
            AddDomainEvent(new UserLockedEvent(new UserId(Id)));
        }

        public void Unlock()
        {
            if (!IsLocked) return;

            IsLocked = false;
        }

        public void UpdateLastActive()
        {
            LastActive = DateTime.UtcNow;
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        private void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
    }
}

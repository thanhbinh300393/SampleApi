using Sample.Common.CQRS.DomainEvents;
using Sample.Common.Exceptions;
using Sample.Common.Heplers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sample.Common.Domain;

namespace Sample.Domain.System.Users
{
    [Table("AspNetUsers")]
    public partial class User : IdentityUser, IEntity
    {
        [Key]
        [Dapper.Contrib.Extensions.ExplicitKey]
        public override string? Id { get; set; }

        public string? Address { get; set; }
        public string? FullName { get; set; }
        public long? UserGroupID { get; set; }
        public bool IsActive { get; set; }
        public bool? IsSystem { get; set; }

        public string? AvatarUrl { get; set; }
        public byte? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? PasswordChangeDate { get; set; }


        #region Base

        private List<IDomainEvent> _domainEvents;

        [NotMapped] public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents?.AsReadOnly();

        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents = _domainEvents ?? new List<IDomainEvent>();
            this._domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        protected static void CheckRule(IBusinessRule rule)
        {
            if (rule.IsBroken())
            {
                throw new BusinessRuleValidationException(rule);
            }
        }

        #endregion
    }

    public enum EMPUserTypes
    {
        Staff = 2,
        Employer = 3,
        JobSeeker = 4,
    }

    public enum ActivityStateEnum : byte
    {
        Active = 1,
        Locked = 2,
        PendingApproval = 3
    }

    public enum PasswordResetStatusEnum
    {
        NotRequest = 1,
        NotReview = 2,
        Changed = 3,
        MobileRequested = 4
    }

    public enum UserTypes
    {
        AdminSys = 1,
        AdminDep = 2,
        AdminDep2 = 3,
        AdminUnit = 4,
        Cadre = 5,
        Pupil = 6,
    }
}
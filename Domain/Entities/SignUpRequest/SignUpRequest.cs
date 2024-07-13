using Domain.Entities.Users;
using Domain.Shared;
using System;

namespace Domain.Entities.SignUpRequests
{
    public class SignUpRequest : Entity
    {
        private string MAX_COMMENT_LENGTH = "255";
        public string IamId { get; private set; }

        public UserEmail Email { get; private set; }
        public UserName Name { get; private set; }
        public UserNif Nif { get; private set; }
        public UserPhoneNumber PhoneNumber { get; private set; }
        public SignUpRequestStatus Status { get; private set; }
        public DateTime CreationTime { get; private set; }
        public DateTime ActionTime { get; private set; }
        public User ActionedBy { get; private set; }
        public string ActionComment { get; private set; }



        protected SignUpRequest()
        {
            // Required by EF!!!
        }

        public SignUpRequest(string iamId, string email, string name, string nif, string phoneNumber)
        {
            this.IamId = iamId;
            this.Email = new UserEmail(email);
            this.Name = new UserName(name);
            this.Nif = new UserNif(nif);
            this.PhoneNumber = new UserPhoneNumber(phoneNumber);
            this.Status = SignUpRequestStatus.Requested;
            this.CreationTime = DateTime.Now;
        }

        /// <summary>
        /// Once the Users approves/rejects - Improve
        /// </summary>
        /// <param name="actionedBy"></param>
        /// <param name="actionComment"></param>
        public void addAction(User actionedBy, string actionComment)
        {
            this.ActionedBy = actionedBy;
            this.ActionComment = actionComment;
            this.ActionTime = DateTime.Now;
        }


        /// <summary>
        /// Approves a SignUpRequest
        /// </summary>
        /// <param name="actionedBy"></param>
        /// <param name="actionComment"></param>
        public void Approve(User actionedBy, string actionComment)
        {
            this.Status = SignUpRequestStatus.Approved;
            this.addAction(actionedBy, actionComment);
        }

        /// <summary>
        /// Rejects a SignUpRequest
        /// </summary>
        /// <param name="actionedBy"></param>
        /// <param name="actionComment"></param>
        public void Reject(User actionedBy, string actionComment)
        {
            this.Status = SignUpRequestStatus.Rejected;
            this.addAction(actionedBy, actionComment);
        }
    }   
}

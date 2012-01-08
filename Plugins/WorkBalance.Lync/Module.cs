using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using CommunicatorAPI;
using Microsoft.Lync.Model;
using Microsoft.Win32;
using WorkBalance.Contracts;

namespace WorkBalance.Lync
{
    [Export(typeof(IPlugin))]
    public class Module : IPlugin, IPartImportsSatisfiedNotification, IDisposable
    {
        [Import]
        ITimer Timer { get; set; }

        private LyncClient _client;
        private bool _snapshotIsValid;
        private string _snapshotPersonalNote;
        private ContactAvailability _snapshotAvailability;

        #region Implementation of IPartImportsSatisfiedNotification

        public void OnImportsSatisfied()
        {
            Timer.StateChanged += HandleStateChanged;
        }

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            if (_snapshotIsValid && IsClientValid())
            {
                RestoreFromSnapshot();
            }
        }

        #endregion

        #region Methods

        private void HandleStateChanged(object sender, TimerStateChangedEventArgs e)
        {
            if (IsClientValid())
            {
                try
                {
                    switch (e.Transition)
                    {
                        case TimerStateTransition.ReadyToSprint:
                            CreateSnapshot();
                            var finishTime = DateTime.Now + Timer.SprintDuration;
                            var personalNote = string.Format("I'm working till {0} on: {1}",
                                                             finishTime.ToShortTimeString(), Timer.CurrentActivity.Name);
                            SetStatus(ContactAvailability.Busy, personalNote);
                            break;
                        case TimerStateTransition.SprintToReady:
                            RestoreFromSnapshot();
                            break;
                        case TimerStateTransition.SprintToHomeStraight:
                            break;
                        case TimerStateTransition.HomeStraightToReady:
                            RestoreFromSnapshot();
                            break;
                        case TimerStateTransition.HomeStraightToBreak:
                            RestoreFromSnapshot();
                            break;
                        case TimerStateTransition.BreakToReady:
                            break;
                        case TimerStateTransition.BreakToBreakOverrun:
                            break;
                        case TimerStateTransition.BreakOverrunToReady:
                            break;
                    }
                }
                // Catch NullReferenceException which is thrown when client get disconnected
                catch (NullReferenceException)
                {
                }
            }
        }

        private void CreateSnapshot()
        {
            _snapshotPersonalNote = (string)_client.Self.Contact.GetContactInformation(ContactInformationType.PersonalNote);
            _snapshotAvailability = (ContactAvailability)_client.Self.Contact.GetContactInformation(ContactInformationType.Availability);
            _snapshotIsValid = true;
        }

        private void RestoreFromSnapshot()
        {
            if (_snapshotIsValid)
            {
                SetStatus(_snapshotAvailability, _snapshotPersonalNote);
                _snapshotIsValid = false;
            }
        }

        private bool IsClientValid()
        {
            if (_client == null || _client.State == ClientState.Invalid)
            {
                try
                {
                    _client = null;
                    _client = LyncClient.GetClient();
                }
                catch (ClientNotFoundException)
                {
                }
            }
            return _client != null && _client.State == ClientState.SignedIn;
        }

        private void SetStatus(ContactAvailability availability, string personalNote)
        {
            var parameters = new Dictionary<PublishableContactInformationType, object>();
            parameters.Add(PublishableContactInformationType.Availability, availability);
            parameters.Add(PublishableContactInformationType.PersonalNote, personalNote);
            _client.Self.BeginPublishContactInformation(parameters, BeginPublishContactInformationCallback, _client.Self);
        }

        public void BeginPublishContactInformationCallback(IAsyncResult ar)
        {
            var self = (Self)ar.AsyncState;
            try
            {
                self.EndPublishContactInformation(ar);
            }
            catch (Exception)
            {
            }
        }

        #endregion
    }
}

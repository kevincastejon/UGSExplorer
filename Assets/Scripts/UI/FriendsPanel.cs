using System;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Friends;
using Unity.Services.Friends.Models;
using Unity.Services.Friends.Notifications;
using Unity.Services.Friends.Options;
using UnityEngine;
using UnityEngine.UI;

namespace KevinCastejon.MultiplayerAPIExplorer
{
    public class FriendsPanel : UIPanel
    {
        [SerializeField] private Button _initBtn;
        [SerializeField] private Button _forceRefreshBtn;
        [SerializeField] private Button _relationshipsBtn;
        [SerializeField] private Button _friendsBtn;
        [SerializeField] private Button _incomingRequestsBtn;
        [SerializeField] private Button _outgoingRequestsBtn;
        [SerializeField] private Button _blocksBtn;
        [SerializeField] private TMP_InputField _addFriendIDInput;
        [SerializeField] private Button _addFriendByIDBtn;
        [SerializeField] private TMP_InputField _addFriendNameInput;
        [SerializeField] private Button _addFriendByNameBtn;
        [SerializeField] private TMP_InputField _deleteFriendIDInput;
        [SerializeField] private Button _deleteFriendBtn;
        [SerializeField] private TMP_InputField _deleteIncomingRequestIDInput;
        [SerializeField] private Button _deleteIncomingRequestBtn;
        [SerializeField] private TMP_InputField _deleteOutgoingRequestIDInput;
        [SerializeField] private Button _deleteOutgoingRequestBtn;
        [SerializeField] private TMP_InputField _addBlockIDInput;
        [SerializeField] private Button _addBlockByIDBtn;
        [SerializeField] private TMP_InputField _deleteBlockIDInput;
        [SerializeField] private Button _deleteBlockBtn;
        [SerializeField] private TMP_InputField _deleteRelationshipIDInput;
        [SerializeField] private Button _deleteRelationshipBtn;
        [SerializeField] private TMP_InputField _sendMessageIDInput;
        [SerializeField] private TMP_InputField _sendMessageContentInput;
        [SerializeField] private Button _sendMessageBtn;

        private static FriendsPanel _instance;
        public static FriendsPanel Instance { get => _instance; }

        protected override void Awake()
        {
            _instance = this;
            _initBtn.onClick.AddListener(Initialize);
            _forceRefreshBtn.onClick.AddListener(ForceRefresh);
            _relationshipsBtn.onClick.AddListener(GetRelationship);
            _friendsBtn.onClick.AddListener(GetFriends);
            _incomingRequestsBtn.onClick.AddListener(GetIncomingRequests);
            _outgoingRequestsBtn.onClick.AddListener(GetOutgoingRequests);
            _blocksBtn.onClick.AddListener(GetBlocks);
            _addFriendByIDBtn.onClick.AddListener(AddFriendByID);
            _addFriendByNameBtn.onClick.AddListener(AddFriendByName);
            _deleteFriendBtn.onClick.AddListener(DeleteFriend);
            _deleteIncomingRequestBtn.onClick.AddListener(DeleteIncomingRequest);
            _deleteOutgoingRequestBtn.onClick.AddListener(DeleteOutgoingRequest);
            _addBlockByIDBtn.onClick.AddListener(AddBlock);
            _deleteBlockBtn.onClick.AddListener(DeleteBlock);
            _deleteRelationshipBtn.onClick.AddListener(DeleteRelationship);
            _sendMessageBtn.onClick.AddListener(SendMsg);
            base.Awake();
        }

        private async void Initialize()
        {
            StartWait();
            Log("Initializing Friends service...");
            try
            {
                InitializeOptions opts = new();
                opts.WithMemberProfile(true);
                opts.WithMemberPresence(true);
                opts.WithEvents(true);
                await FriendsService.Instance.InitializeAsync(opts);
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Initializing Friend service failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Friend service initialized.");
            FriendsService.Instance.RelationshipAdded += RelationshipAdded;
            FriendsService.Instance.RelationshipDeleted += RelationshipDeleted;
            FriendsService.Instance.PresenceUpdated += PresenceUpdated;
            FriendsService.Instance.MessageReceived += MessageReceived;
        }

        private void RelationshipAdded(IRelationshipAddedEvent evt)
        {
            Log("Relationship added : ID:" + evt.Relationship.Id + "    Type:" + evt.Relationship.Type + MemberToString(evt.Relationship.Member));
        }

        private void RelationshipDeleted(IRelationshipDeletedEvent evt)
        {
            Log("Relationship deleted : ID:" + evt.Relationship.Id + "    Type:" + evt.Relationship.Type + MemberToString(evt.Relationship.Member));
        }

        private void PresenceUpdated(IPresenceUpdatedEvent evt)
        {
            Log("Presence updated : MemberID:" + evt.ID + " Availability:" + evt.Presence.Availability + " LastSeen" + (evt.Presence.LastSeen - DateTime.Now).Hours + "h]");
        }
        private void MessageReceived(IMessageReceivedEvent evt)
        {
            Log("Message received from friend " + evt.UserId + ":");
            Log(string.Join("", evt.GetAs<List<char>>()));
        }

        private async void ForceRefresh()
        {
            StartWait();
            Log("Forcing relationships refresh...");
            try
            {
                await FriendsService.Instance.ForceRelationshipsRefreshAsync();
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Forcing relationships refresh failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Relationships refresh forced.");
        }

        private void GetRelationship()
        {
            Log("Relationships :");
            foreach (Relationship relation in FriendsService.Instance.Relationships)
            {
                Log("    - ID:" + relation.Id + "    Type:" + relation.Type +"    "+ MemberToString(relation.Member));
            }
        }


        private void GetFriends()
        {
            Log("Friends :");
            foreach (Relationship relation in FriendsService.Instance.Friends)
            {
                Log("    - ID:" + relation.Id + "    " + MemberToString(relation.Member));
            }
        }

        private void GetIncomingRequests()
        {
            Log("Incoming Requests :");
            foreach (Relationship relation in FriendsService.Instance.IncomingFriendRequests)
            {
                Log("    - ID:" + relation.Id +"    "+ MemberToString(relation.Member));
            }
        }

        private void GetOutgoingRequests()
        {
            Log("Outgoing Requests :");
            foreach (Relationship relation in FriendsService.Instance.OutgoingFriendRequests)
            {
                Log("    - ID:" + relation.Id +"    "+ MemberToString(relation.Member));
            }
        }

        private void GetBlocks()
        {
            Log("Blocked members :");
            foreach (Relationship relation in FriendsService.Instance.Blocks)
            {
                Log("    - ID:" + relation.Id +"    "+ MemberToString(relation.Member));
            }
        }

        private async void AddFriendByID()
        {
            StartWait();
            Log("Adding friend...");
            try
            {
                await FriendsService.Instance.AddFriendAsync(_addFriendIDInput.text);
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Adding friend failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Friend added.");
        }

        private async void AddFriendByName()
        {
            StartWait();
            Log("Adding friend...");
            try
            {
                await FriendsService.Instance.AddFriendByNameAsync(_addFriendNameInput.text);
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Adding friend failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Friend added.");
        }

        private async void DeleteFriend()
        {
            StartWait();
            Log("Deleting friend...");
            try
            {
                await FriendsService.Instance.DeleteFriendAsync(_deleteFriendIDInput.text);
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Deleting friend failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Friend deleted.");
        }

        private async void DeleteIncomingRequest()
        {
            StartWait();
            Log("Deleting incoming request...");
            try
            {
                await FriendsService.Instance.DeleteIncomingFriendRequestAsync(_deleteIncomingRequestIDInput.text);
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Deleting incoming request failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Incoming request deleted.");
        }

        private async void DeleteOutgoingRequest()
        {
            StartWait();
            Log("Deleting outgoing request...");
            try
            {
                await FriendsService.Instance.DeleteOutgoingFriendRequestAsync(_deleteOutgoingRequestIDInput.text);
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Deleting outgoing request failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Outgoing request deleted.");
        }

        private async void AddBlock()
        {
            StartWait();
            Log("Blocking member...");
            try
            {
                await FriendsService.Instance.AddBlockAsync(_addBlockIDInput.text);
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Blocking member failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Member blocked.");
        }

        private async void DeleteBlock()
        {
            StartWait();
            Log("Unblocking member...");
            try
            {
                await FriendsService.Instance.DeleteBlockAsync(_deleteBlockIDInput.text);
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Unblocking member failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Member unblocked.");
        }

        private async void DeleteRelationship()
        {
            StartWait();
            Log("Deleting relationship...");
            try
            {
                await FriendsService.Instance.DeleteRelationshipAsync(_deleteRelationshipIDInput.text);
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Deleting relationship failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Relationship deleted.");
        }

        private async void SendMsg()
        {
            StartWait();
            Log("Sending message...");
            try
            {
                await FriendsService.Instance.MessageAsync(_sendMessageIDInput.text, new List<char>(_sendMessageContentInput.text));
            }
            catch (Exception e)
            {
                LogException(e);
                Log("Sending message failed.");
                return;
            }
            finally
            {
                StopWait();
            }
            Log("Message sent.");
        }

        private string MemberToString(Member member)
        {
            return "Member:[ID:" + member.Id + " Profile:" + member.Profile.Name +"]";
        }
    }
}
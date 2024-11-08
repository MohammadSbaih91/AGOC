﻿using AGOC.Domain.Interfaces;
using AGOC.Models;
using AGOC.Repository.Interfaces;
using AGOC.Services;
using AGOC.ViewModels;

using AutoMapper;

using Serilog;

namespace AGOC.Domain.Managers
{
    public class MessagesManager : IMessagesManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISmslogManager _smslogManager;
        private readonly IMapper _mapper;

        public MessagesManager(IUnitOfWork unitOfWork, ISmslogManager smslogManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _smslogManager = smslogManager;
            _mapper = mapper;
        }

        // Send an SMS message and log it
        public async Task<OperationResult> SendMessageAsync(Message message, bool? MobileServiceEnable)
        {
            var result = new OperationResult();

            try
            {
                message.CreatedOn = DateTime.Now;

                // Add the message to the database
                await _unitOfWork.MessageRepository.AddAsync(message);
                await _unitOfWork.Save();

                // Send SMS message using the Employee's Mobile number
                foreach (var recipient in message.Recipients)
                {
                    try
                    {
                        // Retrieve the mobile number from Employee
                        var mobile = recipient.Employee?.Mobile;
                        if (!string.IsNullOrEmpty(mobile) && MobileServiceEnable.HasValue && MobileServiceEnable.Value)
                        {
                            await _smslogManager.SendSmsAndLogAsync("System", "api_key", mobile, message.MessageContent, true, message.MessageID, recipient.EmployeeID);
                        }
                        else
                        {
                            Log.Warning("No mobile number found for EmployeeID {EmployeeID}", recipient.EmployeeID);
                            result.Success = false;
                            result.ErrorMessage = $"No mobile number for Employee {recipient.EmployeeID}.";
                        }
                    }
                    catch (Exception smsEx)
                    {
                        Log.Error(smsEx, "Failed to send SMS to EmployeeID {EmployeeID}", recipient.EmployeeID);
                        result.Success = false;
                        result.ErrorMessage = $"Failed to send SMS to Employee {recipient.EmployeeID}.";
                    }
                }

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = "An error occurred while sending the message.";
                Log.Error(ex, result.ErrorMessage);
            }

            return result;
        }



        // Get all messages
        public async Task<IEnumerable<MessageViewModel>> GetAllMessagesAsync()
        {
            var messages = await _unitOfWork.MessageRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<MessageViewModel>>(messages);
        }

        // Get a specific message by ID
        public async Task<MessageViewModel> GetMessageByIdAsync(int id)
        {
            var message = await _unitOfWork.MessageRepository.GetByIdAsync(id);
            return _mapper.Map<MessageViewModel>(message);
        }

        // Update a message
        public async Task<OperationResult> UpdateMessageAsync(MessageViewModel messageViewModel)
        {
            var result = new OperationResult();

            try
            {
                var message = await _unitOfWork.MessageRepository.GetByIdAsync(messageViewModel.MessageID);
                if (message == null)
                {
                    result.Success = false;
                    result.ErrorMessage = "Message not found.";
                    return result;
                }

                // Update message details
                message.MessageContent = messageViewModel.MessageContent;

                _unitOfWork.MessageRepository.Update(message);
                await _unitOfWork.Save();

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = "An error occurred while updating the message.";
                Log.Error(ex, result.ErrorMessage);
            }

            return result;
        }

        // Delete a message
        public async Task<OperationResult> DeleteMessageAsync(int id)
        {
            var result = new OperationResult();

            try
            {
                var message = await _unitOfWork.MessageRepository.GetByIdAsync(id);
                if (message == null)
                {
                    result.Success = false;
                    result.ErrorMessage = "Message not found.";
                    return result;
                }

                // Perform a hard delete as there is no IsDeleted property
                _unitOfWork.MessageRepository.Delete(message);
                await _unitOfWork.Save();

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = "An error occurred while deleting the message.";
                Log.Error(ex, result.ErrorMessage);
            }

            return result;
        }
    }
}
